using AutoMapper;
using game_store_be.Models;
using game_store_be.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using game_store_be.CustomModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public BillController(game_storeContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        private ICollection<Bill> ExistBills(string idUser, string idGame)
        {
            return (_context.Bill.Where(b => b.IdUser == idUser && b.IdGame == idGame).ToList());
        }
        
        /// <summary>
        /// Get all Bill [admin]
        /// </summary>
        /// <returns>an array Bill data</returns>
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAllBill()
        {
            var billss = _context.Bill.ToList();
            return Ok(billss);

            var customMapper = new CustomMapper(_mapper);
            var bills = _context.Bill
                .Include(b => b.IdGameNavigation)
                .Include(b => b.IdUserNavigation)
                .ToList();
            var billsDto = customMapper.CustomMapListBill(bills);
            return Ok(billsDto);
        }

        /// <summary>
        /// Create new bill [admin,user]
        /// </summary>
        /// <param name="billBody"></param>
        /// <returns>an new bill</returns>
        [Authorize(Roles = "admin, user")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateNewBill([FromBody] PostBillBody billBody)
        {
            try
            {
                HttpClient client = new HttpClient();
                var customMapper = new CustomMapper(_mapper);

                billBody.NewBill.IdBill = Guid.NewGuid().ToString();
                var existUser = _context.Users.FirstOrDefault(u => u.IdUser == billBody.NewBill.IdUser);
                var existGame = _context.Game.Include(g => g.IdDiscountNavigation).FirstOrDefault(g => g.IdGame == billBody.NewBill.IdGame);
                double cost = 0;

                if (existGame != null)
                {
                    if (existGame.IdDiscountNavigation != null)
                    {
                        cost = (double)((double)existGame.Cost * (1 - existGame.IdDiscountNavigation.PercentDiscount / 100));
                        var billDiscount = _mapper.Map<Discount, BillDiscount>(existGame.IdDiscountNavigation);
                        billBody.NewBill.Discount = JsonConvert.SerializeObject(billDiscount);
                    }
                    else
                    {
                        cost = (double)existGame.Cost;
                    }
                }

                billBody.NewBill.Cost = Math.Ceiling(cost);
                billBody.NewBill.DatePay = DateTime.UtcNow;
                billBody.NewBill.IdUserNavigation = existUser;

                if (billBody.NewBill.Actions == "refund")
                {
                    var existBill = ExistBills(billBody.NewBill.IdUser, billBody.NewBill.IdGame)
                        .OrderByDescending(b => b.DatePay);
                    if (existBill.Count() == 0)
                    {
                        return NotFound(new { message = "Game is not bought" });
                    }
                    else
                    {
                        var firstBill = existBill.ElementAt(0);
                        if (firstBill.Actions == "pay")
                        {
                            billBody.NewBill.Cost = firstBill.Cost;
                        }
                        else return NotFound(new { message = "Game is not bought" });
                    }
                }

                if (billBody.NewBill.Actions != "refund")
                {
                    billBody.NewBill.Actions = "pay";
                }
                //  Case game free
                if (billBody.NewBill.Cost <= 0)
                {
                    billBody.NewBill.DatePay = DateTime.UtcNow;
                    _context.Bill.Add(billBody.NewBill);
                    _context.SaveChanges();
                    var billDto = customMapper.CustomMapBill(billBody.NewBill);
                    return Ok(billDto);
                }

                //Call api check money
                var card = billBody.Card;
                var isRefund = billBody.NewBill.Actions == "refund";
                var cardDefault = new Card()
                {
                    MasterCardName = "ADMIN",
                    MasterCardNumber = "1040000003",
                    MasterCardCCV = 657,
                    MasterCardExpire = "11/21"
                };

                var trans = new Transaction()
                {
                    MasterCardNumberSend = isRefund ? cardDefault.MasterCardNumber : card.MasterCardNumber,
                    MasterCardNumberReceive = isRefund ? card.MasterCardNumber : cardDefault.MasterCardNumber,
                    TransactionMessage = isRefund ? "Refund " : "Payment for ",
                    AmountOfMoney = billBody.NewBill.Cost,
                };

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Card", isRefund ? cardDefault : card);
                values.Add("Trans", trans);

                var res = await client.PostAsync(_config.GetConnectionString("EndpointPayment"), new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"));
                var contentString = res.Content.ReadAsStringAsync().Result;
                if (contentString == "\"accept\"")
                {
                    billBody.NewBill.DatePay = DateTime.UtcNow;
                    _context.Bill.Add(billBody.NewBill);
                    _context.SaveChanges();
                    var billDto = customMapper.CustomMapBill(billBody.NewBill);
                    return Ok(billDto);

                }
                if (contentString == "\"Information does not match\"")
                {

                    return NotFound(new { message = contentString });
                }

                return Ok(new { message = contentString });
            }
            catch (Exception error)
            {
                var message = error.Message;
                if (message == "An error occurred while updating the entries. See the inner exception for details.") return BadRequest(new { message = "Game already payed" });
                if (message == "Object reference not set to an instance of an object.") return BadRequest(new { message = "Body is incorrect." });
                return BadRequest(new { message = message, clientMessage = "Have some error. Please try again." });
            }

        }
    }
}

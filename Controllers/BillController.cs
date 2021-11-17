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
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public BillController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private ICollection<Bill> ExistBills (string idUser, string idGame)
        {
            return (_context.Bill.Where(b => b.IdUser == idUser && b.IdGame == idGame).ToList());
        }

        [HttpGet]
        public IActionResult GetAllBill()
        {
            var customMapper = new CustomMapper(_mapper);
            var bills = _context.Bill
                .Include(b => b.IdGameNavigation)
                .Include(b => b.IdUserNavigation)
                .ToList();
            var billsDto = customMapper.CustomMapListBill(bills);
            return Ok(billsDto);
        }

        [HttpPost("create")]
        public IActionResult CreateNewBill([FromBody] Bill newBill )
        {
            var customMapper = new CustomMapper(_mapper);
            newBill.IdBill = Guid.NewGuid().ToString();
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == newBill.IdUser);
            var existGame = _context.Game.Include(g => g.IdDiscountNavigation).FirstOrDefault(g => g.IdGame == newBill.IdGame);
            double cost = 0 ;
            if (existGame != null)
            {
                if (existGame.IdDiscountNavigation != null)
                {
                    cost = (double)((double)existGame.Cost * (1 - existGame.IdDiscountNavigation.PercentDiscount / 100));
                    var billDiscount = _mapper.Map<Discount, BillDiscount>(existGame.IdDiscountNavigation);
                    newBill.Discount = JsonConvert.SerializeObject(billDiscount);
                } else
                {
                    cost = (double)existGame.Cost;
                }
            }

            newBill.Cost = Math.Ceiling(cost);
            newBill.DatePay =DateTime.UtcNow;
            newBill.IdUserNavigation = existUser;

            if (newBill.Actions == "refund")
            {
                var existBill = ExistBills(newBill.IdUser, newBill.IdGame)
                    .OrderByDescending(b => b.DatePay);

                if (existBill != null )
                {
                    var firstBill = existBill.ElementAt(0);
                    if (firstBill.Actions == "pay")
                    {
                        newBill.Cost = firstBill.Cost;
                    } else return Ok("Khong co gi de tra");
                }
            }

            if (newBill.Actions == null)
            {
                newBill.Actions = "pay";
            }

            _context.Bill.Add(newBill);
            _context.SaveChanges();
            var billDto = customMapper.CustomMapBill(newBill);
            return Ok(billDto);
        }
    }
}

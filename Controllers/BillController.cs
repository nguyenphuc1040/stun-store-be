using AutoMapper;
using game_store_be.Models;
using game_store_be.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private ICollection<Bill> ExistBills (string idUser, string idGame, string action)
        {
            return (_context.Bill.Where(b => b.IdUser == idUser && b.IdGame == idGame && b.Actions == action).ToList());
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
            newBill.IdBill = Guid.NewGuid().ToString();
            var existGame = _context.Game.Include(g => g.IdDiscountNavigation).FirstOrDefault(g => g.IdGame == newBill.IdGame);
            double cost = 0 ;
            if (existGame != null)
            {
                cost = existGame.IdDiscountNavigation != null 
                    ? (double)((double)existGame.Cost * (1 - existGame.IdDiscountNavigation.PercentDiscount / 100)) 
                    : (double)existGame.Cost;
            }

            newBill.Cost = Math.Ceiling(cost);

            if (newBill.Actions == "refund")
            {
                var existBillPayed = ExistBills(newBill.IdUser, newBill.IdGame, "pay");
                var existBillRefund = ExistBills(newBill.IdUser, newBill.IdGame, "refund");

                if (existBillPayed.Count() > existBillRefund.Count())
                {
                    double? totalCostBillPayed = 0;
                    double? totalCostBillRefund = 0;
                    foreach (var item  in existBillPayed )
                    {
                        totalCostBillPayed += item.Cost;
                    }
                    foreach (var item in existBillRefund)
                    {
                        totalCostBillRefund += item.Cost;
                    }
                    if (totalCostBillPayed - totalCostBillRefund != newBill.Cost)
                    {
                        return Ok("So tien hoan tien khong dung' ");
                    }
                }
                else
                {
                    return Ok("Khong co gi de tra");
                }
            }

            if (newBill.Actions == null)
            {
                newBill.Actions = "pay";
            }

            return Ok(cost);
        }
    }
}

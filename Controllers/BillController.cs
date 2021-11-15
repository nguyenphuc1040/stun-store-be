using AutoMapper;
using game_store_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(_context.Bill.ToList());
        }

        [HttpPost("create")]
        public IActionResult CreateNewBill([FromBody] Bill newBill )
        {
            newBill.IdBill = Guid.NewGuid().ToString();
            if (newBill.Actions == "refund")
            {
                var existBillPayed = ExistBills(newBill.IdUser, newBill.IdGame, "pay");
                var existBillRefund = ExistBills(newBill.IdUser, newBill.IdGame, "refund");

                if (existBillPayed.Count() > existBillRefund.Count())
                {
                    return Ok("Tra tien");
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

            _context.Bill.Add(newBill);
            _context.SaveChanges();

            return Ok(newBill);
        }


    }
}

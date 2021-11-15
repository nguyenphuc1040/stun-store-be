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

        [HttpGet]
        public IActionResult GetAllBill()
        {
            return Ok(_context.Bill.ToList());
        }

        [HttpPost("create")]
        public IActionResult CreateNewBill([FromBody] Bill newBill )
        {
            newBill.IdBill = Guid.NewGuid().ToString();
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

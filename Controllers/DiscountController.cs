using game_store_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly game_storeContext _context;
        public DiscountController(game_storeContext context)
        {
            _context = context;
        }
        // GET: api/<DiscountController>
        private Discount GetDiscountByIdService(string idDiscount)
        {
            return _context.Discount.FirstOrDefault(dc => dc.IdDiscount == idDiscount);
        }
        [HttpGet]
        public IActionResult GetAllDiscount()
        {
           
            return Ok(_context.Discount.ToList());
        }

        // GET api/<DiscountController>/5
        [HttpGet("{idDiscount}")]
        public IActionResult GetDiscountById(string idDiscount)
        {
            var existDiscount = GetDiscountByIdService(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not found" });
            }
            return Ok(existDiscount);
        }

        // POST api/<DiscountController>
        [HttpPost("create")]
        public IActionResult CreateDiscount([FromBody] Discount newDiscount)
        {
            newDiscount.IdDiscount = Guid.NewGuid().ToString();
            _context.Add(newDiscount);
            _context.SaveChanges();
            return Ok(newDiscount);
        }

        // PUT api/<DiscountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DiscountController>/5
        [HttpDelete("delete/{idDiscount}")]
        public IActionResult DeleteDiscountById(string idDiscount)
        {
            var existDiscount = GetDiscountByIdService(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not Found" });

            }
            _context.Remove(existDiscount);
            _context.SaveChanges();
            return Ok(new { message = "Delete Success" });
        }
    }
}

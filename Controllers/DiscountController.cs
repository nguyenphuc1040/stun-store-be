using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
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
    public class DiscountController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public DiscountController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Discount ExistDiscount(string idDiscount)
        {
            return _context.Discount.FirstOrDefault(dc => dc.IdDiscount == idDiscount);
        }

        [HttpGet]
        public IActionResult GetAllDiscount()
        {
            var discount = _context.Discount.ToList();
            var discountDto = _mapper.Map<IEnumerable<DiscountDto>>(discount);
            return Ok(discountDto);
        }

        [HttpGet("{idDiscount}")]
        public IActionResult GetDiscountById(string idDiscount)
        {
            var existDiscount = ExistDiscount(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not found" });
            }
            var discountDto  = _mapper.Map<Discount, DiscountDto>(existDiscount);
            return Ok(discountDto);
        }

        [HttpPost("create")]
        public IActionResult CreateDiscount([FromBody] Discount newDiscount)
        {
            newDiscount.IdDiscount = Guid.NewGuid().ToString();
            _context.Discount.Add(newDiscount);
            _context.SaveChanges();
            var discountDto = _mapper.Map<Discount, DiscountDto>(newDiscount);
            return Ok(discountDto);
        }

        [HttpPut("update/{idDiscount}")]
        public IActionResult UpdateDiscountById(string idDiscount,[FromBody] DiscountDto newDiscount )
        {
            var existDiscount = ExistDiscount(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not found" });
            }
            newDiscount.IdDiscount = existDiscount.IdDiscount;
            _mapper.Map(newDiscount, existDiscount);
            _context.SaveChanges();
            return Ok(newDiscount);
        }
    }
}

using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using game_store_be.CustomModel;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    public class StorePolicyController : Controller
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public StorePolicyController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{name}")]
        public IActionResult GetPolicyByName(string name) {
            var existPolicy = _context.StorePolicy.FirstOrDefault(s => s.NamePolicy == name);
            if (existPolicy == null) return NotFound();
            return Ok(existPolicy);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("update")]
        public IActionResult UpdatePolicyByName([FromBody] StorePolicy body) {
            var existPolicy = _context.StorePolicy.FirstOrDefault(s => s.NamePolicy == body.NamePolicy);
            existPolicy.Content = body.Content;
            existPolicy.DigitValue = body.DigitValue;
            _context.SaveChanges();
            return Ok(existPolicy);
        }
    }
}
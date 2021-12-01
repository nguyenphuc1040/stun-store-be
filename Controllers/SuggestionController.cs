using AutoMapper;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace game_store_be.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        public SuggestionController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllSuggestion()
        {
            return Ok(_context.Suggestion.ToList());
        }

        [HttpPost("create")]
        public IActionResult CreateNewSuggestion([FromBody] Suggestion newSuggestion)
        {
            newSuggestion.IdSuggestion = Guid.NewGuid().ToString();
            _context.Suggestion.Add(newSuggestion);
            _context.SaveChanges();
            return Ok(newSuggestion);
        }

        [HttpPut("update")]
        public IActionResult UpdateSuggestion([FromBody] Suggestion updateSuggestion)
        {
            var existSuggestion = _context.Suggestion
                .FirstOrDefault(sg => sg.IdSuggestion == updateSuggestion.IdSuggestion);
            if (existSuggestion == null)
            {
                return NotFound(new { message = "Suggestion Not Found"});
            }
            existSuggestion.Value = updateSuggestion.Value;
            existSuggestion.Position = updateSuggestion.Position;

            _mapper.Map(updateSuggestion, existSuggestion);
            _context.SaveChanges();
            return Ok(existSuggestion);
        }
    }
}

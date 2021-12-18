using AutoMapper;
using game_store_be.CustomModel;
using game_store_be.Dtos;
using game_store_be.Models;
using game_store_be.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet("get-game/{title}/{count}/{start}")]
        public IActionResult GetGameSuggestion(string title, int count, int start){
            var existSuggestion = _context.Suggestion
                .FirstOrDefault(sg => sg.Title == title);
            string[] listGameStr = existSuggestion.Value.Split(",");

            List<GameDto> listGame = new List<GameDto>();
            for (int i=start; i<start+count; i++) {
                if (i>listGameStr.Length -1) break;
                var game = GetGameById(listGameStr[i]);
                if (game!=null) listGame.Add(game);
            }
            return Ok(listGame.Skip(start).Take(count));
        }
        public GameDto GetGameById(string idGame)
        {
            var existGame = _context.Game.Where(u => u.IdGame == idGame)
                    .Include(u => u.IdDiscountNavigation)
                    .Include(x => x.DetailGenre)
                        .ThenInclude(x => x.IdGenreNavigation)
                    .Include(x => x.ImageGameDetail);
            if (existGame == null) return null;
            var existGameDto = _mapper.Map<Game,GameDto>(existGame.First());
            existGameDto.Discount = _mapper.Map<Discount, DiscountDto>(existGame.First().IdDiscountNavigation);
            existGameDto.Genres = _mapper.Map<ICollection<DetailGenreDto>>(existGame.First().DetailGenre);
            existGameDto.ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(existGame.First().ImageGameDetail.OrderBy(i=>i.Url));
            
            return existGameDto;
        }
    }
}

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
        private DiscoverGames gamesDiscover = new DiscoverGames();
        public SuggestionController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            if (existSuggestion.Value == null) return NotFound();
            string[] listGameStr = existSuggestion.Value.Split(",");

            List<GameDto> listGame = new List<GameDto>();
            for (int i=start; i<start+count; i++) {
                if (i>listGameStr.Length -1) break;
                var game = GetGameById(listGameStr[i]);
                if (game!=null) listGame.Add(game);
            }
            return Ok(listGame.Skip(start).Take(count));
        }
        private GameDto GetGameById(string idGame)
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
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllSuggestion(){
           
           string[] title = {
               "Carousel","Top games week","Free now","Top sellers","Free games","Game on sales","New release","Most favorite","Most popular",
               "Top games month"
           };
           foreach(string tit in title){
               switch (tit) {
                    case "Carousel":
                        gamesDiscover.carousel = GetGamesSuggestion(tit, 5);
                        break;
                    case "Top sellers":
                        gamesDiscover.topsellers = GetGamesSuggestion(tit, 5);
                        break;
                    case "New release":
                        gamesDiscover.newReleases = GetGamesSuggestion(tit, 5);
                        break;
                    case "Most favorite":
                        gamesDiscover.mostFavorite = GetGamesSuggestion(tit, 5);
                        break;
                    case "Free games":
                        gamesDiscover.mostFavorite = GetGamesSuggestion(tit, 5);
                        break;
                    case "Most popular":
                        gamesDiscover.mostPopular = GetGamesSuggestion(tit, 5);
                        break;
                    case "Top games week":
                        gamesDiscover.topGamesWeek = GetGamesSuggestion(tit, 12);
                        break;  
                    case "Top games month":
                        gamesDiscover.topGamesMonth = GetGamesSuggestion(tit, 12);
                        break;
                    case "Game on sales":
                        gamesDiscover.gameOnSales = GetGamesSuggestion(tit, 5);
                        break;
                    case "Free now":
                        gamesDiscover.freeNow = GetGamesSuggestion(tit, 5);
                        break;
               } 
           }
           return Ok(gamesDiscover);
        }
        private List<GameDto> GetGamesSuggestion(string title, int count){
            var existSuggestion = _context.Suggestion
                .FirstOrDefault(sg => sg.Title == title);
            if (existSuggestion.Value == null) return null;
            string[] listGameStr = existSuggestion.Value.Split(",");

            List<GameDto> listGame = new List<GameDto>();
            for (int i=0; i<count; i++) {
                if (i>listGameStr.Length -1) break;
                var game = GetGameById(listGameStr[i]);
                if (game!=null) listGame.Add(game);
            }
            return listGame;
        }
        [AllowAnonymous]
        [HttpGet("get-game-suggestion-now")]
        public IActionResult GetGameSuggestionNotification(){
            var existGame = _context.Game.Where(g => g.IdDiscount != null).ToList();
            var length = existGame.Count();
            if (length == 0) return NotFound();
            Random r = new Random();
            var game = existGame[r.Next(0,length)];
            var gameDto = GetGameById(game.IdGame);
            if (gameDto == null) return NotFound();
            return Ok(gameDto);
        }
    }
}

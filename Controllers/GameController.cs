using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public GameController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        private Game GetGameByIdService(string idGame)
        {
            return _context.Game.FirstOrDefault(g => g.IdGame == idGame);
        }
        [HttpGet]
        public IActionResult GetAllGame()
        {
            var games = _context.Game.Include(x => x.IdDiscountNavigation);
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        // GET: GameController/Details/5
        [HttpGet("{idGame}")]
        public IActionResult GetGameById(string idGame)
        {
            var existGame = _context.Game.Where(u => u.IdGame == idGame);
            if (existGame == null)
            {
                return NotFound(new { message = "Not found" });
            }

            return Ok(existGame);
        }
        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] Game newGame)
        {
            newGame.IdGame = Guid.NewGuid().ToString();
            _context.Game.Add(newGame);
            _context.SaveChanges();

            return Ok(newGame);
        }

        [HttpDelete("delete/{idGame}")]
        public IActionResult DeleteGameById(string idGame)
        {
            var existGame = GetGameByIdService(idGame);
            if (existGame == null)
            {
                return NotFound(new { message = "Not found" });
            }

            _context.Remove(existGame);
            _context.SaveChanges();

            return Ok(new { message = "Delete Success" });
        }
    }
}

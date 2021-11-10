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
    public class GameController : Controller
    {
        // GET: GameController
        private readonly AppDbContext _context;
        public GameController(AppDbContext context)
        {
            _context = context;
        }
        private Games GetGameByIdService(uint idGame)
        {
            return _context.Games.FirstOrDefault(g => g.Id == idGame);
        }
        [HttpGet]
        public IActionResult GetAllGame()
        {
            return Ok( _context.Games.ToList());
        }

        // GET: GameController/Details/5
        [HttpGet("{idGame}")]
        public IActionResult GetGameById(uint idGame)
        {
            var existGame = GetGameByIdService(idGame);
            if (existGame == null)
            {
                return NotFound(new { message = "Not found" });
            }

            return Ok(existGame);
        }
        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] Games newGame)
        {
            _context.Games.Add(newGame);
            _context.SaveChanges();

            return Ok(newGame); 
        }

        [HttpDelete("delete/{idGame}")]
        public IActionResult DeleteGameById(uint idGame)
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

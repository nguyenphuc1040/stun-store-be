using game_store_be.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        private Genres GetGenreByIdService( uint idGenre)
        {
            return _context.Genres.FirstOrDefault(u => u.Id == idGenre);

        }

        // GET: api/<GenreController>
        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(_context.Genres.ToList());
        }

        // GET api/<GenreController>/5
        [HttpGet("{idGenre}")]
        public IActionResult GetGenreById(uint idGenre)
        {
            var existGenre = GetGenreByIdService( idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            return Ok(existGenre);
        }

        // POST api/<GenreController>
        [HttpPost("create")]
        public IActionResult CreateNewGenre([FromBody] Genres newGenre)
        {
            _context.Genres.Add(newGenre);
            _context.SaveChanges();
            return Ok(newGenre);
        }

        // PUT api/<GenreController>/5
        [HttpPatch("update/{idGenre}")]
        public IActionResult UpdateGenreById(uint idGenre, [FromBody] Genres newGenre)
        {
            Genres existGenre = GetGenreByIdService( idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            //newGenre.Id = existGenre.Id;

            existGenre.Name = newGenre.Name;
            _context.SaveChanges();
            return Ok( new {a = newGenre.GetType() == existGenre.GetType()});
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("delete/{idGenre}")]
        public IActionResult DeleteGenreById(uint idGenre)
        {
            var existGenre = GetGenreByIdService( idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            _context.Remove(existGenre);
            _context.SaveChanges();

            return Ok(new { message = "Delete Success" });
        }
    }
}

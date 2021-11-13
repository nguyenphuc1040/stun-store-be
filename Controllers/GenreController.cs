using AutoMapper;
using game_store_be.Dtos;
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
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        public GenreController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Genre GetGenreByIdService(string idGenre)
        {
            return _context.Genre.FirstOrDefault(u => u.IdGenre == idGenre);
        }

        // GET: api/<GenreController>
        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _context.Genre.ToList();
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(genresDto);
        }

        // GET api/<GenreController>/5
        [HttpGet("{idGenre}")]
        public IActionResult GetGenreById(string idGenre)
        {
            var existGenre = GetGenreByIdService(idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            return Ok(existGenre);
        }

        // POST api/<GenreController>
        [HttpPost("create")]
        public IActionResult CreateNewGenre([FromBody] Genre newGenre)
        {
            newGenre.IdGenre = Guid.NewGuid().ToString();
            _context.Genre.Add(newGenre);
            _context.SaveChanges();
            return Ok(newGenre);
        }

        // PUT api/<GenreController>/5
        [HttpPatch("update/{idGenre}")]
        public IActionResult UpdateGenreById(string idGenre, [FromBody] Genre newGenre)
        {
            Genre existGenre = GetGenreByIdService(idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            //newGenre.Id = existGenre.Id;

            existGenre.NameGenre = newGenre.NameGenre;
            _context.SaveChanges();
            return Ok(new { a = newGenre.GetType() == existGenre.GetType() });
        }
        [HttpDelete("delete/{idGenre}")]
        public IActionResult DeleteGenreById(string idGenre)
        {
            Genre existGenre = GetGenreByIdService(idGenre);
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

using AutoMapper;
using game_store_be.Dtos;
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

        /// <summary>
        /// Get all Genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetGenres()
        {
            var genres = _context.Genre.ToList();
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);
            return Ok(genresDto);
        }

        /// <summary>
        /// Get Genre by id
        /// </summary>
        /// <param name="idGenre"></param>
        /// <returns></returns>
        [HttpGet("{idGenre}")]
        public IActionResult GetGenreById(string idGenre)
        {
            var existGenre = GetGenreByIdService(idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            var genreDto = _mapper.Map<Genre, GenreDto>(existGenre);
            return Ok(genreDto);
        }

        /// <summary>
        /// Create new Genre [admin]
        /// </summary>
        /// <param name="newGenre"></param>
        /// <returns></returns>
        [Authorize(Roles ="admin")]
        [HttpPost("create")]
        public IActionResult CreateNewGenre([FromBody] Genre newGenre)
        {
            newGenre.IdGenre = Guid.NewGuid().ToString();
            _context.Genre.Add(newGenre);
            _context.SaveChanges();
            var genreDto = _mapper.Map<Genre, GenreDto>(newGenre);
            return Ok(genreDto);
        }

        /// <summary>
        /// Update Genre by Id
        /// </summary>
        /// <param name="idGenre"></param>
        /// <param name="newGenre"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpPut("update/{idGenre}")]
        public IActionResult UpdateGenreById(string idGenre, [FromBody] GenreDto newGenre)
        {
            Genre existGenre = GetGenreByIdService(idGenre);
            if (existGenre == null)
            {
                return NotFound(new { message = "Not found" });
            }
            newGenre.IdGenre = existGenre.IdGenre;
            _mapper.Map(newGenre ,existGenre);
            _context.SaveChanges();
            return Ok(existGenre);
        }
    }
}

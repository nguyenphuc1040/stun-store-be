using game_store_be.Interfaces;
using game_store_be.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace game_store_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private IGenre genreService;
        public GenreController(IGenre _genre)
        {
            genreService = _genre;
        }


        [HttpGet]
        public IActionResult GetGenres()
        {
            return Ok(genreService.GetGenres());
        } 

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetGenre(Guid id)
        {
            GenreModel genre = genreService.GetGenre(id);
            if (genre != null)
            {
                return Ok(genre);
            }
            return NotFound($"Genre not found");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult AddGenre(GenreModel newGenre)
        {
            genreService.AddGenre(newGenre);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + newGenre.Id, newGenre);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult DeleteGenre(Guid id)
        {
            GenreModel genre = genreService.GetGenre(id);
            if (genre != null)
            {
                genreService.DeleteGenre(genre);
                return Ok(new { message = "Delete success" });

            }
            return NotFound($"Genre not found");

        }

        [HttpPatch]
        [Route("update/{id}")]
        public IActionResult EditGenre(Guid id, GenreModel newGenre)
        {
            newGenre.Id = id;
            GenreModel result = genreService.EditGenre(newGenre);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound($"Genre not found");
        }
    }
}

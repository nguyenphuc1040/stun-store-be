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
            return _context.Game.Include(g => g.IdDiscountNavigation).FirstOrDefault(g => g.IdGame == idGame);
        }
        [HttpGet]
        public IActionResult GetAllGame()
        {
            var games = _context.Game
                .Include(x => x.IdDiscountNavigation)
                .Include(x => x.DetailGenre)
                    .ThenInclude(x => x.IdGenreNavigation)
                .Include(x => x.ImageGameDetail);
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            for (var i = 0; i < games.Count(); i++)
            {
                gamesDto.ToList().ElementAt(i).Discount = _mapper.Map<Discount, DiscountDto>(games.ToList().ElementAt(i).IdDiscountNavigation);
                gamesDto.ToList().ElementAt(i).Genres = _mapper.Map<ICollection<DetailGenreDto>>(games.ToList().ElementAt(i).DetailGenre);
                gamesDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(games.ToList().ElementAt(i).ImageGameDetail);
            }
            return Ok(gamesDto);
        }

        [HttpGet("{idGame}")]
        public IActionResult GetGameById(string idGame)
        {
            var existGame = _context.Game.Where(u => u.IdGame == idGame).Include(u => u.IdDiscountNavigation).Include(x => x.DetailGenre).ThenInclude(x => x.IdGenreNavigation);
            if (existGame == null)
            {
                return NotFound(new { message = "Not found" });
            }

            var existGameDto = _mapper.Map<IEnumerable<GameDto>>(existGame);
            existGameDto.First().Discount = _mapper.Map<Discount, DiscountDto>(existGame.First().IdDiscountNavigation);
            existGameDto.First().Genres = _mapper.Map<ICollection<DetailGenreDto>>(existGame.First().DetailGenre);

            return Ok(existGameDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] PostGameBody newGameBody)
        {
            var mapper = new CustomMapper(_mapper);
            var newGame = newGameBody.Game;
            var newGameVersion = newGameBody.GameVersion;
            var listImageDetail = newGameBody.ListImageDetail;
            var listGenreDetail = newGameBody.ListGenreDetail;

            var id = Guid.NewGuid().ToString();
            newGame.IdGame = id;
            newGameVersion.IdGame = id;
            newGameVersion.IdGameVersion = Guid.NewGuid().ToString();
            newGameVersion.DateUpdate = DateTime.UtcNow;
            newGame.ReleaseDate = DateTime.UtcNow;

            var listImageGameDetail = new List<ImageGameDetail>();
            if (listImageDetail != null)
            {
                foreach (var image in listImageDetail)
                {
                    var imageDetail = new ImageGameDetail() { IdGame = id, Url = image, IdImage = Guid.NewGuid().ToString() };
                    listImageGameDetail.Add(imageDetail);
                }
            }

            var listGenreDetailFoundDto = new List<DetailGenreDto>();
            if (listGenreDetail != null)
            {
                var listGenreFound = new List<Genre>();

                foreach (var detailGenre in listGenreDetail)
                {
                    var existGenre = _context.Genre.FirstOrDefault(g => g.IdGenre == detailGenre);
                    if (existGenre == null)
                    {
                        return NotFound(new { message = listGenreDetail + "not found" });
                    }
                    listGenreFound.Add(existGenre);
                }

                foreach (var genreFound in listGenreFound)
                {
                    var genreDeto = _mapper.Map<Genre, GenreDto>(genreFound);
                    var detailGenre = new DetailGenre() { IdGame = id, IdGenre = genreFound.IdGenre };
                    var detailGenreDto = new DetailGenreDto() { IdGenreNavigation = genreDeto };
                    _context.DetailGenre.Add(detailGenre);
                    listGenreDetailFoundDto.Add(detailGenreDto);
                }
            }
            newGame.NumberOfBuyer = 0;
            newGame.NumberOfDownloaders = 0;
            newGame.NumOfRate = 0;

            _context.Game.Add(newGame);
            _context.GameVersion.Add(newGameVersion);
            _context.ImageGameDetail.AddRange(listImageGameDetail);

            _context.SaveChanges();

            var listImageGameDto = mapper.CustomMapListImageGameDetail(listImageGameDetail);
            var newGameDto = _mapper.Map<Game, GameDto>(newGame);
            newGameDto.ImageGameDetail = listImageGameDto;
            newGameDto.Genres = listGenreDetailFoundDto;
            var newGameVersionDto = _mapper.Map<GameVersion, GameVersionDto>(newGameVersion);

            return Ok(new { newGameDto, newGameVersionDto });
        }

        [HttpGet("more-like-this/{idGame}/{amount}")]
        public IActionResult GetGameMoreLikeThis(string idGame, int amount)
        {

            var gameMoreLikeThis = _context.Game
                .Join(
                    _context.DetailGenre,
                    game => game.IdGame,
                    detailGenre => detailGenre.IdGame,
                    (game, detailGenre) => new { detailGenre }
                )
                .Where(e => e.detailGenre.IdGame == idGame)
                .Join(
                    _context.DetailGenre,
                    genre => genre.detailGenre.IdGenre,
                    genreFound => genreFound.IdGenre,
                    (genre, genreFound) => new { genreFound.IdGame }
                )
                .Where(e => e.IdGame != idGame)
                .Take(amount);

            return Ok(gameMoreLikeThis);
        }
    }
}

using System.Collections;
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
                gamesDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(games.ToList().ElementAt(i).ImageGameDetail.OrderBy(i => i.Url));
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
            newGame.NumOfRate = 0;
            newGame.AverageRate = 0;

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

        [Authorize(Roles = "admin")]
        [HttpPut("create-update/{idGame}")]
        public IActionResult CreateNewUpdate(string idGame, [FromBody] PostGameBody newGameBody)
        {
            var customMapper = new CustomMapper(_mapper);
            var newGame = newGameBody.Game;
            var newVersionGame = newGameBody.GameVersion;
            var listImageDetail = newGameBody.ListImageDetail;
            var listGenreDetail = newGameBody.ListGenreDetail;

            var id = Guid.NewGuid().ToString();
            newGame.IdGame = idGame;
            newVersionGame.IdGameVersion = id;
            newVersionGame.IdGame = idGame;
            newVersionGame.DateUpdate = DateTime.UtcNow;

            var existGame = GetGameByIdService(idGame);

            if (existGame == null) return NotFound(new { message = "Game not found" });
            newGame.Plaform = existGame.Plaform;
            newGame.Cost = existGame.Cost;
            newGame.ReleaseDate = existGame.ReleaseDate;
            newGame.NumberOfBuyer = existGame.NumberOfBuyer;
            newGame.NumberOfDownloaders = existGame.NumberOfDownloaders;
            newGame.IdDiscount = existGame.IdDiscount;
            newGame.NumOfRate = existGame.NumOfRate;
            newGame.AverageRate = existGame.AverageRate;
            _mapper.Map(newGame, existGame);

            // remove old images
            var existImageDetails = _context.ImageGameDetail.Where(imgD => imgD.IdGame == idGame);
            _context.ImageGameDetail.RemoveRange(existImageDetails);

            // add new images
            var listImageDetailResult = new List<ImageGameDetail>();
            if (listImageDetail.Count > 0)
            {
                foreach (var imageDetail in listImageDetail)
                {
                    var newImageDetail = new ImageGameDetail() { IdImage = Guid.NewGuid().ToString(), IdGame = idGame, Url = imageDetail };
                    listImageDetailResult.Add(newImageDetail);
                }
                _context.ImageGameDetail.AddRange(listImageDetailResult);
            }

            // remove old genreDetail
            var existGenreDetails = _context.DetailGenre.Where(dg => dg.IdGame == idGame);
            _context.DetailGenre.RemoveRange(existGenreDetails);

            // add new genre detail
            var listGenreDetailResult = new List<DetailGenre>();
            if (listGenreDetail.Count > 0)
            {
                foreach (var genreDetail in listGenreDetail)
                {
                    var existGenre = _context.Genre.FirstOrDefault(g => g.IdGenre == genreDetail);
                    if (existGenre == null) return NotFound(new { message = "Not found genre " + genreDetail });
                    var newGenreDetail = new DetailGenre() { IdGame = idGame, IdGenre = genreDetail };
                    listGenreDetailResult.Add(newGenreDetail);
                }
                _context.DetailGenre.AddRange(listGenreDetailResult);
            }

            _context.GameVersion.Add(newVersionGame);
            _context.SaveChanges();

            var listImageGameDto = customMapper.CustomMapListImageGameDetail(listImageDetailResult);
            var newGameDto = _mapper.Map<Game, GameDto>(newGame);
            newGameDto.ImageGameDetail = listImageGameDto;
            newGameDto.Genres = customMapper.CustomMappDetailGenre(listGenreDetailResult);
            var newGameVersionDto = _mapper.Map<GameVersion, GameVersionDto>(newVersionGame);

            return Ok(new { newGameDto, newGameVersionDto });
        }
        

        [HttpPut("update/{idGame}")]
        public IActionResult Update(string idGame, [FromBody] PostGameBody newGameBody)
        {
            var customMapper = new CustomMapper(_mapper);
            var newGame = newGameBody.Game;
            var listImageDetail = newGameBody.ListImageDetail;
            var listGenreDetail = newGameBody.ListGenreDetail;

            newGame.IdGame = idGame;

            var existGame = GetGameByIdService(idGame);

            if (existGame == null) return NotFound(new { message = "Game not found" });
            existGame.NameGame = newGame.NameGame;
            existGame.Developer = newGame.Developer;
            existGame.Publisher = newGame.Publisher;
            existGame.UrlVideo = newGame.UrlVideo;

            // remove old images
            var existImageDetails = _context.ImageGameDetail.Where(imgD => imgD.IdGame == idGame);
            _context.ImageGameDetail.RemoveRange(existImageDetails);

            // add new images
            var listImageDetailResult = new List<ImageGameDetail>();
            if (listImageDetail.Count > 0)
            {
                foreach (var imageDetail in listImageDetail)
                {
                    var newImageDetail = new ImageGameDetail() { IdImage = Guid.NewGuid().ToString(), IdGame = idGame, Url = imageDetail };
                    listImageDetailResult.Add(newImageDetail);
                }
                _context.ImageGameDetail.AddRange(listImageDetailResult);
            }

            // remove old genreDetail
            var existGenreDetails = _context.DetailGenre.Where(dg => dg.IdGame == idGame);
            _context.DetailGenre.RemoveRange(existGenreDetails);

            // add new genre detail
            var listGenreDetailResult = new List<DetailGenre>();
            if (listGenreDetail.Count > 0)
            {
                foreach (var genreDetail in listGenreDetail)
                {
                    var existGenre = _context.Genre.FirstOrDefault(g => g.IdGenre == genreDetail);
                    if (existGenre == null) return NotFound(new { message = "Not found genre " + genreDetail });
                    var newGenreDetail = new DetailGenre() { IdGame = idGame, IdGenre = genreDetail };
                    listGenreDetailResult.Add(newGenreDetail);
                }
                _context.DetailGenre.AddRange(listGenreDetailResult);
            }

            _context.SaveChanges();

            var listImageGameDto = customMapper.CustomMapListImageGameDetail(listImageDetailResult);
            var newGameDto = _mapper.Map<Game, GameDto>(newGame);
            newGameDto.ImageGameDetail = listImageGameDto;
            newGameDto.Genres = customMapper.CustomMappDetailGenre(listGenreDetailResult);

            return Ok(new { newGameDto });
        }
        [Authorize(Roles = "admin")]
        [HttpGet("check-version-exist/{idGame}/{version}")]
        public IActionResult CheckVersionExist(string idGame, string version){
            var existGame = _context.GameVersion.FirstOrDefault(g => g.IdGame == idGame && g.VersionGame == version);
            if (existGame == null) return Ok(new {message = "no"});
                else return Ok(new {message = "yes"});
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
            List<GameDto> gameMoreLikeThisDto = new List<GameDto>();
            List<string> games = new List<string>();
            foreach (var item in gameMoreLikeThis) {
                games.Add(item.IdGame);
            }
            foreach(var item in games) {
                var gamex = GetGameByIdGame(item);
                gameMoreLikeThisDto.Add(gamex);
            }
            return Ok(gameMoreLikeThisDto);
        }
        private GameDto GetGameByIdGame(string idGame)
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
        [Authorize]
        [HttpGet("installed")]
        public IActionResult UpdateDownloadedOfGame(){

            string gameId = HttpContext.Request.Headers["idGame"];

            var gameExist = _context.Game.FirstOrDefault(g => g.IdGame == gameId);
            if (gameExist != null)
            {
                gameExist.NumberOfDownloaders += 1;
                _context.SaveChanges();
                return Ok(gameExist.NumberOfDownloaders);
            }

            return NotFound();
        }
        [HttpGet("get-game-for-discount")]
        public IActionResult GetGameForDiscount(){
            var listGame = _context.Game
                            .Where(g => g.IdDiscount == null && g.Cost != 0)
                            .ToList();
            return Ok(listGame);
        }
        [HttpPost("lazy-load/browse")]
        public IActionResult GetGameBrowse([FromBody] LazyLoadBrowseBody param){
            List<GameDto> gameBrowse = new List<GameDto>();
            if (param.ListGenreDetail == null || param.ListGenreDetail.Count() == 0) {
                var games = GetGameBrowseAll(param);
                var listGameDto = _mapper.Map<IEnumerable<GameDto>>(games);
                gameBrowse.AddRange(listGameDto);
            } else {
                Hashtable checkSame = new Hashtable();
                foreach(var genreItem in param.ListGenreDetail){
                    var games = _context.Game
                                .Include(x => x.IdDiscountNavigation)
                                .Include(x => x.DetailGenre)
                                    .ThenInclude(x => x.IdGenreNavigation)
                                .Include(x => x.ImageGameDetail)
                                .Join(
                                    _context.DetailGenre,
                                    game => game.IdGame,
                                    detailGenre => detailGenre.IdGame,
                                    (game, detailGenre) => new { detailGenre,game }
                                )
                                .AsNoTracking()
                                .Where(g => g.detailGenre.IdGenre == genreItem)
                                .Skip(param.start).Take(param.count);

                    List<Game> listGame = new List<Game>();
                    foreach (var gamesItem in games) listGame.Add(gamesItem.game);
                    var listGameDto = _mapper.Map<IEnumerable<GameDto>>(listGame);
                    for (var i = 0; i < listGameDto.Count(); i++)
                    {
                        listGameDto.ToList().ElementAt(i).Discount = _mapper.Map<Discount, DiscountDto>(listGame.ToList().ElementAt(i).IdDiscountNavigation);
                        listGameDto.ToList().ElementAt(i).Genres = _mapper.Map<ICollection<DetailGenreDto>>(listGame.ToList().ElementAt(i).DetailGenre);
                        listGameDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(listGame.ToList().ElementAt(i).ImageGameDetail.OrderBy(i => i.Url));
                    }
                    foreach(var item in listGameDto) 
                        if (checkSame[item.IdGame] == null) {
                            checkSame.Add(item.IdGame,true);
                            gameBrowse.Add(item);
                        }
                }
            }
            switch (param.sortBy){
                case "abc":
                    gameBrowse = gameBrowse.OrderBy(e => e.NameGame).ToList();
                    break;
                case "new-release":
                    gameBrowse = gameBrowse.OrderByDescending(e => e.ReleaseDate).ToList();
                    break;
                case "price-to-high":
                    gameBrowse = gameBrowse.OrderBy(e => e.Cost).ToList();
                    break;
                case "price-to-low":
                    gameBrowse = gameBrowse.OrderByDescending(e => e.Cost).ToList();
                    break;
            }
            if (gameBrowse != null) return Ok(gameBrowse);
            return NotFound("Out of data");
        }
        private IEnumerable<GameDto> GetGameBrowseAll(LazyLoadBrowseBody param)
        {
            var games = _context.Game
                .Include(x => x.IdDiscountNavigation)
                .Include(x => x.DetailGenre)
                    .ThenInclude(x => x.IdGenreNavigation)
                .Include(x => x.ImageGameDetail)
                .Skip(param.start).Take(param.count).ToList();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            for (var i = 0; i < games.Count(); i++)
            {
                gamesDto.ToList().ElementAt(i).Discount = _mapper.Map<Discount, DiscountDto>(games.ToList().ElementAt(i).IdDiscountNavigation);
                gamesDto.ToList().ElementAt(i).Genres = _mapper.Map<ICollection<DetailGenreDto>>(games.ToList().ElementAt(i).DetailGenre);
                gamesDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(games.ToList().ElementAt(i).ImageGameDetail.OrderBy(i => i.Url));
            }
            return gamesDto;
        }

        [HttpGet("search-game-by-name/{nameGame}")]
        public IActionResult SearchGameByName(string nameGame){
            var games = _context.Game
                    .Include(x => x.IdDiscountNavigation)
                    .Include(x => x.ImageGameDetail)
                    .Where(g => g.NameGame.ToLower().IndexOf(nameGame.ToLower())!=-1).ToList();
            if (games == null) return NotFound();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);
            for (var i = 0; i < games.Count(); i++)
            {
                gamesDto.ToList().ElementAt(i).Discount = _mapper.Map<Discount, DiscountDto>(games.ToList().ElementAt(i).IdDiscountNavigation);
                gamesDto.ToList().ElementAt(i).ImageGameDetail = _mapper.Map<ICollection<ImageGameDetailDto>>(games.ToList().ElementAt(i).ImageGameDetail.OrderBy(i => i.Url));
            }
            return Ok(gamesDto);
        }
    }
}

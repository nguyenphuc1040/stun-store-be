using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using game_store_be.Utils;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameVersionController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public GameVersionController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllGameVersion()
        {
            var gamesVersions = _context.GameVersion.ToList();
            return Ok(gamesVersions);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public IActionResult CreateGameVersion([FromBody] GameVersion newGameVersion)
        {
            newGameVersion.IdGameVersion = Guid.NewGuid().ToString();
            _context.GameVersion.Add(newGameVersion);
            _context.SaveChanges();
            return Ok(newGameVersion);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("by-game/{idGame}")]
        public IActionResult GetVersionByIdGame(string idGame)
        {
            var existGameVersions = _context.GameVersion.Where(g => g.IdGameNavigation.IdGame == idGame).OrderBy(gv => gv.VersionGame).ToList();
            var existGameDto = _mapper.Map<ICollection<GameVersionDto>>(existGameVersions);
            return Ok(existGameDto);
        }

        [HttpGet("by-game/new-version/{idGame}")]
        public IActionResult GetNewVersionByIdGame(string idGame)
        {
            var existGameVersion = _context.GameVersion
                .Where(g => g.IdGameNavigation.IdGame == idGame)
                .Include(g => g.IdGameNavigation)
                .OrderByDescending(gv => gv.VersionGame)
                .ToList().ElementAt(0);
            var existGame = _context.Game.First(g => g.IdGame == idGame);
            var imageDetail = _context.ImageGameDetail.Where(i => i.IdGame == idGame);

            var existGameversionDto = _mapper.Map<GameVersion, GameVersionDto>(existGameVersion);
            var existGameDto = _mapper.Map<Game, GameDto>(existGame);
            var imageDto = _mapper.Map<ICollection<ImageGameDetailDto>>(imageDetail);
            existGameDto.ImageGameDetail = imageDto;
            existGameversionDto.UrlDowload = null;
            existGameDto.NewVersion = existGameversionDto;

            return Ok(existGameDto);
        }
        [HttpGet("by-game/lastest-version/{idGame}")]
        public IActionResult GetNewVersionByIdGameAndLastestVersion(string idGame)
        {
            var customMapper = new CustomMapper(_mapper);
            var lastestVersion = _context.Game
                                    .FirstOrDefault(g => g.IdGame == idGame).LastestVersion;
            var existGame = _context.Game
                            .Include(g => g.IdDiscountNavigation)
                            .Include(g => g.DetailGenre)
                                .ThenInclude(g => g.IdGenreNavigation)
                            .FirstOrDefault(g => g.IdGame == idGame);
            if (existGame == null) return NotFound(new { message = "Game not found" });

            var existGameVersion = _context.GameVersion
                .FirstOrDefault(gv => gv.IdGame == idGame && gv.VersionGame == lastestVersion);
            if (existGameVersion == null) return NotFound(new { message = "Version not found" });


            var imageDetail = _context.ImageGameDetail
                                .Where(i => i.IdGame == idGame)
                                .OrderBy(i => i.Url);

            var existGameDto = customMapper.CustomMapGame(existGame);
            var existGameversionDto = _mapper.Map<GameVersion, GameVersionDto>(existGameVersion);
            var imageDto = _mapper.Map<ICollection<ImageGameDetailDto>>(imageDetail);
            existGameDto.ImageGameDetail = imageDto;
            existGameversionDto.UrlDowload = null;
            existGameDto.NewVersion = existGameversionDto;

            return Ok(existGameDto);
        }
        [Authorize]
        [HttpGet("by-game/url-download")]
        public IActionResult GetUrlDownloadByIdGameVersion()
        {
            string gameVer = HttpContext.Request.Headers["idGameVersion"];
            var gameVersionExist = _context.GameVersion
                .FirstOrDefault(gv => gv.IdGameVersion == gameVer);
            if (gameVersionExist != null)
            {
                return Ok(gameVersionExist.UrlDowload);
            }

            return NotFound(new { message = "Not found game version" });
        }

    }
}

using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult GetAllGameVersion ()
        {
            var gamesVersions = _context.GameVersion.ToList();
            return Ok(gamesVersions);
        }
        [HttpPost("create")]
        public IActionResult CreateGameVersion([FromBody] GameVersion newGameVersion )
        {
            newGameVersion.IdGameVersion = Guid.NewGuid().ToString();
            _context.GameVersion.Add(newGameVersion);
            _context.SaveChanges();
            return Ok(newGameVersion);
        }

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
                .OrderByDescending(gv => gv.VersionGame)
                .ToList().ElementAt(0);
            var existGame = _context.Game.First(g => g.IdGame == idGame);
            var imageDetail = _context.ImageGameDetail.Where(i => i.IdGame == i.IdGame);

            var existGameversionDto = _mapper.Map<GameVersion, GameVersionDto>(existGameVersion);
            var existGameDto = _mapper.Map<Game, GameDto>(existGame);
            var imageDto = _mapper.Map<ICollection < ImageGameDetailDto>>(imageDetail);
            existGameDto.ImageGameDetail = imageDto;
            existGameDto.NewVersion = existGameversionDto;

            return Ok( existGameDto ); 
        }


    }
}

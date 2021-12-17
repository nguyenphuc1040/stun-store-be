using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using game_store_be.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public CollectionController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{idUser}")]
        public IActionResult GetAllCollection(string idUser)
        {
            var customMapper = new CustomMapper(_mapper);
            var user = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            var collection = _context.Collection
                .Where(c => c.IdUser == idUser)
                .Include(c => c.IdGameNavigation)
                    .ThenInclude(g => g.DetailGenre).ThenInclude(g => g.IdGenreNavigation)
                .Include(c => c.IdGameNavigation).ThenInclude(g => g.ImageGameDetail);
            if (collection == null )
            {
                return NotFound(new { message = "Not found" });
            }
            var collectionsDto = customMapper.CustomMapListCollection(collection.ToList());
            var userDto = _mapper.Map<Users, UserDto>(user);
            return Ok(new { user = userDto, listGame = collectionsDto });
        }
        [AllowAnonymous]
        [HttpGet("is-own-by-user/{idUser}/{idGame}")]
        public IActionResult GetIsOwnByUser(string idUser, string idGame){
            var existGame = _context.Collection
                            .FirstOrDefault(c => c.IdGame == idGame && c.IdUser == idUser);
            if (existGame == null) return NotFound("not found");
            var existBill= _context.Bill
                            .Where(b => b.IdUser == idUser && b.IdGame == idGame && b.Actions == "pay")
                            .OrderByDescending(b => b.DatePay).FirstOrDefault();
            return Ok(existBill);
        }
    }
}

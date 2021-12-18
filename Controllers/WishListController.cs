using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using game_store_be.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace game_store_be.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public WishListController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public WishList ExistWishList (string idGame, string idUser)
        {
            return (_context.WishList.FirstOrDefault(u => u.IdGame == idGame && u.IdUser == idUser));
        }

        [HttpGet("{idUser}")]
        public IActionResult getWishListByIdUser(string idUser)
        {
            var customMapper = new CustomMapper(_mapper);
            var wishlist = _context.WishList
                .Include(wl => wl.IdGameNavigation)
                    .ThenInclude(l => l.ImageGameDetail);

            var wishListDto = customMapper.CustomMapWishList(wishlist.ToList());

            return Ok(wishListDto);
        }

        [HttpPost("create/{idUser}")]
        public IActionResult CreateWishListByIdUser(string idUser, [FromBody] string idGame)
        {
            var newWishtList = new WishList{ IdGame = idGame, IdUser = idUser };
            _context.WishList.Add(newWishtList);
            _context.SaveChanges();
            return Ok(newWishtList);
        }

        [HttpDelete("delete/{idUser}")]
        public IActionResult DeleteWishListByIdUser(string idUser, [FromBody] string idGame)
        {
            var existWishList = ExistWishList(idGame, idUser);
            if (existWishList == null)
            {
                return NotFound(new { message = "Not found" });
            }
            _context.WishList.Remove(existWishList);
            _context.SaveChanges();
            return Ok(new { message = "Delete Success" });
        }
        [HttpGet("check-is-wishlist/{idUser}/{idGame}")]
        public IActionResult CheckIsWishList(string idUser, string idGame){
            var existWishList = ExistWishList(idGame, idUser);
            if (existWishList == null) {
                return NotFound("not found");
            }
            return Ok("found");
        }
    }
}

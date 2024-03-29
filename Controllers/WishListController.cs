﻿using AutoMapper;
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

        private WishList ExistWishList (string idGame, string idUser)
        {
            return (_context.WishList.FirstOrDefault(u => u.IdGame == idGame && u.IdUser == idUser));
        }

        [HttpGet("{idUser}/{start}/{count}")]
        public IActionResult getWishListByIdUser(string idUser, int start, int count)
        {
            var customMapper = new CustomMapper(_mapper);
            var wishlist = _context.WishList
                .Where(c => c.IdUser == idUser)
                .Include(c => c.IdGameNavigation)
                    .ThenInclude(g => g.IdDiscountNavigation)
                .Include(c => c.IdGameNavigation)
                    .ThenInclude(g => g.DetailGenre).ThenInclude(g => g.IdGenreNavigation)
                .Include(c => c.IdGameNavigation).ThenInclude(g => g.ImageGameDetail)
                .Skip(start).Take(count);
            
            var wishListDto = customMapper.CustomMapWishList(wishlist.ToList());

            return Ok(wishListDto);
        }

        [HttpPost("create/{idUser}/{idGame}")]
        public IActionResult CreateWishListByIdUser(string idUser,string idGame)
        {
            var existWishList = _context.WishList.FirstOrDefault(w => w.IdUser == idUser && w.IdGame == idGame);
            if (existWishList != null) return Ok();
            var newWishtList = new WishList{ IdGame = idGame, IdUser = idUser };
            _context.WishList.Add(newWishtList);
            _context.SaveChanges();
            return Ok("created");
        }

        [HttpDelete("delete/{idUser}/{idGame}")]
        public IActionResult DeleteWishListByIdUser(string idUser, string idGame)
        {
            var existWishList = ExistWishList(idGame, idUser);
            if (existWishList == null)
            {
                return NotFound(new { message = "Not found" });
            }
            _context.WishList.Remove(existWishList);
            _context.SaveChanges();
            return Ok("deleted");
        }
        [AllowAnonymous]
        [HttpGet("check-is-wishlist/{idUser}/{idGame}")]
        public IActionResult CheckIsWishList(string idUser, string idGame){
            var existWishList = ExistWishList(idGame, idUser);
            if (existWishList == null) {
                return Ok("not found");
            }
            return Ok("found");
        }
    }
}

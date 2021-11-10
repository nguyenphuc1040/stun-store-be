using game_store_be.Models;
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
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WishListController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/<WishListController>
        [HttpGet]
        public async Task<IActionResult> GetWishLists()
        {
            //var wishLists = await (
            //    from wl in _context.WishLists
            //);
            //return Ok(wishLists);
            return Ok("Chua biet lam :) "); 
        }

        // GET api/<WishListController>/5
        [HttpGet("{idUser}")]
        public  IActionResult GetWishListByUser(int idUser)
        {
            var wishList = _context.WishLists.Where(u => u.IdUser == idUser);
            return Ok(wishList);
        }

        // POST api/<WishListController>
        [HttpPost("create/{idUser}")]
        public async Task<IActionResult> CreateNewWishListByUser(uint idUser, [FromBody] uint idGame)
        {
            try
            {

                WishLists newWishList = new WishLists()
                {
                    //This is mock
                    Id = 4,
                    IdUser = idUser,
                    IdGame = idGame,
                };

                await _context.AddAsync<WishLists>(newWishList);
                await _context.SaveChangesAsync();

                return Ok(newWishList);
            } 
            catch (IndexOutOfRangeException e)
            {
                return Ok(new { message = e.Message });
            }
        }

        [HttpDelete("delete/{idUser}")]
        public async Task<IActionResult> DeleteWishListByUser(uint idUser, [FromBody] uint idGame)
        {
            WishLists existWishList = await _context.WishLists.FirstOrDefaultAsync (wl => wl.IdUser == idUser && wl.IdGame == idGame);
            if (existWishList != null)
            {
                _context.WishLists.Remove(existWishList);
                await _context.SaveChangesAsync();
                return Ok(new { message = "delete success" });
            }

            return NotFound((new { message = "Not found" }));
        }

        // PUT api/<WishListController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<WishListController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

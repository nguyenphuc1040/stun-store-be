//using game_store_be.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace game_store_be.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WishListController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        public WishListController(AppDbContext context)
//        {
//            _context = context;
//        }
//        // GET: api/<WishListController>
//        [HttpGet]
//        public IActionResult GetWishLists()
//        {
//            var wishList = from wl in _context.WishLists
//                           select new
//                           {
//                               _id = wl.Id,
//                               createdAt = wl.CreatedAt,
//                               updatedAt = wl.UpdatedAt,
//                               game = wl.IdGameNavigation,
//                               user = wl.IdUserNavigation ,
//                           };
//            return Ok(wishList);
//        }

//        // GET api/<WishListController>/5
//        [HttpGet("{idUser}")]
//        public IActionResult GetWishListByUser(string idUser)
//        {
//            var wishList = _context.WishLists.FirstOrDefault(u => u.IdUser == idUser);
//            return Ok(new
//            {
//                _id = wishList.Id,
//                createdAt = wishList.CreatedAt,
//                updatedAt = wishList.UpdatedAt,
//                game = wishList.IdGameNavigation,
//                user = wishList.IdUserNavigation,
//            });
//        }

//        // POST api/<WishListController>
//        [HttpPost("create/{idUser}")]
//        public async Task<IActionResult> CreateNewWishListByUser(string idUser, [FromBody] string idGame)
//        {
//            try
//            {

//                WishLists newWishList = new WishLists()
//                {
//                    //This is mock
//                    Id = Guid.NewGuid().ToString(),
//                    IdUser = idUser,
//                    IdGame = idGame,
//                };

//                await _context.AddAsync<WishLists>(newWishList);
//                await _context.SaveChangesAsync();

//                return Ok(newWishList);
//            } 
//            catch (IndexOutOfRangeException e)
//            {
//                return Ok(new { message = e.Message });
//            }
//        }

//        [HttpDelete("delete/{idUser}")]
//        public async Task<IActionResult> DeleteWishListByUser(string idUser, [FromBody] string idGame)
//        {
//            WishLists existWishList = await _context.WishLists.FirstOrDefaultAsync (wl => wl.IdUser == idUser && wl.IdGame == idGame);
//            if (existWishList != null)
//            {
//                _context.WishLists.Remove(existWishList);
//                await _context.SaveChangesAsync();
//                return Ok(new { message = "delete success" });
//            }

//            return NotFound((new { message = "Not found" }));
//        }

//        // PUT api/<WishListController>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {

//        }

//        // DELETE api/<WishListController>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}

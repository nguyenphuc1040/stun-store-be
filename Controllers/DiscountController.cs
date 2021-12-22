using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using game_store_be.CustomModel;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public DiscountController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Discount ExistDiscount(string idDiscount)
        {
            return _context.Discount.FirstOrDefault(dc => dc.IdDiscount == idDiscount);
        }

        [HttpGet]
        public IActionResult GetAllDiscount()
        {
            var discount = _context.Discount.ToList();
            var discountDto = _mapper.Map<IEnumerable<DiscountDto>>(discount);
            return Ok(discountDto);
        }

        [HttpGet("{idDiscount}")]
        public IActionResult GetDiscountById(string idDiscount)
        {
            var existDiscount = ExistDiscount(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not found" });
            }
            var discountDto = _mapper.Map<Discount, DiscountDto>(existDiscount);
            return Ok(discountDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public IActionResult CreateDiscount([FromBody] PostDiscountBody newDiscountBody)
        {
            var newDiscount = newDiscountBody.Discount;
            var listGameDiscount = newDiscountBody.ListGameDiscount;
            var idDiscount = Guid.NewGuid().ToString();
            newDiscount.IdDiscount = idDiscount;


            if (listGameDiscount != null)
            {
                var listGameFound = new List<Game>();
                foreach (var idGame in listGameDiscount)
                {
                    var gameFound = _context.Game.FirstOrDefault(g => g.IdGame == idGame);
                    if (gameFound == null)
                    {
                        return NotFound(new { message = idGame + " not found" });
                    }
                    listGameFound.Add(gameFound);
                }
                foreach (var gameFound in listGameFound)
                {
                    gameFound.IdDiscount = idDiscount;
                }
            }

            _context.Discount.Add(newDiscount);
            _context.SaveChanges();
            var discountDto = _mapper.Map<Discount, DiscountDto>(newDiscount);
            return Ok(discountDto);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("update/{idDiscount}")]
        public IActionResult UpdateDiscountById(string idDiscount, [FromBody] PostDiscountBody newDiscountBody)
        {
            var newDiscount = newDiscountBody.Discount;
            var listGameDiscount = newDiscountBody.ListGameDiscount;

            var existDiscount = ExistDiscount(idDiscount);
            if (existDiscount == null)
            {
                return NotFound(new { message = "Not found" });
            }

            var listGameOldDiscount = _context.Game.Where(g => g.IdDiscount == idDiscount);
            foreach (var game in listGameOldDiscount)
            {
                game.IdDiscount = null;
            }
            _context.SaveChanges();

            var listGameFound = new List<Game>();
            foreach (var idGame in listGameDiscount)
            {
                var gameFound = _context.Game.FirstOrDefault(g => g.IdGame == idGame);
                if (gameFound == null) return NotFound(new { message = idGame + " not found" });
                listGameFound.Add(gameFound);
            }

            for (int i = 0; i < listGameFound.Count; i++)
            {
                listGameFound.ElementAt(i).IdDiscount = idDiscount;
            }

            newDiscount.IdDiscount = idDiscount;
            _mapper.Map<Discount, Discount>(newDiscount, existDiscount);
            _context.SaveChanges();
            return Ok(newDiscount);
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("delete/{idDiscount}")]
        public IActionResult DeleteDiscount(string idDiscount){
            var listGameDiscount = _context.Game.Where(g => g.IdDiscount == idDiscount).ToList();
            foreach (var game in listGameDiscount) {
                game.IdDiscount = null;
            }
           
            var existDiscount = _context.Discount.FirstOrDefault(d => d.IdDiscount == idDiscount);
            _context.Discount.Remove(existDiscount);

            _context.SaveChanges();
            return Ok("success");
        }
        [Authorize(Roles = "admin")]
        [HttpGet("get-game-by-discount/{idDiscount}")]
        public IActionResult GetGameByDiscount(string idDiscount){
            var existGame = _context.Game.Where(g => g.IdDiscount == idDiscount).ToList();
            var existGameDto = _mapper.Map<IEnumerable<GameDto>>(existGame).ToList(); 
            return Ok(existGameDto);
        }
        [AllowAnonymous]
        [HttpDelete("delete/outdate/{idDiscount}")]
        public IActionResult DeleteDiscountOutDate(string idDiscount){
            var existDiscount = _context.Discount.FirstOrDefault(d => d.IdDiscount == idDiscount);
            if (existDiscount == null) return Ok();
            int isOutDate = DateTime.Compare(existDiscount.EndDate,DateTime.Now);
            if (isOutDate >= 0) return Ok();

            var listGameDiscount = _context.Game.Where(g => g.IdDiscount == idDiscount).ToList();
            if (listGameDiscount == null) return Ok();
            foreach (var game in listGameDiscount) {
                game.IdDiscount = null;
            }
            _context.SaveChanges();
            return Ok();
        }
    }
}

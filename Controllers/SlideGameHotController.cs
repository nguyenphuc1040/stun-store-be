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

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlideGameHotController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public SlideGameHotController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private SlideGameHot GetSlideGameHotByIdGameService(string idGame)
        {
            return _context.SlideGameHot.FirstOrDefault(dc => dc.IdGame == idGame);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public IActionResult CreateSlideGameHot([FromBody] SlideGameHot newSlide)
        {
            _context.SlideGameHot.Add(newSlide);
            _context.SaveChanges();
            return Ok(newSlide);
        }

        [HttpGet]
        public IActionResult GetAllSlideGameHot()
        {
            var slideGameHots = _context.SlideGameHot.Include(u => u.IdGameNavigation).ThenInclude(u => u.ImageGameDetail).ToList();
            var slideGameHotDtos = _mapper.Map<ICollection<SlideGameHotDto>>(slideGameHots);
            return Ok(slideGameHotDtos);
        }

        
    }
}

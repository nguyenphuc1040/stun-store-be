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
    [Route("api/[controller]")]
    [ApiController]
    public class LikeCommentController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public LikeCommentController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllLikeCommet()
        {
            var listLikeComment = _context.LikeComment.Include(a => a.IdCommentNavigation).ToList();
            return Ok(listLikeComment);
        }

        [HttpPost("create")]
        public IActionResult CreateLikeComment([FromBody] LikeComment newLikeComment)
        {
            _context.LikeComment.Add(newLikeComment);
            _context.SaveChanges();
            return Ok(newLikeComment);
        }
    }
}

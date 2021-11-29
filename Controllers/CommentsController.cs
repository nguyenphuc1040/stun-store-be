using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public CommentsController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{idGame}")]
        public IActionResult GetCommentByIdGame(string idGame)
        {
            var comments = _context.Comments
                .Where(cmt => cmt.IdGame == idGame).ToList();
            var commentsDto = _mapper.Map<IEnumerable<CommentsDto>>(comments);

            return Ok(commentsDto);
        }
        [HttpGet]
        public IActionResult GetAllComment()
        {
            var comments = _context.Comments.ToList();
            var commentsDto = _mapper.Map<IEnumerable<CommentsDto>>(comments);

            return Ok(commentsDto);
        }
    }
}

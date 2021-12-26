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

        /// <summary>
        /// Get comment by idGame
        /// </summary>
        /// <param name="idGame"></param>
        /// <returns>an array comment by idGamm</returns>
        [HttpGet("{idGame}")]
        public IActionResult GetCommentByIdGame(string idGame)
        {
            var comments = _context.Comments
                .Where(cmt => cmt.IdGame == idGame)
                .OrderBy(cmt => cmt.Time).ToList();
            var commentsDto = _mapper.Map<IEnumerable<CommentsDto>>(comments);

            return Ok(commentsDto);
        }

        /// <summary>
        /// Get comment count by idGame
        /// </summary>
        /// <param name="idGame"></param>
        /// <returns>a count comment</returns>
        [HttpGet("count/{idGame}")]
        public IActionResult GetCommentCountByIdGame(string idGame)
        {
            var comments = _context.Comments
                .Where(cmt => cmt.IdGame == idGame).Count();

            return Ok(comments);
        }

        /// <summary>
        /// Get all comment
        /// </summary>
        /// <returns>an array comment</returns>
        [HttpGet]
        public IActionResult GetAllComment()
        {
            var comments = _context.Comments.ToList();
            var commentsDto = _mapper.Map<IEnumerable<CommentsDto>>(comments);

            return Ok(commentsDto);
        }

        /// <summary>
        /// Get comment of Game of User( by idGame, idUser)
        /// </summary>
        /// <param name="idGame"></param>
        /// <param name="idUser"></param>
        /// <returns>an array comment</returns>
        [HttpGet("{idGame}/{idUser}")]
        public IActionResult GetCommentOfGameOfUser(string idGame, string idUser){
            
            var comments = _context.Comments
                .FirstOrDefault(cmt => cmt.IdUser == idUser && cmt.IdGame == idGame);
            return Ok(comments);
        }
    }
}

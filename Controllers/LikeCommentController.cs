using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using game_store_be.Models;

namespace game_store_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeCommentController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        public LikeCommentController(game_storeContext context, IMapper mapper){
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get like comment
        /// </summary>
        /// <param name="idComment"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        [HttpGet("{idComment}/{idUser}")]
        public IActionResult LikeCommentUserInComment(string idComment, string idUser){
            
            var existLikeComment = _context.LikeComment
                .FirstOrDefault(e => e.IdComment == idComment && e.IdUser == idUser);

            if (existLikeComment == null) return Ok(new {own = "none"});
            
            return Ok(existLikeComment.IsLike ? new {own = "1"} : new {own = "0"});
        }

        /// <summary>
        /// Get count like comment by id
        /// </summary>
        /// <param name="idComment"></param>
        /// <returns></returns>
        [HttpGet("{idComment}")]
        public IActionResult GetCountLikeComment(string idComment){
            
            var existLikeComment = _context.LikeComment
                .Where(e => e.IdComment == idComment && e.IsLike == true).Count();
            var existDisLikeComment = _context.LikeComment
                .Where(e => e.IdComment == idComment && e.IsLike == false).Count();
            
            return Ok(new {like = existLikeComment, dislike = existDisLikeComment});
        }

        /// <summary>
        /// Get update like comment
        /// </summary>
        /// <param name="likeCmt"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateLikeComment([FromBody] LikeComment likeCmt){
            
            var existLikeComment = _context.LikeComment
                .FirstOrDefault(e => e.IdComment == likeCmt.IdComment && e.IdUser == likeCmt.IdUser);
            
            if (existLikeComment == null) return NotFound(new {message = "404"});
            existLikeComment.IsLike = likeCmt.IsLike;
            _mapper.Map(likeCmt, existLikeComment);
            _context.SaveChanges();

            return Ok(existLikeComment);
        }

        /// <summary>
        /// Delete Like comment
        /// </summary>
        /// <param name="likeCmt"></param>
        /// <returns></returns>
        // [Authorize]
        [HttpDelete("delete")]
        public IActionResult DeleteLikeComment([FromBody] LikeComment likeCmt){
            
            var existLikeComment = _context.LikeComment
                .FirstOrDefault(e => e.IdComment == likeCmt.IdComment && e.IdUser == likeCmt.IdUser);
            
            if (existLikeComment == null) return NotFound(new {message = "404"});
            _context.LikeComment.Remove(existLikeComment);
            _context.SaveChanges();
            return Ok(likeCmt);
        }
    }
}
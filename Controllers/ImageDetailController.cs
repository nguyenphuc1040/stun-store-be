﻿using AutoMapper;
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
    public class ImageDetailController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public ImageDetailController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private ImageGameDetail GetDiscountByIdService(string idImageDetail)
        {
            return _context.ImageGameDetail.FirstOrDefault(dc => dc.IdImage == idImageDetail);
        }

        [HttpGet]
        public IActionResult GetAllImageDetail()
        {
            var imageDetails = _context.ImageGameDetail.ToList();
            return Ok(imageDetails);
        }

        [HttpPost("create")]
        public IActionResult CreateImageDetail([FromBody] ImageGameDetail newImageDetail)
        {
            newImageDetail.IdImage = Guid.NewGuid().ToString();
            _context.Add(newImageDetail);
            _context.SaveChanges();
            return Ok(newImageDetail);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("delete/{idImageDetail}")]
        public IActionResult DeleteImageDetailById(string idImageDetail)
        {
            var existImageDetail = GetDiscountByIdService(idImageDetail);
            if (existImageDetail == null)
            {
                return NotFound(new { message = "Not Found" });

            }
            _context.Remove(existImageDetail);
            _context.SaveChanges();
            return Ok(new { message = "Delete Success" });
        }
    }
}

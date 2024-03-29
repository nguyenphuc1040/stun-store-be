﻿using AutoMapper;
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
    public class DetailGenreController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;

        public DetailGenreController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllDetailGenre ()
        {
            var detailGenres = _context.DetailGenre.ToList();
            return Ok(detailGenres);
        }

        [Authorize(Roles ="admin")]
        [HttpPost("create")]
        public IActionResult CreateDetailGenre([FromBody] DetailGenre newDetailGenre )
        {
            _context.Add(newDetailGenre);
            _context.SaveChanges();
            return Ok(newDetailGenre);
        }
    }
}

using AutoMapper;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        public UserController(game_storeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private string HassPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        private Users ExistUser(string idUser)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            return existUser;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var users = _context.Users.ToList();
            var userDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDto);
        }

        [HttpGet("{idUser}")]
        public IActionResult GetUserById(string idUser)
        {
            var existUser = ExistUser(idUser);
            if (existUser == null)
            {
                return NotFound(new { message = "Not found" });
            }
            var userDto = _mapper.Map<Users, UserDto>(existUser);
            return Ok(userDto);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Users newUser)
        {
            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = HassPassword(newUser.Password);

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok(newUser);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login infoLogin)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Email == infoLogin.Email && u.Password == HassPassword(infoLogin.Password));
            if (existUser == null)
            {
                return NotFound(new { message = "Info login is incorrect" });
            }
            return Ok(existUser);
        }

        [HttpPut("update/{idUser}")]
        public IActionResult UpdateUser (string idUser, [FromBody] UserDto newUser)
        {
            var existUser = ExistUser(idUser);
            if (existUser == null)
            {
                return NotFound(new { message ="Not found"});
            }
            newUser.IdUser = existUser.IdUser;
            _mapper.Map(newUser, existUser);
            _context.SaveChanges();
            return Ok(newUser);
        }
    }
}

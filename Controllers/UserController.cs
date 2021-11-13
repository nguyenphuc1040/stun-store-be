using game_store_be.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace game_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
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
        private readonly game_storeContext _context;
        public UserController(game_storeContext context)
        {
            _context = context;
        }
        // GET: api/<UserController>

        private Users GetUserByIdService(string idUser)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            return existUser;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            return Ok(_context.Users.ToList());
        }

        // GET api/<UserController>/5
        [HttpGet("{idUser}")]
        public IActionResult GetUserById(string idUser)
        {
            var existUser = GetUserByIdService(idUser);
            if (existUser == null)
            {
                return NotFound(new { message = "Not found" });
            }
            return Ok(existUser);
        }

        // POST api/<UserController>
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users newUser)
        {
            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = HassPassword(newUser.Password);

            _context.Add(newUser);
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

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}

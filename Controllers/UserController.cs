using AutoMapper;
using game_store_be.CustomModel;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace game_store_be.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserController(game_storeContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
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

        private string CreateJWT(Users user) {

            var secretKey = _config.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser),
                new Claim(ClaimTypes.Role, user.Roles),
            };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature
            );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Users ExistUser(string idUser)
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

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Users newUser)
        {
            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = HassPassword(newUser.Password);
            newUser.Roles = "user";

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login infoLogin)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.Email == infoLogin.Email && u.Password == HassPassword(infoLogin.Password));
            if (existUser == null)
            {
                return NotFound(new { message = "Info login is incorrect" });
            }

            var loginRes = new LoginRes();
            loginRes.Username = existUser.UserName;
            loginRes.Token = CreateJWT(existUser);
            return Ok(loginRes);
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

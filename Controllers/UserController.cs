using System.Net;
using AutoMapper;
using game_store_be.CustomModel;
using game_store_be.Dtos;
using game_store_be.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SmtpManager;

namespace game_store_be.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly game_storeContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private static Hashtable codeVerify = new Hashtable();
        public UserController(game_storeContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
        }

        private LoginRes CreateResLoginSuccess(Users infoUser)
        {
            string token = CreateJWT(infoUser);
            var userDto = _mapper.Map<Users, UserDto>(infoUser);

            return new LoginRes() { Token = token, User = userDto };
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

        private string CreateJWT(Users user)
        {

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
            string lastToken = HttpContext.Request.Headers["token"];
            var loginRes = new LoginRes();
            var findUser = new Users();

            if (lastToken == null || lastToken == "")
            {
                var existUser = _context.Users
                    .FirstOrDefault(u => (u.Email == infoLogin.Email || u.UserName == infoLogin.Email) && u.Password == HassPassword(infoLogin.Password));

                if (existUser == null)
                {
                    return NotFound(new { message = "Info login is incorrect" });
                }

                return Ok(CreateResLoginSuccess(existUser));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(lastToken);

            var idUser = jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value;

            var user = ExistUser(idUser);
            if (user == null) return Unauthorized(new { message = "Invalid token" });
            return Ok(CreateResLoginSuccess(user));
        }

        [HttpPut("update/{idUser}")]
        public IActionResult UpdateUser(string idUser, [FromBody] UserDto newUser)
        {
            var existUser = ExistUser(idUser);
            if (existUser == null)
            {
                return NotFound(new { message = "Not found" });
            }
            newUser.IdUser = existUser.IdUser;
            _mapper.Map(newUser, existUser);
            _context.SaveChanges();
            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("login-sma")]
        public IActionResult LoginWithGoogle([FromBody] LoginWithSMA infoLogin)
        {
            var existUser = _context.Users
                            .FirstOrDefault(u => u.Email == infoLogin.ILogin.Email);
            
            if (existUser == null)
            {
                return Ok(CreateResLoginSuccess(RegisterAccount(infoLogin.IUser)));
            }

            return Ok(CreateResLoginSuccess(existUser));
        }

        public Users RegisterAccount(Users newUser)
        {
            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = null;
            newUser.Roles = "user";

            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        [AllowAnonymous]
        [HttpGet("check-valid-username/{username}")]
        public IActionResult CheckValidUsername(string username){
            var existUsername = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (existUsername == null) {
                return Ok(new { message = "valid"});
            } else {
                return Ok(new { message = "invalid"});
            }              
        }

        [HttpPost("change-password")]
        public IActionResult ChangePassword(){
            string idUser = HttpContext.Request.Headers["idUser"];
            string pwd = HttpContext.Request.Headers["pwd"];

            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);

            if (existUser == null) {
                return NotFound(new {message = "User not exists"});
            }
            existUser.Password = HassPassword(pwd);
            _context.SaveChanges();
            return Ok(new {message = "Change password sucessful !"});
        }

        [HttpPut("change-info/{idUser}")]
        public IActionResult ChangeInfo(string idUser,[FromBody] Users infoUser){
            
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);

            if (existUser == null) {
                return NotFound(new {message = "User not exists"});
            }
            if (infoUser.Avatar != null) existUser.Avatar = infoUser.Avatar;
            if (infoUser.RealName != null) existUser.RealName = infoUser.RealName;
            if (infoUser.Background != null) existUser.Background = infoUser.Background;
            _context.SaveChanges(); 
            return Ok(existUser);
        }
        [AllowAnonymous]
        [HttpPost("send-mail-reset-pwd/{email}")]
        public IActionResult SendMailResetPwd(string email){
            var existUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existUser == null) return NotFound(new {message = "Email is not registered"});
            var rand = new Random();
            string code = rand.Next(111111,988888).ToString();
            codeVerify[email] = code;
            bool result = SmtpController.CreateResetPasswordVerify(email,code);
            return Ok(new {message = result});
        }
        [AllowAnonymous]
        [HttpPost("send-mail-confirm-account/{email}")]
        public IActionResult SendMailConfirmAccount(string email){
            var rand = new Random();
            string code = rand.Next(111111,988888).ToString();
            codeVerify[email] = code;
            bool result = SmtpController.CreateEmailVerify(email,code);
            return Ok(new {message = result});
        }
        [AllowAnonymous]
        [HttpGet("code/{email}/{code}")]
        public IActionResult Code(string email, string code){
            if (codeVerify[email]==null){
                return NotFound(new {message = "not found"});
            } else {
                var result = codeVerify[email].ToString()==code ? true : false;
                if (result) {
                    codeVerify.Remove(email);
                }
                return Ok(new {message = result});
            }
        }
    }
}

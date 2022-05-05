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
using static Pirexcs.UserIC;

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
                new Claim(ClaimTypes.Name, user.UserName.ToLower()),
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

        private Users ExistUser(string idUser)
        {
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            return existUser;
        }
        private Users RegisterAccountSMA(Users newUser)
        {
            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = null;
            newUser.Roles = "user";
            newUser.ConfirmEmail = true;
            var username ="";
            while (true){
                username = CreateUsername(newUser.RealName);
                var existUsername = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
                if (existUsername == null) break;
            }
            
            newUser.UserName = username;
            newUser.Avatar = CreateAvatar(username);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
        [HttpGet("{start}/{count}")]
        public IActionResult GetAllUser(int start, int count)
        {
            var users = _context.Users.Skip(start).Take(count).ToList();
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
            var existUser = _context.Users.FirstOrDefault(u => u.Email.ToLower() == newUser.Email.ToLower());
            if (existUser != null) return NotFound("Email already register");
            existUser = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == newUser.UserName.ToLower());
            if (existUser != null) return NotFound("Username already register");

            newUser.IdUser = Guid.NewGuid().ToString();
            newUser.Password = HassPassword(newUser.Password);
            newUser.Roles = "user";
            newUser.Avatar = CreateAvatar(newUser.UserName);
            newUser.ConfirmEmail = false;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Ok(new {message = "Register Sucessful !"});
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
                    .FirstOrDefault(u => (u.Email.ToLower() == infoLogin.Email.ToLower() || u.UserName.ToLower() == infoLogin.Email.ToLower()) && u.Password == HassPassword(infoLogin.Password));

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
        public IActionResult LoginWithSMA([FromBody] LoginWithSMA infoLogin)
        {
            var existUser = _context.Users
                            .FirstOrDefault(u => u.Email.ToLower() == infoLogin.ILogin.Email.ToLower());
            
            if (existUser == null)
            {
                return Ok(CreateResLoginSuccess(RegisterAccountSMA(infoLogin.IUser)));
            }

            return Ok(CreateResLoginSuccess(existUser));
        }


        [AllowAnonymous]
        [HttpGet("check-valid-email/{email}")]
        public IActionResult CheckValidEmail(string email){
            var existUsername = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (existUsername == null) {
                return Ok("valid");
            } else {
                return NotFound("Email already register");
            }              
        }
        [AllowAnonymous]
        [HttpGet("check-valid-username/{username}")]
        public IActionResult CheckValidUsername(string username){
            var existUsername = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
            if (existUsername == null) {
                return Ok("valid");
            } else {
                return NotFound("Stun ID already exist");
            }              
        }
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(){
            string idUser = HttpContext.Request.Headers["idUser"];
            string pwd = HttpContext.Request.Headers["pwd"];

            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);

            if (existUser == null) {
                return NotFound("User not exists");
            }
            existUser.Password = HassPassword(pwd);
            _context.SaveChanges();
            return Ok("Change password sucessful !");
        }
        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePassBody pwd){
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == pwd.IdUser);
            if (existUser == null) {
                return NotFound("User not exists");
            }
            if (existUser.Password != HassPassword(pwd.CurrentPass)) return NotFound("Wrong current password");
            existUser.Password = HassPassword(pwd.NewPass);
            _context.SaveChanges();
            return Ok("Change password sucessful !");
        }
        [HttpGet("check-sma-account/{idUser}")]
        public IActionResult CheckSmaAccount(string idUser){
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            if (existUser == null) return NotFound("not found user");
            if (existUser.Password == null) return Ok("true");
            return Ok("false");
        }
        [HttpPut("change-info/{idUser}")]
        public IActionResult ChangeInfo(string idUser,[FromBody] Users infoUser){
            
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);

            if (existUser == null) {
                return NotFound("User not exists");
            }
            if (infoUser.Avatar != null) existUser.Avatar = infoUser.Avatar;
            if (infoUser.RealName != null) existUser.RealName = infoUser.RealName;
            if (infoUser.Background != null) existUser.Background = infoUser.Background;
            if (infoUser.UserName != null) existUser.UserName = infoUser.UserName;
            if (infoUser.NumberPhone != null) existUser.NumberPhone = infoUser.NumberPhone;
            _context.SaveChanges(); 
            var userDto = _mapper.Map<Users, UserDto>(existUser);
            return Ok(userDto);
        }
        [AllowAnonymous]
        [HttpPost("send-mail-reset-pwd")]
        public IActionResult SendMailResetPwd(){
            string email = HttpContext.Request.Headers["email"];
            email = email.ToLower();
            var existUser = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
            if (existUser == null) return NotFound("Email is not registered");
            var rand = new Random();
            string code = rand.Next(111111,988888).ToString();
            codeVerify[email] = code;
            bool result = SmtpController.CreateResetPasswordVerify(existUser.Email,code,existUser.IdUser);
            return Ok(new {message = result});
        }
        [AllowAnonymous]
        [HttpPost("send-mail-confirm-account")]
        public IActionResult SendMailConfirmAccount(){
            string email = HttpContext.Request.Headers["email"];
            email = email.ToLower();
            var existUser = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email);
            if (existUser == null) return NotFound("Email is not registered");
            var rand = new Random();
            string code = rand.Next(111111,988888).ToString();
            codeVerify[email] = code;
            bool result = SmtpController.CreateEmailVerify(email, code, existUser.IdUser);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("verification/code")]
        public IActionResult CodeVerify([FromBody] Verification info){
            info.Email = info.Email.ToLower();
            var existUser = _context.Users.FirstOrDefault(u => u.Email.ToLower() == info.Email);
            if (existUser == null) return NotFound("User not exists");
            if (codeVerify[info.Email]==null){
                return NotFound("Fail to verification");
            } else {
                var result = codeVerify[info.Email].ToString()==info.Code ? true : false;
                if (result) {
                    codeVerify.Remove(info.Email);
                    existUser.ConfirmEmail = true;
                    _context.SaveChanges();
                    return Ok(CreateResLoginSuccess(existUser));
                }
                return NotFound("Code Wrong, Try Again");
            }
        }
        [AllowAnonymous]
        [HttpPost("verification/link")]
        public IActionResult LinkVerify(){
            string url = HttpContext.Request.Headers["url"];
            string code = "";
            int j = 1;
            for (int i=0; i<6; i++) {
                code += (char)(url[j]-49);
                j+= 3;
            }
            j = 1;
            for (int i=0; i<6; i++) {
                url = url.Remove(j,1);
                j+=2;
            }
            Console.WriteLine(url);
                  Console.WriteLine(code);
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == url);
            if (existUser == null) return NotFound(new {message = "User not exists"});
            if (codeVerify[existUser.Email]==null){
                return NotFound("Fail to verification");
            } 
            var result = codeVerify[existUser.Email].ToString() == code ? true : false;
            if (result){
                codeVerify.Remove(existUser.Email);
                existUser.ConfirmEmail = true;
                _context.SaveChanges();
                return Ok(CreateResLoginSuccess(existUser));
            }
            return NotFound("Fail to verification");
        }
        [AllowAnonymous]
        [HttpGet("verification-email-status/{idUser}")]
        public IActionResult GetVerificationEmailStatus(string idUser){
            var existUser = _context.Users.FirstOrDefault(u => u.IdUser == idUser);
            if (existUser == null) return NotFound("Not found user");
            return Ok(existUser.ConfirmEmail);
        }
        
    }
}

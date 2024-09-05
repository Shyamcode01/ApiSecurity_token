using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Wepapi_Management.Model;

namespace Wepapi_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public AuthController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpPost("AuthLogin")]

        public IActionResult LoginUser(Login _login)
        {
            if (_login != null)
            {
                var userverify = _context.Application.Where(x => x.Email == _login.email);
                //var password = !BCrypt.Net.BCrypt.Verify(_login.password, userverify.PasswordHash);

               if(userverify != null)
                {
                    var claims = new[]
               {
                   new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                   new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                   new Claim("Email",_login.email.ToString()),

               };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signig = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                        _config["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(2),
                        signingCredentials: signig
                        );

                    string tokenvalue = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { token = tokenvalue });
                }
                else
                {
                    return BadRequest("user and password worng ");
                }
                

            }
            else
            {
                return BadRequest("null value not alloude");
            }
        }

 

    }
}

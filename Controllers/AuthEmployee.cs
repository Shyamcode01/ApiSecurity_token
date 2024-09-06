using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Wepapi_Management.Model;

namespace Wepapi_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthEmployee : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration  _config;

        public AuthEmployee(ApplicationDbContext context,IConfiguration configuration)
        {

            _context = context;
            _config = configuration;

        }

        [HttpPost("Useradd")]
        public IActionResult Adduser(user _user)
        {
            var res = _context.Users.Add(_user);
            _context.SaveChanges();
            return Ok(res);
        }

        [HttpPost("roleAdd")]
        public IActionResult roleadd(role _role)
        {
            var res=_context.roles.Add(_role);
            _context.SaveChanges();
            return Ok(res);
        }
        [HttpPost("UserROLE")]
        public bool Assignrole(AddUserRole userrole)
        {
            try
            {
                var addrole = new List<userRole>();
                var addrole_ = _context.Users.SingleOrDefault(x => x.id == userrole.userid);
                if (addrole_ == null)

                    throw new Exception("not valid");

                foreach (int role in userrole.roleids)
                {
                    var userRole = new userRole();
                    userRole.roleId = role;
                    userRole.userId = userrole.userid;
                    addrole.Add(userRole);
                }
                _context.userRoles.AddRange(addrole);
                _context.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }

             

        }
        [HttpPost("UserLoginRequest")]
        public IActionResult userloginRequest(LoginRequest request)
        {
           if(request != null)
            {
                var user = _context.Users.SingleOrDefault(x => x.username == request.username && x.password == request.password);

                if(user != null)
                {
                    var claims=new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                        new Claim("id",user.name)
                    };
                    var userRole = _context.userRoles.Where(x => x.userId == user.id);
                    var roleid = userRole.Select(x => x.roleId);
                    var role=_context.roles.Where(x=>roleid.Contains(x.Id)).ToList();

                    foreach(var data in role)
                    {
                        claims.Add(new Claim("Role", data.Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var sigin=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _config["Jwt:Issuer"],
                        _config["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(2),
                        signingCredentials: sigin
                        ) ;
                    var jwttoken=new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { token = jwttoken });
                }
                else
                {
                    return BadRequest("not null");
                }
            }
            else
            {
                return BadRequest("not null allowed");
            }


        }
    }
}

using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wepapi_Management.Model;

namespace Wepapi_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public StudentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Route("Registraion")]
        [HttpPost]
        public ActionResult Registration(ApplicationUser user)
        {
            if (user != null)
            {
                var result = _context.Application.FirstOrDefault(x => x.Email == user.Email);
                if (result == null)
                {
                    ApplicationUser userdata = new ApplicationUser()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash),
                        Role = user.Role,
                        ProfilePicture = user.ProfilePicture,

                    };

                    var data = _context.Application.Add(userdata);
                    _context.SaveChanges();
                    return Ok("data added success fully");
                }
                else
                {
                    return BadRequest("user all ready exist");
                }
            }
            else {
                return BadRequest("Null value not require");
            }

        }

        [Route("GetLists")]
        [HttpGet]
        [Authorize(Roles="admin")]
        public IActionResult GetList()
        {
            var listdata = _context.Application.ToList();
            return Ok(listdata);

        }
         
        [HttpPut("DataUpdate/{id}")]
        public IActionResult Updatedata(ApplicationUser user, int id)
        {
            var data = _context.Application.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                var result = _context.Application.Update(user);
                _context.SaveChanges();
                return Ok("data update successfully");
            }
            else
            {
                return BadRequest("data is not valid");
            }

        }

        [HttpDelete("DeleteUserdata/{id}")]
       
        public IActionResult Delete( int id)
        {
            var verify = _context.Application.FirstOrDefault(x => x.Id == id);
            if (verify != null)
            {
                var res = _context.Application.Remove(verify);
                _context.SaveChanges();
                return Ok("data delete success fully");
            }
            else
            {
                return BadRequest("use is not valid ");
            }
        }
    }
}

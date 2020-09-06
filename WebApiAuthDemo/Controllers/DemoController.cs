using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApiAuthDemo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {

        public DemoController()
        {

        }
        [Authorize]
        [HttpGet]
        public string Get()
        {
            //var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            var userName = User.Claims.First(p => p.Type.Contains("name")).Value;
            return userName;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("CheckAdmin")]
        public string CheckAdminRole()
        {
            return "Admin";
        }
        [Authorize]
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [Authorize]
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }
    }
    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebApiAuthDemo.Controllers
{
    /**
     * https://blog.miniasp.com/post/2019/10/13/How-to-use-JWT-token-based-auth-in-aspnet-core-22
     **/

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _configuration;
        private UserValidateService _userValidateService;
        private UserInfoService _userInfoService;

        public TokenController(IConfiguration configuration, UserValidateService userValidateService, UserInfoService userInfoService)
        {
            _configuration = configuration;
            _userValidateService = userValidateService;
            _userInfoService = userInfoService;
        }
        //[ApiExplorerSettings(IgnoreApi = true)]
        [Route("Signin")]
        [HttpPost]
        public ActionResult<string> SignIn(LoginViewModel login)
        {
            // 以下變數值應該透過 IConfiguration 取得
            var issuer = _configuration["JWT:issuser"].ToString(); //"JwtAuthDemo";
            var signKey = _configuration["JWT:signKey"].ToString(); // 請換成至少 16 字元以上的安全亂碼
            var expires = Convert.ToInt32(_configuration["JWT:expires"]); // 單位: 分鐘

            if (_userValidateService.ValidateUser(login))
            {
                return JwtHelpers.GenerateToken(issuer, signKey, login.Username, expires, _userInfoService.GetApiRoles(login.Username), JsonConvert.SerializeObject(_userInfoService.GetUserInfo(login.Username)));
            }
            else
            {
                return BadRequest();
            }
        }

        private bool ValidateUser(LoginViewModel login)
        {
            return true; // TODO
        }
    }
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class PayLoad
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Companys { get; set; }
    }
}
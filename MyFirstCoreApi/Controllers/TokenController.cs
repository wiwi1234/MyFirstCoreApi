using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstCoreApi.Helpers;
using MyFirstCoreApi.Models;

namespace MyFirstCoreApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly JwtHelper jwt;
        public TokenController(JwtHelper jwtHelper)
        {
            this.jwt = jwtHelper;
        }

        //內建驗證login
        [AllowAnonymous]  //繞過驗證
        [HttpPost]
        [Route("SignIn")]
        public ActionResult<string> SignIn(LoginModel login)
        {
            if (ValidateUser(login))
            {
                return jwt.GenerateToken(login.Username);
            }
            else
            {
                return BadRequest();
            }
        }

        private bool ValidateUser(LoginModel login)
        {
            //Todo...
            return true;
        }

        [HttpGet]
        [Route("GetClaims")]
        public IActionResult GetClaims()
        {
            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [HttpGet]
        [Route("GetUserName")]
        public IActionResult GetUserName()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet]
        [Route("GetUniqueId")]
        public IActionResult GetUniqueId()
        {
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }

    }
}

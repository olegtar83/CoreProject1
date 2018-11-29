using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using webapp.Services.Abstractions;
using webapp.Models;
using Microsoft.Extensions.Logging;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private IJwtHelper _jwtHelper;
        private IUserRepository _userRepository;


        public AuthController(IConfiguration config, ILogger<AuthController> logger,IJwtHelper jwtHelper, IUserRepository userRepository) {
            this._userRepository = userRepository;
            this._config = config;
            this._logger = logger;
            this._jwtHelper = jwtHelper;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            _logger.LogInformation("logs working");
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
            var res = _userRepository.LoginUser(user.UserName, user.Password);

            if (user.UserName == "oleg"&& user.Password == "123")
            {
                var claims = new List<Claim> {
                   new Claim(ClaimTypes.Name,user.UserName),
                   new Claim(ClaimTypes.Role, "Manager")
                };
                var tokenString = _jwtHelper.getToken(claims);
                return Ok(new { Token = tokenString,Name= claims.GetClaimValueByType(ClaimTypes.Name) });
            }
            else
            {
                return Unauthorized();
            }
        }


    }
}
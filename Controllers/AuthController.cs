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
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtHelper _jwtHelper;
        private IUserRepository _userRepo;
        private readonly IMapper _mapper;


        public AuthController(IConfiguration config, ILogger<AuthController> logger,IJwtHelper jwtHelper, IUserRepository userRepo, IMapper mapper) {
            this._userRepo = userRepo;
            this._config = config;
            this._logger = logger;
            this._jwtHelper = jwtHelper;
            this._mapper = mapper;
        }

        [HttpPost, Route("login")]
        public async Task< IActionResult> Login([FromBody]LoginModel user)
        {
            _logger.LogInformation("loging working");
            if (user.IsNull())
            {
                return BadRequest("Invalid client request");
            }
            var requestedUser = await _userRepo.LoginUser(user.UserName, user.Password);

            if (!requestedUser.IsNull())
            {

                var claims = new List<Claim> {
                   new Claim(ClaimTypes.Name,requestedUser.UserName),
                   new Claim(ClaimTypes.Role, requestedUser.Role)
                };
                var tokenString = _jwtHelper.getToken(claims);
                return Ok(new { Token = tokenString,Name= requestedUser.FirstName+" "+ requestedUser.LastName });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost,Route("register")]
        public async Task<IActionResult> Register([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
            var item = _mapper.Map<User>(user);
            try
            {
                await _userRepo.AddUser(item);
                var claims = new List<Claim> {
                   new Claim(ClaimTypes.Name,user.UserName),
                   new Claim(ClaimTypes.Role, "User")
                };
                var tokenString = _jwtHelper.getToken(claims);
                return Ok(new { Token = tokenString, Name = claims.GetClaimValueByType(ClaimTypes.Name) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


namespace webapp.Services
{
    public class JwtHelper : IJwtHelper
    {
        private IConfiguration _config;

        public JwtHelper(IConfiguration config) {
            this._config = config;
        }

        public string getToken(List<Claim> claims) {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["ak:superSecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "Issuer",
                audience: "Audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
           return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }
    }
}

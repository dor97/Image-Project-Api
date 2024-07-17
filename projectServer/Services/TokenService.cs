using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using projectServer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using projectServer.DTOs.Account;
using System.Text.Json;
using projectServer.Models.Account;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration m_config;
        private readonly SymmetricSecurityKey m_key;

        public TokenService(IConfiguration config)
        {
            m_config = config;
            m_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_config["JWT:SigningKey"]));
        }
        public string CreateToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            string customObjectJson = JsonSerializer.Serialize(new TokenDataDto() { userName = user.UserName, email = user.Email });

            claims.Add(new Claim("userData", customObjectJson));

            var creds = new SigningCredentials(m_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = m_config["JWT:Issuer"],
                Audience = m_config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
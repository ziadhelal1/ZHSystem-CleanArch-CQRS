using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZHSystem.Application.Common;
using ZHSystem.Application.Common.Interfaces;
using ZHSystem.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using ZHSystem.Application.DTOs.Auth;
using ZHSystem.Application.Common;

namespace ZHSystem.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        
        private readonly JwtSettings _jwt;
        public TokenService(IOptions<JwtSettings> settings)
        {
            _jwt = settings.Value;   
        }

        public int AccessTokenExpirationMinutes => _jwt.AccessTokenExpirationMinutes;
        public string GenerateAccessToken(User user, IList<string> roles)
        {

            var claims = new List<Claim>
            {
 
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
        
     
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),

      
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



      

        public RefreshToken CreateRefreshToken(int userId)
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48)),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays),
                IsRevoked = false
            };
        }

        
    }
}

using ConstructionAPI.DAL;
using ConstructionAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ConstructionAPI.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, AppDbContext dbContext, UserManager<User> userManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public (string jwtToken, string refreshToken) GenerateTokens(User user)
        {
            var jwtToken = GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken();

            var tokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.Now.AddDays(30),
                Created = DateTime.UtcNow,
                CreatedByIp = "ipAddresslocalhost",
                ReplacedByToken = jwtToken,
                Revoked= DateTime.UtcNow,
                RevokedByIp = jwtToken,
                IsRevoked=true
            };
            _dbContext.RefreshTokens.Add(tokenEntity);
            _dbContext.SaveChanges();

            return (jwtToken, refreshToken);
        }

        public string RefreshJwtToken(string refreshToken)
        {
            var tokenEntity = _dbContext.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.Now);
            if (tokenEntity == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = _dbContext.Users.Find(tokenEntity.UserId);

            var jwtToken = GenerateJwtToken(user);

            return jwtToken;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtClaimNames.Sub, user.Email),
            new Claim(JwtClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<User> ValidateTokenAndGetUserAsync(string token)
        {
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return null;
            }

            var tokenValue = token.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(tokenValue, validationParameters, out validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return null;
                }

                return await _userManager.FindByEmailAsync(userIdClaim.Value);
            }
            catch
            {
                return null;
            }
        }
    }
}

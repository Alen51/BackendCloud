using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace StudentService
{
    public static class Jwt
    {
        private static readonly string SecretKey = "your_secret_key_here"; // Secret key for signing the token
        private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public static string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, email) // Add username claim
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SigningKey,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken validatedToken);

                var claims = (validatedToken as JwtSecurityToken)?.Claims;
                var indexClaim = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                return indexClaim?.Value ?? ""; // Return username or empty string if not found
            }
            catch (Exception)
            {
                // Token validation failed
                return "";
            }
        }
    }
}

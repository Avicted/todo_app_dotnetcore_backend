/*using System.Security.Claims;
using TodoApp.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Cryptography;

namespace TodoApp.UseCases.Services;


public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        RandomNumberGenerator.Fill(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = DateTime.UtcNow.AddDays(7), // Set expiry date for refresh token
            Created = DateTime.UtcNow
        };
    }

    public string GenerateJwtToken(User user)
    {
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim("id", user.Id) // Include user ID as a claim
              };

            var refreshToken = GenerateRefreshToken().Token;
            claims = claims.Append(new Claim("refreshToken", refreshToken)).ToArray();

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1), // Set to your desired expiration time
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
*/
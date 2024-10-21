using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration config;
    private readonly SymmetricSecurityKey key;

    public TokenService(IConfiguration config)
    {
        this.config = config;
        var signingKey =
            config["JWT:SigningKey"]
            ?? throw new ArgumentNullException("JWT:SigningKey", "Signing key cannot be null");
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }

    public string CreateToken(AppUser user)
    {
        if (user.Email == null)
        {
            throw new ArgumentNullException(nameof(user), "User Email cannot be null");
        }

        if (user.UserName == null)
        {
            throw new ArgumentNullException(nameof(user), "UserName cannot be null");
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = config["JWT:Issuer"],
            Audience = config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

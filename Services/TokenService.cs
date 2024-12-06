using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(User user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access appsettings.");
        if (tokenKey.Length < 64) throw new Exception("Your token key needs to be longer.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims = new List<Claim> 
        {
            new(ClaimTypes.NameIdentifier, user.Name) 
        };
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescritpor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new ClaimsIdentity(claims)),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescritpor);

        return tokenHandler.WriteToken(token);
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using negare_kanban_api.Models;

namespace negare_kanban_api.Services.TokenService
{
  
  public interface ITokenService 
  {
    string Generate(User user);
  }
  
  public class TokenService:ITokenService
  {
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string Generate(User user)
    {
      var privateKey = _configuration.GetValue<string>("JwtSecurityToken:Key");

      if(privateKey == null)
      {
        throw new Exception("private_key_not_founded");
      }
      
      var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
      var tokenDescriptor = new SecurityTokenDescriptor
      {
          Issuer = _configuration["JwtSecurityToken:Issuer"],
          Audience = _configuration["JwtSecurityToken:Audience"],
          // Expires = DateTime.UtcNow.AddMinutes(1),
          Expires = DateTime.UtcNow.AddHours(2),
          SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
          Subject = GenerateClaims(user)
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
      var authClaims = new List<Claim>
      {
          new Claim(ClaimTypes.Name, user.Email),
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

      return new ClaimsIdentity(authClaims);
      
      // // if(user.Roles != null)
      // // {
      // //   foreach (var role in user.Roles)
      // //     ci.AddClaim(new Claim(ClaimTypes.Role, role));
      // // }

      // return ci;
    }
  }
}
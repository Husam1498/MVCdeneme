using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Herkes_Uyurken_Kodlama__NET6.ile_ASP.NET_Core_MVC_Kodlama_2.Helpers
{
    public interface ITokenHelper
    {
        string GenerateToken(string username, int id);
    }

    public class TokenHelper : ITokenHelper
    {

        private readonly IConfiguration _configuration;

        public TokenHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, int id)
        {
            string secret = _configuration.GetValue<string>("AppSettings:Secret");
            byte[] key = Encoding.UTF8.GetBytes(secret);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role,"admin"),//token bilgisine eklenir
                new Claim("username",username),
                new Claim("userid",id.ToString())
            };

            JwtSecurityToken jwtSecurityToken =
                new JwtSecurityToken(signingCredentials: credentials, claims: claims);

            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            return token;
        }
    }
}

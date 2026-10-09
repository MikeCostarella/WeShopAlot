using WeShopAlot.Data.Interfaces;
using WeShopAlot.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WeShopAlot.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        #region Member Variables

        private readonly IConfiguration configuration;

        #endregion Member Variables

        #region Constructors

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion Constructors

        #region Public Methods

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };
            //var keyString = this.configuration["Token:Key"];
            //var keyValue = this.configuration.GetSection("Token:Key").Value!;
            var encodedKey = Encoding.UTF8.GetBytes(this.configuration["Token:Key"]);
            var symmetricSecurityKey = new SymmetricSecurityKey(encodedKey);
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            // The API validates the issuer (Program.cs), so every token must carry it.
            var token = new JwtSecurityToken(
                issuer: this.configuration["Token:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            try
            {
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Public Methods
    }
}
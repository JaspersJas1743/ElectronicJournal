using ElectronicJournal.API.DBModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ElectronicJournal.API.Utilities.Security.JWT
{
    public class JwtProvider : IJwtProvider
    {
        public string Generate(User tokenOwner)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: JwtOptions.Instance.Issuer,
                audience: JwtOptions.Instance.Audience,
                claims: new List<Claim>
                {
                    new Claim(type: ClaimTypes.Name, value: tokenOwner.Login),
                    new Claim(type: ClaimTypes.Role, value: tokenOwner.UserRoleNavigation.Name)
                },
                signingCredentials: new SigningCredentials(
                    key: JwtOptions.Instance.SymmetricKey,
                    algorithm: SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token: jwt);
        }

        public async Task<string> GenerateAsync(User tokenOwner)
            => await Task.Run(function: () => Generate(tokenOwner: tokenOwner));
    }
}

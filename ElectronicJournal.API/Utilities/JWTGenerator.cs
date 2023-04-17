using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ElectronicJournal.API.Utilities
{
	public class JWTGenerator
	{
		public static string Generate(string data)
		{
			Claim[] claims = new Claim[]
			{
				new Claim(type: JwtRegisteredClaimNames.Name, value: data),
				new Claim(type: JwtRegisteredClaimNames.Iss, value: AuthorizationOptions.ISSUER),
				new Claim(type: JwtRegisteredClaimNames.Aud, value: AuthorizationOptions.AUDIENCE)
			};
			JwtSecurityToken jwtToken = new JwtSecurityToken(
				issuer: AuthorizationOptions.ISSUER,
				audience: AuthorizationOptions.AUDIENCE,
				claims: claims,
				signingCredentials: new SigningCredentials(
					key: AuthorizationOptions.SecurityKey,
					algorithm: SecurityAlgorithms.HmacSha256
				)
			);
			string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			return token.Length > 255 ? token.Substring(startIndex: 0, length: 255) : token;
		}
	}
}

using ElectronicJournal.Properties;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ElectronicJournal.Utilities
{
	public static class JWT
	{
		private static string _issuer = Settings.Default.Issuer;
		private static string _audience = Settings.Default.Audience;

		public static string Generate(string dataForGeneration)
		{
			Claim[] claims = new[]
			{
				new Claim(type: JwtRegisteredClaimNames.Name, value: dataForGeneration),
				new Claim(type: JwtRegisteredClaimNames.Iss, value: _issuer),
				new Claim(type: JwtRegisteredClaimNames.Aud, value: _audience)
			};
			SigningCredentials signingCredentials = new SigningCredentials(
				key: new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: Settings.Default.SecurityKey)), 
				algorithm: SecurityAlgorithms.HmacSha256Signature
			);
			JwtSecurityToken token = new JwtSecurityToken(
				issuer: _issuer, audience: _audience, claims: claims, signingCredentials: signingCredentials
			);
			string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token: token);
			return tokenAsString.Length <= 255 ? tokenAsString : tokenAsString.Substring(startIndex: 0, length: 255);
		}
	}
}

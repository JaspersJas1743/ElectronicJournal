using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicJournal.API.Utilities
{
	public static class Confidentiality
	{
		public static string GenerateJWT(string data)
		{
			Claim[] claims = new Claim[1] 
			{ 
				new Claim(ClaimTypes.Name, data) 
			};

			var jwt = new JwtSecurityToken(
				issuer: AuthorizationOptions.ISSUER,
				audience: AuthorizationOptions.AUDIENCE,
				claims: claims,
				signingCredentials: new SigningCredentials(
					key: AuthorizationOptions.SECURITYKEY, 
					algorithm: SecurityAlgorithms.HmacSha256)
				);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		public static async Task<string> GenerateHashAsync(string data)
		{
			string result = String.Empty;
			using (SHA1 hash = SHA1.Create())
			{
				byte[] bytes = await hash.ComputeHashAsync(inputStream: new MemoryStream(buffer: Encoding.UTF8.GetBytes(s: data)));
				result = String.Concat(values: bytes.Select(x => x.ToString("X2")));
			}
			return result;
		}
	}
}

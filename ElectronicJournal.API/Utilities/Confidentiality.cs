using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicJournal.API.Utilities
{
	public static class Confidentiality
	{
		public static async Task<string> GenerateJWT(string data)
		{
			string token = String.Empty;
			await Task.Run(() =>
			{
                var jwt = new JwtSecurityToken(
                    issuer: AuthorizationOptions.ISSUER,
                    audience: AuthorizationOptions.AUDIENCE,
                    claims: new Claim[1] 
					{ 
						new Claim(ClaimTypes.Name, data) 
					},
                    signingCredentials: new SigningCredentials(
                        key: AuthorizationOptions.SECURITYKEY,
                        algorithm: SecurityAlgorithms.HmacSha256)
                    );

                token = new JwtSecurityTokenHandler().WriteToken(jwt);
            });
			return token;
		}

		public static async Task<string> GenerateHashAsync(string data)
		{
			using SHA1 hash = SHA1.Create();
			byte[] bytes = await hash.ComputeHashAsync(inputStream: new MemoryStream(buffer: Encoding.UTF8.GetBytes(s: data)));
			return String.Concat(values: bytes.Select(x => x.ToString("X2")));
		}
	}
}

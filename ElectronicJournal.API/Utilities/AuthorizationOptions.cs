using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ElectronicJournal.API.Utilities
{
	public class AuthorizationOptions
	{
		private const string _secretKey = "Bh8dTEwz3NAl3PgwQaxwZt5iUcQK1Ib7";

		public const string ISSUER = "JaspersJas1743";
		public const string AUDIENCE = "ElectronicJournalUser";

		public static SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: _secretKey));
	}
}

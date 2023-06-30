using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ElectronicJournal.API.Utilities
{
	public static class AuthorizationOptions
	{
		private const string _secretKey = "Bh8dTEwz3NAl3PgwQaxwZt5iUcQK1Ib7";
		public static readonly SymmetricSecurityKey SECURITYKEY = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: _secretKey));
		public const string ISSUER = "JaspersJas1743";
		public const string AUDIENCE = "ElectronicJournalUser";
	}
}

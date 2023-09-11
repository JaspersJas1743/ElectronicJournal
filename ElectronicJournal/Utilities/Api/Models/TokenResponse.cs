using System.Text.Json.Serialization;

namespace ElectronicJournal.Utilities.Api.Models
{
	public class TokenResponse
	{
		public string Token { get; set; }

		public int Id { get; set; }
	}
}
using System.Text.Json.Serialization;

namespace ElectronicJournal.Utilities.Api.Models
{
	public class TokenResponse
	{
		[JsonPropertyName("token")]
		public string Token { get; set; }

		[JsonPropertyName("id")]
		public int Id { get; set; }
	}
}
namespace ElectronicJournal.API.DTOs
{
	public class TokenDTO
	{
		public TokenDTO(string token, int id)
		{
			Token = token;
			ID = id;
		}

		public string Token { get; set; }

		public int ID { get; set; }
	}
}

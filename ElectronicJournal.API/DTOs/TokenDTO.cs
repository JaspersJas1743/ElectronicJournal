namespace ElectronicJournal.API.DTOs
{
	public class TokenDTO
	{
		public TokenDTO(string token, int id)
		{
			Token = token;
			Id = id;
		}

		public string Token { get; set; }

		public int Id { get; set; }
	}
}

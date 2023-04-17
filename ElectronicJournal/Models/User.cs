using System;
using System.Text.Json.Serialization;

namespace ElectronicJournal.Models
{
	public class User
	{
		[JsonPropertyName(name: "surname")]
		public string Surname { get; set; }

		[JsonPropertyName(name: "name")]
		public string Name { get; set; }

		[JsonPropertyName(name: "patronymic")]
		public string Patronymic { get; set; }

		[JsonPropertyName(name: "role")]
		public string Role { get; set; }

		[JsonPropertyName(name: "sex")]
		public string Sex { get; set; }

		[JsonPropertyName(name: "birthday")]
		public DateTime Birthday { get; set; }

		[JsonPropertyName(name: "phone")]
		public string Phone { get; set; }

		[JsonPropertyName(name: "email")]
		public string Email { get; set; }

		[JsonPropertyName(name: "photo")]
		public string Photo { get; set; }
	}
}

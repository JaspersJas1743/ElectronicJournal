using System;
using System.Text.Json.Serialization;

namespace ElectronicJournal.Models.ForAPI
{
	public class User
	{
		public string Surname { get; set; }

		public string Name { get; set; }

		public string Patronymic { get; set; }

		public string Role { get; set; }

		public string Sex { get; set; }

		public DateTime Birthday { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string Photo { get; set; }
	}
}

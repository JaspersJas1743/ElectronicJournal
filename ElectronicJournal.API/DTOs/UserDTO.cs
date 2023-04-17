using ElectronicJournal.API.Models;

namespace ElectronicJournal.API.DTOs
{
	public class UserDTO
	{
		public string Surname { get; set; } = null!;

		public string Name { get; set; } = null!;

		public string? Patronymic { get; set; }

		public string Role { get; set; } = null!;

		public string Sex { get; set; } = null!;

		public DateTime Birthday { get; set; }

		public string? Phone { get; set; }

		public string? Email { get; set; }

		public string? Photo { get; set; }

		public static UserDTO Copy(User user)
		{
			return new UserDTO()
			{
				Name = user.Name,
				Surname = user.Surname,
				Patronymic = user.Patronymic,
				Birthday = user.Birthday,
				Email = user.Email,
				Phone = user.Phone,
				Photo = user.Photo,
				Role = user.Role,
				Sex = user.Sex
			};
		}
	}
}

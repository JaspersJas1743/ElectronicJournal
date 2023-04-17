using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly ElectronicJournalContext _context;
		private readonly ILogger<UserController> _logger;

		public UserController(ElectronicJournalContext context, ILogger<UserController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet(template: "GetIfExist")]
		public async Task<ActionResult<UserDTO>> GetIfExist(string login, string password)
		{
			string token = JWTGenerator.Generate(data: $"{login};{password}");
			User user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey.Equals(token));
			if (user is null)
				return NotFound(value: new { Message = "Некорректные данные пользователя" });

			return UserDTO.Copy(user: user);
		}
	}
}

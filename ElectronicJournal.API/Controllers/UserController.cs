using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Models;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

		[HttpGet(template: "GetToken/{login}")]
		public async Task<ActionResult<string>> GetToken(string login)
		{
			string token = Confidentiality.GenerateJWT(data: login);
			User _user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey.Equals(token));
			if (_user is null)
				return NotFound(value: new Error(message: "Некорректные данные пользователя", propertyName: "Login"));

			return Ok(token);
		}

		[Authorize]
		[HttpGet(template: "CheckPassword/{password}")]
		public async Task<ActionResult<UserDTO>> Check(string password)
		{
			string hashedPassword = await Confidentiality.GenerateHashAsync(data: password);
			string token = await HttpContext.GetTokenAsync("access_token");
			User user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey.Equals(token));

			if (!user.Password.Equals(hashedPassword))
				return NotFound(value: new Error(message: "Пароль не соответствует пользователю", propertyName: "Password"));

			return Ok(UserDTO.Copy(user: user));
		}
	}
}

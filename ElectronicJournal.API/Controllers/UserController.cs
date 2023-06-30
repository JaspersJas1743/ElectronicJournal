using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.DBModels;
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
		public async Task<ActionResult<TokenDTO>> GetToken(string login)
		{
			string token = Confidentiality.GenerateJWT(data: login);
			User user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey.Equals(token));
			if (user is null)
				return NotFound(value: new Error { Message = "Неправильный логин и/или пароль" });

			return Ok(value: new TokenDTO(token: token));
		}

		[Authorize]
		[HttpGet(template: "CheckPassword/{password}")]
		public async Task<ActionResult<UserDTO>> Check(string password)
		{
			string hashedPassword = await Confidentiality.GenerateHashAsync(data: password);
			string token = await HttpContext.GetTokenAsync("access_token");
			User user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey.Equals(token));

			if (user is null || !user.Password.Equals(hashedPassword))
				return NotFound(value: new Error { Message = "Неправильный логин и/или пароль"});

			return Ok(value: UserDTO.Copy(user: user));
		}
	}
}

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
			User? user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey == token);
			if (user is null)
				return NotFound(value: new Error { Message = "Неправильный логин и/или пароль" });

			return Ok(value: new TokenDTO(token: token, id: user.Id));
		}

		[Authorize]
		[HttpGet(template: "GetUserIfPasswordExist/{password}")]
		public async Task<ActionResult<UserDTO>> GetUserIfPasswordExist(string password)
		{
			string hashedPassword = await Confidentiality.GenerateHashAsync(data: password);
			int id = Int32.Parse(s: HttpContext.Request.Headers["UserId"]);
			User? user = await _context.Users.FindAsync(keyValues: id);
			if (user is null || !user.Password.Equals(hashedPassword))
				return NotFound(value: new Error { Message = "Неправильный логин и/или пароль" });

			return Ok(value: UserDTO.Copy(user: user));
		}

		[HttpGet(template: "CheckRegistrationCode/{code}")]
		public async Task<ActionResult<int>> CheckRegistrationCode(string code)
		{
			User? user = await _context.Users.FirstOrDefaultAsync(predicate: x => x.RegistrationCode == code);
			if (user is null)
				return NotFound(value: new Error { Message = "Проверьте правильность ввода или обратитесь в организацию" });

			return Ok(value: new TokenDTO(token: null, id: user.Id));
		}
	}
}

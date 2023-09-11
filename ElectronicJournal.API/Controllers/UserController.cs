using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.DTOs;
using ElectronicJournal.API.Utilities;
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

		[Authorize]
		[HttpGet(template: "GetUserById/{id}")]
		public async Task<ActionResult<UserDTO>> GetUserById(int id)
		{
			User? user = await _context.Users.FindAsync(keyValues: id);
			if (user is null)
				return NotFound(value: new Error { Message = "Пользователь с заданным идентификатором не найден" });

			return Ok(value: UserDTO.Copy(user: user));
		}

		[HttpGet(template: "GetToken/{login}")]
		public async Task<ActionResult<TokenDTO>> GetToken(string login)
		{
			string token = Confidentiality.GenerateJWT(data: login);
			User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.AuthKey == token);
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

			return CreatedAtAction(actionName: nameof(GetUserById), routeValues: new { id = id }, value: user);
		}

		[HttpGet(template: "GetUserIdByRegistrationCode/{code}")]
		public async Task<ActionResult<int>> GetUserIdByRegistrationCode(string code)
		{
			User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.RegistrationCode == code);
			if (user is null)
				return NotFound(value: new Error { Message = "Проверьте правильность ввода или обратитесь в организацию" });

			return Ok(value: user.Id);
		}

		[HttpGet(template: "GetAvailabilityForLogin/{login}")]
		public async Task<ActionResult<bool>> GetAvailabilityForLogin(string login)
		{
			string token = Confidentiality.GenerateJWT(data: login);
			return Ok(value: await _context.Users.FirstOrDefaultAsync(predicate: u => u.AuthKey == token) is null);
		}

		[HttpPost(template: "Registration")]
		public async Task<ActionResult> Registration([FromBody] AuthDataDTO authData)
		{
			int id = Int32.Parse(s: HttpContext.Request.Headers["UserId"]);
			string token = Confidentiality.GenerateJWT(data: authData.Login);
			string hashedPassword = await Confidentiality.GenerateHashAsync(data: authData.Password);
			User? user = await _context.Users.FindAsync(keyValues: id);

			_context.Entry(entity: user).State = EntityState.Modified;
			user.RegistrationCode = null;
			user.AuthKey = token;
			user.Password = hashedPassword;

			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}

using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using ElectronicJournal.API.Utilities.Security.Hash;
using ElectronicJournal.API.Utilities.Security.JWT;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        #region Fields
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IHashProvider _hashProvider;
        private readonly IJwtProvider _jwtProvider;
        #endregion Fields

        #region Constructors
        public AccountController(ElectronicJournalContext context, ILogger<UserController> logger, IHashProvider hashProvider, IJwtProvider jwtProvider)
        {
            _context = context;
            _logger = logger;
            _hashProvider = hashProvider;
            _jwtProvider = jwtProvider;
        }
        #endregion Constructors

        #region Records
        public record LogInRequest(string Login, string Password);
        public record LogInResponse(string Token);
        public record RegistrationCodeRequest(string RegistrationCode);
        public record RegistrationCodeResponse(bool IsVerified);
        public record SignUpRequest(string RegistrationCode, string Login, string Password);
        #endregion Records

        #region Methods
        #region Private
        private async Task<User?> FindUserByLoginAsync(string login)
        {
            List<User> users = await _context.Users
                .Include(navigationPropertyPath: u => u.UserRoleNavigation)
                .Include(navigationPropertyPath: u => u.GenderNavigation)
                .Where(predicate: u => !String.IsNullOrEmpty(u.Login))
                .ToListAsync();
            return users.FirstOrDefault(predicate: u => _hashProvider.VerifyHash(toHash: login, hashedData: u.Login));
        }

        private async Task<bool> VerifyRegistrationCode(string registrationCode)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(
                predicate: u => u.RegistrationCode!.Equals(registrationCode)
            );
            return user?.RegistrationCode == registrationCode;
        }
        #endregion Private

        #region GET
        [HttpGet(template: nameof(VerifyRegistrationCode))]
        public async Task<ActionResult<RegistrationCodeResponse>> VerifyRegistrationCode([FromQuery] RegistrationCodeRequest data)
            => Ok(value: new RegistrationCodeResponse(IsVerified: await VerifyRegistrationCode(registrationCode: data.RegistrationCode)));
        #endregion GET

        #region POST
        [HttpPost(template: nameof(SignIn))]
        public async Task<ActionResult<LogInResponse>> SignIn([FromBody] LogInRequest data)
        {
            User? user = await FindUserByLoginAsync(login: data.Login);
            if (user is null || !await _hashProvider.VerifyHashAsync(toHash: data.Password, hashedData: user.Password))
                return NotFound(value: new Error { Message = "Неверные авторизационные данные :(" });

            return Ok(value: new LogInResponse(Token: _jwtProvider.Generate(tokenOwner: user)));
        }

        [HttpPost(template: nameof(SignUp))]
        public async Task<ActionResult> SignUp([FromBody] SignUpRequest data)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.RegistrationCode == data.RegistrationCode);
            if (user is null)
                return NotFound(value: new Error { Message = "Указан неверный регистрационный код" });

            if (await FindUserByLoginAsync(login: data.Login) is not null)
                return BadRequest(error: new Error { Message = "Данный логин не может быть занят" });

            _context.Entry(entity: user).State = EntityState.Modified;
            user.RegistrationCode = null;
            user.Login = await _hashProvider.GenerateHashAsync(toHash: data.Login);
            user.Password = await _hashProvider.GenerateHashAsync(toHash: data.Password);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"[SignUp.SaveChangesAsync]: {ex.Message}");
            }
            return Ok();
        }
        #endregion POST
        #endregion Methods
    }
}

using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using ElectronicJournal.API.Validators;
using FluentValidation;
using FluentValidation.Results;
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
        #endregion Fields

        #region Constructors
        public AccountController(ElectronicJournalContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion Constructors

        #region Records
        public record LogInData(string Login, string Password);
        public record LogInResponse(string Token);
        public record RegistrationCodeRequest(string RegistrationCode);
        public record RegistrationCodeResponse(bool IsVerified);
        public record SignUpData(string RegistrationCode, string Login, string Password);
        #endregion Records

        #region Methods
        #region Get
        [HttpGet(template: "VerifyRegistrationCode")]
        public async Task<ActionResult<RegistrationCodeResponse>> VerifyRegistrationCode([FromQuery] RegistrationCodeRequest data)
        {
            ValidationResult validationResult = new AccountRegistrationCodeRequestValidator().Validate(data);
            if (!validationResult.IsValid)
                return BadRequest(error: new Error { Message = validationResult.Errors.First().ErrorMessage });

            User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.RegistrationCode == data.RegistrationCode);
            return Ok(value: new RegistrationCodeResponse(IsVerified: user is not null));
        }
        #endregion Get

        #region Post
        [HttpPost(template: "SignIn")]
        public async Task<ActionResult<LogInResponse>> SignIn([FromBody] LogInData data)
        {
            ValidationResult validationResult = new AccountLogInDataValidator().Validate(data);
            if (!validationResult.IsValid)
                return BadRequest(error: new Error { Message = validationResult.Errors.First().ErrorMessage });

            string token = await Confidentiality.GenerateJWT(data: data.Login);
            User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.AuthKey.Equals(token));
            string hashedPassword = await Confidentiality.GenerateHashAsync(data: data.Password);
            if (user is null || !user.Password.Equals(hashedPassword))
                return NotFound(value: new Error { Message = "Неверные авторизационные данные :(" });

            return Ok(value: new LogInResponse(Token: token));
        }

        [HttpPost(template: "SignUp")]
        public async Task<ActionResult> SignUp([FromBody] SignUpData data)
        {
            ValidationResult validationResult = new AccountSignUpDataValidator().Validate(data);
            if (!validationResult.IsValid)
                return BadRequest(error: new Error { Message = validationResult.Errors.First().ErrorMessage });

            string token = await Confidentiality.GenerateJWT(data: data.Login);
            string hashedPassword = await Confidentiality.GenerateHashAsync(data: data.Password);

            if (await _context.Users.FirstOrDefaultAsync(predicate: u => u.AuthKey.Equals(token)) is not null)
                return BadRequest(error: new Error { Message = "Данный логин не может быть занят" });

            User? user = await _context.Users.FirstOrDefaultAsync(predicate: u => u.RegistrationCode.Equals(data.RegistrationCode));
            if (user is null)
                return BadRequest(error: new Error { Message = "Указан неверный регистрационный код" });

            _context.Entry(entity: user).State = EntityState.Modified;
            user.RegistrationCode = null;
            user.AuthKey = token;
            user.Password = hashedPassword;

            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion Post
        #endregion Methods
    }
}

using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicJournal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;
        #endregion Fields

        #region Constructor
        public UserController(ElectronicJournalContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion Constructor

        #region Records
        public record InfoResponse(int Id, string Surname, string Name, string? Patronymic, string Role, string Gender, DateTime Birthday, string Phone, string Email);
        #endregion Records

        #region Methods
        #region GET
        [Authorize]
        [HttpGet(template: nameof(DownloadProfilePhoto))]
        public async Task<ActionResult> DownloadProfilePhoto()
        {
            int id = Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));
            User? user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound(value: new Error { Message = "Ошибка на стороне сервера. Приносим свои извнения!" });

            if (String.IsNullOrEmpty(user.Photo))
                return NotFound(value: new Error { Message = "Фотография пользователя не установлена" });

            FileInfo photo = new FileInfo(fileName: Environment.CurrentDirectory + user.Photo);
            if (!photo.Exists)
                return NotFound(value: new Error { Message = "Фотография пользователя не установлена" });

            byte[] content = await System.IO.File.ReadAllBytesAsync(path: photo.FullName);
            return File(fileContents: content, contentType: $"image/{photo.Extension.Trim(trimChar: '.')}");
        }

        [Authorize]
        [HttpGet(template: nameof(GetInfo))]
        public async Task<ActionResult<InfoResponse>> GetInfo()
        {
            int id = Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));
            User? user = await _context.Users.FindAsync(id);
            if (user is null)
                return NotFound(value: new Error { Message = "Ошибка на стороне сервера. Приносим свои извинения!" });

            await _context.Entry(entity: user).Reference(propertyExpression: u => u.GenderNavigation).LoadAsync();
            await _context.Entry(entity: user).Reference(propertyExpression: u => u.UserRoleNavigation).LoadAsync();

            return Ok(value: new InfoResponse(Id: user.Id, Surname: user.Surname, Name: user.Name, Patronymic: user.Patronymic, Role: user.UserRoleNavigation.Name,
                                              Gender: user.GenderNavigation.Name, Birthday: user.Birthday, Phone: user.Phone, Email: user.Email));
        }
        #endregion GET
        #region POST
        #endregion POST
        #endregion Methods
    }
}

using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using ElectronicJournal.API.Utilities.Security.Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectronicJournal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IHashProvider _hashProvider;
        private readonly IHostEnvironment _hostEnvironment;
        #endregion Fields

        #region Constructor
        public UserController(ElectronicJournalContext context, ILogger<UserController> logger, IHashProvider hashProvider, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _logger = logger;
            _hashProvider = hashProvider;
            _hostEnvironment = hostEnvironment;
        }
        #endregion Constructor

        #region Records
        public record InfoResponse(int Id, string Surname, string Name, string? Patronymic, string Role, string Gender, DateTime Birthday, string Phone, string Email);
        public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
        public record ChangeEmailRequest(string NewEmail);
        public record ChangePhoneRequest(string NewPhone);
        public record ChangeResponse(bool IsSuccess, string Message);
        #endregion Records

        #region Methods
        #region GET
        [HttpGet(template: nameof(GetInfo))]
        public async Task<ActionResult<InfoResponse>> GetInfo()
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            await _context.Entry(entity: user).Reference(propertyExpression: u => u.GenderNavigation).LoadAsync();
            await _context.Entry(entity: user).Reference(propertyExpression: u => u.UserRoleNavigation).LoadAsync();

            return Ok(value: new InfoResponse(Id: user.Id, Surname: user.Surname, Name: user.Name, Patronymic: user.Patronymic, Role: user.UserRoleNavigation.Name,
                                              Gender: user.GenderNavigation.Name, Birthday: user.Birthday, Phone: user.Phone, Email: user.Email));
        }

        [HttpGet(template: nameof(DownloadProfilePhoto))]
        public async Task<ActionResult> DownloadProfilePhoto()
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            if (String.IsNullOrEmpty(user?.Photo))
                return NotFound(value: new Error { Message = "Фотография пользователя не установлена" });

            FileInfo photo = new FileInfo(fileName: user.Photo);
            if (!photo.Exists)
                return NotFound(value: new Error { Message = "Фотография пользователя не установлена" });

            byte[] content = await System.IO.File.ReadAllBytesAsync(path: photo.FullName);
            return File(fileContents: content, contentType: $"image/{photo.Extension.Trim(trimChar: '.')}");
        }
        #endregion GET

        #region POST
        [HttpPost(template: nameof(UploadProfilePhoto))]
        public async Task<ActionResult> UploadProfilePhoto(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest(error: new Error { Message = "Файл поврежден или пуст" });

            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));
            if (user.Photo != null)
                return BadRequest(error: new Error { Message = "Фото профиля уже установлено" });

            string folder = Path.Combine(path1: "Resources", path2: "Images", path3: "ProfilePhoto");
            DirectoryInfo directory = new DirectoryInfo(path: folder);
            if (!directory.Exists)
                directory.Create();

            string fileName = $"{user.Id}.{file.FileName.Split(separator: '.').Last()}";
            string filePath = Path.Combine(path1: directory.FullName, path2: fileName);
            using (FileStream stream = new FileStream(path: filePath, mode: FileMode.Create))
                await file.CopyToAsync(target: stream);

            _context.Entry(entity: user).State = EntityState.Modified;
            user.Photo = Path.Combine(path1: folder, path2: fileName);

            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion POST

        #region PUT
        [HttpPut(template: nameof(ChangePassword))]
        public async Task<ActionResult<ChangeResponse>> ChangePassword([FromBody] ChangePasswordRequest data)
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            if (_hashProvider.VerifyHash(toHash: data.NewPassword, hashedData: user?.Password))
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Указанный пароль уже установлен"));

            if (!_hashProvider.VerifyHash(toHash: data.CurrentPassword, hashedData: user?.Password))
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Значение текущего пароля неверно"));

            _context.Entry(entity: user).State = EntityState.Modified;
            user.Password = await _hashProvider.GenerateHashAsync(toHash: data.NewPassword);

            await _context.SaveChangesAsync();

            return Ok(value: new ChangeResponse(IsSuccess: true, Message: "Пароль успешно изменен!"));
        }

        [HttpPut(template: nameof(ChangeEmail))]
        public async Task<ActionResult<ChangeResponse>> ChangeEmail([FromBody] ChangeEmailRequest data)
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            if (user.Email == data.NewEmail)
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Указаный адрес электронной почты уже установлен"));

            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == data.NewEmail) is not null)
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Данный адрес электронной почты занят другим пользователем"));

            _context.Entry(entity: user).State = EntityState.Modified;
            user.Email = data.NewEmail;

            await _context.SaveChangesAsync();

            return Ok(value: new ChangeResponse(IsSuccess: true, Message: "Адрес электронной почты успешно изменен!"));
        }

        [HttpPut(template: nameof(ChangePhone))]
        public async Task<ActionResult<ChangeResponse>> ChangePhone([FromBody] ChangePhoneRequest data)
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            if (user.Phone == data.NewPhone)
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Указаный номер телефона уже установлен"));

            if (await _context.Users.FirstOrDefaultAsync(u => u.Phone == data.NewPhone) is not null)
                return Ok(value: new ChangeResponse(IsSuccess: false, Message: "Данный номер телефона занят другим пользователем"));

            _context.Entry(entity: user).State = EntityState.Modified;
            user.Phone = data.NewPhone;
            await _context.SaveChangesAsync();
            return Ok(value: new ChangeResponse(IsSuccess: true, Message: "Номер телефона успешно изменен!"));
        }
        #endregion PUT

        #region DELETE
        [HttpDelete(template: nameof(DeleteProfilePhoto))]
        public async Task<ActionResult> DeleteProfilePhoto()
        {
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            if (user.Photo == null)
                return BadRequest(error: new Error { Message = "Фото профиля не установлено!" });

            FileInfo file = new FileInfo(fileName: user.Photo);
            file.Delete();

            _context.Entry(entity: user).State = EntityState.Modified;
            user.Photo = null;

            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion DELETE

        #endregion Methods
    }
}

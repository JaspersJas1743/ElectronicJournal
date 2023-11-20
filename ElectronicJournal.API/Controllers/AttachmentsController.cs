using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace ElectronicJournal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentsController : ControllerBase
    {
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<MessagesController> _logger;
        public AttachmentsController(ElectronicJournalContext context, ILogger<MessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public record DownloadAttachmentRequest(int Id);
        public record UploadAttachmentResponse(int Id);

        [HttpGet(template: nameof(DownloadAttachment))]
        public async Task<ActionResult> DownloadAttachment([FromQuery] DownloadAttachmentRequest data)
        {   
            Attachment attachment = await _context.Attachments.FindAsync(keyValues: data.Id);

            if (attachment is null)
                return BadRequest(error: new Error { Message = "Некорректный идентификатор вложения" });

            FileInfo file = new FileInfo(fileName: attachment.Path);
            if (!file.Exists)
                return NotFound(value: new Error { Message = "Не удалось найти запрошенный файл!" });

            _logger.LogInformation($"Отправка файла {file.Name}");
            byte[] content = await System.IO.File.ReadAllBytesAsync(path: file.FullName);

            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileNameStar = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(file.Name))
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(fileContents: content, contentType: "application/zip");
        }

        [HttpPost(template: nameof(UploadAttachment))]
        public async Task<ActionResult<UploadAttachmentResponse>> UploadAttachment(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest(error: new Error { Message = "Допустимый размер загружаемого файла - 30Мбайт" });

            Attachment attachment = new Attachment { Path = await FileUploader.Upload(file: file) };
            _context.Attachments.Add(entity: attachment);
            await _context.SaveChangesAsync();
            return Ok(value: new UploadAttachmentResponse(Id: attachment.Id));
        }
    }
}

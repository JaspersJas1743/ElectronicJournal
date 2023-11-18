using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectronicJournal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        #region Fields
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<MessagesController> _logger;
        #endregion Fields

        #region Constructor
        public MessagesController(ElectronicJournalContext context, ILogger<MessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion Constructor

        #region Records
        public record MessageReceiversRequest(string Filter);
        public record MessageReceiversResponse(int Id, string DisplayedName);
        public record Attachment(int Id, string FileName, string Path);
        public record GetMessagesResponse(string Header, string Sender, int SenderId, string Receiver, DateTime Date, bool HaveText, string Text, bool HaveAttachment, Attachment Attachment);
        public record GetMessagesRequest(bool IsFiltered, int UserId, int Offset, int Count);
        public record SendMessageRequest(int ReceiverId, string Text, int? AttachmentId);
        public record SendMessageResponse(string Message);
        #endregion Records

        #region Methods
        #region GET
        [HttpGet(template: nameof(GetMessageReceivers))]
        public async Task<IEnumerable<MessageReceiversResponse>> GetMessageReceivers([FromQuery] MessageReceiversRequest data)
        {
            return _context.Users.AsNoTracking()
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation)
                .Include(navigationPropertyPath: u => u.Teacher)
                .Include(navigationPropertyPath: u => u.Admin.RoleNavigation)
                .Include(navigationPropertyPath: u => u.Parent).AsEnumerable()
                .Where(predicate: u => $"{u.Surname} {u.Name} {u.Patronymic}".ToLower().Contains(data.Filter.ToLower()))
                .Select(u => new MessageReceiversResponse(u.Id, GetSenderString(u)));
        }


        [HttpGet(template: nameof(GetInboundMessages))]
        public async Task<ActionResult<IEnumerable<GetMessagesResponse>>> GetInboundMessages([FromQuery] GetMessagesRequest data)
        {
            _logger.LogInformation(message: $"outbound: offset={data.Offset};count={data.Count};userId={data.UserId};isfiltered={data.IsFiltered}");
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            IQueryable<Message> result = SetAllIncludes().Where(predicate: m => m.Receivers.Any(u => u.Id == user.Id));

            if (data.IsFiltered)
                result = result.Where(predicate: m => m.SenderNavigation.Id == data.UserId);

            return Ok(value: GetMessages(data: data, result: result));
        }

        [HttpGet(template: nameof(GetOutboundMessages))]
        public async Task<ActionResult<IEnumerable<GetMessagesResponse>>> GetOutboundMessages([FromQuery] GetMessagesRequest data)
        {
            _logger.LogInformation(message: $"inbound: offset={data.Offset};count={data.Count}");
            User user = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));

            IQueryable<Message> result = SetAllIncludes().Where(predicate: m => m.SenderNavigation.Id == user.Id);

            if (data.IsFiltered)
                result = result.Where(predicate: m => m.Receivers.Any(u => u.Id == data.UserId));

            return Ok(value: GetMessages(data: data, result: result));
        }
        #endregion GET

        #region POST
        [HttpPost(template: nameof(SendMessage))]
        public async Task<ActionResult<SendMessageResponse>> SendMessage([FromBody] SendMessageRequest data)
        {
            User sender = await _context.Users.FindAsync(keyValues: Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier)));
            User receiver = await _context.Users.FindAsync(keyValues: data.ReceiverId);
            if (receiver is null)
                return BadRequest(error: new Error { Message = "Некорректный идентификатор получателя" });

            Message message = new Message
            {
                Sender = sender.Id,
                Text = data.Text,
                SendDatetime = DateTime.Now,
                Attachment = data.AttachmentId,
                Receivers = new User[] { receiver }
            };

            _context.Messages.Add(entity: message);
            await _context.SaveChangesAsync();
            return Ok(value: new SendMessageResponse(Message: "Сообщение успешно отправлено!"));
        }
        #endregion POST

        private IQueryable<Message> SetAllIncludes()
        {
            return _context.Messages.AsSplitQuery().AsNoTracking()
                .Include(navigationPropertyPath: m => m.AttachmentNavigation)
                .Include(navigationPropertyPath: m => m.Receivers).ThenInclude(navigationPropertyPath: u => u.Student.GroupNavigation)
                .Include(navigationPropertyPath: m => m.Receivers).ThenInclude(navigationPropertyPath: u => u.Teacher)
                .Include(navigationPropertyPath: m => m.Receivers).ThenInclude(navigationPropertyPath: u => u.Admin.RoleNavigation)
                .Include(navigationPropertyPath: m => m.Receivers).ThenInclude(navigationPropertyPath: u => u.Parent)
                .Include(navigationPropertyPath: m => m.SenderNavigation.Student.GroupNavigation)
                .Include(navigationPropertyPath: m => m.SenderNavigation.Teacher)
                .Include(navigationPropertyPath: m => m.SenderNavigation.Admin.RoleNavigation)
                .Include(navigationPropertyPath: m => m.SenderNavigation.Parent);
        }

        private IEnumerable<GetMessagesResponse> GetMessages(GetMessagesRequest data, IQueryable<Message> result)
        {
            return result.OrderByDescending(keySelector: m => m.SendDatetime)
                .Skip(count: data.Offset)
                .Take(count: data.Count)
                .AsEnumerable()
                .Select(selector: m =>
                {
                    string sender = GetSenderString(sender: m.SenderNavigation);
                    string receiver = GetReceiverString(receivers: m.Receivers);
                    bool haveText = !String.IsNullOrEmpty(m.Text) || !String.IsNullOrWhiteSpace(m.Text);
                    bool haveAttachment = m.AttachmentNavigation != null;
                    Attachment attachment = null;
                    if (haveAttachment)
                    {
                        FileInfo attachmentFile = new FileInfo(m.AttachmentNavigation.Path);
                        attachment = new Attachment(m.AttachmentNavigation.Id, attachmentFile.Name, m.AttachmentNavigation.Path);
                    }

                    string header = haveText ? m.Text : (haveAttachment ? $"[{attachment.FileName}]" : "[Пустое сообщение]");

                    return new GetMessagesResponse(header, sender, m.SenderNavigation.Id, receiver, m.SendDatetime, haveText, m.Text, haveAttachment, attachment);
                });
        }

        private string GetSenderString(User sender)
            => GetUserString(user: sender);

        private string GetReceiverString(IEnumerable<User> receivers)
            => String.Join("; ", receivers.Select(selector: u => GetUserString(user: u)));

        private string GetUserString(User user)
        {
            string sender = $"{user.Surname} {user.Name[0]}. {user?.Patronymic?[0]}., ";

            if (user.Student is not null)
                return sender + $"студент {user.Student.GroupNavigation.Name}";

            if (user.Teacher is not null)
                return sender + "преподаватель";

            if (user.Admin is not null)
                return sender + user.Admin.RoleNavigation.Name;

            return sender + "родитель";
        }
        #endregion Methods
    }
}

using ElectronicJournal.API.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectronicJournal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeworkController : ControllerBase
    {
        #region Fields
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<HomeworkController> _logger;
        #endregion Fields

        #region Constructor
        public HomeworkController(ElectronicJournalContext context, ILogger<HomeworkController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion Constructor

        #region Records
        public record HomeworkAttachment(int Id, string FileName, string Path);
        public record GetHomeworkResponse(string Lesson, string Text, DateTime CompletionDate, string Teacher, HomeworkAttachment Attachment);
        #endregion Records

        #region Methods
        #region GET
        [HttpGet(template: nameof(GetHomeworks))]
        public async Task<ActionResult<IEnumerable<GetHomeworkResponse>>> GetHomeworks()
        {
            User? user = await _context.Users.AsNoTracking().AsSplitQuery()
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation.Homeworks)
                    .ThenInclude(navigationPropertyPath: h => h.AttachmentNavigation)
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation.Homeworks)
                    .ThenInclude(navigationPropertyPath: h => h.LessonNavigation.Teachers)
                        .ThenInclude(navigationPropertyPath: t => t.UserNavigation)
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation)
                    .ThenInclude(navigationPropertyPath: g => g.Teachers)
                        .ThenInclude(navigationPropertyPath: t => t.UserNavigation)
                .FirstOrDefaultAsync(u => u.Id == Int32.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var homeworks = user?.Student?.GroupNavigation?.Homeworks
                .Where(predicate: h => h.CompletionDate > DateTime.Now)
                .OrderBy(keySelector: h => h.CompletionDate);
            return Ok(value: homeworks?.Select(h =>
            {
                HomeworkAttachment? attachment = null;
                if (h.AttachmentNavigation != null)
                    attachment = new HomeworkAttachment(h.AttachmentNavigation.Id, new FileInfo(h.AttachmentNavigation.Path).Name, h.AttachmentNavigation.Path);

                IEnumerable<Teacher>? teachers = h.LessonNavigation?.Teachers?.IntersectBy(h.GroupNavigation.Teachers.Select(selector: t => t.User), keySelector: t => t.User);
                string? teachersString = String.Join(separator: ", ", values: teachers?.Select(selector: t => $"{t.UserNavigation.Surname} {t.UserNavigation.Name[0]}. {t.UserNavigation.Patronymic[0]}."));

                return new GetHomeworkResponse(
                    Lesson: h.LessonNavigation.Name,
                    Text: h.Text,
                    CompletionDate: h.CompletionDate,
                    Teacher: teachersString,
                    Attachment: attachment
                );
            }) ?? Enumerable.Empty<GetHomeworkResponse>());
        }
        #endregion GET
        #endregion Methods
    }
}

using ElectronicJournal.API.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ElectronicJournal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route(template: "api/[controller]")]
    public class MarksController : ControllerBase
    {
        private ElectronicJournalContext _content;
        private ILogger<MarksController> _logger;

        public MarksController(ElectronicJournalContext content, ILogger<MarksController> logger)
        {
            _content = content;
            _logger = logger;
        }

        public record Mark(string Value, string Description);
        public record GetMarksReponse(double Average, IEnumerable<Mark> Marks);
        public record GetMarksRequest(int LessonId);

        [HttpGet(template: nameof(GetMarks))]
        public async Task<ActionResult<GetMarksReponse>> GetMarks([FromQuery] GetMarksRequest data)
        {
            int id = Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));
            _logger.LogInformation($"lessonId: {data.LessonId}");
            User user = _content.Users.AsNoTracking()
                .Include(navigationPropertyPath: u => u.Student.Marks)
                    .ThenInclude(navigationPropertyPath: m => m.Mark1Navigation)
                .First(predicate: u => u.Id == id);

            IEnumerable<DBModels.Mark> marks = user.Student.Marks?.Where(predicate: m => m?.Lesson == data.LessonId);
            if (marks.Count() == 0)
                return Ok(new GetMarksReponse(Average: 0, Marks: Enumerable.Empty<Mark>()));

            return Ok(value: new GetMarksReponse(
                Average: marks.Where(m => m.Value < 6).Average(selector: m => m.Value).Value, 
                Marks: marks?.Select(selector: m => new Mark(Value: m.Mark1Navigation.Value, Description: m.Description))
            ));
        }
    }
}

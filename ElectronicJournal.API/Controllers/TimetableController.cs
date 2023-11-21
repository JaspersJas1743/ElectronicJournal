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
    public class TimetableController : ControllerBase
    {
        private ElectronicJournalContext _context;
        private ILogger<TimetableController> _logger;

        public TimetableController(ElectronicJournalContext context, ILogger<TimetableController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public record Lesson(string Name, DateTime Day, TimeSpan Start, TimeSpan End, string Teacher, string Auditorium);
        public record GetTimetableRequest(int? CountLesson, DateTime? Date, TimeSpan? StartTime, TimeSpan? EndTime, IEnumerable<Lesson>? Lessons);
        public record GetTimetableResponse(DateTime StartDate, DateTime EndDate);

        [HttpGet(template: nameof(GetTimetable))]
        public async Task<ActionResult<IEnumerable<GetTimetableRequest>>> GetTimetable([FromQuery] GetTimetableResponse data)
        {
            _logger.LogInformation($"GetTimetable: start={data.StartDate};end={data.EndDate}");
            int id = Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));
            User user = await _context.Users.AsSplitQuery()
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation.CoupleTimings)
                    .ThenInclude(navigationPropertyPath: ct => ct.LessonNavigation.Teachers)
                        .ThenInclude(navigationPropertyPath: t => t.UserNavigation)
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation.Teachers)
                    .ThenInclude(navigationPropertyPath: t => t.UserNavigation)
                .FirstOrDefaultAsync(predicate: u => u.Id == id);

            IEnumerable<Lesson> lessons = user.Student.GroupNavigation.CoupleTimings
                .Where(predicate: ct => ct.Date >= data.StartDate && ct.Date <= data.EndDate)
                .OrderBy(keySelector: ct => ct.Date).ThenBy(keySelector: ct => ct.Start)
                .Select(selector: ct =>
                {
                    string teachers = String.Join(separator: ", ",
                        values: ct.LessonNavigation.Teachers.Intersect(second: ct.GroupNavigation.Teachers)
                            .Select(t => $"{t.UserNavigation.Surname} {t.UserNavigation.Name} {t.UserNavigation.Patronymic}")
                    );

                    return new Lesson(
                        Name: ct.LessonNavigation.Name,
                        Day: ct.Date,
                        Start: ct.Start,
                        End: ct.End,
                        Teacher: teachers,
                        Auditorium: Char.IsNumber(c: ct.Auditorium[0]) ? $"Кабинет {ct.Auditorium}" : ct.Auditorium);
                });

            return Ok(value: Enumerable.Range(
                start: 0,
                count: data.EndDate.Subtract(value: data.StartDate).Days + 1
            ).Select(selector: offset => data.StartDate.AddDays(value: offset))
            .Select(selector: date =>
            {
                IEnumerable<Lesson> lessonsWithDate = lessons.Where(predicate: l => l.Day.Date == date.Date);

                return new GetTimetableRequest(
                    CountLesson: lessonsWithDate.Count(),
                    Date: date,
                    StartTime: lessonsWithDate.MinBy(keySelector: l => l.Start)?.Start,
                    EndTime: lessonsWithDate.MaxBy(keySelector: l => l.End)?.End,
                    Lessons: lessonsWithDate
                );
            }));
        }
    }
}

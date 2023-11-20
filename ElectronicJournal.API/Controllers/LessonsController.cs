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
    public class LessonsController : ControllerBase
    {
        private ElectronicJournalContext _context;
        private ILogger<LessonsController> _logger;

        public LessonsController(ElectronicJournalContext context, ILogger<LessonsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public record GetLessonResponse(int Id, string Name);

        [HttpGet(template: nameof(GetLessons))]
        public async Task<ActionResult<IEnumerable<GetLessonResponse>>> GetLessons()
        {
            int id = Int32.Parse(s: HttpContext.User.FindFirstValue(claimType: ClaimTypes.NameIdentifier));
            User user = _context.Users.AsNoTracking()
                .Include(navigationPropertyPath: u => u.Student.GroupNavigation.SpecializationNavigation.Lessons)
                .First(predicate: u => u.Id == id);

            return Ok(value: user.Student.GroupNavigation.SpecializationNavigation.Lessons
                .Select(selector: l => new GetLessonResponse(Id: l.Id, Name: l.Name))
                .OrderBy(keySelector: l => l.Name));
        }
    }
}

using ElectronicJournal.API.DBModels;
using ElectronicJournal.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController: Controller
    {
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;

        public UtilitiesController(ElectronicJournalContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(template: "GetLoginAvailability")]
        public async Task<ActionResult<bool>> GetLoginAvailability(string login)
        {
            string token = await Confidentiality.GenerateJWT(data: login);
            return Ok(value: await _context.Users.FirstOrDefaultAsync(predicate: u => u.AuthKey == token) is null);
        }
    }
}

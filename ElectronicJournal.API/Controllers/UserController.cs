using ElectronicJournal.API.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ElectronicJournalContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}

using ElectronicJournal.API.DBModels;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicJournal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilitiesController : Controller
    {
        private readonly ElectronicJournalContext _context;
        private readonly ILogger<UserController> _logger;

        public UtilitiesController(ElectronicJournalContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}

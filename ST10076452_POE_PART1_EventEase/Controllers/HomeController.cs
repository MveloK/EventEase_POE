using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST10076452_POE_PART1_EventEase.Models;
using ST10076452_POE_PART1_EventEase.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ST10076452_POE_PART1_EventEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Venues()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Venues(VenueModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Venues.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Events");
            }

            return View(model);
        }

        public IActionResult Events()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Events(EventsModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Success");
            }

            return View(model);
        }

        public IActionResult Bookings()
        {
            var bookings = _context.Bookings.ToList();
            return View(bookings);
        }

        [HttpGet]
        public IActionResult CreateBooking()
        {
            ViewBag.Events = _context.Events.ToList();
            ViewBag.Venues = _context.Venues.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingsModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Bookings.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Bookings");
            }

            ViewBag.Events = _context.Events.ToList();
            ViewBag.Venues = _context.Venues.ToList();
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

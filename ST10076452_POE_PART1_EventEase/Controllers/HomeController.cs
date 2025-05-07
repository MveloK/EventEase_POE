using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ST10076452_POE_PART1_EventEase.Models;
using ST10076452_POE_PART1_EventEase.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
                _logger.LogInformation("Adding venue: {VenueName}, {Location}, {Capacity}, {ImageUrl}", model.VenueName, model.Location, model.Capacity, model.ImageUrl);

                try
                {
                    _context.Venues.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Venue added successfully!";
                    return RedirectToAction("Venues");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while adding venue");
                    TempData["Error"] = "An error occurred while saving the venue.";
                    return View(model);
                }
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage);
            }

            TempData["Error"] = "Please ensure all required fields are filled correctly.";
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
                try
                {
                    _context.Events.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Event created successfully!";
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating event");
                    TempData["Error"] = "Failed to create event. Please check your input.";
                    return View(model);
                }
            }

            TempData["Error"] = "Failed to create event. Please check your input.";
            return View(model);
        }

        // ✅ Bookings with search support
        public IActionResult Bookings(string searchTerm)
        {
            var bookings = _context.Bookings
                .Include(b => b.Events)
                .Include(b => b.Venues)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingID.ToString().Contains(searchTerm) ||
                    b.EventId.ToString().Contains(searchTerm));
            }

            return View(bookings.ToList());
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
                try
                {
                    _context.Bookings.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Booking created successfully!";
                    return RedirectToAction("Bookings");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating booking");
                    TempData["Error"] = "Failed to create booking. Please resolve the issues.";
                    return View(model);
                }
            }

            ViewBag.Events = _context.Events.ToList();
            ViewBag.Venues = _context.Venues.ToList();
            TempData["Error"] = "Failed to create booking. Please resolve the issues.";
            return View(model);
        }

        // ✅ Delete Booking action
        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                // Find the booking by its ID
                var booking = await _context.Bookings.FindAsync(id);

                // Check if the booking exists
                if (booking == null)
                {
                    TempData["Error"] = "Booking not found.";
                    return RedirectToAction("Bookings");
                }

                // Remove the booking from the database
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Booking deleted successfully!";
                return RedirectToAction("Bookings");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting booking");
                TempData["Error"] = "An error occurred while deleting the booking.";
                return RedirectToAction("Bookings");
            }
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

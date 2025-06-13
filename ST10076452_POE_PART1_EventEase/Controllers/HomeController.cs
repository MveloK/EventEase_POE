using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ST10076452_POE_PART1_EventEase.Data;
using ST10076452_POE_PART1_EventEase.Models;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ========== VENUES ==========
        [HttpGet]
        public IActionResult Venues()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Venues(VenueModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Adding venue: {VenueName}, {Location}, {Capacity}, {ImageUrl}",
                        model.VenueName, model.Location, model.Capacity, model.ImageUrl);

                    _context.Venues.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Venue added successfully!";
                    return RedirectToAction(nameof(Venues));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while adding venue");
                    TempData["Error"] = "An error occurred while saving the venue.";
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError("Validation error: {ErrorMessage}", error.ErrorMessage);
                }

                TempData["Error"] = "Please ensure all required fields are filled correctly.";
            }

            return View(model);
        }

        // ========== EVENTS ==========
        [HttpGet]
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
                    return RedirectToAction(nameof(Success));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating event");
                    TempData["Error"] = "Failed to create event. Please check your input.";
                }
            }
            else
            {
                TempData["Error"] = "Failed to create event. Please check your input.";
            }

            return View(model);
        }

        // ========== BOOKINGS ==========
        [HttpGet]
        public async Task<IActionResult> Bookings(string searchTerm, int? eventTypeId, DateTime? startDate, DateTime? endDate, bool? availableOnly)
        {
            var bookings = _context.Bookings
                .Include(b => b.Events).ThenInclude(e => e.EventType)
                .Include(b => b.Venues)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingID.ToString().Contains(searchTerm) ||
                    b.EventId.ToString().Contains(searchTerm));
            }

            if (eventTypeId.HasValue)
            {
                bookings = bookings.Where(b => b.Events != null && b.Events.EventTypeId == eventTypeId);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                bookings = bookings.Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate);
            }

            if (availableOnly == true)
            {
                bookings = bookings.Where(b => b.Venues != null && b.Venues.IsAvailable);
            }

            ViewBag.EventTypes = await _context.EventTypes.ToListAsync();

            return View(await bookings.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> CreateBooking()
        {
            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check for booking conflict: same Venue and same Date
                    var conflictExists = await _context.Bookings.AnyAsync(b =>
                        b.VenueId == model.VenueId && b.BookingDate.Date == model.BookingDate.Date);

                    if (conflictExists)
                    {
                        TempData["Error"] = "This venue is already booked for the selected date.";
                    }
                    else
                    {
                        _context.Bookings.Add(model);
                        await _context.SaveChangesAsync();

                        TempData["Success"] = "Booking created successfully!";
                        return RedirectToAction(nameof(Bookings));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while creating booking");
                    TempData["Error"] = "Failed to create booking. Please resolve the issues.";
                }
            }
            else
            {
                TempData["Error"] = "Failed to create booking. Please resolve the issues.";
            }

            // Reload dropdowns
            ViewBag.Events = await _context.Events.ToListAsync();
            ViewBag.Venues = await _context.Venues.ToListAsync();
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);

                if (booking == null)
                {
                    TempData["Error"] = "Booking not found.";
                    return RedirectToAction(nameof(Bookings));
                }

                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Booking deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting booking");
                TempData["Error"] = "An error occurred while deleting the booking.";
            }

            return RedirectToAction(nameof(Bookings));
        }

        // ========== SUPPORT ==========
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

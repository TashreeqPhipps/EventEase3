using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Services;

namespace EventEase.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;

        public EventsController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        // GET: Events
        public async Task<IActionResult> Index(
            string searchString,
            int? eventTypeId,
            DateTime? startDate,
            DateTime? endDate,
            bool availableOnly = false)
        {
            var events = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .Include(e => e.Bookings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                events = events.Where(e =>
                    e.Name.Contains(searchString) ||
                    e.Venue!.Name.Contains(searchString) ||
                    e.Venue.Location.Contains(searchString) ||
                    e.EventType!.Name.Contains(searchString));
            }

            if (eventTypeId.HasValue && eventTypeId.Value > 0)
            {
                events = events.Where(e => e.EventTypeId == eventTypeId.Value);
            }

            if (startDate.HasValue)
            {
                events = events.Where(e => e.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                events = events.Where(e => e.EndDate <= endDate.Value);
            }

            if (availableOnly)
            {
                events = events.Where(e => e.Bookings == null || !e.Bookings.Any());
            }

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventTypeId);
            ViewData["SearchString"] = searchString;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");
            ViewData["AvailableOnly"] = availableOnly;

            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name");
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                eventItem.ImageUrl = await _blobStorageService.UploadFileAsync(imageFile);
            }

            if (ModelState.IsValid)
            {
                _context.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", eventItem.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
            return View(eventItem);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", eventItem.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem, IFormFile? imageFile)
        {
            if (id != eventItem.EventId)
            {
                return NotFound();
            }

            var existingEvent = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (existingEvent == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingEvent.ImageUrl))
                {
                    await _blobStorageService.DeleteFileAsync(existingEvent.ImageUrl);
                }

                eventItem.ImageUrl = await _blobStorageService.UploadFileAsync(imageFile);
            }
            else
            {
                eventItem.ImageUrl = existingEvent.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.EventId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "Name", eventItem.VenueId);
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
            return View(eventItem);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == eventItem.EventId);

            if (hasBookings)
            {
                TempData["ErrorMessage"] = "This event cannot be deleted because it has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] = "This event cannot be deleted because it has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrWhiteSpace(eventItem.ImageUrl))
            {
                await _blobStorageService.DeleteFileAsync(eventItem.ImageUrl);
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
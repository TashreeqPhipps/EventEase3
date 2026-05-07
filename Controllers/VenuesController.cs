using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Services;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;

        public VenuesController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            var venues = from v in _context.Venues
                         select v;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                venues = venues.Where(v =>
                    v.Name.Contains(searchString) ||
                    v.Location.Contains(searchString));
            }

            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                venue.ImageUrl = await _blobStorageService.UploadFileAsync(imageFile);
            }

            if (ModelState.IsValid)
            {
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venue venue, IFormFile? imageFile)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            var existingVenue = await _context.Venues
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (existingVenue == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(existingVenue.ImageUrl))
                {
                    await _blobStorageService.DeleteFileAsync(existingVenue.ImageUrl);
                }

                venue.ImageUrl = await _blobStorageService.UploadFileAsync(imageFile);
            }
            else
            {
                venue.ImageUrl = existingVenue.ImageUrl;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues
                .Include(v => v.Events)
                    .ThenInclude(e => e.Bookings)
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            bool hasActiveBookings = venue.Events != null &&
                                     venue.Events.Any(e => e.Bookings != null && e.Bookings.Any());

            if (hasActiveBookings)
            {
                TempData["ErrorMessage"] = "This venue cannot be deleted because it has active bookings.";
                return RedirectToAction(nameof(Index));
            }

            if (!string.IsNullOrWhiteSpace(venue.ImageUrl))
            {
                await _blobStorageService.DeleteFileAsync(venue.ImageUrl);
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(v => v.VenueId == id);
        }
    }
}
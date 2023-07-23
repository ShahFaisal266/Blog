using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    public class BookedItemsController : Controller
    {
        private readonly BlogDbContext _context;

        public BookedItemsController(BlogDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserData()
        {

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Query the database to retrieve the booked items for the current user
            var bookedItems = _context.BookedItems
                .Where(item => item.UserId == userId)
                .ToList();

            return View(bookedItems);
        }



        // GET: BookedItems
        public async Task<IActionResult> Index()
        {
            string notificationMessage = TempData["NotificationMessage"] as string;

            // Pass the notification message to the view using ViewBag
            ViewBag.NotificationMessage = notificationMessage;

            return _context.BookedItems != null ?
                        View(await _context.BookedItems.ToListAsync()) :
                        Problem("Entity set 'BlogDbContext.BookedItems'  is null.");

        }

        // GET: BookedItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.BookedItems == null)
            {
                return NotFound();
            }

            var bookedItem = await _context.BookedItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookedItem == null)
            {
                return NotFound();
            }

            return View(bookedItem);
        }

        // GET: BookedItems/Create
        [HttpGet("{id:guid}")]
        public IActionResult Create(Guid id)
        {
            // Your action logic here
            return View();
        }

        [HttpPost]
        [Route("{id:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookedItemReq bookedItem, [FromRoute] Guid id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);

            // Get the user ID of the currently logged-in user
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Query the database to retrieve the user data based on the user ID
            var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == userId);
            var bookedItemEntity = new BookedItem
            {
                UserName = user.Name,
                ItemName = blogPost.PageTitle,
                StartDate = bookedItem.StartDate,
                EndDate = bookedItem.EndDate,
                UserId = user.Id.ToString(),
            };

            _context.BookedItems.Add(bookedItemEntity);
            await _context.SaveChangesAsync();
            TempData["NotificationMessage"] = "Item Booked successfully.";//for Notification

            // Pass the bookedItem object to the "Create" view for display
            return RedirectToAction("GetUserData");
        }
           
        




        // GET: BookedItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.BookedItems == null)
            {
                return NotFound();
            }

            var bookedItem = await _context.BookedItems.FindAsync(id);
            if (bookedItem == null)
            {
                return NotFound();
            }
            return View(bookedItem);
        }

        // POST: BookedItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,UserName,StartDate,EndDate")] BookedItem bookedItem)
        {
            if (id != bookedItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookedItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookedItemExists(bookedItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookedItem);
        }

        // GET: BookedItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.BookedItems == null)
            {
                return NotFound();
            }

            var bookedItem = await _context.BookedItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookedItem == null)
            {
                return NotFound();
            }

            return View(bookedItem);
        }

        // POST: BookedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.BookedItems == null)
            {
                return Problem("Entity set 'BlogDbContext.BookedItems'  is null.");
            }
            var bookedItem = await _context.BookedItems.FindAsync(id);
            if (bookedItem != null)
            {
                _context.BookedItems.Remove(bookedItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookedItemExists(Guid id)
        {
          return (_context.BookedItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

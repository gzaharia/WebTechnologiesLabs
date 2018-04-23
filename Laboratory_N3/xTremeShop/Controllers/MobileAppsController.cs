using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using xTremeShop.Data;
using xTremeShop.Models;
using xTremeShop.ViewModels;

namespace xTremeShop.Controllers
{
    public class MobileAppsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _env;
        public MobileAppsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            List<MobileApp> result = null;

            result = await _context.MobileApps.Where(f => string.Compare(f.Category, query, false) != -1
            || string.Compare(f.Title, query, false) != -1).ToListAsync();

            return View("Index", result);
        }

        // GET: MobileApps
        public async Task<IActionResult> Index(string category)
        {
            List<MobileApp> result = null;

            if (!string.IsNullOrEmpty(category))
            {
                result = await _context.MobileApps.Where(f => f.Category == category).ToListAsync();
            }
            else
            {
                result = await _context.MobileApps.ToListAsync();
            }
            return View(result);
        }

        // GET: MobileApps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileApp = await _context.MobileApps
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobileApp == null)
            {
                return NotFound();
            }

            return View(mobileApp);
        }

        // GET: MobileApps/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> AddToMyLibrary(int appId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userId = user.Id;

            var userLibrary = _context.UserLibraries.FirstOrDefault(f => f.UserId == userId);

            if (userLibrary == null)
            {
                UserLibrary newLibrary = new UserLibrary
                {
                    UserId = userId
                };

                userLibrary = _context.UserLibraries.Add(newLibrary).Entity;

                _context.SaveChanges();
            }

            if (_context.LibraryApps.FirstOrDefault(f => f.AppId == appId && f.LibaryId == userLibrary.Id) == null)
            {
                _context.LibraryApps.Add(new LibraryApps
                {
                    AppId = appId,
                    LibaryId = userLibrary.Id
                });
            }

            _context.SaveChanges();
            return RedirectToAction("Details", new { id = appId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MobileAppCreateViewModel mobileApp)
        {
            if (ModelState.IsValid)
            {
                var newMobileApp = new MobileApp
                {
                    Category = mobileApp.Category,
                    Downloads = mobileApp.Downloads,
                    Price = mobileApp.Price,
                    Rating = mobileApp.Rating,
                    Title = mobileApp.Title
                };

                using (MemoryStream ms = new MemoryStream())
                {
                    mobileApp.AppIcon.CopyTo(ms);
                    newMobileApp.AppIcon = ms.ToArray();
                }
                _context.MobileApps.Add(newMobileApp);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(mobileApp);
        }

        // GET: MobileApps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileApp = await _context.MobileApps.SingleOrDefaultAsync(m => m.Id == id);
            if (mobileApp == null)
            {
                return NotFound();
            }
            return View(mobileApp);
        }

        // POST: MobileApps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Downloads,Rating,Category")] MobileApp mobileApp)
        {
            if (id != mobileApp.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mobileApp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MobileAppExists(mobileApp.Id))
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
            return View(mobileApp);
        }

        // GET: MobileApps/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobileApp = await _context.MobileApps
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobileApp == null)
            {
                return NotFound();
            }

            return View(mobileApp);
        }

        // POST: MobileApps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mobileApp = await _context.MobileApps.SingleOrDefaultAsync(m => m.Id == id);
            _context.MobileApps.Remove(mobileApp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MobileAppExists(int id)
        {
            return _context.MobileApps.Any(e => e.Id == id);
        }
    }
}

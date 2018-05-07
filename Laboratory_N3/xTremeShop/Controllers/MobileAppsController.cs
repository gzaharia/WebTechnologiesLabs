using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public MobileAppsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IHostingEnvironment env,
            ILogger<MobileAppsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            List<MobileAppViewModel> models = null;

            var apps = await _context.MobileApps.Where(f => string.Compare(f.Category, query, false) != -1
            || string.Compare(f.Title, query, false) != -1)
                                 .ToListAsync();

            var user = await _userManager.GetUserAsync(User);
            bool isAdmin = (user == null) ? false : await _userManager.IsInRoleAsync(user, "Administrator");

            models = apps.Select(app => new MobileAppViewModel()
            {
                Id = app.Id,
                Title = app.Title,
                Price = app.Price,
                Downloads = app.Downloads,
                Rating = app.Rating,
                Category = app.Category,
                AppIcon = app.AppIcon,
                LibraryApps = app.LibraryApps,
                FullAccess = isAdmin || user?.Id == app.UserId
            }).ToList();

            return View("Index", models);
        }

        // GET: MobileApps
        public async Task<IActionResult> Index(string category)
        {
            List<MobileApp> mobileApps;
            var result = new List<MobileAppViewModel>();

            if (!string.IsNullOrEmpty(category))
            {
                mobileApps = await _context.MobileApps.Where(f => f.Category == category).ToListAsync();
            }
            else
            {
                mobileApps = await _context.MobileApps.ToListAsync();
            }

            ApplicationUser user = null;

            if (User.Identity.IsAuthenticated)
                user = await _userManager.GetUserAsync(User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

            foreach (var app in mobileApps)
            {
                var model = new MobileAppViewModel()
                {
                    Id = app.Id,
                    Title = app.Title,
                    Price = app.Price,
                    Downloads = app.Downloads,
                    Rating = app.Rating,
                    Category = app.Category,
                    AppIcon = app.AppIcon,
                    LibraryApps = app.LibraryApps
                };

                model.FullAccess = isAdmin || (user?.Id == app.UserId);
                result.Add(model);
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

        [Authorize] // c2-d2
        public async Task<IActionResult> AddToMyLibrary(int appId)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = await _userManager.GetUserAsync(User);
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
        [Authorize] // Added by emil
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MobileAppCreateViewModel mobileApp)
        {
            if (ModelState.IsValid && mobileApp != null)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                var newMobileApp = new MobileApp
                {
                    Category = mobileApp.Category,
                    Downloads = mobileApp.Downloads,
                    Price = mobileApp.Price,
                    Rating = mobileApp.Rating,
                    Title = mobileApp.Title,
                    UserId = user.Id // I shouldnt check for that.
                };

                if (mobileApp.AppIcon != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        mobileApp.AppIcon.CopyTo(ms);
                        newMobileApp.AppIcon = ms.ToArray();
                    }
                }

                _context.MobileApps.Add(newMobileApp);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(mobileApp);
        }

        // GET: MobileApps/Edit/5
        [Authorize]
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

            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (!(mobileApp.UserId == user.Id || await _userManager.IsInRoleAsync(user, "Administrator")))
                return RedirectToAction("NotAuthorized", "Accout");

            return View(mobileApp);
        }

        // POST: MobileApps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Price,Downloads,Rating,Category")] MobileApp mobileApp)
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
        //[Authorize(Roles = "Administrator")] // Modified by emil.
        [Authorize]
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
        //[Authorize(Roles = "Administrator")]
        [Authorize]
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

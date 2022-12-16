using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppAspNetCore.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace AppAspNetCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notifyService { get; }

        public AdminAccountsController(
            Resbooking1Context context,
            INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminAccounts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["AccessRights"] = new SelectList(_context.Roles, "RoleId", "Description");

            List<SelectListItem> status = new List<SelectListItem>();

            status.Add(new SelectListItem() { Text = "Active", Value = "1"  });
            status.Add(new SelectListItem() { Text = "Inactive", Value = "0" });
            ViewData["Status"] = status;

            var resbooking1Context = _context.Accounts.Include(r => r.Role);
            
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Admin/AdminAccount/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            var resbooking1Context = _context.Accounts.Include(r => r.Role);
            
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId, FullName")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                _notifyService.Success("Created Successfully!");

                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Admin/AdminAccounts/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId, FullName, Email, PhoneNumber")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                _context.Update(account);
                await _context.SaveChangesAsync();
                _notifyService.Success("Updated Successfully!");
                }
                catch (DbUpdateConcurrencyException)
                {
                // TODO
                if (!AccountExists(account.AccountId))
                {
                    _notifyService.Success("An error occurred");
                    return NotFound();
                }
                else
                {
                    throw;
                }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Admin/AdminAccounts/Delete/1
        public async Task<IActionResult> Delete(string id) // <-- Here it is
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.AccountId.Equals(id));
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Admin/AdminAccounts/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // <-- Here it is
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            _notifyService.Success("Deleted Successfully!");
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
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
    public class AdminCustomersController : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notifyService { get; }

        public AdminCustomersController(
        Resbooking1Context context,
        INotyfService notifyService)
        {
        _context = context;
        _notifyService = notifyService;
        }

        // GET: Admin/AdminCustomers
        [HttpGet]
        public async Task<IActionResult> Index()
        {
        ViewData["District"] = new SelectList(_context.Locations, "LocationId", "Type");

        List<SelectListItem> status = new List<SelectListItem>();

        status.Add(new SelectListItem() { Text = "Active", Value = "1"  });
        status.Add(new SelectListItem() { Text = "Inactive", Value = "0" });
        ViewData["Status"] = status;

        return View(await _context.Customers.ToListAsync());
        }

        // GET: Admin/AdminCustomer/Details/1
        public async Task<IActionResult> Details(int? id)
        {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers
            .FirstOrDefaultAsync(m => m.CustomerId == id);
        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId, FullName")] Customer customer)
        {
        if (ModelState.IsValid)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
            _notifyService.Success("Created Successfully!");

            return RedirectToAction(nameof(Index));
        }
        return View(customer);
        }

        // GET: Admin/AdminCustomers/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
        if (id == null)
        {
            return NotFound();
        }

        var customer = await _context.Customers.FindAsync(id);

        return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId, FullName")] Customer customer)
        {
        if (id != customer.CustomerId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
            _context.Update(customer);
            await _context.SaveChangesAsync();
            _notifyService.Success("Updated Successfully!");
            }
            catch (DbUpdateConcurrencyException)
            {
            // TODO
            if (!CustomerExists(customer.CustomerId))
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
        return View(customer);
        }

        // GET: Admin/AdminCustomers/Delete/1
        public async Task<IActionResult> Delete(string id) // <-- Here it is
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId.Equals(id));
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/AdminCustomers/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // <-- Here it is
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            _notifyService.Success("Deleted Successfully!");
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
        return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
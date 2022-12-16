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
    public class AdminMealsController : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notifyService { get; }

        public AdminMealsController(
            Resbooking1Context context,
            INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminMeals
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            List<SelectListItem> status = new List<SelectListItem>();

            status.Add(new SelectListItem() { Text = "In of Stock", Value = "1" });
            status.Add(new SelectListItem() { Text = "Out of Stock", Value = "0" });
            ViewData["Status"] = status;

            var resbooking1Context = _context.Meals.Include(mc => mc.MealCategory);

            return View(await _context.Meals.ToListAsync());
        }

        // GET: Admin/AdminMeal/Details/1
        public async Task<IActionResult> Details(int? id)
        {   
            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals
                .FirstOrDefaultAsync(m => m.MealId == id);
            if (meal == null)
            {
                return NotFound();
            }

            var resbooking1Context = _context.Meals.Include(mc => mc.MealCategory);

            return View(meal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealId, Name, Price, Quantity, Discount")] Meal meal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meal);
                await _context.SaveChangesAsync();
                _notifyService.Success("Created Successfully!");

                return RedirectToAction(nameof(Index));
            }
            return View(meal);
        }

        // GET: Admin/AdminMeals/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.FindAsync(id);

            return View(meal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MealId, Name, Description, Price, Quantity, Discount")] Meal meal)
        {
            if (id != meal.MealId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                    {
                        _context.Update(meal);
                        await _context.SaveChangesAsync();
                        _notifyService.Success("Updated Successfully!");
                    }
                catch (DbUpdateConcurrencyException)
                {
                    // TODO
                    if (!MealExists(meal.MealId))
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
            return View(meal);
        }

        // GET: Admin/AdminMeals/Delete/1
        public async Task<IActionResult> Delete(string id) // <-- Here it is
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.FirstOrDefaultAsync(m => m.MealId.Equals(id));
            if (meal == null)
            {
                return NotFound();
            }

            return View(meal);
        }

        // POST: Admin/AdminMeals/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // <-- Here it is
        {
            var meal = await _context.Meals.FindAsync(id);
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
            _notifyService.Success("Deleted Successfully!");
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.MealId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppAspNetCore.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AppAspNetCore.Helper;

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
            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name", "Description");

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

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");
            // ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            // List<SelectListItem> status = new List<SelectListItem>();

            // status.Add(new SelectListItem() { Text = "In of Stock", Value = "1" });
            // status.Add(new SelectListItem() { Text = "Out of Stock", Value = "0" });
            // ViewData["Status"] = status;

            // var resbooking1Context = _context.Meals.Include(mc => mc.MealCategory);

            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealId, Name, Price, Discount, Quantity, MealCategoryId, Image, Description")] Meal meal, Microsoft.AspNetCore.Http.IFormFile image)
        {

            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name", meal.MealCategoryId);
            
            if (meal.MealCategory == null)
            {
                meal.Name = Utilities.ToTitleCase(meal.Name);
                if (image != null)
                {
                    string extension = Path.GetExtension(image.FileName);
                    string _image = Utilities.SEOUrl(meal.Name) + extension;
                    meal.Image = await Utilities.UploadFile(image, @"meals", _image.ToLower());
                }

                if (string.IsNullOrEmpty(meal.Image)) meal.Image = "default.jpg";

                meal.Status = true;
                meal.IsFavorite = false;
                meal.CreateTime = DateTime.Now;
                meal.CreateBy = "Admin";
                meal.UpdateTime = DateTime.Now;
                meal.UpdateBy = "Admin";

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
        public async Task<IActionResult> Edit(int id, [Bind("MealId, Name, Price, Discount, Quantity, MealCategoryId, Image, Description")] Meal meal, Microsoft.AspNetCore.Http.IFormFile image)
        {
            if (id != meal.MealId)
            {
                return NotFound();
            }

            if (meal.MealCategory == null)
            {
                try
                {
                    meal.Name = Utilities.ToTitleCase(meal.Name);

                    if (image != null)
                    {
                        string extension = Path.GetExtension(image.FileName);
                        string _image = Utilities.SEOUrl(meal.Name) + extension;
                        meal.Image = await Utilities.UploadFile(image, @"meals", _image.ToLower());
                    }

                    if (string.IsNullOrEmpty(meal.Name)) meal.Image = "default.jpg";

                    if (meal.MealCategoryId != null)
                    {
                        meal.MealCategoryId = 1;
                    }

                    meal.UpdateTime = DateTime.Now;

                    _context.Update(meal);
                    _notifyService.Success("Updated Successfully!");
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // TODO
                    if (!MealExists(meal.MealId))
                    {
                        _notifyService.Success("This meal deleted or not available!");
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Success("An error occurred");
                        throw;
                    }
                }
            }

            ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name", meal.MealCategoryId);
            return View(meal);
        }

        // GET: Admin/AdminMeals/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meal = await _context.Meals.FindAsync(id);

            return View(meal);
        }

        // POST: Admin/AdminMeals/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            _context.Meals.Remove(meal);
            _notifyService.Success("Deleted Successfully!");

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.Meals.Any(e => e.MealId == id);
        }
    }
}

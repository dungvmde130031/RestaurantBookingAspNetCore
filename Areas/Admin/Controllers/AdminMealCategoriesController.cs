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
    public class AdminMealCategoriesController : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notifyService { get; }

        public AdminMealCategoriesController(
            Resbooking1Context context,
            INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: Admin/AdminMealCategories
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            return View(await _context.Meals.ToListAsync());
        }

        // GET: Admin/AdminMealCategories/Details/1
        public async Task<IActionResult> Details(int? id)
        {   
            // ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var mealCat = await _context.MealCategories
                .FirstOrDefaultAsync(m => m.MealCategoryId == id);
            if (mealCat == null)
            {
                return NotFound();
            }

            return View(mealCat);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name");

            return PartialView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MealCategoryId, Name, Image, Description, Title")] MealCategory mealCat, Microsoft.AspNetCore.Http.IFormFile image)
        {
            if (ModelState.IsValid)
            {
                // meal.Name = Utilities.ToTitleCase(meal.Name);
                if (image != null)
                {
                    string extension = Path.GetExtension(image.FileName);
                    string _image = Utilities.SEOUrl(mealCat.Name) + extension;
                    mealCat.Image = await Utilities.UploadFile(image, @"mealCategories", _image.ToLower());
                }

                if (string.IsNullOrEmpty(mealCat.Image)) mealCat.Image = "default.jpg";
                mealCat.UpdateTime = DateTime.Now;
                mealCat.CreateTime = DateTime.Now;

                _context.Add(mealCat);
                await _context.SaveChangesAsync();
                _notifyService.Success("Created Successfully!");

                return RedirectToAction(nameof(Index));
            }

            return View(mealCat);
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
        public async Task<IActionResult> Edit(int id, [Bind("MealCategoryId, Name, Image, Description, Title")] MealCategory mealCat, Microsoft.AspNetCore.Http.IFormFile image)
        {
            if (id != mealCat.MealCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    mealCat.Name = Utilities.ToTitleCase(mealCat.Name);

                    if (image != null)
                    {
                        string extension = Path.GetExtension(image.FileName);
                        string _image = Utilities.SEOUrl(mealCat.Name) + extension;
                        mealCat.Image = await Utilities.UploadFile(image, @"mealCategories", _image.ToLower());
                    }

                    if (string.IsNullOrEmpty(mealCat.Name)) mealCat.Image = "default.jpg";

                    mealCat.UpdateTime = DateTime.Now;

                    _context.Update(mealCat);
                    _notifyService.Success("Updated Successfully!");
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // TODO
                    if (!MealExists(mealCat.MealCategoryId))
                    {
                        _notifyService.Success("This category deleted or not available!");
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Success("An error occurred");
                        throw;
                    }
                }
            }

            // ViewData["Category"] = new SelectList(_context.MealCategories, "MealCategoryId", "Name", mealCat.MealCategoryId);
            return View(mealCat);
        }

        // GET: Admin/AdminMeals/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mealCat = await _context.MealCategories.FindAsync(id);

            return View(mealCat);
        }

        // POST: Admin/AdminMeals/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mealCat = await _context.MealCategories.FindAsync(id);
            _context.MealCategories.Remove(mealCat);
            _notifyService.Success("Deleted Successfully!");

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        private bool MealExists(int id)
        {
            return _context.MealCategories.Any(e => e.MealCategoryId == id);
        }
    }
}

using AppAspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAspNetCore.Controllers
{
    public class MealController : Controller
    {
        private readonly Resbooking1Context _context;

        public MealController(Resbooking1Context context)
        {
            _context = context;
        }
        public IActionResult Index() 
        {
            try
            {
                var category = _context.Meals
                    .OrderByDescending(x => x.CreateTime);

                List<Meal> models = new List<Meal>(category);
                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult List(int MealCategoryId) 
        {
            try
            {
                var currentCategory = _context.MealCategories.Find(MealCategoryId);
                var category = _context.Meals
                    .Where(x => x.MealCategoryId == MealCategoryId)
                    .OrderByDescending(x => x.CreateTime);

                List<Meal> models = new List<Meal>(category);
                ViewBag.CurrentCategory = currentCategory;

                return View(models);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var meal = _context.Meals
                    .Include(x => x.MealCategory)
                    .FirstOrDefault(x => x.MealId == id);

                if (meal == null)
                {
                    return RedirectToAction("Index");
                }
                return View(meal);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
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
            return View();
        }

        public IActionResult Details(int id)
        {
            var meal = _context.Meals.Include(x => x.MealCategory).FirstOrDefault(x => x.MealId == id);

            if (meal == null)
            {
                return RedirectToAction("Index");
            }
            return View(meal);
        }
    }
}
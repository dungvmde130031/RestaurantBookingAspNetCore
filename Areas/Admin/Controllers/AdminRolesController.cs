using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppAspNetCore.Models;

namespace AppAspNetCore.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class AdminRolesController : Controller
  {
    private readonly Resbooking1Context _context;
    public AdminRolesController(Resbooking1Context context)
    {
      _context = context;
    }

    // GET: Admin/AdminRoles
    [HttpGet]
    public async Task<IActionResult> Index()
    {
      return View(await _context.Roles.ToListAsync());
    }

    // GET: Admin/AdminRole/Details/1
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
          return NotFound();
      }

      var role = await _context.Roles
          .FirstOrDefaultAsync(m => m.RoleId == id);
      if (role == null)
      {
          return NotFound();
      }

      return View(role);
    }


  }
}
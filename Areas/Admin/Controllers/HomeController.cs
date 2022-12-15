using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAspNetCore.Areas.Admin.Controllers
{
  [Area("Admin")]
  public class HomeController : Controller
  {

    public INotyfService _notyfService { get; }

    public HomeController (INotyfService notyfService)
    {
      _notyfService = notyfService;
    }

    public IActionResult Index()
    {
      return View();
    }
  }
}
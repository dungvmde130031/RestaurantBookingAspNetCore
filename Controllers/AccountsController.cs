using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAspNetCore.Extentions;
using AppAspNetCore.Helper;
using AppAspNetCore.Models;
using AppAspNetCore.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AppAspNetCore.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly Resbooking1Context _context;

        public INotyfService _notyfService { get; }

        public AccountsController(Resbooking1Context context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        [Route("my-account.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var cusId = HttpContext.Session.GetString("CustomerId");

            if (cusId != null)
            {
                var customer = _context.Customers
                    .SingleOrDefault(x => x.CustomerId == Convert.ToInt32(cusId));

                if (customer != null)
                {
                    var orderList = _context.Orders
                        .Include(x => x.TransactStatus)
                        .Where(x => x.CustomerId == customer.CustomerId)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.Order = orderList;

                    return View(customer);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register.html", Name = "Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register.html", Name = "Register")]
        public async Task<IActionResult> Register(RegisterViewModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string rdKey = Utilities.GetRandomKey();
                    Customer cus = new Customer
                    {
                        FullName = customer.FullName,
                        Email = customer.Email.Trim().ToLower(),
                        Password = (customer.Password + rdKey.Trim()).ToMD5(),
                        IsActive = true,
                        Salt = rdKey,
                        CreateTime = DateTime.Now
                    };
                    try
                    {
                        _context.Add(cus);
                        await _context.SaveChangesAsync();

                        // Save session CustomerId
                        HttpContext.Session.SetString("CustomerId", cus.CustomerId.ToString());

                        var accountId = HttpContext.Session.GetString("CustomerId");

                        // Identity
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, cus.FullName),
                            new Claim("CustomerId", cus.CustomerId.ToString())
                        };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        _notyfService.Success("Register Successfully!");

                        return RedirectToAction("Dashboard", "Accounts");
                    }
                    catch(Exception ex)
                    {
                        return RedirectToAction("Register", "Accounts");
                    }
                }
                else
                {
                    return View(customer);
                }
            }
            catch
            {
                return View(customer);
            }
        }

        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public IActionResult Login(string returnUrl = null)
        {
            var cusId = HttpContext.Session.GetString("CustomerId");

            if (cusId != null)
            {
                return RedirectToAction("Dashboard", "Accounts");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel customer, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Utilities.IsValidEmail(customer.Username);
                    if (!isEmail) return View(customer);

                    var cus = _context.Customers
                        .SingleOrDefault(x => x.Email.Trim() == customer.Username);

                    if (cus == null) return RedirectToAction("Register");

                    string pass = (customer.Password + cus.Salt.Trim()).ToMD5();

                    if (cus.Password != pass)
                    {
                        _notyfService.Error("The Username or Password is Incorrect!");
                        return View(customer);
                    }

                    // Check account isActive
                    if (cus.IsActive == false) return RedirectToAction("Notification", "Accounts");

                    // Save session CustomerId
                    HttpContext.Session.SetString("CustomerId", cus.CustomerId.ToString());
                    var accountId = HttpContext.Session.GetString("CustomerId");

                    // Identity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, cus.FullName),
                        new Claim("CustomerId", cus.CustomerId.ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    _notyfService.Success("Login Successfully!"); 
                    return RedirectToAction("SHH", "Accounts");
                }
            }
            catch
            {
                return RedirectToAction("Register", "Accounts");
            }
            return View(customer);
        }

        [HttpGet]
        [Route("logout.html", Name="Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }

        // Validate Phone
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidatePhone(string Phone)
        {
            try
            {
                var customer = _context.Customers.SingleOrDefault(x => x.PhoneNumber.ToLower() == Phone.ToLower());
                if (customer != null)
                {
                    return Json(data: "The Phone number is: " + Phone + "used!");
                }

                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }

        // Validate Email
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateEmail(string Email)
        {
            try
            {
                var customer = _context.Customers.SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (customer != null)
                {
                    return Json(data: "The Email is: " + Email + "used!"); 
                }

                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }
        }
    }
}
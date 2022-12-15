using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AppAspNetCore.Areas.Admin.Views.Shared
{
    public class _SideNavPartialView : PageModel
    {
        private readonly ILogger<_SideNavPartialView> _logger;

        public _SideNavPartialView(ILogger<_SideNavPartialView> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
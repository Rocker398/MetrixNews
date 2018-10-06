using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MetrixNews.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
    }
}
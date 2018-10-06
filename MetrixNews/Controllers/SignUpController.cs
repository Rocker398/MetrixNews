using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MetrixNews.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Sign Up";

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetrixNews.Models;
using MetrixNews.Data;
using MySql.Data.MySqlClient;

namespace MetrixNews.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (MySqlConnection conn = DBAccess.GetConnection())
            {
                if (conn != null)
                {
                    List<ArticleData> articles = ArticleData.GetAll(conn);
                    List<SourceData> sources = SourceData.GetAll(conn);
                }
            }

            return View();
        }

        /*public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult SignUp()
        {
            ViewData["Message"] = "Sign Up";

            return View();
        }*/

        public ActionResult Articles(int category)
        {
            return PartialView();
        }

        public ActionResult Topics(int topic)
        {
            return PartialView();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

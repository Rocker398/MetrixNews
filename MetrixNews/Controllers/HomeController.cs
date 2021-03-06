﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetrixNews.Models;
using MetrixNews.Data;
using MetrixNews.Log;
using MySql.Data.MySqlClient;
using System.IO;

namespace MetrixNews.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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
            ArticleViewModel viewModel = new ArticleViewModel();

            using (MySqlConnection conn = DBAccess.GetConnection())
            {
                if (conn != null)
                {
                    viewModel = ArticleViewModel.GetModel(conn);
                }
            }

            return PartialView(viewModel);
        }

        public ActionResult Topics(string topic)
        {
            TopicViewModel viewModel = new TopicViewModel();
            using (MySqlConnection conn = DBAccess.GetConnection())
            {
                if (conn != null)
                {
                    viewModel = TopicViewModel.GetModel(conn, topic);
                }
            }

            return PartialView(viewModel);
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

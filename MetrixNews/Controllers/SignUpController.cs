using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetrixNews.Data;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MetrixNews.Controllers
{
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Sign Up";

            return View();
        }

        public JsonResult Subscribe(string email, string password)
        {
            using (MySqlConnection conn = DBAccess.GetConnection())
            {
                if (conn != null)
                {
                    MailingListData mailingListData = new MailingListData()
                    {
                        EmailAddress = email
                    };

                    mailingListData.Save(conn);

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
    }
}
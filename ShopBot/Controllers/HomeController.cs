using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ShopBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GetSchedule()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Tutorial()
        {
            return View();
        }

        public IActionResult MakeSchedule()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult MakeASchedule(string ItemName, string Date, int Quantity, string URL, string UserName)
        {
            Console.WriteLine(ItemName);
            Console.WriteLine("Must've Worked");
            string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
            MySqlConnection connect = new MySqlConnection(ConnectionStr);
            MySqlCommand makeSchedule = connect.CreateCommand();
            makeSchedule.CommandText = "insert into RST_DB.schedules (`user_email`,`url`,`item`) values ('" + UserName + "','" + URL + "','" + ItemName + "');";
            connect.Open();
            try
            {
                string results = (string)makeSchedule.ExecuteScalar();
                Console.WriteLine(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            connect.Close();

            return View("MakeSchedule");
        }
    }
}

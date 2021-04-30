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
            ViewBag.scheduleMessage = "No Schedules loaded";
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
            Console.WriteLine("Item: " + ItemName + " User name: " + UserName + " Quantity: " + Quantity + " URL: " + URL + " Date: " + Date);
            //Console.WriteLine("Must've Worked");
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

        [HttpPost]
        public ActionResult GetSchedules(string UserName)
        {
            List<String[]> schedules = new();
            Console.WriteLine(" User name: " + UserName);
            //Console.WriteLine("Must've Worked");
            string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
            MySqlConnection connect = new MySqlConnection(ConnectionStr);
            MySqlCommand getSchedule = connect.CreateCommand();
            getSchedule.CommandText = "Select * from RST_DB.schedules where `user_email` ='" + UserName + "';";
            connect.Open();
            try
            {
                bool jumper = true;
                int count = 0;
                MySqlDataReader connection = getSchedule.ExecuteReader();
                connection.Read();
                while (connection.HasRows && jumper && count < 10)
                {

                    Console.WriteLine("Rows: " + connection.HasRows + "Count: " + count);
                    if (connection.FieldCount != 5)
                    {
                        connect.Close();
                        Console.WriteLine("Wrong number of fields: " + connection.FieldCount);
                        jumper = false;
                    }
                    else
                    {
                        int ScheduleIDN = (int)connection.GetValue(0);
                        string User_email = (string)connection.GetValue(1);
                        ulong Archived = (ulong)connection.GetValue(2);
                        string URL = (string)connection.GetValue(3);
                        string ItemText = (string)connection.GetValue(4);

                        string[] results = { User_email, URL, ItemText};
                        Console.WriteLine("SIDN: " + ScheduleIDN + "Archived: " + Archived);
                        Console.WriteLine(results[0] +  results[1] + results[2]);
                        schedules.Add(results);
                    }
                    count++;
                    connection.Read();
                }
                if (schedules.Count == 0)
                {
                    Console.WriteLine("No matching Schedules!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            connect.Close();
            ViewBag.scheduleMessage = "Schedules";
            return View("GetSchedule");
        }
    }
}

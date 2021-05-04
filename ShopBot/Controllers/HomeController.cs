using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBot.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Web;

namespace ShopBot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static string accuout { get; set; }
        public static string Account
        {
            get
            {
                Console.WriteLine("Account get() stub: "+ accuout);
                return accuout;
            }
            set
            {
                Console.WriteLine("Account set() stub: " + value);
                accuout = value;
            }
        }

        public static RSAParameters publicKey;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            GenerateKeys();
        }

        //https://www.youtube.com/watch?v=EA5jF_7FteM
        private static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                Console.WriteLine("GenerateKeys Stub");
                rsa.PersistKeyInCsp = false; //don't store keys in a container
                publicKey = rsa.ExportParameters(false);
            }
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
            if(accuout != null) 
            {
                //string encodedString = HttpUtility.HtmlEncode(scheduleString);
                ViewBag.scheduleMessage = GetSchedules();
                return View();
            }
            else
            {
                string scheduleString = "<h2> Login to view Schedules </h2> <table class=\"table table-striped table-bordered \"> " +
                                                         "<thead> <tr> <th scope=\"col\"> URL </th> <th scope=\"col\"> QTY: </th> <th scope=\"col\"> Item </th> <th scope=\"col\"> Archived: </th> </tr> ";

                //string encodedString = HttpUtility.HtmlEncode(scheduleString);
                ViewBag.scheduleMessage = scheduleString;
                return View();
            }
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Tutorial()
        {
            return View();
        }

        public static void passEmail(string email)
        {
            Account = email;
        }

        //public ActionResult MakeSchedule() { }

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
            makeSchedule.CommandText = "insert into RST_DB.schedules (`user_email`,`url`,`item`,`quantity`) values ('" + UserName + "','" + URL + "','" + ItemName + "','" + Quantity + "');";
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

       
        public string GetSchedules()
        {
            List<String[]> schedules = new();
            Console.WriteLine(" User name: " + Account);
            //Console.WriteLine("Must've Worked");
            string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
            MySqlConnection connect = new MySqlConnection(ConnectionStr);
            MySqlCommand getSchedule = connect.CreateCommand();
            getSchedule.CommandText = "Select * from RST_DB.schedules where `user_email` ='" + Account + "';";
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
                    if (connection.FieldCount != 7)
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
                        int Quantity = (int)connection.GetValue(5);
                        string IsArchived = "no";
                        if (Archived!=0)
                        {
                            IsArchived = "yes";
                        }

                        string[] results = { User_email, URL, ItemText, Quantity.ToString(), IsArchived };
                        Console.WriteLine("SIDN: " + ScheduleIDN + " Archived: " + IsArchived);
                        Console.WriteLine(results[0] + " " + results[1] + " " + results[2] + " " + results[3]);
                        schedules.Add(results);
                        connection.Read();
                    }
                    count++;
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
            string scheduleString = "<h2>" + Account + " </h2> <table class=\"table table-striped table-bordered \"> " +
                                                     "<thead> <tr> <th scope=\"col\"> URL: </th> <th scope=\"col\"> QTY: </th> <th scope=\"col\"> Item: </th> <th scope=\"col\"> Archived: </th> </tr> ";
            foreach (string[] schedule in schedules)
            {
                scheduleString = scheduleString + "<tr> <td>" + schedule[1] + " </td> <td>" + schedule[3] + " </td> <td>" + schedule[2] + " </td> <td>" + schedule[4] + " </td> </tr> ";
            }
            scheduleString = scheduleString + "</thead> </table>";
            //string encodedString = HttpUtility.HtmlEncode(scheduleString);
            return scheduleString;
        }
    }
}

using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;

namespace WalmartAutomation
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var webDriver = LaunchBrowser();
            try
            {
                int n = 1;
                var walmartautomation = new Interacting(webDriver);
                string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
                string email = args[0];
                MySqlConnection connect = new MySqlConnection(ConnectionStr);
                MySqlCommand count = connect.CreateCommand();
                count.CommandText = "SELECT COUNT(*) FROM RST_DB.schedules";
                MySqlCommand first = connect.CreateCommand();
                first.Parameters.AddWithValue("@email", email);
                first.CommandText = "SELECT item  FROM RST_DB.schedules WHERE user_email = @email LIMIT 1;";
                connect.Open();
                await walmartautomation.FindItemAsync((string)first.ExecuteScalar());
                while (n < (Int64)count.ExecuteScalar())
                {
                    MySqlCommand loop = connect.CreateCommand();
                    loop.Parameters.AddWithValue("@email", email);
                    loop.Parameters.AddWithValue("@num", n);
                    loop.CommandText = "SELECT item  FROM RST_DB.schedules WHERE user_email = @email LIMIT @num,1;";
                    await walmartautomation.FindItemAsync((string)loop.ExecuteScalar());
                    n = n + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while executing automation");
                Console.WriteLine(ex.ToString());
            }
        }

        static IWebDriver LaunchBrowser()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument(@"user-data-dir=C:\Users\owens\AppData\Local\Google\Chrome\User Data\");
            options.AddAdditionalCapability("useAutomationExtension", false);
            Process [] chromekiller = Process.GetProcessesByName("chrome");
            foreach (Process p in chromekiller)
            {
                p.Kill();
            }
            var driver = new ChromeDriver(Environment.CurrentDirectory, options);
            return driver;
        }
    }
}

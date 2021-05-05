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
                count.Parameters.AddWithValue("@email", email);
                count.CommandText = "SELECT COUNT(*) FROM RST_DB.schedules WHERE user_email = @email";

                MySqlCommand firsturl = connect.CreateCommand();
                firsturl.Parameters.AddWithValue("@email", email);
                firsturl.CommandText = "SELECT url FROM RST_DB.schedules WHERE user_email = @email LIMIT 1;";


                MySqlCommand firstitem = connect.CreateCommand();
                firstitem.Parameters.AddWithValue("@email", email);
                firstitem.CommandText = "SELECT item  FROM RST_DB.schedules WHERE user_email = @email LIMIT 1;";

                MySqlCommand quantity = connect.CreateCommand();
                quantity.Parameters.AddWithValue("@email", email);
                quantity.CommandText = "SELECT quantity  FROM RST_DB.schedules WHERE user_email = @email LIMIT 1;";

                connect.Open();
                await walmartautomation.FindItemAsync((string)firstitem.ExecuteScalar(), (string)firsturl.ExecuteScalar(), (int)quantity.ExecuteScalar());
                if(n != (Int64)count.ExecuteScalar())
                {
                    while (n < (Int64)count.ExecuteScalar())
                    {
                        MySqlCommand loopurl = connect.CreateCommand();
                        loopurl.Parameters.AddWithValue("@email", email);
                        loopurl.Parameters.AddWithValue("@num", n);
                        loopurl.CommandText = "SELECT url FROM RST_DB.schedules WHERE user_email = @email LIMIT @num,1";

                        MySqlCommand loopitem = connect.CreateCommand();
                        loopitem.Parameters.AddWithValue("@email", email);
                        loopitem.Parameters.AddWithValue("@num", n);
                        loopitem.CommandText = "SELECT item FROM RST_DB.schedules WHERE user_email = @email LIMIT @num,1;";

                        MySqlCommand loopquantitty = connect.CreateCommand();
                        loopquantitty.Parameters.AddWithValue("@email", email);
                        loopquantitty.Parameters.AddWithValue("@num", n);
                        loopquantitty.CommandText = "SELECT quantity FROM RST_DB.schedules WHERE user_email = @email LIMIT @num,1;";

                        await walmartautomation.FindItemAsync((string)loopitem.ExecuteScalar(), (string)loopurl.ExecuteScalar(), (int)quantity.ExecuteScalar());
                        n = n + 1;
                    }
                }
                //await walmartautomation.walmartCheckoutAsync();
                //await walmartautomation.AmazonCheckoutAsync();
                connect.Close();
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

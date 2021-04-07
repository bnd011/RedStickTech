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
                var walmartautomation = new Interacting(webDriver);
                await walmartautomation.FindItemAsync("Great Value Mild Ground Italian Sausage, 16 oz");
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

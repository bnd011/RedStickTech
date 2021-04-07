using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalmartAutomation
{
    public class Interacting
    {
        private readonly IWebDriver webDriver;

        public Interacting(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public async Task FindItemAsync(string ItemName)
        {
            // Navigate to Walmart
            webDriver.Url = "https://www.walmart.com/grocery/";

            //List of Items
            string Item = ItemName;
            //String to put into Xpath
            string XpathStr = $"//button[@aria-label= concat('Add to cart ','{Item}') and //button[@data-automation-id = 'addToCartBtn'] ]";
            // Find the search bar and enter value
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
            await Task.Delay(1000);
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-automation-id = 'searchField']")));
            var input = webDriver.FindElement(By.XPath("//input[@data-automation-id = 'searchField']"));
            await Task.Delay(2000);
            input.SendKeys(ItemName);
            var click = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'searchBtn']"));
            await Task.Delay(2000);
            click.Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'addToCartBtn']")));
            var click2 = webDriver.FindElement(By.XPath(XpathStr));
            await Task.Delay(2000);
            click2.Click();
            webDriver.Quit();
        }

    }
}

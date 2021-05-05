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

        public async Task FindItemAsync(string ItemName, string URL, int Quantity)
        {

            //List of Items
            string Item = ItemName;


            if (URL == "https://www.walmart.com/grocery/")
            {
                // Navigate to Walmart
                webDriver.Url = URL;
                //String to put into Xpath
                string XpathStr = $"//button[@aria-label=" + "\"" + "Add to cart "+ Item +"\" and //button[@data-automation-id = 'addToCartBtn']]";
                int Ammount = Quantity;
                // Find the search bar and enter value
                WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));

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

                if (Ammount > 1)
                {
                    await Task.Delay(2000);
                    var input2 = webDriver.FindElement(By.XPath("//a[@aria-label=" + "\"" + "View details " + Item + "\"]|//input[@data-automation-id = 'quantityInput']"));
                    input2.SendKeys(Keys.Backspace);
                    input2.Click();
                    input2.SendKeys(Ammount.ToString());
                    await Task.Delay(2000);
                    input2.SendKeys(Keys.Enter);

                }
                //await WalmartCheckoutAsync();
            }   
            else if (URL == "https://www.amazon.com/")
                {
                    // Navigate to Amazon
                    webDriver.Url = URL;
                    WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
                string XpathStr = $"//a[@class = 'a-link-normal a-text-normal'] //span[contains(text()," + "\"" + ItemName + "\" )]";

                    await Task.Delay(1000);
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@id='twotabsearchtextbox']")));
                    var input = webDriver.FindElement(By.XPath("//input[@id='twotabsearchtextbox']"));
                    await Task.Delay(2000);
                    input.SendKeys(ItemName);

                    var click = webDriver.FindElement(By.XPath("//input[@id = 'nav-search-submit-button']"));
                    await Task.Delay(2000);
                    click.Click();

                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(XpathStr)));
                    var click2 = webDriver.FindElement(By.XPath(XpathStr));
                    await Task.Delay(2000);
                    click2.Click();

                    var click3 = webDriver.FindElement(By.XPath("//input[@id = 'add-to-cart-button']"));
                    await Task.Delay(2000);
                    click3.Click();
                await AmazonCheckoutAsync();
            }

        }

        public async Task WalmartCheckoutAsync()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'checkoutBtn']")));
            var click3 = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'checkoutBtn']"));
            click3.Click();
            await Task.Delay(1000);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'checkoutLink']")));
            var click4 = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'checkoutLink']"));
            click4.Click();
            await Task.Delay(1000);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'btn-slot-date-3']")));
            var click5 = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'btn-slot-date-3']"));
            click5.Click();
            await Task.Delay(1000);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[@data-automation-id = 'radio-slot-time-1']")));
            var click6 = webDriver.FindElement(By.XPath("//input[@data-automation-id = 'radio-slot-time-1']"));
            click6.Click();
            await Task.Delay(1000);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'btn-book-slot-save']")));
            var click7 = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'btn-book-slot-save']"));
            click7.Click();
            await Task.Delay(1000); 

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@data-automation-id = 'btn-review-place-order']")));
            var click8 = webDriver.FindElement(By.XPath("//button[@data-automation-id = 'btn-review-place-order']"));
            click8.Click();
            await Task.Delay(1000);
        }
        public async Task AmazonCheckoutAsync()
        {
            WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
            var click = webDriver.FindElement(By.XPath("//span[@id = 'hlb-ptc-btn']|//a[@id = 'checkoutBtn']"));
            click.Click();
            await Task.Delay(1000);
        }
    }
}

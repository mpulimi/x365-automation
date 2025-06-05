using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.DevTools.V134.Profiler;
using OpenQA.Selenium.Interactions;


class Program
{
    static void Main()
    {
    Start:
        // Build configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string loginUrl = config["AppSettings:LoginUrl"];
        string actionUrl = config["AppSettings:ActionUrl"];
        string email = config["AppSettings:email"];
        string password = config["AppSettings:password"];
        int interval = Convert.ToInt32(config["AppSettings:intervalInMinutes"]);
        Boolean closePopups = Convert.ToBoolean(config["AppSettings:closePopups"]);

        ChromeOptions options = new ChromeOptions();
        // Uncomment for headless mode
        // options.AddArgument("--headless");
        // options.AddArgument("--headless=new"); // Use "--headless=new" for newer versions
        // options.AddArgument("--window-size=1920,1080"); // Optional: simulate screen size
        // options.AddArgument("--disable-gpu"); // Optional for stability

        IWebDriver driver = new ChromeDriver(options);

        try
        {

            // Navigate to the login page
            driver.Navigate().GoToUrl(loginUrl); // Replace with actual URL

            Thread.Sleep(2000); // Use WebDriverWait in production

            // Find the email input (first input with class 'form-control')
            var emailInput = driver.FindElements(By.CssSelector("input.form-control[type='email']"))[0];
            emailInput.SendKeys(email);

            // Find the password input (second input with class 'form-control')
            var passwordInput = driver.FindElements(By.CssSelector("input.form-control[type='password']"))[0];
            passwordInput.SendKeys(password);

            // Find and click the login button (button with class 'btn-light-primary')
            var loginButton = driver.FindElement(By.CssSelector("button.btn-light-primary"));
            loginButton.Click();
            Console.WriteLine("Logged in, current URL: " + driver.Url);
            string focusedTag = (string)((IJavaScriptExecutor)driver).ExecuteScript("return document.activeElement.tagName;");
            Console.WriteLine($"Focused element: {focusedTag}");

            Thread.Sleep(5000); // Wait for login process
                                // Create an Actions object
            Actions actions = new Actions(driver);
            if (closePopups)
            {
                // Send TAB key to shift focus
                actions.SendKeys(Keys.Tab).Perform();
                actions.SendKeys(Keys.Tab).Perform();
                actions.SendKeys(Keys.Tab).Perform();
                actions.SendKeys(Keys.Tab).Perform();
                actions.SendKeys(Keys.Enter).Perform();
                Console.WriteLine("Closing the popup.");
                Thread.Sleep(1000); // Wait for login process
            }

            driver.Navigate().GoToUrl(actionUrl);

            while (true)
            {
                try
                {
                    // Wait until the button with text 'Click here' is visible and clickable
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    //closing the popups

                    // 1. Wait until page is fully loaded
                    wait.Until(driver => ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return document.readyState").Equals("complete"));
                    Console.WriteLine("Sleeping for 5 seconds..");
                    Thread.Sleep(5 * 1000);
                    // // Perform a generic click at coordinates (10, 10)
                    // // Try clicking on the <body> element
                    // IWebElement body = driver.FindElement(By.TagName("body"));
                    // body.Click(); // Native click
                    // Console.WriteLine("Clicked on body successfully.");
                    //Actions actions = new Actions(driver);
                    if (closePopups)
                    {
                        // Send TAB key to shift focus
                        actions.SendKeys(Keys.Tab).Perform();
                        actions.SendKeys(Keys.Tab).Perform();
                        actions.SendKeys(Keys.Tab).Perform();
                        actions.SendKeys(Keys.Tab).Perform();
                        actions.SendKeys(Keys.Tab).Perform();
                        actions.SendKeys(Keys.Enter).Perform();
                        Console.WriteLine("Closing the popup.");
                        Thread.Sleep(1000); // Wait for login process
                    }

                    // var buttons = driver.FindElements(By.TagName("button"));
                    // foreach (var button in buttons)
                    // {
                    //     if (button.Text == String.Empty)
                    //     {
                    //         Console.WriteLine("button.Text " + button.Text);
                    //         button.Click();
                    //     }
                    // }

                    var timerSpan = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("next-block-timer")));
                    // Get the text
                    string timerText = timerSpan.Text;

                    if (timerText == "00:00:00")
                    {
                        Console.WriteLine("Timer matched 00:00:00 ✅");
                        Console.WriteLine("Maximize the window...");
                        driver.Manage().Window.Maximize();
                    }
                    else
                    {
                        var clainTokenButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions
                            .ElementToBeClickable(By.XPath("//button[.//span[contains(text(), 'CLAIM TOKENS')]]")));
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", clainTokenButton);
                        Thread.Sleep(1000);
                        clainTokenButton.Click();

                        Console.WriteLine($"[{DateTime.Now}] Button clicked successfully.");
                    }

                    // Wait 10 minutes (600,000 milliseconds)
                    Thread.Sleep(interval * 60 * 1000);
                }
                catch (WebDriverException ex)
                {
                    Console.WriteLine($"[{DateTime.Now}] Error clicking button: {ex.Message}");
                    if (ex.Message.Trim().ToLower().Contains("invalid session id"))
                    {
                        goto Start;
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Login failed: " + ex.Message);
        }
        finally
        {
            driver.Quit();
        }
    }
}

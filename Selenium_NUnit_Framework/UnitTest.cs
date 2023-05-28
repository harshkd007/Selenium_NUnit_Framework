using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Selenium_NUnit_Framework
{
    [TestFixture]
    public class Tests
    {
        IWebDriver driver = null;
        public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultPath = dir.Replace("bin\\Debug\\net6.0", "TestResults");

        /// <summary>
        /// Setup method gets called for each test case before each test case 
        /// </summary>
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test,Category("Smoke Testing"),Description("Verify the search functionality of youtube")]
        public void TestYouTube()
        {
           
            driver.Url = "https://www.youtube.com/";
            driver.FindElement(By.XPath("//*[@name='search_query']")).SendKeys("Ch. Shivaji Maharaj");
            driver.FindElement(By.XPath("//*[@name='search_query']")).SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            Assert.IsTrue(driver.Title.Contains("Shivaji Maharaj"));

        }

        [Test, Category("Regression Testing"), Description("Verify the filter functionality of saucedemo")]
        public void TestSauceDemo()
        {

            driver.Url = "https://www.saucedemo.com/";
            driver.FindElement(By.XPath("//input[contains(@id,'user')]")).SendKeys("standard_user");
            driver.FindElement(By.XPath("//input[@id='password']")).SendKeys("secret_sauce");
            driver.FindElement(By.XPath("//input[contains(@id,'login')]")).Click();
            Thread.Sleep(1000);
            SelectElement select = null;
            var dropdown = driver.FindElement(By.ClassName("product_sort_container"));
            select = new SelectElement(dropdown);
            select.SelectByText("Price (low to high)",true);
            Thread.Sleep(2000);
            try
            {
                // After selecting the dropdown has changes. so needed modified DOM
                dropdown = driver.FindElement(By.ClassName("product_sort_container"));
                select = new SelectElement(dropdown);
            }
            catch (Exception ex) { };
            Assert.True(select.SelectedOption.Text.Contains("Price (low to high1)"));
        }

        /// <summary>
        /// TearDown gets called for each test case after test case completion
        /// </summary>
        [TearDown]
        public void TearDown() 
        {
            if(TestContext.Error != null)
            {
                ITakesScreenshot sc = (ITakesScreenshot)driver;
                Screenshot screenshot =  sc.GetScreenshot();

                string title = TestContext.CurrentContext.Test.FullName.ToString();
                string screenshotLocation = Path.Combine(testResultPath, title + ".png");
                screenshot.SaveAsFile(screenshotLocation, ScreenshotImageFormat.Png);
            }
            driver.Quit();
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DevPilot.Tests;

[TestFixture]
public class TestBase
{
    protected IWebDriver Driver { get; private set; }
    protected WebDriverWait Wait { get; private set; }
    protected string BaseUrl { get; } = "http://localhost:5000"; // Adjust as needed

    [OneTimeSetUp]
    public void SetUpDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless"); // Run in headless mode
        Driver = new ChromeDriver(options);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        Driver?.Quit();
        Driver?.Dispose();
    }

    protected void NavigateTo(string path)
    {
        Driver.Navigate().GoToUrl($"{BaseUrl}{path}");
    }

    protected IWebElement WaitForElement(By by)
    {
        return Wait.Until(driver => driver.FindElement(by));
    }

    protected bool IsElementPresent(By by)
    {
        try
        {
            Driver.FindElement(by);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    protected string GetErrorMessage()
    {
        var errorElement = WaitForElement(By.CssSelector(".error-message"));
        return errorElement.Text;
    }
} 
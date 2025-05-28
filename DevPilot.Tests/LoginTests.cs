using NUnit.Framework;
using OpenQA.Selenium;

namespace DevPilot.Tests;

[TestFixture]
public class LoginTests : TestBase
{
    private const string LoginPath = "/Account/Login";

    private void EnterCredentials(string username, string password)
    {
        var usernameInput = WaitForElement(By.Id("Username"));
        var passwordInput = WaitForElement(By.Id("Password"));

        usernameInput.Clear();
        usernameInput.SendKeys(username);
        passwordInput.Clear();
        passwordInput.SendKeys(password);
    }

    private void SubmitLoginForm()
    {
        var submitButton = WaitForElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();
    }

    [Test]
    public void ValidCredentials_ShouldRedirectToDashboard()
    {
        // Given
        NavigateTo(LoginPath);
        EnterCredentials("validuser@example.com", "ValidPassword123!");

        // When
        SubmitLoginForm();

        // Then
        Assert.That(Driver.Url, Is.EqualTo($"{BaseUrl}/Dashboard"));
    }

    [Test]
    public void InvalidCredentials_ShouldShowErrorMessage()
    {
        // Given
        NavigateTo(LoginPath);
        EnterCredentials("invalid@example.com", "WrongPassword123!");

        // When
        SubmitLoginForm();

        // Then
        var errorMessage = GetErrorMessage();
        Assert.That(errorMessage, Is.EqualTo("Invalid username or password"));
    }

    [Test]
    [TestCase("", "Password123!")] // Empty username
    [TestCase("user@example.com", "")] // Empty password
    [TestCase("", "")] // Both empty
    public void EmptyCredentials_ShouldShowErrorMessage(string username, string password)
    {
        // Given
        NavigateTo(LoginPath);
        EnterCredentials(username, password);

        // When
        SubmitLoginForm();

        // Then
        var errorMessage = GetErrorMessage();
        Assert.That(errorMessage, Is.EqualTo("Username and password are required"));
    }
} 
using NUnit.Framework;

namespace DevPilot.TDD.C.Tests;

[TestFixture]
public class LoginTests
{
    private IAuthenticationService _authService;
    private INavigationService _navigationService;

    [SetUp]
    public void Setup()
    {
        _authService = new AuthenticationService();
        _navigationService = new NavigationService();
    }

    [Test]
    public void ValidCredentials_ShouldAuthenticateAndRedirectToDashboard()
    {
        // Arrange
        var username = "validuser@example.com";
        var password = "ValidPassword123!";

        // Act
        var result = _authService.Login(username, password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsAuthenticated, Is.True);
            Assert.That(_navigationService.CurrentPath, Is.EqualTo("/Dashboard"));
        });
    }

    [Test]
    public void InvalidCredentials_ShouldShowErrorMessage()
    {
        // Arrange
        var username = "invalid@example.com";
        var password = "WrongPassword123!";

        // Act
        var result = _authService.Login(username, password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsAuthenticated, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid username or password"));
        });
    }

    [Test]
    [TestCase("", "Password123!", TestName = "Empty Username")]
    [TestCase("user@example.com", "", TestName = "Empty Password")]
    [TestCase("", "", TestName = "Both Empty")]
    public void EmptyCredentials_ShouldShowErrorMessage(string username, string password)
    {
        // Arrange - Done through parameters

        // Act
        var result = _authService.Login(username, password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsAuthenticated, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Username and password are required"));
        });
    }
} 
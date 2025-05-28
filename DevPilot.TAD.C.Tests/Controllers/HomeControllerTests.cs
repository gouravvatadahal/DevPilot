using DevPilot.Controllers;
using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DevPilot.TAD.C.Tests.Controllers;

[TestFixture]
public class HomeControllerTests
{
    private HomeController _controller;
    private Mock<IFeatureManager> _featureManager;
    private Mock<HttpContextBase> _httpContext;
    private Mock<HttpApplicationStateBase> _httpApplicationState;

    [SetUp]
    public void Setup()
    {
        _featureManager = new Mock<IFeatureManager>();
        _httpContext = new Mock<HttpContextBase>();
        _httpApplicationState = new Mock<HttpApplicationStateBase>();

        _httpApplicationState.Setup(x => x["FeatureManager"]).Returns(_featureManager.Object);
        _httpContext.Setup(x => x.Application).Returns(_httpApplicationState.Object);

        _controller = new HomeController();
        _controller.ControllerContext = new ControllerContext(_httpContext.Object, new RouteData(), _controller);
    }

    [Test]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
    }

    [Test]
    public async Task Users_WhenNewFeatureEnabled_RedirectsToUserList()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Users();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult)?.Url, Is.EqualTo("~/UserList.aspx"));
        });
    }

    [Test]
    public async Task Users_WhenNewFeatureDisabled_ReturnsView()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Users();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
    }

    [Test]
    public async Task Users_WhenExceptionOccurs_RedirectsToError()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Users();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult)?.Url, Is.EqualTo("~/Error.asp"));
        });
    }

    [Test]
    public async Task Features_WhenNewFeatureTestEnabled_RedirectsToUserList()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeatureTest"))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Features();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult)?.Url, Is.EqualTo("~/UserList.aspx"));
        });
    }

    [Test]
    public async Task Features_WhenNewFeatureTestDisabled_RedirectsToError()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeatureTest"))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Features();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult)?.Url, Is.EqualTo("~/Error.asp"));
        });
    }

    [Test]
    public async Task Features_WhenExceptionOccurs_RedirectsToError()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeatureTest"))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Features();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectResult>());
            Assert.That((result as RedirectResult)?.Url, Is.EqualTo("~/Error.asp"));
        });
    }

    [Test]
    public void Login_Get_ReturnsViewResult()
    {
        // Act
        var result = _controller.Login();

        // Assert
        Assert.That(result, Is.TypeOf<ViewResult>());
    }

    [Test]
    public void Login_Post_WithValidCredentials_RedirectsToIndex()
    {
        // Arrange
        var username = "admin";
        var password = "password";

        // Act
        var result = _controller.Login(username, password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult?.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(redirectResult?.RouteValues["controller"], Is.EqualTo("Home"));
        });
    }

    [Test]
    public void Login_Post_WithInvalidCredentials_ReturnsViewWithModelError()
    {
        // Arrange
        var username = "invalid";
        var password = "invalid";

        // Act
        var result = _controller.Login(username, password);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(_controller.ModelState.IsValid, Is.False);
            Assert.That(_controller.ModelState[""]?.Errors[0].ErrorMessage, Is.EqualTo("Invalid username or password"));
        });
    }

    [Test]
    public void Logout_RedirectsToLogin()
    {
        // Act
        var result = _controller.Logout();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = result as RedirectToRouteResult;
            Assert.That(redirectResult?.RouteValues["action"], Is.EqualTo("Login"));
            Assert.That(redirectResult?.RouteValues["controller"], Is.EqualTo("Home"));
        });
    }
} 
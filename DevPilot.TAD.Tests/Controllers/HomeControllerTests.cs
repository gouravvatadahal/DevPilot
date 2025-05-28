using DevPilot.Controllers;
using DevPilot.TAD.Tests.Controllers.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace DevPilot.TAD.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private TestHttpContext _httpContext;
        private TestSession _session;
        private TestFeatureManager _featureManager;

        [SetUp]
        public void Setup()
        {
            _featureManager = new TestFeatureManager();
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller = new HomeController();
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);
        }

        [Test]
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Users_WhenFeatureEnabled_RedirectsToUserList()
        {
            // Arrange
            _featureManager = new TestFeatureManager(isEnabled: true);
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Users();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("~/UserList.aspx", redirectResult.Url);
        }

        [Test]
        public async Task Users_WhenFeatureDisabled_ReturnsView()
        {
            // Arrange
            _featureManager = new TestFeatureManager(isEnabled: false);
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Users();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Users_WhenExceptionOccurs_RedirectsToError()
        {
            // Arrange
            _featureManager = new TestFeatureManager(exception: new Exception("Test exception"));
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Users();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("~/Error.asp", redirectResult.Url);
        }

        [Test]
        public async Task Features_WhenFeatureEnabled_RedirectsToUserList()
        {
            // Arrange
            _featureManager = new TestFeatureManager(isEnabled: true);
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Features();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("~/UserList.aspx", redirectResult.Url);
        }

        [Test]
        public async Task Features_WhenFeatureDisabled_RedirectsToError()
        {
            // Arrange
            _featureManager = new TestFeatureManager(isEnabled: false);
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Features();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("~/Error.asp", redirectResult.Url);
        }

        [Test]
        public async Task Features_WhenExceptionOccurs_RedirectsToError()
        {
            // Arrange
            _featureManager = new TestFeatureManager(exception: new Exception("Test exception"));
            _session = new TestSession(_featureManager);
            _httpContext = new TestHttpContext(_session);
            _controller.ControllerContext = new TestControllerContext(_httpContext, new RouteData(), _controller);

            // Act
            var result = await _controller.Features();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirectResult = result as RedirectResult;
            Assert.AreEqual("~/Error.asp", redirectResult.Url);
        }

        [Test]
        public void Login_Get_ReturnsViewResult()
        {
            // Act
            var result = _controller.Login();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Login_Post_WithValidCredentials_RedirectsToIndex()
        {
            // Arrange
            _controller.ModelState.Clear();

            // Act
            var result = _controller.Login("admin", "password");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectResult = result as RedirectToRouteResult;
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
        }

        [Test]
        public void Login_Post_WithInvalidCredentials_AddsError()
        {
            // Arrange
            _controller.ModelState.Clear();

            // Act
            var result = _controller.Login("wronguser", "wrongpass");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsTrue(_controller.ModelState.ContainsKey(""));
            Assert.IsTrue(_controller.ModelState[""].Errors.Any(e => e.ErrorMessage == "Invalid username or password"));
        }

        [Test]
        public void Login_Post_WithInvalidModel_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("username", "Required");
            _controller.ModelState.AddModelError("password", "Required");

            // Act
            var result = _controller.Login("", "");

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }

        [Test]
        public void Logout_SignsOutAndRedirectsToLogin()
        {
            // Act
            var result = _controller.Logout();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            var redirectResult = result as RedirectToRouteResult;
            Assert.AreEqual("Login", redirectResult.RouteValues["action"]);
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
        }
    }
}

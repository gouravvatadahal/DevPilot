using DevPilot.API;
using DevPilot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Results;

namespace DevPilot.TAD.WS.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private UsersController _controller;
        private Mock<HttpRequestMessage> _mockRequest;
        private Mock<ApiController> _mockApiController;

        [TestInitialize]
        public void Initialize()
        {
            _mockRequest = new Mock<HttpRequestMessage>();
            _mockApiController = new Mock<ApiController>();
            _mockApiController.Setup(x => x.Request).Returns(_mockRequest.Object);
            _controller = new UsersController();
        }

        [TestMethod]
        public void Get_ShouldReturnListOfUsers()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(3, result.Content.Count);
            Assert.AreEqual(1, result.Content[0].Id);
            Assert.AreEqual("Alice", result.Content[0].Name);
            Assert.AreEqual("Admin", result.Content[0].AccessLevel);
            Assert.AreEqual(2, result.Content[1].Id);
            Assert.AreEqual("Bob", result.Content[1].Name);
            Assert.AreEqual("Editor", result.Content[1].AccessLevel);
            Assert.AreEqual(3, result.Content[2].Id);
            Assert.AreEqual("Charlie", result.Content[2].Name);
            Assert.AreEqual("Viewer", result.Content[2].AccessLevel);
        }

        [TestMethod]
        public void Get_ShouldReturnOkResult()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<Users>>));
        }

        [TestMethod]
        public void Get_ShouldReturnCorrectAccessLevels()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.IsNotNull(result);
            var users = result.Content;
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Admin"));
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Editor"));
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Viewer"));
        }

        [TestMethod]
        public void Get_ShouldReturnSequentialIds()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.IsNotNull(result);
            var users = result.Content;
            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(i + 1, users[i].Id);
            }
        }

        [TestMethod]
        public void Get_ShouldReturnUsersInCorrectOrder()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.IsNotNull(result);
            var users = result.Content;
            Assert.AreEqual("Alice", users[0].Name);
            Assert.AreEqual("Bob", users[1].Name);
            Assert.AreEqual("Charlie", users[2].Name);
        }
    }
}

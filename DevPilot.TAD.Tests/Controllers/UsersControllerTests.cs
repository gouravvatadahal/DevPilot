using DevPilot.Models;
using DevPilot.API;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Http.Results;
using System.Web.Http;

namespace DevPilot.TAD.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTests
    {
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new UsersController();
        }

        [Test]
        public void Get_ReturnsListOfUsers()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(3, result.Content.Count);
            
            var users = result.Content;
            Assert.AreEqual(1, users[0].Id);
            Assert.AreEqual("Alice", users[0].Name);
            Assert.AreEqual("Admin", users[0].AccessLevel);
            
            Assert.AreEqual(2, users[1].Id);
            Assert.AreEqual("Bob", users[1].Name);
            Assert.AreEqual("Editor", users[1].AccessLevel);
            
            Assert.AreEqual(3, users[2].Id);
            Assert.AreEqual("Charlie", users[2].Name);
            Assert.AreEqual("Viewer", users[2].AccessLevel);
        }

        [Test]
        public void Get_ReturnsOkResult()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<Users>>>(result);
        }

        [Test]
        public void Get_ReturnsCorrectAccessLevels()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.NotNull(result);
            var users = result.Content;
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Admin"));
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Editor"));
            Assert.IsTrue(users.Exists(u => u.AccessLevel == "Viewer"));
        }

        [Test]
        public void Get_ReturnsSequentialIds()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.NotNull(result);
            var users = result.Content;
            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(i + 1, users[i].Id);
            }
        }

        [Test]
        public void Get_ReturnsUsersInCorrectOrder()
        {
            // Act
            var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

            // Assert
            Assert.NotNull(result);
            var users = result.Content;
            Assert.AreEqual("Alice", users[0].Name);
            Assert.AreEqual("Bob", users[1].Name);
            Assert.AreEqual("Charlie", users[2].Name);
        }
    }
}

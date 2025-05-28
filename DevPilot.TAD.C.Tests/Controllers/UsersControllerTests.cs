using DevPilot.API;
using DevPilot.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace DevPilot.TAD.C.Tests.Controllers;

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
    public void Get_ReturnsOkResultWithUsers()
    {
        // Act
        var result = _controller.Get() as OkNegotiatedContentResult<List<Users>>;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.Not.Null);
            Assert.That(result.Content, Has.Count.EqualTo(3));

            // Verify first user
            var firstUser = result.Content.First();
            Assert.That(firstUser.Id, Is.EqualTo(1));
            Assert.That(firstUser.Name, Is.EqualTo("Alice"));
            Assert.That(firstUser.AccessLevel, Is.EqualTo("Admin"));

            // Verify second user
            var secondUser = result.Content.Skip(1).First();
            Assert.That(secondUser.Id, Is.EqualTo(2));
            Assert.That(secondUser.Name, Is.EqualTo("Bob"));
            Assert.That(secondUser.AccessLevel, Is.EqualTo("Editor"));

            // Verify third user
            var thirdUser = result.Content.Last();
            Assert.That(thirdUser.Id, Is.EqualTo(3));
            Assert.That(thirdUser.Name, Is.EqualTo("Charlie"));
            Assert.That(thirdUser.AccessLevel, Is.EqualTo("Viewer"));
        });
    }
} 
using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;

namespace DevPilot.TDD.C.Tests;

[TestFixture]
public class UsersTests
{
    private Mock<IFeatureManager> _featureManager;
    private INavigationService _navigationService;
    private IUsersService _usersService;

    [SetUp]
    public void Setup()
    {
        _featureManager = new Mock<IFeatureManager>();
        _navigationService = new NavigationService();
        _usersService = new UsersService(_featureManager.Object, _navigationService);
    }

    [Test]
    public async Task NewFeatureEnabled_ShouldRedirectToUserList()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);

        // Act
        await _usersService.NavigateToUsers();

        // Assert
        Assert.That(_navigationService.CurrentPath, Is.EqualTo("/Home/UserList"));
    }

    [Test]
    public async Task NewFeatureDisabled_ShouldShowNoAccessMessage()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);

        // Act
        var result = await _usersService.NavigateToUsers();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.ViewName, Is.EqualTo("Users"));
            Assert.That(result.ErrorMessage, Is.EqualTo("No access to this feature"));
        });
    }
} 
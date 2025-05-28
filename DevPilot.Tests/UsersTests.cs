using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace DevPilot.Tests;

[TestFixture]
public class UsersTests : TestBase
{
    private const string UsersPath = "/Home/Users";
    private Mock<IFeatureManager> _featureManager;

    [SetUp]
    public void Setup()
    {
        _featureManager = new Mock<IFeatureManager>();
    }

    [Test]
    public async Task NewFeatureEnabled_ShouldRedirectToUserList()
    {
        // Given
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);
        NavigateTo(UsersPath);

        // When - Navigation happens in the Given step

        // Then
        Assert.That(Driver.Url, Is.EqualTo($"{BaseUrl}/Home/UserList"));
    }

    [Test]
    public async Task NewFeatureDisabled_ShouldShowNoAccessMessage()
    {
        // Given
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);
        NavigateTo(UsersPath);

        // When - Navigation happens in the Given step

        // Then
        var pageTitle = WaitForElement(By.TagName("h1")).Text;
        Assert.That(pageTitle, Is.EqualTo("Users"));

        var errorMessage = GetErrorMessage();
        Assert.That(errorMessage, Is.EqualTo("No access to this feature"));
    }
} 
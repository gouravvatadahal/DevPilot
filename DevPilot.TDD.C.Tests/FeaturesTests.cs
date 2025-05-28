using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;

namespace DevPilot.TDD.C.Tests;

[TestFixture]
public class FeaturesTests
{
    private Mock<IFeatureManager> _featureManager;
    private IFeaturesService _featuresService;

    [SetUp]
    public void Setup()
    {
        _featureManager = new Mock<IFeatureManager>();
        _featuresService = new FeaturesService(_featureManager.Object);
    }

    [Test]
    public async Task AllRequiredFeaturesEnabled_ShouldDisplayFeaturesList()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("newFeatureEnabled"))
            .ReturnsAsync(false);
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);
        _featureManager.Setup(fm => fm.IsEnabledAsync("BetaFeature"))
            .ReturnsAsync(true);

        // Act
        var result = await _featuresService.GetFeaturesPage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.True);
            Assert.That(result.Title, Is.EqualTo("Features"));
            Assert.That(result.Features, Is.Not.Empty);
        });
    }

    [Test]
    public async Task BetaFeatureDisabled_ShouldShowError()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("newFeatureEnabled"))
            .ReturnsAsync(false);
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);
        _featureManager.Setup(fm => fm.IsEnabledAsync("BetaFeature"))
            .ReturnsAsync(false);

        // Act
        var result = await _featuresService.GetFeaturesPage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Oops! Something went wrong. An unexpected error occurred. Please try again later."));
        });
    }

    [Test]
    public async Task NewFeatureDisabled_ShouldShowNoAccessMessage()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync("newFeatureEnabled"))
            .ReturnsAsync(false);
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);

        // Act
        var result = await _featuresService.GetFeaturesPage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No access to this feature."));
        });
    }

    [Test]
    public async Task ProcessingError_ShouldShowErrorMessage()
    {
        // Arrange
        _featureManager.Setup(fm => fm.IsEnabledAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Simulated error"));

        // Act
        var result = await _featuresService.GetFeaturesPage();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Oops! Something went wrong. An unexpected error occurred. Please try again later."));
        });
    }
} 
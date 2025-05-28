using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DevPilot.BDD.C.Tests.Steps;

[Binding]
public class FeaturesSteps
{
    private readonly Mock<IFeatureManager> _featureManager;
    private readonly IFeaturesService _featuresService;
    private FeaturesResult? _result;
    private bool _simulateError;

    public FeaturesSteps(IFeatureManager featureManager, IFeaturesService featuresService)
    {
        _featureManager = new Mock<IFeatureManager>();
        _featuresService = featuresService;
    }

    [Given(@"newFeatureEnabled is disabled")]
    public void GivenNewFeatureEnabledIsDisabled()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("newFeatureEnabled"))
            .ReturnsAsync(false);
    }

    [Given(@"NewFeature is enabled")]
    public void GivenNewFeatureIsEnabled()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);
    }

    [Given(@"NewFeature is disabled")]
    public void GivenNewFeatureIsDisabled()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);
    }

    [Given(@"BetaFeature is enabled")]
    public void GivenBetaFeatureIsEnabled()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("BetaFeature"))
            .ReturnsAsync(true);
    }

    [Given(@"BetaFeature is disabled")]
    public void GivenBetaFeatureIsDisabled()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("BetaFeature"))
            .ReturnsAsync(false);
    }

    [Given(@"an error occurs while processing the request")]
    public void GivenAnErrorOccursWhileProcessingTheRequest()
    {
        _simulateError = true;
    }

    [When(@"the user navigates to the Features page")]
    public async Task WhenTheUserNavigatesToTheFeaturesPage()
    {
        _result = await _featuresService.GetFeaturesPage();
    }

    [Then(@"the Features page is displayed with a list of available features")]
    public void ThenTheFeaturesPageIsDisplayedWithAListOfAvailableFeatures()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result?.Success, Is.True);
            Assert.That(_result?.Features, Is.Not.Empty);
        });
    }

    [Then(@"the page title is ""(.*)""")]
    public void ThenThePageTitleIs(string title)
    {
        Assert.That(_result?.Title, Is.EqualTo(title));
    }

    [Then(@"an error message is displayed with text ""(.*)""")]
    public void ThenAnErrorMessageIsDisplayedWithText(string message)
    {
        Assert.Multiple(() =>
        {
            Assert.That(_result?.Success, Is.False);
            Assert.That(_result?.ErrorMessage, Is.EqualTo(message));
        });
    }
} 
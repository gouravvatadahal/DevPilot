using TechTalk.SpecFlow;
using Xunit;

namespace DevPilot.BDD.WS.Tests.Steps
{
    [Binding]
    public class FeaturesSteps
    {
        private readonly FeatureManager _featureManager;
        private readonly FeaturesPage _featuresPage;
        private readonly ErrorMessage _errorMessage;

        public FeaturesSteps()
        {
            _featureManager = new FeatureManager();
            _featuresPage = new FeaturesPage();
            _errorMessage = new ErrorMessage();
        }

        [Given(@"newFeatureEnabled is disabled")]
        public void GivenNewFeatureEnabledIsDisabled()
        {
            _featureManager.DisableFeature("newFeatureEnabled");
        }

        [Given(@"NewFeature is enabled")]
        public void GivenNewFeatureIsEnabled()
        {
            _featureManager.EnableFeature("NewFeature");
        }

        [Given(@"BetaFeature is enabled")]
        public void GivenBetaFeatureIsEnabled()
        {
            _featureManager.EnableFeature("BetaFeature");
        }

        [Given(@"BetaFeature is disabled")]
        public void GivenBetaFeatureIsDisabled()
        {
            _featureManager.DisableFeature("BetaFeature");
        }

        [Given(@"an error occurs while processing the request to the Features page")]
        public void GivenAnErrorOccursWhileProcessingTheRequestToTheFeaturesPage()
        {
            _featureManager.SimulateError();
        }

        [When(@"the user navigates to the Features page")]
        public void WhenTheUserNavigatesToTheFeaturesPage()
        {
            _featuresPage.NavigateTo();
        }

        [Then(@"the Features page is displayed with a list of available features")]
        public void ThenTheFeaturesPageIsDisplayedWithAListOfAvailableFeatures()
        {
            Assert.True(_featuresPage.HasFeatures());
        }

        [Then(@"the page title is "Features"")]
        public void ThenThePageTitleIsFeatures()
        {
            Assert.Equal("Features", _featuresPage.GetTitle());
        }

        [Then(@"an error message is "(.*)"")]
        public void ThenAnErrorMessageIs(string message)
        {
            Assert.Equal(message, _errorMessage.GetMessage());
        }

        [Then(@"an error message is displayed indicating that an error occurred")]
        public void ThenAnErrorMessageIsDisplayedIndicatingThatAnErrorOccurred()
        {
            Assert.True(_errorMessage.IsDisplayed());
        }
    }

    public class FeaturesPage
    {
        public void NavigateTo() => // Implementation
        public bool HasFeatures() => // Implementation
        public string GetTitle() => // Implementation
    }

    public class ErrorMessage
    {
        public string GetMessage() => // Implementation
        public bool IsDisplayed() => // Implementation
    }
}

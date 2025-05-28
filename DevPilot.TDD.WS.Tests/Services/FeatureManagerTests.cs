using System.Collections.Generic;
using Moq;
using Xunit;

namespace DevPilot.TDD.WS.Tests.Services
{
    public class FeatureManagerTests
    {
        private readonly FeatureManager _featureManager;
        private readonly Mock<IFeatureStore> _featureStore;

        public FeatureManagerTests()
        {
            _featureStore = new Mock<IFeatureStore>();
            _featureManager = new FeatureManager(_featureStore.Object);
        }

        [Fact]
        public void IsFeatureEnabled_ShouldReturnTrue_WhenFeatureIsEnabled()
        {
            // Arrange
            var featureName = "NewFeature";
            _featureStore.Setup(store => store.GetFeatureStatus(featureName)).Returns(true);

            // Act
            var isEnabled = _featureManager.IsFeatureEnabled(featureName);

            // Assert
            Assert.True(isEnabled);
            _featureStore.Verify(store => store.GetFeatureStatus(featureName), Times.Once);
        }

        [Fact]
        public void IsFeatureEnabled_ShouldReturnFalse_WhenFeatureIsDisabled()
        {
            // Arrange
            var featureName = "NewFeature";
            _featureStore.Setup(store => store.GetFeatureStatus(featureName)).Returns(false);

            // Act
            var isEnabled = _featureManager.IsFeatureEnabled(featureName);

            // Assert
            Assert.False(isEnabled);
            _featureStore.Verify(store => store.GetFeatureStatus(featureName), Times.Once);
        }

        [Fact]
        public void GetEnabledFeatures_ShouldReturnListOfEnabledFeatures()
        {
            // Arrange
            var enabledFeatures = new List<string> { "NewFeature", "BetaFeature" };
            _featureStore.Setup(store => store.GetEnabledFeatures()).Returns(enabledFeatures);

            // Act
            var features = _featureManager.GetEnabledFeatures();

            // Assert
            Assert.NotNull(features);
            Assert.Equal(2, features.Count);
            Assert.Contains("NewFeature", features);
            Assert.Contains("BetaFeature", features);
        }

        [Fact]
        public void NoFeaturesEnabled_ShouldReturnEmptyList()
        {
            // Arrange
            _featureStore.Setup(store => store.GetEnabledFeatures()).Returns(new List<string>());

            // Act
            var features = _featureManager.GetEnabledFeatures();

            // Assert
            Assert.NotNull(features);
            Assert.Empty(features);
        }
    }

    public class FeatureManager
    {
        private readonly IFeatureStore _featureStore;

        public FeatureManager(IFeatureStore featureStore)
        {
            _featureStore = featureStore;
        }

        public bool IsFeatureEnabled(string featureName)
        {
            return _featureStore.GetFeatureStatus(featureName);
        }

        public List<string> GetEnabledFeatures()
        {
            return _featureStore.GetEnabledFeatures();
        }
    }

    public interface IFeatureStore
    {
        bool GetFeatureStatus(string featureName);
        List<string> GetEnabledFeatures();
    }
}

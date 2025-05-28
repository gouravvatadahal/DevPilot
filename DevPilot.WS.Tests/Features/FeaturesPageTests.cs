using System.Net.Http.Json;
using Xunit;

namespace DevPilot.WS.Tests.Features
{
    public class FeaturesPageTests
    {
        private readonly HttpClient _httpClient;
        private readonly FeatureManager _featureManager;

        public FeaturesPageTests()
        {
            _httpClient = new HttpClient();
            _featureManager = new FeatureManager();
        }

        [Fact]
        public async Task SuccessfulNavigation_WithFeaturesEnabled_ShouldDisplayFeaturesList()
        {
            // Given
            _featureManager.DisableFeature("newFeatureEnabled");
            _featureManager.EnableFeature("NewFeature");
            _featureManager.EnableFeature("BetaFeature");

            // When
            var response = await _httpClient.GetAsync("/Features");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("Features", responseContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("NewFeature", responseContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("BetaFeature", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task NoFeaturesAvailable_ShouldShowErrorMessage()
        {
            // Given
            _featureManager.DisableFeature("newFeatureEnabled");
            _featureManager.EnableFeature("NewFeature");
            _featureManager.DisableFeature("BetaFeature");

            // When
            var response = await _httpClient.GetAsync("/Features");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("Something went wrong", responseContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Please try again later", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task NoFeaturesEnabled_ShouldShowNoAccessMessage()
        {
            // Given
            _featureManager.DisableFeature("newFeatureEnabled");
            _featureManager.DisableFeature("NewFeature");

            // When
            var response = await _httpClient.GetAsync("/Features");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("No access to this feature", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ErrorHandling_ShouldShowErrorMessage()
        {
            // Given
            // Simulate error condition
            _httpClient.DefaultRequestHeaders.Add("X-Force-Error", "true");

            // When
            var response = await _httpClient.GetAsync("/Features");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("Something went wrong", responseContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Please try again later", responseContent, StringComparison.OrdinalIgnoreCase);
        }
    }
}

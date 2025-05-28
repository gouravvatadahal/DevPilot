using System.Net.Http.Json;
using Xunit;

namespace DevPilot.WS.Tests.Features
{
    public class UsersFeatureTests
    {
        private readonly HttpClient _httpClient;
        private readonly FeatureManager _featureManager;

        public UsersFeatureTests()
        {
            _httpClient = new HttpClient();
            _featureManager = new FeatureManager();
        }

        [Fact]
        public async Task NewFeatureEnabled_ShouldRedirectToUserList()
        {
            // Given
            _featureManager.EnableFeature("NewFeature");

            // When
            var response = await _httpClient.GetAsync("/Home/Users");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("/Home/UserList", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task NewFeatureDisabled_ShouldShowNoAccessMessage()
        {
            // Given
            _featureManager.DisableFeature("NewFeature");

            // When
            var response = await _httpClient.GetAsync("/Home/Users");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("No access to this feature", responseContent, StringComparison.OrdinalIgnoreCase);
        }
    }

    public class FeatureManager
    {
        public void EnableFeature(string featureName)
        {
            // Implementation will be provided by the feature management system
        }

        public void DisableFeature(string featureName)
        {
            // Implementation will be provided by the feature management system
        }
    }
}

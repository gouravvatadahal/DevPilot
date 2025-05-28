using System.Net.Http.Json;
using Xunit;

namespace DevPilot.WS.Tests.Features
{
    public class LoginFeatureTests
    {
        private readonly HttpClient _httpClient;

        public LoginFeatureTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task ValidCredentials_ShouldAuthenticateAndRedirectToDashboard()
        {
            // Given
            var validCredentials = new 
            {
                username = "validuser",
                password = "validpassword"
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", validCredentials);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("dashboard", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task InvalidCredentials_ShouldDisplayErrorMessage()
        {
            // Given
            var invalidCredentials = new
            {
                username = "invaliduser",
                password = "invalidpassword"
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", invalidCredentials);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("invalid credentials", responseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task EmptyCredentials_ShouldDisplayRequiredMessage()
        {
            // Given
            var emptyCredentials = new
            {
                username = "",
                password = ""
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", emptyCredentials);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("credentials are required", responseContent, StringComparison.OrdinalIgnoreCase);
        }
    }
}

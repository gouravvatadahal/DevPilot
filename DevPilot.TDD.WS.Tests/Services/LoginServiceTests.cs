using System.Security.Cryptography;
using System.Text;
using Moq;
using Xunit;

namespace DevPilot.TDD.WS.Tests.Services
{
    public class LoginServiceTests
    {
        private readonly LoginService _loginService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IAuthenticationService> _authService;

        public LoginServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _authService = new Mock<IAuthenticationService>();
            _loginService = new LoginService(_userRepository.Object, _authService.Object);
        }

        [Fact]
        public void Login_ValidCredentials_ShouldAuthenticateAndReturnToken()
        {
            // Arrange
            var validUsername = "testuser";
            var validPassword = "password123";
            var hashedPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(validPassword)));
            var user = new User { Username = validUsername, PasswordHash = hashedPassword };

            _userRepository.Setup(repo => repo.GetUser(validUsername)).Returns(user);
            _authService.Setup(service => service.GenerateToken(user)).Returns("test-token");

            // Act
            var result = _loginService.Login(validUsername, validPassword);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-token", result.Token);
            _authService.Verify(service => service.GenerateToken(user), Times.Once);
        }

        [Fact]
        public void Login_InvalidCredentials_ShouldReturnNull()
        {
            // Arrange
            var invalidUsername = "invaliduser";
            var invalidPassword = "wrongpassword";
            _userRepository.Setup(repo => repo.GetUser(invalidUsername)).Returns((User)null);

            // Act
            var result = _loginService.Login(invalidUsername, invalidPassword);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Login_EmptyCredentials_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _loginService.Login("", ""));
            Assert.Throws<ArgumentException>(() => _loginService.Login(null, null));
        }
    }

    public class LoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authService;

        public LoginService(IUserRepository userRepository, IAuthenticationService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public LoginResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Username and password are required");

            var user = _userRepository.GetUser(username);
            if (user == null)
                return null;

            var hashedPassword = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (user.PasswordHash != hashedPassword)
                return null;

            return new LoginResult
            {
                Token = _authService.GenerateToken(user)
            };
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }
    }

    public interface IUserRepository
    {
        User GetUser(string username);
    }

    public interface IAuthenticationService
    {
        string GenerateToken(User user);
    }

    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}

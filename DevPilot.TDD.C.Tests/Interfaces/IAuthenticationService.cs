namespace DevPilot.TDD.C.Tests.Interfaces;

public interface IAuthenticationService
{
    AuthenticationResult Login(string username, string password);
}

public class AuthenticationResult
{
    public bool IsAuthenticated { get; set; }
    public string? ErrorMessage { get; set; }
} 
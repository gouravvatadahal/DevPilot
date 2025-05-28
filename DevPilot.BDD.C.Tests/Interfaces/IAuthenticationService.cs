namespace DevPilot.BDD.C.Tests.Interfaces;

public interface IAuthenticationService
{
    void SetCredentials(string username, string password);
    AuthenticationResult Login();
}

public class AuthenticationResult
{
    public bool IsAuthenticated { get; set; }
    public string? ErrorMessage { get; set; }
} 
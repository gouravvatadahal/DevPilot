namespace DevPilot.BDD.C.Tests.Interfaces;

public interface IUsersService
{
    Task<UsersResult> NavigateToUsers();
}

public class UsersResult
{
    public string ViewName { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
} 
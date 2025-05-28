using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DevPilot.BDD.C.Tests.Steps;

[Binding]
public class LoginSteps
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private AuthenticationResult? _authResult;

    public LoginSteps(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
    }

    [Given(@"the user enters username ""(.*)"" and password ""(.*)""")]
    public void GivenTheUserEntersUsernameAndPassword(string username, string password)
    {
        _authService.SetCredentials(username, password);
    }

    [Given(@"the user enters empty username or password")]
    public void GivenTheUserEntersEmptyUsernameOrPassword()
    {
        _authService.SetCredentials(string.Empty, string.Empty);
    }

    [When(@"the user submits the login form")]
    public void WhenTheUserSubmitsTheLoginForm()
    {
        _authResult = _authService.Login();
    }

    [Then(@"the user is authenticated")]
    public void ThenTheUserIsAuthenticated()
    {
        Assert.That(_authResult?.IsAuthenticated, Is.True);
    }

    [Then(@"the user is redirected to the dashboard page")]
    public void ThenTheUserIsRedirectedToTheDashboardPage()
    {
        Assert.That(_navigationService.CurrentPath, Is.EqualTo("/Dashboard"));
    }

    [Then(@"an error message is displayed indicating that the credentials are invalid")]
    public void ThenAnErrorMessageIsDisplayedIndicatingThatTheCredentialsAreInvalid()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_authResult?.IsAuthenticated, Is.False);
            Assert.That(_authResult?.ErrorMessage, Is.EqualTo("Invalid username or password"));
        });
    }

    [Then(@"an error message is displayed indicating that the credentials are required")]
    public void ThenAnErrorMessageIsDisplayedIndicatingThatTheCredentialsAreRequired()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_authResult?.IsAuthenticated, Is.False);
            Assert.That(_authResult?.ErrorMessage, Is.EqualTo("Username and password are required"));
        });
    }
} 
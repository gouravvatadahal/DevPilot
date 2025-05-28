using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace DevPilot.BDD.C.Tests.Steps;

[Binding]
public class UsersSteps
{
    private readonly Mock<IFeatureManager> _featureManager;
    private readonly INavigationService _navigationService;
    private readonly IUsersService _usersService;
    private UsersResult? _result;

    public UsersSteps(IFeatureManager featureManager, INavigationService navigationService, IUsersService usersService)
    {
        _featureManager = new Mock<IFeatureManager>();
        _navigationService = navigationService;
        _usersService = usersService;
    }

    [Given(@"the NewFeature is enabled in the feature manager")]
    public void GivenTheNewFeatureIsEnabledInTheFeatureManager()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(true);
    }

    [Given(@"the NewFeature is disabled in the feature manager")]
    public void GivenTheNewFeatureIsDisabledInTheFeatureManager()
    {
        _featureManager.Setup(fm => fm.IsEnabledAsync("NewFeature"))
            .ReturnsAsync(false);
    }

    [When(@"the user navigates to the ""(.*)"" page")]
    public async Task WhenTheUserNavigatesToThePage(string path)
    {
        _result = await _usersService.NavigateToUsers();
    }

    [Then(@"the user is redirected to the ""(.*)"" page")]
    public void ThenTheUserIsRedirectedToThePage(string path)
    {
        Assert.That(_navigationService.CurrentPath, Is.EqualTo(path));
    }

    [Then(@"the user is displayed the ""(.*)"" view")]
    public void ThenTheUserIsDisplayedTheView(string viewName)
    {
        Assert.That(_result?.ViewName, Is.EqualTo(viewName));
    }

    [Then(@"a ""(.*)"" message is shown")]
    public void ThenAMessageIsShown(string message)
    {
        Assert.That(_result?.ErrorMessage, Is.EqualTo(message));
    }
} 
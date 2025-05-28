using TechTalk.SpecFlow;
using Xunit;

namespace DevPilot.BDD.WS.Tests.Steps
{
    [Binding]
    public class UsersSteps
    {
        private readonly FeatureManager _featureManager;
        private readonly UsersPage _usersPage;
        private readonly UserListPage _userListPage;

        public UsersSteps()
        {
            _featureManager = new FeatureManager();
            _usersPage = new UsersPage();
            _userListPage = new UserListPage();
        }

        [Given(@"the NewFeature is enabled in the feature manager")]
        public void GivenTheNewFeatureIsEnabledInTheFeatureManager()
        {
            _featureManager.EnableFeature("NewFeature");
        }

        [Given(@"the NewFeature is disabled in the feature manager")]
        public void GivenTheNewFeatureIsDisabledInTheFeatureManager()
        {
            _featureManager.DisableFeature("NewFeature");
        }

        [When(@"the user navigates to the /Home/Users page")]
        public void WhenTheUserNavigatesToTheHomeUsersPage()
        {
            _usersPage.NavigateTo();
        }

        [Then(@"the user is redirected to the /Home/UserList page")]
        public void ThenTheUserIsRedirectedToTheHomeUserListPage()
        {
            Assert.True(_userListPage.IsOnUserList());
        }

        [Then(@"the user is displayed the "Users" view with a "No access to this feature" message")]
        public void ThenTheUserIsDisplayedTheUsersViewWithANoAccessToThisFeatureMessage()
        {
            Assert.Equal("No access to this feature", _usersPage.GetNoAccessMessage());
        }
    }

    public class FeatureManager
    {
        public void EnableFeature(string featureName) => // Implementation
        public void DisableFeature(string featureName) => // Implementation
    }

    public class UsersPage
    {
        public void NavigateTo() => // Implementation
        public string GetNoAccessMessage() => // Implementation
    }

    public class UserListPage
    {
        public bool IsOnUserList() => // Implementation
    }
}

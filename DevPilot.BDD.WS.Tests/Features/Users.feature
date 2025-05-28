Feature: Users
    As a user
    I want to access the users page based on feature flags
    So that I can view user management options

Scenario: NewFeature Enabled
    Given the NewFeature is enabled in the feature manager
    When the user navigates to the /Home/Users page
    Then the user is redirected to the /Home/UserList page

Scenario: NewFeature Disabled
    Given the NewFeature is disabled in the feature manager
    When the user navigates to the /Home/Users page
    Then the user is displayed the "Users" view with a "No access to this feature" message

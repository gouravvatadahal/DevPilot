Feature: Users
    As a user
    I want to access the users page
    So that I can manage user accounts

Scenario: NewFeature is enabled
    Given the NewFeature is enabled in the feature manager
    When the user navigates to the "/Home/Users" page
    Then the user is redirected to the "/Home/UserList" page

Scenario: NewFeature is disabled
    Given the NewFeature is disabled in the feature manager
    When the user navigates to the "/Home/Users" page
    Then the user is displayed the "Users" view
    And a "No access to this feature" message is shown 
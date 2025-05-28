Feature: Login
    As a user
    I want to be able to login with my credentials
    So that I can access the system

Scenario: Valid Credentials
    Given the user enters valid username and password
    When the user submits the login form
    Then the user is authenticated and redirected to the dashboard page

Scenario: Invalid Credentials
    Given the user enters invalid username or password
    When the user submits the login form
    Then an error message is displayed indicating that the credentials are invalid

Scenario: Empty Credentials
    Given the user enters empty username or password
    When the user submits the login form
    Then an error message is displayed indicating that the credentials are required

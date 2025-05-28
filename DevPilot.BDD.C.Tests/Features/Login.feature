Feature: Login
    As a user
    I want to be able to log in to the system
    So that I can access my account

Scenario: Valid Credentials
    Given the user enters username "validuser@example.com" and password "ValidPassword123!"
    When the user submits the login form
    Then the user is authenticated
    And the user is redirected to the dashboard page

Scenario: Invalid Credentials
    Given the user enters username "invaliduser@example.com" and password "InvalidPassword123!"
    When the user submits the login form
    Then an error message is displayed indicating that the credentials are invalid

Scenario: Empty Credentials
    Given the user enters empty username or password
    When the user submits the login form
    Then an error message is displayed indicating that the credentials are required 
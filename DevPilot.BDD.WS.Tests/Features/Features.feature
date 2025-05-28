Feature: Features
    As a user
    I want to view available features
    So that I can understand what functionality is available

Scenario: Successful Navigation to Features Page
    Given newFeatureEnabled is disabled
    And NewFeature is enabled
    And BetaFeature is enabled
    When the user navigates to the Features page
    Then the Features page is displayed with a list of available features
    And the page title is "Features"

Scenario: No Features Available - BetaFeature Disabled
    Given newFeatureEnabled is disabled
    And NewFeature is enabled
    And BetaFeature is disabled
    When the user navigates to the Features page
    Then an error message is "Oops! Something went wrong. An unexpected error occurred. Please try again later."

Scenario: No Features Available - NewFeature Disabled
    Given newFeatureEnabled is disabled
    And NewFeature is disabled
    When the user navigates to the Features page
    Then an error message is "No access to this feature."

Scenario: Error Handling
    Given an error occurs while processing the request to the Features page
    When the user navigates to the Features page
    Then an error message is displayed indicating that an error occurred
    And the error message is "Oops! Something went wrong. An unexpected error occurred. Please try again later."

Feature: Features
    As a user
    I want to access the features page
    So that I can view available features

Scenario: Successful Navigation to Features Page
    Given newFeatureEnabled is disabled
    And NewFeature is enabled
    And BetaFeature is enabled
    When the user navigates to the Features page
    Then the Features page is displayed with a list of available features
    And the page title is "Features"

Scenario: No Features Available Due to BetaFeature Disabled
    Given newFeatureEnabled is disabled
    And NewFeature is enabled
    And BetaFeature is disabled
    When the user navigates to the Features page
    Then an error message is displayed with text "Oops! Something went wrong. An unexpected error occurred. Please try again later."

Scenario: No Access to Features
    Given newFeatureEnabled is disabled
    And NewFeature is disabled
    When the user navigates to the Features page
    Then an error message is displayed with text "No access to this feature."

Scenario: Error Handling
    Given an error occurs while processing the request
    When the user navigates to the Features page
    Then an error message is displayed with text "Oops! Something went wrong. An unexpected error occurred. Please try again later." 
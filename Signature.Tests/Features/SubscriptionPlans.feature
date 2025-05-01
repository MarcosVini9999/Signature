Feature: Subscription Plans API
  As an API consumer
  I want to create and list subscription plans
  So that I can offer plans to users

  Scenario: Create a new subscription plan
    When I POST to "/api/subscriptionplans" with body:
      """
      {
        "Title": "Gold",
        "Price": 99.99
      }
      """
    Then the response status code should be 201
    And the response JSON field "Title" should be "Gold"
    And the response JSON field "Price" should be 99.99
    And I save the "Id" from the response as "PlanId"

  Scenario: List subscription plans
    Given I have a saved "PlanId"
    When I GET to "/api/subscriptionplans"
    Then the response status code should be 200
    And the response JSON array should contain an object with:
      | Title | Price |
      | Gold  | 99.99 |

  Scenario: Retrieve a non-existent plan
    When I GET to "/api/subscriptionplans/00000000-0000-0000-0000-000000000000"
    Then the response status code should be 404
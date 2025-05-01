Feature: Subscriptions API
  As an API consumer
  I want to subscribe users to plans
  So that I can manage user subscriptions

  Scenario: Subscribe a user to a plan
    Given I have a saved "UserId"
    And I have a saved "PlanId"
    When I POST to "/api/subscriptions/subscribe?userId={UserId}&planId={PlanId}" with no body
    Then the response status code should be 200
    And the response JSON field "Message" should be "Subscription successful."
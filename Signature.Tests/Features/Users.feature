Feature: Users API
  As an API consumer
  I want to create and retrieve users
  So that I can manage user registrations

  Scenario: Create a new user
    When I POST to "/api/users" with body:
      """
      {
        "Name": "Alice",
        "Email": "alice@example.com"
      }
      """
    Then the response status code should be 201
    And the response JSON field "Name" should be "Alice"
    And the response JSON field "Email" should be "alice@example.com"
    And I save the "Id" from the response as "UserId"

  Scenario: Retrieve an existing user
    Given I have a saved "UserId"
    When I GET to "/api/users/{UserId}"
    Then the response status code should be 200
    And the response JSON field "Id" should equal the saved "UserId"
    And the response JSON field "Name" should be "Alice"
    And the response JSON field "Email" should be "alice@example.com"

  Scenario: Retrieve a non-existent user
    When I GET to "/api/users/00000000-0000-0000-0000-000000000000"
    Then the response status code should be 404
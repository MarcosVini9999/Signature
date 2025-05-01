# language: en
Feature: User Management API
  In order to allow clients to manage users
  As an API consumer
  I want to create and retrieve users

  Background:
    Given the API is running

  Scenario: Create a user with valid data
    When I send a POST request to "/api/users" with:
      | Name | Email          |
      | John | john@test.com  |
    Then the response status should be Created
    And the Location header should contain "/api/users/"
    And the response body should be a UserDto with:
      | Name  | Email         |
      | John  | john@test.com |
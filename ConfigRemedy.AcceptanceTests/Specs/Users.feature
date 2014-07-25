Feature: Users
	In order manage available users
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Get users when database is empty
	Given the database is empty
	When I get available users
	Then I should get HTTP OK
	And I should get an empty list

Scenario: Getting a user that don't exist
	Given an user named "james" exist
	When I GET an user named "bond"
	Then I should get HTTP NotFound
	And I should get an empty body

Scenario: Getting an existing user
	Given an user named "bar" exist
	When I GET an user named "bar"
	Then I should get HTTP 200
	And I should get an user model with name "bar"

Scenario: Adding user
	Given the database is empty
	When I POST a user named "james" 
	Then I should get HTTP Created
	And an user named "james" should be persisted
	And location header should contain url for "users/james"
	And I should get an user model with name "james"
	And I should get an the following JSON response: {"id":"users/james","userName":"james","displayName":"james","email":"james","passwordHashed":"_some_hashed_password_"}

Scenario: Adding duplicate user is not allowed
	Given an user named "foo" exist
	When I POST a user named "foo" 
	Then I should get HTTP Forbidden with reason "Duplicates are not allowed"
	And body should be "Duplicates are not allowed"

Scenario: Getting all users
	Given an user named "james" exist
	Given an user named "bond" exist
	When I get available users
	Then I should get HTTP 200

Feature: Environment
	In order manage available environments
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Get environment when database is empty
	Given the database is empty
	When I get available environments
	Then I should get HTTP OK
	And I should get an empty list

Scenario: Getting a environment that don't exist
	Given an environment named "dev" exist
	When I GET an environment named "prod"
	Then I should get HTTP NotFound
	And I should get an empty body

Scenario: Getting an existing enviornment
	Given an environment named "dev" exist
	When I GET an environment named "dev"
	Then I should get HTTP 200
	And I should get an environment model with name "dev"

Scenario: Adding environment
	Given the database is empty
	When I POST a environment named "dev" 
	Then I should get HTTP Created
	And an environment named "dev" should be persisted
	And location header should contain url for "environments/dev"
	And I should get an environment model with name "dev"

Scenario: Adding duplicate environment is not allowed
	Given an environment named "dev" exist
	When I POST a environment named "dev" 
	Then I should get HTTP Forbidden with reason "Duplicates are not allowed"

Scenario: Delete environment that exist
	Given an environment named "dev" exist
	When I DELETE an environment named "dev" 
	Then I should get HTTP NoContent
	And there should be 0 environments

Scenario: Delete environment that does not exist
	Given the database is empty
	When I DELETE an environment named "dev" 
	Then I should get HTTP NotFound

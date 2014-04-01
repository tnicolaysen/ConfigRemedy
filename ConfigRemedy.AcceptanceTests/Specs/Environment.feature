Feature: Environment
	In order manage available environments
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I am testing the Environment module

Scenario: Get environment when database is empty
	Given the database is empty
	When I get available environments
	Then I should get HTTP OK
	And I should get an empty list

Scenario: Adding environment
	Given the database is empty
	When I POST a environment named "dev" 
	Then I should get HTTP Created
	And an environment named "dev" should be persisted
	# TODO: Consider checking for location header

@ignore
Scenario: Delete environment
	Given an environment named "dev" exist
	When I DELETE an environment named "dev" 
	Then I should get HTTP NoContent
	And there should be 0 environments

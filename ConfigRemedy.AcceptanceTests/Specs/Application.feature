Feature: Application
	In order manage available applications in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I am testing the Application module

Scenario: Get application when database is empty
	Given an environment named "dev" exist
	When I get available applications for the "dev" enviroment
	Then I should get HTTP OK
	And I should get an empty list

@ignore
Scenario: Adding application
	Given the database is empty
	When I POST a application named "fixerupper" to the "dev" environment 
	Then I should get HTTP Created
	And an environment named "dev" should be persisted
	And location header should contain url for "environments/dev"

@ignore
Scenario: Delete application that exist
	Given an environment named "dev" exist
	When I DELETE an environment named "dev" 
	Then I should get HTTP NoContent
	And there should be 0 environments

@ignore
Scenario: Delete application that does not exist
	Given the database is empty
	When I DELETE an environment named "dev" 
	Then I should get HTTP NotFound

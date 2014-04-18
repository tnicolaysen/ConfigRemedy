Feature: Application
	In order manage available applications in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Get applications in an environment without applications
	Given an environment named "dev" exist
	When I get available applications for the "dev" enviroment
	Then I should get HTTP OK
	And I should get an empty list

@ignore 
Scenario: Get all applications in an environment 
	Given an environment named "dev" exist
	And "dev" has the application "zaphod"
	And "dev" has the application "arthur"
	When I get available applications for the "dev" enviroment
	Then I should get HTTP OK
	And I should get a list containing: "zaphod", "arthur"

Scenario: Adding application
	Given an environment named "dev" exist
	When I POST a application named "fixerupper" to the "dev" environment 
	Then I should get HTTP Created
	And an application named "fixerupper" should be persisted
	And location header should contain url for "environments/dev/applications/fixerupper"

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

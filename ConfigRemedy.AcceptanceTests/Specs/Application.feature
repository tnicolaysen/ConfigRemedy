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

Scenario: Get all applications in an environment 
	Given an environment named "dev" exist
	Given "dev" has the application "zaphod"
	Given "dev" has the application "arthur"
	When I get available applications for the "dev" enviroment
	Then I should get HTTP OK
	And I should get a list containing "arthur"
	And I should get a list containing "zaphod"

Scenario: Adding application
	Given an environment named "dev" exist
	When I POST a application named "fixerupper" to the "dev" environment 
	Then I should get HTTP Created
	And an application named "fixerupper" should be persisted
	And location header should contain url for "environments/dev/applications/fixerupper"

Scenario: Adding duplicate application is not allowed
	Given an environment named "dev" exist
	Given "dev" has the application "zaphod"
	When I POST a application named "zaphod" to the "dev" environment 
	Then I should get HTTP Forbidden with reason "Duplicates are not allowed"

Scenario: Delete application that exist
	Given an environment named "dev" exist
	Given "dev" has the application "zaphod"
	When I DELETE an app named "zaphod" in the environment "dev"
	Then I should get HTTP NoContent
	And there should be 0 apps in the environment "dev"

Scenario: Delete application that does not exist
	Given an environment named "dev" exist
	When I DELETE an app named "idontexist" in the environment "dev"
	Then I should get HTTP NotFound

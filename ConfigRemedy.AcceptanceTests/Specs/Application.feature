Feature: Application
	In order manage available applications in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Get applications when database is empty
	Given the database is empty
	When I get all applications
	Then I should get HTTP OK
	And I should get an empty list

Scenario: Get all applications in an environment 
	Given an application named "zaphod" exist
	Given an application named "arthur" exist
	When I get available applications
	Then I should get HTTP OK
	And I should get a list containing "arthur"
	And I should get a list containing "zaphod"

Scenario: Get specific application
	Given an application named "zaphod" exist
	When I GET the application "zaphod"
	Then I should get HTTP OK
	And I should get an application model with name "zaphod"

Scenario: Get non-existing application
	Given the database is empty
	When I GET the application "idontexist"
	Then I should get HTTP NotFound

Scenario: Adding application
	Given an environment named "dev" exist
	When I POST a application named "fixerupper"
	Then I should get HTTP Created
	And an application named "fixerupper" should be persisted
	And location header should contain url for "applications/fixerupper"

Scenario: Adding duplicate application is not allowed
	Given an application named "zaphod" exist
	When I POST a application named "zaphod" 
	Then I should get HTTP Forbidden with reason "Duplicates are not allowed"

Scenario: Delete application that exist
	Given an application named "zaphod" exist
	When I DELETE an app named "zaphod" 
	Then I should get HTTP NoContent
	And there should be 0 apps

Scenario: Delete application that does not exist
	Given the database is empty
	When I DELETE an app named "idontexist"
	Then I should get HTTP NotFound

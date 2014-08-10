Feature: Authentication
	In order to provide authentication mechanism for the users
	As a programmer
	I want to have a authentication tools 

Background: 
	Given I have a JSON client

Scenario: Token in header
	Given the database is empty
	Given an user with name  "foo" and password "bar" exists
	When I GET login with Token in header
	Then I should get HTTP 200
	
Scenario: ApiKey in header
	Given the database is empty
	Given an ApiKey "dkjasdhfkjshdfkwieurywiue338479384" for user "users/foo" exist
	Given an user with name  "foo" and password "bar" exists
	When I GET login with ApiKey "dkjasdhfkjshdfkwieurywiue338479384" in header
	Then I should get HTTP 200

Scenario: ApiKey as query string
	Given the database is empty
	Given an ApiKey "dkjasdhfkjshdfkwieurywiue338479384" for user "users/foo" exist
	Given an user with name  "foo" and password "bar" exists
	When I GET login with ApiKey "dkjasdhfkjshdfkwieurywiue338479384" as query string
	Then I should get HTTP 200

Scenario: No token or ApiKey
	Given the database is empty
	Given an user with name  "foo" and password "bar" exists
	When I GET login without token or ApiKey
	Then I should get HTTP 401
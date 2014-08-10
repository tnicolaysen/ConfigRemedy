Feature: Login
	In order to authenticate users
	As a programmer
	I want to have a REST API to manage it

Background: 
	Given I have a JSON client

Scenario: Login
	Given the database is empty
	Given an user with name  "foo" and password "bar" exists
	When I POST username "foo" and password "bar" 
	Then I should get HTTP 200
	And response should contain "userId"
	And response should contain "userName"
	And response should contain "role"
	And response should contain "token"
	



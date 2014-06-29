Feature: CrossOriginHeaders
	In order to support JS SPA and other clients
	As a web developer
	I want to use the API through AJAX

Scenario: Add Access-Control-Allow-Origin header
	Given I make a new module without any customization
	When I make a GET request
	Then the header should contain "Access-Control-Allow-Origin" = "*"

Scenario: Add Access-Control-Allow-Method header to OPTIONS response
	Given I make a new module without any customization
	When I make a OPTIONS request
	Then the header should contain "Access-Control-Allow-Methods" = "DELETE"
Feature: Setting
	In order manage settings for a given application in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Getting settings for an app. without settings
	Given an environment named "dev" exist
	Given "dev" has the application "zaphod"
	When I get available settings for the application "zaphod" in the "dev" enviroment
	Then I should get HTTP OK
	And I should get an empty list

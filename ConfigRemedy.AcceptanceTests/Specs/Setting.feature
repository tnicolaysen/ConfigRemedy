Feature: Setting
	In order manage settings for a given application in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Getting settings for an app. without settings
	Given an environment named "test" exist
	Given "test" has the application "zaphod"
	When I get available settings for the application "zaphod" in the "test" enviroment
	Then I should get HTTP OK
	And I should get an empty object

Scenario: Getting settings for an app.
	Given an environment named "test" exist
	Given "test" has the application "scroogle"
	And the following setting exist in "test/scroogle": "setting1" = "a"
	And the following setting exist in "test/scroogle": "setting2" = "b"
	When I get available settings for the application "scroogle" in the "test" enviroment
	Then I should get HTTP OK
	And I should the following settings:
		| Key      | Value |
		| setting1 | a     |
		| setting2 | b     |

Scenario: Get specific setting value
	Given an environment named "prod" exist
	Given "prod" has the application "fooble"
	And the following setting exist in "prod/fooble": "version" = "1.0 RC1"
	When I get the setting "version" in "prod/fooble"
	Then I should get HTTP OK
	And I should get a string identical to "1.0 RC1"

Scenario: Get specific setting that don't exist
	Given an environment named "prod" exist
	Given "prod" has the application "fooble"
	When I get the setting "idontexist" in "prod/fooble"
	Then I should get HTTP NotFound

Scenario: Adding settings to an application
	Given an environment named "test" exist
	Given "test" has the application "scroogle"
	And I POST the following setting to "test/scroogle": "retries" = "10"
	Then I should get HTTP Created
	And the setting "retries" should be persisted in "test/scroogle" with value "10"
	And location header should contain url for "environments/test/scroogle/retries"

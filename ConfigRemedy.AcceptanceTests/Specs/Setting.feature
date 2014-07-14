Feature: Setting
	In order manage settings for a given application in a given environment
	As a programmer
	I want to have a REST API to manage them

Background: 
	Given I have a JSON client

Scenario: Getting settings for an app. without settings
	Given an application named "zaphod" exist
	When I get available settings for the application "zaphod"
	Then I should get HTTP OK
	And I should get an empty settings list

Scenario: Getting settings for an app.
	Given an application named "scroogle" exist
	Given that "scroogle" have the following settings:
		| Key      | DefaultValue |
		| setting1 | arthur       |
		| setting2 | zaphod       | 
	When I get available settings for the application "scroogle"
	Then I should get HTTP OK
	And I should the following settings:
		| Key      | DefaultValue |
		| setting1 | arthur       |
		| setting2 | zaphod       | 

Scenario: Get a setting value for an environment
	Given an environment named "prod" exist
	Given an application named "fooble" exist
	Given that "fooble" have the following settings:
		| Key     | DefaultValue |
		| version | 0.0.1        |
	And the following overrides exist:
		| App    | Environment | Key     | Value        |
		| fooble | prod        | version | 1.0 RC1-PROD |
	When I get the setting "version" in "fooble/prod"
	Then I should get HTTP OK
	And I should get a string identical to "1.0 RC1-PROD"

Scenario: Get specific setting that don't exist
	Given an environment named "prod" exist
	Given an application named "fooble" exist
	When I get the setting "idontexist" in "fooble/prod"
	Then I should get HTTP NotFound

Scenario: Adding settings to an application
	Given an environment named "test" exist
	Given an application named "scroogle" exist
	When I POST the following settings to "scroogle":
		| Key     | DefaultValue |
		| retries | 10           |	
	Then I should get HTTP Created
	And the setting "retries" should be persisted in "scroogle" with default value "10"
	And location header should contain url for "api/applications/scroogle/settings/retries"

Scenario: Adding duplicate setting is not allowed
	Given an application named "scroogle" exist
	Given that "scroogle" have the following settings:
		| Key             | DefaultValue   |
		| dontDuplicateMe | Sure, whatever |
	When I POST the following settings to "scroogle":
		| Key             | DefaultValue |
		| dontDuplicateMe | uwish        |	
	Then I should get HTTP Forbidden with reason "Duplicates are not allowed"

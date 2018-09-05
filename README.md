# WcRunway

## TODO
* Snowflake queries are extremely slow

## Repository map
### Main applications
#### WcData
Contains connections to the various data sources the game requires, including:
* Game Context: the MySql database that powers the game. Each environment has its own interface (i.e. ISandbox2Context, ILiveSlaveContext) 
  over top of a common
* Sheets: The various data sheets maintained in Google Sheets that designers have edit access on
* Snowflake: Connection to the Snowflake data warehouse that mirrors and expands on game context data

Each of these namespaces will define their own data models in their ```*.Models``` namespace. There is a not insignificant amount of overlap with these models, mostly between Game Context and Snowflake, since Snowflake is partially a mirror of the MySql data. Beware of cases where you may require a fully qualified name.

#### WcData.Microsoft.Extensions.DependenyInjection
Extensions to allow easy wiring of WcData assets when using Microsoft's dependency injection framework

#### WcOffers
Contains the domain logic for nearly all phases of administration of an offer; creating and saving the offer in a testing environment, creating a JIRA ticket for quality assurance, and later generating the cohort. For now, the automation **does not interact with the production database**, so promotion & final setup are left as manual steps.

This project pulls heavily from Snowflake for cohorting, MySql for writing offers and [Google Sheets](https://docs.google.com/spreadsheets/d/1x3nlFmcPUNzJT6wwkqxtGBnxcWALenR5ZnBI5wZjxvw) for template data.


#### WcOffers.Cli
A command-line interface to allow:
1) An analyst to administer the various phases of an offer on an ad-hoc basis
2) Jenkins integration for scheduling and JIRA-reactive actions 

### Testing
#### WcRunway.Core.Tests

#### WcRunway.Cli.Tests
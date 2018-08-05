# WcRunway

## TODO
* Snowflake queries are extremely slow

## Repository map
### Main applications
#### WcCore
The lowest level project that models game objects and their relationships. This project should have no dependences to other Wc* assets

#### WcData
Contains connections to the various data sources the game requires, including:
* Game Context: the MySql database that powers the game. Each environment has its own context class (i.e. Sandbox2, Live - main, Live - slave)
* Sheets: The various data sheets maintained in Google Sheets that designers have edit access on
* Snowflake: Connection to the Snowflake data warehouse that mirrors and expands on game context data

#### WcData.Microsoft.Extensions.DependenyInjection
Extensions to allow easy wiring of WcData assets when using Microsoft's dependency injection framework

#### WcRunway.Cli
A simple interface to the project intended for quick, end-user use.

### Testing
#### WcRunway.Core.Tests

#### WcRunway.Cli.Tests
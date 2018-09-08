# WcRunway
A collection of proof-of-concept projects that are meant to illustrate the feasibility of automating/augmenting some WCB product duties.

## WcData
The ```WcData``` project is the backbone that contains connections to the various data sources the game requires, including:
* Game Context: the MySql database that powers the game. Each environment has its own interface (i.e. ISandbox2Context, ILiveSlaveContext) 
  over top of a common
* Sheets: The various data sheets maintained in Google Sheets that designers have edit access on
* Snowflake: Connection to the Snowflake data warehouse that mirrors and expands on game context data

Each of these namespaces will define their own data models in their ```*.Models``` namespace. There is a not insignificant amount of overlap with these models, mostly between Game Context and Snowflake, since Snowflake is partially a mirror of the MySql data. Beware of cases where you may require a fully qualified name. These models are anemic entities with little more than properties. Each consuming project may define their own domain models with appropriate behaviour.

The ```WcData.Microsoft.Extensions.DependenyInjection``` project is included containing extensions that allow easy wiring of ```WcData``` assets when using Microsoft's dependency injection framework. Note that each consuming project **must** provide connection details when wiring up the container. Credentials, URLs and any other sensitive configuration items are completely omitted from this layer.



## Offers
The offers suite includes a core ```WcOffers``` project that contains the domain logic for nearly all phases of administration of an offer; creating and saving the offer in a testing environment, creating a JIRA ticket for quality assurance, and later generating the cohort. For now, the automation **does not interact with the production database**, so promotion & final setup are left as manual steps.

This project pulls heavily from Snowflake for cohorting, MySql for writing offers and [Google Sheets](https://docs.google.com/spreadsheets/d/1x3nlFmcPUNzJT6wwkqxtGBnxcWALenR5ZnBI5wZjxvw) for template data.

A command-line interface in the ```WcOffers.Cli``` project allows an analyst to administer the various phases of an offer on an ad-hoc basis. It is also intended to allow for Jenkins integration for scheduling and JIRA-reactive actions.

| Command | Description |
| ------- | ----------- |
| ```validate``` | Validates the configuration settings for the application |
| ```qa``` | Creates a JIRA ticket for an offer |
| ```list-templates``` | Shows a listing of offer templates and their IDs |
| ```gen-template``` | Generate offers based on a template |
| ```gen-group``` | Generate offers based on grouped templates, e.g. base compression |
| ```gen-unique``` | Generates a suite of well-understood offers for a given Unique unit based on standard conventions |
| ```help``` | Display more information on a specific command |
| ```version``` | Display version information |



## TODO
* Snowflake queries are extremely slow
* UniqueOfferGenerator test for created time being set
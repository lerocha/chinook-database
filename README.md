## Chinook Database

Chinook is a sample database available for SQL Server, Oracle, MySQL, etc. It can be created by running a single SQL script. Chinook database is an alternative to the Northwind database, being ideal for demos and testing ORM tools targeting single and multiple database servers.

### Supported Database Servers

* DB2
* MySQL
* Oracle
* PostgreSQL
* SQL Server
* SQL Server Compact
* SQLite

### Download
Download the SQL scripts from the [latest release](../../releases) assets. One or more SQL script files are provided for each database vendor supported. You can run these SQL scripts with your preferred database tool.

_Update: Check out the [Netflix Sample Database](https://github.com/lerocha/netflixdb), a sample database with movies and TV shows based on the data from the Netflix Engagement Report and the Netflix Global Top 10 weekly list._

### Data Model

The Chinook data model represents a digital media store, including tables for artists, albums, media tracks, invoices and customers.

<img width="836" alt="image" src="https://github.com/lerocha/chinook-database/assets/135025/cea7a05a-5c36-40cd-84c7-488307a123f4">

### Sample Data

Media related data was created using real data from an iTunes Library. It is possible for you to use your own iTunes Library to generate the SQL scripts, see instructions below.
Customer and employee information was manually created using fictitious names, addresses that can be located on Google maps, and other well formatted data (phone, fax, email, etc.).
Sales information is auto generated using random data for a four year period.

### Why the name Chinook?

The name of this sample database was based on the Northwind database. Chinooks are winds in the interior West of North America, where the Canadian Prairies and Great Plains meet various mountain ranges. Chinooks are most prevalent over southern Alberta in Canada. Chinook is a good name choice for a database that intents to be an alternative to Northwind.

### Development

System Requirements:
* [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* dotnet-t4 (command-line tool for processing T4 templates): `dotnet tool install --global dotnet-t4`

Notes:
* The SQL scripts are auto-generated using [Text Template Transformation](https://learn.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates?view=vs-2022).
* The `ChinookDataSet.xsd` file contains the schema definition, `ChinookData.json` contains the data, and the `*.tt` files are the text templates that are used to generate all SQL scripts.
* You can build the solution using any IDE (Visual Studio Community Edition, Visual Studio Code, Jetbrains Rider) or using `dotnet build` in any OS (macOS, Linux, Windows).

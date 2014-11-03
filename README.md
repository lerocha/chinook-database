## Chinook Database

Chinook is a sample database available for SQL Server, Oracle, MySQL, etc. It can be created by running a single SQL script. Chinook database is an alternative to the Northwind database, being ideal for demos and testing ORM tools targeting single and multiple database servers.

### Supported Database Servers

* MySQL
* SQL Server
* SQL Server Compact
* SQLite
* PostgreSQL
* Oracle
* DB2

### Data Model

The Chinook data model represents a digital media store, including tables for artists, albums, media tracks, invoices and customers.

### Sample Data

Media related data was created using real data from an iTunes Library. It is possible for you to use your own iTunes Library to generate the SQL scripts, see instructions below.
Customer and employee information was manually created using fictitious names, addresses that can be located on Google maps, and other well formatted data (phone, fax, email, etc.).
Sales information is auto generated using random data for a four year period.

### Why the name Chinook?

The name of this sample database was based on the Northwind database. Chinooks are winds in the interior West of North America, where the Canadian Prairies and Great Plains meet various mountain ranges. Chinooks are most prevalent over southern Alberta in Canada. Chinook is a good name choice for a database that intents to be an alternative to Northwind.

### How do I Download and Create the Chinook Database?

Download and extract the zip file from the [Downloads page](https://chinookdatabase.codeplex.com/releases/view/55681). It is provided one or more SQL script file for each database vendor supported. You can run these SQL scripts with your preferred database tool.

For SQL Server, Oracle, MySQL and SQLite, there are available batch files that will execute the respective SQL script(s). These batch files use the database server command tool (mysql.exe for MySQL, sqlcmd.exe for SQL Server, sqlplus.exe for Oracle, and sqlite3.exe for SQLite). It is recommended to open a Command Prompt window as Administrator in order to run these batch files.

For embedded databases, SQLite and SQL Server Compact, it is also provided the embedded database files in addition to the SQL script files. 

Notes:

* MySQL Version
  * The provided scripts were tested using MySQL Server 5.1.
  * The **CreateMySql.bat** file uses the localhost server, and the following user / password: **root / p4ssw0rd**. Change this file to match your settings before running it.
* Oracle Version
  * The provided script was tested using Oracle Database 10g Express Edition (XE).
  * The **CreateOracle.sql** creates a new user / password: **chinook / p4ssw0rd**. Change this script if you want a different user/password.
* SQL Server Version
  * The provided script was tested using SQL Server Express 2008.
* SQL Server Compact Version
  * The provided scripts were tested using SQL Server Compact 3.5 SP1.
  * We included the compact databases (sdf files) using version 3.5.8080.0. If you prefer to create your own compact database, then you can use the .sqlce script with SQL Server Management Studio Express tool.
* Notes for SQLite Version
  * The provided scripts were tested using SQLite 3.7.3
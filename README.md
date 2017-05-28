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

Download the files from the [Downloads page](ChinookDatabase/DataSources). It is provided one or more SQL script file for each database vendor supported. You can run these SQL scripts with your preferred database tool.

For SQL Server, Oracle, MySQL and SQLite, there are available batch files that will execute the respective SQL script(s). These batch files use the database server command tool (**mysql.exe** for MySQL, **sqlcmd.exe** for SQL Server, **sqlplus.exe** for Oracle, and **sqlite3.exe** for SQLite). It is recommended to open a Command Prompt window as Administrator in order to run these batch files.

For embedded databases, SQLite and SQL Server Compact, it is also provided the embedded database files in addition to the SQL script files.

Notes:

* MySQL
  * Tested with MySQL Server 5.1.
  * The **CreateMySql.bat** script uses localhost server with the following user/password: **root/p4ssw0rd**. Change this file to match your settings before running it.
* Oracle
  * Tested with Oracle Database 10g Express Edition (XE).
  * The **CreateOracle.sql** creates a new user/password: **chinook/p4ssw0rd**. Change this script if you want a different user/password.
* SQL Server
  * Tested with SQL Server Express 2008.
* SQL Server Compact
  * Tested with SQL Server Compact 3.5 SP1.
  * We included the compact databases (sdf files) using version 3.5.8080.0. If you prefer to create your own compact database, then you can use the .sqlce script with SQL Server Management Studio Express tool.
* SQLite
  * Tested with SQLite 3.7.3

### Development

#### System Requirements

* Visual Studio 2012
* SQL Server 2008 Express
* Oracle Database 11g Express Edition Release 2 (XE)
* MySQL Community Server 5.5.28
  * Choose all default settings when installing, but on the MySQL Server Instance installer make sure to select:
    * Best Support For Multilingualism
    * Include Bin Directory in Windows PATH
* SQLite 3.7 Command Line
* DB2 Express-C
* PostgreSQL

#### Building and Generating the SQL Scripts

* Open the solution file ChinookDatabase.sln in Visual Studio.
* If you want to use the data from your own iTunes library, then replace the file **ChinookDatabase\DataSources\_Xml\Source\iTunes Music Library.xml** with your version.
* In app.config, verify that the connection strings are matching any changes you made to username/password above. Also, the Oracle connection string uses the Express Edition, e.g. Data Source=xe. So, change it if you are using a different Oracle edition.
* On the *Solution Explorer*, click on your *Solution* and then click on the *Transform All Templates* button at the Solution Explorer buttons bar.
* Rebuild the solution.
* Verify that your database servers are properly set:
  * The batch file DataSources\CreateMySql.bat uses the root user with the password p4ssw0rd.
  * The generated Oracle SQL script creates a new user chinook, with the password p4ssw0rd. You might want to change this to use a different user/password.
* Create the database using the appropriate scripts.
* Run the database tests in ChinookDatabase.Test to make sure your database was created properly.

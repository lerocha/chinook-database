## Project Description
Chinook is a sample database available for SQL Server, Oracle, MySQL, etc. It can be created by running a single SQL script. Chinook database is an alternative to the Northwind database, being ideal for demos and testing ORM tools targeting single and multiple database servers.
## What is New?
* Dec 10, 2012: Chinook database is now available on NuGet! See details [here](http://t.co/3fSbOgVb).
* Dec 2, 2012: [release:55681](release_55681) has been released! In this release we added support for two more databases: [DB2](http://www-01.ibm.com/software/data/db2/express/) and [PostgreSQL](http://www.postgresql.org/). Thanks to  [BriceLambson](http://www.codeplex.com/site/users/view/BriceLambson) for implementing the PostgreSQL support! This is the list of the new changes:
	* [workitem:29378](workitem_29378)
	* [workitem:29374](workitem_29374)
	* [workitem:29375](workitem_29375)
	* [workitem:29377](workitem_29377)
## Supported Database Servers
* [DB2](http://www-01.ibm.com/software/data/db2/express/)
* [EffiProz](http://effiproz.codeplex.com/)
* [MySQL](http://www.mysql.com/)
* [Oracle](http://www.oracle.com/technetwork/database/express-edition/overview/index.html)
* [PostgreSQL](http://www.postgresql.org/)
* [SQL Server](http://www.microsoft.com/sqlserver/)
* [SQL Server Compact](http://www.microsoft.com/sqlserver/2008/en/us/compact.aspx)
* [SQLite](http://www.sqlite.org/)
## Data Model
The Chinook data model represents a digital media store, including tables for artists, albums, media tracks, invoices and customers. You can see the Chinook data model [here](Chinook_Schema).
## Sample Data
* Media related data was created using real data from an iTunes Library. It is possible for you to use your own iTunes Library to generate the SQL scripts, see instructions [here](Building_Scripts). 
* Customer and employee information was manually created using fictitious names, addresses that can be located on Google maps, and other well formatted data (phone, fax, email, etc.).
* Sales information is auto generated using random data for a four year period.
## Why the name Chinook?
The name of this sample database was based on the Northwind database. [Chinooks](http://en.wikipedia.org/wiki/Chinook_wind) are winds in the interior West of North America, where the Canadian Prairies and Great Plains meet various mountain ranges. Chinooks are most prevalent over southern Alberta in Canada. Chinook is a good name choice for a database that intents to be an alternative to Northwind.
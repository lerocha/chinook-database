## How do I Create the Chinook Database?
Download and extract the zip file from the Downloads tab. It is provided one or more SQL script file for each database vendor supported. You can run these SQL scripts with your preferred database tool.

For SQL Server, Oracle, MySQL and SQLite, there are available batch files that will execute the respective SQL script(s). These batch files use the database server command tool (**mysql.exe** for MySQL, **sqlcmd.exe** for SQL Server, **sqlplus.exe** for Oracle, and **sqlite3.exe** for SQLite). If you are using Windows Vista/2008/7, it is recommended to open a Command Prompt window as Administrator in order to run these batch files.

For embedded databases, SQLite and SQL Server Compact, it is also provided the embedded database files in addition to the SQL script files. 

## Notes for MySQL Version
* The provided scripts were tested using [MySQL Server 5.1](http://dev.mysql.com/downloads/mysql/5.1.html).
* The **CreateMySql.bat** file uses the localhost server, and the **root** user with the password **p4ssw0rd**. Change this file to match your settings before running it.
## Notes for Oracle Version
* The provided script was tested using [Oracle Database 10g Express Edition (XE)](http://www.oracle.com/technology/products/database/xe/index.html).
* The **CreateOracle.sql** creates a new user **chinook**, with the password **p4ssw0rd**. Change this script if you want a different user/password.
## Notes for SQL Server Version
* The provided script was tested using [SQL Server Express 2008](http://www.microsoft.com/downloads/details.aspx?FamilyID=7522a683-4cb2-454e-b908-e805e9bd4e28&DisplayLang=en).
## Notes for SQL Server Compact Version
* The provided scripts were tested using [SQL Server Compact 3.5 SP1](http://www.microsoft.com/Sqlserver/2008/en/us/compact.aspx).
* We included the compact databases (**sdf** files) using version 3.5.8080.0. If you prefer to create your own compact database, then you can use the **.sqlce** script with [SQL Server Management Studio Express](http://msdn.microsoft.com/en-us/library/ms365247.aspx) tool.
## Notes for SQLite Version
* The provided scripts were tested using SQLite 3.7.3
## Notes for EffiProz Version
* The provided scripts were tested using the [EffiProz Silverlight Query Tool](http://www.effiproz.com/SL4QTDemo.aspx)

# Generating SQL scripts for the Chinook Database
## Required Tools
* Microsoft Visual Studio 2012
## Database Servers
* SQL Server Express (already installed with Visual Studio).
* [Oracle Database 11g Express Edition (XE)](http://www.oracle.com/technetwork/products/express-edition/overview/index.html).
* [MySQL 5.5 Community Server](http://www.mysql.com/downloads/mysql/): Choose all default settings when installing, but on the MySQL Server Instance installer make sure to select:
	* Best Support For Multilingualism
	* Include Bin Directory in Windows PATH
* Download the latest source code from [here.](http://www.codeplex.com/ChinookDatabase/SourceControl/ListDownloadableCommits.aspx)
* Open the Solution file **ChinookDatabase.sln** in Visual Studio.
* If you want to use the data from your own iTunes library, then replace the file **ChinookDatabase\DataSources\_Xml\Source\iTunes Music Library.xml** with your version.
	* In **app.config**, verify that the connection strings are matching any changes you made to username/password above. Also, the Oracle connection string uses the Express Edition, e.g. **Data Source=xe**. So, change it if you are using a different Oracle edition.
* On the Solution Explorer, click on your Solution and then click on the **Transform All Templates** button at the Solution Explorer buttons bar.
* Rebuild the solution.
* Verify that your database servers are properly set:
	* The batch file **DataSources\CreateMySql.bat** uses the **root** user with the password **p4ssw0rd**. 
	* The generated Oracle SQL script creates a new user **chinook**, with the password **p4ssw0rd**. You might want to change this to use a different user/password.
* Create the database using the appropriate scripts.
* Run the database tests in ChinookDatabase.Test to make sure your database was created properly.
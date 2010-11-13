@echo off
echo Chinook_SqlServer.sql
sqlcmd -E -S .\sqlexpress -i Chinook_SqlServer.sql -b -m 1
echo.

echo Chinook_SqlServer_AutoIncrementPKs.sql
sqlcmd -E -S .\sqlexpress -i Chinook_SqlServer_AutoIncrementPKs.sql -b -m 1
echo.

echo Chinook_MySql.sql
mysql -h localhost -u root --password=p4ssw0rd <Chinook_MySql.sql
echo.

echo Chinook_MySql_AutoIncrementPKs.sql
mysql -h localhost -u root --password=p4ssw0rd <Chinook_MySql_AutoIncrementPKs.sql
echo.

echo Chinook_Oracle.sql
sqlplus -S / as sysdba @ Chinook_Oracle.sql
echo.

echo Chinook_Sqlite.sql
del Chinook_Sqlite.sqlite
sqlite3 -init Chinook_Sqlite.sql Chinook_Sqlite.sqlite
echo.

echo Chinook_Sqlite_AutoIncrementPKs.sql
del Chinook_Sqlite_AutoIncrementPKs.sqlite
sqlite3 -init Chinook_Sqlite_AutoIncrementPKs.sql Chinook_Sqlite_AutoIncrementPKs.sqlite
echo.

rem echo Chinook_SqlServerCompact.sql
rem sqlcecmd –d "Data Source=.\Chinook_SqlServerCompact.sdf" –e create
rem echo.


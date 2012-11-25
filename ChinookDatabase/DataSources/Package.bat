@echo off
set rootDir=%1
if (%rootDir%)==() set rootDir=%cd%
if exist %rootDir%\Packages\. del %rootDir%\Packages\*.* /q
if not exist %rootDir%\Packages mkdir %rootDir%\Packages

7z a %rootDir%\Packages\ChinookDatabase1.3_CompleteVersion.zip Chinook_SqlServer.sql Chinook_SqlServer_AutoIncrementPKs.sql Chinook_SqlServerCompact.sqlce Chinook_SqlServerCompact.sdf Chinook_SqlServerCompact_AutoIncrementPKs.sqlce Chinook_SqlServerCompact_AutoIncrementPKs.sdf Chinook_Sqlite.sql Chinook_Sqlite.sqlite Chinook_Sqlite_AutoIncrementPKs.sql Chinook_Sqlite_AutoIncrementPKs.sqlite Chinook_MySql.sql Chinook_MySql_AutoIncrementPKs.sql Chinook_Oracle.sql Chinook_EffiProz.sql Chinook_EffiProz_AutoIncrementPKs.sql Chinook_PostgreSql.sql CreateSqlServer.bat CreateSqlite.bat CreateMySql.bat CreateOracle.bat CreatePostgreSql.bat _Xml\ChinookData.xml
7z a %rootDir%\Packages\ChinookDatabase1.3_SqlServer.zip Chinook_SqlServer.sql Chinook_SqlServer_AutoIncrementPKs.sql CreateSqlServer.bat
7z a %rootDir%\Packages\ChinookDatabase1.3_SqlServerCompact.zip Chinook_SqlServerCompact.sqlce Chinook_SqlServerCompact.sdf Chinook_SqlServerCompact_AutoIncrementPKs.sqlce Chinook_SqlServerCompact_AutoIncrementPKs.sdf
7z a %rootDir%\Packages\ChinookDatabase1.3_Sqlite.zip Chinook_Sqlite.sql Chinook_Sqlite.sqlite Chinook_Sqlite_AutoIncrementPKs.sql Chinook_Sqlite_AutoIncrementPKs.sqlite CreateSqlite.bat
7z a %rootDir%\Packages\ChinookDatabase1.3_MySql.zip Chinook_MySql.sql Chinook_MySql_AutoIncrementPKs.sql CreateMySql.bat
7z a %rootDir%\Packages\ChinookDatabase1.3_Oracle.zip Chinook_Oracle.sql CreateOracle.bat
7z a %rootDir%\Packages\ChinookDatabase1.3_EffiProz.zip Chinook_EffiProz.sql Chinook_EffiProz_AutoIncrementPKs.sql
7z a %rootDir%\Packages\ChinookDatabase1.3_PostgreSql.zip Chinook_PostgreSql.sql CreatePostgreSql.bat

@echo off
echo Chinook Database Version 1.4
echo.

if "%1"=="" goto MENU
if not exist %1 goto ERROR

set SQLFILE=%1
goto RUNSQL

:ERROR
echo The file %1 does not exist.
echo.
goto END

:MENU
echo Options:
echo.
echo 1. Run Chinook_SqlServer.sql
echo 2. Run Chinook_SqlServer_AutoIncrementPKs.sql
echo 3. Exit
echo.
choice /c 123
if (%ERRORLEVEL%)==(1) set SQLFILE=Chinook_SqlServer.sql
if (%ERRORLEVEL%)==(2) set SQLFILE=Chinook_SqlServer_AutoIncrementPKs.sql
if (%ERRORLEVEL%)==(3) goto END

:RUNSQL
echo.
echo Running %SQLFILE%...
sqlcmd -E -S .\sqlexpress -i %SQLFILE% -b -m 1

:END
echo.
set SQLFILE=


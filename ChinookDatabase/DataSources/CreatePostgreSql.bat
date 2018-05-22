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
echo 1. Run Chinook_PostgreSql.sql
echo 2. Run Chinook_PostgreSql_CaseInsensitive.sql
echo 3. Run Chinook_PostgreSql_SerialPKs.sql
echo 4. Run Chinook_PostgreSql_SerialPKs_CaseInsensitive.sql
echo 5. Exit
echo.
choice /c 12345
if (%ERRORLEVEL%)==(1) set SQLFILE=Chinook_PostgreSql.sql
if (%ERRORLEVEL%)==(2) set SQLFILE=Chinook_PostgreSql_CaseInsensitive.sql
if (%ERRORLEVEL%)==(3) set SQLFILE=Chinook_PostgreSql_SerialPKs.sql
if (%ERRORLEVEL%)==(4) set SQLFILE=Chinook_PostgreSql_SerialPKs_CaseInsensitive.sql
if (%ERRORLEVEL%)==(5) goto END

:RUNSQL
echo.
echo Running %SQLFILE%...
SET "PGOPTIONS=-c client_min_messages=WARNING"
dropdb --if-exists -U postgres Chinook
createdb -U postgres Chinook
psql -f %SQLFILE% -q Chinook postgres


:END
echo.
set SQLFILE=


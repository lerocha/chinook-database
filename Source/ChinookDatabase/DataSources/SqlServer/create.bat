 


@echo off
echo Chinook Database Version 1.2
echo.

if "%1"=="" goto ERROR
if not exist %1 goto ERROR2

echo Running %1...
sqlcmd -E -S .\sqlexpress -i %1 -b -m 1
goto END

:ERROR2
echo The file %1 does not exist.
echo.

:ERROR
echo Please provide the script to be used, for example:
echo CreateDB CreateDB.sql
goto END

:END

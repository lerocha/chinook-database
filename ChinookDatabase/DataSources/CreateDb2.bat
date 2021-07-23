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
echo 1. Run Chinook_Db2.sql
echo 2. Exit
echo.
choice /c 123
if (%ERRORLEVEL%)==(1) set SQLFILE=Chinook_Db2.sql
if (%ERRORLEVEL%)==(2) goto END

:RUNSQL
echo.
echo Running %SQLFILE%...
db2 disconnect ALL
db2 drop database Chinook
db2 create database Chinook
db2 connect to Chinook
db2 -tf %SQLFILE% -z %SQLFILE%.log


:END
echo.
set SQLFILE=


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
echo 1. Run Chinook_Oracle.sql
echo 2. Exit
echo.
choice /c 123
if (%ERRORLEVEL%)==(1) set SQLFILE=Chinook_Oracle.sql
if (%ERRORLEVEL%)==(2) goto END

:RUNSQL
echo.
echo Running %SQLFILE%...
chcp 65001
set NLS_LANG=.AL32UTF8
sqlplus -S / as sysdba @ %SQLFILE%


:END
echo.
set SQLFILE=


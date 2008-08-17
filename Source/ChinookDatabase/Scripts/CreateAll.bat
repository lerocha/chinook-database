@echo off

echo =====================================================================
echo  Creating SqlServer Database... 
echo =====================================================================
cd SqlServer
call CreateDB.bat 
IF ERRORLEVEL 1 GOTO ERROR 
cd.. 
echo.

echo =====================================================================
echo  Creating MySql Database... 
echo =====================================================================
cd MySql
call CreateDB.bat 
IF ERRORLEVEL 1 GOTO ERROR 
cd.. 
echo.

echo =====================================================================
echo  Creating Oracle Database... 
echo =====================================================================
cd Oracle
call CreateDB.bat 
IF ERRORLEVEL 1 GOTO ERROR 
cd.. 
echo.

goto EXIT

:ERROR
cd..
echo.
echo =====================================================================
echo An error occured when creating databases. Please review errors above.
echo =====================================================================
echo.
goto EXIT

:EXIT


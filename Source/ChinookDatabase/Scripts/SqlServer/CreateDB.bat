@echo off
sqlcmd -E -S .\sqlexpress -i CreateDB.sql -b -m 1

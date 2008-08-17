@echo off
OSQL -E -S .\sqlexpress -i CreateDB.sql -n -b -m

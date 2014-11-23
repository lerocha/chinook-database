@echo off
del *.nupkg
nuget pack ChinookDatabase.SQLite\ChinookDatabase.Sqlite.1.4.0.nuspec
nuget pack ChinookDatabase.SqlServerCompact\ChinookDatabase.SqlServerCompact.1.4.2.nuspec
nuget pack ChinookDatabase.SqlScripts\ChinookDatabase.SqlScripts.1.4.0.nuspec


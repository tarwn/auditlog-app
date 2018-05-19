Powershell.exe -File "Database/CreateDatabaseUpdates.ps1" -UpdatesFolder "./Changes" -UpdatesFile "DatabaseUpdate.sql"
Powershell.exe -File "Database/ApplyDatabaseUpdates.ps1" -UpdatesFile "DatabaseUpdate.sql" -Server . -Database AuditLogDev -Trusted
pause
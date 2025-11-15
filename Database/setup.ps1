# Ihospital Database Setup Script
# This script will create and initialize the Ihospital database

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Ihospital Database Setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$serverName = "DESKTOP-BPQ5AR0"
$databaseName = "Ihospital"
$scriptPath = Join-Path $PSScriptRoot "setup-database.sql"

# Check if SQL script exists
if (-not (Test-Path $scriptPath)) {
    Write-Host "Error: SQL script not found at $scriptPath" -ForegroundColor Red
    exit 1
}

Write-Host "Server: $serverName" -ForegroundColor Yellow
Write-Host "Database: $databaseName" -ForegroundColor Yellow
Write-Host "Script: $scriptPath" -ForegroundColor Yellow
Write-Host ""

# Test SQL Server connection
Write-Host "Testing SQL Server connection..." -ForegroundColor Green
try {
    $testQuery = "SELECT @@VERSION"
    $result = Invoke-Sqlcmd -ServerInstance $serverName -Query $testQuery -ErrorAction Stop
    Write-Host "✓ Connected to SQL Server successfully" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "✗ Failed to connect to SQL Server" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please ensure:" -ForegroundColor Yellow
    Write-Host "  1. SQL Server is running" -ForegroundColor Yellow
    Write-Host "  2. Server name is correct: $serverName" -ForegroundColor Yellow
    Write-Host "  3. Windows Authentication is enabled" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}

# Execute the setup script
Write-Host "Executing database setup script..." -ForegroundColor Green
try {
    Invoke-Sqlcmd -ServerInstance $serverName -InputFile $scriptPath -ErrorAction Stop
    Write-Host "✓ Database setup completed successfully!" -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "✗ Failed to execute setup script" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    exit 1
}

# Verify database creation
Write-Host "Verifying database..." -ForegroundColor Green
try {
    $verifyQuery = @"
    SELECT 
        (SELECT COUNT(*) FROM Ihospital.sys.tables) AS TableCount,
        (SELECT COUNT(*) FROM Ihospital.dbo.STAFF) AS StaffCount,
        (SELECT COUNT(*) FROM Ihospital.dbo.QUESTION) AS QuestionCount,
        (SELECT COUNT(*) FROM Ihospital.dbo.OPTION_LIST) AS OptionCount
"@
    $stats = Invoke-Sqlcmd -ServerInstance $serverName -Query $verifyQuery -ErrorAction Stop
    
    Write-Host "✓ Database verification successful" -ForegroundColor Green
    Write-Host ""
    Write-Host "Database Statistics:" -ForegroundColor Cyan
    Write-Host "  Tables created: $($stats.TableCount)" -ForegroundColor White
    Write-Host "  Staff accounts: $($stats.StaffCount)" -ForegroundColor White
    Write-Host "  Sample questions: $($stats.QuestionCount)" -ForegroundColor White
    Write-Host "  Sample options: $($stats.OptionCount)" -ForegroundColor White
    Write-Host ""
}
catch {
    Write-Host "Warning: Could not verify database" -ForegroundColor Yellow
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Default Admin Credentials:" -ForegroundColor Yellow
Write-Host "  Username: admin" -ForegroundColor White
Write-Host "  Password: admin123" -ForegroundColor White
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Cyan
Write-Host "  1. Run the API:" -ForegroundColor White
Write-Host "     cd ..\Ihospital.API" -ForegroundColor Gray
Write-Host "     dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "  2. Test the API:" -ForegroundColor White
Write-Host "     Open: https://localhost:5001/swagger" -ForegroundColor Gray
Write-Host ""
Write-Host "  3. Login with admin credentials to get JWT token" -ForegroundColor White
Write-Host ""
Write-Host "Happy coding! 🚀" -ForegroundColor Green

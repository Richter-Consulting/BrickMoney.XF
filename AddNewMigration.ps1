param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$cleanedName = [Regex]::Replace($MigrationName, "\s", "_")

dotnet ef migrations add -s src/tools/BrickMoney.MigrationHelper -p src/app/BrickMoney "$cleanedName"
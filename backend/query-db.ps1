$connectionString = 'Server=(localdb)\\mssqllocaldb;Database=LM;Trusted_Connection=True;'
$connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
$connection.Open()
$command = $connection.CreateCommand()
$command.CommandText = @'
SELECT COUNT(*) AS UsersCount FROM dbo.Users;
SELECT COUNT(*) AS RolesCount FROM dbo.Roles;
SELECT COUNT(*) AS LeaveTypesCount FROM dbo.LeaveTypes;
SELECT COUNT(*) AS LeaveRequestsCount FROM dbo.LeaveRequests;
'@
$reader = $command.ExecuteReader()
$counts = @()
$index = 0
while ($true) {
    if ($reader.Read()) {
        $counts += $reader.GetInt32(0)
    } else {
        $counts += 0
    }
    if (-not $reader.NextResult()) { break }
}
$connection.Close()
Write-Host "Users=$($counts[0]) Roles=$($counts[1]) LeaveTypes=$($counts[2]) LeaveRequests=$($counts[3])"
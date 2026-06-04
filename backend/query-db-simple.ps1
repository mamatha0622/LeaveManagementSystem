$connectionString = 'Server=(localdb)\\mssqllocaldb;Database=LM;Trusted_Connection=True;'
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection $connectionString
    $connection.Open()
    Write-Host 'Connection opened.'
    foreach ($sql in @('SELECT COUNT(*) FROM dbo.Users;', 'SELECT COUNT(*) FROM dbo.Roles;', 'SELECT COUNT(*) FROM dbo.LeaveTypes;', 'SELECT COUNT(*) FROM dbo.LeaveRequests;')) {
        $cmd = $connection.CreateCommand()
        $cmd.CommandText = $sql
        $count = $cmd.ExecuteScalar()
        Write-Host "$sql -> $count"
    }
    $connection.Close()
} catch {
    Write-Host 'ERROR:'
    $_ | Format-List * -Force
}
$connectionStrings = @(
    'Server=LIN-5CG22145FF;Database=LM;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=True',
    'Server=LIN-5CG22145FF;Database=LM;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True'
)
foreach ($cs in $connectionStrings) {
    Write-Host "Testing: $cs"
    try {
        $conn = New-Object System.Data.SqlClient.SqlConnection $cs
        $conn.Open()
        Write-Host "SUCCESS"
        $conn.Close()
    } catch {
        Write-Host "FAIL: $($_.Exception.Message)"
    }
    Write-Host "---"
}

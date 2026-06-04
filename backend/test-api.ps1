$body = @{ username = 'employee'; password = 'password' }
$json = $body | ConvertTo-Json
try {
    $login = Invoke-RestMethod -Uri 'http://localhost:5011/api/auth/login' -Method Post -Body $json -ContentType 'application/json'
    Write-Output 'LOGIN_OK'
    Write-Output $login
    $token = $login.token
    $leave = @{ leaveTypeId = 1; fromDate = (Get-Date).AddDays(1).ToString('o'); toDate = (Get-Date).AddDays(2).ToString('o') }
    $leaveJson = $leave | ConvertTo-Json
    $apply = Invoke-RestMethod -Uri 'http://localhost:5011/api/leaves/apply' -Method Post -Headers @{ Authorization = "Bearer $token" } -Body $leaveJson -ContentType 'application/json'
    Write-Output 'APPLY_OK'
    Write-Output ($apply | ConvertTo-Json -Depth 5)
    $leaves = Invoke-RestMethod -Uri 'http://localhost:5011/api/leaves' -Method Get -Headers @{ Authorization = "Bearer $token" }
    Write-Output 'LEAVES_OK'
    Write-Output ($leaves | ConvertTo-Json -Depth 5)
} catch {
    Write-Output 'ERROR'
    $_ | Format-List * -Force
}
$serverDir = [System.Environment]::GetEnvironmentVariable("PROJECT_ASSETS_SERVER_DIR", "Machine")
$localDir = [System.Environment]::GetEnvironmentVariable("PROJECT_ASSETS_LOCAL_DIR", "Machine")
$isEnable = $TRUE

if(Test-Path -Path $serverDir){
    Write-Output "server aru"
}else{
    Write-Output "server nai"
    $isEnable = $FALSE
}

if(Test-Path -Path $localDir){
    Write-Output "local aru"
}else{
    Write-Output "local nai"
    $isEnable = $FALSE
}

if($isEnable){
    Robocopy $serverDir $localDir /MIR
}
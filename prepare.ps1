$shellPath = Get-Location
[System.Environment]::SetEnvironmentVariable("PROJECT_ROOT", $shellPath, "Machine")

$scriptsDirPath = Join-Path $shellPath "\Assets\Scripts"
$resourcesDirPath = Join-Path $shellPath "\Assets\Resources"
$projectAssetsDirPath = Join-Path $resourcesDirPath "\ProjectAssets"

if( Test-Path $scriptsDirPath ){

}else{
    New-Item $scriptsDirPath -ItemType Directory
}

if( Test-Path $resourcesDirPath ){

}else{
    New-Item $resourcesDirPath -ItemType Directory
}

if( Test-Path $projectAssetsDirPath ){

}else{
    New-Item $projectAssetsDirPath -ItemType Directory
}

[System.Environment]::SetEnvironmentVariable("PROJECT_ASSETS_LOCAL_DIR", $projectAssetsDirPath, "Machine")
[System.Environment]::SetEnvironmentVariable("PROJECT_ASSETS_SERVER_DIR", "\\201a01\2023_TeamProject_Share\2023_TeamProject02", "Machine")

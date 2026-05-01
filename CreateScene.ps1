$sceneContentFolder = "$PSScriptRoot\scene\*"
$destinationZipFile = "$PSScriptRoot\source\SevenWonders.UI\Resources\Raw\scene.zip"
$destinationZipFile2 = "$PSScriptRoot\source\SevenWonders.Game.Scene.Editor\bin\Debug\net9.0-windows10.0.19041.0\win10-x64\Scenes\scene.zip"

Compress-Archive -Path $sceneContentFolder -DestinationPath $destinationZipFile -Force
Compress-Archive -Path $sceneContentFolder -DestinationPath $destinationZipFile2 -Force
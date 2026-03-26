$sceneContentFolder = "$PSScriptRoot\scene\*"
$destinationZipFile = "$PSScriptRoot\source\SevenWondersUI\Resources\Raw\scene.zip"

Compress-Archive -Path $sceneContentFolder -DestinationPath $destinationZipFile -Force
$region = "westus2"
$rgName = "stc-rg-demo"
$name = "stc-cog-demo"

Write-Host "Log in to your Azure account..."
Connect-AzAccount

Write-Host "Creating resource group..."
New-AzResourceGroup `
    -Location $region `
    -Name $rgName `
    -Force `
| Out-Null

Write-Host "Creating CognitiveServices Account..."
New-AzCognitiveServicesAccount `
    -ResourceGroupName $rgName `
    -Name $name `
    -Type CognitiveServices `
    -SkuName "S0" `
    -Location $region `
    -Force `
| Out-Null

Write-Host "Note: No API exists for creating an CognitiveServices endpoint, so we'll walk through the steps manually..."

Write-Host "Navigating to speech studio..."
Start-Process "https://speech.microsoft.com/portal/844856f4c9ce4426884a3741b3106393/customspeech"

Write-Host "Click 'Create a project'"
Read-Host "`tPress (enter) to continue"

Write-Host "Input details into 'New Project' Form"
Read-Host "`tPress (enter) to continue"

Write-Host "Navigate to your Custom Speech project"
Read-Host "`tPress (enter) to continue"

Write-Host "Click 'Upload data'"
Read-Host "`tPress (enter) to continue"

Write-Host "Click 'Structure text' then 'Next'"
Read-Host "`tPress (enter) to continue"

Write-Host "Select 'config/algebraic-notation.md' to upload"
Read-Host "`tPress (enter) to continue"

Write-Host "Name 'Algebraic Notation Dataset'"
Read-Host "`tPress (enter) to continue"

Write-Host "Review details and click 'Save and close'"
Read-Host "`tPress (enter) to continue"

Write-Host "Click 'Train custom models'"
Read-Host "`tPress (enter) to continue"

Write-Host "Click 'Train a new model'"
Read-Host "`tPress (enter) to continue"

Write-Host "Select 'General' scenario and latest baseline model"
Read-Host "`tPress (enter) to continue"

Write-Host "Choose 'Algebraic Notation Dataset'"
Read-Host "`tPress (enter) to continue"

Write-Host "Name 'Algebraic Notation Model'"
Read-Host "`tPress (enter) to continue"

Write-Host "Click 'Deploy models'"
Read-Host "`tPress (enter) to continue"

Write-Host "Name 'Algebraic Notation Endpoint' and select 'Algebraic Notation Model'"
Read-Host "`tPress (enter) to continue"

Write-Host "Open 'Algebraic Notation Endpoint' to retrieve 'EndpointId', 'Region' and 'Key'"
Read-Host "`tPress (enter) to continue"

Write-Host "Done!"
## STEP 0
# load the secrets

. "C:\Users\ericowens\Desktop\GitHub\cloud\ClusterSetup\ClusterSetup\Development\Secret\AzureIdentityVars.ps1"

$resourceGroupName = "lrp_sf_rg"
$resourceGroupLocation = "westus"
$templateFilePath = "C:\Users\ericowens\Desktop\GitHub\cloud\ClusterSetup\ClusterSetup\Development\SFReverseProxyTemplate.json"
$parametersFilePath = "C:\Users\ericowens\Desktop\GitHub\cloud\ClusterSetup\ClusterSetup\Development\Secret\SFReverseProxyTemplateParameters.json"

Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace;
}

#******************************************************************************
# Script body
# Execution begins here
#******************************************************************************
$ErrorActionPreference = "Stop"

# sign in
Write-Host "Logging in...";
Login-AzureRmAccount;

# select subscription
Write-Host "Selecting subscription '$SubscriptionId'";
Select-AzureRmSubscription -SubscriptionID $SubscriptionId;

# Register RPs
$resourceProviders = @("microsoft.storage","microsoft.network","microsoft.compute","microsoft.servicefabric");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    if(!$resourceGroupLocation) {
        $resourceGroupLocation = Read-Host "resourceGroupLocation";
    }
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
}
else{
    Write-Host "Using existing resource group '$resourceGroupName'";
}

# Test the deployment
# Test-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath

# Start the deployment
Write-Host "Starting deployment"

if(Test-Path $parametersFilePath) {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath;
} else {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath;
}




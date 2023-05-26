# This is for the new Non-Production Environment
$subscription = ""
$rgLocation = "East US"
#************************************************************************
#******* MAKE SURE YOU ARE IN THE RIGHT RG BY SETTING ENVIRONMENT *******
$environment = "ETL" ##Dev/QA/UAT/STAGE/ETL
$rgName = "LL-${environment}"
#************************************************************************

# Set which resources to deploy
$deployResourceGroup = $false
$deployAppServices = $false
$deployStorageAccount = $false
$deployKeyVault = $true
$deployFunctionApp = $false

## Set Azure Subscription
Set-AzContext `
   -Subscription $subscription

# Tags
$agency = "LatvianVillageCloudware"
$project = "WeShopAlot"
$division = "ECommerce"

## Deploy Resource Group
if ($deployResourceGroup)
{
   New-AzResourceGroup `
      -Name $rgName `
      -Location $rgLocation `
      -Tag @{agency=$agency; project=$project; division=$division; environment=$environment}
}

## Deploy App Services and Application Insights
if ($deployAppServices)
{
   $templateFile = "DeployLowerEnvironment_AppServices.json"
   $templateParameterFile = "${environment}/AppServices.parameters.json"

   New-AzResourceGroupDeployment `
      -ResourceGroupName $rgName `
      -TemplateFile $templateFile `
      -TemplateParameterFile $templateParameterFile
}

## Deploy Storage Account
if ($deployStorageAccount)
{
   $templateFile = "DeployLowerEnvironment_StorageAccount.json"
   $templateParameterFile = "${environment}/StorageAccount.parameters.json"

   New-AzResourceGroupDeployment `
      -ResourceGroupName $rgName `
      -TemplateFile $templateFile `
      -TemplateParameterFile $templateParameterFile
}

## Deploy KeyVault
if ($deployKeyVault)
{
   $templateFile = "DeployLowerEnvironment_KeyVault.json"
   $templateParameterFile = "${environment}/KeyVault.parameters.json"

   New-AzResourceGroupDeployment `
      -ResourceGroupName $rgName `
      -TemplateFile $templateFile `
      -TemplateParameterFile $templateParameterFile
}

## Deploy FunctionApp
if ($deployFunctionApp)
{
   $templateFile = "DeployLowerEnvironment_FunctionApp.json"
   $templateParameterFile = "${environment}/KeyVault.parameters.json"

   New-AzResourceGroupDeployment `
      -ResourceGroupName $rgName `
      -TemplateFile $templateFile `
      -TemplateParameterFile $templateParameterFile `
      -WhatIf
}

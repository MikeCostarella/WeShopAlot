# This is for the new NonProd dev Resource Group and Non-Prod Environment
$subscription = ""
$rgLocation = "East US"
#************************************************************************
#********** MAKE SURE YOU ARE IN THE RIGHT RG ***************************
$rgName = "WSA-NonProd"
#************************************************************************

# Tags
$agency = "LatvianVillageCloudware"
$project = "WeShopAlot"
$division = "ECommerce"
$environment = "nonprod"

# App Service Plan Name
$appServicePlanName = "WSA-WebApp"

# Set which resources to deploy
$deployResourceGroup = $false
$deployAppServicePlan = $false

## Set Azure Subscription
Set-AzContext `
   -Subscription $subscription

## Deploy Resource Group
if ($deployResourceGroup)
{
   New-AzResourceGroup `
      -Name $rgName `
      -Location $rgLocation `
      -Tag @{agency=$agency; project=$project; division=$division; environment=$environment}
}

## Deploy App Service Plan
if ($deployAppServicePlan)
{
   $templateFile = "DeployNonProd_AppServicePlan.json"
   $templateParameterFile = "DeployNonProd_AppServicePlan.parameters.json"

   New-AzResourceGroupDeployment `
   -ResourceGroupName $rgName `
   -TemplateFile $templateFile `
   -TemplateParameterFile $templateParameterFile `
   -resourceGroupLocation $rgLocation `
   -appServicePlanName $appServicePlanName `
   -agency $agency `
   -project $project `
   -division $division `
   -environment $environment
}

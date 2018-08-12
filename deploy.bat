az login --service-principal -u %3 -p %4 --tenant %5
az webapp deployment source config-zip -n %1 -g %2 --src AuditLogApp.zip --slot staging
az webapp start -n %1 -g %2 --slot staging
az webapp deployment slot swap -n %1 -g %2 --slot staging
az webapp stop -n %1 -g %2 --slot staging


REM az login --service-principal -u %AzureCLI.Username% -p %AzureCLI.Password% --tenant %AzureCLI.Tenant%
REM az webapp deployment source config-zip -n %AzureAppWebsiteName% -g %AzureAppResourceGroup% --src AuditLogApp.zip --slot staging
REM az webapp start -n %AzureAppWebsiteName% -g %AzureAppResourceGroup% --slot staging
REM az webapp deployment slot swap -n %AzureAppWebsiteName% -g %AzureAppResourceGroup% --slot staging
REM az webapp stop -n %AzureAppWebsiteName% -g %AzureAppResourceGroup% --slot staging

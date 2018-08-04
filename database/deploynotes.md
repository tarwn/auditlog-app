
CI
=========================

CI is going to eventually do client and server tests.

1. `npm install`
2. `npm run build` (csproj file has an explicit include of the Content/build folder)
3. `dotnet build`
4. Build and apply scripts from the `database` folder to local DB to ensure they work

Package for Deploy
=========================

1. `npm install`
2. `npm run build` (csproj file has an explicit include of the Content/build folder)
3. `dotnet publish`
4. Rely on the artifact settings to take the publish folder from #3 and zip it

Azure Deploy
=========================

* Take artifacts from prior step: AuditLogApp.zip, update.sql, DB update posh scripts
* Use azure CLI
* Create a service principal [reference](https://docs.microsoft.com/en-us/cli/azure/create-an-azure-service-principal-azure-cli?view=azure-cli-latest), using --scopes to scope to one subscription
    * Using the password option (lastpass: AuditLogDeployUser, TeamCity parameter)
* Build server will:
    * `az login` w/ service principal options
    * `az webapp deployment source config-zip -n auditlog-app -g auditlog-app --src AuditLogApp.zip` per [random blog](https://markheath.net/post/four-ways-to-deploy-aspnet-core-website-in-azure)


Debugging Production
=============================

Ugh.

Flip all the switches on, try checking the log stream.

Nuclear option, add Application Settings `ASPNETCORE_ENVIRONMENT` w/ value of `Development`.

Ensuring XML Docs are in Production
=====================================

Modified csproj a few times: https://github.com/dotnet/sdk/issues/795


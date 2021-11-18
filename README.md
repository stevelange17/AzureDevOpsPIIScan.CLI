# AzureDevOpsPIIScan.CLI

This is a sample, semi-working CLI app which scans Azure DevOps work items to try and detect PII text. 

You need to provide:
- the Azure DevOps org name
- [PAT to access ADO](https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page)
- Text Analytics service endpoint
- Text Analytics key

The library will attempt to:
- Connect to Azure DevOps
- Loop through each project
- Loop through each work item type
- Loop through each work item by type
- Send HTML/text fields to Text Analytics
- Output any PII detection to the console

Nothing crazy. This may serve as a starter project for something more robust.

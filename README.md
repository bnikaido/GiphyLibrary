# GiphyLibrary

A simple web application to search and store giphies under a user profile

## Getting Started

To run this application locally, you will need to either [install the Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) or [create a Azure Storage Account](https://docs.microsoft.com/en-us/azure/storage/common/storage-quickstart-create-account?tabs=azure-portal) and update the connection string in the `appsettings.json` file. Blob storage is used for storing user information about saved giphies and tags.

On the first run, navigate to either the login or registration page. After entering your information, you may be redirected to migrate data with entity framework. This will setup the database for the user identity, and will only need to be migrated the first time you run the application.

### Prerequisites

You will need the following tools:

* [Visual Studio Code ](https://code.visualstudio.com/) or [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
* .NET Core SDK 3
* Node.js (version 10 or later) with npm (version 6.9.0 or later)

If you install Visual Studio 2019, .NET Core SDK 3 is included by default and Node.js can be included by selecting the option.

## Deployment

I deployed this web application [here](https://giphylibrary20191005085550.azurewebsites.net) to Azure using the publish option in Visual Studio 2019 by [following these instructions](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-azure-webapp-using-vs?view=aspnetcore-3.0)
* If you deploy this application framework dependent, be sure to [add the extension for dotnet core 3.0 preview](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/azure-apps/index?view=aspnetcore-3.0&tabs=visual-studio#install-the-preview-site-extension) otherwise the web app will not run in the deployed environment
* Don't forget to [add a certificate to the new web app](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-custom-ssl) which is required by the identity server

## Built With

* [ASP.Net Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.0&tabs=visual-studio)
* [GiphyDotNet](https://github.com/drasticactions/GiphyDotNet) - For communication with Giphy

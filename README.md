# GoogleRestAPIWrapper
Google Rest API Wrapper for C#  
Currently only supports Google's `Cloud Translation API`, just a template for other developers to adopt to their projects. Example projects is included.

### How it works:  
When logging into the client it generates a `JWT` for authenticating to Google API which gives you an `access token` to access the different scopes of the Google API. All API requests are done through POST and GET requests to the REST API and outputs the results as a Json format which the information can then be extracted for application use.  

This project has 2 version of the wrapper, one that supports .NET Framework and the other supports NET Core.
### Wrappers:  
- TranslationAPI.dll -Built for `.Net Framework 4.6`  
- TranslationAPICore.dll -Built for `.Net Core 3.1`  

Though the .dll were built for the specified version mentioned above, it might work with older and or newer versions.  

### Required Libraries:  
- `Newtonsoft.Json` -Can be installed on NuGet or using the NuGet command: `Install-Package Newtonsoft.Json`  
- `jose-jwt` -Can be installed on NuGet or using the NuGet command: `Install-Package jose-jwt`  


### Usage:  
To start you need to add these namespaces to your project
```cs
using TranslationAPI;
using TranslationAPI.Structs;
```


Then you need to declare the client which then should be cached somewhere
```cs
public TranslationClient GoogleClient = new TranslationClient();
```


You must then input your credentials, should look something like this. Also keep in mind this wrapper uses a `.p12` file for certification and not a `.json`
```cs
LoginCredential credential = new LoginCredential
{
     P12Path = Application.StartupPath + "/Credentials/myprojectid-8538c731jz3h.p12",
     P12Password = "notasecret",
     ServiceAccountPrivateKeyId = "6747l853fe4h5678db01tu8caa08768ce30796nm",
     ServiceAccountEmail = "myserviceaccount@projectid.iam.gserviceaccount.com",
     AuthenticationTokenURI = "https://oauth2.googleapis.com/token",
     AuthenticationScope = "https://www.googleapis.com/auth/cloud-translation",
     ProjectID = "projectid",
     Endpoint = "https://translation.googleapis.com"
};
```


Before you can make any built-in, custom POST, custom GET requests you have to login (using the credentials similar to above) to get your access token. If you logged in successfully the `AccessToken` string should not be empty. Keep in mind access token expires every 60 minutes and you will have to request a new one when it expires, if you only use the built in requests made for the client you don't have to worry about this since a new access token will be requested when the previous token expires automatically when calling on the request. If you are using your own custom requests you have to call `GoogleClient.CheckValidClient()` it renews your access token if it's expired and returns false if renewing the access token was unsucessful.
```cs
await GoogleClient.LoginAsync(credential);
string accessToken = GoogleClient.AccessToken;
```


Examples of some built-in requests
```cs
LanguageDetectResponse detectResponse = await GoogleClient.DetectLanguageAsync("こんにちは、元気ですか");
SupportedLanguagesResponse languagesResponse = await GoogleClient.GetSupportedLanguagesAsync();
TranslatedTextResponse textResponse = await GoogleClient.TranslateTextAsync("こんにちは、元気ですか");
```


To make your own custom request calls use `GoogleClient.Client` to make your own POST and GET requests as long as it's in the same Google API scope as your credential scope. Don't worry about `Authorization: Bearer` headers as it's already applied to the client when you logged on. Be sure to call `GoogleClient.CheckValidClient()` on your custom requests to ensure the access token has not expired and also attempts to renew it if it did expire.

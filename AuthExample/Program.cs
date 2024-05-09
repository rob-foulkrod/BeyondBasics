using System.Net.Http.Headers;
using Microsoft.Identity.Client;


// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var clientId = "ada94ae4-d090-4e8f-bb6c-b28ed2249f4f";
var tenantId = "8d08f145-2af8-4cd8-ac9a-766faebd0594";

var app = PublicClientApplicationBuilder.Create(clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
    .WithRedirectUri("http://localhost")
    .Build();


var scopes = new string[] { "user.read" };

var result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

Console.WriteLine(result.AccessToken); //Oauth token
//blank space
Console.WriteLine();
Console.WriteLine(result.IdToken);     //OpenID token

var httpClient = new HttpClient() { BaseAddress = new Uri("https://graph.microsoft.com/v1.0/") };
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);


var response = await httpClient.GetAsync("me");
if (response.IsSuccessStatusCode)
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(content);
}
else
{
    Console.WriteLine(response.StatusCode);
}
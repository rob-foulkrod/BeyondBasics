// dotnet add package Microsoft.Azure.Cosmos
// dotnet add package Microsoft.Extensions.Configuration.UserSecrets

using System.Reflection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello, Cosmos DB!");

string uri = "https://beyondbasics-cosmos.documents.azure.com:443/";
string key = GetUserSecrets("CosmosDbKey");

var client = new CosmosClient(uri, key);
var dbResponse = await client.CreateDatabaseIfNotExistsAsync("BycyclesDB");

var database = dbResponse.Database;

var containerResponse = await database.CreateContainerIfNotExistsAsync("Bycycles", "/id");
var container = containerResponse.Container;

var bike1 = new { id = Guid.NewGuid().ToString(), brand = "Brompton", type = "Folding" };
var bike2 = new { id = Guid.NewGuid().ToString(), brand = "Shwin", type = "10 Speed" };
var bike3 = new { id = Guid.NewGuid().ToString(), brand = "Trek", type = "Mountain" };

//in a relational database we pass the query to the Server
//In a Cosmos DB we pass the query to the container
var bike1Response = await container.CreateItemAsync(bike1);
var bike2Response = await container.CreateItemAsync(bike2);
var bike3Response = await container.CreateItemAsync(bike3);

Console.WriteLine($"Bike 1: {bike1Response.Resource.id} and a RU of {bike1Response.RequestCharge}");
Console.WriteLine($"Bike 2: {bike2Response.Resource.id} and a RU of {bike2Response.RequestCharge}");
Console.WriteLine($"Bike 3: {bike3Response.Resource.id} and a RU of {bike3Response.RequestCharge}");



//Query up the treck bike
var sql = "SELECT * FROM c WHERE c.brand = 'Trek'";
var query = container.GetItemQueryIterator<Bike>(sql);
var results = await query.ReadNextAsync();

foreach (Bike bike in results)
{
    Console.WriteLine($"Bike: {bike.brand} - {bike.type}");
}

















string GetUserSecrets(string key)
{
    var configBuilder = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly(), true);
    var config = configBuilder.Build();
    // Get the storage account key from the user secrets
    var storageAccountKey = config[key] ?? throw new Exception($"{key} is not in user-secrets"); ;

    return storageAccountKey;
}

class Bike
{
    public string id { get; set; }
    public string brand { get; set; }
    public string type { get; set; }
}
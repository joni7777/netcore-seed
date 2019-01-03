using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

namespace BpSeed.Isolate
{
    public class MongoIsolate : IsolateAction
    {
        public MongoIsolate(IHostingEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
        }

        protected override async Task<JObject> GenerateIsolateConfigAsync()
        {
            var collections = Configuration.GetValue<string[]>("MongoDB:Collections");
            var connectionString = MongoUrl.Create(Configuration["MongoDB:ConnectionString"]);
            
            IMongoClient client = new MongoClient(connectionString);

            var newDatabaseName = $"{connectionString.DatabaseName}-{Environment.EnvironmentName}";
            
            var database = client.GetDatabase(newDatabaseName);
            var existingNames = await database.ListCollectionNames().ToListAsync();

            existingNames.Contains("");

            foreach (var collectionName in collections)
            {
                if(existingNames.Contains(collectionName))
                {
                    await database.DropCollectionAsync(collectionName);
                }
                await database.CreateCollectionAsync(collectionName);
                
                var seedData = Configuration[$"MongoDB:SeedData:{collectionName}"];
                if(seedData != null)
                {
                    var data = JArray.Parse(seedData);
                    var collection = database.GetCollection<BsonDocument>(collectionName);
                    foreach (var item in data)
                    {
                        var document = BsonDocument.Parse(item.ToString());
                        await collection.InsertOneAsync(document);
                        Console.WriteLine($"Inserted document {document.GetElement("_id").ToString()} to collection {collectionName}");
                    }
                    
                }
            }

            return JObject.FromObject(new
            {
                MongoDB = new
                {
                    ConnectionString = Configuration["MongoDB:ConnectionString"].Replace(connectionString.DatabaseName, newDatabaseName)
                }
            });
        }
    }
}

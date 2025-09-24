using MongoDB.Driver;

namespace RouteScheduler.Infrastructure.Persistence.Data.Mongo
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(MongoSettings settings)
        {
            var clientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionUri);
            clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(clientSettings);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string? name = null)
        {
            var collectionName = name ?? typeof(T).Name.ToLowerInvariant() + "s";
            return _database.GetCollection<T>(collectionName);
        }
    }
}





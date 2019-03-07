using Microsoft.Extensions.Options;
using MongoDB.Driver;
using webapp.Abstractions;
using webapp.Models;

namespace webapp.Data
{
    public class MongoContext<T> : IMongoContext<T> 
    {
        private readonly IMongoDatabase _database = null;

        public MongoContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<T> Documents
        {
            get =>  _database.GetCollection<T>(typeof(T).Name);

        }
    }
}
//https://www.qappdesign.com/using-mongodb-with-net-core-webapi/
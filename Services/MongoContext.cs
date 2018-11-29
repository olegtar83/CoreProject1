using Microsoft.Extensions.Options;
using MongoDB.Driver;
using webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp.Services
{
    public class MongoContext : IMongoContext<T>
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
            get
            {
                return _database.GetCollection<T>(nameof(T));
            }
        }
    }
}
//https://www.qappdesign.com/using-mongodb-with-net-core-webapi/
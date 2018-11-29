using MongoDB.Driver;
using webapp.Models;

namespace webapp.Services
{
    public interface IMongoContext
    {
        IMongoCollection<User> Users { get; }
    }
}
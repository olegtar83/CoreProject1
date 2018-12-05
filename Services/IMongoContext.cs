using MongoDB.Driver;

namespace webapp.Services
{
    public interface IMongoContext<T> 
    {
        IMongoCollection<T> Documents { get; }
    }
}
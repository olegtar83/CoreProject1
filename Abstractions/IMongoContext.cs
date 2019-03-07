using MongoDB.Driver;

namespace webapp.Abstractions
{
    public interface IMongoContext<T> 
    {
        IMongoCollection<T> Documents { get; }
    }
}
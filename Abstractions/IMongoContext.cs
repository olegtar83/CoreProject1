using MongoDB.Driver;

namespace webapp.Services.Abstractions
{
    public interface IMongoContext<T> 
    {
        IMongoCollection<T> Documents { get; }
    }
}
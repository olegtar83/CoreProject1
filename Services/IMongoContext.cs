using MongoDB.Driver;
using webapp.Models;

namespace webapp.Services
{
    public interface IMongoContext<T>
    {
        IMongoCollection<T> Documents { get; }
    }
}
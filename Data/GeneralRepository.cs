using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Services.Abstractions;

namespace webapp
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T: BaseEntity
    {
        private readonly IMongoContext<T> _context;
        public GeneralRepository(IMongoContext<T> context ) => this._context = context;

        public async Task<int> GetNextAutoincrementValue()
        {
            try
            {
                var res = await _context.Documents.Find((T x) => true).SortByDescending(s => s.Id).Limit(1)
                    .FirstOrDefaultAsync();
                return res.Id + 1;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
                
        }

        public async Task<bool> RemoveDocument(string id)
        {
            var actionResult
                           = await _context.Documents.DeleteOneAsync(
                               Builders<T>.Filter.Eq("Id", id));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<T> GetDocument(int id)
        {
            ObjectId internalId = GetInternalId(id);
            return await _context.Documents
                            .Find((T u) => u.Id == id
                                    || u.InternalId == internalId)
                            .FirstOrDefaultAsync();

        }

        public async Task<bool> RemoveAllDocuments()
        {
            DeleteResult actionResult
                          = await _context.Documents.DeleteManyAsync<T>(x=>true);

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<T>> GetAllDocuments()
        {
            var documents = await _context.Documents.Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task AddDocument(T item)
        {          
            await _context.Documents.InsertOneAsync(item);
        }
        public async Task<bool> UpdateDocument(T item, int id)
        {
            var actionResult
                           = await _context.Documents
                                           .ReplaceOneAsync(n => n.Id.Equals(id)
                                                   , item
                                                   , new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }
        private ObjectId GetInternalId(int id)
        {
            return (!ObjectId.TryParse(id.ToString(), out var internalId)) ? internalId = ObjectId.Empty : internalId;
        }
        public async Task<bool> UpdateDocument(T item,FilterDefinition<T> filter,UpdateDefinition<T> update)
        {
            var actionResult
               = await _context.Documents.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        ObjectId IGeneralRepository<T>.GetInternalId(int id)
        {
            throw new NotImplementedException();
        }
    }
}

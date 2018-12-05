using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Models;
using webapp.Services.Abstractions;

namespace webapp.Services
{
  
    public class UserRepository : IUserRepository
    {
        private IMongoContext<User> _context;
        private IEncription _crypt;
        public UserRepository(IMongoContext<User> context,IEncription crypt) {
            this._context = context;
            this._crypt = crypt;
        }

        public async Task AddUser(User item)
        {
             string hashedPass;
             _crypt.CreateValueHash(item.Password, out hashedPass);
             item.Password = hashedPass;
             await _context.Documents.InsertOneAsync(item);
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            var documents = await _context.Documents.Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task<User> GetUser(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await _context.Documents
                            .Find(u => u.Id == id
                                    || u.InternalId == internalId)
                            .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<User>> GetUsers(string id, DateTime updatedFrom, long headerSizeLimit)
        {
            var query = _context.Documents.Find(u =>  u.UpdatedOn >= updatedFrom &&
                                            u.HeaderImage.ImageSize <= headerSizeLimit);
            return await query.ToListAsync();
        }

        public async Task<User> LoginUser(string userName, string password)
        {
            var query = _context.Documents.Find(u => u.UserName == userName && u.Password == password);
            return await query.FirstOrDefaultAsync();
        }

        public Task<bool> RemoveAllUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveUser(string id)
        {
            DeleteResult actionResult
                           = await _context.Documents.DeleteOneAsync(
                               Builders<User>.Filter.Eq("Id", id));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateUser(User item)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Id, item.Id);
            var update = Builders<User>.Update
                       .Set(s => s.UserName, item.UserName)
                       .Set(s=>s.Password,item.Password)
                       .CurrentDate(s => s.UpdatedOn);
            UpdateResult actionResult
               = await _context.Documents.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateUser(User item, string id)
        {
            ReplaceOneResult actionResult
                           = await _context.Documents
                                           .ReplaceOneAsync(n => n.Id.Equals(id)
                                                   , item
                                                   , new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateUserDocument(string id, string Name)
        {
            var item = await GetUser(id) ?? new User();
            item.UserName = Name;
            item.UpdatedOn = DateTime.Now;

            return await UpdateUser(item, id);
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            return (!ObjectId.TryParse(id, out internalId)) ? internalId = ObjectId.Empty : internalId;            
        }
    }
}

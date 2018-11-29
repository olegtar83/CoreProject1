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
        private IMongoContext _context;
        public UserRepository(IMongoContext context) {
            this._context = context;
        }

        public async Task AddUser(User item)
        {
            await _context.Users.InsertOneAsync(item);
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            var documents = await _context.Users.Find(_ => true).ToListAsync();
            return documents;
        }

        public async Task<User> GetUser(string id)
        {
            ObjectId internalId = GetInternalId(id);
            return await _context.Users
                            .Find(u => u.Id == id
                                    || u.InternalId == internalId)
                            .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<User>> GetUsers(string id, DateTime updatedFrom, long headerSizeLimit)
        {
            var query = _context.Users.Find(u =>  u.UpdatedOn >= updatedFrom &&
                                            u.HeaderImage.ImageSize <= headerSizeLimit);
            return await query.ToListAsync();
        }

        public async Task<User> LoginUser(string userName, string password)
        {
            var query = _context.Users.Find(u => u.Name == userName && u.Password == password);
            return await query.FirstOrDefaultAsync();
        }

        public Task<bool> RemoveAllUser()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveUser(string id)
        {
            DeleteResult actionResult
                           = await _context.Users.DeleteOneAsync(
                               Builders<User>.Filter.Eq("Id", id));

            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateUser(User item)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Id, item.Id);
            var update = Builders<User>.Update
                       .Set(s => s.Name, item.Name)
                       .Set(s=>s.Password,item.Password)
                       .CurrentDate(s => s.UpdatedOn);
            UpdateResult actionResult
               = await _context.Users.UpdateOneAsync(filter, update);

            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateUser(User item, string id)
        {
            ReplaceOneResult actionResult
                           = await _context.Users
                                           .ReplaceOneAsync(n => n.Id.Equals(id)
                                                   , item
                                                   , new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged
                && actionResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdateUserDocument(string id, string Name)
        {
            var item = await GetUser(id) ?? new User();
            item.Name = Name;
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

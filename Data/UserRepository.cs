using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using webapp.Abstractions;
using webapp.Models;

namespace webapp.Data
{
  
    public class UserRepository : IUserRepository
    {
        private readonly IMongoContext<User> _context;
        private readonly IGeneralRepository<User>_genRepo;
        private readonly IEncryption _crypt;
        public UserRepository(IMongoContext<User> context,IGeneralRepository<User>genRepo,IEncryption crypt) {
            this._context = context;
            this._crypt = crypt;
            this._genRepo = genRepo;
        }

        public async Task AddUser(User item)
        {
             item.Id= await  _genRepo.GetNextAutoincrementValue();
             item.Password = _crypt.CreateValueHash(item.Password);
            await _context.Documents.InsertOneAsync(item);
        }

        public async Task<IEnumerable<User>> GetUsers(string id, DateTime updatedFrom, long headerSizeLimit)
        {
            var query = _context.Documents.Find(u =>  u.UpdatedOn >= updatedFrom &&
                                            u.HeaderImage.ImageSize <= headerSizeLimit);
            return await query.ToListAsync();
        }

        public async Task<User> LoginUser(string userName, string password)
        {
            password = _crypt.CreateValueHash(password);
           var query = _context.Documents.Find(u => u.UserName == userName && u.Password == password);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(User item)
        {
            var filter = Builders<User>.Filter.Eq(s => s.Id, item.Id);
            var update = Builders<User>.Update
                       .Set(s => s.UserName, item.UserName)
                       .Set(s=>s.Password,item.Password)
                       .CurrentDate(s => s.UpdatedOn);
               return await _genRepo.UpdateDocument(item,filter,update);
        }

       

        public async Task<bool> UpdateUserDocument(int id, string name)
        {
            var item = await _genRepo.GetDocument(id) ?? new User();
            item.UserName = name;
            item.UpdatedOn = DateTime.Now;

            return await _genRepo.UpdateDocument(item, id);
        }

      
    }
}

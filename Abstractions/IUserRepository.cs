using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapp.Models;

namespace webapp.Services.Abstractions
{
     public  interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUser();

        Task<User> LoginUser(string userName,string password);
        Task<User> GetUser(string id);

        // query after multiple parameters
        Task<IEnumerable<User>> GetUsers(string id, DateTime updatedFrom, long headerSizeLimit);

        // add new note document
        Task AddUser(User item);

        // remove a single document / note
        Task<bool> RemoveUser(string id);

        // update just a single document / note
        Task<bool> UpdateUser(User item);

        Task<bool> UpdateUser(User item, string id);

        // demo interface - full document update
        Task<bool> UpdateUserDocument(string id, string Name);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllUser();
    }
}

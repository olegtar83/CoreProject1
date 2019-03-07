using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapp.Models;

namespace webapp.Abstractions
{
     public  interface IUserRepository
    {

        Task<User> LoginUser(string userName,string password);

        // query after multiple parameters
        Task<IEnumerable<User>> GetUsers(string id, DateTime updatedFrom, long headerSizeLimit);

        // add new note document
        Task AddUser(User item);

        Task<bool> UpdateUser(User item);
    }
}

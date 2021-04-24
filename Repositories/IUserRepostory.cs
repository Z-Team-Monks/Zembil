using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();

        Task<User> GetUser(int id);

        Task<User> CreateUser(User user);

        Task UpdateUser(User user);

        Task DeleteUser(int id);

    }


}
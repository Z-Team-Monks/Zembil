using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zembil.Services
{
    public interface IAccountService
    {
        public Task<string> Authenticate(string username, string password);
        int Decrypt(string authHeader);
        string HashPassword(string Password);
        bool VerifyPassword(string Password, string hashedPassword);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Repositories;

namespace Zembil.Services
{
    public class AccountService : IAccountService
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IRepositoryWrapper _repoUser;


        public AccountService(IRepositoryWrapper repoWrapper, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _repoUser = repoWrapper;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var users = await _repoUser.UserRepo.GetAll();

            var user = users.Find(u => u.Username == username && VerifyPassword(password, u.Password));
            if (user == null)
            {
                return null;
            }
            return _jwtAuthenticationManager.Authenticate(user.UserId);
        }

        //Decrypts the bearer token from the authentication header
        public int Decrypt(string authHeader)
        {
            return Convert.ToInt32(_jwtAuthenticationManager.Decrypt(authHeader));
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

    }
}

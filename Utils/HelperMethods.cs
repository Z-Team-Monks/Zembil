using System.Threading.Tasks;
using Zembil.ErrorHandler;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;

namespace Zembil.Utils
{
    public class HelperMethods
    {
        public static HelperMethods helperMethodsInstance;
        private IRepositoryWrapper _repoWrapper { get; set; }
        private IAccountService _accountServices;
        private HelperMethods(IRepositoryWrapper repoWrapper, IAccountService accountServices)
        {
            _repoWrapper = repoWrapper;
            _accountServices = accountServices;
        }

        public static HelperMethods getInstance(IRepositoryWrapper repoWrapper, IAccountService accountServices)
        {
            if (helperMethodsInstance == null)
            {
                return new HelperMethods(repoWrapper, accountServices);
            }

            return helperMethodsInstance;
        }

        public async Task<User> getUserFromHeader(string authHeader)
        {
            int tokenid = _accountServices.Decrypt(authHeader);
            var userExists = await _repoWrapper.UserRepo.Get(tokenid);
            if (userExists == null)
            {
                throw new CustomAppException(new ErrorDetail() { StatusCode = 401, Message = "You are not authorized for this action!", Status = "fail" });
            }
            return userExists;
        }
    }
}
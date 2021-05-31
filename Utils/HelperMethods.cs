using System.Linq;
using System.Threading.Tasks;
using Zembil.ErrorHandler;
using Zembil.Models;
using Zembil.Repositories;
using Zembil.Services;
using Zembil.Views;

namespace Zembil.Utils
{
    public class HelperMethods
    {
        public static HelperMethods helperMethodsInstance;
        public static HelperMethods helperMethodsInstanceEmpty;
        private IRepositoryWrapper _repoWrapper { get; set; }
        private IAccountService _accountServices;

        private HelperMethods()
        {
        }
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
        public static HelperMethods getInstanceEmpty()
        {
            if (helperMethodsInstanceEmpty == null)
            {
                return new HelperMethods();
            }

            return helperMethodsInstanceEmpty;
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

        public string getStatusForShop(bool? isActive)
        {
            switch (isActive)
            {
                case null:
                    return "Pending";
                case true:
                    return "Approved";
                case false:
                    return "Declined";
            }
        }

        public async Task<bool> ValidateProduct(IRepositoryWrapper repoWrapper,ProductCreateDto newProduct)
        {
            var categories = await repoWrapper.CategoryRepo.GetAll();
            if (!categories.Any(c => c.CategoryId == newProduct.CategoryId))
            {
                return false;
            }
            return true;
        }
    }
}
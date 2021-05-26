using System.Threading.Tasks;
using Zembil.DbContexts;

namespace Zembil.Repositories
{
    public class WrapperRepository : IRepositoryWrapper
    {
        private ZembilContext _dbContext;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IShopRepository _shopRepository;

        public WrapperRepository(ZembilContext context)
        {
            _dbContext = context;
        }
        public IUserRepository UserRepo
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_dbContext);
                }
                return _userRepository;
            }
        }
        public IProductRepository ProductRepo
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_dbContext);
                }
                return _productRepository;
            }
        }

        public IShopRepository ShopRepo
        {
            get
            {
                if (_shopRepository == null)
                {
                    _shopRepository = new ShopRepository(_dbContext);
                }
                return _shopRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
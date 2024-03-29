using System.Threading.Tasks;
using Zembil.DbContexts;
//using Zembil.Repositories.WishListRepository;

namespace Zembil.Repositories
{
    public class WrapperRepository : IRepositoryWrapper
    {
        private ZembilContext _dbContext;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IShopRepository _shopRepository;
        private IWishListRepository _wishListRepository;
        private IReviewRepository _reviewRepository;
        private ICategoryRepository _categoryRepository;
        private ILocationRepository _locationRepository;
        private IAdsRepository _adsRepository;
        private INotificationRepository _notificationRepository;

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

        public IWishListRepository WishListRepo
        {
            get
            {
                if (_wishListRepository == null)
                {
                    _wishListRepository = new WishListRepository(_dbContext);
                }
                return _wishListRepository;
            }
        }
        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_dbContext);
                }
                return _categoryRepository;
            }
        }

        public IReviewRepository ReviewRepo
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_dbContext);
                }
                return _reviewRepository;
            }
        }
        public ILocationRepository LocationRepo
        {
            get
            {
                if (_locationRepository == null)
                {
                    _locationRepository = new LocationRepository(_dbContext);
                }
                return _locationRepository;
            }
        }
        public IAdsRepository AdsRepo
        {
            get
            {
                if (_adsRepository == null)
                {
                    _adsRepository = new AdsRepository(_dbContext);
                }
                return _adsRepository;
            }
        }

	public INotificationRepository NotificationRepo
        {
            get
            {
                if (_notificationRepository == null)
                {
                    _notificationRepository = new NotificationRepository(_dbContext);
                }
                return _notificationRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
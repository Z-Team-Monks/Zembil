using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.Models;

namespace Zembil.Repositories
{
    public interface IWishListRepository : IRepositoryBase<WishListItem>
    {
        Task<IEnumerable<WishListItem>> GetCart(int userid);
    }
}

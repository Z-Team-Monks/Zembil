using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class WishListRepository : RepositoryBase<WishListItem>, IWishListRepository
    {
        public WishListRepository(ZembilContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WishListItem>> GetCart(int userid)
        {
            var cart = await _databaseContext.WishLists.Where(w => w.UserId == userid).ToListAsync();
            return cart;
        }
    }
}

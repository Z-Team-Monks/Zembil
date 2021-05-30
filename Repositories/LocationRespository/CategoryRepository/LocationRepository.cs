using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class LocationRepository : RepositoryBase<ShopLocation>, ILocationRepository
    {
        public LocationRepository(ZembilContext context) : base(context)
        {
        }
    }
}
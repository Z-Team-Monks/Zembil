using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class AdsRepository : RepositoryBase<Ads>, IAdsRepository
    {
        public AdsRepository(ZembilContext context) : base(context)
        {
        }
    }
}
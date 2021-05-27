using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Models;

namespace Zembil.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ZembilContext context) : base(context)
        {
        }
    }
}
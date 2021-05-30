using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zembil.DbContexts;
using Zembil.Utils;

namespace Zembil.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public ZembilContext _databaseContext { get; set; }

        public RepositoryBase(ZembilContext context)
        {
            _databaseContext = context;
        }

        public async Task<T> Add(T entity)
        {
            _databaseContext.Set<T>().Add(entity);
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            var entity = await _databaseContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _databaseContext.Set<T>().Remove(entity);
            await _databaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Get(int id)
        {
            return await _databaseContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _databaseContext.Set<T>().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> FilterModels(QueryFilterParams queryParams)
        {
            List<T> models = await _databaseContext.Set<T>().ToListAsync();
            int TotalItems = models.Count();
            Console.WriteLine($"total: {TotalItems}");

            int Limit = queryParams.Limit;
            int Pagination = queryParams.Pagination;


            Console.WriteLine($"Limit: {Limit} pagination: {Pagination}");
            if (Limit <= 0)
            {
                Limit = 10;
            }
            if (Pagination <= 0)
            {
                Pagination = 1;
            }

            var startIndex = (Pagination - 1) * Limit;
            var count = TotalItems - startIndex;
            var endIndex = Limit < count ? Limit : count;

            if (Limit < TotalItems && startIndex < TotalItems)
            {
                models = models.GetRange(startIndex, endIndex);

            }
            else if (Limit < TotalItems)
            {
                models = models.GetRange(0, Limit);
            }
            return models;
        }


    }
}
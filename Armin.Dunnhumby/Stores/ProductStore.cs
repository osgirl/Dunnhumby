using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Armin.Dunnhumby.Web.Stores
{
    public class ProductStore : Store<Product>, IProductStore
    {
        private const string CacheKey = "_PROD_";
        private readonly IMemoryCache _cache; // Cache for list on inline products drop down

        public ProductStore(ApplicationDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> Search(string name)
        {
            IQueryable<Product> query = Table;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            }

            return await query.ToListAsync();
        }

        public override void Delete(Product product)
        {
            _cache.Remove(CacheKey);
            base.Delete(product);
        }

        public override Product Create(Product entity)
        {
            _cache.Remove(CacheKey);
            return base.Create(entity);
        }

        public override List<Product> List()
        {
            List<Product> data;
            if (_cache.TryGetValue(CacheKey, out data)) return data;

            data = base.List();
                
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(CacheKey, data, cacheEntryOptions);

            return data;
        }

        public override void Update(Product entity)
        {
            _cache.Remove(CacheKey);
            base.Update(entity);
        }

    }
}
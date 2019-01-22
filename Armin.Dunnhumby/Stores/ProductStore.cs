using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Web.Stores
{
    public class ProductStore : Store<Product>, IProductStore
    {
        public ProductStore(ApplicationDbContext dbContext) : base(dbContext)
        {
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
            base.Delete(product);
        }
    }
}
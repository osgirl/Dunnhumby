using System.Collections.Generic;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;

namespace Armin.Dunnhumby.Web.Stores
{
    public interface IProductStore
    {
        Product Create(Product entity);
        Product GetById(int id);
        void Update(Product entity);
        void Delete(Product entity);
        Task<IEnumerable<Product>> Search(string name);
        List<Product> List();
        PagedResult<Product> List(int page, int pageSize = 10);
    }
}
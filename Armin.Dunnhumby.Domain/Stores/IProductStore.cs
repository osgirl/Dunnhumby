using System.Collections.Generic;
using System.Threading.Tasks;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Helpers;

namespace Armin.Dunnhumby.Domain.Stores
{
    public interface IProductStore
    {
        Product Create(Product entity);
        Product GetById(int id);

        bool Exists(int id);

        void Update(Product entity);
        void Delete(Product entity);
        Task<IEnumerable<Product>> Search(string name);
        List<Product> List();
        PagedResult<Product> List(int page, int pageSize = 10);
    }
}
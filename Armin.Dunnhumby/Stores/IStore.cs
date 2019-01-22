using System.Collections.Generic;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;

namespace Armin.Dunnhumby.Web.Stores
{
    public interface IStore<T> where T : EntityBase

    {
        T GetById(int id);

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);


        List<T> List();

        PagedResult<T> List(int page, int pageSize = 10);

        int SaveChanges();

    }
}


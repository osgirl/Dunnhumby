using System.Collections.Generic;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Helpers;

namespace Armin.Dunnhumby.Domain.Stores
{
    public interface IStore<T> where T : EntityBase

    {
        T GetById(int id);

        bool Exists(int id);

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);


        List<T> List();

        PagedResult<T> List(int page, int pageSize = 10);

        int SaveChanges();

    }
}


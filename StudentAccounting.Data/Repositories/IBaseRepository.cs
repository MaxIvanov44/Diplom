using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentAccounting.Data.Repositories
{
    public interface IBaseRepository<T>
    {
        IQueryable<T> GetQuery();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
        IEnumerable<T> GetAll();
    }
}

using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Repositories.Base
{
    public interface IRepository<TEntity, in TPk> where TEntity : class
    {
        IDbContextTransaction BeginTransaction();
        int SaveChanges();

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity, bool softDelete = false);
        void RemoveRange(IEnumerable<TEntity> entities, bool softDelete = true);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        TEntity GetById(TPk id);
        IEnumerable<TEntity> GetAll();
    }
}
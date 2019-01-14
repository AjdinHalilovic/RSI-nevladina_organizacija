using Core.Entities;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DAL.Repositories.Base
{
    public abstract class Repository<TEntity, TPk> : IRepository<TEntity, TPk> where TEntity : class
    {
        protected readonly NevladinaOrgContext Context;
        protected DbConnection DbConnection => Context.Database.GetDbConnection();

        private DbSet<TEntity> _entity;

        protected Repository(NevladinaOrgContext context)
        {
            Context = context;
            _entity = Context.Set<TEntity>();
        }


        #region Select
        public virtual TEntity GetById(TPk id) => _entity.SingleOrDefault(i => !((IEntity)i).IsDeleted && Equals(((IEntity)i).Id, id));

        public virtual IEnumerable<TEntity> GetAll() => _entity.Where(i => !((IEntity)i).IsDeleted);
        #endregion

        #region Add
        public virtual void Add(TEntity entity)
        {
            try
            {
                _entity.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _entity.AddRange(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update
        public virtual void Update(TEntity entity)
        {
            try
            {
                _entity.Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _entity.UpdateRange(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        public virtual void Remove(TEntity entity, bool softDelete = false)
        {
            if (softDelete)
            {
                ((IEntity)entity).IsDeleted = true;
                return;
            }

            try
            {
                _entity.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities, bool softDelete = true)
        {
            if (softDelete)
            {
                foreach (var entity in entities)
                    ((IEntity)entity).IsDeleted = true;

                return;
            }

            try
            {
                _entity.RemoveRange(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region TRANSACTION
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        public int SaveChanges() => Context.SaveChanges();
        #endregion
    }
}
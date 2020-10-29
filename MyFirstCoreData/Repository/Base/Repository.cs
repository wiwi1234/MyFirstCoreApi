using Microsoft.EntityFrameworkCore;
using MyFirstCoreData.Interface;
using MyFirstCoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFirstCoreData.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> 
        where TEntity : class
    {
        protected DbContext DbContext;
        protected DbSet<TEntity> Entity;
        IQueryable<TEntity> _EntityQuery;

        public Repository() : this(new WilliamHighSchoolContext())
        {

        }

        public Repository(WilliamHighSchoolContext database)
        {
            this.DbContext = database;
            this.Entity = this.DbContext.Set<TEntity>();
            _EntityQuery = this.Entity.AsQueryable();
            SetDatabaseContext();
        }

        //Db參數設定
        private void SetDatabaseContext()
        {
            this.DbContext.Database.SetCommandTimeout(180);
        }

        //新增
        public TEntity Create(TEntity entity)
        {
            DbContext.Add(entity);
            return entity;
        }

        //效能的關係 用不太到
        public void Create(IEnumerable<TEntity> entities)
        {
            this.Entity.AddRange(entities);
        }

        //刪除
        public void Delete(TEntity entity)
        {
            this.Entity.Remove(entity);
        }

        //效能的關係 用不太到
        public void Delete(IEnumerable<TEntity> entities)
        {
            this.Entity.RemoveRange(entities);
        }

        //查詢 single 還會找第二筆 , 有的話會抱報錯
        public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return _EntityQuery.FirstOrDefault(predicate);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _EntityQuery;
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _EntityQuery.Where(predicate);
        }

        public int Save()
        {
            return this.DbContext.SaveChanges();
        }
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.DbContext != null)
                {
                    this.DbContext.Dispose();
                    this.DbContext = null;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyFirstCoreData.Interface
{

    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        #region 新增

        TEntity Create(TEntity entity);

        #endregion
        #region 刪除

        void Delete(TEntity entity);

        #endregion
        #region 查詢

        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        #endregion

        //void Update(TEntity entity);

        //find用法
    }
}

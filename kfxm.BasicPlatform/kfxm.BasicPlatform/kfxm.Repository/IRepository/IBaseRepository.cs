using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity;
using System.Linq.Expressions;
using kfxms.Common;
namespace kfxms.IRepository
{
    public interface IBaseRepository<TEntity> where TEntity : HBEntity
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        int Add(TEntity entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        int Add(IEnumerable<TEntity> entitys);

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        int Delete(object id);

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        int Delete(TEntity entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        int Delete(IEnumerable<TEntity> entitys);

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        int Delete(Expression<Func<TEntity, bool>> where);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        int Update(TEntity entity);


        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        int Update(IEnumerable<TEntity> entitys);


        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        TEntity GetByKey(object key);


         /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        IEnumerable<TEntity> GetAllData();

         /// <summary>
         /// 根据条件查询
         /// </summary>
        /// <returns>实体对象集合</returns>
        IEnumerable<TEntity> GetWhereData(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        IEnumerable<TEntity> GetWhereData<T>(Expression<Func<TEntity, bool>> where,OrderByHelper<TEntity,T> orderBy);

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        IEnumerable<TEntity> GetWhereData(Expression<Func<TEntity, bool>> where, params OrderByHelper<TEntity>[] orderBy);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetPageData<T>(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, out int total, OrderByHelper<TEntity, T> orderBy);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        IEnumerable<TEntity> GetPageData(Expression<Func<TEntity, bool>> where, int pageIndex, int pageSize, out int total, params OrderByHelper<TEntity>[] orderBy);

        }
}

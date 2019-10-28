using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using kfxms.Entity.Supplier;
using kfxms.IService.Supplier;
using kfxms.IRepository.Supplier;

namespace kfxms.ImpService.Supplier
{
    [Export(typeof(IS_SupplierScoreService))]
    public class ImplS_SupplierScoreService : IS_SupplierScoreService
    {
        [Import(typeof(IS_SupplierScoreRepository))]
        public IS_SupplierScoreRepository IS_SupplierScoreRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_SupplierScore"></param>
        /// <returns></returns>
        public int Add(S_SupplierScore entity)
        {
            return IS_SupplierScoreRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_SupplierScore> entitys)
        {
            return IS_SupplierScoreRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_SupplierScoreRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_SupplierScore entity)
        {
            return IS_SupplierScoreRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_SupplierScore> entitys)
        {
            return IS_SupplierScoreRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where)
        {
            return IS_SupplierScoreRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_SupplierScore entity)
        {
            return IS_SupplierScoreRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_SupplierScore> entitys)
        {
            return IS_SupplierScoreRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_SupplierScore GetByKey(object key)
        {
            return IS_SupplierScoreRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_SupplierScore> GetAllData()
        {
            return IS_SupplierScoreRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_SupplierScore> GetWhereData(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where)
        {
            return IS_SupplierScoreRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_SupplierScore> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where, Common.OrderByHelper<S_SupplierScore, T> orderBy)
        {
            return IS_SupplierScoreRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_SupplierScore> GetWhereData(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where, params Common.OrderByHelper<S_SupplierScore>[] orderBy)
        {
            return IS_SupplierScoreRepository_.GetWhereData(where, orderBy);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        public IEnumerable<S_SupplierScore> GetPageData<T>(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_SupplierScore, T> orderBy)
        {
            return IS_SupplierScoreRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="Total">总条数</param>
        /// <returns></returns>
        public IEnumerable<S_SupplierScore> GetPageData(System.Linq.Expressions.Expression<Func<S_SupplierScore, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_SupplierScore>[] orderBy)
        {
            return IS_SupplierScoreRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

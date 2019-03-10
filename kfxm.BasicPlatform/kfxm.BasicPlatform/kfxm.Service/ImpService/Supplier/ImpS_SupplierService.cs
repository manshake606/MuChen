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
    [Export(typeof(IS_SupplierService))]
    public class ImplS_SupplierService : IS_SupplierService
    {
        [Import(typeof(IS_SupplierRepository))]
        public IS_SupplierRepository IS_SupplierRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_Supplier"></param>
        /// <returns></returns>
        public int Add(S_Supplier entity)
        {
            return IS_SupplierRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_Supplier> entitys)
        {
            return IS_SupplierRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_SupplierRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_Supplier entity)
        {
            return IS_SupplierRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_Supplier> entitys)
        {
            return IS_SupplierRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where)
        {
            return IS_SupplierRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_Supplier entity)
        {
            return IS_SupplierRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_Supplier> entitys)
        {
            return IS_SupplierRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_Supplier GetByKey(object key)
        {
            return IS_SupplierRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Supplier> GetAllData()
        {
            return IS_SupplierRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Supplier> GetWhereData(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where)
        {
            return IS_SupplierRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Supplier> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where, Common.OrderByHelper<S_Supplier, T> orderBy)
        {
            return IS_SupplierRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Supplier> GetWhereData(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where, params Common.OrderByHelper<S_Supplier>[] orderBy)
        {
            return IS_SupplierRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_Supplier> GetPageDate<T>(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_Supplier, T> orderBy)
        {
            return IS_SupplierRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_Supplier> GetPageDate(System.Linq.Expressions.Expression<Func<S_Supplier, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_Supplier>[] orderBy)
        {
            return IS_SupplierRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

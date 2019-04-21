using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.IRepository.Project;
using kfxms.Entity.Client;
using kfxms.IService.Client;
using kfxms.IRepository.Client;

namespace kfxms.ImpService.ProjectAndSupplier
{
    [Export(typeof(IS_ProjectAndSupplierService))]
    public class ImplS_ProjectAndSupplierService : IS_ProjectAndSupplierService
    {
        [Import(typeof(IS_ProjectAndSupplierRepository))]
        public IS_ProjectAndSupplierRepository IS_ProjectAndSupplierRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_ProjectAndSupplier"></param>
        /// <returns></returns>
        public int Add(S_ProjectAndSupplier entity)
        {
            return IS_ProjectAndSupplierRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_ProjectAndSupplier> entitys)
        {
            return IS_ProjectAndSupplierRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_ProjectAndSupplierRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_ProjectAndSupplier entity)
        {
            return IS_ProjectAndSupplierRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_ProjectAndSupplier> entitys)
        {
            return IS_ProjectAndSupplierRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where)
        {
            return IS_ProjectAndSupplierRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_ProjectAndSupplier entity)
        {
            return IS_ProjectAndSupplierRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_ProjectAndSupplier> entitys)
        {
            return IS_ProjectAndSupplierRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_ProjectAndSupplier GetByKey(object key)
        {
            return IS_ProjectAndSupplierRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectAndSupplier> GetAllData()
        {
            return IS_ProjectAndSupplierRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectAndSupplier> GetWhereData(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where)
        {
            return IS_ProjectAndSupplierRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectAndSupplier> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where, Common.OrderByHelper<S_ProjectAndSupplier, T> orderBy)
        {
            return IS_ProjectAndSupplierRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectAndSupplier> GetWhereData(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where, params Common.OrderByHelper<S_ProjectAndSupplier>[] orderBy)
        {
            return IS_ProjectAndSupplierRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_ProjectAndSupplier> GetPageDate<T>(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_ProjectAndSupplier, T> orderBy)
        {
            return IS_ProjectAndSupplierRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_ProjectAndSupplier> GetPageDate(System.Linq.Expressions.Expression<Func<S_ProjectAndSupplier, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_ProjectAndSupplier>[] orderBy)
        {
            return IS_ProjectAndSupplierRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
        }

    }
}

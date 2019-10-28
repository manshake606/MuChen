using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using kfxms.Entity.Invoice;
using kfxms.IService.Invoice;
using kfxms.IRepository.Invoice;

namespace kfxms.ImpService.Invoice
{
    [Export(typeof(IS_InvoiceService))]
    public class ImplS_InvoiceService : IS_InvoiceService
    {
        [Import(typeof(IS_InvoiceRepository))]
        public IS_InvoiceRepository IS_InvoiceRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_Invoice"></param>
        /// <returns></returns>
        public int Add(S_Invoice entity)
        {
            return IS_InvoiceRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_Invoice> entitys)
        {
            return IS_InvoiceRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_InvoiceRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_Invoice entity)
        {
            return IS_InvoiceRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_Invoice> entitys)
        {
            return IS_InvoiceRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where)
        {
            return IS_InvoiceRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_Invoice entity)
        {
            return IS_InvoiceRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_Invoice> entitys)
        {
            return IS_InvoiceRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_Invoice GetByKey(object key)
        {
            return IS_InvoiceRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Invoice> GetAllData()
        {
            return IS_InvoiceRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Invoice> GetWhereData(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where)
        {
            return IS_InvoiceRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Invoice> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where, Common.OrderByHelper<S_Invoice, T> orderBy)
        {
            return IS_InvoiceRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Invoice> GetWhereData(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where, params Common.OrderByHelper<S_Invoice>[] orderBy)
        {
            return IS_InvoiceRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_Invoice> GetPageData<T>(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_Invoice, T> orderBy)
        {
            return IS_InvoiceRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_Invoice> GetPageData(System.Linq.Expressions.Expression<Func<S_Invoice, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_Invoice>[] orderBy)
        {
            return IS_InvoiceRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

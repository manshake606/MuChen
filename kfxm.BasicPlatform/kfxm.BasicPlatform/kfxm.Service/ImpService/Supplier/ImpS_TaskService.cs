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
    [Export(typeof(IS_TaskService))]
    public class ImplS_TaskService : IS_TaskService
    {
        [Import(typeof(IS_TaskRepository))]
        public IS_TaskRepository IS_TaskRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_Task"></param>
        /// <returns></returns>
        public int Add(S_Task entity)
        {
            return IS_TaskRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_Task> entitys)
        {
            return IS_TaskRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_TaskRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_Task entity)
        {
            return IS_TaskRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_Task> entitys)
        {
            return IS_TaskRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_Task, bool>> where)
        {
            return IS_TaskRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_Task entity)
        {
            return IS_TaskRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_Task> entitys)
        {
            return IS_TaskRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_Task GetByKey(object key)
        {
            return IS_TaskRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Task> GetAllData()
        {
            return IS_TaskRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Task> GetWhereData(System.Linq.Expressions.Expression<Func<S_Task, bool>> where)
        {
            return IS_TaskRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Task> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_Task, bool>> where, Common.OrderByHelper<S_Task, T> orderBy)
        {
            return IS_TaskRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Task> GetWhereData(System.Linq.Expressions.Expression<Func<S_Task, bool>> where, params Common.OrderByHelper<S_Task>[] orderBy)
        {
            return IS_TaskRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_Task> GetPageData<T>(System.Linq.Expressions.Expression<Func<S_Task, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_Task, T> orderBy)
        {
            return IS_TaskRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_Task> GetPageData(System.Linq.Expressions.Expression<Func<S_Task, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_Task>[] orderBy)
        {
            return IS_TaskRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

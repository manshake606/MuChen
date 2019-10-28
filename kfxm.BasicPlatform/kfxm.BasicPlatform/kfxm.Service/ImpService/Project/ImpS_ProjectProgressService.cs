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

namespace kfxms.ImpService.ProjectProgress
{
    [Export(typeof(IS_ProjectProgressService))]
    public class ImplS_ProjectProgressService : IS_ProjectProgressService
    {
        [Import(typeof(IS_ProjectProgressRepository))]
        public IS_ProjectProgressRepository IS_ProjectProgressRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_ProjectProgress"></param>
        /// <returns></returns>
        public int Add(S_ProjectProgress entity)
        {
            return IS_ProjectProgressRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_ProjectProgress> entitys)
        {
            return IS_ProjectProgressRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_ProjectProgressRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_ProjectProgress entity)
        {
            return IS_ProjectProgressRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_ProjectProgress> entitys)
        {
            return IS_ProjectProgressRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where)
        {
            return IS_ProjectProgressRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_ProjectProgress entity)
        {
            return IS_ProjectProgressRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_ProjectProgress> entitys)
        {
            return IS_ProjectProgressRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_ProjectProgress GetByKey(object key)
        {
            return IS_ProjectProgressRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectProgress> GetAllData()
        {
            return IS_ProjectProgressRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectProgress> GetWhereData(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where)
        {
            return IS_ProjectProgressRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectProgress> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where, Common.OrderByHelper<S_ProjectProgress, T> orderBy)
        {
            return IS_ProjectProgressRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_ProjectProgress> GetWhereData(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where, params Common.OrderByHelper<S_ProjectProgress>[] orderBy)
        {
            return IS_ProjectProgressRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_ProjectProgress> GetPageData<T>(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_ProjectProgress, T> orderBy)
        {
            return IS_ProjectProgressRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_ProjectProgress> GetPageData(System.Linq.Expressions.Expression<Func<S_ProjectProgress, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_ProjectProgress>[] orderBy)
        {
            return IS_ProjectProgressRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }

    }
}

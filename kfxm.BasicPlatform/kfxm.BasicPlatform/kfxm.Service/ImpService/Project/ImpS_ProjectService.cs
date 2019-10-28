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

namespace kfxms.ImpService.Project
{
    [Export(typeof(IS_ProjectService))]
    public class ImplS_ProjectService : IS_ProjectService
    {
        [Import(typeof(IS_ProjectRepository))]
        public IS_ProjectRepository IS_ProjectRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="S_Project"></param>
        /// <returns></returns>
        public int Add(S_Project entity)
        {
            return IS_ProjectRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<S_Project> entitys)
        {
            return IS_ProjectRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return IS_ProjectRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(S_Project entity)
        {
            return IS_ProjectRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<S_Project> entitys)
        {
            return IS_ProjectRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<S_Project, bool>> where)
        {
            return IS_ProjectRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(S_Project entity)
        {
            return IS_ProjectRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<S_Project> entitys)
        {
            return IS_ProjectRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public S_Project GetByKey(object key)
        {
            return IS_ProjectRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Project> GetAllData()
        {
            return IS_ProjectRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Project> GetWhereData(System.Linq.Expressions.Expression<Func<S_Project, bool>> where)
        {
            return IS_ProjectRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Project> GetWhereData<T>(System.Linq.Expressions.Expression<Func<S_Project, bool>> where, Common.OrderByHelper<S_Project, T> orderBy)
        {
            return IS_ProjectRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<S_Project> GetWhereData(System.Linq.Expressions.Expression<Func<S_Project, bool>> where, params Common.OrderByHelper<S_Project>[] orderBy)
        {
            return IS_ProjectRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<S_Project> GetPageData<T>(System.Linq.Expressions.Expression<Func<S_Project, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<S_Project, T> orderBy)
        {
            return IS_ProjectRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<S_Project> GetPageData(System.Linq.Expressions.Expression<Func<S_Project, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<S_Project>[] orderBy)
        {
            return IS_ProjectRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }

    }
}

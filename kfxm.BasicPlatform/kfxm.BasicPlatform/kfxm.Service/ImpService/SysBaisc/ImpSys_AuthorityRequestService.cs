using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using kfxms.Entity.SysBasic;
using kfxms.IService.SysBasic;
using kfxms.IRepository.SysBasic;

namespace kfxms.ImpService.SysBasic
{
    [Export(typeof(ISys_AuthorityRequestService))]
    public class ImplSys_AuthorityRequestService : ISys_AuthorityRequestService
    {
        [Import(typeof(ISys_AuthorityRequestRepository))]
        public ISys_AuthorityRequestRepository ISys_AuthorityRequestRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Sys_AuthorityRequest"></param>
        /// <returns></returns>
        public int Add(Sys_AuthorityRequest entity)
        {
            return ISys_AuthorityRequestRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<Sys_AuthorityRequest> entitys)
        {
            return ISys_AuthorityRequestRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return ISys_AuthorityRequestRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Sys_AuthorityRequest entity)
        {
            return ISys_AuthorityRequestRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<Sys_AuthorityRequest> entitys)
        {
            return ISys_AuthorityRequestRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where)
        {
            return ISys_AuthorityRequestRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(Sys_AuthorityRequest entity)
        {
            return ISys_AuthorityRequestRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<Sys_AuthorityRequest> entitys)
        {
            return ISys_AuthorityRequestRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public Sys_AuthorityRequest GetByKey(object key)
        {
            return ISys_AuthorityRequestRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_AuthorityRequest> GetAllData()
        {
            return ISys_AuthorityRequestRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_AuthorityRequest> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where)
        {
            return ISys_AuthorityRequestRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_AuthorityRequest> GetWhereData<T>(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where, Common.OrderByHelper<Sys_AuthorityRequest, T> orderBy)
        {
            return ISys_AuthorityRequestRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_AuthorityRequest> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where, params Common.OrderByHelper<Sys_AuthorityRequest>[] orderBy)
        {
            return ISys_AuthorityRequestRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<Sys_AuthorityRequest> GetPageDate<T>(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<Sys_AuthorityRequest, T> orderBy)
        {
            return ISys_AuthorityRequestRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<Sys_AuthorityRequest> GetPageDate(System.Linq.Expressions.Expression<Func<Sys_AuthorityRequest, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<Sys_AuthorityRequest>[] orderBy)
        {
            return ISys_AuthorityRequestRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
        }

         /// <summary>
        /// 获取用户有权限的请求
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_AuthorityRequest> GetRequestByUserId(Guid userId) 
        {
            return ISys_AuthorityRequestRepository_.GetRequestByUserId(userId);
        }
    }
}

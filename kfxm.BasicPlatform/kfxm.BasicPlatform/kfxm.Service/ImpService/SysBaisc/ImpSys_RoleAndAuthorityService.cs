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
    [Export(typeof(ISys_RoleAndAuthorityService))]
    public class ImplSys_RoleAndAuthorityService : ISys_RoleAndAuthorityService
    {
        [Import(typeof(ISys_RoleAndAuthorityRepository))]
        public ISys_RoleAndAuthorityRepository ISys_RoleAndAuthorityRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Sys_RoleAndAuthority"></param>
        /// <returns></returns>
        public int Add(Sys_RoleAndAuthority entity)
        {
            return ISys_RoleAndAuthorityRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<Sys_RoleAndAuthority> entitys)
        {
            return ISys_RoleAndAuthorityRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return ISys_RoleAndAuthorityRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Sys_RoleAndAuthority entity)
        {
            return ISys_RoleAndAuthorityRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<Sys_RoleAndAuthority> entitys)
        {
            return ISys_RoleAndAuthorityRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where)
        {
            return ISys_RoleAndAuthorityRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(Sys_RoleAndAuthority entity)
        {
            return ISys_RoleAndAuthorityRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<Sys_RoleAndAuthority> entitys)
        {
            return ISys_RoleAndAuthorityRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public Sys_RoleAndAuthority GetByKey(object key)
        {
            return ISys_RoleAndAuthorityRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_RoleAndAuthority> GetAllData()
        {
            return ISys_RoleAndAuthorityRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_RoleAndAuthority> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where)
        {
            return ISys_RoleAndAuthorityRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_RoleAndAuthority> GetWhereData<T>(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where, Common.OrderByHelper<Sys_RoleAndAuthority, T> orderBy)
        {
            return ISys_RoleAndAuthorityRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_RoleAndAuthority> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where, params Common.OrderByHelper<Sys_RoleAndAuthority>[] orderBy)
        {
            return ISys_RoleAndAuthorityRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<Sys_RoleAndAuthority> GetPageData<T>(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<Sys_RoleAndAuthority, T> orderBy)
        {
            return ISys_RoleAndAuthorityRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<Sys_RoleAndAuthority> GetPageData(System.Linq.Expressions.Expression<Func<Sys_RoleAndAuthority, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<Sys_RoleAndAuthority>[] orderBy)
        {
            return ISys_RoleAndAuthorityRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

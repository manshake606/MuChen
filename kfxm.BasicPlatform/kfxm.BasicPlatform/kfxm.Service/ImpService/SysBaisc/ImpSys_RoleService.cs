using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
using kfxms.IService.SysBasic;
using kfxms.IRepository.SysBasic;
using System.ComponentModel.Composition;
namespace kfxms.ImplService.SysBasic
{
    [Export(typeof(ISys_RoleService))]
    public class ImplSys_RoleService : ISys_RoleService
    {
        [Import(typeof(ISys_RoleRepository))]
        public ISys_RoleRepository ISys_RoleRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sys_Role"></param>
        /// <returns></returns>
        public int Add(Sys_Role entity)
        {
            return ISys_RoleRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<Sys_Role> entitys)
        {
           return ISys_RoleRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return ISys_RoleRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Sys_Role entity)
        {
            return ISys_RoleRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<Sys_Role> entitys)
        {
            return ISys_RoleRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where)
        {
            return ISys_RoleRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(Sys_Role entity)
        {
            return ISys_RoleRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<Sys_Role> entitys)
        {
            return ISys_RoleRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public Sys_Role GetByKey(object key)
        {
            return ISys_RoleRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Role> GetAllData()
        {
            return ISys_RoleRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Role> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where)
        {
            return ISys_RoleRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Role> GetWhereData<T>(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where, Common.OrderByHelper<Sys_Role,T> orderBy)
        {
            return ISys_RoleRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Role> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where, params Common.OrderByHelper<Sys_Role>[] orderBy)
        {
            return ISys_RoleRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<Sys_Role> GetPageDate<T>(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<Sys_Role,T> orderBy)
        {
            return ISys_RoleRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<Sys_Role> GetPageDate(System.Linq.Expressions.Expression<Func<Sys_Role, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<Sys_Role>[] orderBy)
        {
            return ISys_RoleRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
        }

         /// <summary>
        /// 根据用户id，获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_Role> GetRoleByUserId(Guid userId)
        {
            return ISys_RoleRepository_.GetRoleByUserId(userId);
        
        }
    }
}

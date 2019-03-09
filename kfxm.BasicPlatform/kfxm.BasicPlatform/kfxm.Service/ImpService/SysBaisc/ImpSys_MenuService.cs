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
    [Export(typeof(ISys_MenuService))]
    public class ImplSys_MenuService : ISys_MenuService
    {
        [Import(typeof(ISys_MenuRepository))]
        public ISys_MenuRepository ISys_MenuRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Sys_Menu"></param>
        /// <returns></returns>
        public int Add(Sys_Menu entity)
        {
            return ISys_MenuRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<Sys_Menu> entitys)
        {
           return ISys_MenuRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return ISys_MenuRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Sys_Menu entity)
        {
            return ISys_MenuRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<Sys_Menu> entitys)
        {
            return ISys_MenuRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where)
        {
            return ISys_MenuRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(Sys_Menu entity)
        {
            return ISys_MenuRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<Sys_Menu> entitys)
        {
            return ISys_MenuRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public Sys_Menu GetByKey(object key)
        {
            return ISys_MenuRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Menu> GetAllData()
        {
            return ISys_MenuRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Menu> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where)
        {
            return ISys_MenuRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Menu> GetWhereData<T>(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where, Common.OrderByHelper<Sys_Menu,T> orderBy)
        {
            return ISys_MenuRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Menu> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where, params Common.OrderByHelper<Sys_Menu>[] orderBy)
        {
            return ISys_MenuRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<Sys_Menu> GetPageDate<T>(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where, int pageIndex, int pageSize, out int total,Common.OrderByHelper<Sys_Menu,T> orderBy)
        {
            return ISys_MenuRepository_.GetPageDate(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<Sys_Menu> GetPageDate(System.Linq.Expressions.Expression<Func<Sys_Menu, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<Sys_Menu>[] orderBy)
        {
            return ISys_MenuRepository_.GetPageDate(where, pageIndex, pageSize, out total,orderBy);
        }


         /// <summary>
        /// 根据用户id 获取有权限的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_Menu> GetMyMenu(Guid userId) 
        {
           return ISys_MenuRepository_.GetMyMenu(userId);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using kfxms.Entity.SysBasic;
using kfxms.IService.SysBasic;
using kfxms.IRepository.SysBasic;
using kfxms.IService.SysBasic;

namespace kfxms.ImpService.SysBasic
{
    [Export(typeof(ISys_DepartmentService))]
    public class ImpSys_DepartmentService : ISys_DepartmentService
    {
        [Import(typeof(ISys_DepartmentRepository))]
        public ISys_DepartmentRepository ISys_DepartmentRepository_ { get; set; }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="Sys_Department"></param>
        /// <returns></returns>
        public int Add(Sys_Department entity)
        {
            return ISys_DepartmentRepository_.Add(entity);
        }


        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Add(IEnumerable<Sys_Department> entitys)
        {
            return ISys_DepartmentRepository_.Add(entitys);
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(object id)
        {
            return ISys_DepartmentRepository_.Delete(id);
        }

        /// <summary>
        ///  删除实体记录
        /// </summary>
        /// <param name="entity">实体对象 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(Sys_Department entity)
        {
            return ISys_DepartmentRepository_.Delete(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(IEnumerable<Sys_Department> entitys)
        {
            return ISys_DepartmentRepository_.Delete(entitys);
        }

        /// <summary>
        ///  删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="where">查询条件谓语表达式 </param>
        /// <returns>操作影响的行数 </returns>
        public int Delete(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where)
        {
            return ISys_DepartmentRepository_.Delete(where);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作影响的行数</returns>
        public int Update(Sys_Department entity)
        {
            return ISys_DepartmentRepository_.Update(entity);
        }

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>操作影响的行数 </returns>
        public int Update(IEnumerable<Sys_Department> entitys)
        {
            return ISys_DepartmentRepository_.Update(entitys);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public Sys_Department GetByKey(object key)
        {
            return ISys_DepartmentRepository_.GetByKey(key);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Department> GetAllData()
        {
            return ISys_DepartmentRepository_.GetAllData();
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Department> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where)
        {
            return ISys_DepartmentRepository_.GetWhereData(where);
        }


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Department> GetWhereData<T>(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where, Common.OrderByHelper<Sys_Department, T> orderBy)
        {
            return ISys_DepartmentRepository_.GetWhereData(where, orderBy);
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns>实体对象集合</returns>
        public IEnumerable<Sys_Department> GetWhereData(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where, params Common.OrderByHelper<Sys_Department>[] orderBy)
        {
            return ISys_DepartmentRepository_.GetWhereData(where, orderBy);
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
        public IEnumerable<Sys_Department> GetPageData<T>(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where, int pageIndex, int pageSize, out int total, Common.OrderByHelper<Sys_Department, T> orderBy)
        {
            return ISys_DepartmentRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
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
        public IEnumerable<Sys_Department> GetPageData(System.Linq.Expressions.Expression<Func<Sys_Department, bool>> where, int pageIndex, int pageSize, out int total, params Common.OrderByHelper<Sys_Department>[] orderBy)
        {
            return ISys_DepartmentRepository_.GetPageData(where, pageIndex, pageSize, out total, orderBy);
        }
    }
}

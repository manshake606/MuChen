using kfxms.EFData;
using kfxms.Entity.SysBasic;
using kfxms.IRepository.SysBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfxms.ImpRepository.SysBasic
{
    [Export(typeof(ISys_RoleRepository))]
    public class ImpSys_RoleRepository : ImpBaseRepository<Sys_Role>, ISys_RoleRepository
    {

        /// <summary>
        /// 根据用户id，获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_Role> GetRoleByUserId(Guid userId) 
        {
            EFDataContext context = (EFDataContext)base.Context;

            IList<Sys_Role> roleList = (from Sys_Role role in context.Roles
                                        where
                                       ((from Sys_UserAndRole uar in context.UserAndRoles
                                        where uar.UserId == userId select uar.RoleId).ToList().Contains(role.Id))
                    select role).ToList();
            return roleList;
        }
    }
}

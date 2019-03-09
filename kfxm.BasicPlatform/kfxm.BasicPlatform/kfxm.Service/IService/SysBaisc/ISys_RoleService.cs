using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
namespace kfxms.IService.SysBasic
{
    public interface ISys_RoleService : IBaseService<Sys_Role>
    {
        /// <summary>
        /// 根据用户id，获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<Sys_Role> GetRoleByUserId(Guid userId);
    }
}

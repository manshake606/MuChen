using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
namespace kfxms.IService.SysBasic
{
    public interface ISys_MenuService : IBaseService<Sys_Menu>
    {
        /// <summary>
        /// 根据用户id 获取有权限的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<Sys_Menu> GetMyMenu(Guid userId);
    }
}

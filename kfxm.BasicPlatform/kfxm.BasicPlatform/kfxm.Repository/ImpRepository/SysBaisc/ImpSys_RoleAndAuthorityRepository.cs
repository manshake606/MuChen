using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
using kfxms.IRepository.SysBasic;

namespace kfxms.ImpRepository.SysBasic
{
    [Export(typeof(ISys_RoleAndAuthorityRepository))]
    public class ImpSys_RoleAndAuthorityRepository : ImpBaseRepository<Sys_RoleAndAuthority>, ISys_RoleAndAuthorityRepository
    {

    }
}

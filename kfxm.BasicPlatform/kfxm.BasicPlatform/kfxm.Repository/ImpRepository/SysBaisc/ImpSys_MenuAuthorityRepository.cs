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
    [Export(typeof(ISys_MenuAuthorityRepository))]
    public class ImpSys_MenuAuthorityRepository : ImpBaseRepository<Sys_MenuAuthority>, ISys_MenuAuthorityRepository
    {

    }
}

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
    [Export(typeof(ISys_DepartmentRepository))]
    public class ImpSys_DepartmentRepository : ImpBaseRepository<Sys_Department>, ISys_DepartmentRepository
    {

    }
}

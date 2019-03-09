using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Supplier;
using kfxms.IRepository.Supplier;

namespace kfxms.ImpRepository.Package
{
    [Export(typeof(IS_TaskRepository))]
    public class ImpS_TaskRepository : ImpBaseRepository<S_Task>, IS_TaskRepository
    {

    }
}

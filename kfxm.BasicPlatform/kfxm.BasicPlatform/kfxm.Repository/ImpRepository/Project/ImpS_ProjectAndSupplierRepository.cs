using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Project;
using kfxms.IRepository.Project;

namespace kfxms.ImpRepository.Project
{
    [Export(typeof(IS_ProjectAndSupplierRepository))]
    public class ImpS_ProjectAndSupplierRepository : ImpBaseRepository<S_ProjectAndSupplier>, IS_ProjectAndSupplierRepository
    {

    }
}

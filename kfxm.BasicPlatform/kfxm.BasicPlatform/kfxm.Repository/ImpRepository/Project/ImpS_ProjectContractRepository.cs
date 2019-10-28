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
    [Export(typeof(IS_ProjectContractRepository))]
    public class ImpS_ProjectContractRepository : ImpBaseRepository<S_ProjectContract>, IS_ProjectContractRepository
    {

    }
}

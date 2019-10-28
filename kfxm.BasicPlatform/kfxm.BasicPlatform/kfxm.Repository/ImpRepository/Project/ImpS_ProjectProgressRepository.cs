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
    [Export(typeof(IS_ProjectProgressRepository))]
    public class ImpS_ProjectProgressRepository : ImpBaseRepository<S_ProjectProgress>, IS_ProjectProgressRepository
    {

    }
}

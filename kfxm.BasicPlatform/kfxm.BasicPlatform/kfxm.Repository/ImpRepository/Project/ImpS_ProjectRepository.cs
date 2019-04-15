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
    [Export(typeof(IS_ProjectRepository))]
    public class ImpS_ProjectRepository : ImpBaseRepository<S_Project>, IS_ProjectRepository
    {

    }
}

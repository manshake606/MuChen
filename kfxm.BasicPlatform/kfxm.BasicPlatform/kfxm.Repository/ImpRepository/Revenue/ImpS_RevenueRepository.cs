using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Revenue;
using kfxms.IRepository.Revenue;

namespace kfxms.ImpRepository.Revenue
{
    [Export(typeof(IS_RevenueRepository))]
    public class ImpS_RevenueRepository : ImpBaseRepository<S_Revenue>, IS_RevenueRepository
    {

    }
}

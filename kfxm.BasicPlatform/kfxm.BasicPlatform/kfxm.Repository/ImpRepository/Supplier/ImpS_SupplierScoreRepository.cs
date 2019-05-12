using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Supplier;
using kfxms.IRepository.Supplier;

namespace kfxms.ImpRepository.Supplier
{
    [Export(typeof(IS_SupplierScoreRepository))]
    public class ImpS_SupplierScoreRepository : ImpBaseRepository<S_SupplierScore>, IS_SupplierScoreRepository
    {

    }
}

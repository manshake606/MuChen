using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Supplier;
using kfxms.IRepository.Supplier;

namespace kfxms.ImpRepository.SupplierScoreValue
{
    [Export(typeof(IS_SupplierScoreValueRepository))]
    public class ImpS_SupplierScoreValueRepository : ImpBaseRepository<S_SupplierScoreValue>, IS_SupplierScoreValueRepository
    {

    }
}

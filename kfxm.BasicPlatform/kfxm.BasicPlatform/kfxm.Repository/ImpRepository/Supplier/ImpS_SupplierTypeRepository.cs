using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Supplier;
using kfxms.IRepository.Supplier;

namespace kfxms.ImpRepository.SupplierType
{
    [Export(typeof(IS_SupplierTypeRepository))]
    public class ImpS_SupplierTypeRepository : ImpBaseRepository<S_SupplierType>, IS_SupplierTypeRepository
    {

    }
}

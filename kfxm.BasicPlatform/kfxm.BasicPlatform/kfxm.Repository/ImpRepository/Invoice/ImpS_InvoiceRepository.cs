using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Invoice;
using kfxms.IRepository.Invoice;

namespace kfxms.ImpRepository.Invoice
{
    [Export(typeof(IS_InvoiceRepository))]
    public class ImpS_InvoiceRepository : ImpBaseRepository<S_Invoice>, IS_InvoiceRepository
    {

    }
}

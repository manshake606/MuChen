using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Payment;
using kfxms.IRepository.Payment;

namespace kfxms.ImpRepository.Payment
{
    [Export(typeof(IS_ExternalPaymentRepository))]
    public class ImpS_ExternalPaymentRepository : ImpBaseRepository<S_ExternalPayment>, IS_ExternalPaymentRepository
    {

    }
}

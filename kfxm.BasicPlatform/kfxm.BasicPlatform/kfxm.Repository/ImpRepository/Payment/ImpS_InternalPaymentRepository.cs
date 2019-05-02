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
    [Export(typeof(IS_InternalPaymentRepository))]
    public class ImpS_InternalPaymentRepository : ImpBaseRepository<S_InternalPayment>, IS_InternalPaymentRepository
    {

    }
}

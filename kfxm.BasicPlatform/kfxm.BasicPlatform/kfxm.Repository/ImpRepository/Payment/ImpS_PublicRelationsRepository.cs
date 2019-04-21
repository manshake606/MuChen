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
    [Export(typeof(IS_PublicRelationsRepository))]
    public class ImpS_PublicRelationsRepository : ImpBaseRepository<S_PublicRelations>, IS_PublicRelationsRepository
    {

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.Client;
using kfxms.IRepository.Client;

namespace kfxms.ImpRepository.Client
{
    [Export(typeof(IS_ClientRepository))]
    public class ImpS_ClientRepository : ImpBaseRepository<S_Client>, IS_ClientRepository
    {

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
using kfxms.IRepository.SysBasic;

namespace kfxms.ImpRepository.SysBasic
{
    [Export(typeof(ISys_DictionaryRepository))]
    public class ImpSys_DictionaryRepository : ImpBaseRepository<Sys_Dictionary>, ISys_DictionaryRepository
    {

    }
}

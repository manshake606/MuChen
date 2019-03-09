﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;

namespace kfxms.IRepository.SysBasic
{
    public interface ISys_AuthorityRequestRepository : IBaseRepository<Sys_AuthorityRequest>
    {

        /// <summary>
        /// 获取用户有权限的请求
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<Sys_AuthorityRequest> GetRequestByUserId(Guid userId);
    }
}

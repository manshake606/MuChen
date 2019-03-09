using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kfxms.Entity.SysBasic;
using kfxms.IRepository.SysBasic;
using kfxms.EFData;

namespace kfxms.ImpRepository.SysBasic
{
    [Export(typeof(ISys_AuthorityRequestRepository))]
    public class ImpSys_AuthorityRequestRepository : ImpBaseRepository<Sys_AuthorityRequest>, ISys_AuthorityRequestRepository
    {
        /// <summary>
        /// 获取用户有权限的请求
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Sys_AuthorityRequest> GetRequestByUserId(Guid userId) 
        {
            EFDataContext context = (EFDataContext)base.Context;
            //根据用户获取角色id
            var userAndRole = from Sys_UserAndRole uar in context.UserAndRoles where uar.UserId == userId select uar.RoleId;
            //根据角色获取权限id
            var roleAndAuthority = from Sys_RoleAndAuthority raa in context.RoleAndAuthoritys where userAndRole.Contains(raa.RoleId) select raa.AuthorityId;
            //根据权限获取请求
            var authRequest = from Sys_AuthorityRequest ar in context.AuthorityRequests where roleAndAuthority.Contains(ar.AuthorityId) select ar;
            
            return authRequest.ToList();
        }
    }
}

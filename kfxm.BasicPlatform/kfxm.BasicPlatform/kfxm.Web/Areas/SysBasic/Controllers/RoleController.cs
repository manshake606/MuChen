using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using kfxms.IService.SysBasic;
using kfxms.Entity.SysBasic;
using System.Linq.Expressions;
using kfxms.Common;
using System.Collections;


namespace kfxms.Web.Areas.SysBasic.Controllers
{
    [Export]
    public class RoleController : BaseController
    {
        [Import]
        public ISys_RoleService roleService { get; set; }

        [Import]
        public ISys_RoleAndAuthorityService roleAndAuthorityService { get; set; }

        public ActionResult List()
        {
            return View("RoleList");
        }

        public ActionResult Add()
        {
            return View("RoleAdd");
        }

        public ActionResult Edit(Guid roleId)
        {
            Sys_Role role = roleService.GetByKey(roleId);
            if (role == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }

            return View("RoleEdit", role);
        }

        public ActionResult BindAuthority(Guid roleId)
        {
            ViewData["id"] = roleId;
            return View();
        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<Sys_Role, bool>> expre = u => true;


            if (Request.Form["roleName"] != null && !string.IsNullOrEmpty(Request.Form["roleName"]))
            {
                string roleName = Request.Form["roleName"].Trim();
                expre = expre.And(u => u.RoleName.Contains(roleName));
            }

            int total = 0;

            //排序
            OrderByHelper<Sys_Role, DateTime> orderBy = new OrderByHelper<Sys_Role, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            List<Sys_Role> list = roleService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();

            Hashtable ht = new Hashtable();

            ht.Add("total", total);
            ht.Add("data", list);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);
        }

        public ActionResult AddRole(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            Sys_Role eRole = new Sys_Role();
            eRole.Id = Guid.NewGuid();
            eRole.RoleName = row["RoleName"].ToString().Trim();
            eRole.Remark = row["Remark"].ToString().Trim();
            eRole.AddTime = DateTime.Now;
            eRole.AddUserId = base.LoginUser.Id;
            eRole.AddName = base.LoginUser.Name;
            int num = roleService.Add(eRole);
            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "新增成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "新增失败！");
            }
            return Content(resultJson);
        }

        public ActionResult EditRole(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            Sys_Role eRole = roleService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eRole == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            eRole.RoleName = row["RoleName"].ToString().Trim();
            eRole.Remark = row["Remark"].ToString().Trim();

            int num = roleService.Update(eRole);
            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "修改成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "修改失败！");
            }
            return Content(resultJson);

        }

        public ActionResult DeleteRole(Guid roleId)
        {
            string resultJson = "";
            int num = roleService.Delete(roleId);

            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框不关闭窗体, "删除成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "删除失败！");
            }

            return Content(resultJson);
        }

        /// <summary>
        /// 根据角色已有的权限；
        /// </summary>
        public ActionResult GetRoleAuthority(Guid roleId)
        {
            string resultJson = "";
            IList<Sys_RoleAndAuthority> raaList = roleAndAuthorityService.GetWhereData(raa => raa.RoleId == roleId).ToList();
            resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源,raaList);
            return Content(resultJson);
        }

        /// <summary>
        /// 角色绑定权限
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult RoleBindAuthority(Guid roleId, string data) 
        {
            Hashtable rows = (Hashtable)JsonHelp.Decode(data);
            

            IList<Sys_RoleAndAuthority> addRaa = new List<Sys_RoleAndAuthority>();
            IList<Sys_RoleAndAuthority> removeRaa = new List<Sys_RoleAndAuthority>();
            foreach (object key in rows.Keys) 
            {
                Hashtable row = (Hashtable)rows[key];
                var type = row["type"].ToString().Trim();
                Guid? AuthorityId = ((row.ContainsKey("AuthorityId") && row["AuthorityId"] != null)
                    ? Guid.Parse(row["AuthorityId"].ToString()) : Guid.Empty);
                Sys_RoleAndAuthority entity = new Sys_RoleAndAuthority();
                if (type.Equals("add"))
                {
                    entity.Id = Guid.NewGuid();
                    entity.RoleId = roleId;
                    entity.AuthorityId = AuthorityId;
                    addRaa.Add(entity);
                }
                else if (type.Equals("remove"))
                {
                    entity.Id = Guid.Parse(row["Id"].ToString());
                    removeRaa.Add(entity);
                }
                else
                {
                }
            }
            roleAndAuthorityService.Add(addRaa);
            roleAndAuthorityService.Delete(removeRaa);
            string resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "保存成功");
            return Content(resultJson);

        }
         
    }
}

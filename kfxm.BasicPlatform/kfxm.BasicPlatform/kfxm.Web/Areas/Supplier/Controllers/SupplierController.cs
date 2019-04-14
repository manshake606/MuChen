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
using kfxms.Entity.Supplier;
using kfxms.IService.Supplier;
using kfxms.Entity.SupplierType;
using kfxms.IService.SupplierType;

namespace kfxms.Web.Areas.Supplier.Controllers
{
    [Export]
    public class SupplierController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierService supplierService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierTypeService supplierTypeService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("SupplierList");
        }

        public ActionResult Add()
        {
            return View("SupplierAdd");
        }

        public ActionResult Edit(Guid supplierId)
        {
            S_Supplier sys_Supplier = supplierService.GetByKey(supplierId);
            

            if (sys_Supplier == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("SupplierEdit", sys_Supplier);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_Supplier, bool>> expre = u => true;

            if (Request.Form["supplierName"] != null && !string.IsNullOrEmpty(Request.Form["supplierName"]))
            {
                string supplierName = Request.Form["supplierName"].Trim();
                expre = expre.And(u => u.SupplierName.Contains(supplierName));
            }
            if (Request.Form["corporationName"] != null && !string.IsNullOrEmpty(Request.Form["corporationName"]))
            {
                string corporationName = Request.Form["corporationName"].Trim();
                expre = expre.And(u => u.CorporationName.Contains(corporationName));
            }


            ////排序
            OrderByHelper<S_Supplier, DateTime> orderBy = new OrderByHelper<S_Supplier, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Supplier> list = supplierService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_SupplierType> typeList = supplierTypeService.GetAllData().ToList();
            List<S_SupplierHasTypeName> listHasTypeName = new List<S_SupplierHasTypeName>();
            
            foreach(S_Supplier item in list)
            {
                var s_SupplierHasTypeName = new S_SupplierHasTypeName();
                s_SupplierHasTypeName.AddName = item.AddName;
                s_SupplierHasTypeName.Address = item.Address;
                s_SupplierHasTypeName.AddTime = item.AddTime;
                s_SupplierHasTypeName.AddUserId = item.AddUserId;
                s_SupplierHasTypeName.ContactMobile = item.ContactMobile;
                s_SupplierHasTypeName.ContactName = item.ContactName;
                s_SupplierHasTypeName.CorporationMobile = item.CorporationMobile;
                s_SupplierHasTypeName.CorporationName = item.CorporationName;
                s_SupplierHasTypeName.Id = item.Id;
                s_SupplierHasTypeName.LastEditName = item.LastEditName;
                s_SupplierHasTypeName.LastEditTime = item.LastEditTime;
                s_SupplierHasTypeName.LastEditUserID = item.LastEditUserID;
                s_SupplierHasTypeName.Num = item.Num;
                s_SupplierHasTypeName.Remark = item.Remark;
                s_SupplierHasTypeName.SupplierName = item.SupplierName;
                s_SupplierHasTypeName.Type = item.Type;
                s_SupplierHasTypeName.TypeName = "";
                foreach (S_SupplierType typeItem in typeList)
                {
                    if(s_SupplierHasTypeName.Type== typeItem.SupplierTypeId)
                    {
                        s_SupplierHasTypeName.TypeName = typeItem.SupplierTypeName;
                    }
                }
                listHasTypeName.Add(s_SupplierHasTypeName);
            }
            


            /*
            var s_Task = new S_Task()
            {
                 ID=Guid.NewGuid(), 
                 Code="包[2016]00001",
                 Name = "包装一次项目",
                 Number = "一产业",
                 AgreedMatters = "关于包装某二次项目*******改造工程",
                 AddUserName="李xx",
                 AddTime = DateTime.Now
            };
            var s_Task1 = new S_Task()
            {
                ID = Guid.NewGuid(),
                Code = "包[2016]00002",
                Name = "包装某二次项目",
                Number = "二产业",
                AgreedMatters = "关于包装某二次项目石化天然气管道改造工程",
                AddUserName = "李xx",
                AddTime = DateTime.Now
            };
            var s_Task2 = new S_Task()
            {
                ID = Guid.NewGuid(),
                Code = "包[2016]00003",
                Name = "包装某三次项目",
                Number = "三产业",
                AgreedMatters = "关于包装某三次项目夏季暴雨后果树管理 ",
                AddUserName = "李xx",
                AddTime = DateTime.Now
            };
            List<S_Task> list = new List<S_Task>();
            list.Add(s_Task);
            list.Add(s_Task1);
            list.Add(s_Task2);
            Hashtable ht = new Hashtable();

            ht.Add("total", 3);
            ht.Add("data", list);
            */

            Hashtable ht = new Hashtable();
            ht.Add("total", total);
            ht.Add("data", listHasTypeName);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult AddSupplier(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Supplier eSupplier = new S_Supplier();
            eSupplier.Id = Guid.NewGuid();
            eSupplier.SupplierName = row["SupplierName"].ToString().Trim();
            //eSupplier.Password = MD5Helper.GetMD5("123456");
            eSupplier.CorporationName = row["CorporationName"].ToString().Trim();
            eSupplier.CorporationMobile = row["CorporationMobile"].ToString().Trim();
            eSupplier.ContactName = row["ContactName"].ToString().Trim();
            eSupplier.ContactMobile = row["ContactMobile"].ToString().Trim();
            eSupplier.Address = row["Address"].ToString().Trim();
            eSupplier.Remark = row["Remark"].ToString().Trim();
            eSupplier.Type = int.Parse(row["Type"].ToString());

            List<S_Supplier> listSupplier = supplierService.GetWhereData(u => u.SupplierName.Equals(eSupplier.SupplierName)).ToList();
            if (listSupplier.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该供应商已经存在！");
               return Content(resultJson);
            }
            int num = supplierService.Add(eSupplier);
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

        public ActionResult EditSupplier(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Supplier eSupplier = supplierService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eSupplier == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            String supplierName = row["SupplierName"].ToString().Trim();
            //eSupplier.SupplierName = row["SupplierName"].ToString().Trim();
            if (!eSupplier.SupplierName.Equals(supplierName))
            {
                List<S_Supplier> listSupplier = supplierService.GetWhereData(u => u.SupplierName.Equals(supplierName)).ToList();
                if (listSupplier.Count > 0)
                {
                    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该公司名称已经存在！");
                    return Content(resultJson);
                }
            }
            eSupplier.SupplierName = supplierName;
            eSupplier.CorporationName= row["CorporationName"].ToString().Trim();
            eSupplier.CorporationMobile= row["CorporationMobile"].ToString().Trim();
            eSupplier.ContactName = row["ContactName"].ToString().Trim();
            eSupplier.ContactMobile = row["ContactMobile"].ToString().Trim();
            eSupplier.Address = row["Address"].ToString().Trim();
            eSupplier.Remark = row["Remark"].ToString().Trim();
            eSupplier.Type = int.Parse(row["Type"].ToString());

            int num = supplierService.Update(eSupplier);
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

        public ActionResult DeleteSupplier(Guid supplierId)
        {
            int num = supplierService.Delete(supplierId);
            string resultJson = "";
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

        /*
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult ResetPwd(Guid userId)
        {
            string resultJson = "";
            Sys_User eUser = userService.GetByKey(userId);
            if (eUser == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            eUser.Password = MD5Helper.GetMD5("123456");
            int num = userService.Update(eUser);
            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "重置成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "重置失败！");
            }
            return Content(resultJson);
        }

        /// <summary>
        /// 获取用户已经绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult BindRole(Guid userId)
        {

            List<Sys_Role> roleList = roleService.GetAllData().ToList();
            ViewData["roleList"] = roleList;
            List<Sys_UserAndRole> uarList = userAndRoleService.GetWhereData(uar => uar.UserId == userId).ToList();
            ViewData["meRoleList"] = uarList;
            ViewData["userId"] = userId;
            return View();
        }

        /// <summary>
        /// 用户绑定角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult BindRoleHandler(Guid userId, string roleIds)
        {
            userAndRoleService.Delete(uar => uar.UserId == userId);
            List<Sys_UserAndRole> uarList = new List<Sys_UserAndRole>();
            string[] roleIdArray = roleIds.Split(',');
            foreach (string roleId in roleIdArray)
            {
                if (!string.IsNullOrEmpty(roleId.Trim()))
                {
                    Sys_UserAndRole uar = new Sys_UserAndRole();
                    uar.Id = Guid.NewGuid();
                    uar.UserId = userId;
                    uar.RoleId = Guid.Parse(roleId);
                    uarList.Add(uar);
                }
            }

            int num = userAndRoleService.Add(uarList);
            string resultJson = "";
            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "绑定成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "绑定失败！");
            }
            return Content(resultJson);
        }
        */
    }
}

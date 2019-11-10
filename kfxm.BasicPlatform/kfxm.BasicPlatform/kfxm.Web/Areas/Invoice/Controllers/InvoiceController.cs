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
using kfxms.Entity.Invoice;
using kfxms.IService.Invoice;
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.Entity.Client;
using kfxms.IService.Client;

namespace kfxms.Web.Areas.Invoice.Controllers
{
    [Export]
    public class InvoiceController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }


        [Import]
        //public new  userService { get; set; }
        public IS_InvoiceService InvoiceService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectContractService projectContractService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List(Guid projectContractId)
        {
            S_ProjectContract s_ProjectContract = projectContractService.GetByKey(projectContractId);
            if (s_ProjectContract == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("InvoiceList", s_ProjectContract);
        }

        public ActionResult Add(Guid projectContractId)
        {
            S_ProjectContract s_ProjectContract = projectContractService.GetByKey(projectContractId);
            if (s_ProjectContract == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("InvoiceAdd", s_ProjectContract);
        }

        public ActionResult Edit(Guid InvoiceId)
        {
            S_Invoice sys_Invoice = InvoiceService.GetByKey(InvoiceId);
            

            if (sys_Invoice == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("InvoiceEdit", sys_Invoice);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_Invoice, bool>> expre = u => true;
            //var userPojectList;
            /*
            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                string remark = Request.Form["projectName"].Trim();
                expre = expre.And(u => u.Remark.Contains(remark));
            }
            */

            
            


            //var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m=>m.Num);
            //var userPojectList = projectService.GetAllData().Where(u=>u.Num==5).Select(m => m.Num);

            //int[] userPojectList = new int[] { 3, 5 };

            ////排序
            OrderByHelper<S_Invoice, DateTime> orderBy = new OrderByHelper<S_Invoice, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Invoice> list = InvoiceService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();




            List<S_ProjectContract> listProjectContract = projectContractService.GetAllData().ToList();
            List<S_InvoiceDetail> listDetail = new List<S_InvoiceDetail>();

            foreach (S_Invoice item in list)
            {
                var s_InvoiceDetail = new S_InvoiceDetail();
                s_InvoiceDetail.Id = item.Id;
                s_InvoiceDetail.Num = item.Num;
                s_InvoiceDetail.ContractNum = item.ContractNum;
                s_InvoiceDetail.TrackingNum = item.TrackingNum;
                s_InvoiceDetail.Remark = item.Remark;
                s_InvoiceDetail.InvoiceAmout = item.InvoiceAmout;
                s_InvoiceDetail.InvoiceTime = item.InvoiceTime;

                foreach (S_ProjectContract projectItem in listProjectContract)
                {
                    if (s_InvoiceDetail.ContractNum == projectItem.Num)
                    {
                        s_InvoiceDetail.ContractName = projectItem.ProjectContractName;
                        s_InvoiceDetail.ContractAmout = projectItem.ProjectContractAmount;
                    }
                }
                listDetail.Add(s_InvoiceDetail);
            }

            //Expression<Func<S_InvoiceDetail, bool>> exp = u => true;
            if (Request.Form["contractName"] != null && !string.IsNullOrEmpty(Request.Form["contractName"]))
            {
                listDetail = listDetail.Where(u => u.ContractName.Contains(Request.Form["contractName"])).ToList();
            }

            Sys_User sUser = base.LoginUser;
            List<Sys_UserAndRole> currentUserRole = userAndRoleService.GetWhereData(u => u.UserId == sUser.Id).ToList();
            Guid? currentRoleId = currentUserRole[0].RoleId;
            List<Sys_Role> currentRoleList = roleService.GetWhereData(m => m.Id == currentRoleId).ToList();
            string roleName = currentRoleList[0].RoleName;
            Hashtable ht = new Hashtable();
            string json = null;
            //if (roleName != "系统管理员" && roleName != "财务")
            //{
            //    var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m => m.Num);
            //    var result = from p in listDetail where userPojectList.Contains(p.ContractNum) select p;

            //    ht.Add("total", total);
            //    ht.Add("data", result);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            //    return Content(json);
            //}
            //else
            //{
            //    ht.Add("total", total);
            //    ht.Add("data", listDetail);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            //}

            ht.Add("total", total);
            ht.Add("data", listDetail);
            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);

        }

        public ActionResult GetPageDataBySelectedContractNum(int pageSize, int pageIndex, int ContractNum)
        {

            //条件
            Expression<Func<S_Invoice, bool>> expre = u => true;
            //var userPojectList;
            /*
            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                string remark = Request.Form["projectName"].Trim();
                expre = expre.And(u => u.Remark.Contains(remark));
            }
            */





            //var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m=>m.Num);
            //var userPojectList = projectService.GetAllData().Where(u=>u.Num==5).Select(m => m.Num);

            //int[] userPojectList = new int[] { 3, 5 };

            ////排序
            OrderByHelper<S_Invoice, DateTime> orderBy = new OrderByHelper<S_Invoice, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Invoice> list = InvoiceService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).Where(u=>u.ContractNum== ContractNum).ToList();




            List<S_ProjectContract> listProjectContract = projectContractService.GetAllData().ToList();
            List<S_InvoiceDetail> listDetail = new List<S_InvoiceDetail>();

            foreach (S_Invoice item in list)
            {
                var s_InvoiceDetail = new S_InvoiceDetail();
                s_InvoiceDetail.Id = item.Id;
                s_InvoiceDetail.Num = item.Num;
                s_InvoiceDetail.ContractNum = item.ContractNum;
                s_InvoiceDetail.TrackingNum = item.TrackingNum;
                s_InvoiceDetail.Remark = item.Remark;
                s_InvoiceDetail.InvoiceAmout = item.InvoiceAmout;
                s_InvoiceDetail.InvoiceTime = item.InvoiceTime;

                foreach (S_ProjectContract projectItem in listProjectContract)
                {
                    if (s_InvoiceDetail.ContractNum == projectItem.Num)
                    {
                        s_InvoiceDetail.ContractName = projectItem.ProjectContractName;
                        s_InvoiceDetail.ContractAmout = projectItem.ProjectContractAmount;
                    }
                }
                listDetail.Add(s_InvoiceDetail);
            }

            //Expression<Func<S_InvoiceDetail, bool>> exp = u => true;
            if (Request.Form["contractName"] != null && !string.IsNullOrEmpty(Request.Form["contractName"]))
            {
                listDetail = listDetail.Where(u => u.ContractName.Contains(Request.Form["contractName"])).ToList();
            }

            Sys_User sUser = base.LoginUser;
            List<Sys_UserAndRole> currentUserRole = userAndRoleService.GetWhereData(u => u.UserId == sUser.Id).ToList();
            Guid? currentRoleId = currentUserRole[0].RoleId;
            List<Sys_Role> currentRoleList = roleService.GetWhereData(m => m.Id == currentRoleId).ToList();
            string roleName = currentRoleList[0].RoleName;
            Hashtable ht = new Hashtable();
            string json = null;
            //if (roleName != "系统管理员" && roleName != "财务")
            //{
            //    var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m => m.Num);
            //    var result = from p in listDetail where userPojectList.Contains(p.ContractNum) select p;

            //    ht.Add("total", total);
            //    ht.Add("data", result);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            //    return Content(json);
            //}
            //else
            //{
            //    ht.Add("total", total);
            //    ht.Add("data", listDetail);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            //}

            //ht.Add("total", total);
            ht.Add("data", listDetail);
            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Invoice eInvoice = new S_Invoice();
            List<S_Invoice> listInvoice = InvoiceService.GetAllData().ToList();


            Sys_User sUser = base.LoginUser;
            List<Sys_UserAndRole> currentUserRole = userAndRoleService.GetWhereData(u => u.UserId == sUser.Id).ToList();
            Guid? currentRoleId = currentUserRole[0].RoleId;
            List<Sys_Role> currentRoleList = roleService.GetWhereData(m => m.Id == currentRoleId).ToList();
            string roleName = currentRoleList[0].RoleName;


            Hashtable ht = new Hashtable();
            string json = null;
            //if (roleName != "系统管理员" && roleName != "财务")
            //{
            //    var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m => m.Num);
            //    var result = from p in listInvoice where userPojectList.Contains(p.ProjectNum) select p;

            //    //ht.Add("total", total);
            //    ht.Add("data", result);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            //    return Content(json);
            //}
            //else
            //{
            //    //ht.Add("total", total);
            //    ht.Add("data", listInvoice);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            //}
            ht.Add("data", listInvoice);
            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddInvoice(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Invoice eInvoice = new S_Invoice();
            eInvoice.Id = Guid.NewGuid();
            //eSupplier.Type = int.Parse(arrType[0]);
            eInvoice.ContractNum= int.Parse(row["ContractNum"].ToString());
            eInvoice.InvoiceAmout= Convert.ToDecimal(row["InvoiceAmout"].ToString().Trim());
            eInvoice.InvoiceTime = Convert.ToDateTime(row["InvoiceTime"].ToString().Trim());
            eInvoice.TrackingNum = row["TrackingNum"].ToString();
            eInvoice.Remark = row["Remark"].ToString().Trim();
            eInvoice.AddTime = DateTime.Now;
            eInvoice.AddUserId = base.LoginUser.Id;
            eInvoice.AddName = base.LoginUser.Name;
            eInvoice.LastEditName = base.LoginUser.Name;
            eInvoice.LastEditTime = DateTime.Now;
            eInvoice.LastEditUserID = base.LoginUser.Id;


            int num = InvoiceService.Add(eInvoice);
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

        public ActionResult EditInvoice(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Invoice eInvoice = InvoiceService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eInvoice == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String InvoiceName = row["InvoiceName"].ToString().Trim();
            //eInvoice.InvoiceName = row["InvoiceName"].ToString().Trim();

            //eInvoice.ProjectNum = int.Parse(row["Project"].ToString());

            eInvoice.InvoiceAmout = Convert.ToDecimal(row["InvoiceAmout"].ToString().Trim());
            eInvoice.InvoiceTime = Convert.ToDateTime(row["InvoiceTime"].ToString().Trim());
            eInvoice.TrackingNum= row["TrackingNum"].ToString().Trim();
            eInvoice.Remark = row["Remark"].ToString().Trim();
            eInvoice.LastEditName = base.LoginUser.Name;
            eInvoice.LastEditTime = DateTime.Now;
            eInvoice.LastEditUserID = base.LoginUser.Id;


            int num = InvoiceService.Update(eInvoice);
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

        public ActionResult DeleteInvoice(Guid InvoiceId)
        {
            int num = InvoiceService.Delete(InvoiceId);
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

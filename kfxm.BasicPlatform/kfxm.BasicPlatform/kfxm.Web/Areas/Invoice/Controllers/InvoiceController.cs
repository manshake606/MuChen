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
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("InvoiceList");
        }

        public ActionResult Add()
        {
            return View("InvoiceAdd");
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

            if (Request.Form["remark"] != null && !string.IsNullOrEmpty(Request.Form["remark"]))
            {
                string remark = Request.Form["remark"].Trim();
                expre = expre.And(u => u.Remark.Contains(remark));
            }
            


            ////排序
            OrderByHelper<S_Invoice, DateTime> orderBy = new OrderByHelper<S_Invoice, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Invoice> list = InvoiceService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_Project> listProject = projectService.GetAllData().ToList();
            List<S_InvoiceDetail> listDetail = new List<S_InvoiceDetail>();

            foreach (S_Invoice item in list)
            {
                var s_InvoiceDetail = new S_InvoiceDetail();
                s_InvoiceDetail.Id = item.Id;
                s_InvoiceDetail.Num = item.Num;
                s_InvoiceDetail.ProjectNum = item.ProjectNum;
                s_InvoiceDetail.Remark = item.Remark;
                s_InvoiceDetail.InvoiceAmout = item.InvoiceAmout;
                s_InvoiceDetail.InvoiceTime = item.InvoiceTime;

                foreach (S_Project projectItem in listProject)
                {
                    if (s_InvoiceDetail.ProjectNum == projectItem.Num)
                    {
                        s_InvoiceDetail.ProjectName = projectItem.ProjectName;
                    }
                }
                listDetail.Add(s_InvoiceDetail);
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
            ht.Add("data", listDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Invoice eInvoice = new S_Invoice();
            List<S_Invoice> listInvoice = InvoiceService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listInvoice);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddInvoice(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Invoice eInvoice = new S_Invoice();
            eInvoice.Id = Guid.NewGuid();
            //eSupplier.Type = int.Parse(arrType[0]);
            eInvoice.ProjectNum= int.Parse(row["Project"].ToString());
            eInvoice.InvoiceAmout= Convert.ToDecimal(row["InvoiceAmout"].ToString().Trim());
            eInvoice.InvoiceTime = Convert.ToDateTime(row["InvoiceTime"].ToString().Trim());
            eInvoice.Remark = row["Remark"].ToString().Trim();

            
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
            eInvoice.Remark = row["Remark"].ToString().Trim();


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

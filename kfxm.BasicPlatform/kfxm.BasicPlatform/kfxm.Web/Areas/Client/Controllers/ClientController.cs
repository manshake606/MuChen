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
using kfxms.Entity.Client;
using kfxms.IService.Client;

namespace kfxms.Web.Areas.Client.Controllers
{
    [Export]
    public class ClientController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ClientService ClientService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("ClientList");
        }

        public ActionResult Add()
        {
            return View("ClientAdd");
        }

        public ActionResult Edit(Guid clientId)
        {
            S_Client sys_Client = ClientService.GetByKey(clientId);
            

            if (sys_Client == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ClientEdit", sys_Client);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_Client, bool>> expre = u => true;

            if (Request.Form["ClientName"] != null && !string.IsNullOrEmpty(Request.Form["ClientName"]))
            {
                string ClientName = Request.Form["ClientName"].Trim();
                expre = expre.And(u => u.ClientName.Contains(ClientName));
            }
            if (Request.Form["SocialSecurityNum"] != null && !string.IsNullOrEmpty(Request.Form["SocialSecurityNum"]))
            {
                string socialSecurityNum = Request.Form["SocialSecurityNum"].Trim();
                expre = expre.And(u => u.SocialSecurityNum.Contains(socialSecurityNum));
            }
            if (Request.Form["ClientContact"] != null && !string.IsNullOrEmpty(Request.Form["ClientContact"]))
            {
                string clientContact = Request.Form["ClientContact"].Trim();
                expre = expre.And(u => u.ClientContact.Contains(clientContact));
            }


            ////排序
            OrderByHelper<S_Client, DateTime> orderBy = new OrderByHelper<S_Client, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Client> list = ClientService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();

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
            ht.Add("data", list);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Client eClient = new S_Client();
            List<S_Client> listClient = ClientService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listClient);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddClient(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Client eClient = new S_Client();
            eClient.Id = Guid.NewGuid();
            eClient.ClientName = row["ClientName"].ToString().Trim();
            //eClient.Password = MD5Helper.GetMD5("123456");
            eClient.SocialSecurityNum = row["SocialSecurityNum"].ToString().Trim();
            eClient.ClientAddress = row["ClientAddress"].ToString().Trim();
            eClient.TelephoneNum = row["TelephoneNum"].ToString().Trim();
            eClient.Bank = row["Bank"].ToString().Trim();
            eClient.BankAccount = row["BankAccount"].ToString().Trim();
            eClient.ClientContact = row["ClientContact"].ToString().Trim();
            eClient.ClientContactMobile = row["ClientContactMobile"].ToString().Trim();
            eClient.ClientContactPosition = row["ClientContactPosition"].ToString().Trim();
            eClient.Remark = row["Remark"].ToString().Trim();

            List<S_Client> listClient = ClientService.GetWhereData(u => u.ClientName.Equals(eClient.ClientName)).ToList();
            if (listClient.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该甲方已经存在！");
               return Content(resultJson);
            }
            int num = ClientService.Add(eClient);
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

        public ActionResult EditClient(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Client eClient = ClientService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eClient == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            String ClientName = row["ClientName"].ToString().Trim();
            //eClient.ClientName = row["ClientName"].ToString().Trim();
            if (!eClient.ClientName.Equals(ClientName))
            {
                List<S_Client> listClient = ClientService.GetWhereData(u => u.ClientName.Equals(ClientName)).ToList();
                if (listClient.Count > 0)
                {
                    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该公司名称已经存在！");
                    return Content(resultJson);
                }
            }
            eClient.ClientName = ClientName;
            eClient.SocialSecurityNum = row["SocialSecurityNum"].ToString().Trim();
            eClient.ClientAddress = row["ClientAddress"].ToString().Trim();
            eClient.TelephoneNum = row["TelephoneNum"].ToString().Trim();
            eClient.Bank = row["Bank"].ToString().Trim();
            eClient.BankAccount = row["BankAccount"].ToString().Trim();
            eClient.ClientContact = row["ClientContact"].ToString().Trim();
            eClient.ClientContactMobile = row["ClientContactMobile"].ToString().Trim();
            eClient.ClientContactPosition = row["ClientContactPosition"].ToString().Trim();
            eClient.Remark = row["Remark"].ToString().Trim();

            int num = ClientService.Update(eClient);
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

        public ActionResult DeleteClient(Guid ClientId)
        {
            int num = ClientService.Delete(ClientId);
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

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
using kfxms.Entity.Revenue;
using kfxms.IService.Revenue;
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.Entity.Client;
using kfxms.IService.Client;

namespace kfxms.Web.Areas.Revenue.Controllers
{
    [Export]
    public class RevenueController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_RevenueService RevenueService { get; set; }

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
            return View("RevenueList", s_ProjectContract);
        }

        public ActionResult Add(Guid projectContractId)
        {
            S_ProjectContract s_ProjectContract = projectContractService.GetByKey(projectContractId);
            if (s_ProjectContract == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("RevenueAdd", s_ProjectContract);
        }

        public ActionResult Edit(Guid RevenueId)
        {
            S_Revenue sys_Revenue = RevenueService.GetByKey(RevenueId);
            

            if (sys_Revenue == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("RevenueEdit", sys_Revenue);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_Revenue, bool>> expre = u => true;

            //if (Request.Form["remark"] != null && !string.IsNullOrEmpty(Request.Form["remark"]))
            //{
            //    string remark = Request.Form["remark"].Trim();
            //    expre = expre.And(u => u.Remark.Contains(remark));
            //}
            


            ////排序
            OrderByHelper<S_Revenue, DateTime> orderBy = new OrderByHelper<S_Revenue, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Revenue> list = RevenueService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_ProjectContract> listProjectContract = projectContractService.GetAllData().ToList();
            List<S_RevenueDetail> listDetail = new List<S_RevenueDetail>();

            foreach (S_Revenue item in list)
            {
                var s_RevenueDetail = new S_RevenueDetail();
                s_RevenueDetail.Id = item.Id;
                s_RevenueDetail.Num = item.Num;
                s_RevenueDetail.ContractNum = item.ContractNum;
                s_RevenueDetail.Remark = item.Remark;
                s_RevenueDetail.RevenueAmout = item.RevenueAmout;
                s_RevenueDetail.RevenueTime = item.RevenueTime;

                foreach (S_ProjectContract projectItem in listProjectContract)
                {
                    if (s_RevenueDetail.ContractNum == projectItem.Num)
                    {
                        s_RevenueDetail.ContractName = projectItem.ProjectContractName;
                        s_RevenueDetail.ContractAmout = projectItem.ProjectContractAmount;
                    }
                }
                listDetail.Add(s_RevenueDetail);
            }

            if (Request.Form["contractName"] != null && !string.IsNullOrEmpty(Request.Form["contractName"]))
            {
                listDetail=listDetail.Where(u => u.ContractName.Contains(Request.Form["contractName"])).ToList();
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


            Sys_User sUser = base.LoginUser;
            List<Sys_UserAndRole> currentUserRole = userAndRoleService.GetWhereData(u => u.UserId == sUser.Id).ToList();
            Guid? currentRoleId = currentUserRole[0].RoleId;
            List<Sys_Role> currentRoleList = roleService.GetWhereData(m => m.Id == currentRoleId).ToList();
            string roleName = currentRoleList[0].RoleName;
            Hashtable ht = new Hashtable();
            string json = null;
            if (roleName != "系统管理员" && roleName != "财务")
            {
                var userPojectList = projectService.GetWhereData(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).Select(m => m.Num);
                var result = from p in listDetail where userPojectList.Contains(p.ContractNum) select p;

                ht.Add("total", total);
                ht.Add("data", result);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

                return Content(json);
            }
            else
            {
                ht.Add("total", total);
                ht.Add("data", listDetail);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            }

           

            return Content(json);

        }

        public ActionResult GetPageDataBySelectedContractNum(int pageSize, int pageIndex, int ContractNum)
        {

            //条件
            Expression<Func<S_Revenue, bool>> expre = u => true;

            //if (Request.Form["remark"] != null && !string.IsNullOrEmpty(Request.Form["remark"]))
            //{
            //    string remark = Request.Form["remark"].Trim();
            //    expre = expre.And(u => u.Remark.Contains(remark));
            //}



            ////排序
            OrderByHelper<S_Revenue, DateTime> orderBy = new OrderByHelper<S_Revenue, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_Revenue> list = RevenueService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).Where(u=>u.ContractNum== ContractNum).ToList();
            List<S_ProjectContract> listProjectContract = projectContractService.GetAllData().Where(u=>u.Num== ContractNum).ToList();
            List<S_RevenueDetail> listDetail = new List<S_RevenueDetail>();

            foreach (S_Revenue item in list)
            {
                var s_RevenueDetail = new S_RevenueDetail();
                s_RevenueDetail.Id = item.Id;
                s_RevenueDetail.Num = item.Num;
                s_RevenueDetail.ContractNum = item.ContractNum;
                s_RevenueDetail.Remark = item.Remark;
                s_RevenueDetail.RevenueAmout = item.RevenueAmout;
                s_RevenueDetail.RevenueTime = item.RevenueTime;

                foreach (S_ProjectContract projectItem in listProjectContract)
                {
                    if (s_RevenueDetail.ContractNum == projectItem.Num)
                    {
                        s_RevenueDetail.ContractName = projectItem.ProjectContractName;
                        s_RevenueDetail.ContractAmout = projectItem.ProjectContractAmount;
                    }
                }
                listDetail.Add(s_RevenueDetail);
            }

            if (Request.Form["contractName"] != null && !string.IsNullOrEmpty(Request.Form["contractName"]))
            {
                listDetail = listDetail.Where(u => u.ContractName.Contains(Request.Form["contractName"])).ToList();
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
            S_Revenue eRevenue = new S_Revenue();
            List<S_Revenue> listRevenue = RevenueService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listRevenue);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddRevenue(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Revenue eRevenue = new S_Revenue();
            eRevenue.Id = Guid.NewGuid();
            //eSupplier.Type = int.Parse(arrType[0]);
            eRevenue.ContractNum= int.Parse(row["ContractNum"].ToString());
            eRevenue.RevenueAmout= Convert.ToDecimal(row["RevenueAmout"].ToString().Trim());
            eRevenue.RevenueTime = Convert.ToDateTime(row["RevenueTime"].ToString().Trim());
            eRevenue.Remark = row["Remark"].ToString().Trim();
            eRevenue.AddTime = DateTime.Now;
            eRevenue.AddUserId = base.LoginUser.Id;
            eRevenue.AddName = base.LoginUser.Name;
            eRevenue.LastEditName = base.LoginUser.Name;
            eRevenue.LastEditTime = DateTime.Now;
            eRevenue.LastEditUserID = base.LoginUser.Id;

            decimal? existRevenueSum = 0;
            List < S_Revenue > existRevenuelist = RevenueService.GetWhereData(u=>u.ContractNum== eRevenue.ContractNum).ToList();
            if (existRevenuelist.Count > 0)
            {
                foreach (S_Revenue existRevenue in existRevenuelist)
                {
                    existRevenueSum += existRevenue.RevenueAmout;
                }
            }
            List<S_ProjectContract> targetProjectContractList = projectContractService.GetWhereData(u => u.Num == eRevenue.ContractNum).ToList();
            S_ProjectContract targetProject = targetProjectContractList[0];
            decimal? contractAmout = targetProject.ProjectContractAmount;

            if (eRevenue.RevenueAmout <=0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "收入金额需大于0！");
                return Content(resultJson);
            }else if((contractAmout - existRevenueSum- eRevenue.RevenueAmout)<0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "收入超出合同剩余金额，请检查！");
                return Content(resultJson);
            }


            int num = RevenueService.Add(eRevenue);
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

        public ActionResult EditRevenue(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Revenue eRevenue = RevenueService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eRevenue == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String RevenueName = row["RevenueName"].ToString().Trim();
            //eRevenue.RevenueName = row["RevenueName"].ToString().Trim();

            //eRevenue.ProjectNum = int.Parse(row["Project"].ToString());

            eRevenue.RevenueAmout = Convert.ToDecimal(row["RevenueAmout"].ToString().Trim());
            eRevenue.RevenueTime = Convert.ToDateTime(row["RevenueTime"].ToString().Trim());
            eRevenue.Remark = row["Remark"].ToString().Trim();
            eRevenue.LastEditName = base.LoginUser.Name;
            eRevenue.LastEditTime = DateTime.Now;
            eRevenue.LastEditUserID = base.LoginUser.Id;


            int num = RevenueService.Update(eRevenue);
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

        public ActionResult DeleteRevenue(Guid RevenueId)
        {
            int num = RevenueService.Delete(RevenueId);
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

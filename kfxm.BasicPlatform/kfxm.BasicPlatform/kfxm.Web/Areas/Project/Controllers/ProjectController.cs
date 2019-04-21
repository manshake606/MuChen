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
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.Entity.Client;
using kfxms.IService.Client;

namespace kfxms.Web.Areas.Project.Controllers
{
    [Export]
    public class ProjectController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ClientService clientService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("ProjectList");
        }

        public ActionResult Add()
        {
            return View("ProjectAdd");
        }

        public ActionResult Edit(Guid ProjectId)
        {
            S_Project sys_Project = projectService.GetByKey(ProjectId);
            

            if (sys_Project == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectEdit", sys_Project);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_Project, bool>> expre = u => true;

            if (Request.Form["ProjectName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectName"]))
            {
                string ProjectName = Request.Form["ProjectName"].Trim();
                expre = expre.And(u => u.ProjectName.Contains(ProjectName));
            }
            


            ////排序
            OrderByHelper<S_Project, int?> orderBy = new OrderByHelper<S_Project, int?>() { OrderByType = OrderByType.DESC, Expression = u => u.Num };

            int total = 0;

            List<S_Project> list = projectService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();

            List<S_Client> listClient = clientService.GetAllData().ToList();

            List<S_ProjectInfo> listProjectInfo = new List<S_ProjectInfo>();

            foreach (S_Project item in list)
            {
                S_ProjectInfo s_ProjectInfo = new S_ProjectInfo();
                s_ProjectInfo.Id = item.Id;
                s_ProjectInfo.Num = item.Num;
                s_ProjectInfo.ProjectName = item.ProjectName;
                s_ProjectInfo.SettlementBase = item.SettlementBase;
                s_ProjectInfo.Status = item.Status;
                if (s_ProjectInfo.Status == 1)
                {
                    s_ProjectInfo.StatusName = "激活";
                }else if(s_ProjectInfo.Status == 0)
                {
                    s_ProjectInfo.StatusName = "结束";
                }
                s_ProjectInfo.ClientId = item.ClientId;
                s_ProjectInfo.ContractAmout = item.ContractAmout;
                s_ProjectInfo.Remark = item.Remark;
                foreach (S_Client clientItem in listClient)
                {
                    if (s_ProjectInfo.ClientId == clientItem.Num)
                    {
                        s_ProjectInfo.TelephoneNum = clientItem.TelephoneNum;
                        s_ProjectInfo.ClientName = clientItem.ClientName;
                        s_ProjectInfo.ClientAddress = clientItem.ClientAddress;
                        s_ProjectInfo.ClientContact = clientItem.ClientContact;
                        s_ProjectInfo.ClientContactMobile = clientItem.ClientContactMobile;
                        s_ProjectInfo.ClientContactPosition= clientItem.ClientContactPosition;
                    }
                }
                listProjectInfo.Add(s_ProjectInfo);
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
            ht.Add("data", listProjectInfo);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Project eProject = new S_Project();
            List<S_Project> listProject = projectService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProject);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddProject(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Project eProject = new S_Project();
            eProject.Id = Guid.NewGuid();
            eProject.ProjectName = row["ProjectName"].ToString().Trim();
            //eProject.Password = MD5Helper.GetMD5("123456");
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            eProject.ClientId = int.Parse(row["Client"].ToString());
            eProject.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            eProject.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            eProject.Status = 1;
            eProject.Remark = row["Remark"].ToString().Trim();

            List<S_Project> listProject = projectService.GetWhereData(u => u.ProjectName.Equals(eProject.ProjectName)).ToList();
            if (listProject.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该甲方已经存在！");
               return Content(resultJson);
            }
            int num = projectService.Add(eProject);
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

        public ActionResult EditProject(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Project eProject = projectService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eProject == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            String ProjectName = row["ProjectName"].ToString().Trim();
            //eProject.ProjectName = row["ProjectName"].ToString().Trim();
            if (!eProject.ProjectName.Equals(ProjectName))
            {
                List<S_Project> listProject = projectService.GetWhereData(u => u.ProjectName.Equals(ProjectName)).ToList();
                if (listProject.Count > 0)
                {
                    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该项目名称已经存在！");
                    return Content(resultJson);
                }
            }
            eProject.ProjectName = ProjectName;
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProject.ClientId = int.Parse(arrClient[0]);
            eProject.ClientId = int.Parse(row["Client"].ToString().Trim());
            eProject.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            eProject.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            eProject.Status = int.Parse(row["Status"].ToString().Trim());
            eProject.Remark = row["Remark"].ToString().Trim();

            int num = projectService.Update(eProject);
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

        public ActionResult DeleteProject(Guid ProjectId)
        {
            int num = projectService.Delete(ProjectId);
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

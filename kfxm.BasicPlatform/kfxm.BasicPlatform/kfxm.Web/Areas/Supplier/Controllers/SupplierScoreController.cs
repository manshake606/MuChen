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
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.Entity.SupplierType;
using kfxms.IService.SupplierType;



namespace kfxms.Web.Areas.Supplier.Controllers
{
    [Export]
    public class SupplierScoreController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierScoreService SupplierScoreService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierService supplierService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierTypeService SupplierTypeService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("SupplierScoreList");
        }

        public ActionResult Add()
        {
            return View("SupplierScoreAdd");
        }

        public ActionResult Edit(Guid SupplierScoreId)
        {
            S_SupplierScore sys_SupplierScore = SupplierScoreService.GetByKey(SupplierScoreId);
            

            if (sys_SupplierScore == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("SupplierScoreEdit", sys_SupplierScore);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_SupplierScore eSupplierScore = new S_SupplierScore();
            List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listSupplierScore);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult GetAllDataByProjectNum(int ProjectNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_SupplierScore eSupplierScore = new S_SupplierScore();
            List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData().Where(u=>u.ProjectNum== ProjectNum).ToList();
            List<S_SupplierScoreDetail> listSupplierScoreDetail = new List<S_SupplierScoreDetail>();
            List<S_Project> listProject = listProject = projectService.GetAllData().ToList();
            List<S_Supplier> listSupplier = supplierService.GetAllData().ToList();

            foreach (S_SupplierScore item in listSupplierScore)
            {
                var S_SupplierScoreDetail = new S_SupplierScoreDetail();
                S_SupplierScoreDetail.ProjectNum = item.ProjectNum;
                S_SupplierScoreDetail.Id = item.Id;

                S_SupplierScoreDetail.Num = item.Num;

                foreach (S_Project typeItem in listProject)
                {
                    if (S_SupplierScoreDetail.ProjectNum == typeItem.Num)
                    {
                        S_SupplierScoreDetail.ProjectName = typeItem.ProjectName;
                    }
                }
                S_SupplierScoreDetail.SupplierNum = item.SupplierNum;
                foreach (S_Supplier typeItem in listSupplier)
                {
                    if (S_SupplierScoreDetail.SupplierNum == typeItem.Num)
                    {
                        S_SupplierScoreDetail.SupplierName = typeItem.SupplierName;
                    }
                }
                S_SupplierScoreDetail.SupplierScore = item.SupplierScore;
                S_SupplierScoreDetail.ScoreRemark = item.ScoreRemark;

                listSupplierScoreDetail.Add(S_SupplierScoreDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listSupplierScoreDetail);
            ht.Add("code", 0);

            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }



        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_SupplierScore, bool>> expre = u => true;

            //if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectNum"]))
            //{
            //    int projectNum = int.Parse(Request.Form["projectNum"].Trim());
            //    expre = expre.And(u => u.ProjectNum==projectNum);
            //}
            //if (Request.Form["corporationName"] != null && !string.IsNullOrEmpty(Request.Form["corporationName"]))
            //{
            //    string corporationName = Request.Form["corporationName"].Trim();
            //    expre = expre.And(u => u.CorporationName.Contains(corporationName));
            //}


            ////排序
            OrderByHelper<S_SupplierScore, DateTime> orderBy = new OrderByHelper<S_SupplierScore, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_SupplierScore> list = SupplierScoreService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_SupplierScoreDetail> listSupplierScoreDetail = new List<S_SupplierScoreDetail>();
            List<S_Project> listProject = listProject = projectService.GetAllData().ToList();
            List<S_Supplier> listSupplier = supplierService.GetAllData().ToList();
            //List<S_SupplierScoreType> typeList = SupplierScoreTypeService.GetAllData().ToList();
            //List<S_SupplierScoreHasTypeName> listHasTypeName = new List<S_SupplierScoreHasTypeName>();

            foreach (S_SupplierScore item in list)
            {
                var S_SupplierScoreDetail = new S_SupplierScoreDetail();
                S_SupplierScoreDetail.ProjectNum = item.ProjectNum;
                S_SupplierScoreDetail.Id = item.Id;

                S_SupplierScoreDetail.Num = item.Num;

                foreach (S_Project typeItem in listProject)
                {
                    if (S_SupplierScoreDetail.ProjectNum == typeItem.Num)
                    {
                        S_SupplierScoreDetail.ProjectName = typeItem.ProjectName;
                    }
                }
                S_SupplierScoreDetail.SupplierNum = item.SupplierNum;
                foreach (S_Supplier typeItem in listSupplier)
                {
                    if (S_SupplierScoreDetail.SupplierNum == typeItem.Num)
                    {
                        S_SupplierScoreDetail.SupplierName = typeItem.SupplierName;
                    }
                }
                S_SupplierScoreDetail.SupplierScore = item.SupplierScore;
                S_SupplierScoreDetail.ScoreRemark = item.ScoreRemark;

                listSupplierScoreDetail.Add(S_SupplierScoreDetail);
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

            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                string projectName = Request.Form["projectName"].Trim();
                listSupplierScoreDetail = listSupplierScoreDetail.Where(u => u.ProjectName.Contains(projectName)).ToList();
            }



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
                var result = from p in listSupplierScoreDetail where userPojectList.Contains(p.ProjectNum) select p;

                //ht.Add("total", total);
                ht.Add("data", result);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

                return Content(json);
            }
            else
            {
                //ht.Add("total", total);
                ht.Add("data", listSupplierScoreDetail);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            }


            return Content(json);

        }

        public ActionResult AddSupplierScore(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_SupplierScore eSupplierScore = new S_SupplierScore();
            eSupplierScore.Id = Guid.NewGuid();
            eSupplierScore.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eSupplierScore.SupplierNum= int.Parse(row["Supplier"].ToString().Trim());
            eSupplierScore.SupplierScore= int.Parse(row["SupplierScore"].ToString().Trim());
            eSupplierScore.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            eSupplierScore.AddTime = DateTime.Now;
            eSupplierScore.AddUserId = base.LoginUser.Id;
            eSupplierScore.AddName = base.LoginUser.Name;
            eSupplierScore.LastEditName = base.LoginUser.Name;
            eSupplierScore.LastEditTime = DateTime.Now;
            eSupplierScore.LastEditUserID = base.LoginUser.Id;
            //eSupplierScore.Password = MD5Helper.GetMD5("123456");
            // eSupplierScore.CorporationName = row["CorporationName"].ToString().Trim();
            //eSupplierScore.CorporationMobile = row["CorporationMobile"].ToString().Trim();
            //eSupplierScore.ContactName = row["ContactName"].ToString().Trim();
            //eSupplierScore.ContactMobile = row["ContactMobile"].ToString().Trim();
            //eSupplierScore.Address = row["Address"].ToString().Trim();
            //eSupplierScore.Remark = row["Remark"].ToString().Trim();
            //eSupplierScore.Type = int.Parse(row["Type"].ToString());

            List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetWhereData(u => u.ProjectNum == eSupplierScore.ProjectNum && u.SupplierNum==eSupplierScore.SupplierNum).ToList();
            if (listSupplierScore.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该供应商评分已经存在！");
                return Content(resultJson);
            }
            int num = SupplierScoreService.Add(eSupplierScore);
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

        public ActionResult EditSupplierScore(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_SupplierScore eSupplierScore = SupplierScoreService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eSupplierScore == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String SupplierScoreName = row["SupplierScoreName"].ToString().Trim();
            //eSupplierScore.SupplierScoreName = row["SupplierScoreName"].ToString().Trim();
            //if (!eSupplierScore.SupplierScoreName.Equals(SupplierScoreName))
            //{
            //    List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetWhereData(u => u.SupplierScoreName.Equals(SupplierScoreName)).ToList();
            //    if (listSupplierScore.Count > 0)
            //    {
            //        resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该公司名称已经存在！");
            //        return Content(resultJson);
            //    }
            //}
            eSupplierScore.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eSupplierScore.SupplierNum = int.Parse(row["Supplier"].ToString().Trim());
            eSupplierScore.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            eSupplierScore.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            eSupplierScore.LastEditName = base.LoginUser.Name;
            eSupplierScore.LastEditTime = DateTime.Now;
            eSupplierScore.LastEditUserID = base.LoginUser.Id;

            int num = SupplierScoreService.Update(eSupplierScore);
            if (num > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "修改成功！");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "修改失败！");
            }
            return Content(resultJson);
            //return true;
        }

        public ActionResult DeleteSupplierScore(Guid supplierScoreId)
        {
            int num = SupplierScoreService.Delete(supplierScoreId);
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

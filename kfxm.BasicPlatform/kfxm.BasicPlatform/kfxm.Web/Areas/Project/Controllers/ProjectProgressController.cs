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
using kfxms.Entity.Supplier;
using kfxms.IService.Supplier;
using kfxms.IService.SupplierType;
using kfxms.Entity.Payment;
using kfxms.IService.Payment;
using Newtonsoft.Json;

namespace kfxms.Web.Areas.Project.Controllers
{
    [Export]
    public class ProjectProgressController : BaseController
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
        //public new  userService { get; set; }
        public IS_SupplierService supplierService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierScoreService SupplierScoreService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ExternalPaymentService ExternalPaymentService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectProgressService ProjectProgressService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List(Guid ProjectId)
        {
            S_Project s_Project = projectService.GetByKey(ProjectId);
            if (s_Project == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectProgressList", s_Project);
        }

        public ActionResult Add(Guid ProjectId)
        {
            S_Project s_Project = projectService.GetByKey(ProjectId);
            if (s_Project == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectProgressAdd", s_Project);
        }

        public ActionResult Edit(Guid projectProgressId)
        {
            S_ProjectProgress sys_ProjectProgress = ProjectProgressService.GetByKey(projectProgressId);
            

            if (sys_ProjectProgress == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectProgressEdit", sys_ProjectProgress);

        }

        
        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_ProjectProgress, bool>> expre = u => true;

            //if (Request.Form["ProjectProgressName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectProgressName"]))
            //{
            //    string ProjectProgressName = Request.Form["ProjectProgressName"].Trim();
            //    expre = expre.And(u => u.ProjectProgressName.Contains(ProjectProgressName));
            //}



            ////排序
            OrderByHelper<S_ProjectProgress, int?> orderBy = new OrderByHelper<S_ProjectProgress, int?>() { OrderByType = OrderByType.DESC, Expression = u => u.Num };

            int total = 0;

            List<S_ProjectProgress> list = ProjectProgressService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();
            //List<S_ProjectProgressDetail> listProjectAndSupplierDetail = new List<S_ProjectProgressDetail>();
            //List<S_Project> ListProject = projectService.GetAllData().ToList();
            //List<S_Supplier> ListSupplier = supplierService.GetAllData().ToList();
            List<S_ProjectProgressDetail> listProjectProgressDetail = new List<S_ProjectProgressDetail>();
            //List<S_Client> listClient = clientService.GetAllData().ToList();


            foreach (S_ProjectProgress item in list)
            {
                S_ProjectProgressDetail s_ProjectProgressDetail = new S_ProjectProgressDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num ==(item.ProjectNum)).ToList();
                
                s_ProjectProgressDetail.Id = item.Id;
                s_ProjectProgressDetail.ProjectNum = item.ProjectNum;
                s_ProjectProgressDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectProgressDetail.ProjectDetail = item.ProjectDetail;
                s_ProjectProgressDetail.AddName = item.AddName;
                s_ProjectProgressDetail.AddTime = item.AddTime;
                s_ProjectProgressDetail.AddUserId = item.AddUserId;
                s_ProjectProgressDetail.LastEditName = item.LastEditName;
                s_ProjectProgressDetail.LastEditTime = item.LastEditTime;
                s_ProjectProgressDetail.AddUserId = item.AddUserId;



                listProjectProgressDetail.Add(s_ProjectProgressDetail);
            }

            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                listProjectProgressDetail = listProjectProgressDetail.Where(u => u.ProjectName.Contains((Request.Form["projectName"]))).ToList();
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
            //    var result = from p in listProjectProgressDetail where userPojectList.Contains(p.ProjectNum) select p;

            //    //ht.Add("total", total);
            //    ht.Add("data", result);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            //    return Content(json);
            //}
            //else
            //{
                //ht.Add("total", total);
                ht.Add("data", listProjectProgressDetail);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            //}
            
            

            return Content(json);

            

        }


            
        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = new S_ProjectProgress();
            List<S_ProjectProgress> listProjectProgress = ProjectProgressService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectProgress);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        
        public ActionResult GetAllDataByProjectNum(int ProjectNum)
        {
            
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = new S_ProjectProgress();
            List<S_ProjectProgress> listProjectProgress = ProjectProgressService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();
            //listProjectProgress = listProjectProgress.Where(u => u.ProjectNum == ProjectNum).ToList();
            List<S_ProjectProgressDetail> listProjectProgressDetail = new List<S_ProjectProgressDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData();

            foreach (S_ProjectProgress item in listProjectProgress)
            {
                S_ProjectProgressDetail s_ProjectProgressDetail = new S_ProjectProgressDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                s_ProjectProgressDetail.Id = item.Id;
                s_ProjectProgressDetail.Num = item.Num;
                s_ProjectProgressDetail.ProjectNum = item.ProjectNum;
                s_ProjectProgressDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectProgressDetail.ProjectDetail = item.ProjectDetail;
                s_ProjectProgressDetail.LastEditName = item.LastEditName;
                s_ProjectProgressDetail.LastEditTime = item.LastEditTime;
                listProjectProgressDetail.Add(s_ProjectProgressDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("code", 0);
            ht.Add("msg", "");
            ht.Add("data", listProjectProgressDetail);

            string json=JsonConvert.SerializeObject(ht);
            
           
            return Content(json);

            
    }

        public ActionResult GetAllDataBySelectedProjectNum(int ProjectNum)
        {

            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = new S_ProjectProgress();
            List<S_ProjectProgress> listProjectProgress = ProjectProgressService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();
            //listProjectProgress = listProjectProgress.Where(u => u.ProjectNum == ProjectNum).ToList();
            List<S_ProjectProgressDetail> listProjectProgressDetail = new List<S_ProjectProgressDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData();

            foreach (S_ProjectProgress item in listProjectProgress)
            {
                S_ProjectProgressDetail s_ProjectProgressDetail = new S_ProjectProgressDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                s_ProjectProgressDetail.Id = item.Id;
                s_ProjectProgressDetail.Num = item.Num;
                s_ProjectProgressDetail.ProjectNum = item.ProjectNum;
                s_ProjectProgressDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectProgressDetail.ProjectDetail = item.ProjectDetail;
                s_ProjectProgressDetail.LastEditName = item.LastEditName;
                s_ProjectProgressDetail.LastEditTime = item.LastEditTime;
                listProjectProgressDetail.Add(s_ProjectProgressDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectProgressDetail);
            string json = null;

            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);


            return Content(json);


        }


        /*
        public ActionResult GetAllDataBySupplierNum(int SupplierNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = new S_ProjectProgress();
            List<S_ProjectProgress> listProjectProgress = ProjectProgressService.GetAllData().ToList();
            listProjectProgress = listProjectProgress.Where(u => u.SupplierNum == SupplierNum).ToList();
            List<S_ProjectProgressDetail> listProjectProgressDetail = new List<S_ProjectProgressDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();

            foreach (S_ProjectProgress item in listProjectProgress)
            {
                S_ProjectProgressDetail s_ProjectProgressDetail = new S_ProjectProgressDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                List<S_Supplier> ListSupplier = supplierService.GetWhereData(u => u.Num == (item.SupplierNum)).ToList();
                s_ProjectProgressDetail.Id = item.Id;
                s_ProjectProgressDetail.ProjectNum = item.ProjectNum;
                s_ProjectProgressDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectProgressDetail.SupplierNum = item.SupplierNum;
                s_ProjectProgressDetail.SupplierName = ListSupplier[0].SupplierName;
                s_ProjectProgressDetail.SupplierScore = item.SupplierScore;
                s_ProjectProgressDetail.ScoreRemark = item.ScoreRemark;
                s_ProjectProgressDetail.SupplierContractAmout = item.SupplierContractAmout;
                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectProgressDetail.SupplierNum)
                //    {
                //        s_ProjectProgressDetail.SupplierScore = score.SupplierScore;
                //    }
                //}
                List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == item.ProjectNum && u.ExternalPaymentSupplier == item.SupplierNum).ToList();
                decimal? SupplierCurrentPaymentAmout = 0;
                if (listExternalPayment.Count > 0)
                {
                    foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                    {
                        SupplierCurrentPaymentAmout += ExternalPayment.ExternalPaymentAmout;
                    }
                }
                decimal? SupplierLeftPaymentAmout = s_ProjectProgressDetail.SupplierContractAmout - SupplierCurrentPaymentAmout;
                s_ProjectProgressDetail.SupplierCurrentPaymentAmout = SupplierCurrentPaymentAmout;
                s_ProjectProgressDetail.SupplierLeftPaymentAmout = SupplierLeftPaymentAmout;

                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectProgressDetail.SupplierNum)
                //    {
                //        s_ProjectProgressDetail.SupplierScore = score.SupplierScore;
                //    }
                //}


                listProjectProgressDetail.Add(s_ProjectProgressDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectProgressDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        */

        public ActionResult AddProjectProgress(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = new S_ProjectProgress();
            eProjectProgress.Id = Guid.NewGuid();
            eProjectProgress.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            eProjectProgress.ProjectDetail = row["ProjectDetail"].ToString().Trim();
            /*
            //eProjectProgress.SupplierNum= int.Parse(row["Supplier"].ToString().Trim());
            if(row["SupplierScore"] == null || row["SupplierScore"].ToString() == "")
            {
                eProjectProgress.SupplierScore = null;
            }
            else
            {
                eProjectProgress.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            }
            eProjectProgress.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            if(row["SupplierContractAmout"]==null || row["SupplierContractAmout"].ToString() == "")
            {
                eProjectProgress.SupplierContractAmout = null;
            }else
            {
                eProjectProgress.SupplierContractAmout = Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            }
            */
            eProjectProgress.AddTime = DateTime.Now;
            eProjectProgress.AddUserId = base.LoginUser.Id;
            eProjectProgress.AddName = base.LoginUser.Name;
            eProjectProgress.LastEditName = base.LoginUser.Name;
            eProjectProgress.LastEditTime = DateTime.Now;
            eProjectProgress.LastEditUserID = base.LoginUser.Id;

            //eProjectProgress.ProjectProgressName = row["ProjectProgressName"].ToString().Trim();
            //eProjectProgress.Password = MD5Helper.GetMD5("123456");
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectProgress.ClientId = int.Parse(row["Client"].ToString());
            //eProjectProgress.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectProgress.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectProgress.Status = 1;
            //eProjectProgress.Remark = row["Remark"].ToString().Trim();

            //List<S_ProjectProgress> listProjectProgress = ProjectProgressService.GetWhereData(u => u.ProjectId.Equals(eProjectProgress.ProjectId)).ToList();
            //List<S_ProjectProgress> listSupplierScore = ProjectProgressService.GetWhereData(u => u.ProjectNum == eProjectProgress.ProjectNum).ToList();
            //if (listSupplierScore.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该供应商关系已经存在！");
            //    return Content(resultJson);
            //}


            int num = ProjectProgressService.Add(eProjectProgress);
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

        
        public ActionResult EditProjectProgress(string data)
        {
            
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectProgress eProjectProgress = ProjectProgressService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eProjectProgress == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String ProjectProgressName = row["ProjectProgressName"].ToString().Trim();
            //eProjectProgress.ProjectProgressName = row["ProjectProgressName"].ToString().Trim();\

            eProjectProgress.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eProjectProgress.ProjectDetail = row["ProjectDetail"].ToString().Trim();

            //eProjectProgress.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            //eProjectProgress.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            //eProjectProgress.SupplierContractAmout = Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            //eProjectProgress.ProjectProgressName = ProjectProgressName;
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectProgress.ClientId = int.Parse(arrClient[0]);
            //eProjectProgress.ClientId = int.Parse(row["Client"].ToString().Trim());
            //eProjectProgress.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectProgress.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectProgress.Status = int.Parse(row["Status"].ToString().Trim());
            //eProjectProgress.Remark = row["Remark"].ToString().Trim();
            eProjectProgress.LastEditName = base.LoginUser.Name;
            eProjectProgress.LastEditTime = DateTime.Now;
            eProjectProgress.LastEditUserID = base.LoginUser.Id;
            int num = ProjectProgressService.Update(eProjectProgress);
            
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
            


        public ActionResult DeleteProjectProgress(Guid ProjectProgressId)
        {
            
            S_ProjectProgress eProjectProgress = ProjectProgressService.GetByKey(ProjectProgressId);
            //List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == eProjectProgress.ProjectNum && u.ExternalPaymentSupplier == eProjectProgress.SupplierNum).ToList();



            string resultJson = "";
            //if (listExternalPayment.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框不关闭窗体, "该供应商已有付款信息，请删除付款信息后再删除该供应商关系！");
            //    return Content(resultJson);
            //}
            int num = ProjectProgressService.Delete(ProjectProgressId);
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

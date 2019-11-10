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
    public class ProjectContractController : BaseController
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
        public IS_ProjectContractService ProjectContractService { get; set; }

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
            return View("ProjectContractList", s_Project);
        }



        public ActionResult Add(Guid ProjectId)
        {
            S_Project s_Project = projectService.GetByKey(ProjectId);
            if (s_Project == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectContractAdd",s_Project);
        }

        public ActionResult Edit(Guid projectContractId)
        {
            S_ProjectContract sys_ProjectContract = ProjectContractService.GetByKey(projectContractId);


            if (sys_ProjectContract == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectContractEdit", sys_ProjectContract);

        }


        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_ProjectContract, bool>> expre = u => true;

            //if (Request.Form["ProjectContractName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectContractName"]))
            //{
            //    string ProjectContractName = Request.Form["ProjectContractName"].Trim();
            //    expre = expre.And(u => u.ProjectContractName.Contains(ProjectContractName));
            //}



            ////排序
            OrderByHelper<S_ProjectContract, int?> orderBy = new OrderByHelper<S_ProjectContract, int?>() { OrderByType = OrderByType.DESC, Expression = u => u.Num };

            int total = 0;

            List<S_ProjectContract> list = ProjectContractService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();
            //List<S_ProjectContractDetail> listProjectAndSupplierDetail = new List<S_ProjectContractDetail>();
            //List<S_Project> ListProject = projectService.GetAllData().ToList();
            //List<S_Supplier> ListSupplier = supplierService.GetAllData().ToList();
            List<S_ProjectContractDetail> listProjectContractDetail = new List<S_ProjectContractDetail>();
            //List<S_Client> listClient = clientService.GetAllData().ToList();


            foreach (S_ProjectContract item in list)
            {
                S_ProjectContractDetail s_ProjectContractDetail = new S_ProjectContractDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();

                s_ProjectContractDetail.Id = item.Id;
                s_ProjectContractDetail.ProjectNum = item.ProjectNum;
                s_ProjectContractDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectContractDetail.ProjectContractName = item.ProjectContractName;
                s_ProjectContractDetail.ProjectContractDetail = item.ProjectContractDetail;
                s_ProjectContractDetail.ProjectContractAmount = item.ProjectContractAmount;
                s_ProjectContractDetail.AddName = item.AddName;
                s_ProjectContractDetail.AddTime = item.AddTime;
                s_ProjectContractDetail.AddUserId = item.AddUserId;
                s_ProjectContractDetail.LastEditName = item.LastEditName;
                s_ProjectContractDetail.LastEditTime = item.LastEditTime;
                s_ProjectContractDetail.AddUserId = item.AddUserId;



                listProjectContractDetail.Add(s_ProjectContractDetail);
            }

            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                listProjectContractDetail = listProjectContractDetail.Where(u => u.ProjectName.Contains((Request.Form["projectName"]))).ToList();
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
            //    var result = from p in listProjectContractDetail where userPojectList.Contains(p.ProjectNum) select p;

            //    //ht.Add("total", total);
            //    ht.Add("data", result);
            //    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            //    return Content(json);
            //}
            //else
            //{
            //ht.Add("total", total);
            ht.Add("data", listProjectContractDetail);
            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            //}



            return Content(json);



        }



        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = new S_ProjectContract();
            List<S_ProjectContract> listProjectContract = ProjectContractService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectContract);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }


        public ActionResult GetAllDataByProjectNum(int ProjectNum)
        {
            
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = new S_ProjectContract();
            List<S_ProjectContract> listProjectContract = ProjectContractService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();
            //listProjectContract = listProjectContract.Where(u => u.ProjectNum == ProjectNum).ToList();
            List<S_ProjectContractDetail> listProjectContractDetail = new List<S_ProjectContractDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData();

            foreach (S_ProjectContract item in listProjectContract)
            {
                S_ProjectContractDetail s_ProjectContractDetail = new S_ProjectContractDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                s_ProjectContractDetail.Id = item.Id;
                s_ProjectContractDetail.Num = item.Num;
                s_ProjectContractDetail.ProjectNum = item.ProjectNum;
                s_ProjectContractDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectContractDetail.ProjectContractName = item.ProjectContractName;
                s_ProjectContractDetail.ProjectContractDetail = item.ProjectContractDetail;
                s_ProjectContractDetail.ProjectContractAmount = item.ProjectContractAmount;
                s_ProjectContractDetail.LastEditName = item.LastEditName;
                s_ProjectContractDetail.LastEditTime = item.LastEditTime;
                


                listProjectContractDetail.Add(s_ProjectContractDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            //ht.Add("code", 0);
            //ht.Add("msg", "");
            ht.Add("data", listProjectContractDetail);


            string json = JsonConvert.SerializeObject(ht);
            return Content(json);

            
    }

        public ActionResult GetAllDataBySelectedProjectNum(int ProjectNum)
        {

            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = new S_ProjectContract();
            List<S_ProjectContract> listProjectContract = ProjectContractService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();
            //listProjectContract = listProjectContract.Where(u => u.ProjectNum == ProjectNum).ToList();
            List<S_ProjectContractDetail> listProjectContractDetail = new List<S_ProjectContractDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData();

            foreach (S_ProjectContract item in listProjectContract)
            {
                S_ProjectContractDetail s_ProjectContractDetail = new S_ProjectContractDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                s_ProjectContractDetail.Id = item.Id;
                s_ProjectContractDetail.Num = item.Num;
                s_ProjectContractDetail.ProjectNum = item.ProjectNum;
                s_ProjectContractDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectContractDetail.ProjectContractName = item.ProjectContractName;
                s_ProjectContractDetail.ProjectContractDetail = item.ProjectContractDetail;
                s_ProjectContractDetail.ProjectContractAmount = item.ProjectContractAmount;
                s_ProjectContractDetail.LastEditName = item.LastEditName;
                s_ProjectContractDetail.LastEditTime = item.LastEditTime;



                listProjectContractDetail.Add(s_ProjectContractDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectContractDetail);
            string json = null;

            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);


        }


        /*
        public ActionResult GetAllDataBySupplierNum(int SupplierNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = new S_ProjectContract();
            List<S_ProjectContract> listProjectContract = ProjectContractService.GetAllData().ToList();
            listProjectContract = listProjectContract.Where(u => u.SupplierNum == SupplierNum).ToList();
            List<S_ProjectContractDetail> listProjectContractDetail = new List<S_ProjectContractDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();

            foreach (S_ProjectContract item in listProjectContract)
            {
                S_ProjectContractDetail s_ProjectContractDetail = new S_ProjectContractDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                List<S_Supplier> ListSupplier = supplierService.GetWhereData(u => u.Num == (item.SupplierNum)).ToList();
                s_ProjectContractDetail.Id = item.Id;
                s_ProjectContractDetail.ProjectNum = item.ProjectNum;
                s_ProjectContractDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectContractDetail.SupplierNum = item.SupplierNum;
                s_ProjectContractDetail.SupplierName = ListSupplier[0].SupplierName;
                s_ProjectContractDetail.SupplierScore = item.SupplierScore;
                s_ProjectContractDetail.ScoreRemark = item.ScoreRemark;
                s_ProjectContractDetail.SupplierContractAmout = item.SupplierContractAmout;
                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectContractDetail.SupplierNum)
                //    {
                //        s_ProjectContractDetail.SupplierScore = score.SupplierScore;
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
                decimal? SupplierLeftPaymentAmout = s_ProjectContractDetail.SupplierContractAmout - SupplierCurrentPaymentAmout;
                s_ProjectContractDetail.SupplierCurrentPaymentAmout = SupplierCurrentPaymentAmout;
                s_ProjectContractDetail.SupplierLeftPaymentAmout = SupplierLeftPaymentAmout;

                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectContractDetail.SupplierNum)
                //    {
                //        s_ProjectContractDetail.SupplierScore = score.SupplierScore;
                //    }
                //}


                listProjectContractDetail.Add(s_ProjectContractDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectContractDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        */

        public ActionResult AddProjectContract(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = new S_ProjectContract();
            eProjectContract.Id = Guid.NewGuid();
            eProjectContract.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eProjectContract.ProjectContractName = row["ProjectContractName"].ToString().Trim();
            eProjectContract.ProjectContractDetail = row["ProjectContractDetail"].ToString().Trim();
            eProjectContract.ProjectContractAmount = Convert.ToDecimal( row["ProjectContractAmount"].ToString().Trim());
            /*
            //eProjectContract.SupplierNum= int.Parse(row["Supplier"].ToString().Trim());
            if(row["SupplierScore"] == null || row["SupplierScore"].ToString() == "")
            {
                eProjectContract.SupplierScore = null;
            }
            else
            {
                eProjectContract.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            }
            eProjectContract.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            if(row["SupplierContractAmout"]==null || row["SupplierContractAmout"].ToString() == "")
            {
                eProjectContract.SupplierContractAmout = null;
            }else
            {
                eProjectContract.SupplierContractAmout = Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            }
            */
            eProjectContract.AddTime = DateTime.Now;
            eProjectContract.AddUserId = base.LoginUser.Id;
            eProjectContract.AddName = base.LoginUser.Name;
            eProjectContract.LastEditName = base.LoginUser.Name;
            eProjectContract.LastEditTime = DateTime.Now;
            eProjectContract.LastEditUserID = base.LoginUser.Id;

            //eProjectContract.ProjectContractName = row["ProjectContractName"].ToString().Trim();
            //eProjectContract.Password = MD5Helper.GetMD5("123456");
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectContract.ClientId = int.Parse(row["Client"].ToString());
            //eProjectContract.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectContract.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectContract.Status = 1;
            //eProjectContract.Remark = row["Remark"].ToString().Trim();

            //List<S_ProjectContract> listProjectContract = ProjectContractService.GetWhereData(u => u.ProjectId.Equals(eProjectContract.ProjectId)).ToList();
            //List<S_ProjectContract> listSupplierScore = ProjectContractService.GetWhereData(u => u.ProjectNum == eProjectContract.ProjectNum).ToList();
            //if (listSupplierScore.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该供应商关系已经存在！");
            //    return Content(resultJson);
            //}


            int num = ProjectContractService.Add(eProjectContract);
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


        public ActionResult EditProjectContract(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectContract eProjectContract = ProjectContractService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eProjectContract == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String ProjectContractName = row["ProjectContractName"].ToString().Trim();
            //eProjectContract.ProjectContractName = row["ProjectContractName"].ToString().Trim();\

            eProjectContract.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eProjectContract.ProjectContractName = row["ProjectContractName"].ToString().Trim();
            eProjectContract.ProjectContractDetail = row["ProjectContractDetail"].ToString().Trim();
            eProjectContract.ProjectContractAmount = Convert.ToDecimal(row["ProjectContractAmount"].ToString().Trim());
            //eProjectContract.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            //eProjectContract.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            //eProjectContract.SupplierContractAmout = Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            //eProjectContract.ProjectContractName = ProjectContractName;
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectContract.ClientId = int.Parse(arrClient[0]);
            //eProjectContract.ClientId = int.Parse(row["Client"].ToString().Trim());
            //eProjectContract.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectContract.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectContract.Status = int.Parse(row["Status"].ToString().Trim());
            //eProjectContract.Remark = row["Remark"].ToString().Trim();

            eProjectContract.LastEditName = base.LoginUser.Name;
            eProjectContract.LastEditTime = DateTime.Now;
            eProjectContract.LastEditUserID = base.LoginUser.Id;
            int num = ProjectContractService.Update(eProjectContract);
           
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



        public ActionResult DeleteProjectContract(Guid ProjectContractId)
        {

            S_ProjectContract eProjectContract = ProjectContractService.GetByKey(ProjectContractId);
            //List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == eProjectContract.ProjectNum && u.ExternalPaymentSupplier == eProjectContract.SupplierNum).ToList();



            string resultJson = "";
            //if (listExternalPayment.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框不关闭窗体, "该供应商已有付款信息，请删除付款信息后再删除该供应商关系！");
            //    return Content(resultJson);
            //}
            int num = ProjectContractService.Delete(ProjectContractId);
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

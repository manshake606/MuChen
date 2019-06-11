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
using kfxms.Entity.SupplierType;
using kfxms.IService.SupplierType;
using kfxms.Entity.Payment;
using kfxms.IService.Payment;

namespace kfxms.Web.Areas.Project.Controllers
{
    [Export]
    public class ProjectAndSupplierController : BaseController
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
        public IS_ProjectAndSupplierService ProjectAndSupplierService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("ProjectAndSupplierList");
        }

        public ActionResult Add()
        {
            return View("ProjectAndSupplierAdd");
        }

        public ActionResult Edit(Guid projectAndSupplierId)
        {
            S_ProjectAndSupplier sys_ProjectAndSupplier = ProjectAndSupplierService.GetByKey(projectAndSupplierId);
            

            if (sys_ProjectAndSupplier == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectAndSupplierEdit", sys_ProjectAndSupplier);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_ProjectAndSupplier, bool>> expre = u => true;

            //if (Request.Form["ProjectAndSupplierName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectAndSupplierName"]))
            //{
            //    string ProjectAndSupplierName = Request.Form["ProjectAndSupplierName"].Trim();
            //    expre = expre.And(u => u.ProjectAndSupplierName.Contains(ProjectAndSupplierName));
            //}
            


            ////排序
            OrderByHelper<S_ProjectAndSupplier, int?> orderBy = new OrderByHelper<S_ProjectAndSupplier, int?>() { OrderByType = OrderByType.DESC, Expression = u => u.Num };

            int total = 0;

            List<S_ProjectAndSupplier> list = ProjectAndSupplierService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();
            //List<S_Project> ListProject = projectService.GetAllData().ToList();
            //List<S_Supplier> ListSupplier = supplierService.GetAllData().ToList();
            List<S_ProjectAndSupplierDetail> listProjectAndSupplierDetail = new List<S_ProjectAndSupplierDetail>();
            //List<S_Client> listClient = clientService.GetAllData().ToList();

            foreach (S_ProjectAndSupplier item in list)
            {
                S_ProjectAndSupplierDetail s_ProjectAndSupplierDetail = new S_ProjectAndSupplierDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num ==(item.ProjectNum)).ToList();
                List<S_Supplier> ListSupplier = supplierService.GetWhereData(u => u.Num ==(item.SupplierNum)).ToList();
                s_ProjectAndSupplierDetail.Id = item.Id;
                s_ProjectAndSupplierDetail.ProjectNum = item.ProjectNum;
                s_ProjectAndSupplierDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectAndSupplierDetail.SupplierNum = item.SupplierNum;
                s_ProjectAndSupplierDetail.SupplierName = ListSupplier[0].SupplierName;
                s_ProjectAndSupplierDetail.SupplierScore = item.SupplierScore;
                s_ProjectAndSupplierDetail.ScoreRemark = item.ScoreRemark;
                s_ProjectAndSupplierDetail.SupplierContractAmout = item.SupplierContractAmout;
                List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == item.ProjectNum && u.ExternalPaymentSupplier == item.SupplierNum).ToList();
                decimal? SupplierCurrentPaymentAmout = 0;
                if (listExternalPayment.Count > 0)
                {
                    foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                    {
                        SupplierCurrentPaymentAmout += ExternalPayment.ExternalPaymentAmout;
                    }
                }
                decimal? SupplierLeftPaymentAmout = s_ProjectAndSupplierDetail.SupplierContractAmout - SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierCurrentPaymentAmout = SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierLeftPaymentAmout = SupplierLeftPaymentAmout;


                listProjectAndSupplierDetail.Add(s_ProjectAndSupplierDetail);
            }

            if (Request.Form["projectName"] != null && !string.IsNullOrEmpty(Request.Form["projectName"]))
            {
                listProjectAndSupplierDetail = listProjectAndSupplierDetail.Where(u => u.ProjectName.Contains((Request.Form["projectName"]))).ToList();
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
                var result = from p in listProjectAndSupplierDetail where userPojectList.Contains(p.ProjectNum) select p;

                //ht.Add("total", total);
                ht.Add("data", result);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

                return Content(json);
            }
            else
            {
                //ht.Add("total", total);
                ht.Add("data", listProjectAndSupplierDetail);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            }


            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectAndSupplier eProjectAndSupplier = new S_ProjectAndSupplier();
            List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectAndSupplier);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult GetAllDataByProjectNum(int ProjectNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectAndSupplier eProjectAndSupplier = new S_ProjectAndSupplier();
            List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();
            //listProjectAndSupplier = listProjectAndSupplier.Where(u => u.ProjectNum == ProjectNum).ToList();
            List<S_ProjectAndSupplierDetail> listProjectAndSupplierDetail = new List<S_ProjectAndSupplierDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData();

            foreach (S_ProjectAndSupplier item in listProjectAndSupplier)
            {
                S_ProjectAndSupplierDetail s_ProjectAndSupplierDetail = new S_ProjectAndSupplierDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                List<S_Supplier> ListSupplier = supplierService.GetWhereData(u => u.Num == (item.SupplierNum)).ToList();
                s_ProjectAndSupplierDetail.Id = item.Id;
                s_ProjectAndSupplierDetail.ProjectNum = item.ProjectNum;
                s_ProjectAndSupplierDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectAndSupplierDetail.SupplierNum = item.SupplierNum;
                s_ProjectAndSupplierDetail.SupplierName = ListSupplier[0].SupplierName;
                s_ProjectAndSupplierDetail.SupplierScore = item.SupplierScore;
                s_ProjectAndSupplierDetail.ScoreRemark = item.ScoreRemark;
                s_ProjectAndSupplierDetail.SupplierContractAmout = item.SupplierContractAmout;
                List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == item.ProjectNum && u.ExternalPaymentSupplier == item.SupplierNum).ToList();
                decimal? SupplierCurrentPaymentAmout = 0;
                if (listExternalPayment.Count > 0)
                {
                    foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                    {
                        SupplierCurrentPaymentAmout += ExternalPayment.ExternalPaymentAmout;
                    }
                }
                decimal? SupplierLeftPaymentAmout = s_ProjectAndSupplierDetail.SupplierContractAmout - SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierCurrentPaymentAmout = SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierLeftPaymentAmout = SupplierLeftPaymentAmout;

                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectAndSupplierDetail.SupplierNum)
                //    {
                //        s_ProjectAndSupplierDetail.SupplierScore = score.SupplierScore;
                //    }
                //}


                listProjectAndSupplierDetail.Add(s_ProjectAndSupplierDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectAndSupplierDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult GetAllDataBySupplierNum(int SupplierNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectAndSupplier eProjectAndSupplier = new S_ProjectAndSupplier();
            List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetAllData().ToList();
            listProjectAndSupplier = listProjectAndSupplier.Where(u => u.SupplierNum == SupplierNum).ToList();
            List<S_ProjectAndSupplierDetail> listProjectAndSupplierDetail = new List<S_ProjectAndSupplierDetail>();
            //List<S_SupplierScore> listSupplierScore = SupplierScoreService.GetAllData().Where(u => u.ProjectNum == ProjectNum).ToList();

            foreach (S_ProjectAndSupplier item in listProjectAndSupplier)
            {
                S_ProjectAndSupplierDetail s_ProjectAndSupplierDetail = new S_ProjectAndSupplierDetail();
                List<S_Project> ListProject = projectService.GetWhereData(u => u.Num == (item.ProjectNum)).ToList();
                List<S_Supplier> ListSupplier = supplierService.GetWhereData(u => u.Num == (item.SupplierNum)).ToList();
                s_ProjectAndSupplierDetail.Id = item.Id;
                s_ProjectAndSupplierDetail.ProjectNum = item.ProjectNum;
                s_ProjectAndSupplierDetail.ProjectName = ListProject[0].ProjectName;
                s_ProjectAndSupplierDetail.SupplierNum = item.SupplierNum;
                s_ProjectAndSupplierDetail.SupplierName = ListSupplier[0].SupplierName;
                s_ProjectAndSupplierDetail.SupplierScore = item.SupplierScore;
                s_ProjectAndSupplierDetail.ScoreRemark = item.ScoreRemark;
                s_ProjectAndSupplierDetail.SupplierContractAmout = item.SupplierContractAmout;
                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectAndSupplierDetail.SupplierNum)
                //    {
                //        s_ProjectAndSupplierDetail.SupplierScore = score.SupplierScore;
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
                decimal? SupplierLeftPaymentAmout = s_ProjectAndSupplierDetail.SupplierContractAmout - SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierCurrentPaymentAmout = SupplierCurrentPaymentAmout;
                s_ProjectAndSupplierDetail.SupplierLeftPaymentAmout = SupplierLeftPaymentAmout;

                //foreach (S_SupplierScore score in listSupplierScore)
                //{
                //    if(score.SupplierNum== s_ProjectAndSupplierDetail.SupplierNum)
                //    {
                //        s_ProjectAndSupplierDetail.SupplierScore = score.SupplierScore;
                //    }
                //}


                listProjectAndSupplierDetail.Add(s_ProjectAndSupplierDetail);
            }

            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listProjectAndSupplierDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddProjectAndSupplier(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectAndSupplier eProjectAndSupplier = new S_ProjectAndSupplier();
            eProjectAndSupplier.Id = Guid.NewGuid();
            eProjectAndSupplier.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            eProjectAndSupplier.SupplierNum= int.Parse(row["Supplier"].ToString().Trim());
            eProjectAndSupplier.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            eProjectAndSupplier.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            eProjectAndSupplier.SupplierContractAmout= Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            eProjectAndSupplier.AddTime = DateTime.Now;
            eProjectAndSupplier.AddUserId = base.LoginUser.Id;
            eProjectAndSupplier.AddName = base.LoginUser.Name;
            eProjectAndSupplier.LastEditName = base.LoginUser.Name;
            eProjectAndSupplier.LastEditTime = DateTime.Now;
            eProjectAndSupplier.LastEditUserID = base.LoginUser.Id;

            //eProjectAndSupplier.ProjectAndSupplierName = row["ProjectAndSupplierName"].ToString().Trim();
            //eProjectAndSupplier.Password = MD5Helper.GetMD5("123456");
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectAndSupplier.ClientId = int.Parse(row["Client"].ToString());
            //eProjectAndSupplier.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectAndSupplier.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectAndSupplier.Status = 1;
            //eProjectAndSupplier.Remark = row["Remark"].ToString().Trim();

            //List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetWhereData(u => u.ProjectId.Equals(eProjectAndSupplier.ProjectId)).ToList();
            List<S_ProjectAndSupplier> listSupplierScore = ProjectAndSupplierService.GetWhereData(u => u.ProjectNum == eProjectAndSupplier.ProjectNum && u.SupplierNum == eProjectAndSupplier.SupplierNum).ToList();
            if (listSupplierScore.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该供应商关系已经存在！");
                return Content(resultJson);
            }


            int num = ProjectAndSupplierService.Add(eProjectAndSupplier);
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

        public ActionResult EditProjectAndSupplier(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ProjectAndSupplier eProjectAndSupplier = ProjectAndSupplierService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eProjectAndSupplier == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String ProjectAndSupplierName = row["ProjectAndSupplierName"].ToString().Trim();
            //eProjectAndSupplier.ProjectAndSupplierName = row["ProjectAndSupplierName"].ToString().Trim();\

            eProjectAndSupplier.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            eProjectAndSupplier.SupplierNum = int.Parse(row["Supplier"].ToString().Trim());
            eProjectAndSupplier.SupplierScore = int.Parse(row["SupplierScore"].ToString().Trim());
            eProjectAndSupplier.ScoreRemark = row["ScoreRemark"].ToString().Trim();
            eProjectAndSupplier.SupplierContractAmout = Convert.ToDecimal(row["SupplierContractAmout"].ToString().Trim());
            //eProjectAndSupplier.ProjectAndSupplierName = ProjectAndSupplierName;
            //string[] arrClient = row["Client"].ToString().Split(':');
            //eSupplier.Type = int.Parse(arrType[0]);
            //eProjectAndSupplier.ClientId = int.Parse(arrClient[0]);
            //eProjectAndSupplier.ClientId = int.Parse(row["Client"].ToString().Trim());
            //eProjectAndSupplier.ContractAmout = Convert.ToDecimal(row["ContractAmout"].ToString().Trim());
            //eProjectAndSupplier.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            //eProjectAndSupplier.Status = int.Parse(row["Status"].ToString().Trim());
            //eProjectAndSupplier.Remark = row["Remark"].ToString().Trim();

            int num = ProjectAndSupplierService.Update(eProjectAndSupplier);
            eProjectAndSupplier.LastEditName = base.LoginUser.Name;
            eProjectAndSupplier.LastEditTime = DateTime.Now;
            eProjectAndSupplier.LastEditUserID = base.LoginUser.Id;
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

        public ActionResult DeleteProjectAndSupplier(Guid ProjectAndSupplierId)
        {
            
            S_ProjectAndSupplier eProjectAndSupplier = ProjectAndSupplierService.GetByKey(ProjectAndSupplierId);
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == eProjectAndSupplier.ProjectNum && u.ExternalPaymentSupplier == eProjectAndSupplier.SupplierNum).ToList();



            string resultJson = "";
            if (listExternalPayment.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框不关闭窗体, "该供应商已有付款信息，请删除付款信息后再删除该供应商关系！");
                return Content(resultJson);
            }
            int num = ProjectAndSupplierService.Delete(ProjectAndSupplierId);
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

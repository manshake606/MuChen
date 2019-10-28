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
using kfxms.Entity.Payment;
using kfxms.IService.Payment;
using kfxms.Entity.Project;
using kfxms.IService.Project;
using kfxms.Entity.Client;
using kfxms.IService.Client;
using kfxms.Entity.Supplier;
using kfxms.IService.Supplier;

namespace kfxms.Web.Areas.Payment.Controllers
{
    [Export]
    public class ExternalPaymentController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_SupplierService supplierService { get; set; }

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
            return View("ExternalPaymentList");
        }

        public ActionResult Add()
        {
            return View("ExternalPaymentAdd");
        }

        public ActionResult Edit(Guid ExternalPaymentId)
        {
            S_ExternalPayment sys_ExternalPayment = ExternalPaymentService.GetByKey(ExternalPaymentId);
            

            if (sys_ExternalPayment == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ExternalPaymentEdit", sys_ExternalPayment);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_ExternalPayment, bool>> expre = u => true;

            /*
            if (Request.Form["ProjectNum"] != null && !string.IsNullOrEmpty(Request.Form["ProjectNum"]))
            {
                int ProjectNum = int.Parse(Request.Form["ProjectNum"].Trim());
                expre = expre.And(u => u.ProjectNum.Equals(ProjectNum));
            }
            */
            
            


            ////排序
            OrderByHelper<S_ExternalPayment, DateTime> orderBy = new OrderByHelper<S_ExternalPayment, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_ExternalPayment> list = ExternalPaymentService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_Project> listProject = projectService.GetAllData().ToList();
            List<S_Supplier> ListSupplier = supplierService.GetAllData().ToList();
            List<S_ExternalPaymentDetail> listExternalPaymentDetail = new List<S_ExternalPaymentDetail>();

            
            foreach (S_ExternalPayment item in list)
            {
                var s_ExternalPaymentDetail = new S_ExternalPaymentDetail();
                s_ExternalPaymentDetail.Id = item.Id;
                s_ExternalPaymentDetail.Num = item.Num;
                s_ExternalPaymentDetail.ProjectNum = item.ProjectNum;
                s_ExternalPaymentDetail.ExternalPaymentAmout = item.ExternalPaymentAmout;
                s_ExternalPaymentDetail.ExternalPaymentSupplier = item.ExternalPaymentSupplier;
                s_ExternalPaymentDetail.ExternalPaymentContent = item.ExternalPaymentContent;
                s_ExternalPaymentDetail.ExternalPaymentTime = item.ExternalPaymentTime;

                foreach (S_Project projectItem in listProject)
                {
                    if (s_ExternalPaymentDetail.ProjectNum == projectItem.Num)
                    {
                        s_ExternalPaymentDetail.ProjectName = projectItem.ProjectName;
                    }
                }

                foreach (S_Supplier supplierItem in ListSupplier)
                {
                    if (s_ExternalPaymentDetail.ExternalPaymentSupplier == supplierItem.Num)
                    {
                        s_ExternalPaymentDetail.ExternalPaymentSupplierName = supplierItem.SupplierName;
                    }
                }
                


                listExternalPaymentDetail.Add(s_ExternalPaymentDetail);
            }

            Expression<Func<S_ExternalPaymentDetail, bool>> exp = u => true;

            if (Request.Form["ProjectName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectName"]))
            {
                string ProjectName = Request.Form["ProjectName"].Trim();
                //exp = exp.And(u => u.ExternalPaymentUserName.Equals(ProjectName));

                listExternalPaymentDetail = listExternalPaymentDetail.Where(u => u.ProjectName.Contains(ProjectName)).ToList();
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
            ht.Add("data", listExternalPaymentDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ExternalPayment eExternalPayment = new S_ExternalPayment();
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listExternalPayment);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddExternalPayment(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ExternalPayment eExternalPayment = new S_ExternalPayment();
            eExternalPayment.Id = Guid.NewGuid();
            eExternalPayment.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            //eExternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim()); 
            eExternalPayment.ExternalPaymentAmout = Convert.ToDecimal(row["ExternalPaymentAmout"].ToString().Trim());
            eExternalPayment.ExternalPaymentContent = row["ExternalPaymentContent"].ToString().Trim();
            eExternalPayment.ExternalPaymentTime = Convert.ToDateTime(row["ExternalPaymentTime"].ToString().Trim());
            eExternalPayment.ExternalPaymentSupplier= int.Parse(row["ExternalPaymentSupplier"].ToString().Trim());
            eExternalPayment.AddTime = DateTime.Now;
            eExternalPayment.AddUserId = base.LoginUser.Id;
            eExternalPayment.AddName = base.LoginUser.Name;
            eExternalPayment.LastEditName = base.LoginUser.Name;
            eExternalPayment.LastEditTime = DateTime.Now;
            eExternalPayment.LastEditUserID = base.LoginUser.Id;
            List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetAllData().Where(u => u.ProjectNum == eExternalPayment.ProjectNum && u.SupplierNum == eExternalPayment.ExternalPaymentSupplier).ToList();
            decimal? SupplierContractAmout = 0;
            if (listProjectAndSupplier.Count > 0)
            {
                SupplierContractAmout = listProjectAndSupplier[0].SupplierContractAmout;
            }
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == eExternalPayment.ProjectNum && u.ExternalPaymentSupplier == eExternalPayment.ExternalPaymentSupplier).ToList();
            decimal? SupplierCurrentPaymentAmout = 0;
            if (listExternalPayment.Count > 0)
            {
                foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                {
                    SupplierCurrentPaymentAmout += ExternalPayment.ExternalPaymentAmout;
                }
            }
            decimal? SupplierLeftPaymentAmout = SupplierContractAmout - SupplierCurrentPaymentAmout;
            if (eExternalPayment.ExternalPaymentAmout > SupplierLeftPaymentAmout)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "新增付款额大于剩余付款额:" + SupplierLeftPaymentAmout.ToString() + "，请检查付款金额！");
                return Content(resultJson);
            }

            //eExternalPayment.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            //eExternalPayment.PRContact = row["PRContact"].ToString().Trim();
            //eExternalPayment.PRContactPhone= row["PRContactPhone"].ToString().Trim();
            //eExternalPayment.PRContent= row["PRContent"].ToString().Trim();
            //eExternalPayment.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            //eExternalPayment.PRTime= Convert.ToDateTime(row["PRTime"].ToString().Trim());
            //eExternalPayment.ExternalPaymentName = row["ExternalPaymentName"].ToString().Trim();
            //eExternalPayment.Password = MD5Helper.GetMD5("123456");
            //eExternalPayment.ProjectNum
            //eRole.AddUserId = base.LoginUser.Id;
            //eRole.AddName = base.LoginUser.Name;
            /*
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetWhereData(u => u.ExternalPaymentName.Equals(eExternalPayment.ExternalPaymentName)).ToList();
            if (listExternalPayment.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该甲方已经存在！");
               return Content(resultJson);
            }
            */
            int num = ExternalPaymentService.Add(eExternalPayment);
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

        public ActionResult EditExternalPayment(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_ExternalPayment eExternalPayment = ExternalPaymentService.GetByKey(Guid.Parse(row["Id"].ToString()));
            decimal? existingSupplierContractAmout = eExternalPayment.ExternalPaymentAmout;
            if (eExternalPayment == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }

            eExternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            //eExternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim()); 
            
            eExternalPayment.ExternalPaymentContent = row["ExternalPaymentContent"].ToString().Trim();
            eExternalPayment.ExternalPaymentTime = Convert.ToDateTime(row["ExternalPaymentTime"].ToString().Trim());
            eExternalPayment.ExternalPaymentSupplier = int.Parse(row["ExternalPaymentSupplier"].ToString().Trim());
            eExternalPayment.LastEditName = base.LoginUser.Name;
            eExternalPayment.LastEditTime = DateTime.Now;
            eExternalPayment.LastEditUserID = base.LoginUser.Id;
            List<S_ProjectAndSupplier> listProjectAndSupplier = ProjectAndSupplierService.GetAllData().Where(u => u.ProjectNum == eExternalPayment.ProjectNum && u.SupplierNum == eExternalPayment.ExternalPaymentSupplier).ToList();
            decimal? SupplierContractAmout = 0;
            if (listProjectAndSupplier.Count > 0)
            {
                SupplierContractAmout = listProjectAndSupplier[0].SupplierContractAmout;
            }
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetAllData().Where(u => u.ProjectNum == eExternalPayment.ProjectNum && u.ExternalPaymentSupplier == eExternalPayment.ExternalPaymentSupplier).ToList();
            decimal? SupplierCurrentPaymentAmout = 0;
            if (listExternalPayment.Count > 0)
            {
                foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                {
                    SupplierCurrentPaymentAmout += ExternalPayment.ExternalPaymentAmout;
                }
            }
            decimal? SupplierLeftPaymentAmout = SupplierContractAmout - SupplierCurrentPaymentAmout+ existingSupplierContractAmout;

            if (Convert.ToDecimal(row["ExternalPaymentAmout"].ToString().Trim()) > SupplierLeftPaymentAmout)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "更新付款额大于剩余付款额:"+SupplierLeftPaymentAmout.ToString()+"，请检查付款金额！");
                return Content(resultJson);
            }

            eExternalPayment.ExternalPaymentAmout = Convert.ToDecimal(row["ExternalPaymentAmout"].ToString().Trim());
            //String ExternalPaymentName = row["ExternalPaymentName"].ToString().Trim();

            //eExternalPayment.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            //eExternalPayment.PRContact = row["PRContact"].ToString().Trim();
            //eExternalPayment.PRContactPhone = row["PRContactPhone"].ToString().Trim();
            //eExternalPayment.PRContent = row["PRContent"].ToString().Trim();
            //eExternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            //eExternalPayment.PRTime = Convert.ToDateTime(row["PRTime"].ToString().Trim());
            //eExternalPayment.ExternalPaymentName = row["ExternalPaymentName"].ToString().Trim();
            /*
             eExternalPayment.ExternalPaymentName = ExternalPaymentName;
             eExternalPayment.SocialSecurityNum = row["SocialSecurityNum"].ToString().Trim();
             eExternalPayment.ExternalPaymentAddress = row["ExternalPaymentAddress"].ToString().Trim();
             eExternalPayment.TelephoneNum = row["TelephoneNum"].ToString().Trim();
             eExternalPayment.Bank = row["Bank"].ToString().Trim();
             eExternalPayment.BankAccount = row["BankAccount"].ToString().Trim();
             eExternalPayment.ExternalPaymentContact = row["ExternalPaymentContact"].ToString().Trim();
             eExternalPayment.ExternalPaymentContactMobile = row["ExternalPaymentContactMobile"].ToString().Trim();
             eExternalPayment.ExternalPaymentContactPosition = row["ExternalPaymentContactPosition"].ToString().Trim();
             eExternalPayment.Remark = row["Remark"].ToString().Trim();
             */

            int num = ExternalPaymentService.Update(eExternalPayment);
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

        public ActionResult DeleteExternalPayment(Guid ExternalPaymentId)
        {
            int num = ExternalPaymentService.Delete(ExternalPaymentId);
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

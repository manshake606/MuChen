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

namespace kfxms.Web.Areas.Payment.Controllers
{
    [Export]
    public class InternalPaymentController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_InternalPaymentService InternalPaymentService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("InternalPaymentList");
        }

        public ActionResult Add()
        {
            return View("InternalPaymentAdd");
        }

        public ActionResult Edit(Guid InternalPaymentId)
        {
            S_InternalPayment sys_InternalPayment = InternalPaymentService.GetByKey(InternalPaymentId);
            

            if (sys_InternalPayment == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("InternalPaymentEdit", sys_InternalPayment);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_InternalPayment, bool>> expre = u => true;

            /*
            if (Request.Form["ProjectNum"] != null && !string.IsNullOrEmpty(Request.Form["ProjectNum"]))
            {
                int ProjectNum = int.Parse(Request.Form["ProjectNum"].Trim());
                expre = expre.And(u => u.ProjectNum.Equals(ProjectNum));
            }
            */
            
            


            ////排序
            OrderByHelper<S_InternalPayment, DateTime> orderBy = new OrderByHelper<S_InternalPayment, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_InternalPayment> list = InternalPaymentService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_Project> listProject = projectService.GetAllData().ToList();
            List<Sys_User> ListUser = userService.GetAllData().ToList();
            List<S_InternalPaymentDetail> listInternalPaymentDetail = new List<S_InternalPaymentDetail>();

            
            foreach (S_InternalPayment item in list)
            {
                var s_InternalPaymentDetail = new S_InternalPaymentDetail();
                s_InternalPaymentDetail.Id = item.Id;
                s_InternalPaymentDetail.Num = item.Num;
                s_InternalPaymentDetail.ProjectNum = item.ProjectNum;
                s_InternalPaymentDetail.InternalPaymentAmout = item.InternalPaymentAmout;
                s_InternalPaymentDetail.InternalPaymentUser = item.InternalPaymentUser;
                s_InternalPaymentDetail.InternalPaymentContent = item.InternalPaymentContent;
                s_InternalPaymentDetail.InternalPaymentTime = item.InternalPaymentTime;

                foreach (S_Project projectItem in listProject)
                {
                    if (s_InternalPaymentDetail.ProjectNum == projectItem.Num)
                    {
                        s_InternalPaymentDetail.ProjectName = projectItem.ProjectName;
                    }
                }

                foreach (Sys_User userItem in ListUser)
                {
                    if (s_InternalPaymentDetail.InternalPaymentUser == userItem.Num)
                    {
                        s_InternalPaymentDetail.InternalPaymentUserName = userItem.UserName;
                    }
                }
                


                listInternalPaymentDetail.Add(s_InternalPaymentDetail);
            }

            Expression<Func<S_InternalPaymentDetail, bool>> exp = u => true;

            if (Request.Form["ProjectName"] != null && !string.IsNullOrEmpty(Request.Form["ProjectName"]))
            {
                string ProjectName = Request.Form["ProjectName"].Trim();
                //exp = exp.And(u => u.InternalPaymentUserName.Equals(ProjectName));

                listInternalPaymentDetail = listInternalPaymentDetail.Where(u => u.ProjectName.Contains(ProjectName)).ToList();
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
            ht.Add("data", listInternalPaymentDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_InternalPayment eInternalPayment = new S_InternalPayment();
            List<S_InternalPayment> listInternalPayment = InternalPaymentService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listInternalPayment);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddInternalPayment(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_InternalPayment eInternalPayment = new S_InternalPayment();
            eInternalPayment.Id = Guid.NewGuid();
            eInternalPayment.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            //eInternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim()); 
            eInternalPayment.InternalPaymentAmout = Convert.ToDecimal(row["InternalPaymentAmout"].ToString().Trim());
            eInternalPayment.InternalPaymentContent = row["InternalPaymentContent"].ToString().Trim();
            eInternalPayment.InternalPaymentTime = Convert.ToDateTime(row["InternalPaymentTime"].ToString().Trim());
            eInternalPayment.InternalPaymentUser= int.Parse(row["InternalPaymentUser"].ToString().Trim());
            eInternalPayment.AddTime = DateTime.Now;
            eInternalPayment.AddUserId = base.LoginUser.Id;
            eInternalPayment.AddName = base.LoginUser.Name;
            eInternalPayment.LastEditName = base.LoginUser.Name;
            eInternalPayment.LastEditTime = DateTime.Now;
            eInternalPayment.LastEditUserID = base.LoginUser.Id;
            //eInternalPayment.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            //eInternalPayment.PRContact = row["PRContact"].ToString().Trim();
            //eInternalPayment.PRContactPhone= row["PRContactPhone"].ToString().Trim();
            //eInternalPayment.PRContent= row["PRContent"].ToString().Trim();
            //eInternalPayment.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            //eInternalPayment.PRTime= Convert.ToDateTime(row["PRTime"].ToString().Trim());
            //eInternalPayment.InternalPaymentName = row["InternalPaymentName"].ToString().Trim();
            //eInternalPayment.Password = MD5Helper.GetMD5("123456");
            //eInternalPayment.ProjectNum
            //eRole.AddUserId = base.LoginUser.Id;
            //eRole.AddName = base.LoginUser.Name;
            /*
            List<S_InternalPayment> listInternalPayment = InternalPaymentService.GetWhereData(u => u.InternalPaymentName.Equals(eInternalPayment.InternalPaymentName)).ToList();
            if (listInternalPayment.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该甲方已经存在！");
               return Content(resultJson);
            }
            */
            int num = InternalPaymentService.Add(eInternalPayment);
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

        public ActionResult EditInternalPayment(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_InternalPayment eInternalPayment = InternalPaymentService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (eInternalPayment == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }

            eInternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            //eInternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim()); 
            eInternalPayment.InternalPaymentAmout = Convert.ToDecimal(row["InternalPaymentAmout"].ToString().Trim());
            eInternalPayment.InternalPaymentContent = row["InternalPaymentContent"].ToString().Trim();
            eInternalPayment.InternalPaymentTime = Convert.ToDateTime(row["InternalPaymentTime"].ToString().Trim());
            eInternalPayment.InternalPaymentUser = int.Parse(row["InternalPaymentUser"].ToString().Trim());
            eInternalPayment.LastEditName = base.LoginUser.Name;
            eInternalPayment.LastEditTime = DateTime.Now;
            eInternalPayment.LastEditUserID = base.LoginUser.Id;
            //String InternalPaymentName = row["InternalPaymentName"].ToString().Trim();

            //eInternalPayment.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            //eInternalPayment.PRContact = row["PRContact"].ToString().Trim();
            //eInternalPayment.PRContactPhone = row["PRContactPhone"].ToString().Trim();
            //eInternalPayment.PRContent = row["PRContent"].ToString().Trim();
            //eInternalPayment.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            //eInternalPayment.PRTime = Convert.ToDateTime(row["PRTime"].ToString().Trim());
            //eInternalPayment.InternalPaymentName = row["InternalPaymentName"].ToString().Trim();
            /*
             eInternalPayment.InternalPaymentName = InternalPaymentName;
             eInternalPayment.SocialSecurityNum = row["SocialSecurityNum"].ToString().Trim();
             eInternalPayment.InternalPaymentAddress = row["InternalPaymentAddress"].ToString().Trim();
             eInternalPayment.TelephoneNum = row["TelephoneNum"].ToString().Trim();
             eInternalPayment.Bank = row["Bank"].ToString().Trim();
             eInternalPayment.BankAccount = row["BankAccount"].ToString().Trim();
             eInternalPayment.InternalPaymentContact = row["InternalPaymentContact"].ToString().Trim();
             eInternalPayment.InternalPaymentContactMobile = row["InternalPaymentContactMobile"].ToString().Trim();
             eInternalPayment.InternalPaymentContactPosition = row["InternalPaymentContactPosition"].ToString().Trim();
             eInternalPayment.Remark = row["Remark"].ToString().Trim();
             */

            int num = InternalPaymentService.Update(eInternalPayment);
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

        public ActionResult DeleteInternalPayment(Guid InternalPaymentId)
        {
            int num = InternalPaymentService.Delete(InternalPaymentId);
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

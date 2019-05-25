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
    public class PublicRelationsController : BaseController
    {

        [Import]
        public new ISys_UserService userService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_ProjectService projectService { get; set; }

        [Import]
        //public new  userService { get; set; }
        public IS_PublicRelationsService publicRelationsService { get; set; }

        [Import]
        public ISys_UserAndRoleService userAndRoleService { get; set; }
        [Import]
        public ISys_RoleService roleService { get; set; }

        public ActionResult List()
        {
            return View("PublicRelationsList");
        }

        public ActionResult Add()
        {
            return View("PublicRelationsAdd");
        }

        public ActionResult Edit(Guid PublicRelationsId)
        {
            S_PublicRelations sys_PublicRelations = publicRelationsService.GetByKey(PublicRelationsId);
            

            if (sys_PublicRelations == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("PublicRelationsEdit", sys_PublicRelations);

        }

        public ActionResult GetPageData(int pageSize, int pageIndex)
        {

            //条件
            Expression<Func<S_PublicRelations, bool>> expre = u => true;

            
            if (Request.Form["PRContact"] != null && !string.IsNullOrEmpty(Request.Form["PRContact"]))
            {
                string PRContact = Request.Form["PRContact"].Trim();
                expre = expre.And(u => u.PRContact.Contains(PRContact));
            }
            


            ////排序
            OrderByHelper<S_PublicRelations, DateTime> orderBy = new OrderByHelper<S_PublicRelations, DateTime>() { OrderByType = OrderByType.DESC, Expression = u => u.AddTime.Value };

            int total = 0;

            List<S_PublicRelations> list = publicRelationsService.GetPageDate(expre, pageIndex, pageSize, out total, orderBy).ToList();
            List<S_Project> listProject = projectService.GetAllData().ToList();
            List<S_PublicRelationsDetail> listPublicRelationsDetail = new List<S_PublicRelationsDetail>();


            foreach (S_PublicRelations item in list)
            {
                var s_PublicRelationsDetail = new S_PublicRelationsDetail();
                s_PublicRelationsDetail.Id = item.Id;
                s_PublicRelationsDetail.Num = item.Num;
                s_PublicRelationsDetail.ProjectNum = item.ProjectNum;
                s_PublicRelationsDetail.PRAmout = item.PRAmout;
                s_PublicRelationsDetail.PRContact = item.PRContact;
                s_PublicRelationsDetail.PRContactPhone = item.PRContactPhone;
                s_PublicRelationsDetail.PRContent = item.PRContent;
                s_PublicRelationsDetail.PRTime = item.PRTime;

                foreach (S_Project projectItem in listProject)
                {
                    if (s_PublicRelationsDetail.ProjectNum == projectItem.Num)
                    {
                        s_PublicRelationsDetail.ProjectName = projectItem.ProjectName;
                    }
                }
                listPublicRelationsDetail.Add(s_PublicRelationsDetail);
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
            ht.Add("data", listPublicRelationsDetail);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_PublicRelations ePublicRelations = new S_PublicRelations();
            List<S_PublicRelations> listPublicRelations = publicRelationsService.GetAllData().ToList();
            Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            ht.Add("data", listPublicRelations);
            string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return Content(json);
        }

        public ActionResult AddPublicRelations(string data)
        {
            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_PublicRelations ePublicRelations = new S_PublicRelations();
            ePublicRelations.Id = Guid.NewGuid();
            ePublicRelations.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            ePublicRelations.PRContact = row["PRContact"].ToString().Trim();
            ePublicRelations.PRContactPhone= row["PRContactPhone"].ToString().Trim();
            ePublicRelations.PRContent= row["PRContent"].ToString().Trim();
            ePublicRelations.ProjectNum= int.Parse(row["Project"].ToString().Trim());
            ePublicRelations.PRTime= Convert.ToDateTime(row["PRTime"].ToString().Trim());
            ePublicRelations.AddTime = DateTime.Now;
            ePublicRelations.AddUserId = base.LoginUser.Id;
            ePublicRelations.AddName = base.LoginUser.Name;
            ePublicRelations.LastEditName = base.LoginUser.Name;
            ePublicRelations.LastEditTime = DateTime.Now;
            ePublicRelations.LastEditUserID = base.LoginUser.Id;
            //ePublicRelations.PublicRelationsName = row["PublicRelationsName"].ToString().Trim();
            //ePublicRelations.Password = MD5Helper.GetMD5("123456");
            //ePublicRelations.ProjectNum
            /*
            List<S_PublicRelations> listPublicRelations = publicRelationsService.GetWhereData(u => u.PublicRelationsName.Equals(ePublicRelations.PublicRelationsName)).ToList();
            if (listPublicRelations.Count > 0)
            {
               resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该甲方已经存在！");
               return Content(resultJson);
            }
            */
            int num = publicRelationsService.Add(ePublicRelations);
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

        public ActionResult EditPublicRelations(string data)
        {

            string resultJson = "";
            Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_PublicRelations ePublicRelations = publicRelationsService.GetByKey(Guid.Parse(row["Id"].ToString()));
            if (ePublicRelations == null)
            {
                resultJson = resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            //String PublicRelationsName = row["PublicRelationsName"].ToString().Trim();

            ePublicRelations.PRAmout = Convert.ToDecimal(row["PRAmout"].ToString().Trim());
            ePublicRelations.PRContact = row["PRContact"].ToString().Trim();
            ePublicRelations.PRContactPhone = row["PRContactPhone"].ToString().Trim();
            ePublicRelations.PRContent = row["PRContent"].ToString().Trim();
            ePublicRelations.ProjectNum = int.Parse(row["Project"].ToString().Trim());
            ePublicRelations.PRTime = Convert.ToDateTime(row["PRTime"].ToString().Trim());
            ePublicRelations.LastEditName = base.LoginUser.Name;
            ePublicRelations.LastEditTime = DateTime.Now;
            ePublicRelations.LastEditUserID = base.LoginUser.Id;
            //ePublicRelations.PublicRelationsName = row["PublicRelationsName"].ToString().Trim();
            /*
             ePublicRelations.PublicRelationsName = PublicRelationsName;
             ePublicRelations.SocialSecurityNum = row["SocialSecurityNum"].ToString().Trim();
             ePublicRelations.PublicRelationsAddress = row["PublicRelationsAddress"].ToString().Trim();
             ePublicRelations.TelephoneNum = row["TelephoneNum"].ToString().Trim();
             ePublicRelations.Bank = row["Bank"].ToString().Trim();
             ePublicRelations.BankAccount = row["BankAccount"].ToString().Trim();
             ePublicRelations.PublicRelationsContact = row["PublicRelationsContact"].ToString().Trim();
             ePublicRelations.PublicRelationsContactMobile = row["PublicRelationsContactMobile"].ToString().Trim();
             ePublicRelations.PublicRelationsContactPosition = row["PublicRelationsContactPosition"].ToString().Trim();
             ePublicRelations.Remark = row["Remark"].ToString().Trim();
             */

            int num = publicRelationsService.Update(ePublicRelations);
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

        public ActionResult DeletePublicRelations(Guid PublicRelationsId)
        {
            int num = publicRelationsService.Delete(PublicRelationsId);
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

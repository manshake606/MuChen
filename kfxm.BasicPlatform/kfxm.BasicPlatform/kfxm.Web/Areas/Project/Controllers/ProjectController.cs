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
using kfxms.Entity.Invoice;
using kfxms.IService.Invoice;
using kfxms.Entity.Revenue;
using kfxms.IService.Revenue;
using kfxms.Entity.Payment;
using kfxms.IService.Payment;

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
        //public new  InvoiceService { get; set; }
        public IS_InvoiceService InvoiceService { get; set; }

        [Import]
        //public new  RevenueService { get; set; }
        public IS_RevenueService RevenueService { get; set; }

        [Import]
        //public new  ExternalPaymentService { get; set; }
        public IS_ExternalPaymentService ExternalPaymentService { get; set; }

        [Import]
        //public new  InternalPaymentService { get; set; }
        public IS_InternalPaymentService InternalPaymentService { get; set; }

        [Import]
        //public new  publicRelationsService { get; set; }
        public IS_PublicRelationsService publicRelationsService { get; set; }

        [Import]
        //public new  publicRelationsService { get; set; }
        public IS_ProjectProgressService projectProgressService { get; set; }

        [Import]
        //public new  publicRelationsService { get; set; }
        public IS_ProjectContractService projectContractService { get; set; }

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

        public ActionResult Detail(Guid ProjectId)
        {
            S_Project sys_Project = projectService.GetByKey(ProjectId);
            S_ProjectInfo s_ProjectInfo = new S_ProjectInfo();
            List<S_Client> listClient = clientService.GetAllData().ToList();
            List<Sys_User> s_userList = userService.GetAllData().ToList();
            s_ProjectInfo.Id = sys_Project.Id;
            s_ProjectInfo.Num = sys_Project.Num;
            s_ProjectInfo.ProjectName = sys_Project.ProjectName;
            s_ProjectInfo.SettlementBase = sys_Project.SettlementBase;
            s_ProjectInfo.Status = sys_Project.Status;
            s_ProjectInfo.CoreDesigner = sys_Project.CoreDesigner;
            s_ProjectInfo.Area = sys_Project.Area;
            

            s_ProjectInfo.AssistantDesigner = sys_Project.AssistantDesigner;

            s_ProjectInfo.BusinessManager = sys_Project.BusinessManager;


            s_ProjectInfo.BusinessAssistant = sys_Project.BusinessAssistant;

            s_ProjectInfo.ProjectManager = sys_Project.ProjectManager;
            

            if (s_ProjectInfo.CoreDesigner != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.CoreDesigner == s_user.Num)
                    {
                        s_ProjectInfo.CoreDesignerName = s_user.Name;
                        break;
                    }
                }
            }

            s_ProjectInfo.AssistantDesigner = sys_Project.AssistantDesigner;
            if (s_ProjectInfo.AssistantDesigner != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.AssistantDesigner == s_user.Num)
                    {
                        s_ProjectInfo.AssistantDesignerName = s_user.Name;
                        break;
                    }
                }
            }


            s_ProjectInfo.BusinessManager = sys_Project.BusinessManager;
            if (s_ProjectInfo.BusinessManager != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.BusinessManager == s_user.Num)
                    {
                        s_ProjectInfo.BusinessManagerName = s_user.Name;
                        break;
                    }
                }
            }
            s_ProjectInfo.BusinessAssistant = sys_Project.BusinessAssistant;
            if (s_ProjectInfo.BusinessAssistant != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.BusinessAssistant == s_user.Num)
                    {
                        s_ProjectInfo.BusinessAssistantName = s_user.Name;
                        break;
                    }
                }
            }
            s_ProjectInfo.ProjectManager = sys_Project.ProjectManager;
            if (s_ProjectInfo.ProjectManager != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.ProjectManager == s_user.Num)
                    {
                        s_ProjectInfo.ProjectManagerName = s_user.Name;
                        break;
                    }
                }
            }


            if (s_ProjectInfo.Status == 1)
            {
                s_ProjectInfo.StatusName = "激活";
            }
            else if (s_ProjectInfo.Status == 0)
            {
                s_ProjectInfo.StatusName = "结束";
            }
            s_ProjectInfo.ClientId = sys_Project.ClientId;
            // Sum Contract Amount
            List<S_ProjectContract> listContract = projectContractService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal? ContractAmout = 0;
            if (listContract.Count > 0)
            {
                foreach (S_ProjectContract Contract in listContract)
                {
                    ContractAmout += Contract.ProjectContractAmount;
                }
            }
            s_ProjectInfo.ContractAmout = ContractAmout;

            if (s_ProjectInfo.ContractAmout > 0 && s_ProjectInfo.Area > 0)
            {
                s_ProjectInfo.UnitPrice = s_ProjectInfo.ContractAmout / s_ProjectInfo.Area;
            }
            else
            {
                s_ProjectInfo.UnitPrice = null;
            }
            s_ProjectInfo.Remark = sys_Project.Remark;
            foreach (S_Client clientItem in listClient)
            {
                if (s_ProjectInfo.ClientId == clientItem.Num)
                {
                    s_ProjectInfo.ClientKey = clientItem.Id;
                    s_ProjectInfo.TelephoneNum = clientItem.TelephoneNum;
                    s_ProjectInfo.ClientName = clientItem.ClientName;
                    s_ProjectInfo.ClientAddress = clientItem.ClientAddress;
                    s_ProjectInfo.ClientContact = clientItem.ClientContact;
                    s_ProjectInfo.ClientContactMobile = clientItem.ClientContactMobile;
                    s_ProjectInfo.ClientContactPosition = clientItem.ClientContactPosition;
                }
            }

            //Sum Invoice
            //List<S_Invoice> listInvoice = InvoiceService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            //decimal SumInvoice = 0;
            //if (listInvoice.Count > 0)
            //{
            //    foreach (S_Invoice Invoice in listInvoice)
            //    {
            //        SumInvoice += Invoice.InvoiceAmout;
            //    }
            //}

            //Sum Revenue
            //List<S_Revenue> listRevenue = RevenueService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            //decimal SumRevenue = 0;
            //if (listRevenue.Count > 0)
            //{
            //    foreach (S_Revenue Revenue in listRevenue)
            //    {
            //        SumRevenue += Revenue.RevenueAmout;
            //    }
            //}

            //Sum ExternalPayment
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumExternalPayment = 0;
            if (listExternalPayment.Count > 0)
            {
                foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                {
                    SumExternalPayment += ExternalPayment.ExternalPaymentAmout;
                }
            }

            //Sum InternalPayment
            List<S_InternalPayment> listInternalPayment = InternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumInternalPayment = 0;
            if (listInternalPayment.Count > 0)
            {
                foreach (S_InternalPayment InternalPayment in listInternalPayment)
                {
                    SumInternalPayment += InternalPayment.InternalPaymentAmout;
                }
            }

            //Sum publicRelations
            List<S_PublicRelations> listPublicRelations = publicRelationsService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumPublicRelations = 0;
            if (listPublicRelations.Count > 0)
            {
                foreach (S_PublicRelations PublicRelations in listPublicRelations)
                {
                    SumPublicRelations += PublicRelations.PRAmout;
                }
            }

            

            decimal SumPayment = SumExternalPayment + SumInternalPayment + SumPublicRelations;


            //s_ProjectInfo.SumInvoice = SumInvoice;
            //s_ProjectInfo.SumRevenue = SumRevenue;
            s_ProjectInfo.SumInternalPayment = SumInternalPayment;
            s_ProjectInfo.SumExternalPayment = SumExternalPayment;
            s_ProjectInfo.SumPublicRelations = SumPublicRelations;
            s_ProjectInfo.SumPayment = SumPayment;
            //if (sys_Project.ContractAmout > 0)
            //{
            //    s_ProjectInfo.RevenueRate = (SumRevenue / sys_Project.ContractAmout * 100).ToString("0.00") + "%";
            //}
            //else
            //{
            //    s_ProjectInfo.RevenueRate= "0.00" +"%";
            //}
            


            if (sys_Project == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            return View("ProjectDetail", s_ProjectInfo);

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

            if (Request.Form["projectStatus"] != null && !string.IsNullOrEmpty(Request.Form["projectStatus"]))
            {
                int ProjectStatus = int.Parse(Request.Form["projectStatus"].Trim());
                expre = expre.And(u => u.Status== ProjectStatus);
            }

            //expre = expre.And(u => u.Status == 1);

            ////排序
            OrderByHelper<S_Project, int?> orderBy = new OrderByHelper<S_Project, int?>() { OrderByType = OrderByType.DESC, Expression = u => u.Num };

            int total = 0;

            List<S_Project> list = projectService.GetPageData(expre, pageIndex, pageSize, out total, orderBy).ToList();

            List<S_Client> listClient = clientService.GetAllData().ToList();

            List<S_ProjectInfo> listProjectInfo = new List<S_ProjectInfo>();

            foreach (S_Project item in list)
            {
                S_ProjectInfo s_ProjectInfo = new S_ProjectInfo();
                s_ProjectInfo.Id = item.Id;
                s_ProjectInfo.AddTime = item.AddTime;
                s_ProjectInfo.Num = item.Num;
                s_ProjectInfo.ProjectName = item.ProjectName;
                s_ProjectInfo.SettlementBase = item.SettlementBase;
                s_ProjectInfo.Status = item.Status;
                s_ProjectInfo.Area = item.Area;

                if (s_ProjectInfo.Status == 1)
                {
                    s_ProjectInfo.StatusName = "激活";
                }else if(s_ProjectInfo.Status == 0)
                {
                    s_ProjectInfo.StatusName = "结束";
                }
                s_ProjectInfo.ClientId = item.ClientId;
                s_ProjectInfo.Remark = item.Remark;

                //Sum Contract Amount
                List<S_ProjectContract> listContract = projectContractService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
                decimal? ContractAmout = 0;
                if (listContract.Count > 0)
                {
                    foreach (S_ProjectContract Contract in listContract)
                    {
                        ContractAmout += Contract.ProjectContractAmount;
                    }
                }
                s_ProjectInfo.ContractAmout = ContractAmout;
                if (s_ProjectInfo.ContractAmout > 0 && s_ProjectInfo.Area>0)
                {
                    s_ProjectInfo.UnitPrice = s_ProjectInfo.ContractAmout / s_ProjectInfo.Area;
                }
                else
                {
                    s_ProjectInfo.UnitPrice = null;
                }
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

                //Sum Invoice
                //List<S_Invoice> listInvoice = InvoiceService.GetWhereData(u => u.ProjectNum==s_ProjectInfo.Num).ToList();
                //decimal SumInvoice = 0;
                //if (listInvoice.Count > 0)
                //{
                //    foreach(S_Invoice Invoice in listInvoice)
                //    {
                //        SumInvoice += Invoice.InvoiceAmout;
                //    }
                //}

                //Sum Revenue
                //List<S_Revenue> listRevenue = RevenueService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
                //decimal SumRevenue = 0;
                //if (listRevenue.Count > 0)
                //{
                //    foreach (S_Revenue Revenue in listRevenue)
                //    {
                //        SumRevenue += Revenue.RevenueAmout;
                //    }
                //}

                //Sum ExternalPayment
                List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
                decimal SumExternalPayment = 0;
                if (listExternalPayment.Count > 0)
                {
                    foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                    {
                        SumExternalPayment += ExternalPayment.ExternalPaymentAmout;
                    }
                }

                //Sum InternalPayment
                List<S_InternalPayment> listInternalPayment = InternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
                decimal SumInternalPayment = 0;
                if (listInternalPayment.Count > 0)
                {
                    foreach (S_InternalPayment InternalPayment in listInternalPayment)
                    {
                        SumInternalPayment += InternalPayment.InternalPaymentAmout;
                    }
                }

                //Sum publicRelations
                List<S_PublicRelations> listPublicRelations = publicRelationsService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
                decimal SumPublicRelations = 0;
                if (listPublicRelations.Count > 0)
                {
                    foreach (S_PublicRelations PublicRelations in listPublicRelations)
                    {
                        SumPublicRelations += PublicRelations.PRAmout;
                    }
                }

                decimal SumPayment = SumExternalPayment + SumInternalPayment + SumPublicRelations;


                //s_ProjectInfo.SumInvoice = SumInvoice;
                //s_ProjectInfo.SumRevenue = SumRevenue;
                s_ProjectInfo.SumInternalPayment = SumInternalPayment;
                s_ProjectInfo.SumExternalPayment = SumExternalPayment;
                s_ProjectInfo.SumPublicRelations = SumPublicRelations;
                s_ProjectInfo.SumPayment = SumPayment;
                //if (item.ContractAmout > 0)
                //{
                //    s_ProjectInfo.RevenueRate = (SumRevenue / item.ContractAmout * 100).ToString("0.00") + "%";
                //}
                //else
                //{
                //    s_ProjectInfo.RevenueRate ="0.00" + "%";
                //}
                

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
                var result = from p in listProjectInfo where userPojectList.Contains(p.Num) select p;

                //ht.Add("total", total);
                ht.Add("data", result);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

                return Content(json);
            }
            else
            {
                //ht.Add("total", total);
                ht.Add("data", listProjectInfo);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            }


            //Hashtable ht = new Hashtable();
            //ht.Add("total", total);
            //ht.Add("data", listProjectInfo);
            //string json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

            return Content(json);

        }

        public ActionResult GetAllProjectDataByProjectNum(int ProjectNum)
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Project eProject = new S_Project();
            List<S_Project> listProject = projectService.GetAllData().Where(u => u.Num == ProjectNum).ToList();
            List<S_ProjectInfo> listProjectInfo = new List<S_ProjectInfo>();
            S_Project sys_Project = listProject[0];
            S_ProjectInfo s_ProjectInfo = new S_ProjectInfo();
            List<S_Client> listClient = clientService.GetAllData().ToList();
            List<Sys_User> s_userList = userService.GetAllData().ToList();
            s_ProjectInfo.Id = sys_Project.Id;
            s_ProjectInfo.Num = sys_Project.Num;
            s_ProjectInfo.ProjectName = sys_Project.ProjectName;
            s_ProjectInfo.SettlementBase = sys_Project.SettlementBase;
            s_ProjectInfo.Status = sys_Project.Status;
            s_ProjectInfo.CoreDesigner = sys_Project.CoreDesigner;
            //Sum Contract Amount
            List<S_ProjectContract> listContract = projectContractService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal? ContractAmout = 0;
            if (listContract.Count > 0)
            {
                foreach (S_ProjectContract Contract in listContract)
                {
                    ContractAmout += Contract.ProjectContractAmount;
                }
            }
            s_ProjectInfo.ContractAmout = ContractAmout;

            if (s_ProjectInfo.CoreDesigner != null) 
            {
                foreach(Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.CoreDesigner == s_user.Num)
                    {
                        s_ProjectInfo.CoreDesignerName = s_user.Name;
                        break;
                    }
                }
            }

            s_ProjectInfo.AssistantDesigner = sys_Project.AssistantDesigner;
            if (s_ProjectInfo.AssistantDesigner != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.AssistantDesigner == s_user.Num)
                    {
                        s_ProjectInfo.AssistantDesignerName = s_user.Name;
                        break;
                    }
                }
            }


            s_ProjectInfo.BusinessManager = sys_Project.BusinessManager;
            if (s_ProjectInfo.BusinessManager != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.BusinessManager == s_user.Num)
                    {
                        s_ProjectInfo.BusinessManagerName = s_user.Name;
                        break;
                    }
                }
            }
            s_ProjectInfo.BusinessAssistant = sys_Project.BusinessAssistant;
            if (s_ProjectInfo.BusinessAssistant != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.BusinessAssistant == s_user.Num)
                    {
                        s_ProjectInfo.BusinessAssistantName = s_user.Name;
                        break;
                    }
                }
            }
            s_ProjectInfo.ProjectManager = sys_Project.ProjectManager;
            if (s_ProjectInfo.ProjectManager != null)
            {
                foreach (Sys_User s_user in s_userList)
                {
                    if (s_ProjectInfo.ProjectManager == s_user.Num)
                    {
                        s_ProjectInfo.ProjectManagerName = s_user.Name;
                        break;
                    }
                }
            }
            if (s_ProjectInfo.Status == 1)
            {
                s_ProjectInfo.StatusName = "激活";
            }
            else if (s_ProjectInfo.Status == 0)
            {
                s_ProjectInfo.StatusName = "结束";
            }
            s_ProjectInfo.ClientId = sys_Project.ClientId;
            s_ProjectInfo.ContractAmout = sys_Project.ContractAmout;
            s_ProjectInfo.Area = sys_Project.Area;
            if (s_ProjectInfo.ContractAmout > 0 && s_ProjectInfo.Area > 0)
            {
                s_ProjectInfo.UnitPrice = s_ProjectInfo.ContractAmout / s_ProjectInfo.Area;
            }
            else
            {
                s_ProjectInfo.UnitPrice = null;
            }
            s_ProjectInfo.Remark = sys_Project.Remark;
            foreach (S_Client clientItem in listClient)
            {
                if (s_ProjectInfo.ClientId == clientItem.Num)
                {
                    s_ProjectInfo.ClientKey = clientItem.Id;
                    s_ProjectInfo.TelephoneNum = clientItem.TelephoneNum;
                    s_ProjectInfo.ClientName = clientItem.ClientName;
                    s_ProjectInfo.ClientAddress = clientItem.ClientAddress;
                    s_ProjectInfo.ClientContact = clientItem.ClientContact;
                    s_ProjectInfo.ClientContactMobile = clientItem.ClientContactMobile;
                    s_ProjectInfo.ClientContactPosition = clientItem.ClientContactPosition;
                }
            }

            //Sum Invoice
            //List<S_Invoice> listInvoice = InvoiceService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            //decimal SumInvoice = 0;
            //if (listInvoice.Count > 0)
            //{
            //    foreach (S_Invoice Invoice in listInvoice)
            //    {
            //        SumInvoice += Invoice.InvoiceAmout;
            //    }
            //}

            //Sum Revenue
            //List<S_Revenue> listRevenue = RevenueService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            //decimal SumRevenue = 0;
            //if (listRevenue.Count > 0)
            //{
            //    foreach (S_Revenue Revenue in listRevenue)
            //    {
            //        SumRevenue += Revenue.RevenueAmout;
            //    }
            //}

            //Sum ExternalPayment
            List<S_ExternalPayment> listExternalPayment = ExternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumExternalPayment = 0;
            if (listExternalPayment.Count > 0)
            {
                foreach (S_ExternalPayment ExternalPayment in listExternalPayment)
                {
                    SumExternalPayment += ExternalPayment.ExternalPaymentAmout;
                }
            }

            //Sum InternalPayment
            List<S_InternalPayment> listInternalPayment = InternalPaymentService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumInternalPayment = 0;
            if (listInternalPayment.Count > 0)
            {
                foreach (S_InternalPayment InternalPayment in listInternalPayment)
                {
                    SumInternalPayment += InternalPayment.InternalPaymentAmout;
                }
            }

            //Sum publicRelations
            List<S_PublicRelations> listPublicRelations = publicRelationsService.GetWhereData(u => u.ProjectNum == s_ProjectInfo.Num).ToList();
            decimal SumPublicRelations = 0;
            if (listPublicRelations.Count > 0)
            {
                foreach (S_PublicRelations PublicRelations in listPublicRelations)
                {
                    SumPublicRelations += PublicRelations.PRAmout;
                }
            }

            decimal SumPayment = SumExternalPayment + SumInternalPayment + SumPublicRelations;


            //s_ProjectInfo.SumInvoice = SumInvoice;
            //s_ProjectInfo.SumRevenue = SumRevenue;
            s_ProjectInfo.SumInternalPayment = SumInternalPayment;
            s_ProjectInfo.SumExternalPayment = SumExternalPayment;
            s_ProjectInfo.SumPublicRelations = SumPublicRelations;
            s_ProjectInfo.SumPayment = SumPayment;
            //s_ProjectInfo.RevenueRate = (SumRevenue / sys_Project.ContractAmout * 100).ToString("0.00") + "%";

            listProjectInfo.Add(s_ProjectInfo);

            Hashtable ht = new Hashtable();
            string json = null;
            ht.Add("data", listProjectInfo);
            json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);


            return Content(json);
        }


        public ActionResult GetAllData()
        {
            //string resultJson = "";
            //Hashtable row = (Hashtable)JsonHelp.Decode(data);
            S_Project eProject = new S_Project();
            List<S_Project> listProject = projectService.GetAllData().ToList();


            Sys_User sUser = base.LoginUser;
            List<Sys_UserAndRole> currentUserRole = userAndRoleService.GetWhereData(u => u.UserId == sUser.Id).ToList();
            Guid? currentRoleId = currentUserRole[0].RoleId;
            List<Sys_Role> currentRoleList = roleService.GetWhereData(m => m.Id == currentRoleId).ToList();
            string roleName = currentRoleList[0].RoleName;

            Hashtable ht = new Hashtable();
            string json = null;
            if (roleName != "系统管理员" && roleName != "财务")
            {
                listProject = listProject.Where(m => m.ProjectManager == sUser.Num || m.CoreDesigner == sUser.Num || m.AssistantDesigner == sUser.Num || m.BusinessAssistant == sUser.Num || m.BusinessManager == sUser.Num).ToList();
                

                //ht.Add("total", total);
                ht.Add("data", listProject);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);

                return Content(json);
            }
            else
            {
                //ht.Add("total", total);
                ht.Add("data", listProject);
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            }

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
            eProject.CoreDesigner= int.Parse(row["CoreDesigner"].ToString());
            eProject.AssistantDesigner= int.Parse(row["AssistantDesigner"].ToString());
            eProject.BusinessManager = int.Parse(row["BusinessManager"].ToString());
            eProject.BusinessAssistant = int.Parse(row["BusinessAssistant"].ToString());
            eProject.ProjectManager = int.Parse(row["ProjectManager"].ToString());
            eProject.Area= Convert.ToDecimal(row["Area"].ToString().Trim());
            if(eProject.ContractAmout>0 && eProject.Area>0)
            {
                eProject.UnitPrice = eProject.ContractAmout / eProject.Area;
            }
            else
            {
                eProject.UnitPrice = null;
            }
            eProject.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            eProject.Status = 1;
            eProject.Remark = row["Remark"].ToString().Trim();
            eProject.AddTime = DateTime.Now;
            eProject.AddUserId = base.LoginUser.Id;
            eProject.AddName = base.LoginUser.Name;
            eProject.LastEditName = base.LoginUser.Name;
            eProject.LastEditTime = DateTime.Now;
            eProject.LastEditUserID = base.LoginUser.Id;

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
            eProject.CoreDesigner = int.Parse(row["CoreDesigner"].ToString());
            eProject.AssistantDesigner = int.Parse(row["AssistantDesigner"].ToString());
            eProject.BusinessManager = int.Parse(row["BusinessManager"].ToString());
            eProject.BusinessAssistant = int.Parse(row["BusinessAssistant"].ToString());
            eProject.ProjectManager = int.Parse(row["ProjectManager"].ToString());
            eProject.SettlementBase = Convert.ToDecimal(row["SettlementBase"].ToString().Trim());
            eProject.Area = Convert.ToDecimal(row["Area"].ToString().Trim());
            if (eProject.ContractAmout > 0 && eProject.Area > 0)
            {
                eProject.UnitPrice = eProject.ContractAmout / eProject.Area;
            }
            else
            {
                eProject.UnitPrice = null;
            }
            eProject.Status = int.Parse(row["Status"].ToString().Trim());
            eProject.Remark = row["Remark"].ToString().Trim();
            eProject.LastEditName = base.LoginUser.Name;
            eProject.LastEditTime = DateTime.Now;
            eProject.LastEditUserID = base.LoginUser.Id;

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

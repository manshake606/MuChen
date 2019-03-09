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

namespace kfxms.Web.Areas.SysBasic.Controllers
{

    [Export]
    public class DepartmentController : BaseController
    {

        [Import]
        public ISys_DepartmentService departmentService { get; set; }

        #region 页面

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View("DepartmentList");
        }


        /// <summary>
        /// 新增页面
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public ActionResult Add(string parentDptID)
        {
            ViewData["ParentDptID"] = parentDptID;
            return View("DepartmentAdd");
        }


        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid ID)
        {
            Sys_Department oDpt = departmentService.GetByKey(ID);
            string parentName = "";
            if (oDpt == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            if (oDpt.ParentDptID.HasValue)
            {
                parentName = departmentService.GetByKey(oDpt.ParentDptID.Value).DptName;
                ViewData["ParentDptID"] = oDpt.ParentDptID.ToString();
            }
            else
            {
                ViewData["ParentDptID"] = "";
            }
            ViewData["ParentDptName"] = parentName;
            return View("DepartmentEdit", oDpt);
        }

        #endregion


        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData()
        {
            string resultJson = "";
            Expression<Func<Sys_Department, bool>> expre = (m => m.State != -1);
            OrderByHelper<Sys_Department, int> Order = new OrderByHelper<Sys_Department, int>() { OrderByType = OrderByType.ASC, Expression = m => m.OrderIndex.Value };
            var dictionaryList = departmentService.GetWhereData(expre, Order).Select(m => new { id = m.DptID, pid = m.ParentDptID, text = m.DptName });
            resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, dictionaryList);
            return Content(resultJson);
        }


        /// <summary>
        /// 获取某ID下属子单位列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData()
        {
            string resultJson = "";
            Expression<Func<Sys_Department, bool>> expre = (m => m.State != -1);
            if (Request.Form["ParentDptID"] != null && !string.IsNullOrEmpty(Request.Form["ParentDptID"].Trim()))
            {
                Guid parentID = Guid.Parse(Request.Form["ParentDptID"].Trim());
                expre = expre.And(m => m.ParentDptID == parentID);
            }
            else
            {
                expre = expre.And(m => m.ParentDptID == null);
            }
            OrderByHelper<Sys_Department, int> Order = new OrderByHelper<Sys_Department, int>() { OrderByType = OrderByType.ASC, Expression = m => m.OrderIndex.Value };

            List<Sys_Department> dictionaryList = departmentService.GetWhereData(expre, Order).ToList();
            resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, dictionaryList);
            return Content(resultJson);
        }


        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        public ActionResult AddDepartment(string data)
        {
            string resultJson = "";
            Hashtable ht = (Hashtable)JsonHelp.Decode(data);
            Sys_Department oDpt = new Sys_Department();
            oDpt.DptID = Guid.NewGuid();

            oDpt.DptName = ht["txt_DptName"].ToString().Trim();
            oDpt.State = 0;
            int OrderIndex;
            if (int.TryParse(ht["txt_OrderIndex"].ToString(), out OrderIndex))
            {
                oDpt.OrderIndex = OrderIndex;
            }
            Guid ParentID;
            if (Guid.TryParse(ht["txt_ParentDptID"].ToString(), out ParentID))
            {
                oDpt.ParentDptID = ParentID;
            }
            Expression<Func<Sys_Department, bool>> expre = (m => m.DptName == oDpt.DptName);
            expre = expre.Or(m => m.OrderIndex == oDpt.OrderIndex);
            Expression<Func<Sys_Department, bool>> where = (m => m.ParentDptID == null);
            if (oDpt.ParentDptID.HasValue)
            {
                where = (m => m.ParentDptID == oDpt.ParentDptID);
            }
            where = where.And(expre);
            List<Sys_Department> existList = departmentService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "名称或排序值重复！");
                return Content(resultJson);
            }
            oDpt.AddTime = DateTime.Now;
            oDpt.AddName = base.LoginUser.Name;
            oDpt.AddUserID = base.LoginUser.Id;
            if (departmentService.Add(oDpt) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "添加成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "添加失败");
            }
            return Content(resultJson);
        }

        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        public ActionResult EditDepartment(string data)
        {
            string resultJson = "";
            Hashtable ht = (Hashtable)JsonHelp.Decode(data);
            Sys_Department oDpt = departmentService.GetByKey(Guid.Parse(ht["txt_DptID"].ToString()));
            if (oDpt == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            oDpt.DptName = ht["txt_DptName"].ToString().Trim();
            oDpt.State = 0;
            int OrderIndex;
            if (int.TryParse(ht["txt_OrderIndex"].ToString(), out OrderIndex))
            {
                oDpt.OrderIndex = OrderIndex;
            }
            Expression<Func<Sys_Department, bool>> expre = (m => m.DptName == oDpt.DptName);
            expre = expre.Or(m => m.OrderIndex == oDpt.OrderIndex);
            Expression<Func<Sys_Department, bool>> where = (m => m.ParentDptID == null);
            if (oDpt.ParentDptID.HasValue)
            {
                where = (m => m.ParentDptID == oDpt.ParentDptID);
            }
            where = where.And(m => m.DptID != oDpt.DptID);
            where = where.And(expre);
            List<Sys_Department> existList = departmentService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "名称或排序值重复！");
                return Content(resultJson);
            }
            oDpt.LastEditTime = DateTime.Now;
            oDpt.LastEditUserID = base.LoginUser.Id;
            if (departmentService.Update(oDpt) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "修改成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "修改失败");
            }
            return Content(resultJson);
        }



        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="data">部门ID</param>
        /// <returns></returns>
        public ActionResult DeleteDepartment(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Department oDpt = departmentService.GetByKey(ID);
            if (oDpt == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            Expression<Func<Sys_Department, bool>> where = (m => m.ParentDptID == oDpt.DptID);
            where = where.And(m => m.State != -1);
            List<Sys_Department> existList = departmentService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该记录下含有下级记录，不能删除！");
                return Content(resultJson);
            }
            oDpt.State = -1;
            oDpt.LastEditTime = DateTime.Now;
            oDpt.LastEditUserID = base.LoginUser.Id;
            if (departmentService.Update(oDpt) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "删除成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "删除失败");
            }
            return Content(resultJson);
        }

        /// <summary>
        /// 停用部门
        /// </summary>
        /// <param name="data">部门ID</param>
        /// <returns></returns>
        public ActionResult StopDepartment(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Department oDpt = departmentService.GetByKey(ID);
            if (oDpt == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            Expression<Func<Sys_Department, bool>> where = (m => m.ParentDptID == oDpt.DptID);
            where = where.And(m => m.State == 0);
            List<Sys_Department> existList = departmentService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该记录下含有正常使用的下级记录，不能停用！");
                return Content(resultJson);
            }
            oDpt.State = 1;
            oDpt.LastEditTime = DateTime.Now;
            oDpt.LastEditUserID = base.LoginUser.Id;
            if (departmentService.Update(oDpt) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "停用成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "停用失败");
            }
            return Content(resultJson);
        }


        /// <summary>
        /// 启用部门
        /// </summary>
        /// <param name="data">部门ID</param>
        /// <returns></returns>
        public ActionResult StartDepartment(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Department oDpt = departmentService.GetByKey(ID);
            if (oDpt == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            oDpt.State = 0;
            oDpt.LastEditTime = DateTime.Now;
            oDpt.LastEditUserID = LoginUser.Id;
            if (departmentService.Update(oDpt) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "启用成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "启用失败");
            }
            return Content(resultJson);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

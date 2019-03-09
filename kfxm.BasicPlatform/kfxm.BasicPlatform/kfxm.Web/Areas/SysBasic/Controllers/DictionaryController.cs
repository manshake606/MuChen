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
    public class DictionaryController : BaseController
    {
        //
        // GET: /SysBasic/Dictionary/

        

        [Import]
        public ISys_DictionaryService dictionaryService { get; set; }


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
            return View("DictionaryList");
        }


        /// <summary>
        /// 新增页面
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public ActionResult Add(string parentID)
        {
            ViewData["ParentID"] = parentID;
            return View("DictionaryAdd");
        }


        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            Sys_Dictionary eDictionary = dictionaryService.GetByKey(id);
            string parentName = "";
            if (eDictionary == null)
            {
                Response.Write("<p style='color:red;'>该条记录不存在！</p>");
                Response.End();
            }
            if (eDictionary.ParentID.HasValue)
            {
                parentName = dictionaryService.GetByKey(eDictionary.ParentID.Value).Item;
                ViewData["ParentID"] = eDictionary.ParentID.ToString();
            }
            else
            {
                ViewData["ParentID"] = "";
            }            
            ViewData["ParentName"] = parentName;
            return View("DictionaryEdit", eDictionary);
        }

        #endregion


        /// <summary>
        /// 获取字典树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeData()
        {
            string resultJson = "";
            Expression<Func<Sys_Dictionary, bool>> expre = (m => m.State != -1);
            OrderByHelper<Sys_Dictionary, int> Order = new OrderByHelper<Sys_Dictionary, int>() { OrderByType = OrderByType.ASC, Expression = m => m.OrderIndex.Value };
            var dictionaryList = dictionaryService.GetWhereData(expre, Order).Select(m => new { id = m.ID, pid = m.ParentID, text = m.Item });
            resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, dictionaryList);
            return Content(resultJson);
        }


        /// <summary>
        /// 获取某ID下属子项目字典列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData()
        {
            string resultJson = "";
            Expression<Func<Sys_Dictionary, bool>> expre = (m => m.State != -1);
            if (Request.Form["ParentID"] != null && !string.IsNullOrEmpty(Request.Form["ParentID"].Trim()))
            {
                Guid parentID = Guid.Parse(Request.Form["ParentID"].Trim());
                expre = expre.And(m => m.ParentID == parentID);
            }
            else
            {
                expre = expre.And(m => m.ParentID == null);
            }
            OrderByHelper<Sys_Dictionary, int> Order = new OrderByHelper<Sys_Dictionary, int>() { OrderByType = OrderByType.ASC, Expression = m => m.OrderIndex.Value };

            List<Sys_Dictionary> dictionaryList = dictionaryService.GetWhereData(expre, Order).ToList();
            resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, dictionaryList);
            return Content(resultJson);
        }

        /// <summary>
        /// 新增字典项目
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        public ActionResult AddDictionary(string data)
        {
            string resultJson = "";
            Hashtable ht = (Hashtable)JsonHelp.Decode(data);
            Sys_Dictionary eDictionary = new Sys_Dictionary();
            eDictionary.ID = Guid.NewGuid();

            eDictionary.Item = ht["txt_Item"].ToString().Trim();
            eDictionary.State = 0;
            int OrderIndex;
            if (int.TryParse(ht["txt_OrderIndex"].ToString(), out OrderIndex))
            {
                eDictionary.OrderIndex = OrderIndex;
            }
            Guid ParentID;
            if (Guid.TryParse(ht["txt_ParentID"].ToString(), out ParentID))
            {
                eDictionary.ParentID = ParentID;
            }
            //Expression<Func<Sys_Dictionary, bool>> expre = (m => m.Item == eDictionary.Item);
            //expre = expre.Or(m => m.OrderIndex == eDictionary.OrderIndex);
            //Expression<Func<Sys_Dictionary, bool>> where = (m => m.ParentID == null);
            //if(eDictionary.ParentID.HasValue)
            //{
            //    where = (m => m.ParentID == eDictionary.ParentID);
            //}
            //where = where.And(expre);
            //List<Sys_Dictionary> existList = dictionaryService.GetWhereData(where).ToList();
            //if (existList != null && existList.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "名称或排序值重复！");
            //    return Content(resultJson);
            //}
            eDictionary.AddTime = DateTime.Now;
            eDictionary.AddName = base.LoginUser.Name;
            eDictionary.AddUserID = base.LoginUser.Id;
            if (dictionaryService.Add(eDictionary) > 0)
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
        /// 编辑字典项目
        /// </summary>
        /// <param name="data">表单数据</param>
        /// <returns></returns>
        public ActionResult EditDictionary(string data)
        {
            string resultJson = "";
            Hashtable ht = (Hashtable)JsonHelp.Decode(data);
            Sys_Dictionary eDictionary = dictionaryService.GetByKey(Guid.Parse(ht["txt_ID"].ToString()));
            if (eDictionary == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            eDictionary.Item = ht["txt_Item"].ToString().Trim();
            eDictionary.State = 0;
            int OrderIndex;
            if (int.TryParse(ht["txt_OrderIndex"].ToString(), out OrderIndex))
            {
                eDictionary.OrderIndex = OrderIndex;
            }
            //Expression<Func<Sys_Dictionary, bool>> expre = (m => m.Item == eDictionary.Item);
            //expre = expre.Or(m => m.OrderIndex == eDictionary.OrderIndex);
            //Expression<Func<Sys_Dictionary, bool>> where = (m => m.ParentID == null);
            //if (eDictionary.ParentID.HasValue)
            //{
            //    where = (m => m.ParentID == eDictionary.ParentID);
            //}
            //where = where.And(m => m.ID != eDictionary.ID);
            //where = where.And(expre);
            //List<Sys_Dictionary> existList = dictionaryService.GetWhereData(where).ToList();
            //if (existList != null && existList.Count > 0)
            //{
            //    resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "名称或排序值重复！");
            //    return Content(resultJson);
            //}
            eDictionary.LastEditTime = DateTime.Now;
            eDictionary.LastEditUserID = base.LoginUser.Id;
            if (dictionaryService.Update(eDictionary) > 0)
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
        /// 删除字典项目
        /// </summary>
        /// <param name="data">字典ID</param>
        /// <returns></returns>
        public ActionResult DeleteDictionary(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Dictionary eDictionary = dictionaryService.GetByKey(ID);
            if (eDictionary == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            Expression<Func<Sys_Dictionary, bool>> where = (m => m.ParentID == eDictionary.ID);
            where = where.And(m => m.State != -1);
            List<Sys_Dictionary> existList = dictionaryService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该记录下含有下级记录，不能删除！");
                return Content(resultJson);
            }
            eDictionary.State = -1;
            eDictionary.LastEditTime = DateTime.Now;
            eDictionary.LastEditUserID = base.LoginUser.Id;
            if (dictionaryService.Update(eDictionary) > 0)
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
        /// 停用字典项目
        /// </summary>
        /// <param name="data">字典ID</param>
        /// <returns></returns>
        public ActionResult StopDictionary(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Dictionary eDictionary = dictionaryService.GetByKey(ID);
            if (eDictionary == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }
            Expression<Func<Sys_Dictionary, bool>> where = (m => m.ParentID == eDictionary.ID);
            where = where.And(m => m.State == 0);
            List<Sys_Dictionary> existList = dictionaryService.GetWhereData(where).ToList();
            if (existList != null && existList.Count > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该记录下含有正常使用的下级记录，不能停用！");
                return Content(resultJson);
            }
            eDictionary.State = 1;
            eDictionary.LastEditTime = DateTime.Now;
            eDictionary.LastEditUserID = base.LoginUser.Id;
            if (dictionaryService.Update(eDictionary) > 0)
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
        /// 启用字典项目
        /// </summary>
        /// <param name="data">字典ID</param>
        /// <returns></returns>
        public ActionResult StartDictionary(string data)
        {
            string resultJson = "";
            Guid ID = Guid.Parse(data);
            Sys_Dictionary eDictionary = dictionaryService.GetByKey(ID);
            if (eDictionary == null)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "该条记录不存在！");
                return Content(resultJson);
            }            
            eDictionary.State = 0;
            eDictionary.LastEditTime = DateTime.Now;
            eDictionary.LastEditUserID = base.LoginUser.Id;
            if (dictionaryService.Update(eDictionary) > 0)
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框关闭窗体, "启用成功");
            }
            else
            {
                resultJson = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "启用失败");
            }
            return Content(resultJson);
        }


    }
}

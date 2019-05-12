using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using kfxms.IService.SysBasic;
using kfxms.Entity.SysBasic;
using kfxms.Common;
namespace kfxms.Web.Controllers
{
    [Export]
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [Import]
        public ISys_MenuService menuService { get;set;}

        [Import]
        public new ISys_UserService userService { get; set; }

        public ActionResult Index()
        {

            IList<Sys_Menu> listMenu = menuService.GetMyMenu(base.LoginUser.Id.Value);
            var menus = listMenu.Select(p => new { pid = p.ParentId, id = p.Id, text = p.MenuName, Url = p.Url });
            ViewData["menu"] = JsonHelp.Encode(menus);
            if (DataCache.GetCache("myRequest") != null)
            {
                DataCache.RemoveCahche("myRequest");
            }
            ViewBag.name = base.LoginUser.Name;

            return View();
        }


        public ActionResult Welcome() 
        {
            return View();
        }


        public ActionResult GetButton()
        {
            string href  = Request.UrlReferrer.AbsolutePath;
            if (href.IndexOf("?") > 0)
            {
                href = href.Substring(0, href.IndexOf("?"));
            }
            if (href.IndexOf("/") != 0)
            {
                href = "/" + href;
            }
            IList<Sys_AuthorityRequest> mrList = GetMyRequest();
            IList<Sys_AuthorityRequest> currentUrlRequest = mrList.Where(x => x.Url.Equals(href)).ToList();
            List<String> buttonId = new List<String>();

            foreach (Sys_AuthorityRequest req in currentUrlRequest)
            {
                if (!string.IsNullOrEmpty(req.ButtonId))
                {
                    buttonId.Add(req.ButtonId);
                }
            }
            return Content(HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源,buttonId));
        }

        public ActionResult UpdatePwd() 
        {
            string oldPwd = Request["oldPwd"];
            string newPwd = Request["newPwd"];
            string json = "";
           
            if (string.IsNullOrEmpty(oldPwd))
            {
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "请输入原密码");
            }
            else if (string.IsNullOrEmpty(newPwd))
            {
                json = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "请输入密码"); 
            }
            else
            {
                Framework.Security.LxdRSACryption rsaCryption = new Framework.Security.LxdRSACryption(Framework.Security.RSAConfig.privateKey, Framework.Security.RSAConfig.publicKey);
                oldPwd = rsaCryption.Decrypt(oldPwd);
                newPwd = rsaCryption.Decrypt(newPwd);
                if (!base.LoginUser.Password.Equals(MD5Helper.GetMD5(oldPwd)))
                {
                    json = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "原密码不正确");
                }
                else
                {
                    base.LoginUser.Password = MD5Helper.GetMD5(newPwd);
                   
                    if (userService.Update(base.LoginUser) > 0)
                    {
                        json = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出OK提示框不关闭窗体, "修改成功");
                    }
                    else
                    {
                        json = HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "修改失败");
                    }
                }
            }
           return Content(json);
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult TestAdd() 
        {
            return View();
        } 

        public String GetPageData(int pageIndex,int pageSize)
        {
            int rowNum = 46;
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("c1");
            dt.Columns.Add("c2");
            dt.Columns.Add("c3");
            dt.Columns.Add("c4");

            Thread.Sleep(1000);//进程睡眠1秒，有正在处理的效果
          
            int pageStart = (pageIndex - 1) * pageSize + 1;
            int pageEnd = pageIndex * pageSize;
            for (int i = 1; i <= rowNum; i++)
            {
                if (i >= pageStart && i <= pageEnd)
                {
                    DataRow row = dt.NewRow();
                    row["Id"] = i + 1;
                    row["c1"] = "c1" + (i + 1);
                    row["c2"] = "c2" + (i + 1);
                    row["c3"] = "c3" + (i + 1);
                    row["c4"] = null;
                    dt.Rows.Add(row);
                }
            }


            Hashtable ht = new Hashtable();
            ht.Add("total", 46);
            ht.Add("data", dt);
            String str = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, ht);
            return str;
        
        }

        public String GetTreeData() 
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("pid");
            dt.Columns.Add("text");

            DataRow row = dt.NewRow();
            row["id"] = "1";
            row["pid"] = "0";
            row["text"] = "一级菜单";
            dt.Rows.Add(row);

            DataRow row1 = dt.NewRow();
            row1["id"] = "2";
            row1["pid"] = "1";
            row1["text"] = "二级菜单";
            dt.Rows.Add(row1);

            DataRow row2= dt.NewRow();
            row2["id"] = "3";
            row2["pid"] = "2";
            row2["text"] = "三级菜单";
            dt.Rows.Add(row2);

            String str = HbesAjaxHelper.AjaxResult(HbesAjaxType.执行数据源, dt);
            return str;
        }
    }
}

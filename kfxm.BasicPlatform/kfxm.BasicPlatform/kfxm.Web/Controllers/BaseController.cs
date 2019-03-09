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
using System.Text;

namespace kfxms.Web
{
    [Export]
    public class BaseController : Controller
    {
       
        [Import]
        public ISys_UserService userService{get;set;}

        [Import]
        public ISys_AuthorityRequestService requestService { get; set; }
        

        public Sys_User LoginUser
        {
            get
            {
                if (Session["hbes_login_user"] != null)
                {
                    return (Sys_User)Session["hbes_login_user"];
                }
                else
                {
                    return GetLoginUserByCookie();
                }
            }
            private set { Session["hbes_login_user"] = value; }
        }

        

        private Sys_User GetLoginUserByCookie() 
        {
            Framework.Security.LxdRSACryption rsaCryption = new Framework.Security.LxdRSACryption(Framework.Security.RSAConfig.privateKey, Framework.Security.RSAConfig.publicKey);
            Sys_User user = null;
            bool isLogin = true;
            try
            {
                if (Request.Cookies["hbes_login_username"] == null)
                {
                    isLogin = false;
                }
                else if (string.IsNullOrEmpty(Request.Cookies["hbes_login_username"].Value))
                {
                    isLogin = false;
                }
                else if (string.IsNullOrEmpty(rsaCryption.Decrypt(Request.Cookies["hbes_login_username"].Value)))
                {
                    isLogin = false;
                }
                else 
                {
                    string userName = rsaCryption.Decrypt(Request.Cookies["hbes_login_username"].Value);
                    user = userService.GetWhereData(u => u.UserName.Equals(userName)).First();
                    if (user != null) 
                    {
                        isLogin = true;
                        this.LoginUser = user;
                    }
                    else
                    {
                        isLogin = false;
                    }
                }
            }
            catch
            {
               isLogin = false;
            }
            if (!isLogin)
            {
                if (!IsHandler)
                {
                    Response.Write("<script>alert('登录状态丢失，请您重新登录！');window.opener=null;window.top.location='/Login';</script>");
                    
                }
                else
                {
                    Response.Write(HbesAjaxHelper.AjaxResult(HbesAjaxType.登录过期返回登录页面, "登录状态丢失，请您重新登录！"));
                }
                Response.End();
            }
            return user;
        }

        private bool IsHandler = false;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (Request["IsHandler"] != null)
            {
                IsHandler = true;
            }
            if (IsHandler)
            {
                LoadHandler(filterContext); 
            }
            else 
            {
                LoadPage(filterContext);
            } 
            base.OnActionExecuting(filterContext);
        }


        private void LoadPage(ActionExecutingContext filterContext)
        {
            bool hasRole = true;
            try
            {
                if (this.LoginUser == null)
                {
                    GetLoginUserByCookie();
                }

                string appPath = Request.ApplicationPath;
                string href = Request.Url.AbsolutePath;
                if (appPath.Length > 1 && href.IndexOf(appPath) == 0)
                {
                    href = href.Substring(appPath.Length);
                }
                if (href.IndexOf("?") > 0)
                {
                    href = href.Substring(0, href.IndexOf("?"));
                }
                if (href.IndexOf("/") != 0)
                {
                    href = "/" + href;
                }

                if (Request.UrlReferrer != null)
                {
                    Uri uriRef = Request.UrlReferrer;
                    Uri uriCur = Request.Url;
                    if (uriRef.Host != uriCur.Host || uriRef.Port != uriCur.Port)
                    {
                        Response.Write("<span style='color:red;'>非法访问 </span>");
                        Response.End();
                    }
                }
                else
                {
                    if (filterContext.ActionDescriptor.ActionName.Equals("Index") && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home"))
                    {

                    }
                    else
                    {
                        Response.Write("<span style='color:red;'>请不要直接链接请求页面 </span>");
                        Response.End();
                    }
                }

                IList<Sys_AuthorityRequest> mrList = GetMyRequest();
                IList<Sys_AuthorityRequest> currentUrlRequest= mrList.Where(x => x.Url.Equals(href)).ToList();
                if (currentUrlRequest == null || currentUrlRequest.Count == 0)
                {
                    hasRole = false;
                }
                else 
                {
                    List<String> buttonId = new List<String>();
                   
                    foreach (Sys_AuthorityRequest req in currentUrlRequest) 
                    {
                        if (!string.IsNullOrEmpty(req.ButtonId)) 
                        {
                            buttonId.Add(req.ButtonId);
                        }
                    }
                    ViewData["ButtonId"] = JsonHelp.Encode(buttonId);
                }
                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home"))
                {
                    hasRole = true;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:red;'>您访问的页面出错！:" + ex.Message + " </span>");
                Response.End();
            }
            if (!hasRole)
            {

                Response.Write("<span style='color:red;'>您访问的页面不存在或者您无权限访问该页面！</span>");
                Response.End();
            }

        }


        private void LoadHandler(ActionExecutingContext filterContext) 
        {
            bool hasRole = true;
            try
            {
                if (this.LoginUser == null)
                {
                    GetLoginUserByCookie();
                }

                string appPath = Request.ApplicationPath;
                string href = Request.Url.AbsolutePath;
                if (appPath.Length > 1 && href.IndexOf(appPath) == 0)
                {
                    href = href.Substring(appPath.Length);
                }
                if (href.IndexOf("?") > 0)
                {
                    href = href.Substring(0, href.IndexOf("?"));
                }
                if (href.IndexOf("/") != 0)
                {
                    href = "/" + href;
                }

                if (Request.UrlReferrer != null)
                {
                    Uri uriRef = Request.UrlReferrer;
                    Uri uriCur = Request.Url;
                    if (uriRef.Host != uriCur.Host || uriRef.Port != uriCur.Port)
                    {
                        Response.Write(HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出错误提示框不关闭窗体, "非法访问"));
                        Response.End();
                    }
                }
                else
                {
                    if (filterContext.ActionDescriptor.ActionName.Equals("Index") && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home"))
                    {

                    }
                    else
                    {
                        Response.Write("<span style='color:red;'>请不要直接链接请求页面 </span>");
                        Response.End();
                    }
                }

                IList<Sys_AuthorityRequest> mrList = GetMyRequest();
                IList<Sys_AuthorityRequest> currentUrlRequest = mrList.Where(x => x.Url.Equals(href)).ToList();
                if (currentUrlRequest == null || currentUrlRequest.Count == 0)
                {
                    hasRole = false;
                }
                else
                {
                    List<String> buttonId = new List<String>();

                    foreach (Sys_AuthorityRequest req in currentUrlRequest)
                    {
                        if (!string.IsNullOrEmpty(req.ButtonId))
                        {
                            buttonId.Add(req.ButtonId);
                        }
                    }
                    ViewData["ButtonId"] = JsonHelp.Encode(buttonId);
                }
                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Home"))
                {
                    hasRole = true;
                }
            }
            catch
            {

              
            }
            if (!hasRole)
            {

                Response.Write(HbesAjaxHelper.AjaxResult(HbesAjaxType.弹出警告提示框不关闭窗体, "无操作权限！"));
                Response.End();
            }

        }


        protected IList<Sys_AuthorityRequest> GetMyRequest() 
        {
            if (DataCache.GetCache("myRequest") != null)
            {
                return (IList<Sys_AuthorityRequest>)DataCache.GetCache("myRequest");
            }
            else 
            {
                IList<Sys_AuthorityRequest> list = requestService.GetRequestByUserId(this.LoginUser.Id.Value);
                DataCache.SetCache("myRequest", list);
                return list;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {

            StringBuilder exceptionMsg = new StringBuilder();
            Exception exception = filterContext.Exception;
            exceptionMsg.AppendLine("");
            exceptionMsg.AppendLine("");
            exceptionMsg.AppendLine("时间："+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            exceptionMsg.AppendLine("异常信息：" + exception.Message + exception.ToString());
            exceptionMsg.AppendLine("异常堆栈：" + exception.StackTrace.ToString());
            exceptionMsg.AppendLine("");
            exceptionMsg.AppendLine("-----------------------------------------------------------------------");
            WriteLog.WriteErrorLog(exceptionMsg.ToString());
            if (!IsHandler)
            {
                filterContext.HttpContext.Response.Write("<span style='color:red;'>" + filterContext.Exception.Message + " </span>");
                filterContext.HttpContext.Response.End();
            }
            else
            {
                filterContext.HttpContext.Response.Write(HbesAjaxHelper.AjaxResult(HbesAjaxType.异常, filterContext.Exception.Message));
            }
           filterContext.HttpContext.Response.End();
           base.OnException(filterContext);
        }

        
    }
}
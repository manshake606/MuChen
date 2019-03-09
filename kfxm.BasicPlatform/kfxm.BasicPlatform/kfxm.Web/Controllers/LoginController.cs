using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kfxms.IService.SysBasic;
using kfxms.Entity.SysBasic;
using kfxms.Common;
namespace kfxms.Web.Controllers
{
    [Export]
    public class LoginController : Controller
    {

        [Import]
        public ISys_UserService userService { get; set; }

        public ActionResult Index()
        {
            if (Session["hbes_login_user"] != null)
            {
                Session["hbes_login_user"] = null;
            }

            if (Request.Cookies["hbes_login_username"] != null)
            {
                HttpCookie cookie = Request.Cookies["hbes_login_username"];
                cookie.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(cookie);

            }
            return View("../Login");
        }

        public ActionResult CheckCode()
        {
            string checkCode = GetRandomCode(4);
            int iwidth = (int)(checkCode.Length * 14);
            Bitmap image = new Bitmap(iwidth, 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Font f = new System.Drawing.Font("Arial ", 10);//, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(Color.Black);
                Brush r = new System.Drawing.SolidBrush(Color.FromArgb(166, 8, 8));

                g.Clear(System.Drawing.ColorTranslator.FromHtml("#99C1CB"));//背景色

                char[] ch = checkCode.ToCharArray();
                for (int i = 0; i < ch.Length; i++)
                {
                    if (ch[i] >= '0' && ch[i] <= '9')
                    {
                        //数字用红色显示
                        g.DrawString(ch[i].ToString(), f, r, 3 + (i * 12), 3);
                    }
                    else
                    {   //字母用黑色显示
                        g.DrawString(ch[i].ToString(), f, b, 3 + (i * 12), 3);
                    }
                }

                MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                Response.Cookies.Add(new HttpCookie("checkCode", checkCode.ToUpper()));

                return File(ms.ToArray(), "image/jpeg");
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        private string GetRandomCode(int CodeCount)
        {
            string allChar = "0,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(32);

                while (temp == t)
                {
                    t = rand.Next(32);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }

        public ActionResult Login()
        {
            string userName = Request["userName"];
            string password = Request["password"];
            string checkCode = Request["checkCode"];
            HttpCookie checkCookie = Request.Cookies["checkCode"];
            ResultMsg result = new ResultMsg();

            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    result.IsSuccess = false;
                    result.Msg = "请输入用户名！";
                }
                else if (string.IsNullOrEmpty(password))
                {
                    result.IsSuccess = false;
                    result.Msg = "请输入密码！";
                }
                else if (string.IsNullOrEmpty(checkCode))
                {
                    result.IsSuccess = false;
                    result.Msg = "请输入验证码！";
                }
                else if (checkCookie == null)
                {
                    result.IsSuccess = false;
                    result.Msg = "验证码过期！";
                }
                else
                {
                    Framework.Security.LxdRSACryption rsaCryption = new Framework.Security.LxdRSACryption(Framework.Security.RSAConfig.privateKey, Framework.Security.RSAConfig.publicKey);
                    if (checkCookie.Value.ToUpper().Equals(checkCode.ToUpper()))
                    {
                        userName = rsaCryption.Decrypt(userName);
                        password = rsaCryption.Decrypt(password);

                        IList<Sys_User> userList = userService.GetWhereData(u => u.UserName.Equals(userName)).ToList();
                        if (userList == null || userList.Count == 0)
                        {
                            result.IsSuccess = false;
                            result.Msg = "用户或密码不正确！";
                        }
                        else if (userList.Count > 1)
                        {
                            result.IsSuccess = false;
                            result.Msg = "用户或密码不正确！";
                        }
                        else
                        {
                            Sys_User user = userList[0];
                            password = kfxms.Common.MD5Helper.GetMD5(password);
                            if (user.Password.Equals(password))
                            {
                                result.IsSuccess = true;
                                result.Msg = "登录成功";

                                 string hbes_logininfo_key = Guid.NewGuid().ToString().Replace("-", "");

                                HttpCookie cookie = new HttpCookie("hbes_login_username", rsaCryption.Encrypt(userName));
                                int expiresHours = 2;
                                cookie.Expires = DateTime.Now.AddHours(expiresHours);
                                Response.Cookies.Add(cookie);

                                  DataCache.SetCache(hbes_logininfo_key, user, DateTime.Now.AddHours(expiresHours), System.Web.Caching.Cache.NoSlidingExpiration);
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Msg = "用户或密码不正确！";
                            }
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Msg = "验证码不正确！";
                    }
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Msg = ex.Message;
            }
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            return Content(json);
        }
    }
    public class ResultMsg
    {
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
    }
}

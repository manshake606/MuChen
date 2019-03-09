using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kfxms.Web
{
    /// <summary>
    ///PublicEnum 的摘要说明
    /// </summary>
    //Ajax返回类型
    public enum HbesAjaxType
    {
        执行数据源 = 1,
        弹出OK提示框不关闭窗体 = 2,
        弹出OK提示框关闭窗体 = 3,
        弹出警告提示框不关闭窗体 = 4,
        弹出警告提示框关闭窗体 = 5,
        弹出错误提示框不关闭窗体 = 6,
        弹出错误提示框关闭窗体 = 7,
        登录过期返回登录页面 = 8,
        异常 = 9
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace kfxms.Web
{
    /// <summary>
    /// 公用结果类
    /// </summary>
    public class PublicResultData
    {

        public PublicResultData()
        {
        }

        public PublicResultData(HbesAjaxType resultTyle, string resultData)
        {
            this.ResultType = resultTyle;
            this.ResultData = resultData;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public HbesAjaxType ResultType { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public string ResultData { get; set; }

    }

    public class HbesAjaxHelper
    {
        public static string AjaxResult(HbesAjaxType resultTyle, object obj)
        {
            PublicResultData prd = new PublicResultData(resultTyle, JsonHelp.Encode(obj));
            return JsonHelp.Encode(prd);
        }
        /// <summary>
        /// 设置时间类型
        /// </summary>
        /// <param name="resultTyle"></param>
        /// <param name="obj">1：时间 2：日期</param>
        /// <returns></returns>
        public static string AjaxResult(HbesAjaxType resultTyle, object obj, int timeType)
        {
            PublicResultData prd = new PublicResultData(resultTyle, JsonHelp.Encode(obj, timeType));
            return JsonHelp.Encode(prd);
        }
    }
}
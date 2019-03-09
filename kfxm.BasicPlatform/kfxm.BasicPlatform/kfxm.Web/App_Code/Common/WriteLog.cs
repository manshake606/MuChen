using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace kfxms.Web
{
    public class WriteLog
    {

        /// <summary>
        /// 写操作日志文件
        /// </summary>
        /// <param name="input">日志内容</param>
        /// <returns>返回值true成功,false失败</returns>
        public static bool WriteErrorLog(string input)
        {
            bool rBool = true;
            try
            {
                DirectoryInfo di = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/Log"));
                if (!di.Exists)
                {
                    di.Create();
                }
                string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Log/" + DateTime.Now.ToString("yyyy-MM-dd") + "-Log.txt");
                FileInfo logFile = new FileInfo(filePath);
                if (logFile.Exists)
                {
                    if (logFile.Length >= 800000)
                    {
                        logFile.CopyTo(filePath.Replace("Log.txt", RndNum(9) + "Log.txt"));
                        File.Delete(filePath);
                    }
                }

                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                StreamWriter w = new StreamWriter(fs, Encoding.UTF8);
                w.BaseStream.Seek(0, SeekOrigin.End);
                w.Write(input + "\r\n\r\n");
                w.Flush();
                w.Close();
                w.Dispose();
                fs.Close();
                fs.Dispose();
            }
            catch (Exception ex)
            {
                rBool = false;
            }
            return rBool;
        }


        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="VcodeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int VcodeNum)
        {
            StringBuilder sb = new StringBuilder(VcodeNum);
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }

    }
}
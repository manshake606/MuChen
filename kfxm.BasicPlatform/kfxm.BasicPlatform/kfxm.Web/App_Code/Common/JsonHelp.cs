﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Data;
namespace kfxms.Web
{
    public class JsonHelp
    {
        public static string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        public static string DateTimeFormatDay = "yyyy'-'MM'-'dd";

        /// <summary>
        /// 对象 解析为 JSON字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Encode(object o)
        {
            if (o == null || o.ToString() == "null") return null;

            if (o != null && (o.GetType() == typeof(String) || o.GetType() == typeof(string)))
            {
                return o.ToString();
            }
            IsoDateTimeConverter dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = DateTimeFormat;
            return JsonConvert.SerializeObject(o, dt);
        }

        /// <summary>
        /// 对象 解析为 JSON字符串 （加个 时间格式）
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type">1：时间 2：日期</param>
        /// <returns></returns>
        public static string Encode(object o, int type)
        {
            if (o == null || o.ToString() == "null") return null;

            if (o != null && (o.GetType() == typeof(String) || o.GetType() == typeof(string)))
            {
                return o.ToString();
            }

            if (type == 1)
            {
                return Encode(o);
            }
            else
            {
                IsoDateTimeConverter dt = new IsoDateTimeConverter();
                dt.DateTimeFormat = DateTimeFormatDay;
                return JsonConvert.SerializeObject(o, dt);
            }


        }

        /// <summary>
        /// json字符串转对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Decode(string json)
        {
            if (String.IsNullOrEmpty(json)) return "";
            object o = JsonConvert.DeserializeObject(json);
            if (o.GetType() == typeof(String) || o.GetType() == typeof(string))
            {
                o = JsonConvert.DeserializeObject(o.ToString());
            }
            object v = toObject(o);
            return v;
        }

        public static object Decode(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        private static object toObject(object o)
        {
            if (o == null) return null;

            if (o.GetType() == typeof(string))
            {
                //判断是否符合2010-09-02T10:00:00的格式
                string s = o.ToString();
                if (s.Length == 19 && s[10] == 'T' && s[4] == '-' && s[13] == ':')
                {
                    o = System.Convert.ToDateTime(o);
                }
            }
            else if (o is JObject)
            {
                JObject jo = o as JObject;

                Hashtable h = new Hashtable();

                foreach (KeyValuePair<string, JToken> entry in jo)
                {
                    h[entry.Key] = toObject(entry.Value);
                }

                o = h;
            }
            else if (o is IList)
            {

                ArrayList list = new ArrayList();
                list.AddRange((o as IList));
                int i = 0, l = list.Count;
                for (; i < l; i++)
                {
                    list[i] = toObject(list[i]);
                }
                o = list;

            }
            else if (typeof(JValue) == o.GetType())
            {
                JValue v = (JValue)o;
                o = toObject(v.Value);
            }
            else
            {
            }
            return o;
        }

    }
}
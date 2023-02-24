using Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

/*----------------------------------------------------  
//文件名：ExtensionHelper
//文件功能描述：扩展方法
     
//创建标识：by Zhao Bo 2020/3/31
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Helper
{
    /// <summary>
    /// 界面显示样式扩展
    /// </summary>
    public static class DisplayStyle
    {
        /// <summary>
        /// 设置目标值自适应当前操作系统缩放比例（如：100%、150%）
        /// </summary>
        /// <param name="value">目标值</param>
        /// <returns>缩放后的值（四舍五入）</returns>
        public static int ToScaling(this int value)
        {
            return (int)Math.Round(value * App.ThisApp.DPI_Percent, MidpointRounding.AwayFromZero);
        }
    }

    public static class Data
    {
        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>克隆后的数据</returns>
        public static T DeepClone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("类型必须可序列化");

            if (object.ReferenceEquals(source, null))
                return default(T);

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
        }
    }

    /// <summary>
    /// 格式转换扩展
    /// </summary>
    public static class FormatConversion
    {
        /// <summary>
        /// 字符串转化成double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            double result;
            StringUtility.ConvertToDouble(str, out result);
            return result;
        }

        /// <summary>
        /// 字符串转化成int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            int result;
            StringUtility.ConvertToInt(str, out result);
            return result;
        }

        /// <summary>
        /// 字符串转化成double
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimals">保留位数</param>
        /// <returns></returns>
        public static double ToDouble(this string str, int decimals)
        {
            return CommonToDouble(str, decimals);
        }

        /// <summary>
        /// 格式化成指定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimals">保留位数, 默认0位</param>
        /// <returns></returns>
        public static string ToFormatString(this string str, int decimals = 0)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            return CommonToDouble(str, decimals).ToString();
        }

        /// <summary>
        /// 格式化成指定字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="decimals">保留位数, 默认0位</param>
        /// <returns></returns>
        public static string ToFormatString(this object obj, int decimals = 0)
        {
            if (obj == null)
                return string.Empty;
            string str = obj.ToString();
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            if (decimals == 0)// obj是int类型或string类型
                return str;
            return CommonToDouble(str, decimals).ToString();
        }

        /// <summary>
        /// 可空double类型转换不可空double类型，如果null返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this double? value)
        {
            return value ?? 0;
        }

        /// <summary>
        /// 可空Int类型转换不可空Int类型，如果null返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this int? value)
        {
            return value ?? 0;
        }

        private static double CommonToDouble(string str, int decimals)
        {
            double result;
            StringUtility.ConvertToDouble(str, decimals, out result);
            return result;
        }
    }
}

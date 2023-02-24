using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableCommonLibrary.Helper
{
    public class StringUtility
    {
        /// <summary>
        /// 移除换行符（"\n"和"\r"）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLineBreak(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            return str.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// 移除换行符和括号及括号内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLineBreakAndParentheses(string str)
        {
            string temp = RemoveLineBreak(str);
            return RemoveParentheses(temp);
        }

        /// <summary>
        /// 移除括号及括号内容
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveParentheses(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            string pattern = @"(（[^（]*\）)+";
            return System.Text.RegularExpressions.Regex.Replace(str, pattern, string.Empty);
        }

        /// <summary>
        /// 字符分隔符( ※ 适用于属性框、DataGridView行分割)
        /// </summary>
        public static char CharSeparator
        {
            get { return '※'; }
        }

        /// <summary>
        /// 字符串分隔符( ※ 适用于属性框、DataGridView行分割)
        /// </summary>
        public static string StringSeparator
        {
            get { return "※"; }
        }

        /// <summary>
        /// 字符分隔符( ∷ 适用于DataGridView单元格分割)
        /// </summary>
        public static char CellCharSeparator
        {
            get { return '∷'; }
        }

        /// <summary>
        /// 字符串分隔符( ∷ 适用于DataGridView单元格分割)
        /// </summary>
        public static string CellStringSeparator
        {
            get { return "∷"; }
        }

        /// <summary>
        /// 将字符串转化为整数
        /// </summary>
        /// <param name="sValue">要转化的字符串</param>
        /// <param name="iValue">转化后的数值</param>
        /// <returns>是否转化成功</returns>
        public static bool ConvertToInt(string sValue, out int iValue)
        {
            iValue = 0;
            try
            {
                if (string.IsNullOrEmpty(sValue))
                    return false;

                if (!int.TryParse(sValue, out iValue))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串转化为数字
        /// </summary>
        /// <param name="sValue">要转化的字符串</param>
        /// <param name="dValue">转化后的数值</param>
        /// <returns>是否转化成功</returns>
        public static bool ConvertToDouble(string sValue, out double dValue)
        {
            dValue = 0;
            try
            {
                if (string.IsNullOrEmpty(sValue))
                    return false;

                if (!double.TryParse(sValue, out dValue))
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将字符串转化为数字
        /// </summary>
        /// <param name="sValue">要转化的字符串</param>
        /// <param name="decimals">小数位数</param>
        /// <param name="dValue">转化后的数值</param>
        /// <returns>是否转化成功</returns>
        public static bool ConvertToDouble(string sValue, int decimals, out double dValue)
        {
            dValue = 0;
            try
            {
                if (string.IsNullOrEmpty(sValue))
                    return false;

                if (!double.TryParse(sValue, out dValue))
                    return false;

                // 精度设置
                dValue = Math.Round(dValue, decimals);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
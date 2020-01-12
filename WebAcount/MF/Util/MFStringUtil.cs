using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// 字符床辅助工具类
/// </summary>
public static class MFStringUtil
{
    /// <summary>
    /// 物体是否是null值或空的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this object value)
    {
        bool retVal = false;
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()) || value.GetType().Equals(DBNull.Value.GetType()))
        {
            retVal = true;
        }
        return retVal;
    }
    #region ToInt

    /// <summary>
    /// 转换成int
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int ToInt(this object value)
    {
        return value.ToInt(0);
    }
    /// <summary>
    /// 转换成int，有默认值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defeultValue"></param>
    /// <returns></returns>
    public static int ToInt(this object value, int defeultValue)
    {
        defeultValue = 0;
        if (!value.IsNullOrEmpty())
        {
            if (value.ToString().IsNumberSign())
            {
                defeultValue = Convert.ToInt32(value);
            }
        }
        return defeultValue;
    }
    #endregion

    #region Object 转 String
    public static string ObjectToString(this object canNullStr)
    {
        return canNullStr.ObjectToString("");
    }

    public static string ObjectToString(this object canNullStr, string defaultStr)
    {
        try
        {
            if (canNullStr == null || canNullStr is DBNull)
            {
                if (defaultStr != null)
                {
                    return defaultStr;
                }
                return string.Empty;
            }
            return Convert.ToString(canNullStr).Trim();
        }
        catch
        {
            return defaultStr;
        }
    }
    #endregion

    #region string 转 IList<string> 
    /// <summary>
    /// 将以","隔开的文本 转化成IList(string)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IList<string> ToList(this string value)
    {
        return value.ToList(',');
    }

    public static IList<string> ToList(this string value, char opr)
    {
        IList<string> lst = new List<string>();

        if (!value.IsNullOrEmpty())
        {
            string[] arr = value.ToString().TrimStart(opr).TrimEnd(opr).Split(opr);

            foreach (string s in arr)
            {
                if (!s.IsNullOrEmpty())
                {
                    if (!lst.Contains(s))
                    {
                        lst.Add(s);
                    }
                }
            }
        }
        return lst;
    }
    #endregion

    /// <summary>
    /// 转化为数据库字符（防止sql注入）
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToDBStrl(this string value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        bool hasKeyword = false;

        if (value.Length > 0)
        {
            Regex reg = new Regex("insert|update|delete|drop");
            string slowerStr = value.ToLower();
            Match m = reg.Match(slowerStr);
            hasKeyword = m.Success;
        }
        if (hasKeyword)
        {
            return string.Empty;
        }
        return value;
    }

    /// <summary>
    /// 相当于string.Format方法
    /// </summary>
    /// <param name="target"></param>
    /// <param name="args">参数数组</param>
    /// <returns></returns>
    public static string FormatWith(this string target, params object[] args)
    {
        return string.Format(target, args);
    }
}


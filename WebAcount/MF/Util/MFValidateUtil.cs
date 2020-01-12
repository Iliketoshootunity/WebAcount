using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// 验证工具
/// </summary>
public static class MFValidateUtil
{
    #region 是否数字字符床正则表达式 可带正负号
    /// <summary>
    /// 是否数字字符床正则表达式 可带正负号
    /// </summary>
    public static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");

    /// <summary>
    /// 是否数字字符串 可带正负号
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsNumberSign(this string value)
    {
        Match m = RegNumberSign.Match(value);
        return m.Success;
    }
    #endregion
}


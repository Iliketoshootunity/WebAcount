using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 返回值类
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class MFReturnValue<T>
{
    #region 构造函数
    public MFReturnValue()
    {

    }
    public MFReturnValue(T value)
        : this()
    {
        this.Value = value;
    }
    #endregion

    /// <summary>
    /// 返回值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 提示信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 是否有错误信息
    /// </summary>
    public bool HasError { get; set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    public Exception Error { get; set; }

    /// <summary>
    /// 返回代码 一般返回存储过程中的Return值
    /// </summary>
    public int ReturnCode { get; set; }

    #region 附加参数
    protected IDictionary<string, object> _outputValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 获取所有的附加参数
    /// </summary>
    public IDictionary<string, object> OutputValues
    {
        get
        {
            return _outputValues;
        }

    }

    /// <summary>
    /// 获取附加参数
    /// </summary>
    /// <typeparam name="TM"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public TM GetOutputValue<TM>(string key)
    {
        if (!_outputValues.ContainsKey(key)) return default(TM);
        return (TM)_outputValues[key];
    }

    public void SetOutputValue<TM>(string key, TM value)
    {
        _outputValues[key] = value;
    }
    #endregion
}



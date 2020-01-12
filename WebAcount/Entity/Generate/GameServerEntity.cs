
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mmcoy.Framework.AbstractBase;

/// <summary>
/// 服务器基类
/// </summary>
[Serializable]
public partial class GameServerEntity : MFAbstractEntity
{
    #region 重写基类属性
    /// <summary>
    /// 主键
    /// </summary>
    public override int? PKValue
    {
        get
        {
            return this.Id;
        }
        set
        {
            this.Id = value;
        }
    }
    #endregion

    #region 实体属性

    /// <summary>
    /// 编号
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public EnumEntityStatus Status { get; set; }

    /// <summary>
    ///运行状态 
    /// </summary>
    public int RunSatus { get; set; }

    /// <summary>
    ///是否推荐 
    /// </summary>
    public bool IsCommand { get; set; }

    /// <summary>
    ///是否新服 
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    ///名称 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///Ip 
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    ///端口 
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    ///创建时间 
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    ///更新时间 
    /// </summary>
    public DateTime UpdateTime { get; set; }

    #endregion
}

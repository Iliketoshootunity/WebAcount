
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mmcoy.Framework.AbstractBase;

/// <summary>
/// 
/// </summary>
[Serializable]
public partial class AccountEntity : MFAbstractEntity
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
    ///用户名 
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///密码 
    /// </summary>
    public string Pwd { get; set; }

    /// <summary>
    ///手机 
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    ///邮箱 
    /// </summary>
    public string Mail { get; set; }

    /// <summary>
    ///元宝 
    /// </summary>
    public int Money { get; set; }

    /// <summary>
    ///渠道号 
    /// </summary>
    public string Channelld { get; set; }

    /// <summary>
    ///最后登录服务器id 
    /// </summary>
    public int LastLogOnServerId { get; set; }

    /// <summary>
    ///最后登录服务器的名称 
    /// </summary>
    public string LastLogOnServerName { get; set; }

    /// <summary>
    ///最后登录服务器时间 
    /// </summary>
    public DateTime LastLogOnServerTime { get; set; }

    /// <summary>
    ///最后登录的角色id 
    /// </summary>
    public int LastLogOnRoleId { get; set; }

    /// <summary>
    ///最后登录角色名称 
    /// </summary>
    public string LastLogOnRoleName { get; set; }

    /// <summary>
    ///最后登录角色职业Id 
    /// </summary>
    public int LastLogOnRoleJobId { get; set; }

    /// <summary>
    ///创建时间 
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    ///更新时间 
    /// </summary>
    public DateTime UpdateTime { get; set; }

    /// <summary>
    ///设备标识 
    /// </summary>
    public string DeviceIdentifier { get; set; }

    /// <summary>
    ///设备型号 
    /// </summary>
    public string DeviceModel { get; set; }

    #endregion
}

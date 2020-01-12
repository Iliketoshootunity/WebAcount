using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



public class RetAccountEntity
{

    public RetAccountEntity()
    {

    }
    public RetAccountEntity(AccountEntity accountEntity)
    {
        Id = accountEntity.Id.Value;
        UserName = accountEntity.UserName;
        YuanBao = accountEntity.Money;
        GameServerEntity serverEntity = GameServerCacheModel.Instance.GetEntity(accountEntity.LastLogOnServerId);
        //第一次登录 没有选择服务器,默认返回选择最新的服务器
        if (serverEntity == null)
        {
            List<GameServerEntity> list = GameServerCacheModel.Instance.GetList(isDesc: true);
            if (list != null && list.Count > 0)
            {
                LastServerId = list[0].Id.Value;
                LastServerName = list[0].Name;
                LastServerIP = list[0].Ip;
                LastServerPort = list[0].Port;
            }
        }
        else
        {
            LastServerId = serverEntity.Id.Value;
            LastServerName = serverEntity.Name;
            LastServerIP = serverEntity.Ip;
            LastServerPort = serverEntity.Port;
        }


    }
    public int Id { get; set; }

    public string UserName { get; set; }

    public int Pwd { get; set; }

    public int YuanBao { get; set; }

    public int LastServerId { get; set; }

    public string LastServerName { get; set; }

    public string LastServerIP { get; set; }

    public int LastServerPort { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}

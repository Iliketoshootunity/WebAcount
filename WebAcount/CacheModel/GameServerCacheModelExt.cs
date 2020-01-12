using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public partial class GameServerCacheModel
{
    public List<RetGameServerPageEntity> GetGameServerPageList()
    {
        List<GameServerEntity> list = this.GetList(isDesc: false);
        List<RetGameServerPageEntity> pageList = new List<RetGameServerPageEntity>();
        int pageIndex = 1;
        for (int i = 1; i <= list.Count; i++)
        {
            if (i % 10 == 0)
            {
                RetGameServerPageEntity entity = new RetGameServerPageEntity();
                entity.PageIndex = pageIndex;
                entity.Name = string.Format("{0}-{1}", i - 9, i);
                pageIndex++;
                pageList.Add(entity);
            }
            else if (i == list.Count && i % 10 != 0)
            {
                RetGameServerPageEntity entity = new RetGameServerPageEntity();
                entity.PageIndex = pageIndex;
                entity.Name = string.Format("{0}-{1}", i - (i % 10) + 1, i);
                pageIndex++;
                pageList.Add(entity);
            }

        }
        return pageList;
    }

    public List<RetGameserverEntity> GetGameServerList(int pageIndex)
    {
        MFReturnValue<List<GameServerEntity>> list = null;
        List<RetGameserverEntity> retList = new List<RetGameserverEntity>();
        if (pageIndex == 0)
        {
            //推荐服务器 玩家所在服务器
            list = this.GetPageList(pageSize: 3, isDesc: false);
        }
        else
        {
            list = this.GetPageList(pageSize: 10, pageIndex: pageIndex, isDesc: false);
        }
        if (list != null)
        {
            for (int i = 0; i < list.Value.Count; i++)
            {
                retList.Add(new RetGameserverEntity()
                {
                    Id = list.Value[i].Id.Value,
                    RunSatus = list.Value[i].RunSatus,
                    IsCommand = list.Value[i].IsCommand,
                    IsNew = list.Value[i].IsNew,
                    Name = list.Value[i].Name,
                    Ip = list.Value[i].Ip,
                    Port = list.Value[i].Port
                });
            }
        }

        return retList;
    }



}

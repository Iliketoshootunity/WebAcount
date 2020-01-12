using LitJson;
using Mmcoy.Framework.AbstractBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAcount.Controllers.api
{
    public class GameServerController : ApiController
    {
        // GET: api/GameServer
        public IEnumerable<string> Get()
        {

            return new string[] { "a,", "b" };
        }

        // GET: api/GameServer/5
        public List<RetGameserverEntity> Get(int pageIndex)
        {
            return GameServerCacheModel.Instance.GetGameServerList(pageIndex);

        }

        // POST: api/GameServer
        public object Post([FromBody]string value)
        {
            RetValue ret = new RetValue();
            JsonData jsonData = JsonMapper.ToObject(value);
            //1.验证时间戳
            //2.验证签名

            //客户端时间戳
            long t = Convert.ToInt64(jsonData["t"].ToString());
            //签名
            string sign = jsonData["sign"].ToString();
            //客户端驱动id
            string deviceIdentifier = jsonData["deviceIdentifier"].ToString();
            string deviceModel = jsonData["DeviceModel"].ToString();
            long st = MFDSAUtil.GetTimestamp();
            //第一重检验
            //if (st - t > 300)
            //{
            //    ret.IsError = true;
            //    ret.ErrorMsg = "请求失败";
            //    return ret;
            //}
            //第二重检验
            if (string.Format("{0},{1}", deviceIdentifier, t) != sign)
            {
                ret.IsError = true;
                ret.ErrorMsg = "请求失败";
                return ret;
            }
            //0.获取页签列表  1.获取服务器列表
            int type = Convert.ToInt32(jsonData["Type"].ToString());
            if (type == 0)
            {
                int pageIndex = Convert.ToInt32(jsonData["PageIndex"].ToString());
                List<RetGameserverEntity> gameServerList = GameServerCacheModel.Instance.GetGameServerList(pageIndex);
                return gameServerList;
            }
            else if (type == 1)
            {
                List<RetGameServerPageEntity> pageList = GameServerCacheModel.Instance.GetGameServerPageList();
                return pageList;
            }
            //更新登录结果
            else if (type == 2)
            {
                IDictionary<string, object> dic = new Dictionary<string, object>();
                int userID = Convert.ToInt32(jsonData["UserID"].ToString());
                int lastServerId = Convert.ToInt32(jsonData["LastServerId"].ToString());
                string lastServerName = jsonData["LastServerName"].ToString();
                dic["Id"] = userID;
                dic["LastLogOnServerId"] = lastServerId;
                dic["LastLogOnServerName"] = lastServerName;
                dic["LastLogOnServerTime"] = DateTime.Now;
                AccountCacheModel.Instance.Update("LastLogOnServerId=@LastLogOnServerId,LastLogOnServerName=@LastLogOnServerName,LastLogOnServerTime=@LastLogOnServerTime", "Id=@Id", dic);
            }

            ret.IsError = true;
            ret.ErrorMsg = "请求失败";
            return ret;
        }

        // PUT: api/GameServer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GameServer/5
        public void Delete(int id)
        {
        }
    }
}

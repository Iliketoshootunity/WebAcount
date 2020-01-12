using LitJson;
using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAcount.Entity;
using WebAcount.Models;

namespace WebAcount.Controllers.api
{
    public class AccountController : ApiController
    {
        // GET: api/Account
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value5" };
        }

        // GET: api/Account/5
        public AccountEntityTest Get(int id)
        {
            return AccountDBModelTest.Instance.Get(id);
        }

        // POST: api/Account
        public RetValue Post([FromBody]string value)
        {
            RetValue ret = new RetValue();
            JsonData jsonData = JsonMapper.ToObject(value);
            //1.验证时间戳
            //2.验证签名
            int type = Convert.ToInt32(jsonData["Type"].ToString());
            string userName = jsonData["UserName"].ToString();
            string pwd = jsonData["Pwd"].ToString();
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

            if (type == 0)
            {
                string ChannelId = JsonMapper.ToObject(value)["Channelld"].ToString();
                MFReturnValue<int> ret2 = AccountCacheModel.Instance.Register(userName, pwd, ChannelId, deviceIdentifier, deviceModel);
                ret.IsError = ret2.HasError;
                ret.ErrorMsg = ret2.Message;
                AccountEntity entity = AccountCacheModel.Instance.GetEntity(ret2.Value);
                ret.RetData = JsonMapper.ToJson(new RetAccountEntity(entity));
            }
            else
            {
                AccountEntity entity = AccountCacheModel.Instance.LogOn(userName, pwd, deviceIdentifier, deviceModel);
                if (entity != null)
                {
                    ret.IsError = false;
                    ret.RetData = entity.Id;
                }
                else
                {
                    ret.IsError = true;
                    ret.ErrorMsg = "账号或者密码错误";
                }
                RetAccountEntity retEnitity = new RetAccountEntity(entity);
                ret.RetData = JsonMapper.ToJson(retEnitity);
            }
            return ret;
        }

        // PUT: api/Account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}

using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public partial class AccountDBModel
{
    public MFReturnValue<int> Register(string userName, string pwd, string channelID, string deviceIdentifier, string deviceModel)
    {
        MFReturnValue<int> ret1 = new MFReturnValue<int>();

        using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=DBAccount;Integrated Security=True"))
        {
            conn.Open();
            //1.查询是否存在相同的账号
            //2.没有则注册
            SqlTransaction trans = conn.BeginTransaction();
            List<AccountEntity> list = this.GetListWithTran(this.TableName, "Id", "UserName='" + userName + "'", trans: trans, isAutoStatus: false);
            if (list == null || list.Count == 0)
            {
                AccountEntity entity = new AccountEntity();
                entity.UserName = userName;
                entity.Status = Mmcoy.Framework.AbstractBase.EnumEntityStatus.Released;
                entity.Pwd = pwd;
                entity.Channelld = channelID;
                entity.LastLogOnServerTime = DateTime.Now;
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime = DateTime.Now;
                entity.DeviceIdentifier = deviceIdentifier;
                entity.DeviceModel = deviceModel;
                MFReturnValue<object> ret2 = this.Create(trans, entity);
                if (ret2.HasError)
                {
                    ret1.HasError = true;
                    ret1.Message = ret1.Message;
                    trans.Rollback();
                }
                else
                {
                    ret1.HasError = false;
                    ret1.Value = (int)ret2.OutputValues["Id"];
                    trans.Commit();
                }

            }
            else
            {
                ret1.HasError = true;
                ret1.Message = "账号已经存在";
            }
            return ret1;
        }
    }

    public AccountEntity LogOn(string userName, string pwd, string deviceIdentifier, string deviceModel)
    {
        AccountEntity entity = null;
        string condition = string.Format("[UserName]='{0}' and [Pwd]='{1}'", userName, pwd);
        entity = this.GetEntity(condition);
        if(entity!=null)
        {
            entity.DeviceIdentifier = deviceIdentifier;
            entity.DeviceModel = deviceModel;
            this.Update(entity);
        }
        return entity;
    }
}

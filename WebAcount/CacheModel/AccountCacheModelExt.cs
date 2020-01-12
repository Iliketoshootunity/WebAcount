using Mmcoy.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public partial class AccountCacheModel
{
    public MFReturnValue<int> Register(string userName, string pwd, string channelID, string deviceIdentifier, string deviceModel)
    {
        AccountDBModel t = this.DBModel;
        return this.DBModel.Register(userName, pwd, channelID, deviceIdentifier, deviceModel);
    }

    public AccountEntity LogOn(string userName, string pwd, string deviceIdentifier, string deviceModel)
    {
        return this.DBModel.LogOn(userName, pwd,deviceIdentifier, deviceModel);
    }
}

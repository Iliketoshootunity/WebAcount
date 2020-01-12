using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAcount.Entity
{
    public class AccountEntityTest
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int Pwd { get; set; }

        public int YuanBao { get; set; }

        public int LastServerId { get; set; }

        public string LastServerName { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
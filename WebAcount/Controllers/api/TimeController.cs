﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAcount.Controllers.api
{
    public class TimeController : ApiController
    {
        public long Get()
        {
            return MFDSAUtil.GetTimestamp();
        }
    }
}

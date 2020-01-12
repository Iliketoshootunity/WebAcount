using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class DBConn
{
    private static string m_DBAccount;

    public static string DBAccount
    {
        get
        {
            if (string.IsNullOrEmpty(m_DBAccount))
            {
                m_DBAccount = "Data Source=.;Initial Catalog=DBAccount;Integrated Security=True";

            }
            return m_DBAccount;
        }
    }
}

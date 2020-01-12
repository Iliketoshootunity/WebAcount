using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebAcount.Entity;

namespace WebAcount.Models
{
    public class AccountDBModelTest
    {
        private static object lock_obj = new object();

        private static AccountDBModelTest _instance;

        public static AccountDBModelTest Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lock_obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AccountDBModelTest();
                        }
                    }
                }
                return _instance;
            }
        }


        private const string connString = "Data Source=.;Initial Catalog=DBAccount;Integrated Security=True";

        public AccountEntityTest Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //打开数据连接
                conn.Open();
                //建立一个命令，参数是存储过程代码和数据库连接
                SqlCommand cmmand = new SqlCommand("Account_Get", conn);
                //命令类型为存储过程
                cmmand.CommandType = CommandType.StoredProcedure;
                //添加 Accout_Get 参数,根据ID
                cmmand.Parameters.Add(new SqlParameter(@"Id", id));

                using (SqlDataReader dr = cmmand.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dr.HasRows && dr.Read())
                    {
                        AccountEntityTest entity = new AccountEntityTest();

                        entity.Id = dr["Id"] is DBNull ? 0 : Convert.ToInt32(dr["Id"]);
                        entity.UserName = dr["UserName"] is DBNull ? string.Empty : dr["UserName"].ToString();
                        entity.Pwd = dr["Pwd"] is DBNull ? 0 : Convert.ToInt32(dr["Pwd"]);
                        entity.YuanBao = dr["YuanBao"] is DBNull ? 0 : Convert.ToInt32(dr["YuanBao"]);
                        entity.LastServerId = dr["LastServerId"] is DBNull ? 0 : Convert.ToInt32(dr["LastServerId"]);
                        entity.LastServerName = dr["LastServerName"] is DBNull ? string.Empty : dr["LastServerName"].ToString();
                        entity.CreateTime = dr["CreateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["CreateTime"]);
                        entity.UpdateTime = dr["CreateTime"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(dr["CreateTime"]);

                        return entity;
                    }
                }
            }

            return null;
        }

        public RetValue Register(string userName, string pwd)
        {
            RetValue ret = new RetValue();
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand com = new SqlCommand("Account_Register", conn);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.Add(new SqlParameter("UserName", userName));
                    com.Parameters.Add(new SqlParameter("Pwd", pwd));
                    int data = Convert.ToInt32(com.ExecuteScalar().ToString());
                    ret.RetData = data;
                }
            }
            catch (Exception e)
            {
                ret.IsError = true;
                ret.ErrorMsg = e.ToString();
            }
            return ret;
 
        }



    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MFSqlHelper
{

    #region ExecuteNonQuery
    /// <summary>
    ///  对连接执行 Transact-SQL 语句并返回受影响的行数。不加入事务
    /// </summary>
    /// <returns></returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameterArr)
    {
        if (connectionString == null || connectionString.Length == 0)
        {
            throw new ArgumentNullException("connectionString");
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteNonQuery(connection, commandType, commandText, parameterArr);
        }
    }

    /// <summary>
    /// 对连接执行 Transact-SQL 语句并返回受影响的行数。不加入事务
    /// </summary>
    /// <param name="connection"></param>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <param name="parameterArr"></param>
    /// <returns></returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parameterArr)
    {
        if (connection == null)
        {
            throw new ArgumentNullException("connection");
        }
        SqlCommand command = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(command, connection, null, commandType, commandText, parameterArr, out mustCloseConnection);
        int num = command.ExecuteNonQuery();
        command.Parameters.Clear();
        if (mustCloseConnection)
        {
            connection.Close();
        }
        return num;
    }
    /// <summary>
    /// 对连接执行 Transact-SQL 语句并返回受影响的行数。加入事务
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="commandType"></param>
    /// <param name="commandText"></param>
    /// <param name="parameterArr"></param>
    /// <returns></returns>

    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] parameterArr)
    {
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException("The transaction war rollbacked or commited,please provide an open transaction", "transaction");
            }
            SqlCommand command = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, parameterArr, out mustCloseConnection);
            int num = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return num;
        }
    }
    #endregion

    #region ExecuteReader
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameterArr)
    {
        SqlDataReader reader = null;
        if (connectionString.IsNullOrEmpty())
        {
            throw new ArgumentNullException("connectionString");
        }
        SqlConnection conn = null;
        try
        {
            conn = new SqlConnection(connectionString);
            conn.Open();
            reader = ExecuteReader(conn, null, commandType, commandText, parameterArr, SqlConnectionOwnership.Internal);


        }
        catch
        {
            if (conn != null)
            {
                conn.Close();
            }
            throw;
        }
        return reader;
    }
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parameterArr)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException("transaction");
        }
        if ((transaction != null) && (transaction.Connection == null))
        {
            throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        }
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, parameterArr, SqlConnectionOwnership.External);
    }
    public static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parameterArr, SqlConnectionOwnership connectionOwnership)
    {
        if (connection == null)
        {
            throw new ArgumentNullException("connection");
        }
        bool mustCloseConnection = false;
        SqlDataReader reader = null;
        SqlCommand cmd = new SqlCommand();
        try
        {
            PrepareCommand(cmd, connection, transaction, commandType, commandText, parameterArr, out mustCloseConnection);
            if (connectionOwnership == SqlConnectionOwnership.External)
            {
                reader = cmd.ExecuteReader();
            }
            else
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            bool flag = true;
            foreach (SqlParameter item in cmd.Parameters)
            {
                //有一个不是input 
                if (item.Direction != ParameterDirection.Input)
                {
                    flag = false;
                }
            }
            //不能有输入
            if (flag)
            {
                //清除parameterArr引用
                cmd.Parameters.Clear();
            }

        }
        catch (Exception e)
        {
            if (mustCloseConnection)
            {
                connection.Close();
            }
            throw;
        }
        return reader;
    }
    #endregion

    #region ExecuteScalar
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteScalar(connectionString, commandType, commandText, null);
    }
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if ((connectionString == null) || (connectionString.Length == 0))
        {
            throw new ArgumentNullException("connectionString");
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteScalar(connection, commandType, commandText, commandParameters);
        }
    }
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null)
        {
            throw new ArgumentNullException("connection");
        }
        SqlCommand command = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(command, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
        object obj2 = command.ExecuteScalar();
        command.Parameters.Clear();
        if (mustCloseConnection)
        {
            connection.Close();
        }
        return obj2;
    }

    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException("transaction");
        }
        if ((transaction != null) && (transaction.Connection == null))
        {
            throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        }
        SqlCommand command = new SqlCommand();
        //这个值没用，有SqlTransaction 的 由外面关闭
        bool mustCloseConnection = false;
        PrepareCommand(command, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
        object obj2 = command.ExecuteScalar();
        command.Parameters.Clear();
        return obj2;
    }
    #endregion
    /// <summary>
    /// 发布命令前的准备
    /// </summary>
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parameterArr, out bool mustCloseConnection)
    {
        if (command == null)
        {
            throw new ArgumentNullException("command");
        }
        if (commandText == null || commandText.Length == 0)
        {
            throw new ArgumentNullException("commandText");
        }
        if (connection.State != ConnectionState.Open)
        {
            mustCloseConnection = true;
            connection.Open();
        }
        else
        {
            mustCloseConnection = false;
        }
        command.Connection = connection;
        command.CommandText = commandText;
        if (transaction != null)
        {
            if (transaction.Connection == null)
            {
                throw new ArgumentException("The transaction war rollbacked or commited,please provide an open transaction", "transaction");
            }
            command.Transaction = transaction;
        }
        command.CommandType = commandType;
        if (parameterArr != null)
        {
            AttachParameters(command, parameterArr);
        }

    }


    /// <summary>
    /// 附加参数到SqlCommand
    /// </summary>
    /// <param name="command"></param>
    /// <param name="commandParameters"></param>
    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
        if (command == null)
        {
            throw new ArgumentNullException("command");
        }
        if (commandParameters != null)
        {
            foreach (SqlParameter parameter in commandParameters)
            {
                if (parameter != null)
                {
                    if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Input)) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }
        }
    }
    public enum SqlConnectionOwnership
    {
        /// <summary>
        /// 内部的
        /// </summary>
        Internal,
        /// <summary>
        /// 外部的
        /// </summary>
        External
    }
}




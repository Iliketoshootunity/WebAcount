using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mmcoy.Framework.AbstractBase
{
    /// <summary>
    /// SQL DBModel 抽象基类
    /// </summary>
    public abstract class MFAbstractSQLDBModel<T> where T : MFAbstractEntity, new()
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected abstract string ConnectionString { get; }

        /// <summary>
        /// 表名
        /// </summary>
        protected abstract string TableName { get; }


        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        /// <summary>
        /// 实体转数据库参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract SqlParameter[] ValueParas(T entity);


        /// <summary>
        /// 封装实体
        /// </summary>
        /// <param name="render"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        protected abstract T GetEntitySelfProperty(IDataReader render, DataTable table);

        /// <summary>
        /// 列集合
        /// </summary>
        protected abstract IList<string> ColumnList { get; }

        #region Create
        /// <summary>
        /// 创建对象，单独执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Create(T entity)
        {
            return Create(null, entity);
        }

        /// <summary>
        /// 创建对象，存在事务中
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Create(SqlTransaction transaction, T entity)
        {
            //设置参数
            SqlParameter[] parameterArray = ValueParas(entity);
            //第一个参数是id，所以传入的参数是无效的,类似out
            parameterArray[0].Direction = ParameterDirection.Output;
            //倒数第二个参数(RetMsg)，闯入的参数是无效的,类似 out
            parameterArray[parameterArray.Length - 2].Direction = ParameterDirection.Output;
            //最后一个参数是存储过程的返回值
            parameterArray[parameterArray.Length - 1].Direction = ParameterDirection.ReturnValue;

            //执行没有事务的查询
            if (transaction.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Create", this.TableName), parameterArray);
            }
            //执行有事务的查询
            else
            {
                MFSqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, string.Format("{0}_Create", this.TableName), parameterArray);
            }
            //获取存储过程返回值
            int returnCode = parameterArray[parameterArray.Length - 1].Value.ToInt();
            //设置返回值
            MFReturnValue<object> val = new MFReturnValue<object>();
            if (returnCode < 0)
            {
                val.HasError = true;
            }
            else
            {
                val.HasError = false;
                val.OutputValues["Id"] = parameterArray[0].Value.ToInt();
            }
            val.Message = parameterArray[parameterArray.Length - 2].Value.ObjectToString();
            val.ReturnCode = returnCode;
            return val;
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新对象，单独执行
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(T entity)
        {
            return Update(null, entity);
        }

        /// <summary>
        /// 更新对象 存在事务中
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(SqlTransaction transaction, T entity)
        {
            //设置参数
            SqlParameter[] parameterArray = ValueParas(entity);
            //倒数第二个参数(RetMsg)，闯入的参数是无效的,类似 out
            parameterArray[parameterArray.Length - 2].Direction = ParameterDirection.Output;
            //最后一个参数是存储过程的返回值
            parameterArray[parameterArray.Length - 1].Direction = ParameterDirection.ReturnValue;

            //执行没有事务的查询
            if (transaction.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Update", this.TableName), parameterArray);
            }
            //执行有事务的查询
            else
            {
                MFSqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, string.Format("{0}_Update", this.TableName), parameterArray);
            }
            //获取存储过程返回值
            int returnCode = parameterArray[parameterArray.Length - 1].Value.ToInt();
            //设置返回值
            MFReturnValue<object> val = new MFReturnValue<object>();
            if (returnCode < 0)
            {
                val.HasError = true;
            }
            else
            {
                val.HasError = false;
            }
            val.Message = parameterArray[parameterArray.Length - 2].Value.ObjectToString();
            val.ReturnCode = returnCode;
            return null;
        }

        /// <summary>
        /// 更新对象 单独执行 命令自己写
        /// </summary>
        /// <param name="setStr"></param>
        /// <param name="conditionStr"></param>
        /// <param name="parameterDic"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(string setStr, string conditionStr, IDictionary<string, object> parameterDic)
        {
            return Update(null, setStr, conditionStr, parameterDic);
        }

        /// <summary>
        /// 更新对象 存在事务中 命令自己写
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="setStr"></param>
        /// <param name="conditionStr"></param>
        /// <param name="parameterDic"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(SqlTransaction transaction, string setStr, string conditionStr, IDictionary<string, object> parameterDic)
        {

            MFReturnValue<object> retValue = new MFReturnValue<object>();
            try
            {
                SqlParameter[] paramArray = new SqlParameter[parameterDic.Count];
                int index = 0;
                foreach (var item in parameterDic)
                {
                    paramArray[index] = new SqlParameter(item.Key, item.Value);
                    index++;
                }

                string commandText = string.Format("UPDATE {0} SET {1} WHERE {2}", this.TableName, setStr, conditionStr);
                if (transaction != null)
                {
                    MFSqlHelper.ExecuteNonQuery(transaction, CommandType.Text, commandText, paramArray);
                }
                else
                {
                    MFSqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, commandText, paramArray);
                }
            }
            catch (Exception e)
            {
                retValue.HasError = true;
                retValue.Message = e.Message;
            }
            return retValue;

        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除，单独执行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Delete(int? id)
        {
            return Delete(id, null);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">,隔开编号</param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Delete(string ids)
        {
            MFReturnValue<object> retValue = new MFReturnValue<object>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    IList<string> list = ids.ToList(',');
                    foreach (var item in list)
                    {
                        Delete(item.ToInt(), trans);
                    }
                    trans.Commit();
                    retValue.HasError = false;
                    retValue.Message = "批量上传成功";
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    retValue.HasError = true;
                    retValue.Message = e.Message;
                }

            }
            return retValue;
        }
        /// <summary>
        /// 删除 在事务中
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Delete(int? id, SqlTransaction transaction)
        {
            MFReturnValue<object> retValue = new MFReturnValue<object>();
            int nId;
            if (id.HasValue)
            {
                nId = id.Value;
            }
            else
            {
                retValue.HasError = true;
                retValue.Message = "无效的主键";
                return retValue;
            }

            SqlParameter[] parameterArr = new SqlParameter[] {
                new SqlParameter("@Id",nId),
                new SqlParameter("@RetMsg",SqlDbType.NVarChar,255),
                new SqlParameter("@ReturnValue",SqlDbType.Int)
            };

            parameterArr[parameterArr.Length - 2].Direction = ParameterDirection.Output;
            parameterArr[parameterArr.Length - 1].Direction = ParameterDirection.ReturnValue;

            if (transaction.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, string.Format("{0}_Delete", this.TableName), parameterArr);
            }
            else
            {
                MFSqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, String.Format("{0}_Delete", this.TableName), parameterArr);
            }
            int returnCode = parameterArr[parameterArr.Length - 1].Value.ToInt();
            if (returnCode < 0)
            {
                retValue.HasError = true;
            }
            else
            {
                retValue.HasError = false;
            }
            return retValue;
        }
        #endregion

        #region 通用执行方法
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] paramArr = null)
        {
            return MFSqlHelper.ExecuteNonQuery(this.ConnectionString, cmdType, cmdText, paramArr);
        }

        /// <summary>
        /// 执行单值查询
        /// </summary>
        /// <param name="cmdText">要执行的命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        protected object ExecuteScalar(string cmdText, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] param = null)
        {
            return MFSqlHelper.ExecuteScalar(this.ConnectionString, cmdType, cmdText, param);
        }
        protected object ExecuteScalar(SqlTransaction trans, string cmdText, CommandType cmdType, SqlParameter[] param)
        {
            if (trans == null)
            {
                return MFSqlHelper.ExecuteScalar(this.ConnectionString, cmdType, cmdText, param);
            }
            else
            {
                return MFSqlHelper.ExecuteScalar(trans, cmdType, cmdText, param);
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity(int? id)
        {
            SqlParameter[] paramArray = new SqlParameter[] {
                new SqlParameter("@Id", id)
            };

            return GetEntity("{0}_GetEntity".FormatWith(this.TableName), paramArray);
        }
        /// <summary>
        /// 根据查询条件查询实体
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isAutoStatus"></param>
        /// <returns></returns>
        public T GetEntity(string condition, bool isAutoStatus = true)
        {
            if (isAutoStatus && ColumnList.Contains("Status") && condition.IndexOf("Status") == -1)
            {
                var statuString = " Status = 1 ";
                condition = (statuString + (condition.IsNullOrEmpty() ? string.Empty : " And ") + condition).ToDBStrl();
            }
            return GetEntity(string.Format("select * from [{0}] where {1}", this.TableName, condition), null, CommandType.Text);
        }

        /// <summary>
        ///自定义sql会存储过程查询实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArray"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public T GetEntity(string sql, SqlParameter[] paramArray, CommandType cmdType = CommandType.StoredProcedure)
        {
            T entity = default(T);
            using (SqlDataReader reader = MFSqlHelper.ExecuteReader(this.ConnectionString, cmdType, sql.ToDBStrl(), paramArray))
            {
                //前进到下一条记录
                if (reader != null && reader.Read())
                {
                    DataTable columnData = reader.GetSchemaTable();
                    entity = GetEntitySelfProperty(reader, columnData);
                }
            }
            return entity;
        }

        /// <summary>
        /// 通过DataReader 获取实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<T> GetEntityList(IDataReader reader)
        {
            List<T> list = new List<T>();
            T entity = default(T);
            using (reader)
            {
                while (reader != null && reader.Read())
                {
                    DataTable columnData = reader.GetSchemaTable();
                    list.Add(GetEntitySelfProperty(reader, columnData));
                }
            }
            return list;
        }
        #endregion

        public MFReturnValue<List<T>> GetPageList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false)
        {
            return GetPageListWithTran(tableName: tableName, columns: columns, condition: condition, orderby: orderby, isDesc: isDesc, pageSize: pageSize, pageIndex: pageIndex, isAutoStatus: isAutoStatus, isEfficient: isEfficient, trans: null);
        }
        /// <summary>
        /// 返回分页集合
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        public MFReturnValue<List<T>> GetPageListWithTran(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false, SqlTransaction trans = null)
        {
            if (isAutoStatus && ColumnList.Contains("Status") && condition.IndexOf("Status") == -1)
            {
                var statuString = " Status = 1 ";
                condition = statuString + (condition.IsNullOrEmpty() ? string.Empty : " And ") + condition;
            }

            //非高效存储过程 并且 不包含排序关键字的时候 自动加排序关键字
            if (orderby.IndexOf("asc", StringComparison.CurrentCultureIgnoreCase) == -1 && orderby.IndexOf("desc", StringComparison.CurrentCultureIgnoreCase) == -1 && orderby.IndexOf(",") == -1 && isEfficient == false)
            {
                orderby += isDesc ? " desc" : " asc";
            }

            if (tableName.IsNullOrEmpty())
            {
                tableName = (this.TableName.IndexOf(" join ", StringComparison.CurrentCultureIgnoreCase) == -1) ? string.Format("[{0}]", this.TableName) : this.TableName;
            }
            //表名
            var tableParam = new SqlParameter("@TableName", tableName);
            //查询列
            var colParam = new SqlParameter("@Fields", columns);
            //查询条件
            var whereParam = new SqlParameter("@OrderField", orderby);
            //排序条件
            var orderbyParam = new SqlParameter("@sqlWhere", condition);
            //分页条数
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            //当前页码
            var pageIndexParam = new SqlParameter("@PageIndex", pageIndex);
            //总记录数
            var pagesParam = new SqlParameter("@TotalCount", SqlDbType.Int);
            pagesParam.Direction = ParameterDirection.Output;

            //排序方式
            var orderTypeParam = new SqlParameter("@OrderType", isDesc);

            SqlParameter[] paramArray = null;

            //高效存储过程多一个参数
            if (isEfficient)
            {
                paramArray = new SqlParameter[] { tableParam, colParam, whereParam, orderbyParam, pageSizeParam, pageIndexParam, orderTypeParam, pagesParam };
            }
            else
            {
                paramArray = new SqlParameter[] { tableParam, colParam, whereParam, orderbyParam, pageSizeParam, pageIndexParam, pagesParam };
            }

            IDataReader reader;
            if (trans == null)
            {
                reader = MFSqlHelper.ExecuteReader(this.ConnectionString, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            else
            {
                reader = MFSqlHelper.ExecuteReader(trans, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            var backVal = new MFReturnValue<List<T>>();
            backVal.Value = GetEntityList(reader);
            backVal.OutputValues["TotalCount"] = pagesParam.Value.ToInt();
            return backVal;
        }


        #region GetList 返回集合
        public List<T> GetList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, bool isAutoStatus = true, bool isEfficient = false)
        {
            return GetListWithTran(tableName: tableName, columns: columns, condition: condition, orderby: orderby, isDesc: isDesc, isAutoStatus: isAutoStatus, isEfficient: isEfficient, trans: null);
        }

        /// <summary>
        /// 返回集合
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        public List<T> GetListWithTran(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, bool isAutoStatus = true, bool isEfficient = false, SqlTransaction trans = null)
        {
            return GetPageListWithTran(tableName, columns, condition, orderby, isDesc, 1000, 1, isAutoStatus, isEfficient, trans: trans).Value;
        }
        #endregion

        #region GetCount 返回总数
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int GetCount(string condition = "")
        {
            condition = condition.IsNullOrEmpty() ? string.Empty : "where {0}".FormatWith(condition);
            string strSql = "select count(0) from [{0}] {1}".FormatWith(this.TableName, condition);
            return MFStringUtil.ToInt(MFSqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, strSql));
        }

        #endregion
    }
}

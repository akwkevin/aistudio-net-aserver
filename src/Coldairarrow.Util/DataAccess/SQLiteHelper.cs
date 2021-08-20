using EFCore.Sharding;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Coldairarrow.Util
{
    /// <summary>
    /// SqlServer数据库操作帮助类
    /// </summary>
    public class SQLiterHelper : DbHelper
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conString">完整连接字符串</param>
        public SQLiterHelper(string conString)
            : base(DatabaseType.SQLite, conString)
        {
            DbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(DatabaseType.SQLite);
            ConnectionString = conString;
        }

        #endregion

        #region 私有成员

        protected override Dictionary<string, Type> DbTypeDic { get; } = new Dictionary<string, Type>()
        {
            { "int", typeof(Int32) },
            { "text", typeof(string) },
            { "bigint", typeof(Int64) },
            { "binary", typeof(byte[]) },
            { "bit", typeof(bool) },
            { "char", typeof(string) },
            { "date", typeof(DateTime) },
            { "datetime", typeof(DateTime) },
            { "datetime2", typeof(DateTime) },
            { "decimal", typeof(decimal) },
            { "float", typeof(double) },
            { "image", typeof(byte[]) },
            { "money", typeof(decimal) },
            { "nchar", typeof(string) },
            { "ntext", typeof(string) },
            { "numeric", typeof(decimal) },
            { "nvarchar", typeof(string) },
            { "real", typeof(Single) },
            { "smalldatetime", typeof(DateTime) },
            { "smallint", typeof(Int16) },
            { "smallmoney", typeof(decimal) },
            { "timestamp", typeof(DateTime) },
            { "tinyint", typeof(byte) },
            { "varbinary", typeof(byte[]) },
            { "varchar", typeof(string) },
            { "variant", typeof(object) },
            { "uniqueidentifier", typeof(Guid) },
        };

        #endregion

        #region 外部接口

        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <param name="schemaName">模式（架构）</param>
        /// <returns></returns>
        public override List<DbTableInfo> GetDbAllTables(string schemaName = null)
        {
            throw new Exception("暂未实现");
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<TableInfo> GetDbTableInfo(string tableName)
        {
            throw new Exception("暂未实现");
        }

        /// <summary>
        /// 生成实体文件
        /// </summary>
        /// <param name="infos">表字段信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="tableDescription">表描述信息</param>
        /// <param name="filePath">文件路径（包含文件名）</param>
        /// <param name="nameSpace">实体命名空间</param>
        /// <param name="schemaName">架构（模式）名</param>
        public override void SaveEntityToFile(List<TableInfo> infos, string tableName, string tableDescription, string filePath, string nameSpace, string schemaName = null)
        {
            base.SaveEntityToFile(infos, tableName, tableDescription, filePath, nameSpace, schemaName);
        }

        #endregion

        #region dbhelper
        //以下实现的帮助类方法，仅供该例子使用，具体请参照其他完整的DbHelp帮助类
        DbProviderFactory DbProviderFactory;
        string ConnectionString;

        private void ThrowExceptionIfLengthNotEqual(string[] sqls, params DbParameter[][] parameters)
        {
            if (parameters.GetLength(0) != 0 && sqls.Length != parameters.GetLength(0)) throw new ArgumentException($"一维数组{nameof(sqls)}的长度与二维数组{nameof(parameters)}长度的第一维长度不一致");
        }

        private T[] Execute<T>(string[] sqls, CommandType commandType = CommandType.Text, ExecuteMode executeMode = ExecuteMode.NonQuery, params DbParameter[][] parameters)
        {
            ThrowExceptionIfLengthNotEqual(sqls, parameters);
            if (executeMode == ExecuteMode.NonQuery && typeof(T) != typeof(int)) throw new InvalidCastException("使用NonQuery模式时，必须将类型T指定为int");
            using (DbConnection connection = DbProviderFactory.CreateConnection())
            using (DbCommand command = DbProviderFactory.CreateCommand())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command.Connection = connection;
                command.CommandType = commandType;
                DbTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    List<T> resultList = new List<T>();
                    for (int i = 0; i < sqls.Length; i++)
                    {
                        command.CommandText = sqls[i];
                        if (parameters.GetLength(0) != 0)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddRange(parameters[i]);
                        }
                        object result = null;
                        switch (executeMode)
                        {
                            case ExecuteMode.NonQuery:
                                result = command.ExecuteNonQuery(); break;
                            case ExecuteMode.Scalar:
                                result = command.ExecuteScalar(); break;
                            default: throw new NotImplementedException();
                        }
                        resultList.Add((T)Convert.ChangeType(result, typeof(T)));
                    }
                    transaction.Commit();
                    return resultList.ToArray();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public int ExecuteNonQuery(string sql, params DbParameter[] parameter) => ExecuteNonQuery(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public int[] ExecuteNonQuery(string[] sqls, params DbParameter[][] parameters) => Execute<int>(sqls, CommandType.Text, ExecuteMode.NonQuery, parameters);

        public int ExecuteNonQueryWithProc(string sql, params DbParameter[] parameter) => ExecuteNonQueryWithProc(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public int[] ExecuteNonQueryWithProc(string[] sqls, params DbParameter[][] parameters) => Execute<int>(sqls, CommandType.StoredProcedure, ExecuteMode.NonQuery, parameters);

        public T ExecuteScalar<T>(string sql, params DbParameter[] parameter) => ExecuteNonQuery<T>(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public T[] ExecuteNonQuery<T>(string[] sqls, params DbParameter[][] parameters) => Execute<T>(sqls, CommandType.Text, ExecuteMode.Scalar, parameters);

        public T ExecuteScalarWithProc<T>(string sql, params DbParameter[] parameter) => ExecuteNonQuery<T>(new string[] { sql }, new DbParameter[][] { parameter })[0];

        public T[] ExecuteNonQueryWithProc<T>(string[] sqls, params DbParameter[][] parameters) => Execute<T>(sqls, CommandType.StoredProcedure, ExecuteMode.Scalar, parameters);

        enum ExecuteMode
        {
            NonQuery, Scalar
        }

        private DataTable[] Fill(string[] selectSqls, CommandType commandType = CommandType.Text, params DbParameter[][] parameters)
        {
            ThrowExceptionIfLengthNotEqual(selectSqls, parameters);
            using (DbConnection connection = DbProviderFactory.CreateConnection())
            using (DbDataAdapter adapter = DbProviderFactory.CreateDataAdapter())
            using (DbCommand command = DbProviderFactory.CreateCommand())
            {
                connection.ConnectionString = ConnectionString;
                connection.Open();
                command.Connection = connection;
                command.CommandType = commandType;
                adapter.SelectCommand = command;
                List<DataTable> resultList = new List<DataTable>();
                for (int i = 0; i < selectSqls.Length; i++)
                {
                    command.CommandText = selectSqls[i];
                    if (parameters.GetLength(0) != 0)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddRange(parameters[i]);
                    }
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    resultList.Add(table);
                }
                return resultList.ToArray();
            }
        }

        public DataTable Fill(string selectSql, params DbParameter[] parameter) => Fill(new string[] { selectSql }, new DbParameter[][] { parameter })[0];

        public DataTable[] Fill(string[] selectSqls, params DbParameter[][] parameters) => Fill(selectSqls, CommandType.Text, parameters);

        public DataTable FillWithProc(string selectSql, params DbParameter[] parameter) => FillWithProc(new string[] { selectSql }, new DbParameter[][] { parameter })[0];

        public DataTable[] FillWithProc(string[] selectSqls, params DbParameter[][] parameters) => Fill(selectSqls, CommandType.StoredProcedure, parameters);
        #endregion
    }
}

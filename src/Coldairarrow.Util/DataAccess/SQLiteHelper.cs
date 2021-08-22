using EFCore.Sharding;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Microsoft.Data.Sqlite;

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
            if (schemaName.IsNullOrEmpty())
                schemaName = "dbo";

            string sql = @"select name as [TableName] from sqlite_master where type='table' order by name; ";
            return GetListBySql<DbTableInfo>(sql);
        }

        /// <summary>
        /// 通过连接字符串和表名获取数据库表的信息
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<TableInfo> GetDbTableInfo(string tableName)
        {
            string sql = @$"pragma table_info('{tableName}')";
            //实在不知道怎么一把获取到TableInfo,只好按返回的建立一个类来获取然后转接一下了。
            var sqliteInfo = GetListBySql<SQLiteTableInfo>(sql);
            return sqliteInfo.Select(p => new TableInfo() { Name = p.name, Type = p.type, IsKey = p.pk, IsNullable = p.notnull, ColumnId = p.cid }).ToList();
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


        public class SQLiteTableInfo
        {
            public int cid { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public bool notnull { get; set; }
            public bool pk { get; set; }
        }
    }
}

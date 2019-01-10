using DB.DatabaseConnectionSQLServer;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DB.DatabaseModification
{
    class DBmodify
    {
        /// <summary>
        /// Oracle查询列名对应的索引名
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static bool DBOracleFieldToIndex(string tableName, string fieldName,ref string indexName)
        {
            if (OracleCompareTable(tableName) == true)
            {
                string sql = @"SELECT INDEX_NAME FROM USER_IND_COLUMNS WHERE TABLE_NAME = '"+tableName+"' AND COLUMN_NAME='"+fieldName+"'";
                return DBconnectionOracle.ExecuteSql(sql,ref indexName);
            }
            return false;
        }

        /// <summary>
        /// 为Oracle数据库某个表的某个字段删除一个非聚集索引
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBOracleDeleteIndex(string indexName, string tableName, string fieldName, ref string sql)
        {
            if (OracleCompareTable(tableName) == true)
            {
                sql = @"DROP INDEX " + indexName;
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
            //create index toys_color_i on toys(color);
        }

        /// <summary>
        /// Oracle查询表中索引数目
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexNum"></param>
        /// <returns></returns>
        public static bool DBOracleCountIndex( string tableName, ref string indexNum)
        {
            if (OracleCompareTable(tableName) == true)
            {
                string sql = @"select count(*) from all_ind_columns a, all_indexes b where a.index_name=b.index_name and a.table_name = upper('" + tableName + "')";
                return DBconnectionOracle.ExecuteSql(sql, ref indexNum);
            }
            return false;
        }

        /// <summary>
        /// SQLServer查询表中索引数目
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="indexNum"></param>
        /// <returns></returns>
        public static bool DBSqlCountIndex(string DBName,string tableName,ref string indexNum)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                string sql = @"USE "+DBName+ "; SELECT count(*) FROM sysindexes  a JOIN   sysindexkeys b   ON a.id = b.id   AND a.indid = b.indid  JOIN sysobjects   c ON   b.id = c.id  JOIN syscolumns   d ON   b.id = d.id   AND b.colid = d.colid  WHERE a.indid NOT IN(0, 255)  AND c.name = '"+tableName+"';";
                return DBconnectionSQLServer.ExecuteSql(sql, ref indexNum);
            }
            return false;
        }


        /// <summary>
        /// SQLServer查询列名对应的索引名
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static bool DBSqlFieldToIndex(string DBName,string tableName, string fieldName, ref string indexName)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                string sql = @"USE " + DBName + "; SELECT indexname = a.name FROM sysindexes  a JOIN   sysindexkeys b   ON a.id = b.id   AND a.indid = b.indid  JOIN sysobjects   c ON   b.id = c.id  JOIN syscolumns   d ON   b.id = d.id   AND b.colid = d.colid  WHERE a.indid NOT IN(0, 255)  AND c.name = '"+tableName+"' AND d.name = '"+fieldName+"';";
                return DBconnectionSQLServer.ExecuteSql(sql, ref indexName);
            }
            return false;
        }

        /// <summary>
        /// 为SQLServer数据库某个表的某个字段删除一个非聚集索引
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="indexName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQLDeleteIndex(string DBName,string indexName, string tableName, string fieldName, ref string sql)
        {
            if (SQLCompareTable(DBName,tableName) == true)
            {
                sql = @"USE " + DBName + "; DROP INDEX " +tableName+"." + indexName+";";
                return DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
            //create index toys_color_i on toys(color);
        }

        /// <summary>
        /// 为Oracle数据库某个表的某个字段添加一个非聚集索引
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBOracleAddIndex(string indexName, string tableName, string fieldName, ref string sql)
        {
            if (OracleCompareTable(tableName) == true)
            {
                sql = @"CREATE INDEX " + indexName + " ON " + tableName + "(" + fieldName + ")";
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
            //create index toys_color_i on toys(color);
        }

        /// <summary>
        /// 为sql数据库某个表的某个字段添加一个非聚集索引
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="indexName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQLAddIndex(string DBName,string indexName,string tableName, string fieldName,ref string sql)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                sql= @"USE " + DBName + "; CREATE INDEX " + indexName + " ON " + tableName + "(" + fieldName + ");";
                return DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
            //create index toys_color_i on toys(color);
         }

        /// <summary>
        /// 为sql数据库中某个表添加一个字段
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldLength"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQLAddField(string DBName, string tableName, string fieldName, string fieldType,string fieldLength, ref string sql)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                if (fieldType != "VARCHAR")
                { sql = @"USE " + DBName + "; ALTER TABLE " + tableName + " ADD " + fieldName + " " + fieldType + ";"; }
                else
                { sql = @"USE " + DBName + "; ALTER TABLE " + tableName + " ADD " + fieldName + " " + fieldType + "(" + fieldLength + ");"; }
                return DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
        }

        /// <summary>
        /// 为sql数据库中某个表删除一个字段
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQLDeleteField(string DBName, string tableName, string fieldName,  ref string sql)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                sql = @"USE " + DBName + "; ALTER TABLE " + tableName + " DROP COLUMN " + fieldName + ";";
                return DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
        }

        /// <summary>
        /// 为sql数据库中某个表修改一个字段
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldLength"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQLModifyField(string DBName, string tableName, string fieldName, string fieldType, string fieldLength, ref string sql)
        {
            if (SQLCompareTable(DBName, tableName) == true)
            {
                if (fieldType != "VARCHAR")
                {
                    sql = @"USE " + DBName + "; ALTER TABLE " + tableName + " ALTER COLUMN " + fieldName + " " + fieldType + ";";
                } 
                else
                {
                    sql = @"USE " + DBName + "; ALTER TABLE " + tableName + " ALTER COLUMN " + fieldName + " " + fieldType + "(" + fieldLength + ");";
                }
                return DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
        }


        /// <summary>
        /// 为Oracle数据库中某个表添加一个字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldLength"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBOracleAddField(string tableName, string fieldName, string fieldType, string fieldLength, ref string sql)
        {
            if (OracleCompareTable(tableName) == true)
            {
                if (fieldType != "VARCHAR2")
                { sql = @"ALTER TABLE " + tableName + " ADD " + fieldName + " " + fieldType; }
                else
                { sql = @"ALTER TABLE " + tableName + " ADD " + fieldName + " " + fieldType + "(" + fieldLength + ")"; }
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
        }

        /// <summary>
        /// 为Oracle数据库中某个表删除一个字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBOracleDeleteField( string tableName, string fieldName, ref string sql)
        {
            if (OracleCompareTable(tableName) == true)
            {
                sql = @"ALTER TABLE " + tableName + " DROP COLUMN " + fieldName;
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
        }

        /// <summary>
        /// 为Oracle数据库中某个表修改一个字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldLength"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBOracleModifyField( string tableName, string fieldName, string fieldType, string fieldLength, ref string sql)
        {
            if (OracleCompareTable(tableName) == true)
            {
                if (fieldType != "VARCHAR2")
                {
                    //ALTER TABLE tableName modify(columnName 类型);
                    sql = @"ALTER TABLE " + tableName + " MODIFY(" + fieldName + " " + fieldType+")";
                }
                else
                {
                    sql = @"ALTER TABLE " + tableName + " MODIFY(" + fieldName + " " + fieldType + "(" + fieldLength + "))";
                }
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
        }



        /// <summary>
        /// 读取指定数据库中所有表名 与 配置文件中的表名作比较
        /// 如果没找到则调用连接类的方法,将配置文件中的表创建到指定数据库中
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <param name="infoLists"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool DBSQlAddTable(string DBName,string tableName,List<ParsedInfo> infoLists,ref string sql)
        {
            if (SQLCompareTable( DBName, tableName) == false)
            {
                sql = @"USE "+DBName+"; CREATE TABLE " + tableName + "(";
                foreach (ParsedInfo info in infoLists)
                {
                    string value = info.TypeInfo.Values.First().ToString();
                    if (info.TypeInfo.Keys.First().ToString() != "VARCHAR")
                    { sql = sql + info.Column + " " + info.TypeInfo.Keys.First().ToString() + ","; }
                    else
                    { sql = sql + info.Column + " " + info.TypeInfo.Keys.First().ToString() + "(" + info.TypeInfo.Values.First().ToString() + ")" + ","; }
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += ");";
                return  DBconnectionSQLServer.ExecuteSql(sql);
            }
            return false;
        }

        public static bool DBOrcaleAddTable(string tableName, List<ParsedInfo> infoLists,ref string sql)
        {
           
            if (OracleCompareTable(tableName) == false)
            {
                sql = @"CREATE TABLE " + tableName + "(";
                foreach (ParsedInfo info in infoLists)
                {
                    string value = info.TypeInfo.Values.First().ToString();
                    if (info.TypeInfo.Keys.First().ToString() != "VARCHAR2")
                    { sql = sql + info.Column + " " + info.TypeInfo.Keys.First().ToString() + ","; }
                    else
                    { sql = sql + info.Column + " " + info.TypeInfo.Keys.First().ToString() + "(" + info.TypeInfo.Values.First().ToString() + ")" + ","; }
                }
                sql = sql.Substring(0, sql.Length - 1);
                sql += ")";
                return DBconnectionOracle.ExecuteSql(sql);
            }
            return false;
        }


        /// <summary>
        /// 读取指定数据库中所有表名 与 指定表名做对比
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool SQLCompareTable( string DBName,string tableName)
        {
            //读取指定数据库中所有表名 与 配置文件中的表名作比较
            DataTable dt = DBconnectionSQLServer.ShowTableList(DBName);
            foreach(DataRow row in dt.Rows)
            {
                if (row["name"].ToString() != "tempdb")
                {
                    if (tableName == row["name"].ToString())
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static bool OracleCompareTable(string tableName)
        {
            DataTable dt = DBconnectionOracle.ShowTableList();
            foreach (DataRow row in dt.Rows)
            {
                if (row["table_name"].ToString() != "tempdb")
                {
                    if (tableName == row["table_name"].ToString())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}

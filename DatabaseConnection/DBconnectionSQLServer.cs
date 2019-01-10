using DB.DatabaseConnectionSQLServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace DB
{

    class DBconnectionSQLServer
    {
        private static SqlConnection sqlConnection; //表示到 SQL Server 数据库的打开连接
        private static string strConnection; //用于初始化 SqlConnection 新实例的包含连接信息的字符串
        private string userId; //连接信息：用户id
        private string password; //连接信息：用户密码
        private string server; //连接信息：服务器地址（本地/远程）

        /// <summary>
        /// 用于连接SQLServer的工具类
        /// </summary>
        /// <param name="userId">连接信息：用户id</param>
        /// <param name="password">连接信息：用户密码</param>
        /// <param name="server">连接信息：服务器地址（本地/远程）</param>
        public DBconnectionSQLServer(string userId, string password, string server)
        {
            this.userId = userId;
            this.password = password;
            this.server = server;
            strConnection = "user id=" + userId + ";password=" + password + ";Server=" + server + ";Connect Timeout=10";
            sqlConnection = new SqlConnection(strConnection);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        public static bool OpenConnect()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State.ToString().ToLower() == "open")
                {
                    Console.Write("已经连接到数据库");
                    return false;
                }
                else
                {
                    try
                    {
                        sqlConnection.Open();
                        Console.Write("成功打开连接");
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public static void CloseConnect()
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State.ToString().ToLower() == "open")
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                    Console.Write("成功关闭连接");
                }
            }
        }

        /// <summary>
        /// 获取所有数据库名
        /// </summary>
        /// <returns></returns>
        public static DataTable ShowDatabaseList()
        {
            //定义SQL查询语句
            string sql = @"Select Name FROM Master.dbo.SysDatabases ORDER BY Name;";
            SqlCommand cmd = new SqlCommand(sql, sqlConnection);
            //获得数据集
            SqlDataReader sRead = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sRead);
            sRead.Close();
            return dt;
        }

        /// <summary>
        /// 获取指定数据库所有表名
        /// </summary>
        /// <param name="DBName"></param>
        /// <returns></returns>
        public static DataTable ShowTableList(string DBName)
        {
            String sql = @"USE " + DBName + "; select name from sysobjects where xtype='U' order by name;";
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader sRead = null;
            //获得数据集
            sRead=cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sRead);
            sRead.Close();
            
            return dt;
        }

        /// <summary>
        /// 显示指定表单
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static DataTable ShowTable(string DBName, string TableName)
        {
            string sql = @"USE " + DBName + "; SELECT * FROM " + TableName + ";";
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader sRead = null;
            //获得数据集
            DataTable dt = new DataTable();



            // DataSet ds = new DataSet();
            //SqlCommand cmd = sqlConnection.CreateCommand();
            //string sql = @"USE " + DBName + "; SELECT * FROM " + TableName + ";";
            //获得数据集
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            try
            {
                sRead = cmd.ExecuteReader();
               
                dt.Load(sRead);
                sRead.Close();
                //adapter.Fill(ds);
            }
            catch (SqlException)
            {
                MessageBox.Show("数据格式有误，请尝试重新连接。", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //System.Threading.Thread.CurrentThread.Abort();
            }
            return dt;
            // return ds;
        }

        /// <summary>
        /// 存取某个表的所有字段名和字段类型
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="TableName"></param>
        /// <param name="lists"></param>
        public static void GetNndT(string DBName,string TableName, ref List<DBField> lists)
        {
            List<string> indexs = FindIndexs(DBName,TableName);
            string sql = @"USE " + DBName+ "; select COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS t where t.TABLE_NAME =  '" + TableName+"';";
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    string type = reader.GetString(1);
                    string length = (reader.IsDBNull(2)) ?  "": Convert.ToString(reader.GetInt32(2));

                    DBField field = new DBField();
                    field.Name = name;
                    field.Type = type;
                    field.Length = length;
                    field.IsIndex = indexs.Find(x => x == name) != null ? true : false;

                    lists.Add(field);
                }
            }
            reader.Close();
        }

        /// <summary>
        /// 查找所有索引字段
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<string> FindIndexs(string DBName,string tableName)
        {
            List<string> indexs = new List<string>();
            string sql = @"USE " + DBName+" ;SELECT colname=d.name FROM   sysindexes  a  JOIN   sysindexkeys   b   ON   a.id=b.id   AND   a.indid=b.indid  JOIN   sysobjects   c   ON   b.id=c.id  JOIN   syscolumns   d   ON   b.id=d.id   AND   b.colid=d.colid  WHERE   a.indid   NOT IN(0,255)  AND   c.name='"+tableName+"'";
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    indexs.Add(name);
                }
            }
            reader.Close();
            return indexs;

        }

        /// <summary>
        /// 执行特定sql语句；有返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql, ref string result)
        {
            if (sql != null && sql.Length != 0)
            {
                try
                {
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = sql;
                    SqlDataReader sRead = cmd.ExecuteReader();
                    if(sRead.Read())
                    {
                        result = sRead.GetValue(0).ToString();
                    } 
                    sRead.Close();

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

            }
            return false;

        }


        /// <summary>
        /// 执行特定sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql)
        {
            if(sql!=null&&sql.Length!=0)
            {
                try
                {
                    SqlCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                
            }
            return false;
        }

        /// <summary>
        /// 现在采用另一种方法，见DBmodify类。查询数据库中是否有特定表
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="tableName"></param>
        /// <returns>返回1表示有，0表示没有</returns>
        public static bool findTable(string DBName,string tableName)
        {
            string sql = @"use " + DBName + "; select count(*) from sysobjects where id = object_id('" + tableName + "');";
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            //获得数据集
            SqlDataReader sRead = cmd.ExecuteReader();
            string isInTable = sRead.ToString();
            sRead.Close();
            if (isInTable == tableName)
                return false;
            else
                return true;
        }



    }
}

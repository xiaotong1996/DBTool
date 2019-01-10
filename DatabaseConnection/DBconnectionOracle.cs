using DB.DatabaseConnectionSQLServer;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB
{
    class DBconnectionOracle
    {
        private static OracleConnection sqlConnection; //表示到 SQL Server 数据库的打开连接
        private static string strConnection; //用于初始化 SqlConnection 新实例的包含连接信息的字符串
        private string userId; //连接信息：用户id
        private string password; //连接信息：用户id
        private string server; //连接信息：服务器地址（本地/远程）

        /// <summary>
        /// 用于连接Oracle的工具类
        /// </summary>
        /// <param name="userId">连接信息：用户id</param>
        /// <param name="password">连接信息：用户id</param>
        /// <param name="server">连接信息：服务器地址（本地/远程）</param>
        public DBconnectionOracle(string userId, string password, string server)
        {
            this.userId = userId;
            this.password = password;
            this.server = server;
            strConnection = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = "+server + ")(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = orcl)));Persist Security Info = true;User Id = " + userId+"; Password = "+password+";";
            sqlConnection = new OracleConnection(strConnection);

            
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
                    catch(Exception e)
                    {
                        Console.Write(e);
                        return false;
                    }
                }
            }
            else
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
        /// 获取所有表名
        /// </summary>
        /// <returns></returns>
        public static DataTable ShowTableList()
        {
            //DataSet ds = new DataSet();
            string sql = "select * from user_tables order by table_name";
            OracleCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;

            OracleDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            return dt;
        }

        /// <summary>
        /// 显示指定表单
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static DataTable ShowTable(string TableName)
        {
            DataTable dt = new DataTable();
            try {
            OracleCommand cmd = sqlConnection.CreateCommand();
            string sql = @"SELECT * FROM " + TableName;
            cmd.CommandText = sql;
            OracleDataReader reader = cmd.ExecuteReader();
            
            dt.Load(reader);
            reader.Close();
            
            }
            //DataSet ds = new DataSet();
            
            //获得数据集
            //OracleDataAdapter sAdapter = new OracleDataAdapter(cmd);//sAdapter.Fill(ds);
              
            catch (SqlException)
            {
                MessageBox.Show("数据格式有误，请尝试重新连接。", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //System.Threading.Thread.CurrentThread.Abort();
            }
            //return ds;
            return dt;
        }


        /// <summary>
        /// 存取某个表的所有字段名和字段类型
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="lists"></param>
        public static void GetNndT( string TableName, ref List<DBField> lists)
        {
            List<string> indexs = FindIndexs(TableName);
            string sql = @"select column_name, data_type, DATA_LENGTH from user_tab_columns where table_name = '" + TableName+"'";
            OracleCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            OracleDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0).ToUpper();
                    string type = reader.GetString(1);
                    string length = Convert.ToString(reader.GetInt64(2));

                    DBField field = new DBField();
                    field.Name = name;
                    field.Type = type;
                    field.Length = length;
                    field.IsIndex = indexs.Find(x => x == name)!=null ? true:false;

                    lists.Add(field);
                }
            }
            reader.Close();

        }



        /// <summary>
        /// 查找所有索引字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<string> FindIndexs(string tableName)
        {
            List<string> indexs = new List<string>();
            string sql = @"select a.column_name from all_ind_columns a, all_indexes b where a.index_name=b.index_name and a.table_name = upper('"+tableName+"') order by a.table_name";
            OracleCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            OracleDataReader reader = cmd.ExecuteReader();

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
        /// 执行特定sql语句;无返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql)
        {
            if (sql != null && sql.Length != 0)
            {
                try
                {
                    OracleCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    return true;
                } catch(Exception e)
                {
                    MessageBox.Show(e.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                
            }
            return false;
        }

        /// <summary>
        /// 执行特定sql语句；有返回值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql,ref string result)
        {
            if (sql != null && sql.Length != 0)
            {
                try
                {
                    OracleCommand cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = sql;
                    OracleDataReader sRead = cmd.ExecuteReader();
                    if (sRead.Read())
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
        /// 找到特定表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool findTable( string tableName)
        {
            string sql = @"select table_name from user_tables where table_name like '"+tableName+"'";
            OracleCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = sql;
            //获得数据集
            OracleDataReader sRead = cmd.ExecuteReader();
            string isInTable = sRead.ToString();
            sRead.Close();
            if (isInTable == "0")
                return false;
            else
                return true;
        }
    }
}


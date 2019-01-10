using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DB
{
    using Forms;
    using Properties;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml.Linq;

    public partial class FormLogin : Form
    {
        //FormMainTableShow formMain = new FormMainTableShow();

        //static bool isShown=false;
        static string path = Directory.GetCurrentDirectory();
       // static File infoFile = Resources.ResourceManager.GetObject("ConnectInfo.bin") as file;

        private DBconnectionSQLServer dbConnection;
        private DBconnectionOracle oracleConnection;
        private static Dictionary<string, ConfigerManger> configs = new Dictionary<string, ConfigerManger>();
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        //  读取配置文件寻找记住的服务器，用户名和密码
             FileStream fs = new FileStream(path+"\\data1.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                
                configs = bf.Deserialize(fs) as Dictionary<string, ConfigerManger>;

                foreach (ConfigerManger conf in configs.Values)
                {
                    this.serverBox.Items.Add(conf.Server);
                }

                for (int i = 0; i < configs.Count; i++)
                {
                    if (this.serverBox.Text != "")
                    {
                        if (configs.ContainsKey(this.serverBox.Text))
                        {
                            this.userIdBox.Text = configs[this.serverBox.Text].UserId;
                            this.pwdBox.Text = configs[this.serverBox.Text].Pwd;
                            this.DBShowBox.Text = configs[this.serverBox.Text].DbName;
                            this.remberpwdCB.Checked = true;
                        }
                    }
                }
            }
            fs.Close();
            //  用户名默认选中第一个
            if (this.serverBox.Items.Count > 0)
            {
                this.serverBox.SelectedIndex = this.serverBox.Items.Count - 1;
            }
            
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string userId = userIdBox.Text;
            string pwd = pwdBox.Text;
            string server = serverBox.Text;
            string serverType = fwqTypeBox.Text;
            string dbName = DBShowBox.Text;

            if (userId.Length == 0 || server.Length == 0 || pwd.Length == 0 || fwqTypeBox.Text.Length == 0)
            {
                MessageBox.Show("请填写完整信息！", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FileStream fs = new FileStream(path + "\\data1.bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                ConfigerManger conf = new ConfigerManger();
                conf.UserId = userId;
                conf.Server = server;
                conf.ServerType = serverType;
                if (serverType != "SQLServer")
                    conf.DbName = "";
                else
                    conf.DbName = dbName;

                if (remberpwdCB.Checked)       //  如果单击了记住密码的功能
                {   //  在文件中保存密码
                conf.Pwd = pwd;
                }
                else
                {   //  不在文件中保存密码
                conf.Pwd = "";
                }

                //  选在集合中是否存在用户名 
                if (configs.ContainsKey(conf.Server))
                {
                configs.Remove(conf.Server);
                }
                configs.Add(conf.Server, conf);
                //要先将User类先设为可以序列化(即在类的前面加[Serializable])
                bf.Serialize(fs, configs);
                //user.Password = this.PassWord.Text;
                fs.Close();

                switch (serverType)
                {
                    case "SQLServer":
                        {
                           // dbConnection = new DBconnectionSQLServer(userId: userId, password: pwd, server: server);
                            dbConnection = new DBconnectionSQLServer(userId, pwd, server);
                            if (DBconnectionSQLServer.OpenConnect())
                            {
                                this.Hide();
                                //this.TopLevel = false;
                                FormMainTableShow formMain = new FormMainTableShow();
                                formMain.DbName = dbName;
                                formMain.DbType = serverType;
                               
                                formMain.Tag = this;
                                formMain.Show();
                                   

                            }
                            else
                            {
                                MessageBox.Show("连接失败，信息有误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                        }
                    case "Oracle":
                        {
                            oracleConnection = new DBconnectionOracle(userId, pwd, server);
                            if (DBconnectionOracle.OpenConnect())
                            {
                                this.Hide();
                                //this.TopLevel = false;
                                FormMainTableShow formMain = new FormMainTableShow();
                                formMain.DbName = dbName;
                                formMain.DbType = serverType;
                                formMain.Tag = this;
                                formMain.Show();
                                //formMain.Parent = this;
                            }
                            else
                            {
                                MessageBox.Show("连接失败，信息有误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            break;
                        }
                }
                

            }

            }

        


        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                pwdBox.UseSystemPasswordChar = false;
            else
                pwdBox.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void serverBox_SelectedValueChanged(object sender, EventArgs e)
        {

            //  首先读取记住密码的配置文件
            FileStream fs = new FileStream(path + "\\data1.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();

                configs = bf.Deserialize(fs) as Dictionary<string, ConfigerManger>;

                for (int i = 0; i < configs.Count; i++)
                {
                    if (serverBox.Text != "")
                    {
                        if (configs.ContainsKey(serverBox.Text) && configs[serverBox.Text].UserId != "" && configs[serverBox.Text].Pwd != "" &&configs[serverBox.Text].ServerType!="")
                        {
                            userIdBox.Text = configs[serverBox.Text].UserId;
                            pwdBox.Text = configs[serverBox.Text].Pwd;
                            fwqTypeBox.Text = configs[serverBox.Text].ServerType;
                            if(configs[serverBox.Text].ServerType=="SQLServer")
                                DBShowBox.Text = configs[serverBox.Text].DbName;
                            remberpwdCB.Checked = true;
                        }
                        else
                        {
                            userIdBox.Text = "";
                            pwdBox.Text = "";
                            fwqTypeBox.Text = "";
                            remberpwdCB.Checked = false;
                        }
                    }
                }
                
            }
            fs.Close();
        }

        private void checkDBButton_Click(object sender, EventArgs e)
        {
            string userId = userIdBox.Text;
            string pwd = pwdBox.Text;
            string server = serverBox.Text;
            string serverType = fwqTypeBox.Text;
            if(userId.Length!=0&&pwd.Length!=0&&server.Length!=0&&serverType.Length!=0)
            {
                dbConnection = new DBconnectionSQLServer(userId, pwd, server);
                if (DBconnectionSQLServer.OpenConnect())
                {
                    DBShowBox.ForeColor = System.Drawing.Color.Black;
                    DataTable dt = DBconnectionSQLServer.ShowDatabaseList();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["name"].ToString() != "tempdb")
                        {
                            DBShowBox.Items.Add(row["name"].ToString());
                        }
                    }
                    DBShowBox.SelectedItem = DBShowBox.Items[0];
                    DBconnectionSQLServer.CloseConnect();
                }
                else
                {
                    MessageBox.Show("连接失败，信息有误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                DBShowBox.ForeColor = System.Drawing.Color.Red;
                DBShowBox.Text = "请先填写完整信息!";
            }
            

            
        }

        private void fwqTypeBox_TextChanged(object sender, EventArgs e)
        {
            if(fwqTypeBox.Text!="SQLServer")
            {
                checkDBButton.Enabled = false;
                DBShowBox.Text = "无需检索,请直接连接";
            }
            else
            {
                checkDBButton.Enabled = true;
                DBShowBox.Text = "请先检索选择数据库,再连接";
            }
        }

        private void DBShowBox_TextChanged(object sender, EventArgs e)
        {
            if (DBShowBox.Text == "请先检索选择数据库,再连接")
                button1.Enabled= false;
            else
                button1.Enabled = true;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            serverBox.Text = "";
            fwqTypeBox.SelectedIndex = fwqTypeBox.Items.Count - 1;
            userIdBox.Clear();
            pwdBox.Clear();
            remberpwdCB.Checked = false;
        }
    }
}

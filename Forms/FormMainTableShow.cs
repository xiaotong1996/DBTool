using DB.DatabaseModification;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB.Forms
{
    public partial class FormMainTableShow : Form
    {

        FormXMLExecute formxml;
        bool justBack = false;
        private string dbType;
        private string dbName;
        public string DbType
        {
            get
            {
                return dbType;
            }

            set
            {
                dbType = value;
            }
        }

        public string DbName
        {
            get
            {
                return dbName;
            }

            set
            {
                dbName = value;
            }
        }

        


        public FormMainTableShow()
        {
            InitializeComponent();
        }

        private void FormMainOrcl_Load(object sender, EventArgs e)
        {
            DataTable dt = (DbType == "Oracle")?DBconnectionOracle.ShowTableList():DBconnectionSQLServer.ShowTableList(DbName);
            foreach (DataRow row in dt.Rows)
            {
                int index = TableView.Rows.Add();
                TableView.Rows[index].Cells[0].Value = (DbType == "Oracle") ? row["table_name"].ToString():row["name"].ToString();
            }
            if (TableView.CurrentCell != null && TableView.CurrentCell.Value != null)
            {
                DataTable ds = (DbType == "Oracle") ? DBconnectionOracle.ShowTable(TableView.CurrentCell.Value.ToString()) : DBconnectionSQLServer.ShowTable(DbName, TableView.CurrentCell.Value.ToString());

                try
                {
                    DataView.DataSource = ds;//表从起始行显示在dataGridView里
                }
                catch (Exception)
                {
                    MessageBox.Show("数据未能正确获得,请重试", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    System.Threading.Thread.CurrentThread.Abort();
                    //DBView.CurrentCell = DBView.Rows[0].Cells[0];
                }
            }
        }

        private void FormMainOrcl_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(justBack==false)
            {
                if (DbType == "Oracle")
                    DBconnectionOracle.CloseConnect();
                else if (DbType == "SQLServer")
                    DBconnectionSQLServer.CloseConnect();
                Application.Exit();
            }
            
        }

        private void TableView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (TableView.CurrentCell != null && TableView.CurrentCell.Value != null)
            {
                DataTable ds = (DbType == "Oracle") ? DBconnectionOracle.ShowTable( TableView.CurrentCell.Value.ToString()):DBconnectionSQLServer.ShowTable(DbName, TableView.CurrentCell.Value.ToString());
                
                try
                {
                    DataView.DataSource = ds;//表从起始行显示在dataGridView里
                }
                catch (Exception)
                {
                    MessageBox.Show("数据未能正确获得,请重试", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    System.Threading.Thread.CurrentThread.Abort();
                    //DBView.CurrentCell = DBView.Rows[0].Cells[0];
                }
            }
        }

        private void chooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\
            openFileDialog.Filter = "(xml文件)|*.xml";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath.Text = openFileDialog.FileName;
            }
            else
            {
                filePath.Text = "";
            }

            if (filePath.Text != "" && filePath.Text != "请先选择一个XML配置文件！")
            {
                if (DbType == "Oracle")
                {
                    formxml = new FormXMLExecute("*Oracle*", filePath.Text);
                    formxml.FormClosing += new FormClosingEventHandler(this.FromXMLExecute_FormClosing);
                    formxml.Show();
                }
                else if (DbType == "SQLServer")
                {
                    formxml = new FormXMLExecute(DbName, filePath.Text);
                    formxml.FormClosing += new FormClosingEventHandler(this.FromXMLExecute_FormClosing);
                    formxml.Show();
                }

            }
            else
            {
                filePath.Text = "请先选择一个XML配置文件！";
            }
        }


        private void quit_Click(object sender, EventArgs e)
        {
            if (DbType == "Oracle")
                DBconnectionOracle.CloseConnect();
            else if (DbType == "SQLServer")
                DBconnectionSQLServer.CloseConnect();
            justBack = true;
            FormLogin login = (FormLogin)this.Tag;
            login.Show();
            this.Close();
        }

        private void TableView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void filePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                //this.filePath.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void filePath_DragDrop(object sender, DragEventArgs e)
        {
            //GetValue(0) 为第1个文件全路径
            //DataFormats 数据的格式，下有多个静态属性都为string型，除FileDrop格式外还有Bitmap,Text,WaveAudio等格式
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            filePath.Text = path;
            //this.filePath.Cursor = System.Windows.Forms.Cursors.IBeam; //还原鼠标形状


            if (filePath.Text != "" && filePath.Text != "请先选择一个XML配置文件！")
            {
                if (DbType == "Oracle")
                {
                    formxml = new FormXMLExecute("*Oracle*", filePath.Text);
                    formxml.FormClosing += new FormClosingEventHandler(this.FromXMLExecute_FormClosing);
                    formxml.Show();
                }
                else if (DbType == "SQLServer")
                {
                    formxml = new FormXMLExecute(DbName, filePath.Text);
                    formxml.FormClosing += new FormClosingEventHandler(this.FromXMLExecute_FormClosing);
                    formxml.Show();
                }

            }
            else
            {
                filePath.Text = "请先选择一个XML配置文件！";
            }
        }

        private void FromXMLExecute_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Do your stuff here.
            TableView.Rows.Clear();
            int findIndex = 0;
            DataTable dt = (DbType == "Oracle") ? DBconnectionOracle.ShowTableList() : DBconnectionSQLServer.ShowTableList(DbName);
            foreach (DataRow row in dt.Rows)
            {
                
                int index = TableView.Rows.Add();
                TableView.Rows[index].Cells[0].Value = (DbType == "Oracle") ? row["table_name"].ToString() : row["name"].ToString();
                string tableName = formxml.tableName;
                if(tableName!="")
                {
                    if((string)TableView.Rows[index].Cells[0].Value==tableName)
                    {
                        findIndex = index;
                    }
                }
            }
            TableView.CurrentCell = TableView.Rows[findIndex].Cells[0];

            /*
            if (TableView.CurrentCell != null && TableView.CurrentCell.Value != null)
            {
                DataSet ds = (DbType == "Oracle") ? DBconnectionOracle.ShowTable(TableView.CurrentCell.Value.ToString()) : DBconnectionSQLServer.ShowTable(DbName, TableView.CurrentCell.Value.ToString());

                try
                {
                    DataView.DataSource = ds.Tables[0].DefaultView;//表从起始行显示在dataGridView里
                }
                catch (Exception)
                {
                    MessageBox.Show("数据未能正确获得,请重试", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    System.Threading.Thread.CurrentThread.Abort();
                    //DBView.CurrentCell = DBView.Rows[0].Cells[0];
                }
            }*/

        }
        
    }
}

using DB.DatabaseConnectionSQLServer;
using DB.DatabaseModification;
using DB.ParsingTool;
using DB.UndoTool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace DB.Forms
{
    public partial class FormXMLExecute : Form
    {
        /// <summary>
        /// 字段缺失
        /// </summary>
        private static Color colorLack = ColorTranslator.FromHtml("#d9534f");
        /// <summary>
        /// 字段一致
        /// </summary>
        private static Color colorSame = ColorTranslator.FromHtml("#ffffff");
        /// <summary>
        /// 字段差异
        /// </summary>
        private static Color colorDiff = ColorTranslator.FromHtml("#f0ad4e");
        //鼠标所在行的背景色
        //private static Color colorBG = ColorTranslator.FromHtml("#E6E6E6");

        private string DBName;
        private string filePath;

        public string tableName = "";
        /// <summary>
        /// 存储xml解析出来的信息
        /// </summary>
        List<ParsedInfo> infoLists = new List<ParsedInfo>();

        /// <summary>
        /// 存储数据库中查询到信息
        /// </summary>
        List<DBField> dbLists = new List<DBField>();

        /// <summary>
        /// 存储sql语句用于导出
        /// </summary>
        List<string> sqlcommands = new List<string>();

        /// <summary>
        /// 用栈存储sql指令，用于撤销操作
        /// </summary>
        Stack<RowRecord> rowRecords = new Stack<RowRecord>();

        /// <summary>
        /// 存储运行结果，现实在messagebox中
        /// </summary>
        string showResults = "";

        /// <summary>
        /// 指示两个表是否完全一致
        /// </summary>
        bool allSame = true;
        /// <summary>
        /// 指示是否正在刷新xml
        /// </summary>
        static bool xmlshowing = false;
        /// <summary>
        /// 指示是否只改变索引
        /// </summary>
        //static bool justindex = false;
        /// <summary>
        /// 指示是否正在删除一行
        /// </summary>
        static bool deleting = false;
        /// <summary>
        /// 指示是否在重新刷新界面
        /// </summary>
        static bool refreshing = false;

        public FormXMLExecute(string DBName, string filePath)
        {
            this.DBName = DBName;
            this.filePath = filePath;
            InitializeComponent();
        }

        /// <summary>
        /// 将xml中的数据按DB中的排序，对应排序
        /// </summary>
        private void SortDGV()
        {
            List<ParsedInfo> infoListsNew = new List<ParsedInfo>();
            foreach (DBField db in dbLists)
            {
                if (infoLists.Exists(x => x.Column == db.Name))
                {
                    infoListsNew.Add(infoLists.Find(x => x.Column == db.Name));
                } else
                {
                    infoListsNew.Add(new ParsedInfo());
                }
            }
            foreach (ParsedInfo p in infoLists)
            {
                if (!infoListsNew.Exists(x => x.Column == p.Column))
                {
                    infoListsNew.Add(p);
                    dbLists.Add(new DBField());
                }
            }
            infoLists = infoListsNew;
        }


        /// <summary>
        /// 将infoLists中的字段名和默认字段类型显示在dataXML数据显示框中
        /// </summary>
        private void fillDataXML()
        {
            refreshing = true;
            xmlshowing = true;
            dataXML.Rows.Clear();
            if (infoLists != null)
            {

                foreach (ParsedInfo info in infoLists)
                {
                    if (info == null || info.Column == null)
                    {
                        DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                        DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                        DataGridViewTextBoxCell lengthCell = new DataGridViewTextBoxCell();
                        DataGridViewRow row = new DataGridViewRow();
                        if (DBName == "*Oracle*")
                        {
                            Dictionary<string, string> typeLists = MappingTool.GetAllTypesOracle();
                            List<string> showTypes = new List<string>();
                            foreach (string key in typeLists.Keys)
                            {
                                showTypes.Add(key);
                            }
                            typeCell.DataSource = showTypes;
                        }
                        else
                        {
                            Dictionary<string, string> typeLists = MappingTool.GetAllTypesSQL();
                            List<string> showTypes = new List<string>();
                            foreach (string key in typeLists.Keys)
                            {
                                showTypes.Add(key);
                            }
                            typeCell.DataSource = showTypes;
                        }
                        row.Cells.Add(nameCell);
                        row.Cells.Add(typeCell);
                        row.Cells.Add(lengthCell);

                        dataXML.Rows.Add(row);
                    }
                    else
                    {
                        DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                        nameCell.Value = info.Column;


                        DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();

                        List<string> showTypes = new List<string>();
                        foreach (string key in info.TypeInfo.Keys)
                        {
                            showTypes.Add(key);
                        }
                        typeCell.DataSource = showTypes;
                        //设置默认值
                        typeCell.Value = (showTypes != null) ? showTypes[0].ToUpper() : "";

                        DataGridViewTextBoxCell lengthCell = new DataGridViewTextBoxCell();
                        lengthCell.Value = info.TypeInfo[showTypes[0]].ToString();

                        DataGridViewRow row = new DataGridViewRow();
                        row.Cells.Add(nameCell);
                        row.Cells.Add(typeCell);
                        row.Cells.Add(lengthCell);

                        dataXML.Rows.Add(row);
                    }
                }
            }
            xmlshowing = false;
            refreshing = false;
        }

        /// <summary>
        /// 将dbLists中的字段名和默认字段类型显示在dataDB数据显示框中
        /// </summary>
        private void fillDataDB()
        {
            refreshing = true;
            dataDB.Rows.Clear();
            if (dbLists != null)
            {
                foreach (DBField list in dbLists)
                {
                    if (list == null || list.Name == null)
                    {

                        DataGridViewRow row = new DataGridViewRow();


                        dataDB.Rows.Add(row);

                    }
                    else
                    {
                        DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                        nameCell.Value = list.Name;

                        DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell();
                        typeCell.Value = list.Type.ToUpper();

                        DataGridViewTextBoxCell lengthCell = new DataGridViewTextBoxCell();
                        lengthCell.Value = list.Length;

                        DataGridViewCheckBoxCell indexCell = new DataGridViewCheckBoxCell();
                        indexCell.Value = list.IsIndex;

                        DataGridViewRow row = new DataGridViewRow();
                        row.Cells.Add(nameCell);
                        row.Cells.Add(typeCell);
                        row.Cells.Add(lengthCell);
                        row.Cells.Add(indexCell);

                        dataDB.Rows.Add(row);
                    }

                }
            }
            refreshing = false;
        }

        /// <summary>
        /// 初始化时同步界面时，将xml的索引字段与db的索引字段同步
        /// </summary>
        private void SynchronizeIndex()
        {
            foreach (DataGridViewRow rowXML in dataXML.Rows)
            {
                foreach (DataGridViewRow rowDB in dataDB.Rows)
                {
                    if (rowDB.Cells[0].Value != null && rowXML.Cells[0].Value != null)
                    {
                        if (rowXML.Cells[0].EditedFormattedValue.ToString().ToUpper() == rowDB.Cells[0].Value.ToString().ToUpper())
                        {
                            rowXML.Cells[3].Value = ((bool)rowDB.Cells[3].Value == true) ? true : false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在DB列中找XML的字段，作比较
        /// 比较两个视图中列表，显示出不同
        /// </summary>
        private void compareLists1()
        {

            foreach (DataGridViewRow rowXML in dataXML.Rows)
            {
                bool findCell = false;
                foreach (DataGridViewRow rowDB in dataDB.Rows)
                {
                    if (rowDB.Cells[0].Value != null && rowXML.Cells[0].Value != null)
                    {
                        //若字段名相等比较其他
                        if (rowXML.Cells[0].EditedFormattedValue.ToString().ToUpper() == rowDB.Cells[0].Value.ToString().ToUpper())
                        {
                            findCell = true;
                            //若字段长度为0
                            if (rowXML.Cells[2].EditedFormattedValue == null)
                            {
                                //若字段长度为0，若数据库字段同样为0，判断索引列
                                if (rowDB.Cells[2].Value == null)
                                {
                                    if (rowDB.Cells[3].Value == rowXML.Cells[3].Value)
                                    {
                                        rowXML.Cells[0].Style.BackColor = colorSame;
                                        rowXML.Cells[1].Style.BackColor = colorSame;
                                        rowDB.Cells[1].Style.BackColor = colorSame;
                                        rowDB.Cells[0].Style.BackColor = colorSame;
                                        rowXML.Cells[2].Style.BackColor = colorSame;
                                        rowDB.Cells[2].Style.BackColor = colorSame;
                                        rowXML.Cells[3].Style.BackColor = colorSame;
                                        rowDB.Cells[3].Style.BackColor = colorSame;
                                        rowXML.Tag = "Same";
                                        rowDB.Tag = "Same";

                                    }
                                    else
                                    {
                                        rowXML.Cells[0].Style.BackColor = colorDiff;
                                        rowXML.Cells[1].Style.BackColor = colorDiff;
                                        rowDB.Cells[1].Style.BackColor = colorDiff;
                                        rowDB.Cells[0].Style.BackColor = colorDiff;
                                        rowXML.Cells[2].Style.BackColor = colorDiff;
                                        rowDB.Cells[2].Style.BackColor = colorDiff;
                                        rowXML.Cells[3].Style.BackColor = colorDiff;
                                        rowDB.Cells[3].Style.BackColor = colorDiff;
                                        rowDB.Tag = "Diff_justindex";
                                        rowXML.Tag = "Diff_justindex";
                                        // justindex = true;
                                    }
                                }
                                else//若字段长度为0，若数据库字段步为0，有差异
                                {
                                    rowXML.Cells[0].Style.BackColor = colorDiff;
                                    rowXML.Cells[1].Style.BackColor = colorDiff;
                                    rowDB.Cells[1].Style.BackColor = colorDiff;
                                    rowDB.Cells[0].Style.BackColor = colorDiff;
                                    rowXML.Cells[2].Style.BackColor = colorDiff;
                                    rowDB.Cells[2].Style.BackColor = colorDiff;
                                    rowXML.Cells[3].Style.BackColor = colorDiff;
                                    rowDB.Cells[3].Style.BackColor = colorDiff;
                                    rowDB.Tag = "Diff";
                                    rowXML.Tag = "Diff";
                                    // justindex = false;
                                }
                            }
                            else//若字段长度不为0
                            {
                                //比较字段类型和字段长度
                                if (rowXML.Cells[1].EditedFormattedValue.ToString().ToUpper() == rowDB.Cells[1].Value.ToString().ToUpper() && rowXML.Cells[2].EditedFormattedValue.ToString().ToUpper() == rowDB.Cells[2].Value.ToString().ToUpper())
                                {
                                    //相等则比较索引，若都相等，设为一致
                                    if (rowXML.Cells[3].EditedFormattedValue.Equals(rowDB.Cells[3].Value))
                                    {
                                        rowXML.Cells[0].Style.BackColor = colorSame;
                                        rowXML.Cells[1].Style.BackColor = colorSame;
                                        rowDB.Cells[1].Style.BackColor = colorSame;
                                        rowDB.Cells[0].Style.BackColor = colorSame;
                                        rowXML.Cells[2].Style.BackColor = colorSame;
                                        rowDB.Cells[2].Style.BackColor = colorSame;
                                        rowXML.Cells[3].Style.BackColor = colorSame;
                                        rowDB.Cells[3].Style.BackColor = colorSame;
                                        rowXML.Tag = "Same";
                                        rowDB.Tag = "Same";

                                    }
                                    else
                                    {

                                        //相等则比较索引，若索引不相等，设为差异
                                        rowXML.Cells[0].Style.BackColor = colorDiff;
                                        rowXML.Cells[1].Style.BackColor = colorDiff;
                                        rowDB.Cells[1].Style.BackColor = colorDiff;
                                        rowDB.Cells[0].Style.BackColor = colorDiff;
                                        rowXML.Cells[2].Style.BackColor = colorDiff;
                                        rowDB.Cells[2].Style.BackColor = colorDiff;
                                        rowXML.Cells[3].Style.BackColor = colorDiff;
                                        rowDB.Cells[3].Style.BackColor = colorDiff;
                                        rowDB.Tag = "Diff_justindex";
                                        rowXML.Tag = "Diff_justindex";
                                        //justindex = true;

                                    }
                                }
                                else//比较字段类型和字段长度,不相等设为差异
                                {
                                    rowXML.Cells[0].Style.BackColor = colorDiff;
                                    rowXML.Cells[1].Style.BackColor = colorDiff;
                                    rowDB.Cells[1].Style.BackColor = colorDiff;
                                    rowDB.Cells[0].Style.BackColor = colorDiff;
                                    rowXML.Cells[2].Style.BackColor = colorDiff;
                                    rowDB.Cells[2].Style.BackColor = colorDiff;
                                    rowXML.Cells[3].Style.BackColor = colorDiff;
                                    rowDB.Cells[3].Style.BackColor = colorDiff;
                                    rowDB.Tag = "Diff";
                                    rowXML.Tag = "Diff";
                                    //  justindex = false;
                                }

                            }
                        }
                    }
                }
                if (findCell == false && rowXML.Index != dataXML.Rows.Count - 1)
                {
                    //未在DB列中找到对应XML列中的字段，执行操作
                    rowXML.Cells[0].Style.BackColor = colorLack;
                    rowXML.Cells[1].Style.BackColor = colorLack;
                    rowXML.Cells[2].Style.BackColor = colorLack;
                    rowXML.Cells[3].Style.BackColor = colorLack;
                    rowXML.Tag = "Lack";
                }
            }
        }

        /// <summary>
        /// 在XML中，找DB列中的字段，作比较
        /// </summary>
        private void compareLists2()
        {

            foreach (DataGridViewRow rowDB in dataDB.Rows)
            {
                bool findCell = false;
                foreach (DataGridViewRow rowXML in dataXML.Rows)
                {
                    if (rowDB.Cells[0].Value != null && rowXML.Cells[0].Value != null)
                    {
                        if (rowXML.Cells[0].EditedFormattedValue.ToString().ToUpper() == rowDB.Cells[0].Value.ToString().ToUpper())
                        {
                            findCell = true;
                        }
                    }
                }
                if (findCell == false && rowDB.Index != dataDB.Rows.Count - 1)
                {
                    //未在DB列中找到对应XML列中的字段，执行操作
                    rowDB.Cells[0].Style.BackColor = colorLack;
                    rowDB.Cells[1].Style.BackColor = colorLack;
                    rowDB.Cells[2].Style.BackColor = colorLack;
                    rowDB.Cells[3].Style.BackColor = colorLack;
                    rowDB.Tag = "Lack";
                }
            }
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new System.Text.StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }



        /// <summary>
        /// 同步单行sql
        /// </summary>
        /// <param name="row"></param>
        /// <param name="DGV"></param>
        /// <param name="indexList"></param>
        /// <param name="hasChanged"></param>
        /// <returns></returns>
        private bool SynchronizeSQLRow(DataGridViewRow row, string DGV, List<string> indexList, ref bool hasChanged)
        {
            if (DGV == "dataXML")
            {
                DataGridViewRow rowXML = row;
                if (rowXML.Cells[0].Value == null) { return false; }
                string fieldName = rowXML.Cells[0].Value.ToString();
                if (rowXML.Tag.ToString() == "Lack")
                {

                    string fieldType = rowXML.Cells[1].Value.ToString();
                    string fieldLength = (rowXML.Cells[2].Value == null) ? "" : rowXML.Cells[2].Value.ToString();
                    string sql = "";
                    //增加字段
                    if (DBmodify.DBSQLAddField(DBName, tableName, fieldName, fieldType, fieldLength, ref sql))
                    {
                        sqlcommands.Add(sql);
                        showResults = showResults + "增加字段：" + fieldName + "\n";
                        hasChanged = true;
                        // 根据checkbox值，选择是否添加索引
                        if (rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true && !indexList.Contains(fieldName))
                        {
                            string indexNum1 = GenerateRandomCode(6);
                            string indexName1 = tableName + "_INDEX_" + indexNum1;
                            string sql3 = "";
                            if (DBmodify.DBSQLAddIndex(DBName, indexName1, tableName, fieldName, ref sql3))
                            {
                                sqlcommands.Add(sql3);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (rowXML.Tag.ToString().Contains("Diff"))
                {
                    string sql = "";
                    string indexName1 = "";
                    if (rowXML.Tag.ToString() == "Diff_justindex")
                    {
                        if ((rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == false) && indexList.Contains(fieldName))
                        {
                            DBmodify.DBSqlFieldToIndex(DBName, tableName, fieldName, ref indexName1);
                            DBmodify.DBSQLDeleteIndex(DBName, indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "删除索引：" + indexName1 + "\n";
                            hasChanged = true;

                        }
                        else if ((rowXML.Cells[3].Value == null || (bool)rowXML.Cells[3].Value == true) && !indexList.Contains(fieldName))
                        {
                            string indexNum = GenerateRandomCode(6);
                            indexName1 = tableName + "_INDEX_" + indexNum;
                            if (DBmodify.DBSQLAddIndex(DBName, indexName1, tableName, fieldName, ref sql))
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                    }
                    else
                    {

                        string fieldType = rowXML.Cells[1].Value.ToString();
                        string fieldLength;
                        if (rowXML.Cells[2].Value != null)
                        {
                            fieldLength = rowXML.Cells[2].Value.ToString();
                        }
                        else
                        {
                            fieldLength = "";
                        }

                        DBmodify.DBSqlFieldToIndex(DBName, tableName, fieldName, ref indexName1);
                        if ((rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true) && indexList.Contains(fieldName))
                        {
                            DBmodify.DBSQLDeleteIndex(DBName, indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            DBmodify.DBSQLModifyField(DBName, tableName, fieldName, fieldType, fieldLength, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "修改字段：" + fieldName + "\n";
                            hasChanged = true;
                            DBmodify.DBSQLAddIndex(DBName, indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                        }
                        else if (rowXML.Cells[3].Value == null || (bool)rowXML.Cells[3].Value == false && indexList.Contains(fieldName))
                        {
                            DBmodify.DBSQLDeleteIndex(DBName, indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "删除索引：" + indexName1 + "\n";
                            hasChanged = true;
                            if (DBmodify.DBSQLModifyField(DBName, tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                        }
                        else if ((rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true) && !indexList.Contains(fieldName))
                        {
                            if (DBmodify.DBSQLModifyField(DBName, tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                            string indexNum = GenerateRandomCode(6);
                            indexName1 = tableName + "_INDEX_" + indexNum;
                            if (DBmodify.DBSQLAddIndex(DBName, indexName1, tableName, fieldName, ref sql))
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                        else if ((rowXML.Cells[3].Value != null || (bool)rowXML.Cells[3].Value == false) && !indexList.Contains(fieldName))
                        {
                            if (DBmodify.DBSQLModifyField(DBName, tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                        }
                    }
                }
            }
            else if (DGV == "dataDB")
            {
                DataGridViewRow rowDB = row;
                if (rowDB.Tag.ToString() == "Lack")
                {
                    string fieldNameDB = rowDB.Cells[0].Value == null ? "" : rowDB.Cells[0].Value.ToString();
                    string sql = "";
                    string indexName = "";
                    string sql2 = "";
                    if ((bool)rowDB.Cells[3].Value == true && indexList.Contains(fieldNameDB))
                    {
                        if (DBmodify.DBSqlFieldToIndex(DBName, tableName, fieldNameDB, ref indexName))
                        {
                            if (DBmodify.DBSQLDeleteIndex(DBName, indexName, tableName, fieldNameDB, ref sql2))
                            {
                                sqlcommands.Add(sql2);
                                showResults = showResults + "删除索引：" + indexName + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (DBmodify.DBSQLDeleteField(DBName, tableName, fieldNameDB, ref sql))
                    {
                        sqlcommands.Add(sql);
                        showResults = showResults + "删除字段：" + fieldNameDB + "\n";
                        hasChanged = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 同步SQLServer
        /// </summary>
        /// <returns></returns>
        private bool SynchronizeSQL()
        {
            List<string> indexList = DBconnectionSQLServer.FindIndexs(DBName, tableName);
            bool hasChanged = false;
            foreach (DataGridViewRow rowXML in dataXML.Rows)
            {
                if (rowXML.Index == dataXML.RowCount - 1) { break; }
                if (rowXML.Cells[0].Value == null) { continue; }
                if (SynchronizeSQLRow(rowXML, "dataXML", indexList, ref hasChanged) == false)
                {
                    return false;
                }

            }
            foreach (DataGridViewRow rowDB in dataDB.Rows)
            {
                if (rowDB.Cells[0].Value == null) { continue; }
                if (SynchronizeSQLRow(rowDB, "dataDB", indexList, ref hasChanged) == false)
                {
                    return false;
                }

            }
            if (hasChanged == false)
            {
                allSame = true;
            }
            else
            {
                allSame = false;
            }
            return true;

        }

        /// <summary>
        /// 同步Oracle数据库中的单行
        /// </summary>
        /// <param name="row"></param>
        /// <param name="DGV"></param>
        /// <param name="hasChanged"></param>
        private bool SynchronizeOracleRow(DataGridViewRow row, string DGV, List<string> indexList, ref bool hasChanged)
        {
            if (DGV == "dataXML")
            {
                DataGridViewRow rowXML = row;
                if (rowXML.Cells[0].Value == null) { return false; }
                string fieldName = rowXML.Cells[0].Value.ToString();
                if (rowXML.Tag.ToString() == "Lack")
                {

                    string fieldType = rowXML.Cells[1].Value.ToString();
                    string fieldLength = (rowXML.Cells[2].Value == null) ? "" : rowXML.Cells[2].Value.ToString();
                    string sql = "";
                    //增加字段
                    if (DBmodify.DBOracleAddField(tableName, fieldName, fieldType, fieldLength, ref sql))
                    {
                        sqlcommands.Add(sql);
                        showResults = showResults + "增加字段：" + fieldName + "\n";
                        hasChanged = true;
                        // 根据checkbox值，选择是否添加索引
                        if (rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true && !indexList.Contains(fieldName))
                        {
                            string indexNum1 = GenerateRandomCode(6);
                            string indexName1 = tableName + "_INDEX_" + indexNum1;
                            string sql3 = "";
                            if (DBmodify.DBOracleAddIndex(indexName1, tableName, fieldName, ref sql3))
                            {
                                sqlcommands.Add(sql3);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (rowXML.Tag.ToString().Contains("Diff"))
                {
                    string sql = "";
                    string indexName1 = "";
                    if (rowXML.Tag.ToString() == "Diff_justindex")
                    {
                        if ((rowXML.Cells[3].Value == null || (bool)rowXML.Cells[3].Value == false) && indexList.Contains(fieldName))
                        {
                            DBmodify.DBOracleFieldToIndex(tableName, fieldName, ref indexName1);
                            DBmodify.DBOracleDeleteIndex(indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "删除索引：" + indexName1 + "\n";
                            hasChanged = true;

                        }
                        else if (rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true && !indexList.Contains(fieldName))
                        {
                            string indexNum = GenerateRandomCode(6);
                            indexName1 = tableName + "_INDEX_" + indexNum;
                            if (DBmodify.DBOracleAddIndex(indexName1, tableName, fieldName, ref sql))
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                    }
                    else
                    {

                        string fieldType = rowXML.Cells[1].Value.ToString();
                        string fieldLength;
                        if (rowXML.Cells[2].Value != null)
                        {
                            fieldLength = rowXML.Cells[2].Value.ToString();
                        }
                        else
                        {
                            fieldLength = "";
                        }

                        DBmodify.DBOracleFieldToIndex(tableName, fieldName, ref indexName1);
                        if (rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true && indexList.Contains(fieldName))
                        {
                            DBmodify.DBOracleDeleteIndex(indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            DBmodify.DBOracleModifyField(tableName, fieldName, fieldType, fieldLength, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "修改字段：" + fieldName + "\n";
                            hasChanged = true;
                            DBmodify.DBOracleAddIndex(indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                        }
                        else if ((rowXML.Cells[3].Value == null || (bool)rowXML.Cells[3].Value == false) && indexList.Contains(fieldName))
                        {
                            DBmodify.DBOracleDeleteIndex(indexName1, tableName, fieldName, ref sql);
                            sqlcommands.Add(sql);
                            showResults = showResults + "删除索引：" + indexName1 + "\n";
                            hasChanged = true;
                            if (DBmodify.DBOracleModifyField(tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                        }
                        else if (rowXML.Cells[3].Value != null && (bool)rowXML.Cells[3].Value == true && !indexList.Contains(fieldName))
                        {
                            if (DBmodify.DBOracleModifyField(tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                            string indexNum = GenerateRandomCode(6);
                            indexName1 = tableName + "_INDEX_" + indexNum;
                            if (DBmodify.DBOracleAddIndex(indexName1, tableName, fieldName, ref sql))
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "增加索引：" + indexName1 + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                        else if ((rowXML.Cells[3].Value == null || (bool)rowXML.Cells[3].Value == false) && !indexList.Contains(fieldName))
                        {
                            if (DBmodify.DBOracleModifyField(tableName, fieldName, fieldType, fieldLength, ref sql) && rowXML.Tag.ToString() == "Diff")
                            {
                                sqlcommands.Add(sql);
                                showResults = showResults + "修改字段：" + fieldName + "\n";
                                hasChanged = true;
                            }
                            else
                            { return false; }
                        }
                    }
                }
            }
            else if (DGV == "dataDB")
            {
                DataGridViewRow rowDB = row;
                if (rowDB.Tag.ToString() == "Lack")
                {
                    if (rowDB.Cells[0].Value == null) { return false; }
                    string fieldNameDB = rowDB.Cells[0].Value.ToString();
                    string sql = "";
                    string indexName = "";
                    string sql2 = "";
                    if ((bool)rowDB.Cells[3].Value == true && indexList.Contains(fieldNameDB))
                    {
                        if (DBmodify.DBOracleFieldToIndex(tableName, fieldNameDB, ref indexName))
                        {
                            if (DBmodify.DBOracleDeleteIndex(indexName, tableName, fieldNameDB, ref sql2))
                            {
                                sqlcommands.Add(sql2);
                                showResults = showResults + "删除索引：" + indexName + "\n";
                                hasChanged = true;
                            }
                            else { return false; }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if (DBmodify.DBOracleDeleteField(tableName, fieldNameDB, ref sql))
                    {
                        sqlcommands.Add(sql);
                        showResults = showResults + "删除字段：" + fieldNameDB + "\n";
                        hasChanged = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 同步全部Oracle
        /// </summary>
        /// <returns></returns>
        private bool SynchronizeOracle()
        {
            List<string> indexList = DBconnectionOracle.FindIndexs(tableName);
            bool hasChanged = false;
            foreach (DataGridViewRow rowXML in dataXML.Rows)
            {
                if (rowXML.Index == dataXML.RowCount - 1) { break; }
                if (rowXML.Cells[0].Value == null) { continue; }
                if (SynchronizeOracleRow(rowXML, "dataXML", indexList, ref hasChanged) == false)
                {
                    return false;
                }
            }
            foreach (DataGridViewRow rowDB in dataDB.Rows)
            {
                if (rowDB.Cells[0].Value == null) { continue; }
                if (SynchronizeOracleRow(rowDB, "dataDB", indexList, ref hasChanged) == false)
                {
                    return false;
                }

            }
            if (hasChanged == false)
            {
                allSame = true;
            }
            else
            {
                allSame = false;
            }
            return true;

        }



        private static bool loadFinish = false;

        /// <summary>
        /// 页面登录时根据有无在数据库中查询到xml中的表，显示添加/同步界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormXMLExecute_Load(object sender, EventArgs e)
        {

            if (DBName == "*Oracle*")
            {
                if (XMLParsing.ParserInfoOracle(filePath, ref infoLists, ref tableName))
                {
                    if (tableName != "")
                    {
                        tableName = tableName.ToUpper();
                        this.Text = "Tofflon开发小工具 处理表：" + tableName;
                    }

                    if (DBmodify.OracleCompareTable(tableName))
                    {
                        tipe.Visible = false;
                        addButton.Text = "同步";
                        //exportSqlButton.Visible = true;
                        //addButton.Enabled = false;
                        //compareButton.Enabled = true;
                        DBconnectionOracle.GetNndT(tableName, ref dbLists);
                        SortDGV();
                        fillDataXML();
                        fillDataDB();
                        SynchronizeIndex();
                        compareLists1();
                        compareLists2();

                    }
                    else
                    {
                        fillDataXML();
                        tipe.Visible = true;
                        addButton.Text = "添加";
                        //exportSqlButton.Visible = false;
                        //addButton.Enabled = true;
                        // compareButton.Enabled = false;
                    }
                    loadFinish = true;

                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                //调用方法将解析xml出来的配置信息存到内存。
                if (XMLParsing.ParserInfoSQL(filePath, ref infoLists, ref tableName))
                {
                    if (tableName != "")
                    {
                        tableName = tableName.ToUpper();
                        this.Text = "Tofflon开发小工具 处理表：" + tableName;
                    }
                    //在当前DB中判断该配置信息对应的表是否已存在
                    if (DBmodify.SQLCompareTable(DBName, tableName))
                    {
                        //表已存在
                        //显示完整界面，用于执行类比操作
                        tipe.Visible = false;
                        //addButton.Enabled = false;
                        //compareButton.Enabled = true;
                        addButton.Text = "同步";
                        //exportSqlButton.Visible = true;

                        //在一边显示xml字段
                        //1.将infoLists中的字段名和默认字段类型显示在dataXML数据显示框中
                        //在另半边显示数据库字段
                        DBconnectionSQLServer.GetNndT(DBName, tableName, ref dbLists);
                        SortDGV();
                        fillDataXML();
                        fillDataDB();
                        SynchronizeIndex();
                        compareLists1();
                        compareLists2();

                    }
                    else
                    {
                        //表不存在
                        //将配置文件中的表加入数据库
                        //DBmodify.DBSQlAddTable(DBName, filePath);
                        //1.将infoLists中的字段名和默认字段类型显示在dataXML数据显示框中
                        fillDataXML();

                        //2.在另半边显示界面提示“数据库中无此表！点击添加按钮添加表”
                        tipe.Visible = true;
                        addButton.Text = "添加";
                        //exportSqlButton.Visible = false;
                        //addButton.Enabled = true;
                        //compareButton.Enabled = false;

                        //3. 见addButton_Click

                    }

                    loadFinish = true;
                }
                else
                {
                    this.Close();
                }
            }
        }



        /// <summary>
        /// 以更清晰的方式在messagebox中显示运行结果
        /// </summary>
        /// <returns></returns>
        private string EditResults()
        {
            string addString = "";
            string deleteString = "";
            string modifyString = "";
            string addIndexString = "";
            string deleteIndexString = "";
            string[] Results = showResults.Split('\n');
            foreach (string result in Results)
            {
                if (result.Contains("增加字段"))
                {
                    addString = addString + result.Substring(5) + "\n";
                }
                else if (result.Contains("删除字段"))
                {
                    deleteString = deleteString + result.Substring(5) + "\n";
                }
                else if (result.Contains("修改字段"))
                {
                    modifyString = modifyString + result.Substring(5) + "\n";
                }
                else if (result.Contains("增加索引"))
                {
                    addIndexString = addIndexString + result.Substring(5) + "\n";
                }
                else if (result.Contains("删除索引"))
                {
                    deleteIndexString = deleteIndexString + result.Substring(5) + "\n";
                }
            }


            string newResult = "增加字段：\n";
            if (addString == "")
            {
                newResult += "无\n";
            }
            else
            {
                newResult += addString;
            }
            newResult += "删除字段：\n";
            if (deleteString == "")
            {
                newResult += "无\n";
            }
            else
            {
                newResult += deleteString;
            }
            newResult += "修改字段：\n";
            if (modifyString == "")
            {
                newResult += "无\n";
            }
            else
            {
                newResult += modifyString;
            }
            newResult += "增加索引：\n";
            if (addIndexString == "")
            {
                newResult += "无\n";
            }
            else
            {
                newResult += addIndexString;
            }
            newResult += "删除索引：\n";
            if (deleteIndexString == "")
            {
                newResult += "无\n";
            }
            else
            {
                newResult += deleteIndexString;
            }
            return newResult;

        }

        /// <summary>
        /// 添加/同步表的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            dataXML.CurrentCell = dataXML.Rows[dataXML.Rows.Count - 1].Cells[0];
            if (addButton.Text == "添加")
            {
                //存储建表信息
                List<ParsedInfo> infoListsNew = new List<ParsedInfo>();
                //存储建索引信息
                List<string> infoIndex = new List<string>();
                try
                {
                    for (int rowNum = 0; rowNum < dataXML.RowCount - 1; rowNum++)
                    {
                        DataGridViewRow row = dataXML.Rows[rowNum];
                        ParsedInfo infoNew = new ParsedInfo();
                        infoNew.Column = row.Cells[0].Value.ToString();
                        infoNew.TypeInfo = new Dictionary<string, string>();
                        string length = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                        infoNew.TypeInfo.Add(row.Cells[1].Value.ToString(), length);
                        infoListsNew.Add(infoNew);
                        DataGridViewCheckBoxCell chkchecking = row.Cells[3] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chkchecking.Value) == true)
                        {
                            infoIndex.Add(row.Cells[0].Value.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (DBName == "*Oracle*")
                {
                    string sql = "";
                    tableName = tableName.ToUpper();
                    if (DBmodify.DBOrcaleAddTable(tableName, infoListsNew, ref sql))
                    {
                        sqlcommands.Add(sql);

                        showResults = showResults + "添加索引：\n";
                        for (int i = 0; i < infoIndex.Count(); i++)
                        {
                            string indexName = tableName + "_INDEX_" + GenerateRandomCode(6);
                            string sql1 = "";
                            if (DBmodify.DBOracleAddIndex(indexName, tableName, infoIndex[i], ref sql1))
                            {
                                sqlcommands.Add(sql1);
                                showResults = showResults + indexName + "\n";
                            }
                            else
                            {
                                MessageBox.Show("添加" + indexName + "索引失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        if (infoIndex.Count() == 0)
                        {
                            showResults = showResults + "添加索引：无\n";
                        }
                        exportSqlButton.Enabled = true;
                        MessageBox.Show("添加" + tableName + "表成功\n" + showResults, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        showResults = "";

                        tipe.Visible = false;
                        addButton.Text = "同步";
                        DBconnectionOracle.GetNndT(tableName, ref dbLists);
                        fillDataDB();
                        UpdateInfoLists();
                        SortDGV();
                        fillDataXML();
                        SynchronizeIndex();
                        compareLists1();
                        compareLists2();
                    }
                    else
                    {
                        MessageBox.Show("添加" + tableName + "表失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    string sql = "";
                    tableName = tableName.ToUpper();
                    if (DBmodify.DBSQlAddTable(DBName, tableName, infoListsNew, ref sql))
                    {
                        sqlcommands.Add(sql);

                        showResults = showResults + "添加索引：\n";
                        for (int i = 0; i < infoIndex.Count(); i++)
                        {
                            string indexName = tableName + "_INDEX_" + GenerateRandomCode(6);
                            string sql1 = "";
                            if (DBmodify.DBSQLAddIndex(DBName, indexName, tableName, infoIndex[i], ref sql1))
                            {
                                sqlcommands.Add(sql1);
                                showResults = showResults + indexName + "\n";
                            }
                            else
                            {
                                MessageBox.Show("添加" + indexName + "索引失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        if (infoIndex.Count() == 0)
                        {
                            showResults = showResults + "添加索引：无\n";
                        }
                        exportSqlButton.Enabled = true;
                        MessageBox.Show("添加" + tableName + "表至" + DBName + "数据库成功\n" + showResults, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        showResults = "";

                        tipe.Visible = false;
                        addButton.Text = "同步";
                        DBconnectionSQLServer.GetNndT(DBName, tableName, ref dbLists);
                        fillDataDB();
                        UpdateInfoLists();
                        SortDGV();
                        fillDataXML();
                        SynchronizeIndex();
                        compareLists1();
                        compareLists2();

                    }
                    else
                    {
                        MessageBox.Show("添加" + tableName + "表至" + DBName + "数据库失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
            else if (addButton.Text == "同步")
            {
                if (DBName == "*Oracle*")
                {
                    if (SynchronizeOracle())
                    {
                        if (allSame == true)
                        {
                            MessageBox.Show("已全部同步，无需再次同步！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            exportSqlButton.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(EditResults(), "同步成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dbLists.Clear();
                            DBconnectionOracle.GetNndT(tableName, ref dbLists);
                            fillDataDB();
                            UpdateInfoLists();
                            SortDGV();
                            fillDataXML();
                            SynchronizeIndex();
                            compareLists1();
                            compareLists2();
                            showResults = "";
                            exportSqlButton.Enabled = true;
                        }


                    }
                    else
                    {

                        MessageBox.Show("同步失败，请重试！", "同步失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (SynchronizeSQL())
                    {
                        if (allSame == true)
                        {
                            MessageBox.Show("已全部同步，无需再次同步！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            exportSqlButton.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show(EditResults(), "同步成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            dbLists.Clear();
                            DBconnectionSQLServer.GetNndT(DBName, tableName, ref dbLists);
                            fillDataDB();
                            UpdateInfoLists();
                            SortDGV();
                            fillDataXML();
                            SynchronizeIndex();
                            compareLists1();
                            compareLists2();
                            showResults = "";
                            exportSqlButton.Enabled = true;
                        }

                    }
                    else
                    {

                        MessageBox.Show("同步失败，请重试！", "同步失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                    }
                }
            }
        }

        /// <summary>
        /// 同步完成后，将新的xml更新之前的infolists
        /// </summary>
        private void UpdateInfoLists()
        {
            refreshing = true;
            infoLists.Clear();
            //List<int> records = new List<int>();

            foreach (DataGridViewRow row in dataXML.Rows)
            {
                if (row.Index == dataXML.Rows.Count - 1) { break; }
                ParsedInfo info = new ParsedInfo();
                if ((row.Cells[0].Value != null && row.Cells[0].Value.ToString() != ""))
                {
                    info.Column = row.Cells[0].Value.ToString();
                    info.Name = row.Cells[0].Value.ToString();
                    info.TypeInfo = new Dictionary<string, string>();
                    info.TypeInfo.Add(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString());
                    infoLists.Add(info);
                }

            }
            refreshing = false;
        }

        /// <summary>
        /// 退出窗口返回上级页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
        private void compareButton_Click(object sender, EventArgs e)
        {
            compareLists1();
            compareLists2();
        }*/

        /// <summary>
        /// 增加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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


        /*
        private void dataDB_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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
        }*/



        /// <summary>
        /// 实现导出sql语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportSqlButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "TXT|*.txt";
            saveFileDialog1.Title = "选择导出sql记录的存储路径";


            //System.DateTime currentTime = new System.DateTime();
            DateTime.Now.ToString("f");
            string strY = DateTime.Now.ToString("f"); //不显示秒
            string fileName = Regex.Replace(strY, @":", ".");
            if (DBName == "*Oracle*")
            {
                fileName = fileName + " (Oracle)" + tableName;
            }
            else
            {
                fileName = fileName + " (" + DBName + ")" + tableName;
            }


            saveFileDialog1.FileName = fileName;//设置默认文件名
            saveFileDialog1.RestoreDirectory = true;

            saveFileDialog1.ShowDialog();



            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the  
                // File type selected in the dialog box.  
                // NOTE that the FilterIndex property is one-based.  
                StreamWriter sw = new StreamWriter(fs);
                foreach (string result in sqlcommands)
                {
                    sw.WriteLine(result);
                }

                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();

                fs.Close();

            }



        }

        /// <summary>
        /// 同步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType().Equals(typeof(DataGridViewComboBoxEditingControl)))//cell为类ComboBox时
            {
                e.CellStyle.BackColor = Color.FromName("window");
                DataGridViewComboBoxEditingControl editingControl = e.Control as DataGridViewComboBoxEditingControl;
                editingControl.SelectedIndexChanged += new EventHandler(EditingTB_TypeChanged);
            }
            else if (e.Control.GetType().Equals(typeof(DataGridViewTextBoxEditingControl)))//cell为类TextBox时
            {
                e.CellStyle.BackColor = Color.FromName("window");
                DataGridViewTextBoxEditingControl editingControl = e.Control as DataGridViewTextBoxEditingControl;
                editingControl.TextChanged += new EventHandler(EditingTB_LengthChanged);
                editingControl.TextChanged += CheckNames_NameChanged;
            }


            //设置第一列自动转换为大写
            DataGridView dgv = (DataGridView)sender;
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl editingControl = (DataGridViewTextBoxEditingControl)e.Control;
                if (dgv.CurrentCell.OwningColumn.Name == "fieldName")
                {
                    editingControl.CharacterCasing = CharacterCasing.Upper;
                }
                else
                {
                    editingControl.CharacterCasing = CharacterCasing.Normal;
                }
            }


        }
        /// <summary>
        /// 自定义事件，用于改变字段名称时作出检查和刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckNames_NameChanged(object sender, EventArgs e)
        {

            int colIndex = dataXML.CurrentCell.ColumnIndex;
            if (colIndex == 0)
            {
                if (addButton.Text == "同步")
                {
                    compareLists1();
                    compareLists2();
                }
            }

        }


        /// <summary>
        /// 自定义事件，用于改选字段类型时同步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditingTB_TypeChanged(object sender, EventArgs e)
        {

            string type = dataXML.CurrentCell.EditedFormattedValue.ToString();
            int rowIndex = dataXML.CurrentCell.RowIndex;
            // if (!type.Contains("VARCHAR"))
            // {
            if (DBName == "*Oracle*")
            {
                string length = MappingTool.OracletypeToLength(type);
                dataXML.Rows[rowIndex].Cells[2].Value = length;
            }
            else
            {
                // string type = dataXML.Rows[e.RowIndex].Cells[1].Value.ToString();
                string length = MappingTool.SQltypeToLength(type);
                dataXML.Rows[rowIndex].Cells[2].Value = length;
            }
            //  }

            if (addButton.Text == "同步")
            {
                compareLists1();
                compareLists2();
            }
        }
        /// <summary>
        /// 自定义事件，用于改选字段长度时同步刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditingTB_LengthChanged(object sender, EventArgs e)
        {
            int rowIndex = dataXML.CurrentCell.ColumnIndex;
            if (rowIndex == 2)
            {
                if (addButton.Text == "同步")
                {
                    compareLists1();
                    compareLists2();
                }
            }
        }
        /// <summary>
        /// 行增加时触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (deleting != true && refreshing != true&&isUndoing==false)
            {

                dataDB.Rows.Add(new DataGridViewRow());
                if (xmlshowing != true)
                {

                    int rowIndex = e.RowIndex;
                    if (DBName == "*Oracle*")
                    {
                        Dictionary<string, string> typeLists = MappingTool.GetAllTypesOracle();
                        List<string> showTypes = new List<string>();
                        foreach (string key in typeLists.Keys)
                        {
                            showTypes.Add(key);
                        }
                        DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)dataXML.Rows[rowIndex - 1].Cells[1];
                        typeCell.DataSource = showTypes;
                        //设置默认值
                        typeCell.Value = showTypes[0].ToUpper();
                    }
                    else
                    {
                        Dictionary<string, string> typeLists = MappingTool.GetAllTypesSQL();
                        List<string> showTypes = new List<string>();
                        foreach (string key in typeLists.Keys)
                        {
                            showTypes.Add(key);
                        }
                        DataGridViewComboBoxCell typeCell = (DataGridViewComboBoxCell)dataXML.Rows[rowIndex - 1].Cells[1];
                        typeCell.DataSource = showTypes;
                        //设置默认值
                        typeCell.Value = showTypes[0].ToUpper();
                    }

                    if (dataXML.CurrentCell != null && dataXML.CurrentCell.Value != null && dataXML.CurrentCell.ColumnIndex == 0)
                    {
                        string Name = dataXML.CurrentCell.Value.ToString();

                        for (int i = 0; i < dataXML.RowCount - 1; i++)
                        {
                            if (i == rowIndex) { break; }
                            if (dataXML.Rows[i].Cells[0].Value != null && Name == dataXML.Rows[i].Cells[0].Value.ToString())
                            {
                                MessageBox.Show("第" + i + 1 + "行已存在该字段名，请勿添加重复字段名", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                dataXML.CurrentCell.Value = null;
                                dataXML.CurrentCell = dataXML.Rows[i].Cells[0];
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 行删除时触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            
            if (addButton.Text == "同步")
            {
                int rowIndex = e.RowIndex;
                if (refreshing != true && xmlshowing != true)
                {
                    deleting = true;
                    
                    if (dataDB.Rows[rowIndex].Cells[0].Value != null)
                    {

                        DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                        DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                        DataGridViewTextBoxCell lengthCell = new DataGridViewTextBoxCell();
                        DataGridViewRow row = new DataGridViewRow();
                        if (DBName == "*Oracle*")
                        {
                            Dictionary<string, string> typeLists = MappingTool.GetAllTypesOracle();
                            List<string> showTypes = new List<string>();
                            foreach (string key in typeLists.Keys)
                            {
                                showTypes.Add(key);
                            }
                            typeCell.DataSource = showTypes;
                        }
                        else
                        {
                            Dictionary<string, string> typeLists = MappingTool.GetAllTypesSQL();
                            List<string> showTypes = new List<string>();
                            foreach (string key in typeLists.Keys)
                            {
                                showTypes.Add(key);
                            }
                            typeCell.DataSource = showTypes;
                        }
                        row.Cells.Add(nameCell);
                        row.Cells.Add(typeCell);
                        row.Cells.Add(lengthCell);
                        dataXML.Rows.Insert(rowIndex, row);
                    }
                    else
                    {
                        dataDB.Rows.RemoveAt(rowIndex);
                    }

                    compareLists1();
                    compareLists2();
                    deleting = false;
                }
            }
        }
        /// <summary>
        /// 更改索引时触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3 && e.RowIndex != -1)
            {

                if (addButton.Text == "同步")
                {
                    compareLists1();
                    compareLists2();
                }
            }
        }

        /// <summary>
        /// 确认字段名不重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int colIndex = dataXML.CurrentCell.ColumnIndex;
            if (colIndex == 0)
            {
                if (dataXML.CurrentCell.ColumnIndex == 0 && dataXML.CurrentCell.Value != null)
                {
                    string Name = dataXML.CurrentCell.Value.ToString();
                    int rowIndex = dataXML.CurrentCell.RowIndex;
                    for (int i = 0; i < dataXML.RowCount - 1; i++)
                    {
                        if (i == rowIndex) { break; }
                        if (dataXML.Rows[i].Cells[0].Value != null && Name == dataXML.Rows[i].Cells[0].Value.ToString())
                        {
                            int row = i + 1;
                            MessageBox.Show("第" + row + "行已存在该字段名，请勿添加重复字段名", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            dataXML.CurrentCell.Value = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 使两个DGV同步滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_Scroll(object sender, ScrollEventArgs e)
        {
            if (addButton.Text == "同步")
            {
                this.dataDB.FirstDisplayedScrollingRowIndex = this.dataXML.FirstDisplayedScrollingRowIndex;
            }
        }

        private void dataDB_Scroll(object sender, ScrollEventArgs e)
        {
            if (addButton.Text == "同步")
            { this.dataXML.FirstDisplayedScrollingRowIndex = this.dataDB.FirstDisplayedScrollingRowIndex; }
        }

        //保存移入之前的颜色
        //private static Color colorUsed;
        /// <summary>
        /// 鼠标移到哪一行就把它设为所选行，但在鼠标左键点击状态下不响应，为了不与多选行冲突
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (Control.MouseButtons != MouseButtons.Left)
            {
                int rowIndex = e.RowIndex;
                if (rowIndex > -1)
                {
                    if (rowIndex < dataDB.RowCount)
                    {
                        dataDB.CurrentCell = dataDB.Rows[rowIndex].Cells[0];
                    }
                    if (rowIndex < dataXML.RowCount)
                    {
                        dataXML.CurrentCell = dataXML.Rows[rowIndex].Cells[0];
                    }
                }
            } else
            {
                if (addButton.Text == "同步")
                {
                    dataDB.Rows[e.RowIndex].Selected = true;
                }

            }

        }
        /*
        private void dataXML_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex > -1)
            {
                dataXML.Rows[rowIndex].Cells[0].Style.BackColor = colorUsed;
                dataXML.Rows[rowIndex].Cells[1].Style.BackColor = colorUsed;
                dataXML.Rows[rowIndex].Cells[2].Style.BackColor = colorUsed;
                dataXML.Rows[rowIndex].Cells[3].Style.BackColor = colorUsed;
                dataDB.Rows[rowIndex].Cells[0].Style.BackColor = colorUsed;
                dataDB.Rows[rowIndex].Cells[1].Style.BackColor = colorUsed;
                dataDB.Rows[rowIndex].Cells[2].Style.BackColor = colorUsed;
                dataDB.Rows[rowIndex].Cells[3].Style.BackColor = colorUsed;

            }
        }*/
        /// <summary>
        /// 鼠标移到哪一行就把它设为所选行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataDB_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex > -1)
            {
                if (rowIndex < dataDB.RowCount)
                {
                    dataDB.CurrentCell = dataDB.Rows[rowIndex].Cells[0];
                }
                if (rowIndex < dataXML.RowCount)
                {
                    dataXML.CurrentCell = dataXML.Rows[rowIndex].Cells[0];
                }

                /*
                colorUsed = dataXML.Rows[rowIndex].Cells[0].Style.BackColor;
                dataDB.Rows[rowIndex].Cells[0].Style.BackColor = colorBG;
                dataDB.Rows[rowIndex].Cells[1].Style.BackColor = colorBG;
                dataDB.Rows[rowIndex].Cells[2].Style.BackColor = colorBG;
                dataDB.Rows[rowIndex].Cells[3].Style.BackColor = colorBG;
                dataXML.Rows[rowIndex].Cells[0].Style.BackColor = colorBG;
                dataXML.Rows[rowIndex].Cells[1].Style.BackColor = colorBG;
                dataXML.Rows[rowIndex].Cells[2].Style.BackColor = colorBG;
                dataXML.Rows[rowIndex].Cells[3].Style.BackColor = colorBG;
                */
            }
        }

        /// <summary>
        /// 鼠标右键添加删除行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex;
            foreach (DataGridViewRow row in dataXML.SelectedRows)
            {
                rowIndex = row.Index;
                if (rowIndex > -1 && rowIndex < dataXML.Rows.Count - 1)
                {
                    RowRecord r = new RowRecord();
                    if(addButton.Text=="同步")
                    {
                        if (dataDB.Rows[rowIndex] != null && dataDB.Rows[rowIndex].Cells[0].Value != null && dataDB.Rows[rowIndex].Cells[0].Value.ToString() != "")
                        {
                            r.RowIndex = rowIndex;
                            r.Tag = "RemovedFake";
                            r.Name = (row.Cells[0].Value == null) ? "" : row.Cells[0].Value.ToString();
                            r.Type = (row.Cells[1].Value == null) ? "" : row.Cells[1].Value.ToString();
                            r.Length = (row.Cells[2].Value == null) ? "" : row.Cells[2].Value.ToString();
                            r.Index = (row.Cells[3].Value == null) ? false : (bool)row.Cells[3].Value;
                            rowRecords.Push(r);
                        }
                        else
                        {
                            r.RowIndex = rowIndex;
                            r.Tag = "Removed";
                            r.Name = (row.Cells[0].Value == null) ? "" : row.Cells[0].Value.ToString();
                            r.Type = (row.Cells[1].Value == null) ? "" : row.Cells[1].Value.ToString();
                            r.Length = (row.Cells[2].Value == null) ? "" : row.Cells[2].Value.ToString();
                            r.Index = (row.Cells[3].Value == null) ? false : (bool)row.Cells[3].Value;
                            rowRecords.Push(r);
                        }
                    }
                    else
                    {
                        r.RowIndex = rowIndex;
                        r.Tag = "Removed";
                        r.Name = (row.Cells[0].Value == null) ? "" : row.Cells[0].Value.ToString();
                        r.Type = (row.Cells[1].Value == null) ? "" : row.Cells[1].Value.ToString();
                        r.Length = (row.Cells[2].Value == null) ? "" : row.Cells[2].Value.ToString();
                        r.Index = (row.Cells[3].Value == null) ? false : (bool)row.Cells[3].Value;
                        rowRecords.Push(r);
                    }
                    dataXML.Rows.RemoveAt(rowIndex);
                }
            }
        }

        /// <summary>
        /// 单行同步，非整个文件同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (addButton.Text == "同步")
            {
            List<string> indexListOra;
            List<string> indexListSQL;
            bool haschanged = false;
            foreach (DataGridViewRow row in dataXML.SelectedRows)
            {
                if (row.Index == dataXML.Rows.Count - 1) { break; }
                if (DBName == "*Oracle*")
                {
                    indexListOra = DBconnectionOracle.FindIndexs(tableName);
                        SynchronizeOracleRow(row, "dataXML", indexListOra, ref haschanged);

                }
                else
                {
                    indexListSQL = DBconnectionSQLServer.FindIndexs(DBName, tableName);
                        SynchronizeSQLRow(row, "dataXML", indexListSQL, ref haschanged);
                }
            }
            foreach (DataGridViewRow row2 in dataDB.SelectedRows)
            {
                if (row2.Index == dataDB.Rows.Count - 1) { break; }
                if (DBName == "*Oracle*")
                {
                    indexListOra = DBconnectionOracle.FindIndexs(tableName);
                        SynchronizeOracleRow(row2, "dataDB", indexListOra, ref haschanged);

                }
                else
                {
                    indexListSQL = DBconnectionSQLServer.FindIndexs(DBName, tableName);
                        SynchronizeSQLRow(row2, "dataDB", indexListSQL, ref haschanged);

                }
            }
            if (haschanged == false)
            {
                MessageBox.Show("选定行同步，无需再次同步！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                exportSqlButton.Enabled = false;
            }
            else
            {
                MessageBox.Show(EditResults(), "同步成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                dbLists.Clear();
                if (DBName == "*Oracle*")
                {
                    DBconnectionOracle.GetNndT(tableName, ref dbLists);
                }
                else
                {
                    DBconnectionSQLServer.GetNndT(DBName, tableName, ref dbLists);
                }
                fillDataDB();
                UpdateInfoLists();
                SortDGV();
                fillDataXML();
                SynchronizeIndex();
                compareLists1();
                compareLists2();
                showResults = "";
                exportSqlButton.Enabled = true;
            }
         }
        }

        private static bool isUndoing = false;
        //CancelButton.Enabled = true;
        /// <summary>
        /// 点击撤销按钮，撤销操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            isUndoing = true;
            
                if (rowRecords.Any()==true)
                {
                    ///若有记录，点击撤销后，用该记录查询对应逆操作，执行逆操作
                    RowRecord rowTemp = rowRecords.Pop();
                    int index = rowTemp.RowIndex;
                    if(rowTemp.Tag=="Added")
                    {
                        if (index != dataXML.RowCount - 1) { dataXML.Rows.RemoveAt(index); }
                    }
                    else if(rowTemp.Tag=="Removed")
                    {
                    //撤销删除实际是重新新增那一行
                    DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell();
                    DataGridViewComboBoxCell typeCell = new DataGridViewComboBoxCell();
                    DataGridViewTextBoxCell lengthCell = new DataGridViewTextBoxCell();
                    DataGridViewCheckBoxCell indexCell = new DataGridViewCheckBoxCell();
                    DataGridViewRow row = new DataGridViewRow();

                    typeCell.Value = rowTemp.Type;

                    if (DBName == "*Oracle*")
                    {
                        Dictionary<string, string> typeLists = MappingTool.GetAllTypesOracle();
                        List<string> showTypes = new List<string>();
                        foreach (string key in typeLists.Keys)
                        {
                            showTypes.Add(key);
                        }
                        typeCell.DataSource = showTypes;
                    }
                    else
                    {
                        Dictionary<string, string> typeLists = MappingTool.GetAllTypesSQL();
                        List<string> showTypes = new List<string>();
                        foreach (string key in typeLists.Keys)
                        {
                            showTypes.Add(key);
                        }
                        typeCell.DataSource = showTypes;
                    }
                    nameCell.Value = rowTemp.Name;
                    
                    lengthCell.Value = rowTemp.Length;
                    indexCell.Value = rowTemp.Index;
                    int rowIndex = rowTemp.RowIndex;
                    row.Cells.Add(nameCell);
                    row.Cells.Add(typeCell);
                    row.Cells.Add(lengthCell);
                    row.Cells.Add(indexCell);
                    dataXML.Rows.Insert(rowIndex, row);

                    if(addButton.Text == "同步" && (dataDB.Rows[rowIndex]==null||dataDB.Rows[rowIndex].Cells[0].Value==null|| dataDB.Rows[rowIndex].Cells[0].Value.ToString()==""))
                    {
                        DataGridViewRow rowDB = new DataGridViewRow();
                        dataDB.Rows.Insert(rowIndex, rowDB);
                    }

                }
                else
                    {
                        dataXML.Rows[index].Cells[0].Value = rowTemp.Name;
                        dataXML.Rows[index].Cells[1].Value = rowTemp.Type;
                        dataXML.Rows[index].Cells[2].Value = rowTemp.Length;
                        ((DataGridViewCheckBoxCell)dataXML.Rows[index].Cells[3]).Value = rowTemp.Index; 
                    }
                    
                }
            if (addButton.Text == "同步")
            {
                compareLists1();
                compareLists2();
            }
            isUndoing = false;
        }

        
        /// <summary>
        /// 记录修改之前的行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (isUndoing == false)
            {
                DataGridViewRow row = dataXML.CurrentRow;
                RowRecord r = new RowRecord();
                r.RowIndex = row.Index;
                if ((row.Cells[0].Value==null||row.Cells[0].Value.ToString()=="")&&row.Index==dataXML.RowCount-1)
                {
                    r.Tag ="Added";
                    rowRecords.Push(r);
                }
                else
                {
                    r.Name = (row.Cells[0].Value==null)?"": row.Cells[0].Value.ToString();
                    r.Type = (row.Cells[1].Value == null) ? "" : row.Cells[1].Value.ToString();
                    r.Length = (row.Cells[2].Value == null) ? "" : row.Cells[2].Value.ToString();
                    r.Index = (row.Cells[3].Value == null) ? false : (bool)row.Cells[3].Value;
                    rowRecords.Push(r);
                }
            }
        }

        /// <summary>
        /// 删除未更改的行状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataXML_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(isUndoing==false)
            {
                DataGridViewRow row = dataXML.CurrentRow;
                if (rowRecords.Any() == true)
                {
                    RowRecord r = rowRecords.First();

                    if (r.Name == row.Cells[0].EditedFormattedValue.ToString() && r.Type == row.Cells[1].EditedFormattedValue.ToString() && r.Length == row.Cells[2].EditedFormattedValue.ToString() && r.Index == (bool)row.Cells[3].FormattedValue)
                    {
                        rowRecords.Pop();
                    }
                    
                }
            }
        }

        private void dataXML_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewRow row1 = e.Row;
            int rowIndex = row1.Index;
            RowRecord r = new RowRecord();

            if (addButton.Text=="同步")
            {
                if (dataDB.Rows[rowIndex] != null && dataDB.Rows[rowIndex].Cells[0].Value != null && dataDB.Rows[rowIndex].Cells[0].Value.ToString() != "")
                {
                    r.RowIndex = rowIndex;
                    r.Tag = "RemovedFake";
                    r.Name = (row1.Cells[0].Value == null) ? "" : row1.Cells[0].Value.ToString();
                    r.Type = (row1.Cells[1].Value == null) ? "" : row1.Cells[1].Value.ToString();
                    r.Length = (row1.Cells[2].Value == null) ? "" : row1.Cells[2].Value.ToString();
                    r.Index = (row1.Cells[3].Value == null) ? false : (bool)row1.Cells[3].Value;
                    rowRecords.Push(r);
                }
                else
                {
                    r.RowIndex = rowIndex;
                    r.Tag = "Removed";
                    r.Name = (row1.Cells[0].Value == null) ? "" : row1.Cells[0].Value.ToString();
                    r.Type = (row1.Cells[1].Value == null) ? "" : row1.Cells[1].Value.ToString();
                    r.Length = (row1.Cells[2].Value == null) ? "" : row1.Cells[2].Value.ToString();
                    r.Index = (row1.Cells[3].Value == null) ? false : (bool)row1.Cells[3].Value;
                    rowRecords.Push(r);
                }
            }
           else
            {
                r.RowIndex = rowIndex;
                r.Tag = "Removed";
                r.Name = (row1.Cells[0].Value == null) ? "" : row1.Cells[0].Value.ToString();
                r.Type = (row1.Cells[1].Value == null) ? "" : row1.Cells[1].Value.ToString();
                r.Length = (row1.Cells[2].Value == null) ? "" : row1.Cells[2].Value.ToString();
                r.Index = (row1.Cells[3].Value == null) ? false : (bool)row1.Cells[3].Value;
                rowRecords.Push(r);
            }        
        }
    }
}


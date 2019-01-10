namespace DB.Forms
{
    partial class FormXMLExecute
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXMLExecute));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataXML = new System.Windows.Forms.DataGridView();
            this.fieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.fieldLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isIndex = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipe = new System.Windows.Forms.Label();
            this.dataDB = new System.Windows.Forms.DataGridView();
            this.fieldNameDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldTypeDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldLengthDB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isIndexDB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BackButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.exportSqlButton = new System.Windows.Forms.Button();
            this.tip = new System.Windows.Forms.Label();
            this.UndoButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataXML)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataDB)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1124, 601);
            this.splitContainer1.SplitterDistance = 538;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataXML);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tipe);
            this.splitContainer2.Panel2.Controls.Add(this.dataDB);
            this.splitContainer2.Size = new System.Drawing.Size(1124, 538);
            this.splitContainer2.SplitterDistance = 553;
            this.splitContainer2.TabIndex = 0;
            // 
            // dataXML
            // 
            this.dataXML.AllowUserToResizeColumns = false;
            this.dataXML.AllowUserToResizeRows = false;
            this.dataXML.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataXML.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataXML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataXML.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fieldName,
            this.fieldType,
            this.fieldLength,
            this.isIndex});
            this.dataXML.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataXML.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataXML.Location = new System.Drawing.Point(0, 0);
            this.dataXML.Name = "dataXML";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataXML.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataXML.RowTemplate.Height = 23;
            this.dataXML.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataXML.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataXML.Size = new System.Drawing.Size(553, 538);
            this.dataXML.TabIndex = 0;
            this.dataXML.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataXML_CellBeginEdit);
            this.dataXML.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataXML_CellContentClick);
            this.dataXML.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataXML_CellEndEdit);
            this.dataXML.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataXML_CellMouseEnter);
            this.dataXML.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataXML_CellValidated);
            this.dataXML.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataXML_EditingControlShowing);
            this.dataXML.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataXML_RowPostPaint);
            this.dataXML.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataXML_RowsAdded);
            this.dataXML.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataXML_RowsRemoved);
            this.dataXML.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataXML_Scroll);
            this.dataXML.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataXML_UserDeletingRow);
            // 
            // fieldName
            // 
            this.fieldName.HeaderText = "字段名";
            this.fieldName.Name = "fieldName";
            // 
            // fieldType
            // 
            this.fieldType.HeaderText = "字段类型";
            this.fieldType.Name = "fieldType";
            // 
            // fieldLength
            // 
            this.fieldLength.HeaderText = "字段长度";
            this.fieldLength.Name = "fieldLength";
            // 
            // isIndex
            // 
            this.isIndex.HeaderText = "索引";
            this.isIndex.Name = "isIndex";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.TBToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.deleteToolStripMenuItem.Text = "删除";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // TBToolStripMenuItem
            // 
            this.TBToolStripMenuItem.Name = "TBToolStripMenuItem";
            this.TBToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.TBToolStripMenuItem.Text = "同步";
            this.TBToolStripMenuItem.Click += new System.EventHandler(this.TBToolStripMenuItem_Click);
            // 
            // tipe
            // 
            this.tipe.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tipe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tipe.Location = new System.Drawing.Point(0, 0);
            this.tipe.Name = "tipe";
            this.tipe.Size = new System.Drawing.Size(567, 538);
            this.tipe.TabIndex = 1;
            this.tipe.Text = "数据库中无此表！点击添加按钮添加表";
            this.tipe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tipe.Visible = false;
            // 
            // dataDB
            // 
            this.dataDB.AllowUserToResizeColumns = false;
            this.dataDB.AllowUserToResizeRows = false;
            this.dataDB.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataDB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataDB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataDB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fieldNameDB,
            this.fieldTypeDB,
            this.fieldLengthDB,
            this.isIndexDB});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataDB.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataDB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDB.Location = new System.Drawing.Point(0, 0);
            this.dataDB.Name = "dataDB";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataDB.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataDB.RowTemplate.Height = 23;
            this.dataDB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataDB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataDB.Size = new System.Drawing.Size(567, 538);
            this.dataDB.TabIndex = 0;
            this.dataDB.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataDB_CellMouseEnter);
            this.dataDB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataDB_Scroll);
            // 
            // fieldNameDB
            // 
            this.fieldNameDB.HeaderText = "字段名";
            this.fieldNameDB.Name = "fieldNameDB";
            this.fieldNameDB.ReadOnly = true;
            // 
            // fieldTypeDB
            // 
            this.fieldTypeDB.HeaderText = "字段类型";
            this.fieldTypeDB.Name = "fieldTypeDB";
            this.fieldTypeDB.ReadOnly = true;
            // 
            // fieldLengthDB
            // 
            this.fieldLengthDB.HeaderText = "字段长度";
            this.fieldLengthDB.Name = "fieldLengthDB";
            this.fieldLengthDB.ReadOnly = true;
            // 
            // isIndexDB
            // 
            this.isIndexDB.HeaderText = "索引";
            this.isIndexDB.Name = "isIndexDB";
            this.isIndexDB.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1124, 59);
            this.panel1.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.82206F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.14235F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.03203F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.58719F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.3452F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.936975F));
            this.tableLayoutPanel1.Controls.Add(this.BackButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.addButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.exportSqlButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UndoButton, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1124, 59);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // BackButton
            // 
            this.BackButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BackButton.AutoSize = true;
            this.BackButton.Location = new System.Drawing.Point(954, 18);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 1;
            this.BackButton.Text = "返回";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.addButton.AutoSize = true;
            this.addButton.Location = new System.Drawing.Point(698, 18);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "添加";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // exportSqlButton
            // 
            this.exportSqlButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.exportSqlButton.AutoSize = true;
            this.exportSqlButton.Enabled = false;
            this.exportSqlButton.Location = new System.Drawing.Point(576, 18);
            this.exportSqlButton.Name = "exportSqlButton";
            this.exportSqlButton.Size = new System.Drawing.Size(81, 23);
            this.exportSqlButton.TabIndex = 2;
            this.exportSqlButton.Text = "导出SQL";
            this.exportSqlButton.UseVisualStyleBackColor = true;
            this.exportSqlButton.Click += new System.EventHandler(this.exportSqlButton_Click);
            // 
            // tip
            // 
            this.tip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tip.Location = new System.Drawing.Point(3, 0);
            this.tip.Name = "tip";
            this.tip.Size = new System.Drawing.Size(554, 59);
            this.tip.TabIndex = 3;
            this.tip.Text = "提示：移动鼠标选中单行或按住左键多选后，\r\n      点击Delete或点击鼠标右键选择删除即可删除所选行\r\n      点击鼠标右键选择同步即可同步所选行\r\n" +
    "      在最下面空白行直接输入即可增加新行";
            this.tip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UndoButton
            // 
            this.UndoButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UndoButton.AutoSize = true;
            this.UndoButton.Location = new System.Drawing.Point(820, 18);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(75, 23);
            this.UndoButton.TabIndex = 4;
            this.UndoButton.Text = "撤销";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FormXMLExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 601);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormXMLExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tofflon开发小工具";
            this.Load += new System.EventHandler(this.FormXMLExecute_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataXML)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataDB)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dataDB;
        private System.Windows.Forms.DataGridView dataXML;
        private System.Windows.Forms.Label tipe;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button BackButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldName;
        private System.Windows.Forms.DataGridViewComboBoxColumn fieldType;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldLength;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button exportSqlButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label tip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldNameDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldTypeDB;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldLengthDB;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isIndexDB;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TBToolStripMenuItem;
        private System.Windows.Forms.Button UndoButton;
    }
}
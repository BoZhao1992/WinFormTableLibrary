namespace TableCommonLibrary.UC
{
    partial class UCGBTable
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCGBTable));
            this.tlpTable = new System.Windows.Forms.TableLayoutPanel();
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTable = new System.Windows.Forms.Panel();
            this.dgv = new Controls.HeaderUnitViewEx(this.components);
            this.tlpTable.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpTable
            // 
            this.tlpTable.AutoSize = true;
            this.tlpTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTable.ColumnCount = 1;
            this.tlpTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTable.Controls.Add(this.pnlButton, 0, 2);
            this.tlpTable.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpTable.Controls.Add(this.pnlTable, 0, 1);
            this.tlpTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTable.Location = new System.Drawing.Point(0, 0);
            this.tlpTable.Name = "tlpTable";
            this.tlpTable.RowCount = 3;
            this.tlpTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTable.Size = new System.Drawing.Size(778, 137);
            this.tlpTable.TabIndex = 0;
            // 
            // pnlButton
            // 
            this.pnlButton.AutoSize = true;
            this.pnlButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlButton.Controls.Add(this.btnAdd);
            this.pnlButton.Controls.Add(this.btnDel);
            this.pnlButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButton.Location = new System.Drawing.Point(0, 110);
            this.pnlButton.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButton.Name = "pnlButton";
            this.pnlButton.Size = new System.Drawing.Size(778, 27);
            this.pnlButton.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImage = global::TableCommonLibrary.Properties.Resources.Add;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAdd.Location = new System.Drawing.Point(712, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(30, 23);
            this.btnAdd.TabIndex = 113;
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.BackgroundImage = global::TableCommonLibrary.Properties.Resources.Del;
            this.btnDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDel.Location = new System.Drawing.Point(748, 4);
            this.btnDel.Margin = new System.Windows.Forms.Padding(0);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(30, 23);
            this.btnDel.TabIndex = 114;
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // pnlTitle
            // 
            this.pnlTitle.AutoSize = true;
            this.pnlTitle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(778, 20);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(14, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(29, 12);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "标题";
            // 
            // pnlTable
            // 
            this.pnlTable.AutoSize = true;
            this.pnlTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTable.Controls.Add(this.dgv);
            this.pnlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTable.Location = new System.Drawing.Point(0, 20);
            this.pnlTable.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTable.Name = "pnlTable";
            this.pnlTable.Size = new System.Drawing.Size(778, 90);
            this.pnlTable.TabIndex = 1;
            // 
            // dgv
            // 
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.CellHeight = 17;
            this.dgv.ColumnDeep = 1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ColumnTreeView = null;
            this.dgv.Location = new System.Drawing.Point(14, 8);
            this.dgv.Margin = new System.Windows.Forms.Padding(0);
            this.dgv.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("dgv.MergeColumnNames")));
            this.dgv.MergeRowIndexs = ((System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<int>>)(resources.GetObject("dgv.MergeRowIndexs")));
            this.dgv.Name = "dgv";
            this.dgv.RefreshAtHscroll = false;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.Size = new System.Drawing.Size(764, 82);
            this.dgv.TabIndex = 0;
            // 
            // UCGBTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tlpTable);
            this.Name = "UCGBTable";
            this.Size = new System.Drawing.Size(778, 137);
            this.tlpTable.ResumeLayout(false);
            this.tlpTable.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTable;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Button btnDel;
        public Controls.HeaderUnitViewEx dgv;
        public System.Windows.Forms.Panel pnlTitle;
        public System.Windows.Forms.Panel pnlTable;
        public System.Windows.Forms.Panel pnlButton;
        public System.Windows.Forms.Label lblTitle;

    }
}

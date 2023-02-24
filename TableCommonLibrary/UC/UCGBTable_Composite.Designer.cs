namespace TableCommonLibrary.UC
{
    partial class UCGBTable_Composite
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
            this.cmsShortcutKey = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpTable_Composite = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTable_Composite = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.cmsShortcutKey.SuspendLayout();
            this.tlpTable_Composite.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlTable_Composite.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmsShortcutKey
            // 
            this.cmsShortcutKey.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRename,
            this.tsmiDelete});
            this.cmsShortcutKey.Name = "contextMenuStrip1";
            this.cmsShortcutKey.Size = new System.Drawing.Size(113, 48);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(112, 22);
            this.tsmiRename.Text = "重命名";
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(112, 22);
            this.tsmiDelete.Text = "删除";
            // 
            // tlpTable_Composite
            // 
            this.tlpTable_Composite.AutoSize = true;
            this.tlpTable_Composite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpTable_Composite.BackColor = System.Drawing.Color.White;
            this.tlpTable_Composite.ColumnCount = 1;
            this.tlpTable_Composite.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTable_Composite.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpTable_Composite.Controls.Add(this.pnlTable_Composite, 0, 1);
            this.tlpTable_Composite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTable_Composite.Location = new System.Drawing.Point(0, 0);
            this.tlpTable_Composite.Name = "tlpTable_Composite";
            this.tlpTable_Composite.RowCount = 2;
            this.tlpTable_Composite.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTable_Composite.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTable_Composite.Size = new System.Drawing.Size(778, 110);
            this.tlpTable_Composite.TabIndex = 110;
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
            this.pnlTitle.TabIndex = 110;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(14, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(29, 12);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "标题";
            // 
            // pnlTable_Composite
            // 
            this.pnlTable_Composite.AutoSize = true;
            this.pnlTable_Composite.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTable_Composite.Controls.Add(this.tabControl);
            this.pnlTable_Composite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTable_Composite.Location = new System.Drawing.Point(14, 28);
            this.pnlTable_Composite.Margin = new System.Windows.Forms.Padding(14, 8, 0, 0);
            this.pnlTable_Composite.Name = "pnlTable_Composite";
            this.pnlTable_Composite.Size = new System.Drawing.Size(764, 82);
            this.pnlTable_Composite.TabIndex = 111;
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(764, 82);
            this.tabControl.TabIndex = 110;
            // 
            // UCGBTable_Composite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tlpTable_Composite);
            this.Name = "UCGBTable_Composite";
            this.Size = new System.Drawing.Size(778, 110);
            this.cmsShortcutKey.ResumeLayout(false);
            this.tlpTable_Composite.ResumeLayout(false);
            this.tlpTable_Composite.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.pnlTable_Composite.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ContextMenuStrip cmsShortcutKey;
        protected System.Windows.Forms.ToolStripMenuItem tsmiRename;
        protected System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.TableLayoutPanel tlpTable_Composite;
        public System.Windows.Forms.Label lblTitle;
        protected System.Windows.Forms.TabControl tabControl;
        public System.Windows.Forms.Panel pnlTitle;
        public System.Windows.Forms.Panel pnlTable_Composite;
    }
}

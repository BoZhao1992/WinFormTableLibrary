namespace TableCommonLibrary.UC
{
    partial class GBTablePopupWindow
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
            this.tlpTablePopupWindow = new System.Windows.Forms.TableLayoutPanel();
            this.pnlOK = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.gbItems = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tlpTablePopupWindow.SuspendLayout();
            this.pnlOK.SuspendLayout();
            this.gbItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTablePopupWindow
            // 
            this.tlpTablePopupWindow.ColumnCount = 1;
            this.tlpTablePopupWindow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTablePopupWindow.Controls.Add(this.pnlOK, 0, 1);
            this.tlpTablePopupWindow.Controls.Add(this.gbItems, 0, 0);
            this.tlpTablePopupWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTablePopupWindow.Location = new System.Drawing.Point(0, 0);
            this.tlpTablePopupWindow.Name = "tlpTablePopupWindow";
            this.tlpTablePopupWindow.RowCount = 2;
            this.tlpTablePopupWindow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tlpTablePopupWindow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpTablePopupWindow.Size = new System.Drawing.Size(564, 156);
            this.tlpTablePopupWindow.TabIndex = 1;
            // 
            // pnlOK
            // 
            this.pnlOK.AutoSize = true;
            this.pnlOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlOK.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOK.Controls.Add(this.btnOK);
            this.pnlOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOK.Location = new System.Drawing.Point(0, 132);
            this.pnlOK.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOK.Name = "pnlOK";
            this.pnlOK.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlOK.Size = new System.Drawing.Size(564, 24);
            this.pnlOK.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(479, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // gbItems
            // 
            this.gbItems.Controls.Add(this.panel1);
            this.gbItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbItems.Location = new System.Drawing.Point(10, 10);
            this.gbItems.Margin = new System.Windows.Forms.Padding(10);
            this.gbItems.Name = "gbItems";
            this.gbItems.Size = new System.Drawing.Size(544, 112);
            this.gbItems.TabIndex = 2;
            this.gbItems.TabStop = false;
            this.gbItems.Text = "勾选列表";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(538, 92);
            this.panel1.TabIndex = 0;
            // 
            // GBTablePopupWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 156);
            this.Controls.Add(this.tlpTablePopupWindow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GBTablePopupWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.GBTablePopupWindow_Load);
            this.tlpTablePopupWindow.ResumeLayout(false);
            this.tlpTablePopupWindow.PerformLayout();
            this.pnlOK.ResumeLayout(false);
            this.gbItems.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTablePopupWindow;
        private System.Windows.Forms.Panel pnlOK;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox gbItems;
        private System.Windows.Forms.Panel panel1;

    }
}
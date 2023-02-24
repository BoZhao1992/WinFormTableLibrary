using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TableCommonLibrary.Helper;
using TableCommonLibrary.Interface;

/*----------------------------------------------------
//文件名：UCGBTableGeneric_Composite
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/9/19
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.UC
{
    /// <summary>
    /// 复式表格界面
    /// </summary>
    /// <typeparam name="TPro">表格属性</typeparam>
    /// <typeparam name="TSub">复式表格子表</typeparam>
    public class UCGBTable_Composite<TPro, TSub> : UCGBTable_Composite
        where TPro : class, IGBTableProReadWrite, new()
        where TSub : class, IGBTableSub<TPro>, new()
    {
        const int TableAdjustmentPixel = 10;// UCGBTable控件的表格调整像素
        Type type;
        Action action;
        GBIndentationLevel indentationLevel;
        IGBTableComposite<TPro, TSub> data;
        List<TSub> tableComposite;
        int textIndentationDistance;// 文本的缩进距离
        int tableIndentationDistance;// 表格的缩进距离
        bool isExistTitle = true;// 是否存在标题
        IGBUCSubTable<TPro> ucSubTable;// 当前子表
        List<UCGBTable<TPro>> ucGBTables = new List<UCGBTable<TPro>>();// 容器中子表数量集合
        /// <summary>
        /// 点击添加按钮触发的事件
        /// </summary>
        public event Action ClickedAddButton;

        public UCGBTable_Composite(IGBTableComposite<TPro, TSub> data)
        {
            this.data = data;
            tableComposite = data.GBTableComposite;
            tsmiRename.Click += tsmiRename_Click;
            tsmiDelete.Click += tsmiDelete_Click;
            tabControl.MouseClick += tabControl_MouseClick;
            pnlTitle.Visible = false;
        }

        /// <summary>
        /// 加载复合表格界面
        /// </summary>
        /// <param name="type"></param>
        /// <param name="action">删除子表时执行的委托，如刷新分数和界面显示数据</param>
        /// <param name="indentationLevel">缩进级别</param>
        public void LoadDGV(Type type, Action action, GBIndentationLevel indentationLevel)
        {
            this.type = type;
            this.action = action;
            this.indentationLevel = indentationLevel;
            // 计算文本的缩进距离
            textIndentationDistance = (14 * (int)indentationLevel).ToScaling();
            // 计算表格缩进距离
            tableIndentationDistance = (16 * (int)indentationLevel).ToScaling();
            // 是否存在标题
            isExistTitle = !string.IsNullOrWhiteSpace(data.Title);
            // 初始化标题
            InitialTitle();
            // 初始化复式表格
            InitializeTableComposite();
        }

        /// <summary>
        /// 初始化标题
        /// </summary>
        private void InitialTitle()
        {
            if (isExistTitle)
            {
                pnlTitle.Visible = true;
                lblTitle.Text = data.Title;
                // 缩进后位置
                lblTitle.Location = new Point(lblTitle.Location.X + textIndentationDistance, lblTitle.Location.Y);
            }
        }

        /// <summary>
        /// 初始化复式表格
        /// </summary>
        private void InitializeTableComposite()
        {
            foreach (TSub sub in tableComposite)
            {
                TabPage tp = new TabPage();
                tp.BackColor = Color.White;
                tp.Text = sub.Name;
                tp.Name = sub.Name;
                tp.Tag = sub;

                AddTableSubToTabPage(tp, sub);
                tabControl.TabPages.Add(tp);
            }

            if (tableComposite.Count == 0)//至少有一个Page页
            {
                TabPage tp = GetTabPage();
                tabControl.TabPages.Add(tp);
                tabControl.SelectedTab = tp;
            }

            // 添加"+"TabPage
            AddPlusPage();

            // 设置缩进级别
            pnlTable_Composite.Margin = new Padding(pnlTable_Composite.Margin.Left + tableIndentationDistance, pnlTable_Composite.Margin.Top, pnlTable_Composite.Margin.Right, pnlTable_Composite.Margin.Bottom);
        }

        /// <summary>
        /// 获取TabPage
        /// </summary>
        /// <returns>TabPage</returns>
        private TabPage GetTabPage()
        {
            // 初始化子表实体
            TSub sub = new TSub();
            string name = GetTabPageName(sub);
            sub.Name = name;
            tableComposite.Add(sub);
            // 初始化TabPage
            TabPage tp = new TabPage();
            tp.BackColor = Color.White;
            tp.Text = name;
            tp.Name = name;
            tp.Tag = sub;

            AddTableSubToTabPage(tp, sub);
            return tp;
        }

        /// <summary>
        /// 添加子表到TabPage
        /// </summary>
        /// <param name="tp">tabPage</param>
        /// <param name="sub">子表实体</param>
        private void AddTableSubToTabPage(TabPage tp, TSub sub)
        {
            // 初始化子表界面
            object obj = Activator.CreateInstance(type, sub, action);
            ucSubTable = obj as IGBUCSubTable<TPro>;
            if (ucSubTable == null)
                throw new NullReferenceException("类型转换失败，检查类型" + type + "是否实现IGBUCSubTable接口");
            ucGBTables.Add(ucSubTable.UCGBTable);
            SetUCGBTableInfo(ucSubTable.UCGBTable);
            //调整UC高度
            Control tableSub = obj as Control;
            int height = 55;
            if (!isExistTitle)
                height = height - pnlTitle.Height;
            Height = tableSub.PreferredSize.Height + height.ToScaling();
            tp.Controls.Add(tableSub);
        }

        /// <summary>
        /// 设置UCGBTable内部控件的信息
        /// </summary>
        /// <param name="ucGBTable">表格界面</param>
        private void SetUCGBTableInfo(UCGBTable<TPro> ucGBTable)
        {
            #region 初始化UCGBTable内部控件位置
            // 标题
            ucGBTable.lblTitle.Location = new Point(0, 0);
            // 表格
            int x = ucGBTable.dgv.Location.X;// 左移距离
            ucGBTable.dgv.Location = new Point(0, 0);
            ucGBTable.dgv.Size = new Size(ucGBTable.dgv.Size.Width - 10, ucGBTable.dgv.Size.Height);
            // 按钮
            ucGBTable.btnAdd.Location = new Point(ucGBTable.btnAdd.Location.X - x - TableAdjustmentPixel, ucGBTable.btnAdd.Location.Y);
            ucGBTable.btnDel.Location = new Point(ucGBTable.btnDel.Location.X - x - TableAdjustmentPixel, ucGBTable.btnDel.Location.Y);
            #endregion

            // 根据缩进级别调整UCGBTable内部控件
            ucGBTable.dgv.Size = new Size(ucGBTable.dgv.Size.Width - tableIndentationDistance, ucGBTable.dgv.Size.Height);
            ucGBTable.btnAdd.Location = new Point(ucGBTable.btnAdd.Location.X - tableIndentationDistance, ucGBTable.btnAdd.Location.Y);
            ucGBTable.btnDel.Location = new Point(ucGBTable.btnDel.Location.X - tableIndentationDistance, ucGBTable.btnDel.Location.Y);
            // 刷新表格列宽度
            ucGBTable.RefreshTableColumnsWidth();
        }

        /// <summary>
        /// 获取TabPage名称
        /// </summary>
        /// <returns>名称</returns>
        private string GetTabPageName(TSub sub)
        {
            List<int> indexs = new List<int>();
            string schemeRegex = GetTabPageNameMark(sub);
            foreach (TabPage page in tabControl.TabPages)
            {
                string name = page.Name;
                if (System.Text.RegularExpressions.Regex.IsMatch(name, schemeRegex))
                {
                    string result = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]+", "");
                    int i;
                    bool isInt = int.TryParse(result, out i);
                    if (isInt)
                    {
                        indexs.Add(Convert.ToInt32(result));
                    }
                }
            }

            if (indexs.Count > 0)
            {
                return schemeRegex + (indexs.Max() + 1);
            }

            return schemeRegex + "1";
        }

        private void AddPlusPage()
        {
            TabPage tabPage = new TabPage();
            tabPage.Text = "  ＋";
            tabPage.Name = "tpPlus";
            tabPage.BackColor = Color.White;
            tabControl.TabPages.Add(tabPage);
        }

        /// <summary>
        /// 子表重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiRename_Click(object sender, EventArgs e)
        {
            FrmChangeTab ftb = new FrmChangeTab();
            if (ftb.ShowDialog() == DialogResult.OK)
            {
                string name = ftb.txtNewName.Text.Trim();
                TabPage tabPage = tabControl.SelectedTab;
                tabPage.Text = name;
                tabPage.Name = name;

                TSub sub = tabPage.Tag as TSub;
                sub.Name = name;

                tabControl.Refresh();
            }
        }

        /// <summary>
        /// 删除子表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "确定要删除类型吗？删除后无法恢复。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                TabPage tabPage = tabControl.SelectedTab;
                TSub sub = tabPage.Tag as TSub;
                // 移除复式表格指定子表
                tableComposite.Remove(sub);
                // 移除tabControl指定tabPage
                tabControl.TabPages.Remove(tabPage);

                tabControl.Refresh();

                // 执行委托
                Action handle = action;
                if (handle != null)
                    handle();
            }
        }

        /// <summary>
        /// tabControl鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)// 鼠标右键
            {
                // TabPage页显示快捷菜单("+"除外)
                bool isShow = false;
                TabPage page = null;
                for (int i = 0; i < tabControl.TabCount; i++)
                {
                    if (tabControl.TabCount - 1 != i)
                    {
                        Rectangle recTab = tabControl.GetTabRect(i);
                        if (recTab.Contains(e.X, e.Y))
                        {
                            isShow = true;
                            page = tabControl.TabPages[i];
                            break;
                        }
                    }
                }
                if (isShow)
                {
                    cmsShortcutKey.Show(tabControl, e.Location);
                    tabControl.SelectedTab = page;

                    // 至少有一个构件子类型
                    if (tabControl.TabCount == 2)
                    {
                        tsmiDelete.Enabled = false;
                    }
                    else
                    {
                        tsmiDelete.Enabled = true;
                    }
                }
                else
                {
                    cmsShortcutKey.Hide();
                }
            }
            else if (e.Button == MouseButtons.Left)// 鼠标左键
            {
                // 切换"+"页时，在"+"前插入TabPage
                if (tabControl.SelectedIndex == tabControl.TabCount - 1)
                {
                    // 触发事件
                    Action handler = ClickedAddButton;
                    if (handler != null)
                        handler();
                    // 添加TabPage
                    TabPage tabPage = GetTabPage();
                    tabControl.TabPages.Insert(tabControl.TabCount - 1, tabPage);
                    tabControl.SelectedTab = tabPage;
                    ucSubTable.UCGBTable.DGVLoadedAfterHandle();
                }
            }
        }

        /// <summary>
        /// 获取TabPage名称标记
        /// </summary>
        /// <returns>TabPage名称标记</returns>
        private string GetTabPageNameMark(TSub sub)
        {
            if (!string.IsNullOrWhiteSpace(sub.Name))
                return sub.Name;
            else
                return "建筑";
        }

        /// <summary>
        /// 刷新复式表格数据
        /// </summary>
        public void RefreshTableCompositeData()
        {
            foreach (var ucGBTable in ucGBTables)
            {
                if (ucGBTable == null)
                    throw new NullReferenceException("子表界面为Null, 检查是否初始化");
                ucGBTable.RefreshTableData();
            }
        }
    }
}

using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TableCommonLibrary.Common.UCData;
using TableCommonLibrary.Helper;
using TableCommonLibrary.Interface;

/*----------------------------------------------------
//文件名：UCGBTableGeneric
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/9/3
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.UC
{
    /// <summary>
    /// 表格界面
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UCGBTable<T> : UCGBTable
        where T : class, IGBTableProReadWrite, new()
    {
        private readonly GBTable<T> table;
        private readonly GBTable_SingleLineHeader<T> table_SingleLineHeader;
        private readonly GBTable_MultiLineHeader<T> table_MultiLineHeader;
        private Action<DataGridViewRow, bool> updateDataOfRowHandle;
        private GBIndentationLevel indentationLevel;
        private Action doCalHandle;
        private Controls.ComboBoxEx selectedColumnComboBox;
        private DataGridViewComboBoxExColumn comboBoxExColumn;
        private int cellEndEditColumnIndex;

        public UCGBTable(GBTable<T> table)
        {
            this.table = table;
            if (this.table == null)
                throw new ArgumentNullException("table", "参数为空");
            this.table_SingleLineHeader = this.table as GBTable_SingleLineHeader<T>;
            if (this.table_SingleLineHeader == null)
            {
                this.table_MultiLineHeader = this.table as GBTable_MultiLineHeader<T>;
                if (this.table_MultiLineHeader == null)
                    throw new ArgumentException("table", "参数类型转换失败");
            }
            pnlTitle.Visible = false;
            pnlButton.Visible = false;
        }

        public GBTable<T> Table { get { return table; } }

        /// <summary>
        /// 单元格结束编辑时列索引
        /// </summary>
        public int CellEndEditColumnIndex { get { return cellEndEditColumnIndex; } }

        /// <summary>
        /// 已选择列不可修改下拉框
        /// </summary>
        public Controls.ComboBoxEx SelectedColumnComboBox { get { return selectedColumnComboBox; } }

        /// <summary>
        /// 已选择列可修改下拉框对应的列信息
        /// </summary>
        public DataGridViewComboBoxExColumn ComboBoxExColumn { get { return comboBoxExColumn; } }

        /// <summary>
        /// 加载表格
        /// </summary>
        /// <param name="updateDataOfRowHandle">更新表格行数据</param>
        /// <param name="doCalHandle">计算</param>
        /// <param name="indentationLevel">缩进级别, 默认无缩进</param>
        public void LoadDGV(Action<DataGridViewRow, bool> updateDataOfRowHandle,
            Action doCalHandle, GBIndentationLevel indentationLevel = GBIndentationLevel.Nono)
        {
            this.updateDataOfRowHandle = updateDataOfRowHandle;
            this.indentationLevel = indentationLevel;
            this.doCalHandle = doCalHandle;

            // 初始化标题
            InitialTitle();

            // 初始化表格和按钮事件
            InitialEvents();

            // 表格缩进后的宽度和表格位置
            UCTableHelper.SetTableIndentedWidthLocation(dgv, indentationLevel);

            //  初始化表格列
            InitialDGVColumns();

            // 表格通用参数
            if (table_SingleLineHeader != null)
                UCTableHelper.SetTableParameter(dgv, table.DefaultTableRows, false, table_SingleLineHeader.HeaderLines);
            else
                UCTableHelper.SetTableParameter(dgv, table.DefaultTableRows, true);

            // 初始化表格数据
            InitialDGVDatas();
        }

        /// <summary>
        /// 初始化标题
        /// </summary>
        private void InitialTitle()
        {
            if (!string.IsNullOrWhiteSpace(table.Title))
            {
                pnlTitle.Visible = true;
                lblTitle.Text = table.Title;
                SetTitleIndentationLevel(indentationLevel);
            }
        }

        /// <summary>
        /// 设置表格标题的缩进级别
        /// </summary>
        /// <param name="indentationLevel">缩进级别</param>
        private void SetTitleIndentationLevel(GBIndentationLevel indentationLevel)
        {
            int indentationDistance = (14 * (int)indentationLevel).ToScaling();
            lblTitle.Location = new Point(lblTitle.Location.X + indentationDistance, lblTitle.Location.Y);
        }

        /// <summary>
        /// 初始化表格列
        /// </summary>
        private void InitialDGVColumns()
        {
            // 多行表头
            if (table_MultiLineHeader != null)
            {
                // 多行表头高度
                dgv.CellHeight = dgv.CellHeight.ToScaling();
                dgv.ColumnDeep = table_MultiLineHeader.Deep;
                dgv.ColumnTreeView = new TreeView[] { table_MultiLineHeader.TreeView };
            }

            // 填充表格列
            List<GBTableColumn> tableColumns = table.TableColumns;
            for (int i = 0; i < table.TableColumns.Count(); i++)
            {
                DataGridViewTextBoxColumn dgvColumn;
                DropDownBox dropDownBox = tableColumns[i].DropDownBox;
                if (dropDownBox != null)// 下拉框列
                {
                    if (dropDownBox.IsModified)// 可修改下拉框
                    {
                        DataGridViewComboBoxExColumn dgvColumnEx = new DataGridViewComboBoxExColumn();// 创建列
                        tableColumns[i].DropDownBox.ComboBoxExColumn = dgvColumnEx;
                        dgvColumnEx.DataSource = dropDownBox.Items;// 下拉框赋值
                        dgvColumn = dgvColumnEx;
                        SetDGVColumnReadOnly(dgvColumn, false);
                    }
                    else// 不可修改下拉框
                    {
                        dgvColumn = new DataGridViewTextBoxColumn();// 创建列
                        // 设置属性
                        tableColumns[i].DropDownBox.ComboBox = new Controls.ComboBoxEx();
                        tableColumns[i].DropDownBox.ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                        tableColumns[i].DropDownBox.ComboBox.Visible = false;
                        // 下拉框赋值
                        if (dropDownBox.Items != null)
                            tableColumns[i].DropDownBox.ComboBox.Items.AddRange(dropDownBox.Items);
                        SetDGVColumnReadOnly(dgvColumn, true);
                    }
                }
                else// 文本框列
                {
                    dgvColumn = new DataGridViewTextBoxColumn();
                    SetDGVColumnReadOnly(dgvColumn, tableColumns[i].IsReadonly);
                }

                dgvColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvColumn.Resizable = DataGridViewTriState.False;
                dgvColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvColumn.Name = "Column" + i.ToString();// 列名
                SetTableColumnWidth(dgvColumn, tableColumns[i]);// 表格列宽度
                // 单行表头
                if (table_SingleLineHeader != null)
                {
                    dgvColumn.HeaderText = tableColumns[i].Name;
                }

                dgv.Columns.Add(dgvColumn);
            }
        }

        /// <summary>
        /// 刷新表格列宽度
        /// </summary>
        public void RefreshTableColumnsWidth()
        {
            List<GBTableColumn> tableColumns = table.TableColumns;
            for (int i = 0; i < table.TableColumns.Count(); i++)
            {
                SetTableColumnWidth(dgv.Columns[i], tableColumns[i]);
            }
        }

        /// <summary>
        /// 设置表格列的宽度
        /// </summary>
        /// <param name="dgvColumn">表格列</param>
        /// <param name="tableColumn">表格列结构</param>
        private void SetTableColumnWidth(DataGridViewColumn dgvColumn, GBTableColumn tableColumn)
        {
            if (table.IsSetColumnAverageWidth)// 平均宽度
            {
                dgvColumn.Width = UCTableHelper.SetTableColumnAverageWidth(dgv.Width - 3, table.TableColumns.Count);
            }
            else
            {
                if (tableColumn.WidthPercentage.HasValue)
                    dgvColumn.Width = UCTableHelper.SetTableColumnPercentageWidth(dgv.Width - 3, tableColumn.WidthPercentage.Value);
            }
        }

        /// <summary>
        /// 设置表格列是否只读
        /// </summary>
        /// <param name="dgvColumn">表格列</param>
        /// <param name="isReadonly">是否只读</param>
        private void SetDGVColumnReadOnly(DataGridViewTextBoxColumn dgvColumn, bool isReadonly)
        {
            dgvColumn.ReadOnly = isReadonly;
        }

        /// <summary>
        /// 初始化表格和按钮事件
        /// </summary>
        private void InitialEvents()
        {
            // 表格
            dgv.CellEndEdit += dgv_CellEndEdit;
            if (IsAddColumnInputRangeEvent())
                dgv.EditingControlShowing += dgv_EditingControlShowing;
            if (IsAdddDropDownBoxDisplayEvent())
            {
                dgv.CellEnter += dgv_CellEnter;
            }
            if (IsAddNotModifiedDropDownBoxDisplayEvent())
            {
                dgv.Scroll += dgv_Scroll;
                dgv.Leave += dgv_Leave;
            }
            if (IsAddPopupWindowDisplayEvent())
            {
                dgv.CellMouseClick += dgv_CellMouseClick;
            }

            // 添加删除按钮
            if (table.IsExistAddDeleteButton)
            {
                pnlButton.Visible = true;
                btnAdd.Click += btnAdd_Click;
                btnDel.Click += btnDel_Click;
            }
        }

        /// <summary>
        /// 初始化表格数据
        /// </summary>
        private void InitialDGVDatas()
        {
            while (table.TableDatas.Count < table.DefaultTableRows)
            {
                T t = new T();
                table.TableDatas.Add(t);
            }
            dgv.Rows.Add(table.TableDatas.Count);
            for (int i = 0; i < table.TableDatas.Count; i++)
            {
                dgv.Rows[i].Tag = table.TableDatas[i];
                if (updateDataOfRowHandle != null)
                    updateDataOfRowHandle(dgv.Rows[i], true);
            }
            DGVLoadedAfterHandle();
        }

        /// <summary>
        /// 表格数据加载后的处理
        /// </summary>
        public void DGVLoadedAfterHandle()
        {
            HideSelectedColumnComboBox();// 第一次加载表格时第一列如果是下拉框隐藏
            dgv.CurrentCell = null;// 移除当前活动单元格焦点
        }

        void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (selectedColumnComboBox != null)
                selectedColumnComboBox.Visible = false;
            if (e.RowIndex < 0) return;
            DropDownBox dropDownBox = table.TableColumns[e.ColumnIndex].DropDownBox;
            if (dropDownBox == null) return;
            // 不可修改下拉框
            selectedColumnComboBox = dropDownBox.ComboBox;
            // 可修改下拉框
            comboBoxExColumn = dropDownBox.ComboBoxExColumn;
            if (selectedColumnComboBox == null) return;
            // 设置当前下拉框已选择项的索引
            SetCurrentComboBoxSelectedIndex(e.RowIndex, e.ColumnIndex);
            // 将下拉框移动到指定单元格位置
            selectedColumnComboBox.Parent = this.dgv;
            System.Drawing.Rectangle rect = this.dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            selectedColumnComboBox.Top = rect.Top;
            selectedColumnComboBox.Height = rect.Height;
            selectedColumnComboBox.Width = rect.Width;
            selectedColumnComboBox.Left = rect.Left - 1;
            selectedColumnComboBox.Visible = true;
        }

        void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (rowIndex < 0) return;// 鼠标点击了表头
            PopupWindow popupWindow = table.TableColumns[columnIndex].PopupWindow;
            if (popupWindow == null) return;
            GBTablePopupWindow tablePopupWindow = null;
            object cellCurrentValue = dgv[columnIndex, rowIndex].Value;
            if (popupWindow.GeneralPopupWindow != null)// 通用弹窗
            {
                tablePopupWindow = new GBTablePopupWindow(cellCurrentValue, popupWindow.GeneralPopupWindow.Items, popupWindow.GeneralPopupWindow.IsExistOther);
            }
            else if (popupWindow.SpecialPopupWindow != null)// 特殊弹窗
            {
                List<RowPopupWindow> list = popupWindow.SpecialPopupWindow.List;
                RowPopupWindow row = list.Find(s => s.RowIndex == rowIndex);
                if (row == null) return;
                tablePopupWindow = new GBTablePopupWindow(cellCurrentValue, row.Items, row.IsExistOther);
            }
            if (tablePopupWindow == null) return;
            tablePopupWindow.ShowDialog();
            if (tablePopupWindow.DialogResult == DialogResult.OK)
            {
                dgv[columnIndex, rowIndex].Value = tablePopupWindow.Result;
                updateDataOfRowHandle(dgv.Rows[rowIndex], false);
                // 光标移动到下一个位置
                if (columnIndex == dgv.Columns.Count - 1)
                {
                    int nextRowIndex = rowIndex + 1;
                    if (nextRowIndex < dgv.Rows.Count)
                        dgv.CurrentCell = dgv[0, nextRowIndex];
                    else
                        dgv.CurrentCell = null;
                }
                else
                {
                    dgv.CurrentCell = dgv[columnIndex + 1, rowIndex];
                }
            }
        }

        void dgv_Leave(object sender, EventArgs e)
        {
            HideSelectedColumnComboBox();
        }

        void dgv_Scroll(object sender, ScrollEventArgs e)
        {
            HideSelectedColumnComboBox();
        }

        /// <summary>
        /// 隐藏已选择列下拉框
        /// </summary>
        private void HideSelectedColumnComboBox()
        {
            if (selectedColumnComboBox == null) return;
            selectedColumnComboBox.Visible = false;
        }

        /// <summary>
        /// 是否添加输入限制事件
        /// </summary>
        private bool IsAddColumnInputRangeEvent()
        {
            return table.TableColumns.Exists(s => s.ColumnInputRange != GBColumnInputRange.Text);
        }

        /// <summary>
        /// 是否添加下拉框显示相关事件
        /// </summary>
        /// <returns></returns>
        private bool IsAdddDropDownBoxDisplayEvent()
        {
            var dropDownBoxColumns = table.TableColumns.Where(s => s.DropDownBox != null);
            if (dropDownBoxColumns.Count() == 0) return false;
            return true;
        }

        /// <summary>
        /// 是否添加不可修改下拉框显示相关事件
        /// </summary>
        /// <returns></returns>
        private bool IsAddNotModifiedDropDownBoxDisplayEvent()
        {
            var dropDownBoxColumns = table.TableColumns.Where(s => s.DropDownBox != null);
            if (dropDownBoxColumns.Count() == 0) return false;
            return dropDownBoxColumns.ToList().Exists(s => !s.DropDownBox.IsModified);
        }

        /// <summary>
        /// 是否添加弹窗显示事件
        /// </summary>
        /// <returns></returns>
        private bool IsAddPopupWindowDisplayEvent()
        {
            return table.TableColumns.Exists(s => s.PopupWindow != null
                && (s.PopupWindow.GeneralPopupWindow != null || s.PopupWindow.SpecialPopupWindow != null));
        }

        void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= GBController.EditControl_IntegerKeyPress;
            e.Control.KeyPress -= GBController.EditControl_KeyPress;
            e.Control.KeyPress -= GBController.EditControl_KeyPressEx;
            e.Control.KeyPress -= GBController.EditControl_IntegerKeyPressEx;
            int columnIndex = dgv.CurrentCell.ColumnIndex;
            GBColumnInputRange columnInputRange = table.TableColumns[columnIndex].ColumnInputRange;
            if (columnInputRange == GBColumnInputRange.Integer)
                e.Control.KeyPress += GBController.EditControl_IntegerKeyPress;
            else if (columnInputRange == GBColumnInputRange.Float)
                e.Control.KeyPress += GBController.EditControl_KeyPress;
            else if(columnInputRange == GBColumnInputRange.FloatNegative)
                e.Control.KeyPress += GBController.EditControl_KeyPressEx;
            else if(columnInputRange == GBColumnInputRange.IntegerNegative)
                e.Control.KeyPress += GBController.EditControl_IntegerKeyPressEx;
        }

        void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            cellEndEditColumnIndex = e.ColumnIndex;
            // 更新Tag数据 
            DataGridViewRow row = dgv.Rows[rowIndex];
            updateDataOfRowHandle(row, false);
            updateDataOfRowHandle(row, true);
            // 触发行改变事件
            table.TriggerRowChangedEvent(rowIndex);
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            T t = new T();
            int index = dgv.Rows.Add();
            dgv.Rows[index].Tag = t;
            table.TableDatas.Add(t);
            if (updateDataOfRowHandle != null)
                updateDataOfRowHandle(dgv.Rows[index], true);
            dgv.Rows[index].Selected = true;// 光标定位行
            dgv.FirstDisplayedScrollingRowIndex = dgv.Rows.Count - 1;// 下拉框到底
        }

        void btnDel_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count <= table.DefaultTableRows)
                return;
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                T pro = row.Tag as T;
                dgv.Rows.Remove(row);
                table.TableDatas.Remove(pro);
            }
            if (doCalHandle != null)
                doCalHandle();
            HideSelectedColumnComboBox();
        }

        /// <summary>
        /// 设置当前选中列下拉框包含项的集合
        /// </summary>
        /// <param name="items">包含项的数组</param>
        public void SetSelectedColumnComboBoxItems(string[] items)
        {
            if (selectedColumnComboBox != null)// 不可修改下拉框
            {
                if (items != null)
                {
                    selectedColumnComboBox.Items.Clear();
                    selectedColumnComboBox.Items.AddRange(items);
                }
            }
            else if (comboBoxExColumn != null)// 可修改下拉框
            {
                if (items != null)
                    comboBoxExColumn.DataSource = items;
            }
        }

        /// <summary>
        /// 设置当前下拉框已选择项的索引
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        public void SetCurrentComboBoxSelectedIndex(int rowIndex, int columnIndex)
        {
            if (selectedColumnComboBox == null) return;
            object selectedCellValue = dgv.Rows[rowIndex].Cells[columnIndex].Value;
            // 下拉框初始化值
            if (selectedCellValue == null || string.IsNullOrWhiteSpace(selectedCellValue.ToString()))
                selectedColumnComboBox.SelectedIndex = -1;
            else
                selectedColumnComboBox.SelectedItem = selectedCellValue;
        }

        /// <summary>
        /// 刷新表格数据
        /// </summary>
        public void RefreshTableData()
        {
            for (int i = 0; i < table.TableDatas.Count; i++)
            {
                RefreshRowData(i);
            }
        }

        /// <summary>
        /// 刷新指定行数据
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public void RefreshRowData(int rowIndex)
        {
            DataGridViewRow row = dgv.Rows[rowIndex];
            updateDataOfRowHandle(row, true);
        }

        /// <summary>
        /// 设置表格和标题的缩进级别
        /// </summary>
        /// <param name="indentationLevel"></param>
        public void SetTableIndentationLevel(GBIndentationLevel indentationLevel)
        {
            // 设置表格缩进后的宽度和表格位置
            UCTableHelper.SetTableIndentedWidthLocation(dgv, indentationLevel);
            // 设置表格标题的缩进级别
            SetTitleIndentationLevel(indentationLevel);
        }
    }

    /// <summary>
    /// 表格缩进级别
    /// </summary>
    public enum GBIndentationLevel
    {
        /// <summary>
        /// 无缩进
        /// </summary>
        Nono = 0,
        /// <summary>
        /// 一级缩进
        /// </summary>
        Level1,
        /// <summary>
        /// 二级缩进
        /// </summary>
        Level2,
        /// <summary>
        /// 三级缩进
        /// </summary>
        Level3
    }
}

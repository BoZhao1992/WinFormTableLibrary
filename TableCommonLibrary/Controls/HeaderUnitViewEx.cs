using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Diagnostics;

namespace TableCommonLibrary.Controls
{
    /// <summary>
    ///  DataGridView二维表头与合并单元格(扩展了行合并单元格)
    /// </summary>
    public partial class HeaderUnitViewEx : DataGridView
    {
        private TreeView[] _columnTreeView;
        private ArrayList _columnList = new ArrayList();
        private int _cellHeight = 17;
        public int CellHeight
        {
            get { return _cellHeight; }
            set { _cellHeight = value; }
        }
        private int _columnDeep = 1;
        private bool HscrollRefresh = false;
        /// <summary>  
        /// 水平滚动时是否刷新表头，数据较多时可能会闪烁，不刷新时可能显示错误  
        /// </summary>  
        [Description("水平滚动时是否刷新表头，数据较多时可能会闪烁，不刷新时可能显示错误")]
        public bool RefreshAtHscroll
        {
            get { return HscrollRefresh; }
            set { HscrollRefresh = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeaderUnitViewEx()
        {
            InitializeComponent();
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //设置列高度显示模式              
        }
        public HeaderUnitViewEx(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        [Description("设置或获得合并表头树的深度")]
        public int ColumnDeep
        {
            get
            {
                if (this.Columns.Count == 0)
                    _columnDeep = 1;
                this.ColumnHeadersHeight = _cellHeight * _columnDeep;
                return _columnDeep;
            }
            set
            {
                if (value < 1)
                    _columnDeep = 1;
                else
                    _columnDeep = value;
                this.ColumnHeadersHeight = _cellHeight * _columnDeep;
            }
        }

        [Description("添加合并式单元格绘制的所需要的节点对象")]
        public TreeView[] ColumnTreeView
        {
            get { return _columnTreeView; }
            set
            {
                if (_columnTreeView != null)
                {
                    for (int i = 0; i <= _columnTreeView.Length - 1; i++)
                        _columnTreeView[i].Dispose();
                }
                _columnTreeView = value;
            }
        }

        [Description("设置添加的字段树的相关属性")]
        public TreeView ColumnTreeViewNode
        {
            get { return _columnTreeView[0]; }
        }

        /// <summary>
        /// 设置或获取合并列的集合
        /// </summary>
        [MergableProperty(false)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Description("设置或获取合并列的集合"), Browsable(true), Category("单元格合并")]
        public List<string> MergeColumnNames
        {
            get
            {
                return _mergecolumnname;
            }
            set
            {
                _mergecolumnname = value;
            }
        }

        private List<string> _mergecolumnname = new List<string>();


        Dictionary<int, List<int>> mergeRowIndexs = new Dictionary<int, List<int>>();

        /// <summary>
        /// 合并行单元格（Key不可重复，否则会有问题）
        /// </summary>
        public Dictionary<int, List<int>> MergeRowIndexs
        {
            get { return mergeRowIndexs; }
            set { mergeRowIndexs = value; }
        }


        public ArrayList NadirColumnList
        {
            get
            {
                if (_columnTreeView == null)
                    return null;
                if (_columnTreeView[0] == null)
                    return null;
                if (_columnTreeView[0].Nodes == null)
                    return null;
                if (_columnTreeView[0].Nodes.Count == 0)
                    return null;
                _columnList.Clear();
                GetNadirColumnNodes(_columnList, _columnTreeView[0].Nodes[0], false);
                return _columnList;
            }
        }

        ///<summary>  
        ///绘制合并表头  
        ///</summary>  
        ///<param name="node">合并表头节点</param>  
        ///<param name="e">绘图参数集</param>  
        ///<param name="level">结点深度</param>  
        ///<remarks></remarks>  
        public void PaintUnitHeader(
                        TreeNode node,
                        System.Windows.Forms.DataGridViewCellPaintingEventArgs e,
                        int level)
        {
            //根节点时退出递归调用  
            if (level == 0)
                return;
            RectangleF uhRectangle;
            int uhWidth;
            SolidBrush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backColorBrush = new SolidBrush(e.CellStyle.BackColor);
            Pen gridLinePen = new Pen(gridBrush);
            StringFormat textFormat = new StringFormat();

            textFormat.Alignment = StringAlignment.Center;
            uhWidth = GetUnitHeaderWidth(node);
            if (node.Nodes.Count == 0)
            {
                uhRectangle = new Rectangle(e.CellBounds.Left,
                            e.CellBounds.Top + node.Level * _cellHeight,
                            uhWidth - 1,
                            _cellHeight * (_columnDeep - node.Level) - 1);
            }
            else
            {
                uhRectangle = new Rectangle(
                            e.CellBounds.Left,
                            e.CellBounds.Top + node.Level * _cellHeight,
                            uhWidth - 1,
                            _cellHeight - 1);
            }
            //画矩形  
            e.Graphics.FillRectangle(backColorBrush, uhRectangle);
            //划底线  
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Left
                                , uhRectangle.Bottom
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            //划右端线  
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Right
                                , uhRectangle.Top
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            ////写字段文本  
            e.Graphics.DrawString(node.Text, this.Font
                                    , new SolidBrush(e.CellStyle.ForeColor)
                                    , uhRectangle.Left + uhRectangle.Width / 2 -
                                    e.Graphics.MeasureString(node.Text, this.Font).Width / 2 - 1
                                    , uhRectangle.Top +
                                    uhRectangle.Height / 2 - e.Graphics.MeasureString(node.Text, this.Font).Height / 2);
            //递归调用()  
            if (node.PrevNode == null)
                if (node.Parent != null)
                    PaintUnitHeader(node.Parent, e, level - 1);
        }

        /// <summary>  
        /// 获得合并标题字段的宽度  
        /// </summary>  
        /// <param name="node">字段节点</param>  
        /// <returns>字段宽度</returns>  
        /// <remarks></remarks>  
        private int GetUnitHeaderWidth(TreeNode node)
        {
            //获得非最底层字段的宽度  
            int uhWidth = 0;
            //获得最底层字段的宽度  
            if (node.Nodes == null)
                return this.Columns[GetColumnListNodeIndex(node)].Width;
            if (node.Nodes.Count == 0)
                return this.Columns[GetColumnListNodeIndex(node)].Width;
            for (int i = 0; i <= node.Nodes.Count - 1; i++)
            {
                uhWidth = uhWidth + GetUnitHeaderWidth(node.Nodes[i]);
            }
            return uhWidth;
        }

        /// <summary>  
        /// 获得底层字段索引  
        /// </summary>  
        /// <param name="node">底层字段节点</param>  
        /// <returns>索引</returns>  
        /// <remarks></remarks>  
        private int GetColumnListNodeIndex(TreeNode node)
        {
            for (int i = 0; i <= _columnList.Count - 1; i++)
            {
                if (((TreeNode)_columnList[i]).Equals(node))
                    return i;
            }
            return -1;
        }

        /// <summary>  
        /// 获得底层字段集合  
        /// </summary>  
        /// <param name="alList">底层字段集合</param>  
        /// <param name="node">字段节点</param>  
        /// <param name="isChecked">向上搜索与否</param>  
        /// <remarks></remarks>  
        private void GetNadirColumnNodes(
                        ArrayList alList,
                        TreeNode node,
                        Boolean isChecked)
        {
            if (isChecked == false)
            {
                if (node.FirstNode == null)
                {
                    alList.Add(node);
                    if (node.NextNode != null)
                    {
                        GetNadirColumnNodes(alList, node.NextNode, false);
                        return;
                    }
                    if (node.Parent != null)
                    {
                        GetNadirColumnNodes(alList, node.Parent, true);
                        return;
                    }
                }
                else
                {
                    if (node.FirstNode != null)
                    {
                        GetNadirColumnNodes(alList, node.FirstNode, false);
                        return;
                    }
                }
            }
            else
            {
                if (node.FirstNode == null)
                {
                    return;
                }
                else
                {
                    if (node.NextNode != null)
                    {
                        GetNadirColumnNodes(alList, node.NextNode, false);
                        return;
                    }
                    if (node.Parent != null)
                    {
                        GetNadirColumnNodes(alList, node.Parent, true);
                        return;
                    }
                }
            }
        }

        /// <summary>  
        /// 滚动  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnScroll(ScrollEventArgs e)
        {
            bool scrollDirection = (e.ScrollOrientation == ScrollOrientation.HorizontalScroll);
            base.OnScroll(e);
            if (RefreshAtHscroll && scrollDirection)
                this.Refresh();
        }

        /// <summary>  
        /// 列宽度改变的重写  
        /// </summary>  
        /// <param name="e"></param>  
        protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            float uwh = g.MeasureString(e.Column.HeaderText, this.Font).Width;
            if (uwh >= e.Column.Width) { e.Column.Width = Convert.ToInt16(uwh); }
            base.OnColumnWidthChanged(e);
        }

        /// <summary>  
        /// 单元格绘制(重写)  
        /// </summary>  
        /// <param name="e"></param>  
        /// <remarks></remarks>  
        protected override void OnCellPainting(System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    DrawCell(e);
                    MergeRowCells(e);
                }
                else
                {
                    //行标题不重写  
                    if (e.ColumnIndex < 0)
                    {
                        base.OnCellPainting(e);
                        return;
                    }
                    if (_columnDeep == 1)
                    {
                        base.OnCellPainting(e);
                        return;
                    }
                    //绘制表头  
                    if (e.RowIndex == -1)
                    {
                        if (e.ColumnIndex >= NadirColumnList.Count) { e.Handled = true; return; }
                        PaintUnitHeader((TreeNode)NadirColumnList[e.ColumnIndex]
                                        , e
                                        , _columnDeep);
                        e.Handled = true;
                    }
                }
            }
            catch
            { }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // 至少应该有一列
            if (this.Columns.Count <= 0)
                return;

            // 找出字体大小,并算出比例(DPI)
            float dpiX;
            Graphics graphics = this.CreateGraphics();
            dpiX = graphics.DpiX;
            int percent = (int)(dpiX / 0.96);

            // DPI默认：100%
            if (percent == 100)
            {
                return;
            }

            // 调整列宽
            int dTotal = 0;
            foreach (DataGridViewColumn column in this.Columns)
            {
                column.Width = column.Width * percent / 100;
                dTotal += column.Width;
            }

            // 把余留的列宽均给第一列（名称列）
            int surplus = this.Width - dTotal - 3;
            if (surplus > 0)
            {
                DataGridViewColumn firstColumn = this.Columns[0];
                firstColumn.Width += surplus;
            }
        }

        #region 合并单元格

        /// <summary>
        /// 合并行单元格
        /// </summary>
        /// <param name="e"></param>
        private void MergeRowCells(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                bool isMerge = false;
                if (MergeRowIndexs == null) return;
                foreach (var key in MergeRowIndexs.Keys)
                {
                    if (key != e.RowIndex)
                    {
                        continue;
                    }

                    if (!MergeRowIndexs[key].Contains(e.ColumnIndex))
                    {
                        continue;
                    }

                    if (MergeRowIndexs[key][MergeRowIndexs[key].Count - 1] == e.ColumnIndex)
                    {
                        continue;
                    }

                    using (Brush gridBrush = new SolidBrush(this.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            // 擦除原单元格背景
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                            if (e.Value != null)
                            {
                                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font,
                                      Brushes.Black, e.CellBounds.X + 2,
                                      e.CellBounds.Y + 5, StringFormat.GenericDefault);
                            }

                            // 下边缘的线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                        }
                        e.Handled = true;
                    }
                }

                if (isMerge)
                {
                    e.Handled = true;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 画单元格
        /// </summary>
        /// <param name="e"></param>
        private void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            Brush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellwidth;
            //上面相同的行数
            int UpRows = 0;
            //下面相同的行数
            int DownRows = 0;
            //总行数
            int count = 0;
            if (this.MergeColumnNames == null) return;
            if (this.MergeColumnNames.Contains(this.Columns[e.ColumnIndex].Name) && e.RowIndex != -1)
            {
                cellwidth = e.CellBounds.Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = e.Value == null ? "" : e.Value.ToString();
                //string curSelected = this.CurrentRow.Cells[e.ColumnIndex].Value == null ? "" : this.CurrentRow.Cells[e.ColumnIndex].Value.ToString();
                if (!string.IsNullOrEmpty(curValue))
                {
                    #region 获取下面的行数
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                            DownRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                            UpRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    count = DownRows + UpRows - 1;
                    if (count < 2)
                    {
                        return;
                    }
                }
                if (this.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e, cellwidth, UpRows, DownRows, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    count = 0;
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 画字符串
        /// </summary>
        /// <param name="e"></param>
        /// <param name="cellwidth"></param>
        /// <param name="UpRows"></param>
        /// <param name="DownRows"></param>
        /// <param name="count"></param>
        private void PaintingFont(System.Windows.Forms.DataGridViewCellPaintingEventArgs e, int cellwidth, int UpRows, int DownRows, int count)
        {
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int fontheight = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Height;
            int fontwidth = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Width;
            int cellheight = e.CellBounds.Height;
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomLeft)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopCenter)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopLeft)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopRight)
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else
            {
                e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
        }
        #endregion
    }
}

using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*----------------------------------------------------
//文件名：GBTableColumn
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/10/24
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Common.UCData
{
    /// <summary>
    /// 表格列
    /// </summary>
    public class GBTableColumn
    {
        private readonly string name;
        private readonly double? widthPercentage;
        private readonly GBColumnInputRange columnInputRange;
        private readonly bool isReadonly;
        private readonly DropDownBox dropDownBox;

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        public GBTableColumn()
            : this("", null, GBColumnInputRange.Text, false, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        public GBTableColumn(string name)
            : this(name, null, GBColumnInputRange.Text, false, null)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="isReadonly">是否只读</param>
        public GBTableColumn(bool isReadonly)
            : this("", null, GBColumnInputRange.Text, isReadonly, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="isReadonly">是否只读</param>
        public GBTableColumn(string name, bool isReadonly)
            : this(name, null, GBColumnInputRange.Text, isReadonly, null)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="columnInputRange">列输入范围</param>
        public GBTableColumn(GBColumnInputRange columnInputRange)
            : this("", null, columnInputRange, false, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="columnInputRange">列输入范围</param>
        public GBTableColumn(string name, GBColumnInputRange columnInputRange)
            : this(name, null, columnInputRange, false, null)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="dropDownBox">下拉框</param>
        public GBTableColumn(DropDownBox dropDownBox)
            : this("", null, GBColumnInputRange.Text, false, dropDownBox)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="dropDownBox">下拉框</param>
        public GBTableColumn(string name, DropDownBox dropDownBox)
            : this(name, null, GBColumnInputRange.Text, false, dropDownBox)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="isReadonly">是否只读</param>
        public GBTableColumn(double? widthPercentage, bool isReadonly)
            : this("", widthPercentage, GBColumnInputRange.Text, isReadonly, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="isReadonly">是否只读</param>
        public GBTableColumn(string name, double? widthPercentage, bool isReadonly)
            : this(name, widthPercentage, GBColumnInputRange.Text, isReadonly, null)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="dropDownBox">下拉框</param>
        public GBTableColumn(double? widthPercentage, DropDownBox dropDownBox)
            : this("", widthPercentage, GBColumnInputRange.Text, false, dropDownBox)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="dropDownBox">下拉框</param>
        public GBTableColumn(string name, double? widthPercentage, DropDownBox dropDownBox)
            : this(name, widthPercentage, GBColumnInputRange.Text, false, dropDownBox)
        {
        }

        /// <summary>
        /// 构造函数（多行表头）
        /// </summary>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="columnInputRange">列输入范围</param>
        public GBTableColumn(double? widthPercentage, GBColumnInputRange columnInputRange)
            : this("", widthPercentage, columnInputRange, false, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="widthPercentage">宽度百分比</param>
        /// <param name="columnInputRange">列输入范围</param>
        /// <param name="isReadonly">是否只读</param>
        /// <param name="dropDownBox">下拉框</param>
        public GBTableColumn(string name, double? widthPercentage = null, GBColumnInputRange columnInputRange = GBColumnInputRange.Text,
            bool isReadonly = false, DropDownBox dropDownBox = null)
        {
            this.name = name;
            this.widthPercentage = widthPercentage;
            this.columnInputRange = columnInputRange;
            this.isReadonly = isReadonly;
            this.dropDownBox = dropDownBox;
        }

        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// 宽度百分比
        /// </summary>
        public double? WidthPercentage { get { return widthPercentage; } }

        /// <summary>
        /// 列输入范围
        /// </summary>
        public GBColumnInputRange ColumnInputRange { get { return columnInputRange; } }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool IsReadonly { get { return isReadonly; } }

        /// <summary>
        /// 下拉框
        /// </summary>
        public DropDownBox DropDownBox { get { return dropDownBox; } }

        /// <summary>
        /// 弹窗
        /// </summary>
        public PopupWindow PopupWindow { get; set; }
    }

    /// <summary>
    /// 表格列输入范围
    /// </summary>
    public enum GBColumnInputRange
    {
        /// <summary>
        /// 文本
        /// </summary>
        Text = 0,
        /// <summary>
        /// 浮点数
        /// </summary>
        Float,
        /// <summary>
        /// 整数
        /// </summary>
        Integer,
        /// <summary>
        ///  浮点数（可输入负数）
        /// </summary>
        FloatNegative,
        /// <summary>
        /// 整数（可输入负数）
        /// </summary>
        IntegerNegative
    }

    /// <summary>
    /// 下拉框
    /// </summary>
    public class DropDownBox
    {
        private readonly string[] items;
        private readonly bool isModified;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">下拉框集</param>
        /// <param name="isModified">下拉框是否可修改， 默认不可修改</param>
        public DropDownBox(string[] items, bool isModified = false)
        {
            this.items = items;
            this.isModified = isModified;
        }

        /// <summary>
        /// 下拉框是否可修改
        /// </summary>
        public bool IsModified { get { return isModified; } }

        /// <summary>
        /// 下拉框集
        /// </summary>
        public string[] Items { get { return items; } }

        /// <summary>
        /// 不可修改下拉框
        /// </summary>
        public Controls.ComboBoxEx ComboBox { get; set; }

        /// <summary>
        /// 可修改下拉框对应列
        /// </summary>
        public DataGridViewComboBoxExColumn ComboBoxExColumn { get; set; }
    }

    #region 弹窗
    /// <summary>
    /// 弹窗
    /// </summary>
    public class PopupWindow
    {
        public PopupWindow()
        {
        }

        public GeneralPopupWindow GeneralPopupWindow { get; set; }

        public SpecialPopupWindow SpecialPopupWindow { get; set; }
    }

    /// <summary>
    /// 通用弹窗：指定列中每行弹窗内容相同
    /// </summary>
    public class GeneralPopupWindow
    {
        private readonly string[] items;
        private readonly bool isExistOther;
        public GeneralPopupWindow(string[] items, bool isExistOther = true)
        {
            this.items = items;
            this.isExistOther = isExistOther;
        }

        public string[] Items { get { return items; } }

        public bool IsExistOther { get { return isExistOther; } }
    }

    /// <summary>
    /// 特殊弹窗：指定列中每行弹窗内容不同
    /// </summary>
    public class SpecialPopupWindow
    {
        private readonly List<RowPopupWindow> list;
        public SpecialPopupWindow(List<RowPopupWindow> list)
        {
            this.list = list;
        }

        public List<RowPopupWindow> List { get { return list; } }
    }

    /// <summary>
    /// 每行弹窗
    /// </summary>
    public class RowPopupWindow
    {
        private readonly int rowIndex;
        private readonly string[] items;
        private readonly bool isExistOther;
        public RowPopupWindow(int rowIndex, string[] items, bool isExistOther = true)
        {
            this.rowIndex = rowIndex;
            this.items = items;
            this.isExistOther = isExistOther;
        }

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get { return rowIndex; } }

        /// <summary>
        /// 要显示的子项集合
        /// </summary>
        public string[] Items { get { return items; } }

        /// <summary>
        /// 是否存在其他勾选项
        /// </summary>
        public bool IsExistOther { get { return isExistOther; } }
    }
    #endregion
}

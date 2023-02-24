using Project;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using TableCommonLibrary.Helper;
using TableCommonLibrary.UC;

/*----------------------------------------------------  
//文件名：UCTableHelper
//文件功能描述：界面表格帮助类
     
//创建标识：by Zhao Bo 2020/8/25
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Common.UCData
{
    /// <summary>
    /// 界面表格帮助类
    /// </summary>
    public class UCTableHelper
    {
        /// <summary>
        /// 设置表格参数
        /// </summary>
        /// <param name="dgv">表格</param>
        /// <param name="defaultDGVRows">表格行数</param>
        /// <param name="IsMultiHeaders">是否为多行头表格（ColumnDeep大于1）</param>
        /// <param name="headerLines">表头换行之后的行数，IsMultiHeaders为false时此参数才有效，默认没有换行为1</param>
        public static void SetTableParameter(DataGridView dgv, int defaultDGVRows, bool IsMultiHeaders, int headerLines = 1)
        {
            // 通用参数
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;//列自动填充
            dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            // 列头高度不可调整
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // 表格行高度
            dgv.RowTemplate.Height = dgv.RowTemplate.Height.ToScaling();

            // 设置ColumnHeadersHeight
            if (!IsMultiHeaders)
            {
                if (headerLines == 1)// 标题头单行
                    dgv.ColumnHeadersHeight = dgv.RowTemplate.Height;// 标题头的高度
                else// 标题头换行（如：UCPollutantControlTable界面表头）
                    dgv.ColumnHeadersHeight = (dgv.ColumnHeadersHeight * headerLines).ToScaling();// 标题头的高度
            }

            // 通用参数
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.ScrollBars = ScrollBars.Vertical;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 表格高度
            if (dgv.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.None)
                dgv.Size = new Size(dgv.Width, dgv.ColumnHeadersHeight + dgv.RowTemplate.Height * defaultDGVRows + 2);
            else if (dgv.AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.AllCells)
                dgv.Size = new Size(dgv.Width, dgv.ColumnHeadersHeight * (defaultDGVRows + 1) + 2);
        }

        /// <summary>
        /// 设置表格添加删除按钮的位置
        /// </summary>
        /// <param name="dgv">表格</param>
        /// <param name="btnAdd">添加按钮</param>
        /// <param name="btnDel">删除按钮</param>
        public static void SetTableAddDelButtonLocation(DataGridView dgv, Button btnAdd, Button btnDel)
        {
            SetTableViewButtonLocation(dgv, btnAdd);
            SetTableViewButtonLocation(dgv, btnDel);
        }

        private static void SetTableViewButtonLocation(DataGridView dgv, Button btn)
        {
            btn.Location = new Point(btn.Location.X, dgv.Location.Y + dgv.Size.Height + 8);
        }

        /// <summary>
        /// 设置表格列的平均宽度（四舍五入）
        /// </summary>
        /// <param name="width">表格宽度</param>
        /// <param name="columnNum">表格列数</param>
        /// <returns>平均每列大小</returns>
        public static int SetTableColumnAverageWidth(int width, int columnNum)
        {
            double originalWidth = width / App.ThisApp.DPI_Percent;// 缩放之前的宽度
            return (int)Math.Round(originalWidth / columnNum, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 设置表格列百分比宽度
        /// </summary>
        /// <param name="width"></param>
        /// <param name="Percentage"></param>
        /// <returns></returns>
        public static int SetTableColumnPercentageWidth(int width, double Percentage)
        {
            double originalWidth = width / App.ThisApp.DPI_Percent;// 缩放之前的宽度
            return (int)Math.Round(originalWidth * Percentage / 100, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 表格缩进后的宽度和表格位置
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="indentationLevel"></param>
        public static void SetTableIndentedWidthLocation(DataGridView dgv, GBIndentationLevel indentationLevel = GBIndentationLevel.Nono)
        {
            int indentationDistance = (16 * (int)indentationLevel).ToScaling();// 缩进距离
            dgv.Size = new Size(dgv.Width - indentationDistance, dgv.Height);
            dgv.Location = new Point(dgv.Location.X + indentationDistance, dgv.Location.Y);
        }

        /// <summary>
        /// 获取符合查找条件的XmlElement
        /// </summary>
        /// <param name="node">要查找的结点</param>
        /// <returns></returns>
        public static XmlElement GetXmlElement(string node)
        {
            if (node == null) return null;
            List<XmlElement> xes = App.ThisApp.XMLHelper.GetElements();
            if (xes == null) return null;
            return xes.Find(s => s.Name == node);
        }
    }
}

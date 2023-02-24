using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TableCommonLibrary.Helper
{
    /// <summary>
    /// 控件操作控制器
    /// </summary>
    public static class GBController
    {
        /// <summary>
        /// 加载图片到PictureBox控件中，若图片尺寸大于PictureBox的尺寸则同比例缩放
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="pictureBox">PictureBox控件</param>
        public static void LoadImageToPictureBox(Image image, PictureBox pictureBox)
        {
            try
            {
                Bitmap bmp = null;

                if (image.Width <= pictureBox.Width && image.Height <= pictureBox.Height)
                {
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    pictureBox.Image = image;
                    return;
                }
                else if (image.Width > pictureBox.Width && image.Height > pictureBox.Height)
                {
                    double widthRatio = (double)image.Width / (double)pictureBox.Width;
                    double heightRatio = (double)image.Height / (double)pictureBox.Height;
                    if (Math.Abs(widthRatio - heightRatio) < 1E-2)
                    {
                        bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
                    }
                    else if (heightRatio < widthRatio)
                    {
                        bmp = new Bitmap(pictureBox.Width, image.Height * pictureBox.Width / image.Width);
                    }
                    else
                    {
                        bmp = new Bitmap(image.Width * pictureBox.Height / image.Height, pictureBox.Height);
                    }
                }
                else if (image.Width > pictureBox.Width)
                {
                    bmp = new Bitmap(pictureBox.Width, image.Height * pictureBox.Width / image.Width);
                }
                else if (image.Height > pictureBox.Height)
                {
                    bmp = new Bitmap(image.Width * pictureBox.Height / image.Height, pictureBox.Height);
                }

                if (bmp != null)
                {
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                    pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                    pictureBox.Image = bmp;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 控制只允许输入浮点数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EditControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 46)
                {
                    e.Handled = true;
                }

                // 保证只输入一个小数点
                if (sender is System.Windows.Forms.TextBox)
                {
                    System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
                    if (null == textBox)
                        return;
                    string text = textBox.Text;
                    if (e.KeyChar == 46 && (text.IndexOf(".") >= 0))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 控制只允许输入浮点数（可输入负数）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EditControl_KeyPressEx(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 46 && e.KeyChar != 45)
                {
                    e.Handled = true;
                }

                // 保证只输入一个小数点、一个负号(且只能在第一位)
                if (sender is System.Windows.Forms.TextBox)
                {
                    System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
                    if (null == textBox)
                        return;
                    string text = textBox.Text;
                    if (e.KeyChar == 46 && (text.IndexOf(".") >= 0))
                    {
                        e.Handled = true;
                    }

                    if (e.KeyChar == 45 && (textBox.SelectionStart != 0 || text.IndexOf("-") >= 0))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 控制只允许整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EditControl_IntegerKeyPress(object sender, KeyPressEventArgs e)
        {
            Char keyChar = e.KeyChar;
            if (keyChar != '\b')//这是允许输入退格键
            {
                if ((keyChar < '0') || (keyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 控制只允许整数（可输入负整数）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void EditControl_IntegerKeyPressEx(object sender, KeyPressEventArgs e)
        {
            Char keyChar = e.KeyChar;
            if (keyChar != '\b')//这是允许输入退格键
            {
                //输入为负号时，只能输入一次且只能在第一位
                if (keyChar == '-')
                {
                    System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
                    if (textBox.SelectionStart != 0 || textBox.Text.IndexOf('-') >= 0)
                    {
                        e.Handled = true;
                    }
                    return;
                }

                if ((keyChar < '0') || (keyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// 合并DataGridView的单元格
        /// </summary>
        /// <param name="dgv">DataGridView控件</param>
        /// <param name="e">绘制环境对象</param>
        /// <param name="startRow">起始行</param>
        /// <param name="startCol">起始列</param>
        /// <param name="endRow">终止行</param>
        /// <param name="endCol">终止列</param>
        public static void MergeDataGridViewCells(DataGridView dgv, PaintEventArgs e, int startRow, int startCol, int endRow, int endCol)
        {
            try
            {
                // 判定参数范围的有效性
                int minRow = startRow, minCol = startCol, maxRow = endRow, maxCol = endCol;
                if (endRow < startRow) { minRow = endRow; maxRow = startRow; }
                if (endCol < startCol) { minCol = endCol; maxCol = startCol; }
                if (minRow < 0 || maxRow >= dgv.Rows.Count || minCol < 0 || maxCol >= dgv.Columns.Count)
                    return;
                if (minRow == maxRow && minCol == maxCol)
                    return;

                // 统计合并区域
                Rectangle minCellRect = dgv.GetCellDisplayRectangle(minCol, minRow, false);
                Rectangle maxCellRect = dgv.GetCellDisplayRectangle(maxCol, maxRow, false);
                Rectangle unionRect = Rectangle.Union(minCellRect, maxCellRect);

                // 合并区域的背景颜色
                Color cellBackColor = dgv.Rows[minRow].Cells[minCol].Tag == null ? Color.Empty : (Color)dgv.Rows[minRow].Cells[minCol].Tag;
                Color backColor = Color.Empty;
                for (int col = minCol; col <= maxCol; col++)
                {
                    for (int row = minRow; row <= maxRow; row++)
                    {
                        if (dgv.Rows[row].Cells[col].Selected)
                        {
                            backColor = dgv.DefaultCellStyle.SelectionBackColor;
                            break;
                        }
                    }

                    if (backColor != Color.Empty)
                    {
                        break;
                    }
                }

                if (backColor == Color.Empty)
                {
                    backColor = dgv.DefaultCellStyle.BackColor;
                }

                // 重绘背景
                using (Brush backColorBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backColorBrush, unionRect);
                }
                // 重绘网格线
                using (Pen gridLinePen = new Pen(dgv.GridColor))
                {
                    e.Graphics.DrawLine(gridLinePen, unionRect.Left, unionRect.Bottom - 1, unionRect.Right - 1, unionRect.Bottom - 1);
                    e.Graphics.DrawLine(gridLinePen, unionRect.Right - 1, unionRect.Top, unionRect.Right - 1, unionRect.Bottom - 1);
                }
                // 重绘文字
                for (int row = minRow; row <= maxRow; row++)
                    for (int col = minCol; col <= maxCol; col++)
                    {
                        DataGridViewCell cell = dgv.Rows[row].Cells[col];
                        Brush fontBrush = new SolidBrush(cell.InheritedStyle.ForeColor);
                        Rectangle cellRect = dgv.GetCellDisplayRectangle(col, row, false);
                        int x = cellRect.X;
                        int y = cellRect.Y + (cellRect.Height - cell.InheritedStyle.Font.Height) / 2 + 1;
                        e.Graphics.DrawString((String)cell.Value, cell.InheritedStyle.Font, fontBrush, x, y, StringFormat.GenericDefault);
                    }

                // 重绘合并区域
                if (backColor != cellBackColor)
                {
                    dgv.Invalidate(unionRect, false);
                    dgv.Rows[minRow].Cells[minCol].Tag = backColor;
                }
            }
            catch (Exception)
            {
            }
        }

        public static void MergeDataGridViewCellsEx(DataGridView dgv, PaintEventArgs e, int startRow, int startCol, int endRow, int endCol)
        {
            try
            {
                // 判定参数范围的有效性
                int minRow = startRow, minCol = startCol, maxRow = endRow, maxCol = endCol;
                if (endRow < startRow) { minRow = endRow; maxRow = startRow; }
                if (endCol < startCol) { minCol = endCol; maxCol = startCol; }
                if (minRow < 0 || maxRow >= dgv.Rows.Count || minCol < 0 || maxCol >= dgv.Columns.Count)
                    return;
                if (minRow == maxRow && minCol == maxCol)
                    return;

                // 统计合并区域
                Rectangle minCellRect = dgv.GetCellDisplayRectangle(minCol, minRow, false);
                Rectangle maxCellRect = dgv.GetCellDisplayRectangle(maxCol, maxRow, false);
                Rectangle unionRect = Rectangle.Union(minCellRect, maxCellRect);

                // 合并区域的背景颜色
                Color cellBackColor = dgv.Rows[minRow].Cells[minCol].Tag == null ? Color.Empty : (Color)dgv.Rows[minRow].Cells[minCol].Tag;
                Color backColor = Color.Empty;
                for (int col = minCol; col <= maxCol; col++)
                {
                    for (int row = minRow; row <= maxRow; row++)
                    {
                        if (dgv.Rows[row].Cells[col].Selected)
                        {
                            backColor = dgv.DefaultCellStyle.SelectionBackColor;
                            break;
                        }
                    }

                    if (backColor != Color.Empty)
                    {
                        break;
                    }
                }

                if (backColor == Color.Empty)
                {
                    backColor = dgv.DefaultCellStyle.BackColor;
                }

                // 重绘背景
                using (Brush backColorBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backColorBrush, unionRect);
                }
                // 重绘网格线
                using (Pen gridLinePen = new Pen(dgv.GridColor))
                {
                    e.Graphics.DrawLine(gridLinePen, unionRect.Left, unionRect.Bottom - 1, unionRect.Right - 1, unionRect.Bottom - 1);
                    e.Graphics.DrawLine(gridLinePen, unionRect.Right - 1, unionRect.Top, unionRect.Right - 1, unionRect.Bottom - 1);
                }
                // 重绘文字
                DataGridViewCell cell = dgv.Rows[minRow].Cells[minCol];
                int x = unionRect.X;
                int y = unionRect.Y;
                StringFormat textFormat = StringFormat.GenericDefault;
                if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.MiddleLeft) != 0)
                {
                    y = unionRect.Y + unionRect.Height / 2;
                    textFormat.Alignment = StringAlignment.Near;
                    textFormat.LineAlignment = StringAlignment.Center;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.BottomLeft) != 0)
                {
                    y = unionRect.Y + unionRect.Height;
                    textFormat.Alignment = StringAlignment.Near;
                    textFormat.LineAlignment = StringAlignment.Far;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.TopCenter) != 0)
                {
                    x = unionRect.X + unionRect.Width / 2;
                    textFormat.Alignment = StringAlignment.Center;
                    textFormat.LineAlignment = StringAlignment.Near;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.MiddleCenter) != 0)
                {
                    x = unionRect.X + unionRect.Width / 2;
                    y = unionRect.Y + unionRect.Height / 2;
                    textFormat.Alignment = StringAlignment.Center;
                    textFormat.LineAlignment = StringAlignment.Center;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.BottomCenter) != 0)
                {
                    x = unionRect.X + unionRect.Width / 2;
                    y = unionRect.Y + unionRect.Height;
                    textFormat.Alignment = StringAlignment.Center;
                    textFormat.LineAlignment = StringAlignment.Far;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.TopRight) != 0)
                {
                    x = unionRect.X + unionRect.Width;
                    textFormat.Alignment = StringAlignment.Far;
                    textFormat.LineAlignment = StringAlignment.Near;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.MiddleRight) != 0)
                {
                    x = unionRect.X + unionRect.Width;
                    y = unionRect.Y + unionRect.Height / 2;
                    textFormat.Alignment = StringAlignment.Far;
                    textFormat.LineAlignment = StringAlignment.Center;
                }
                else if ((cell.InheritedStyle.Alignment & DataGridViewContentAlignment.BottomRight) != 0)
                {
                    x = unionRect.X + unionRect.Width;
                    y = unionRect.Y + unionRect.Height;
                    textFormat.Alignment = StringAlignment.Far;
                    textFormat.LineAlignment = StringAlignment.Far;
                }

                Brush fontBrush = new SolidBrush(cell.InheritedStyle.ForeColor);
                e.Graphics.DrawString((String)cell.Value, cell.InheritedStyle.Font, fontBrush, x, y, textFormat);

                // 重绘合并区域
                if (backColor != cellBackColor)
                {
                    dgv.Invalidate(unionRect, false);
                    dgv.Rows[minRow].Cells[minCol].Tag = backColor;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*----------------------------------------------------
//文件名：GBTablePopupWindow
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/11/12,周四 18:23:25
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.UC
{
    public partial class GBTablePopupWindow : Form
    {
        private readonly string[] items;
        private readonly object cellCurrentValue;
        private readonly bool isExistOther;
        private const string Separator = "；";// 分隔符
        List<CheckBox> checkBoxs = new List<CheckBox>();
        TextBox textBox;// 其他文本框
        int totalWidth;// 界面总宽度
        int leftDistance = 20;// 下一个勾选框放置的左边距
        int height = 20;// 勾选项高度
        const int distance = 20;// 勾选项之间的间距
        public GBTablePopupWindow(object cellCurrentValue, string[] items, bool isExistOther)
        {
            if (items == null)
                throw new ArgumentNullException("items", "参数为空");
            this.cellCurrentValue = cellCurrentValue;
            this.items = items;
            this.isExistOther = isExistOther;
            InitializeComponent();
            totalWidth = this.panel1.Width;
        }

        public string Result { get; set; }

        private void GBTablePopupWindow_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < items.Length; i++)
            {
                // 创建控件
                Control control = CreateControl(i);
                // 调整位置
                AdjustPosition(control, i);
            }
        }

        /// <summary>
        /// 调整位置
        /// </summary>
        /// <param name="control"></param>
        /// <param name="i"></param>
        private void AdjustPosition(Control control, int i)
        {
            if (i == 0)
            {
                if ((leftDistance + control.PreferredSize.Width) > totalWidth)
                    throw new Exception(items[i] + "的宽度超出界面总宽度");
                control.Location = new Point(leftDistance, height);
                panel1.Controls.Add(control);
            }
            else
            {
                int temp = leftDistance + control.PreferredSize.Width;// 预计是否超出界面总宽度
                if (temp > totalWidth)
                {
                    leftDistance = 20;// 还原为20
                    if ((leftDistance + control.PreferredSize.Width) > totalWidth)
                        throw new Exception(items[i] + "的宽度超出界面总宽度");
                    height += control.PreferredSize.Height + distance;
                    control.Location = new Point(leftDistance, height);
                    panel1.Controls.Add(control);
                }
                else
                {
                    control.Location = new Point(leftDistance, height);
                    panel1.Controls.Add(control);
                }
            }
            leftDistance += control.PreferredSize.Width + distance;// 更新leftDistance
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Control CreateControl(int i)
        {
            List<string> cellCurrentValues = new List<string>();
            if (cellCurrentValue != null)
                cellCurrentValues = cellCurrentValue.ToString().Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Control control;
            if (i == items.Length - 1 && isExistOther)// 最后一个是其他勾选项
            {
                if (cellCurrentValues.Count() > 0)
                {
                    List<string> list = items.ToList();
                    list.RemoveAt(items.Length - 1);// 移除最后一项其他勾选项
                    var otherContents = cellCurrentValues.Except(list);// 其他勾选项内容
                    if (otherContents.Count() > 0)
                    {
                        control = CreateOtherItem(items[i], true, string.Join(Separator, otherContents));
                        return control;
                    }
                }
                control = CreateOtherItem(items[i], false);
            }
            else
            {
                if (cellCurrentValues.Count() > 0)
                    control = CreateItem(items[i], cellCurrentValues.Exists(s => s == items[i]));
                else
                    control = CreateItem(items[i], false);
            }

            return control;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            if (isExistOther)
            {
                for (int i = 0; i < checkBoxs.Count(); i++)
                {
                    if (i == checkBoxs.Count() - 1)
                    {
                        if (checkBoxs[i].Checked)
                            list.Add(textBox.Text);
                    }
                    else
                    {
                        if (checkBoxs[i].Checked)
                            list.Add(checkBoxs[i].Text);
                    }
                }
            }
            else
            {
                list = checkBoxs.Where(s => s.Checked).Select(s => s.Text).ToList();
            }
            Result = string.Join(Separator, list);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private CheckBox CreateItem(string text, bool state)
        {
            CheckBox checkBox = new CheckBox();
            checkBoxs.Add(checkBox);
            checkBox.AutoSize = true;
            checkBox.Text = text;
            checkBox.Location = new Point(0, 0);
            checkBox.Checked = state;
            checkBox.CheckedChanged += checkBox_CheckedChanged;
            return checkBox;
        }

        private Panel CreateOtherItem(string text, bool state, string content = "")
        {
            Panel panel = new Panel();
            panel.AutoSize = true;
            panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            CheckBox checkBox = CreateItem(text, state);
            panel.Controls.Add(checkBox);
            textBox = new TextBox();
            panel.Controls.Add(textBox);
            textBox.Location = new Point(checkBox.Location.X + checkBox.Size.Width + 1, checkBox.Location.Y);
            textBox.Size = new Size(150, textBox.Height);
            if (state)
            {
                textBox.Enabled = true;
                textBox.Text = content;
            }
            else
            {
                textBox.Enabled = false;
            }

            return panel;
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isExistOther)
            {
                CheckBox checkBox = (sender as CheckBox);
                if (checkBox.Text == items[items.Length - 1])// 其他勾选框
                {
                    if (textBox != null)
                        textBox.Enabled = checkBox.Checked;
                }
            }
        }
    }
}

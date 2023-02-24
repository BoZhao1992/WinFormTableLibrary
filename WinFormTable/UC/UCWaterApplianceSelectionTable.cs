using System;
using System.Linq;
using System.Windows.Forms;
using TableCommonLibrary.Helper;
using TableCommonLibrary.UC;
using WinFormTable.UCData;

namespace WinFormTable.UC
{
    /// <summary>
    /// 节水器具统计表
    /// </summary>
    public partial class UCWaterApplianceSelectionTable : UserControl
    {
        WaterApplianceSelectionTable waterApplianceSelectionTable;
        Action doCalHandle;
        UCGBTable<WaterApplianceSelectionPro> ucGBTable;
        TableCommonLibrary.Controls.ComboBoxEx comboBox0;// 第一列下拉框
        TableCommonLibrary.Controls.ComboBoxEx comboBox1;// 第二列下拉框

        public UCWaterApplianceSelectionTable()
        {
            InitializeComponent();
        }

        public UCWaterApplianceSelectionTable(WaterApplianceSelectionTable waterApplianceSelectionTable, Action doCal)
        {
            InitializeComponent();
            BindData(waterApplianceSelectionTable, doCal);
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="data"></param>
        /// <param name="doCal"></param>
        public void BindData(WaterApplianceSelectionTable waterApplianceSelectionTable, Action doCal)
        {
            this.waterApplianceSelectionTable = waterApplianceSelectionTable;
            this.doCalHandle = doCal;
            Initial();
        }

        private void Initial()
        {
            ucGBTable = new UCGBTable<WaterApplianceSelectionPro>(waterApplianceSelectionTable.GBTable);
            panel.Controls.Add(ucGBTable);
            ucGBTable.LoadDGV(UpdateDataOfRow, Execute);
            comboBox0 = ucGBTable.Table.TableColumns[0].DropDownBox.ComboBox;
            comboBox1 = ucGBTable.Table.TableColumns[1].DropDownBox.ComboBox;
            comboBox0.SelectedIndexChanged += comboBox0_SelectedIndexChanged;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            ucGBTable.dgv.CellEnter += dgv_CellEnter;
        }

        void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // 表格第二列下拉框动态设置下拉集合
            if (e.ColumnIndex != 1) return;
            object obj = ucGBTable.dgv.Rows[e.RowIndex].Cells[0].Value;// 卫生器具名称
            if (obj == null || string.IsNullOrWhiteSpace(obj.ToString()))
            {
                ucGBTable.SelectedColumnComboBox.Items.Clear();
                return;
            }
            if (obj.ToString() == waterApplianceSelectionTable.Names[5])// 大便器冲洗阀
                ucGBTable.SetSelectedColumnComboBoxItems(waterApplianceSelectionTable.FiveLevels);
            else // 其他
                ucGBTable.SetSelectedColumnComboBoxItems(waterApplianceSelectionTable.ThreeLevels);
            ucGBTable.SetCurrentComboBoxSelectedIndex(e.RowIndex, e.ColumnIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedItem = comboBox1.SelectedItem;
            if (selectedItem == null)
                return;

            DataGridViewRow row = ucGBTable.dgv.CurrentRow;
            WaterApplianceSelectionPro wap = row.Tag as WaterApplianceSelectionPro;
            if (wap.Grade == selectedItem.ToString())
                return;

            wap.Grade = selectedItem.ToString();
            WaterSavingApplianceChart chart = waterApplianceSelectionTable.WaterSavingApplianceCharts.SingleOrDefault(s => s.Name == wap.Name);
            if (chart != null)
            {
                GradeIndex gi = chart.GradeIndexs.SingleOrDefault(s => s.Grade == wap.Grade);
                if (gi != null)
                {
                    wap.RunOff = gi.RunOff;
                    UpdateDataOfRow(row, true);
                    return;
                }
            }
            wap.RunOff = string.Empty;
            UpdateDataOfRow(row, true);
        }

        void comboBox0_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedItem = comboBox0.SelectedItem;
            if (selectedItem == null)
                return;

            DataGridViewRow row = ucGBTable.dgv.CurrentRow;
            WaterApplianceSelectionPro pro = row.Tag as WaterApplianceSelectionPro;
            if (pro.Name == selectedItem.ToString())
                return;

            pro.Name = selectedItem.ToString();
            pro.Grade = string.Empty;
            pro.RunOff = string.Empty;
            UpdateDataOfRow(row, true);
        }

        private void UpdateDataOfRow(DataGridViewRow row, bool flag)
        {
            // 没有行数，返回
            if (row.Tag == null)
                return;

            WaterApplianceSelectionPro pro = (WaterApplianceSelectionPro)row.Tag;
            if (flag)// 显示界面
            {
                row.Cells[0].Value = pro.Name;
                row.Cells[1].Value = pro.Grade;
                row.Cells[2].Value = pro.RunOff;
                row.Cells[3].Value = pro.WaterSavingCount;
            }
            else// 保存数据
            {
                pro.Name = row.Cells[0].Value.ToFormatString();
                pro.Grade = row.Cells[1].Value.ToFormatString();
                pro.RunOff = row.Cells[2].Value.ToFormatString();
                pro.WaterSavingCount = row.Cells[3].Value.ToFormatString();
            }
            Execute();
        }

        public void Execute()
        {
            // 计算
            if (doCalHandle != null)
            {
                doCalHandle();
            }
        }
    }
}

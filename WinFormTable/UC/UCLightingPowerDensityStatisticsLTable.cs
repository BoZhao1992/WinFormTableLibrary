using System;
using System.Linq;
using System.Windows.Forms;
using TableCommonLibrary.Helper;
using TableCommonLibrary.Interface;
using TableCommonLibrary.UC;
using WinFormTable.UCData;

namespace WinFormTable.UC
{
    /// <summary>
    /// 居建照明功率密度统计表格
    /// </summary>
    public partial class UCLightingPowerDensityStatisticsLTable : UserControl, IGBUCSubTable<LightingPowerDensityStatisticsLPPro>
    {
        LightingPowerDensityStatisticsLTableSub sub;
        Action doCalHandle;

        public UCGBTable<LightingPowerDensityStatisticsLPPro> UCGBTable { get; set; }

        public UCLightingPowerDensityStatisticsLTable(LightingPowerDensityStatisticsLTableSub sub, Action doCalHandle)
        {
            InitializeComponent();
            this.sub = sub;
            this.doCalHandle = doCalHandle;
            Initial();
        }

        private void Initial()
        {
            UCGBTable = new UCGBTable<LightingPowerDensityStatisticsLPPro>(sub.GBTable);
            panel1.Controls.Add(UCGBTable);
            UCGBTable.LoadDGV(UpdateDataOfRow, Execute);
        }

        private void UpdateDataOfRow(DataGridViewRow row, bool flag)
        {
            // 没有行数，返回
            if (row.Tag == null)
                return;

            LightingPowerDensityStatisticsLPPro pro = (LightingPowerDensityStatisticsLPPro)row.Tag;
            if (flag)// 显示界面
            {
                row.Cells[0].Value = pro.Room;
                row.Cells[1].Value = pro.LX_DesignValue;
                row.Cells[2].Value = pro.LX_StandardValue;
                row.Cells[3].Value = pro.LPD_DesignValue;
                row.Cells[4].Value = pro.LPD_CurrentValue;
                row.Cells[5].Value = pro.LPD_AimsValue;
            }
            else// 保存数据
            {
                pro.LX_DesignValue = row.Cells[1].Value.ToFormatString(1);
                pro.LX_StandardValue = row.Cells[2].Value.ToFormatString(1);
                pro.LPD_DesignValue = row.Cells[3].Value.ToFormatString(1);
                pro.LPD_CurrentValue = row.Cells[4].Value.ToFormatString(1);
                pro.LPD_AimsValue = row.Cells[5].Value.ToFormatString(1);
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

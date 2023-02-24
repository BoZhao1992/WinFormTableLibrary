using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TableCommonLibrary.Helper;
using TableCommonLibrary.UC;
using WinFormTable.UCData;

namespace WinFormTable.UC
{
    /// <summary>
    /// 居建照明功率密度统计
    /// </summary>
    public partial class UCLightingPowerDensityStatisticsL : UserControl
    {
        LightingPowerDensityStatisticsL lightingPowerDensityStatisticsL;
        LightingPowerDensityStatisticsLData data;
        Action doCalHandle;

        public UCLightingPowerDensityStatisticsL()
        {
            InitializeComponent();
        }

        public UCLightingPowerDensityStatisticsL(LightingPowerDensityStatisticsL lightingPowerDensityStatisticsL, Action doCal)
        {
            InitializeComponent();
            BindData(lightingPowerDensityStatisticsL, doCal);
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="data"></param>
        /// <param name="doCal"></param>
        public void BindData(LightingPowerDensityStatisticsL lightingPowerDensityStatisticsL, Action doCal)
        {
            this.lightingPowerDensityStatisticsL = lightingPowerDensityStatisticsL;
            data = lightingPowerDensityStatisticsL.Data;
            this.doCalHandle = doCal;
            Initial();
        }

        private void Initial()
        {
            // 复式表格-户内功能房间
            UCGBTable_Composite<LightingPowerDensityStatisticsLPPro, LightingPowerDensityStatisticsLTableSub> ucGBTable_Composite =
                new UCGBTable_Composite<LightingPowerDensityStatisticsLPPro, LightingPowerDensityStatisticsLTableSub>(data);
            panel1.Controls.Add(ucGBTable_Composite);
            ucGBTable_Composite.LoadDGV(typeof(UCLightingPowerDensityStatisticsLTable), Execute, GBIndentationLevel.Nono);
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

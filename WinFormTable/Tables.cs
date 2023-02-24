using Project;
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormTable.UC;
using WinFormTable.UCData;

namespace WinFormTable
{
    public partial class Tables : Form
    {
        public Tables()
        {
            InitializeComponent();

            float dpiX, dpiY;
            Graphics graphics = this.CreateGraphics();
            dpiX = graphics.DpiX;
            dpiY = graphics.DpiY;

            double intPercent = dpiX / 0.96 / 100;

            // 软件所在系统的DPI百分比
            App.ThisApp.DPI_Percent = intPercent;
        }

        private void Tables_Load(object sender, EventArgs e)
        {
            var uc1 = new UCWaterApplianceSelectionTable(WaterApplianceSelectionTable.Instance, null);
            splitContainer1.Panel1.Controls.Add(uc1);

            var uc2 = new UCLightingPowerDensityStatisticsL(LightingPowerDensityStatisticsL.Instance, null);
            splitContainer1.Panel2.Controls.Add(uc2);
        }

        private void Tables_FormClosed(object sender, FormClosedEventArgs e)
        {
            WaterApplianceSelectionTable.Instance.WriteData();
            LightingPowerDensityStatisticsL.Instance.WriteData();
            // 保存XML文件
            App.ThisApp.XMLHelper.SaveXml();
        }
    }
}

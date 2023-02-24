using Project;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TableCommonLibrary.Common.UCData;
using TableCommonLibrary.Helper;
using TableCommonLibrary.Interface;

/*----------------------------------------------------
//文件名：LightingPowerDensityStatisticsL
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/8/12,周三 9:35:13
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace WinFormTable.UCData
{
    /// <summary>
    /// 居建照明功率密度统计
    /// </summary>
    public class LightingPowerDensityStatisticsL
    {
        const string Node_Root = "ProjectData/LightingPowerDensityStatisticsL";
        static LightingPowerDensityStatisticsL thisData = null;
        public static LightingPowerDensityStatisticsL Instance
        {
            get
            {
                if (thisData == null)
                    thisData = new LightingPowerDensityStatisticsL();

                return thisData;
            }
        }

        private LightingPowerDensityStatisticsL()
        {
            Data = new LightingPowerDensityStatisticsLData();
            ReadData();
        }

        public LightingPowerDensityStatisticsLData Data { get; set; }

        public void ReadData()
        {
            if (!App.ThisApp.XMLHelper.SelectNode(Node_Root))
                return;
            Data.ReadData();
        }

        public void WriteData()
        {
            if (!App.ThisApp.XMLHelper.SelectNode(XmlHelper.AddElement(App.ThisApp.XMLHelper.XmlDoc, Node_Root)))
                return;
            Data.WriteData();
        }
    }

    /// <summary>
    /// 数据
    /// </summary>
    public class LightingPowerDensityStatisticsLData : IGBTableComposite<LightingPowerDensityStatisticsLPPro, LightingPowerDensityStatisticsLTableSub>
    {
        public LightingPowerDensityStatisticsLData()
        {
            GBTableComposite = new List<LightingPowerDensityStatisticsLTableSub>();
        }

        public string Title { get; set; }

        /// <summary>
        /// 表格-户内功能房间
        /// </summary>
        public List<LightingPowerDensityStatisticsLTableSub> GBTableComposite { get; set; }
    }

    public class LightingPowerDensityStatisticsLTableSub : IGBTableSub<LightingPowerDensityStatisticsLPPro>
    {
        public LightingPowerDensityStatisticsLTableSub()
        {
            List<GBTableColumn> tableColumns = new List<GBTableColumn>()
            {
                new GBTableColumn(true),
                new GBTableColumn(GBColumnInputRange.Float),
                new GBTableColumn(GBColumnInputRange.Float),
                new GBTableColumn(GBColumnInputRange.Float),
                new GBTableColumn(GBColumnInputRange.Float),
                new GBTableColumn(GBColumnInputRange.Float),
            };
            List<LightingPowerDensityStatisticsLPPro> tableDatas = new List<LightingPowerDensityStatisticsLPPro>()
            {
                new LightingPowerDensityStatisticsLPPro() { Room = "卧室" },
                new LightingPowerDensityStatisticsLPPro() { Room = "起居室" },
                new LightingPowerDensityStatisticsLPPro() { Room = "餐厅" },
                new LightingPowerDensityStatisticsLPPro() { Room = "厨房" },
                new LightingPowerDensityStatisticsLPPro() { Room = "卫生间" },
            };

            #region 多行表头树结构
            TreeView tv = new TreeView();
            TreeNode tn1 = new TreeNode("户内功能房间");
            tv.Nodes.Add(tn1);

            TreeNode tn2 = new TreeNode("设计照度值（Lx）");
            tv.Nodes.Add(tn2);
            TreeNode tn2_1 = new TreeNode("设计值");
            tn2.Nodes.Add(tn2_1);
            TreeNode tn2_2 = new TreeNode("标准值");
            tn2.Nodes.Add(tn2_2);

            TreeNode tn3 = new TreeNode("照明功率密度（W/m²)");
            tv.Nodes.Add(tn3);
            TreeNode tn3_1 = new TreeNode("设计值");
            tn3.Nodes.Add(tn3_1);
            TreeNode tn3_2 = new TreeNode("现行值");
            tn3.Nodes.Add(tn3_2);
            TreeNode tn3_3 = new TreeNode("目标值");
            tn3.Nodes.Add(tn3_3);
            #endregion

            GBTable = new GBTable_MultiLineHeader<LightingPowerDensityStatisticsLPPro>(tableColumns, tableDatas, tv)
            {
                IsExistAddDeleteButton = false
            };
            Name = "户型";
        }

        /// <summary>
        /// 构件类型名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 表格
        /// </summary>
        public GBTable<LightingPowerDensityStatisticsLPPro> GBTable { get; set; }
    }

    public class LightingPowerDensityStatisticsLPPro : IGBTableProReadWrite
    {
        const string Att_Room = "Room";
        const string Att_LX_DesignValue = "LX_DesignValue";
        const string Att_LX_StandardValue = "LX_StandardValue";
        const string Att_LPD_DesignValue = "LPD_DesignValue";
        const string Att_LPD_CurrentValue = "LPD_CurrentValue";
        const string Att_LPD_AimsValue = "LPD_AimsValue";
        public LightingPowerDensityStatisticsLPPro()
        {
            Room = string.Empty;
            LX_DesignValue = string.Empty;
            LX_StandardValue = string.Empty;
            LPD_DesignValue = string.Empty;
            LPD_CurrentValue = string.Empty;
            LPD_AimsValue = string.Empty;
        }

        /// <summary>
        /// 功能房间
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// 设计照度值（Lx）-设计值
        /// </summary>
        public string LX_DesignValue { get; set; }

        /// <summary>
        /// 设计照度值（Lx）-标准值
        /// </summary>
        public string LX_StandardValue { get; set; }

        /// <summary>
        /// 照度（lx）：标准值*低倍数≤设计值≤高倍数*标准值
        /// </summary>
        /// <param name="lowMultiple">低倍数</param>
        /// <param name="highMultiple">高倍数</param>
        /// <returns></returns>
        public bool IsMeetLX(double lowMultiple, double highMultiple)
        {
            if (!string.IsNullOrWhiteSpace(LX_DesignValue) && !string.IsNullOrWhiteSpace(LX_StandardValue))
            {
                if (LX_DesignValue.ToDouble() >= LX_StandardValue.ToDouble() * lowMultiple
                    && LX_DesignValue.ToDouble() <= LX_StandardValue.ToDouble() * highMultiple)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 照明功率密度（W/m²)-设计值
        /// </summary>
        public string LPD_DesignValue { get; set; }

        /// <summary>
        /// 照明功率密度（W/m²)-现行值
        /// </summary>
        public string LPD_CurrentValue { get; set; }

        /// <summary>
        /// 照明功率密度（W/m²)-目标值
        /// </summary>
        public string LPD_AimsValue { get; set; }

        /// <summary>
        /// 照明功率密度（W/m²)：设计值≤现行值
        /// </summary>
        /// <returns></returns>
        public bool IsMeetLPDCurrentValue()
        {
            if (!string.IsNullOrWhiteSpace(LPD_DesignValue) && !string.IsNullOrWhiteSpace(LPD_CurrentValue))
            {
                if (LPD_DesignValue.ToDouble() <= LPD_CurrentValue.ToDouble())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 照明功率密度（W/m²)：设计值≤目标值
        /// </summary>
        /// <returns></returns>
        public bool IsMeetLPDAimsValue()
        {
            if (!string.IsNullOrWhiteSpace(LPD_DesignValue) && !string.IsNullOrWhiteSpace(LPD_AimsValue))
            {
                if (LPD_DesignValue.ToDouble() <= LPD_AimsValue.ToDouble())
                    return true;
            }
            return false;
        }

        public void ReadData()
        {
            Room = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_Room);
            LX_DesignValue = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_LX_DesignValue);
            LX_StandardValue = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_LX_StandardValue);
            LPD_DesignValue = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_LPD_DesignValue);
            LPD_CurrentValue = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_LPD_CurrentValue);
            LPD_AimsValue = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_LPD_AimsValue);
        }

        public void WriteData()
        {
            App.ThisApp.XMLHelper.SetAttribute(Att_Room, Room);
            App.ThisApp.XMLHelper.SetAttribute(Att_LX_DesignValue, LX_DesignValue);
            App.ThisApp.XMLHelper.SetAttribute(Att_LX_StandardValue, LX_StandardValue);
            App.ThisApp.XMLHelper.SetAttribute(Att_LPD_DesignValue, LPD_DesignValue);
            App.ThisApp.XMLHelper.SetAttribute(Att_LPD_CurrentValue, LPD_CurrentValue);
            App.ThisApp.XMLHelper.SetAttribute(Att_LPD_AimsValue, LPD_AimsValue);
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(LX_DesignValue) && !string.IsNullOrWhiteSpace(LX_StandardValue)
                && !string.IsNullOrWhiteSpace(LPD_DesignValue) && !string.IsNullOrWhiteSpace(LPD_CurrentValue);
        }
    }
}

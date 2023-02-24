using Project;
using System.Collections.Generic;
using System.Linq;
using TableCommonLibrary.Common.UCData;
using TableCommonLibrary.Helper;
using TableCommonLibrary.Interface;

/*----------------------------------------------------
//文件名：WaterApplianceSelectionTable
//文件功能描述：
//创建标识：by Zhao Bo 2020/8/18
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace WinFormTable.UCData
{
    /// <summary>
    /// 节水器具统计表
    /// </summary>
    public class WaterApplianceSelectionTable
    {
        const string Node_Root = "ProjectData/WaterApplianceSelectionTable";
        static WaterApplianceSelectionTable thisData = null;
        public static WaterApplianceSelectionTable Instance
        {
            get
            {
                if (thisData == null)
                    thisData = new WaterApplianceSelectionTable();

                return thisData;
            }
        }

        private WaterApplianceSelectionTable()
        {
            ReadData();
        }

        void InitializationData()
        {
            Names = new string[] { "水嘴", "单冲坐便器", "双冲坐便器", "小便器", "淋浴器", "大便器冲洗阀", "小便器冲洗阀", "蹲便器" };
            ThreeLevels = new string[] { "1级", "2级", "3级" };
            FiveLevels = new string[] { "1级", "2级", "3级", "4级", "5级" };
            #region 节水器具对照表初始化
            WaterSavingApplianceCharts = new List<WaterSavingApplianceChart>()
            {
                new WaterSavingApplianceChart()
                {
                    Name = "水嘴",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "0.100"},
                        new GradeIndex(){ Grade = "2级", RunOff = "0.125"},
                        new GradeIndex(){ Grade = "3级", RunOff = "0.150"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "小便器",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "2.0"},
                        new GradeIndex(){ Grade = "2级", RunOff = "3.0"},
                        new GradeIndex(){ Grade = "3级", RunOff = "4.0"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "淋浴器",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "0.08"},
                        new GradeIndex(){ Grade = "2级", RunOff = "0.12"},
                        new GradeIndex(){ Grade = "3级", RunOff = "0.15"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "大便器冲洗阀",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "4.0"},
                        new GradeIndex(){ Grade = "2级", RunOff = "5.0"},
                        new GradeIndex(){ Grade = "3级", RunOff = "6.0"},
                        new GradeIndex(){ Grade = "4级", RunOff = "7.0"},
                        new GradeIndex(){ Grade = "5级", RunOff = "8.0"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "小便器冲洗阀",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "2.0"},
                        new GradeIndex(){ Grade = "2级", RunOff = "3.0"},
                        new GradeIndex(){ Grade = "3级", RunOff = "4.0"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "蹲便器",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "5.0"},
                        new GradeIndex(){ Grade = "2级", RunOff = "6.0"},
                        new GradeIndex(){ Grade = "3级", RunOff = "8.0"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "双冲坐便器",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "全冲≤5.0、半冲≤3.5"},
                        new GradeIndex(){ Grade = "2级", RunOff = "全冲≤6.0、半冲≤4.2"},
                        new GradeIndex(){ Grade = "3级", RunOff = "全冲≤8.0、半冲≤5.6"}
                    }
                },
                new WaterSavingApplianceChart()
                {
                    Name = "单冲坐便器",
                    GradeIndexs = new List<GradeIndex>()
                    {
                        new GradeIndex(){ Grade = "1级", RunOff = "≤4.0"},
                        new GradeIndex(){ Grade = "2级", RunOff = "≤5.0"},
                        new GradeIndex(){ Grade = "3级", RunOff = "≤6.4"}
                    }
                },
            };
            #endregion
            List<WaterApplianceSelectionPro> tableDatas = new List<WaterApplianceSelectionPro>();
            List<GBTableColumn> tableColumns = new List<GBTableColumn>()
            {
                new GBTableColumn("卫生器具名称", new DropDownBox(Names)),
                new GBTableColumn("用水效率等级", new DropDownBox(null)),
                new GBTableColumn("流量（L/s）或用水量（L）", true),
                new GBTableColumn("卫生器具数量", GBColumnInputRange.Integer)
            };
            GBTable = new GBTable_SingleLineHeader<WaterApplianceSelectionPro>(tableColumns, tableDatas)
            {
                Title = "节水器具统计表："
            };
        }

        /// <summary>
        /// 表格
        /// </summary>
        public GBTable_SingleLineHeader<WaterApplianceSelectionPro> GBTable { get; set; }

        /// <summary>
        /// 卫生器具名称列下拉框数据集
        /// </summary>
        public string[] Names { get; set; }

        /// <summary>
        /// 用水效率等级(三种级别)
        /// </summary>
        public string[] ThreeLevels { get; set; }

        /// <summary>
        /// 用水效率等级(五种级别)
        /// </summary>
        public string[] FiveLevels { get; set; }

        /// <summary>
        /// 节水器具对照表
        /// </summary>
        public List<WaterSavingApplianceChart> WaterSavingApplianceCharts { get; set; }

        public void ReadData()
        {
            InitializationData();
            if (!App.ThisApp.XMLHelper.SelectNode(Node_Root))
                return;
            GBTable.ReadData();
        }

        public void WriteData()
        {
            if (!App.ThisApp.XMLHelper.SelectNode(XmlHelper.AddElement(App.ThisApp.XMLHelper.XmlDoc, Node_Root)))
                return;
            GBTable.WriteData();
        }
    }

    /// <summary>
    /// 表格属性
    /// </summary>
    public class WaterApplianceSelectionPro : IGBTableProReadWrite
    {
        const string Att_Name = "Name";
        const string Att_Grade = "Grade";
        const string Att_RunOff = "RunOff";
        const string Att_WaterSavingCount = "WaterSavingCount";
        public WaterApplianceSelectionPro()
        {
            Name = string.Empty;
            Grade = string.Empty;
            RunOff = string.Empty;
            WaterSavingCount = string.Empty;
        }

        /// <summary>
        /// 卫生器具名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用水效率等级 
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// 流量（L/s）或用水量（L）
        /// </summary>
        public string RunOff { get; set; }

        /// <summary>
        /// 卫生器具数量
        /// </summary>
        public string WaterSavingCount { get; set; }

        /// <summary>
        /// 表格有效性
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Grade)
                && !string.IsNullOrWhiteSpace(RunOff) && !string.IsNullOrWhiteSpace(WaterSavingCount);
        }

        public void ReadData()
        {
            Name = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_Name);
            Grade = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_Grade);
            RunOff = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_RunOff);
            WaterSavingCount = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_WaterSavingCount);
        }

        public void WriteData()
        {
            App.ThisApp.XMLHelper.SetAttribute(Att_Name, Name);
            App.ThisApp.XMLHelper.SetAttribute(Att_Grade, Grade);
            App.ThisApp.XMLHelper.SetAttribute(Att_RunOff, RunOff);
            App.ThisApp.XMLHelper.SetAttribute(Att_WaterSavingCount, WaterSavingCount);
        }
    }

    /// <summary>
    /// 节水器具对照表
    /// </summary>
    public class WaterSavingApplianceChart
    {
        public WaterSavingApplianceChart()
        {
            Name = string.Empty;
            GradeIndexs = new List<GradeIndex>();
        }

        public string Name { get; set; }

        public List<GradeIndex> GradeIndexs { get; set; }
    }

    /// <summary>
    /// 用水效率等级指标
    /// </summary>
    public class GradeIndex
    {
        public GradeIndex()
        {
            Grade = string.Empty;
            RunOff = string.Empty;
        }

        public string Grade { get; set; }

        public string RunOff { get; set; }
    }
}

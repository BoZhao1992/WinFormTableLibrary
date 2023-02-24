using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TableCommonLibrary.Interface;

/*----------------------------------------------------  
//文件名：GBTable
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/10/15,周四 19:22:37
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Common.UCData
{
    /// <summary>
    /// 表格抽象类
    /// </summary>
    /// <typeparam name="TPro">表格列属性类型</typeparam>
    public abstract class GBTable<TPro>
        where TPro : class, IGBTableProReadWrite, new()
    {
        private const string NodeTableMark = "Table_";
        private const string NodeTableRow = "Row";
        private readonly List<GBTableColumn> tableColumns;
        private readonly List<TPro> tableDatas;
        private string nodeTable;
        /// <summary>
        /// 表格行改变事件
        /// </summary>
        public event Action<int> GBRowChanged;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableColumns">表格列集</param>
        /// <param name="tableDatas">表格数据集</param>
        public GBTable(List<GBTableColumn> tableColumns, List<TPro> tableDatas)
        {
            if (tableColumns == null)
                throw new ArgumentNullException("tableColumns", "参数为空");
            this.tableColumns = tableColumns;
            if (tableDatas == null)
                throw new ArgumentNullException("tableDatas", "参数为空");
            this.tableDatas = tableDatas;
            // xml表结点名称
            Type type = typeof(TPro);
            nodeTable = NodeTableMark + type.Name;
            // 初始化
            Title = string.Empty;
            IsSetColumnAverageWidth = true;
            IsExistAddDeleteButton = true;
            DefaultTableRows = 5;
        }

        /// <summary>
        /// 表格列集
        /// </summary>
        public List<GBTableColumn> TableColumns { get { return tableColumns; } }

        /// <summary>
        /// 表格数据集
        /// </summary>
        public List<TPro> TableDatas { get { return tableDatas; } }

        /// <summary>
        /// 表格标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否设置列为平均宽度，默认true设置为平均宽度
        /// </summary>
        public bool IsSetColumnAverageWidth { get; set; }

        /// <summary>
        /// 是否存在添加删除按钮，默认true存在添加删除按钮
        /// </summary>
        public bool IsExistAddDeleteButton { get; set; }

        /// <summary>
        /// 默认表格行数，默认5行
        /// </summary>
        public int DefaultTableRows { get; set; }

        #region 事件
        /// <summary>
        /// 触发行改变事件
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        public void TriggerRowChangedEvent(int rowIndex)
        {
            Action<int> handle = GBRowChanged;
            if (handle != null)
            {
                handle(rowIndex);
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        /// <param name="action"></param>
        public void BindEvent(Action<int> action)
        {
            this.GBRowChanged += action;
        }

        /// <summary>
        /// 注销事件
        /// </summary>
        public void LogoutEvent(Action<int> action)
        {
            if (this != null)
                this.GBRowChanged -= action;
        }
        #endregion

        #region 读写
        /// <summary>
        /// 读数据，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="customNode">自定义节点</param>
        public void ReadData(string customNode = "")
        {
            string node = nodeTable;
            if (!string.IsNullOrWhiteSpace(customNode))
                node = customNode;
            XmlElement xe = UCTableHelper.GetXmlElement(node);
            if (xe == null) return;
            if (!App.ThisApp.XMLHelper.SelectNode(xe))
                return;
            List<XmlElement> xmlElements = App.ThisApp.XMLHelper.GetElements();
            if (tableDatas.Count() == 0)
            {
                foreach (var item in xmlElements)
                {
                    if (!App.ThisApp.XMLHelper.SelectNode(item))
                        continue;
                    TPro t = new TPro();
                    tableDatas.Add(t);
                    t.ReadData();
                }
            }
            else// 表格已赋值数据
            {
                for (int i = 0; i < xmlElements.Count(); i++)
                {
                    if (!App.ThisApp.XMLHelper.SelectNode(xmlElements[i]))
                        continue;
                    if (i < tableDatas.Count())
                        tableDatas[i].ReadData();
                    else
                    {
                        TPro t = new TPro();
                        tableDatas.Add(t);
                        t.ReadData();
                    }
                }
            }
        }

        /// <summary>
        /// 写数据，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <param name="customNode">自定义节点</param>
        public void WriteData(string customNode = "")
        {
            string node = nodeTable;
            if (!string.IsNullOrWhiteSpace(customNode))
                node = customNode;
            XmlElement xe = UCTableHelper.GetXmlElement(node);
            if (xe == null)
            {
                XmlElement xmlElement = App.ThisApp.XMLHelper.AddElement(node);
                WriteTableRowDatas(xmlElement);
            }
            else
            {
                if (!App.ThisApp.XMLHelper.SelectNode(xe))
                    return;
                App.ThisApp.XMLHelper.ClearChildNodes();
                WriteTableRowDatas(xe);
            }
        }

        /// <summary>
        /// 写入表格行数据
        /// </summary>
        /// <param name="xmlElement">表节点</param>
        private void WriteTableRowDatas(XmlElement xmlElement)
        {
            foreach (TPro t in tableDatas)
            {
                if (!App.ThisApp.XMLHelper.SelectNode(xmlElement))
                    return;
                if (!App.ThisApp.XMLHelper.SelectNode(App.ThisApp.XMLHelper.AddElement(NodeTableRow)))
                    continue;
                t.WriteData();
            }
        }
        #endregion
    }
}

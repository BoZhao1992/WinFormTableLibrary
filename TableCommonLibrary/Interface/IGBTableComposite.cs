using Project;
using System;
using System.Collections.Generic;
using System.Xml;
using TableCommonLibrary.Common.UCData;

/*----------------------------------------------------
//文件名：IGBTableComposite
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/9/28,周一 11:26:39
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Interface
{
    /// <summary>
    /// 复式表格接口
    /// </summary>
    /// <typeparam name="TPro">表格属性</typeparam>
    /// <typeparam name="TSub">复式表格子表</typeparam>
    public interface IGBTableComposite<TPro, TSub>
        where TPro : class, IGBTableProReadWrite, new()
        where TSub : class, IGBTableSub<TPro>, new()
    {
        /// <summary>
        /// 表格标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 复式表格集合
        /// </summary>
        List<TSub> GBTableComposite { get; set; }
    }

    /// <summary>
    /// 复式表格子表接口
    /// </summary>
    /// <typeparam name="TPro">表格属性</typeparam>
    public interface IGBTableSub<TPro>
        where TPro : class, IGBTableProReadWrite, new()
    {
        GBTable<TPro> GBTable { get; set; }

        /// <summary>
        /// 子表表名，默认公建为“建筑”， 住宅为“户型”
        /// </summary>
        string Name { set; get; }
    }

    /// <summary>
    /// 复式表格子表读写接口（除子表中接口表格之外的数据读写）
    /// </summary>
    public interface IGBTableSubReadWrite
    {
        /// <summary>
        /// 读数据
        /// </summary>
        void ReadData();

        /// <summary>
        /// 写数据
        /// </summary>
        void WriteData();
    }

    /// <summary>
    /// 复式表格读写扩展
    /// </summary>
    public static class GBTableCompositeReadWriteEx
    {
        const string Att_Name = "Name";
        /// <summary>
        /// 读数据，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <typeparam name="TPro">表格属性</typeparam>
        /// <typeparam name="TSub">复式表格子表</typeparam>
        /// <param name="tableComposite"></param>
        public static void ReadData<TPro, TSub>(this IGBTableComposite<TPro, TSub> tableComposite)
            where TPro : class, IGBTableProReadWrite, new()
            where TSub : class, IGBTableSub<TPro>, new()
        {
            if (tableComposite == null)
                throw new ArgumentNullException("tableComposite", "参数为空");
            string nodeTableComposite = tableComposite.GetType().Name;
            XmlElement xe_tableComposite = UCTableHelper.GetXmlElement(nodeTableComposite);
            if (xe_tableComposite == null) return;
            if (!App.ThisApp.XMLHelper.SelectNode(xe_tableComposite))
                return;
            List<XmlElement> xes = App.ThisApp.XMLHelper.GetElements();
            foreach (XmlElement xe in xes)
            {
                if (!App.ThisApp.XMLHelper.SelectNode(xe))
                    continue;
                TSub sub = new TSub();
                tableComposite.GBTableComposite.Add(sub);
                sub.Name = App.ThisApp.XMLHelper.GetAttributeValueEx(Att_Name);
                IGBTableSubReadWrite subReadWrite = sub as IGBTableSubReadWrite;
                if (subReadWrite != null)// 存在表格之外的数据
                    subReadWrite.ReadData();
                sub.GBTable.ReadData();
            }
        }

        /// <summary>
        /// 写数据，通常情况下先调用SelectNode进行定位
        /// </summary>
        /// <typeparam name="TPro">表格属性</typeparam>
        /// <typeparam name="TSub">复式表格子表</typeparam>
        /// <param name="tableComposite"></param>
        public static void WriteData<TPro, TSub>(this IGBTableComposite<TPro, TSub> tableComposite)
            where TPro : class, IGBTableProReadWrite, new()
            where TSub : class, IGBTableSub<TPro>, new()
        {
            if (tableComposite == null)
                throw new ArgumentNullException("tableComposite", "参数为空");
            string nodeTableComposite = tableComposite.GetType().Name;
            XmlElement xe = UCTableHelper.GetXmlElement(nodeTableComposite);
            if (xe == null)
            {
                XmlElement xe_TableComposite = App.ThisApp.XMLHelper.AddElement(nodeTableComposite);
                WriteTableCompositeRowDatas(tableComposite, xe_TableComposite);
            }
            else
            {
                if (!App.ThisApp.XMLHelper.SelectNode(xe))
                    return;
                App.ThisApp.XMLHelper.ClearChildNodes();
                WriteTableCompositeRowDatas(tableComposite, xe);
            }
        }

        /// <summary>
        /// 写入复式表格数据
        /// </summary>
        /// <param name="xmlElement">表节点</param>
        private static void WriteTableCompositeRowDatas<TPro, TSub>(IGBTableComposite<TPro, TSub> tableComposite, XmlElement xmlElement)
            where TPro : class, IGBTableProReadWrite, new()
            where TSub : class, IGBTableSub<TPro>, new()
        {
            foreach (TSub sub in tableComposite.GBTableComposite)
            {
                if (!App.ThisApp.XMLHelper.SelectNode(xmlElement))
                    return;
                XmlElement xe_TableSub = App.ThisApp.XMLHelper.AddElement(typeof(TSub).Name);
                if (!App.ThisApp.XMLHelper.SelectNode(xe_TableSub))
                    return;
                App.ThisApp.XMLHelper.SetAttribute(Att_Name, sub.Name);
                IGBTableSubReadWrite subReadWrite = sub as IGBTableSubReadWrite;
                if (subReadWrite != null)// 存在表格之外的数据
                    subReadWrite.WriteData();
                sub.GBTable.WriteData();
            }
        }
    }
}

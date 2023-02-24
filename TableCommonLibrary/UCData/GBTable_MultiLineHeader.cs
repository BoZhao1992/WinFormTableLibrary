using TableCommonLibrary.Interface;
using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*----------------------------------------------------
//文件名：GBTable_MultiLineHeader
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/9/4
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Common.UCData
{
    /// <summary>
    /// 表格-多行表头
    /// </summary>
    /// <typeparam name="T">表格列属性类型</typeparam>
    public class GBTable_MultiLineHeader<T> : GBTable<T>
        where T : class, IGBTableProReadWrite, new()
    {
        private readonly TreeView treeView;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableColumns">表格列集合</param>
        /// <param name="tableDatas">表格数据集</param>
        /// <param name="treeView">多行表头树视图</param>
        public GBTable_MultiLineHeader(List<GBTableColumn> tableColumns, List<T> tableDatas, TreeView treeView)
            : base(tableColumns, tableDatas)
        {
            this.treeView = treeView;
            Deep = 2;
        }

        /// <summary>
        /// 多行表头树视图
        /// </summary>
        public TreeView TreeView { get { return treeView; } }

        /// <summary>
        /// 多行表头深度，默认深度为2
        /// </summary>
        public int Deep { get; set; }
    }
}

using System.Collections.Generic;
using TableCommonLibrary.Interface;

/*----------------------------------------------------  
//文件名：GBTable
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/9/4
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Common.UCData
{
    /// <summary>
    /// 表格-单行表头
    /// </summary>
    /// <typeparam name="T">表格列属性类型</typeparam>
    public class GBTable_SingleLineHeader<T> : GBTable<T>
        where T : class, IGBTableProReadWrite, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary
        /// <param name="tableColumns">表格列集合</param>
        /// <param name="tableDatas">表格数据集</param>
        public GBTable_SingleLineHeader(List<GBTableColumn> tableColumns, List<T> tableDatas)
            : base(tableColumns, tableDatas)
        {
            HeaderLines = 1;
        }

        /// <summary>
        /// 表头行数，默认1行
        /// </summary>
        public int HeaderLines { get; set; }
    }
}

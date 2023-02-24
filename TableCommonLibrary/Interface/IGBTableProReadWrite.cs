using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*----------------------------------------------------  
//文件名：IGBTablePro
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/10/13,周二 17:11:22
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Interface
{
    /// <summary>
    /// 表格属性读写接口
    /// </summary>
    public interface IGBTableProReadWrite
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
}

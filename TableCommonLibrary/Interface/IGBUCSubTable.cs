using TableCommonLibrary.UC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/*----------------------------------------------------
//文件名：IGBUCSubTable
//文件功能描述：
     
//创建标识：by Zhao Bo 2020/10/28,周三 18:40:28
//修改标识：
//修改描述：
//---------------------------------------------------- */

namespace TableCommonLibrary.Interface
{
    public interface IGBUCSubTable<TPro>
        where TPro : class, IGBTableProReadWrite, new()
    {
        UCGBTable<TPro> UCGBTable { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasS.Base.Lib
{
    /// <summary>
    /// 本地表定义
    /// </summary>
    public class SLocalDataDef
    {
        public string LocalName;

        /// <summary>
        /// 1 整个表同步到本地表中；
        /// 2 通过语句查询结果同步到本地表中；
        /// 3 通过语句查询结构同步到本地表中(不同步数据)；
        /// 4 通过语句查询主数据库(不存在本地表中)；
        /// 5  应用程序中添加的查本地存在表；
        /// 6  应用程序中添加通过语句查询本地存在表；
        /// 7 本地表的查询语句(需要写本地数据库的语句)；
        /// 8 本地表的视图(需要写本地数据库的视图语句)；
        /// 9 本地存储过程；（不存储在这里）
        /// 10 手动增加的表
        /// </summary>
        public int DefType;
        public int SDbType;
        public string SDataBase;
        public string STableName;
        public string SSQLCode;
        public int Scope;
        public string MySQLCode;
         
        public string SQLiteCode;
        public bool Enable;
        public string Note;
        public DateTime  upTime;
 
    }
}

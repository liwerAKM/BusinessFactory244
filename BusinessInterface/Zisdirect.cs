using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    /// <summary>
    /// 无其他用处，只是在为了判断此DLL是否是提供业务的DLL
    /// </summary>
    public   interface  IZisdirect
    {
        /// <summary>
        /// 说明
        /// </summary>
        /// <returns></returns>
        string Note();

        /// <summary>
        /// FullName  Type
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Type> GetTypes();


        /// <summary>
        /// 兼容最小版本号 格式1.0.0.0-65535.65535.65535.65535
        /// </summary>
        /// <returns></returns>
        string ComMinVersion();

        /// <summary>
        /// 兼容 最大版本号格式1.0-65535.65535.65535.65535
        /// </summary>
        /// <returns></returns>
        string ComMaxVersion();
    }
}

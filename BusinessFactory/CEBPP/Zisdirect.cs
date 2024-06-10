using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    /// <summary>
    /// 无其他用处，只是在为了判断此DLL是否是提供业务的DLL，请注意命名空间和类名都不用改变
    /// </summary>
    public class Zisdirect : IZisdirect
    {
        public Zisdirect()
        {
        }

        public string Note()
        {

            return "电子票据服务";// + string.Format(@" 兼容版本{0}-{1}", ComMinVersion(), ComMaxVersion()); ;

        }

        public Dictionary<string, Type> GetTypes()
        {
            Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

            if (Types != null && Types.Length > 0)
            {
                for (int i = 0; i < Types.Length; i++)
                {
                    try
                    {
                        if (Types[i].FullName != "BusinessInterface.Zisdirect")
                        {
                            if (Types[i].BaseType.FullName.StartsWith("BusinessInterface."))
                                dictionary.Add(Types[i].FullName, Types[i]);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 兼容最小版本号格式1.0.0.0-65535.65535.65535.65535
        /// </summary>
        /// <returns></returns>
        public string ComMinVersion()
        {
            return "1.0.0.0";
        }
        /// <summary>
        /// 兼容最大版本号 格式1.0.0.0-65535.65535.65535.65535
        /// </summary>
        /// <returns></returns>
        public string ComMaxVersion()
        {
            return "1.0.65535.65535";
        }
    }
}

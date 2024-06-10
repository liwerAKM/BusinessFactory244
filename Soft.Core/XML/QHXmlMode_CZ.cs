using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Soft.Core
{
    public class QHXmlMode_CZ
    {
        public QHXmlMode_CZ()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 获取XML基本结构 
        /// </summary>
        /// <param name="TYPE">消息类型</param>
        /// <param name="CZLX">操作类型 0 新增 1 修改 2 删除 3作废...</param>
        /// <returns></returns>
        public static XmlDocument GetBaseXml()
        {
            XmlDocument doc = XMLHelper.X_CreateXmlDocument("request");
            return doc;
        }
    }
}

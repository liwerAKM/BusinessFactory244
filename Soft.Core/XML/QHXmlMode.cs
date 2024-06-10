using System;
using System.Data;
using System.Xml;
/// <summary>
///QHXmlMode 的摘要说明
/// </summary>
public class QHXmlMode
{
	public QHXmlMode()
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
    public  static XmlDocument GetBaseXml(string TYPE, string CZLX)
    {
        XmlDocument doc = XMLHelper.X_CreateXmlDocument("ROOT");
        XMLHelper.X_XmlInsertNode(doc, "ROOT", "HEADER", "TYPE", TYPE);
        XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "CZLX", CZLX.ToString());
        XMLHelper.X_XmlInsertNode(doc, "ROOT", "BODY");
        return doc;
    }
    /// <summary>
    /// 生产返回xml中的BODY
    /// </summary>
    /// <param name="doc">xml</param>
    /// <param name="CLBZ">-处理标志位 参见附录表</param>
    /// <param name="CLJG"> 处理结果 反馈处理失败或者成功消息</param>
    /// <param name="SNoteS"></param>
    public  static void GetReturnXml(XmlDocument doc, string CLBZ, string CLJG, params SNote[] SNoteS)
    {
        try
        {
            XMLHelper.X_XmlNodeDelete(doc, "ROOT/BODY");
        }
        catch
        {
        }
        XMLHelper.X_XmlInsertNode(doc, "ROOT", "BODY");
        XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CLBZ", CLBZ);
        XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CLJG", CLJG);
        XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", SNoteS);
    }
    /// <summary>
    ///获取 消息类型
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public static string GetTYPE(XmlDocument doc)
    {
        string TYPE = "";
        try
        {
            DataSet ds = XMLHelper.X_GetXmlData(doc, "ROOT/HEADER");
            if (ds.Tables.Count == 1 && ds.Tables[0].Columns.Contains("TYPE") && ds.Tables[0].Rows.Count == 1)
            {
                TYPE = ds.Tables[0].Rows[0]["TYPE"].ToString().Trim().ToUpper();
            }
        }
        catch
        {
        }
        return TYPE;
        
    }
    /// <summary>
    ///获取 消息类型
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public static string GetUnionPayHosType(XmlDocument doc)
    {
        string TYPE = "";
        try
        {
            DataSet ds = XMLHelper.X_GetXmlData(doc, "request");
            if (ds.Tables.Count == 1 && ds.Tables[0].Columns.Contains("TYPE") && ds.Tables[0].Rows.Count == 1)
            {
                TYPE = ds.Tables[0].Rows[0]["TYPE"].ToString().Trim().ToUpper();
            }
        }
        catch
        {
        }
        return TYPE;

    }
    /// <summary>
    ///获取 操作类型
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public static string GetCZLX(XmlDocument doc)
    {
        string CZLX = "";
        try
        {
            DataSet ds = XMLHelper.X_GetXmlData(doc, "ROOT/HEADER");
            if (ds.Tables.Count == 1 && ds.Tables[0].Columns.Contains("CZLX") && ds.Tables[0].Rows.Count == 1)
            {
                CZLX = ds.Tables[0].Rows[0]["CZLX"].ToString().Trim().ToUpper();
            }
        }
        catch
        {
        }
        return CZLX;
    }
}

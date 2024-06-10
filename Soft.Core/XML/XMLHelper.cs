using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
/**/
/// <summary>
/// XML 操作基类
/// </summary>
public class XMLHelper
{
    /**/
    /// <summary>
    /// 读取Xml到DataSet中
    /// </summary>
    /// <param name="XmlPath">路径</param>
    /// <returns>结果集</returns>
    public static DataSet GetXml(string XmlPath)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(@XmlPath);
        return ds;
    }

    /**/
    /// <summary>
    /// 读取xml文档并返回一个节点:适用于一级节点
    /// </summary>
    /// <param name="XmlPath">xml路径</param>
    /// <param name="NodeName">节点</param>
    /// <returns></returns>
    public static string ReadXmlReturnNode(string XmlPath, string Node)
    {
        XmlDocument docXml = new XmlDocument();
        docXml.Load(@XmlPath);
        XmlNodeList xn = docXml.GetElementsByTagName(Node);
        return xn.Item(0).InnerText.ToString();
    }

    /**/
    /// <summary>
    /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
    /// </summary>
    /// <param name="xmlPath">xml文档路径</param>
    /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
    /// <returns></returns>
    public static DataSet GetXmlData(string xmlPath, string XmlPathNode)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        DataSet ds = new DataSet();
        StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
        ds.ReadXml(read);
        return ds;
    }

    /**/
    /// <summary>
    /// 更新Xml节点内容
    /// </summary>
    /// <param name="xmlPath">xml路径</param>
    /// <param name="Node">要更换内容的节点:节点路径 根节点/父节点/当前节点</param>
    /// <param name="Content">新的内容</param>
    public static void XmlNodeReplace(string xmlPath, string Node, string Content)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        objXmlDoc.SelectSingleNode(Node).InnerText = Content;
        objXmlDoc.Save(xmlPath);

    }

    /**/
    /// <summary>
    /// 更改节点的属性值
    /// </summary>
    /// <param name="xmlPath">文件路径</param>
    /// <param name="NodePath">节点路径</param>
    /// <param name="NodeAttribute1">要更改的节点属性的名称</param>
    /// <param name="NodeAttributeText">更改的属性值</param>
    public static void XmlAttributeEdit(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
        XmlElement xe = (XmlElement)nodePath;
        xe.SetAttribute(NodeAttribute1, NodeAttributeText);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 删除XML节点和此节点下的子节点
    /// </summary>
    /// <param name="xmlPath">xml文档路径</param>
    /// <param name="Node">节点路径</param>
    public static void XmlNodeDelete(string xmlPath, string Node)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
        objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 删除一个节点的属性
    /// </summary>
    /// <param name="xmlPath">文件路径</param>
    /// <param name="NodePath">节点路径（xpath）</param>
    /// <param name="NodeAttribute">属性名称</param>
    public static void xmlnNodeAttributeDel(string xmlPath, string NodePath, string NodeAttribute)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
        XmlElement xe = (XmlElement)nodePath;
        xe.RemoveAttribute(NodeAttribute);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 插入一个节点和此节点的子节点
    /// </summary>
    /// <param name="xmlPath">xml路径</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="Element">插入节点的子节点</param>
    /// <param name="Content">子节点的内容</param>
    public static void XmlInsertNode(string xmlPath, string MailNode, string ChildNode, string Element, string Content)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
        objRootNode.AppendChild(objChildNode);
        XmlElement objElement = objXmlDoc.CreateElement(Element.ToUpper());
        objElement.InnerText = Content;
        objChildNode.AppendChild(objElement);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 向一个节点添加属性
    /// </summary>
    /// <param name="xmlPath">xml文件路径</param>
    /// <param name="NodePath">节点路径</param>
    /// <param name="NodeAttribute1">要添加的节点属性的名称</param>
    /// <param name="NodeAttributeText">要添加属性的值</param>
    public static void AddAttribute(string xmlPath, string NodePath, string NodeAttribute1, string NodeAttributeText)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlAttribute nodeAttribute = objXmlDoc.CreateAttribute(NodeAttribute1);
        XmlNode nodePath = objXmlDoc.SelectSingleNode(NodePath);
        nodePath.Attributes.Append(nodeAttribute);
        XmlElement xe = (XmlElement)nodePath;
        xe.SetAttribute(NodeAttribute1, NodeAttributeText);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 插入一节点,带一属性
    /// </summary>
    /// <param name="xmlPath">Xml文档路径</param>
    /// <param name="MainNode">当前节点路径</param>
    /// <param name="Element">新节点</param>
    /// <param name="Attrib">属性名称</param>
    /// <param name="AttribContent">属性值</param>
    /// <param name="Content">新节点值</param>
    public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Attrib, string AttribContent, string Content)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
        XmlElement objElement = objXmlDoc.CreateElement(Element);
        objElement.SetAttribute(Attrib, AttribContent);
        objElement.InnerText = Content;
        objNode.AppendChild(objElement);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 插入一节点,不带属性
    /// </summary>
    /// <param name="xmlPath">Xml文档路径</param>
    /// <param name="MainNode">当前节点路径</param>
    /// <param name="Element">新节点</param>
    /// <param name="Content">新节点值</param>
    public static void XmlInsertElement(string xmlPath, string MainNode, string Element, string Content)
    {
        XmlDocument objXmlDoc = new XmlDocument();
        objXmlDoc.Load(xmlPath);
        XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
        XmlElement objElement = objXmlDoc.CreateElement(Element);
        objElement.InnerText = Content;
        objNode.AppendChild(objElement);
        objXmlDoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 在根节点下添加父节点
    /// </summary>
    public static void AddParentNode(string xmlPath, string parentNode)
    {
        XmlDocument xdoc = new XmlDocument();
        xdoc.Load(xmlPath);
        // 创建一个新的menber节点并将它添加到根节点下
        XmlElement Node = xdoc.CreateElement(parentNode);
        xdoc.DocumentElement.PrependChild(Node);
        xdoc.Save(xmlPath);
    }

    /**/
    /// <summary>
    /// 根据节点属性读取子节点值(较省资源模式)
    /// </summary>
    /// <param name="XmlPath">xml路径</param>
    /// <param name="FatherElement">父节点值</param>
    /// <param name="AttributeName">属性名称</param>
    /// <param name="AttributeValue">属性值</param>
    /// <param name="ArrayLength">返回的数组长度</param>
    /// <returns></returns>
    public static System.Collections.ArrayList GetSubElementByAttribute(string XmlPath, string FatherElement, string AttributeName, string AttributeValue, int ArrayLength)
    {
        System.Collections.ArrayList al = new System.Collections.ArrayList();
        XmlDocument docXml = new XmlDocument();
        docXml.Load(@XmlPath);
        XmlNodeList xn;
        xn = docXml.DocumentElement.SelectNodes("//" + FatherElement + "[" + @AttributeName + "='" + AttributeValue + "']");
        XmlNodeList xx = xn.Item(0).ChildNodes;
        for (int i = 0; i < ArrayLength & i < xx.Count; i++)
        {

            al.Add(xx.Item(i).InnerText);
        }

        return al;
    }
    /// <summary>
    /// 创建XML文档
    /// </summary>
    /// <param name="name">根节点名称</param>
    /// <param name="type">根节点的一个属性值</param>
    /// <returns></returns>
    /// moss中调用方法：创建的文件如果要存到moss的文档库中,则：
    ///          XmlDocument doc = XmlOperate.CreateXmlDocument("project", "T");
    ///            在此可嵌入增加子节点方法,如AddTaskNode(taskObj, ref doc); ..
    ///          byte[] fileContent = Encoding.UTF8.GetBytes(doc.OuterXml);
    ///          folder.Files.Add("name.xml", fileContent, true);
    ///          web.Update();
    /// .net中调用方法：写入文件中,则：
    ///          document = XmlOperate.CreateXmlDocument("sex", "sexy");
    ///          document.Save("c:/bookstore.xml");         
    public static XmlDocument CreateXmlDocument(string name, string type)
    {
        XmlDocument doc = null;
        XmlElement rootEle = null;
        try
        {
            doc = new XmlDocument();
            doc.LoadXml("<" + name + "/>");
            rootEle = doc.DocumentElement;
            rootEle.SetAttribute("type", type);
        }
        catch (Exception er)
        {
            throw er;
        }
        return doc;
    }

    /// <summary>
    /// 在根节点下增加子元素
    /// </summary>
    /// <param name="document"></param>
    /// <param name="nodeName"></param>
    /// <param name="type"></param>
    /// 调用方法：
    ///      document = xmloper.CreateXmlDocument("animal", "carnivore");
    ///      XmlOperate.AddNewNode1(ref document, "carnivore", "high");
    public static void AddNewNode1(ref XmlDocument document, string nodeName, string type)
    {
        XmlElement taskEle = null;
        try
        {
            taskEle = document.CreateElement(nodeName);
            taskEle.SetAttribute("type", type);
            document.DocumentElement.AppendChild((XmlNode)taskEle);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 在元素下增加子元素
    /// </summary>
    /// <param name="element"></param>
    /// <param name="nodeName"></param>
    /// <param name="type"></param>
    /// 调用方法：
    ///       XmlDocument document = new XmlDocument();
    ///       先取到相应的元素,然后调用该方法在该元素下增加子元素
    ///       XmlElement root = (XmlElement)document.SelectSingleNode("//animal/third");
    ///       XmlOperate.AddNewNode2(ref root,"thaw","boost investor confidence");
    ///注意上面的"//animal/third"也可换成"workflow[@type='T' and @isSpecial='1']"这种形式用来获取带有相应属性的元素
    public static void AddNewNode2(ref XmlElement element, string nodeName, string type)
    {
        XmlElement taskEle = null;
        try
        {
            taskEle = element.OwnerDocument.CreateElement(nodeName);
            taskEle.SetAttribute("type", type);
            element.AppendChild((XmlNode)taskEle);
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// 获取类型为制定值的一组节点
    /// </summary>
    /// <param name="type">类型值。E.G.workflow[@type='T' and @isSpecial='1']</param>
    /// <returns></returns>
    /// 注意：返回的 XmlNodeList 类型是个类似于arraylist的类型,所以要得到它的值只能遍历
    public static XmlNodeList GetDesiredNode(string type)
    {
        XmlDocument document = new XmlDocument();
        return document.SelectNodes("type");
    }


    ///// <summary>
    ///// 抓取网页上的xml文档赋值给XmlDocument对象
    ///// </summary>
    ///// <param name="url">网页的url（网页的内容必须是xml格式的）</param>
    ///// <returns></returns>
    //public static XmlDocument GetXMLDocumentFromWebPage(string url)
    //{
    //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);

    //    myRequest.Method = "POST";
    //    myRequest.ContentType = "application/x-www-form-urlencoded";
    //    myRequest.ContentLength = 0;

    //    // Get response 
    //    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
    //    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.Default);
    //    string content = reader.ReadToEnd();

    //    XmlDocument document = new XmlDocument();
    //    document.LoadXml(content);
    //    return document;
    //}

    /// <summary>
    /// 获取服务器上指定文件的xml文件内容
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static string GetXMLFile(string location)
    {
        XmlDocument document = new XmlDocument();
        document.Load(@"G:\ttt.xml");
        return document.InnerXml;
    }

    //获取sharepoint中指定文件的内容
    //public string GetProjectWorkflowUrl(SPFile file)
    //{      
    //    byte[] content = file.OpenBinary();
    //    contentStr = Encoding.UTF8.GetString(content);
    //    return contentStr;       
    //}

    #region GetDataSetByXml

    /// <summary>
    /// 读取xml直接返回DataSet 
    /// </summary>
    /// <param name="strXmlPath">xml文件相对路径</param>
    /// <returns></returns>
    public static DataSet GetDataSetByXml(string strXmlPath)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            if (ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }
        catch (Exception ex)
        {
            //throw ex;
            return null;
        }
    }
    #endregion
    #region GetDataViewByXml
    /// <summary>
    /// 读取Xml返回一个经排序或筛选后的DataView 
    /// </summary>
    /// <param name="strXmlPath">xml文件相对路径</param>
    /// <param name="strWhere">筛选条件,如："name = ＇kgdiwss＇"</param>
    /// <param name="strSort">排序条件,如："Id desc"</param>
    /// <returns></returns>        
    public static DataView GetDataViewByXml(string strXmlPath, string strWhere, string strSort)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            DataView dv = new DataView(ds.Tables[0]);
            if (strSort != null)
            {
                dv.Sort = strSort;
            }
            if (strWhere != null)
            {
                dv.RowFilter = strWhere;
            }
            return dv;
        }
        catch (Exception)
        {
            return null;
        }
    }
    #endregion

    #region WriteXmlByDataSet  向Xml文件插入一行数据
    /// <summary>
    /// 向Xml文件插入一行数据 
    /// </summary>
    /// <param name="strXmlPath"> xml文件相对路径</param>
    /// <param name="Columns">要插入行的列名数组,如：string[] Columns = {"name","IsMarried"}; </param>
    /// <param name="ColumnValue">要插入行每列的值数组,如：string[] ColumnValue={"明天去要饭","false"}; </param>
    /// <returns>成功返回true,否则返回false </returns>
    public static bool WriteXmlByDataSet(string strXmlPath, string[] Columns, string[] ColumnValue)
    {
        try
        {
            //根据传入的XML路径得到.XSD的路径,两个文件放在同一个目录下 
            string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
            DataSet ds = new DataSet();
            //读xml架构,关系到列的数据类型 
            ds.ReadXmlSchema(GetXmlFullPath(strXsdPath));
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            DataTable dt = ds.Tables[0];
            //在原来的表格基础上创建新行 
            DataRow newRow = dt.NewRow();
            //循环给一行中的各个列赋值 
            for (int i = 0; i < Columns.Length; i++)
            {
                newRow[Columns[i]] = ColumnValue[i];
            }
            dt.Rows.Add(newRow);
            dt.AcceptChanges();
            ds.AcceptChanges();
            ds.WriteXml(GetXmlFullPath(strXmlPath));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region UpdateXmlRow 更新符合条件的一条Xml记录
    /// <summary>
    /// 更新符合条件的一条Xml记录 
    /// </summary>
    /// <param name="strXmlPath">XML文件路径 </param>
    /// <param name="Columns">列名数组 </param>
    /// <param name="ColumnValue">列值数组 </param>
    /// <param name="strWhereColumnName"> 条件列名 </param>
    /// <param name="strWhereColumnValue">条件列值 </param>
    /// <returns></returns>
    public static bool UpdateXmlRow(string strXmlPath, string[] Columns, string[] ColumnValue, string strWhereColumnName, string strWhereColumnValue)
    {
        try
        {
            string strXsdPath = strXmlPath.Substring(0, strXmlPath.IndexOf(".")) + ".xsd";
            DataSet ds = new DataSet();
            //读xml架构,关系到列的数据类型 
            ds.ReadXmlSchema(GetXmlFullPath(strXsdPath));
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            //先判断行数 
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //如果当前记录为符合Where条件的记录 
                    if (ds.Tables[0].Rows[i][strWhereColumnName].ToString().Trim().Equals(strWhereColumnValue))
                    {
                        //循环给找到行的各列赋新值 
                        for (int j = 0; j < Columns.Length; j++)
                        {
                            ds.Tables[0].Rows[i][Columns[j]] = ColumnValue[j];
                        }
                        //更新DataSet 
                        ds.AcceptChanges();
                        //重新写入XML文件 
                        ds.WriteXml(GetXmlFullPath(strXmlPath));
                        return true;
                    }
                }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region DeleteXmlRowByIndex  通过删除DataSet中iDeleteRow这一行,然后重写Xml以实现删除指定行
    /// <summary>
    /// 通过删除DataSet中iDeleteRow这一行,然后重写Xml以实现删除指定行 
    /// </summary>
    /// <param name="strXmlPath"> xml相对路径</param>
    /// <param name="iDeleteRow">要删除的行在DataSet中的Index值 </param>
    /// <returns></returns>
    public static bool DeleteXmlRowByIndex(string strXmlPath, int iDeleteRow)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //删除符号条件的行 
                ds.Tables[0].Rows[iDeleteRow].Delete();
            }
            ds.WriteXml(GetXmlFullPath(strXmlPath));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region DeleteXmlRows 删除strColumn列中值为ColumnValue的行
    /// <summary>
    /// 删除strColumn列中值为ColumnValue的行 
    /// </summary>
    /// <param name="strXmlPath"> xml相对路径 </param>
    /// <param name="strColumn">列名</param>
    /// <param name="ColumnValue">strColumn列中值为ColumnValue的行均会被删除 </param>
    /// <returns></returns>
    public static bool DeleteXmlRows(string strXmlPath, string strColumn, string[] ColumnValue)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            //先判断行数 
            if (ds.Tables[0].Rows.Count > 0)
            {
                //判断行多还是删除的值多,多的for循环放在里面 
                if (ColumnValue.Length > ds.Tables[0].Rows.Count)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < ColumnValue.Length; j++)
                        {
                            if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                            {
                                ds.Tables[0].Rows[i].Delete();
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < ColumnValue.Length; j++)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i][strColumn].ToString().Trim().Equals(ColumnValue[j]))
                            {
                                ds.Tables[0].Rows[i].Delete();
                            }
                        }
                    }
                }
                ds.WriteXml(GetXmlFullPath(strXmlPath));
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region DeleteXmlAllRows  删除所有行
    /// <summary>
    /// 删除所有行 
    /// </summary>
    /// <param name="strXmlPath"> XML路径 </param>
    /// <returns></returns>
    public static bool DeleteXmlAllRows(string strXmlPath)
    {
        try
        {
            DataSet ds = new DataSet();
            ds.ReadXml(GetXmlFullPath(strXmlPath));
            //如果记录条数大于0 
            if (ds.Tables[0].Rows.Count > 0)
            {
                //移除所有记录 
                ds.Tables[0].Rows.Clear();
            }
            //重新写入,这时XML文件中就只剩根节点了 
            ds.WriteXml(GetXmlFullPath(strXmlPath));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion

    #region GetXmlFullPath 返回完整路径
    /// <summary>
    /// 返回完整路径 
    /// </summary>
    /// <param name="strPath"> Xml的路径 </param>
    /// <returns>返回完整路径 </returns>
    public static string GetXmlFullPath(string strPath)
    {
        //如果路径中含有:符号,则认定为传入的是完整路径 
        if (strPath.IndexOf(":") > 0)
        {
            return strPath;
        }
        else
        {
            //返回完整路径 
            //return System.Web.HttpContext.Current.Server.MapPath(strPath);
            return Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, strPath);
        }
    }
    #endregion

    /// <summary>
    /// 基于Xpath获取指定xml节点
    /// </summary>
    /// <param name="document"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNode(XmlDocument document, string xpath)
    {
        return document.SelectSingleNode(xpath);
    }

    /// <summary>
    /// 基于Xpath获取一组XMl节点
    /// </summary>
    /// <param name="document"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static XmlNodeList GetXmlNodeList(XmlDocument document, string xpath)
    {
        return document.SelectNodes(xpath);
    }

    /// <summary>
    /// 基于Xpath获取指定xml节点
    /// </summary>
    /// <param name="document"></param>
    /// <param name="xpath"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNode(string document, string xpath)
    {
        XmlDocument xd = new XmlDocument();
        {
            xd.LoadXml(document);//加载xml
            return xd.SelectSingleNode(xpath);
        }
    }
    public static XmlDocument X_GetXmlDocument(string xml)
    {
        XmlDocument doc = null;
        try
        {
            doc = new XmlDocument();
            doc.LoadXml(xml);
        }
        catch (Exception er)
        {
            throw er;
        }
        return doc;
    }
    /// <summary>
    /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
    /// </summary>
    /// <param name="xml">xmlDOC</param>
    /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
    /// <returns></returns>
    public static DataSet X_GetXmlData(XmlDocument objXmlDoc, string XmlPathNode)
    {
        using (DataSet ds = new DataSet())
        {

            string OUT = objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml;
            StringReader read = new StringReader(OUT);
            ds.ReadXml(read);
            return ds;
        }
    }
    /// <summary>
    /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
    /// </summary>
    /// <param name="xml">xmlDOC</param>
    /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
    /// <returns></returns>
    public static DataTable X_GetXmlDataTable(XmlDocument objXmlDoc, string XmlPathNode)
    {
        // using (DataSet ds = new DataSet())
        {
            DataTable dt = new DataTable();
            XmlNodeList nodelist = objXmlDoc.SelectNodes(XmlPathNode);
            foreach (XmlNode node in nodelist)
            {
                DataSet ds = new DataSet();
                StringReader read = new StringReader(node.OuterXml);
                ds.ReadXml(read);
                if (dt.Rows.Count == 0)
                {
                    dt = ds.Tables[0].Copy();
                }
                else
                {
                    DataTable dtresult = ds.Tables[0];
                    foreach (DataRow dr in dtresult.Rows)
                    {
                        dt.ImportRow(dr);
                    }
                }
            }
            return dt;
        }
    }
    /// <summary>
    /// 查找数据,返回当前节点的所有下级节点,填充到一个DataSet中
    /// </summary>
    /// <param name="xml">xml文档</param>
    /// <param name="XmlPathNode">节点的路径:根节点/父节点/当前节点</param>
    /// <returns></returns>
    public static DataSet X_GetXmlData(string Xml, string XmlPathNode)
    {
        return X_GetXmlData(X_GetXmlDocument(Xml), XmlPathNode);
    }
    /// <summary>
    /// 创建一个指定根节点的XML
    /// </summary>
    /// <param name="name">根节点名称</param>
    /// <returns></returns>
    public static XmlDocument X_CreateXmlDocument(string name)
    {
        XmlDocument doc = null;
        XmlElement rootEle = null;
        try
        {
            doc = new XmlDocument();
            doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?> " + @"
                    <" + name + "/>");
            rootEle = doc.DocumentElement;
        }
        catch (Exception er)
        {
            throw er;
        }
        return doc;
    }

    /**/
    /// <summary>
    /// 删除XML节点和此节点下的子节点
    /// </summary>
    /// <param name="xmlPath">xml文档路径</param>
    /// <param name="Node">节点路径</param>
    public static void X_XmlNodeDelete(XmlDocument objXmlDoc, string Node)
    {
        string mainNode = Node.Substring(0, Node.LastIndexOf("/"));
        objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(Node));
    }

    /// <summary>
    /// 插入一个节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    public static void X_XmlInsertNode(XmlDocument objXmlDoc, string MailNode, string ChildNode)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);

        objRootNode.AppendChild(objChildNode);

    }

    /// <summary>
    /// 插入一个节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="Content">子节点的内容</param>
    public static void X_XmlInsertNode(XmlDocument objXmlDoc, string MailNode, string ChildNode, string Content)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
        objChildNode.InnerText = Content;
        objRootNode.AppendChild(objChildNode);

    }

    /// <summary>
    /// 插入一个节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="Content">子节点的内容</param>
    public static void X_XmlInsertNode_NOCHANGE(XmlDocument objXmlDoc, string MailNode, string ChildNode, string Content)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
        objChildNode.InnerText = Content;
        objRootNode.AppendChild(objChildNode);

    }
    public static void X_XmlInsertNode(XmlDocument objXmlDoc, string MailNode, params SNote[] SNoteS)
    {
        if (SNoteS != null && SNoteS.Length > 0)
        {
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            foreach (SNote sat in SNoteS)
            {
                XmlElement objChildNode = objXmlDoc.CreateElement(sat.Name.ToUpper());
                objChildNode.InnerText = sat.Value;
                objRootNode.AppendChild(objChildNode);
            }
        }
    }

    public static void X_XmlInsertRow(XmlDocument objXmlDoc, string MailNode, DataRow dr)
    {
        if (dr != null && dr.Table.Columns.Count > 0)
        {
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            foreach (DataColumn dc in dr.Table.Columns)
            {
                XmlElement objChildNode = objXmlDoc.CreateElement(dc.ColumnName.ToUpper());
                if (dr[dc.ColumnName] != DBNull.Value)
                {
                    objChildNode.InnerText = dr[dc.ColumnName].ToString();
                }
                objRootNode.AppendChild(objChildNode);
            }
        }
    }

    /// <summary>
    /// 添加一条默认数据  add by wl 2015.09.17
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="dr"></param>
    public static void X_XmlInsertDefaultRow(XmlDocument objXmlDoc, string MailNode, DataTable dt)
    {
        if (dt != null && dt.Columns.Count > 0)
        {
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            foreach (DataColumn dc in dt.Columns)
            {
                XmlElement objChildNode = objXmlDoc.CreateElement(dc.ColumnName.ToUpper());

                objChildNode.InnerText = null;
                objRootNode.AppendChild(objChildNode);
            }
        }
    }

    /// <summary>
    /// 插入一个节点和此节点的子节点
    /// </summary>
    /// <param name="xmlPath">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="Element">插入节点的子节点</param>
    /// <param name="Content">子节点的内容</param>
    public static void X_XmlInsertNode(XmlDocument objXmlDoc, string MailNode, string ChildNode, string Element, string Content)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
        objRootNode.AppendChild(objChildNode);
        XmlElement objElement = objXmlDoc.CreateElement(Element.ToUpper());
        objElement.InnerText = Content;
        objChildNode.AppendChild(objElement);

    }

    /// <summary>
    /// 插入一个节点和此节点的子节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="SNoteS">插入节点的子节点集合</param>
    public static void X_XmlInsertNode(XmlDocument objXmlDoc, string MailNode, string ChildNode, params SNote[] SNoteS)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
        objRootNode.AppendChild(objChildNode);
        if (SNoteS != null && SNoteS.Length > 0)
        {

            foreach (SNote sat in SNoteS)
            {
                XmlElement objElement = objXmlDoc.CreateElement(sat.Name.ToUpper());
                objElement.InnerText = sat.Value;
                objChildNode.AppendChild(objElement);
            }
        }
    }
    /// <summary>
    /// 插入一个节点和此节点的子节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="dr">插入节点的子节点集合</param>
    public static void X_XmlInsertRow_NOCHANGE(XmlDocument objXmlDoc, string MailNode, string ChildNode, DataRow dr)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
        objRootNode.AppendChild(objChildNode);
        if (dr != null && dr.Table.Columns.Count > 0)
        {

            foreach (DataColumn dc in dr.Table.Columns)
            {
                XmlElement objElement = objXmlDoc.CreateElement(dc.ColumnName);
                if (dr[dc.ColumnName] != DBNull.Value)
                {
                    objElement.InnerText = dr[dc.ColumnName].ToString();
                }
                objChildNode.AppendChild(objElement);
            }
        }
    }
    /// <summary>
    /// 插入一个节点和此节点的子节点
    /// </summary>
    /// <param name="objXmlDoc">xml</param>
    /// <param name="MailNode">当前节点路径</param>
    /// <param name="ChildNode">新插入节点</param>
    /// <param name="dr">插入节点的子节点集合</param>
    public static void X_XmlInsertRow(XmlDocument objXmlDoc, string MailNode, string ChildNode, DataRow dr)
    {

        XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
        XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
        objRootNode.AppendChild(objChildNode);
        if (dr != null && dr.Table.Columns.Count > 0)
        {

            foreach (DataColumn dc in dr.Table.Columns)
            {
                XmlElement objElement = objXmlDoc.CreateElement(dc.ColumnName.ToUpper());
                if (dr[dc.ColumnName] != DBNull.Value)
                {
                    objElement.InnerText = dr[dc.ColumnName].ToString();
                }
                objChildNode.AppendChild(objElement);
            }
        }
    }

    public static void X_XmlInsertTable(XmlDocument objXmlDoc, string MailNode, DataTable dataTable, string RowName)
    {
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                X_XmlInsertRow(objXmlDoc, MailNode, RowName, dr);
            }
        }
    }

    public static void X_XmlInsertTable_NOCHANGE(XmlDocument objXmlDoc, string MailNode, DataTable dataTable, string RowName)
    {
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                X_XmlInsertRow_NOCHANGE(objXmlDoc, MailNode, RowName, dr);
            }
        }
    }


    public static void X_XmlInsertTable(XmlDocument objXmlDoc, string MailNode, DataTable dataTable, string ListName, string RowName)
    {
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ListName.ToUpper());
            objRootNode.AppendChild(objChildNode);
            foreach (DataRow dr in dataTable.Rows)
            {
                X_XmlInsertRow(objXmlDoc, MailNode + "/" + ListName.ToUpper(), RowName, dr);
            }
        }
    }
}




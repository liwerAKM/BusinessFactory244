using CommonModel;
using DB.Core;
using MySql.Data.MySqlClient;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos44_Common.BUS
{
    class GETGOODSLIST
    {
        public static string B_GETGOODSLIST(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETGOODSLIST.GETGOODSLIST_IN _in = JSONSerializer.Deserialize<Model.GETGOODSLIST.GETGOODSLIST_IN>(json_in);
                Model.GETGOODSLIST.GETGOODSLIST_OUT _out = new Model.GETGOODSLIST.GETGOODSLIST_OUT();
                XmlDocument doc = new XmlDocument();
                doc = QHXmlMode.GetBaseXml("GETGOODSMX", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CATE_CODE", string.IsNullOrEmpty(_in.CATE_CODE) ? "" : _in.CATE_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PINYINCODE", "");//string.IsNullOrEmpty(_in.PINYINCODE) ? "" : _in.PINYINCODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYTYPE", string.IsNullOrEmpty(_in.CATE_CODE) ? "" : _in.CATE_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERY_CODE", string.IsNullOrEmpty(_in.PINYINCODE) ? "" : _in.PINYINCODE.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = his_rtnxml;

                XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                DataTable dtGOODSLIST = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/ITEMLIST").Tables[0];
                dtGOODSLIST = CommonFunction.GetNewTable(dtGOODSLIST, "ITEM_CLASS = " + _in.CATE_CODE);

                _out.ITEMLIST = new List<Model.GETGOODSLIST.ITEM>();
                foreach (DataRow dr in dtGOODSLIST.Rows)
                {
                    Model.GETGOODSLIST.ITEM item = new Model.GETGOODSLIST.ITEM();
                    item.ITEM_CODE = dr["ITEM_CODE"].ToString();
                    item.ITEM_NAME= dr["ITEM_NAME"].ToString();
                    item.ITEM_PRICE = dr["ITEM_PRICE"].ToString();
                    item.ITEM_UNIT = dr["ITEM_UNIT"].ToString();
                    item.YL_PREC = dr["YL_PREC"].ToString();
                    item.ITEM_GG= dr["ITEM_GG"].ToString();
                    item.EXCEPT= dr["EXCEPT"].ToString();
                    item.APPR_NUM= dr["APPR_NUM"].ToString();
                    _out.ITEMLIST.Add(item);
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:" + ex.ToString();
                goto EndPoint;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}

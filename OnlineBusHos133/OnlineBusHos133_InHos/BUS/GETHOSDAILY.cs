﻿using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OnlineBusHos133_InHos.BUS
{
    class GETHOSDAILY
    {
        public static string B_GETHOSDAILY(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out;
            try
            {
                Model.GETHOSDAILY_M.GETHOSDAILY_IN _in = JSONSerializer.Deserialize<Model.GETHOSDAILY_M.GETHOSDAILY_IN>(json_in);
                Model.GETHOSDAILY_M.GETHOSDAILY_OUT _out = new Model.GETHOSDAILY_M.GETHOSDAILY_OUT();

                XmlDocument doc = QHXmlMode.GetBaseXml("GETHOSDAILY", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "USERNAME", "QHZZJ");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_NO", string.IsNullOrEmpty(_in.HOS_NO) ? "" : _in.HOS_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_PAT_ID", string.IsNullOrEmpty(_in.HOS_PAT_ID) ? "" : _in.HOS_PAT_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BEGIN_DATE", string.IsNullOrEmpty(_in.BEGIN_DATE) ? "" : _in.BEGIN_DATE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "END_DATE", string.IsNullOrEmpty(_in.END_DATE) ? "" : _in.END_DATE.Trim());

                string inxml = doc.InnerXml;
                //StreamReader stream = new StreamReader("D:/test1.txt");
                //string his_rtnxml = stream.ReadToEnd();
                string his_rtnxml = "";
                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = "";
                try
                {
                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    _out.HOSPATID = dtrev.Columns.Contains("HOS_PAT_ID") ? dtrev.Rows[0]["HOS_PAT_ID"].ToString() : _in.HOS_PAT_ID;
                    _out.JE_PAY = dtrev.Rows[0]["JE_PAY"].ToString();
                    _out.JE_YET = dtrev.Rows[0]["JE_YET"].ToString();
                    _out.JE_REMAIN = dtrev.Rows[0]["JE_REMAIN"].ToString();
                    _out.JE_TODAY = dtrev.Rows[0]["JE_TODAY"].ToString();

                    _out.BIGITEMLIST = new List<Model.GETHOSDAILY_M.BIGITEM>();

                    if (!ds.Equals(null) && ds.Tables.Count > 0)
                    {
                        //DataTable dtOfBigItem = ds.Tables[1];
                        DataTable dtOfItem = ds.Tables["BIGITEM"];

                        foreach (DataRow dr in dtOfItem.Rows)
                        {
                            Model.GETHOSDAILY_M.BIGITEM newBigItem = new Model.GETHOSDAILY_M.BIGITEM();
                            newBigItem.ITEM_NAME = dr["ITEM_NAME"].ToString();
                            newBigItem.JE_ALL = dr["JE_ALL"].ToString();
                            newBigItem.ITEMLIST = new List<Model.GETHOSDAILY_M.ITEM>();
                            //newBigItem.GITEMLIST = new List<Model.GETHOSDAILY_M.GITEM>();


                            Model.GETHOSDAILY_M.ITEM newItem = new Model.GETHOSDAILY_M.ITEM();
                            newItem.NAME = dr["ITEM_NAME"].ToString();
                            newItem.GG = "";
                            newItem.AMOUNT = dr["JE_ALL"].ToString();
                            newItem.CAMT = "1";
                            newItem.JE = dr["JE_ALL"].ToString();
                            newItem.JE_ALL = dr["JE_ALL"].ToString();
                            newItem.DJ_DATE = "";

                            newBigItem.ITEMLIST.Add(newItem);
                            _out.BIGITEMLIST.Add(newBigItem);

                            //#region 汇总合并 add at 20221117
                            //newBigItem.GITEMLIST = (from items in newBigItem.ITEMLIST
                            //                        group items by new { items.NAME, items.CAMT }
                            //                        into newItems
                            //                        select new Model.GETHOSDAILY_M.GITEM
                            //                        {
                            //                            BIG_NAME = newBigItem.ITEM_NAME,
                            //                            NAME = newItems.Key.NAME,
                            //                            CAMT = newItems.Key.CAMT,
                            //                            GG = FormatHelper.GetStr(newItems.FirstOrDefault().GG),
                            //                            JE = FormatHelper.GetStr(newItems.FirstOrDefault().JE),
                            //                            AMOUNT = newItems.Sum(itm => Convert.ToDecimal(itm.AMOUNT)).ToString(),
                            //                            JE_ALL = newItems.Sum(itm => Convert.ToDecimal(itm.JE_ALL)).ToString()
                            //                        }).ToList();
                            //#endregion

                            ////newBigItem.GITEMLIST = (List<Model.GETHOSDAILY_M.GITEM>)newList;
                            //

                        }
                    }
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确：" + ex.Message;
                    goto EndPoint;
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);

            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常" + ex.Message;
            }

        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }

    }
}

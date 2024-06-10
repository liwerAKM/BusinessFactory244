using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos324_InHos.BUS
{
    class GETPATINFBYHOSNO
    {
        public static string B_GETPATINFBYHOSNO(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETPATINFBYHOSNO_M.GETPATINFBYHOSNO_IN _in = JSONSerializer.Deserialize<Model.GETPATINFBYHOSNO_M.GETPATINFBYHOSNO_IN>(json_in);
                Model.GETPATINFBYHOSNO_M.GETPATINFBYHOSNO_OUT _out = new Model.GETPATINFBYHOSNO_M.GETPATINFBYHOSNO_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFBYHOSNO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_NO", string.IsNullOrEmpty(_in.HOS_NO) ? "" : _in.HOS_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                _out.HIS_RTNXML = his_rtnxml;
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

                    //string SFZ_NO = dtrev.Rows[0]["SFZ_NO"].ToString();
                    //string tempStr = SFZ_NO.Substring(SFZ_NO.Length - 6);//取后六位

                    //if (!_in.SFZ_NO.Equals(tempStr))
                    //{
                    //    dataReturn.Code = 1;
                    //    dataReturn.Msg = "身份证校验不通过，请核对";
                    //    goto EndPoint;
                    //}


                    _out.HOS_NO = dtrev.Columns.Contains("HOS_NO")? dtrev.Rows[0]["HOS_NO"].ToString():_in.HOS_NO;
                    _out.HOSPATID = dtrev.Columns.Contains("HOS_PAT_ID") ? dtrev.Rows[0]["HOS_PAT_ID"].ToString():"";
                    _out.PAT_NAME = dtrev.Columns.Contains("PAT_NAME")?dtrev.Rows[0]["PAT_NAME"].ToString():"";
                    _out.SEX = dtrev.Columns.Contains("SEX") ?dtrev.Rows[0]["SEX"].ToString():"";
                    _out.HIN_TIME = dtrev.Columns.Contains("HIN_TIME") ? dtrev.Rows[0]["HIN_TIME"].ToString() : "";
                    _out.BED_NO = dtrev.Columns.Contains("BED_NO") ? dtrev.Rows[0]["BED_NO"].ToString() : "";
                    _out.SFZ_NO = dtrev.Columns.Contains("SFZ_NO") ? dtrev.Rows[0]["SFZ_NO"].ToString() : _in.SFZ_NO;

                }
                catch
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
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

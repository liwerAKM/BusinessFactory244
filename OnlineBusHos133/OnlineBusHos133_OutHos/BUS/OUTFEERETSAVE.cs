using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos133_OutHos.BUS
{
    class OUTFEERETSAVE
    {
        public static string B_OUTFEERETSAVE(string json_in)
        {
            return Business(json_in);

        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.OUTFEERETSAVE_M.OUTFEERETSAVE_IN _in = JSONSerializer.Deserialize<Model.OUTFEERETSAVE_M.OUTFEERETSAVE_IN>(json_in);
                Model.OUTFEERETSAVE_M.OUTFEERETSAVE_OUT _out = new Model.OUTFEERETSAVE_M.OUTFEERETSAVE_OUT();

                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEERETSAVE_YB", "0");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "invoiceNo", FormatHelper.GetStr(_in.InvoiceNo));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", FormatHelper.GetStr(_in.HOS_SN));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", FormatHelper.GetStr(_in.HOSPATID));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YY_LSH", FormatHelper.GetStr(_in.YY_LSH));
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", "qhzzj001");


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
                        _out.STATUS = "FAlL";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    _out.STATUS = "SUCCESS";


                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.Message;
                }


                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:" + ex.Message;
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

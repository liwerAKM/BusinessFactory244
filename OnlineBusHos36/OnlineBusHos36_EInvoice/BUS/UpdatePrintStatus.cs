using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos36_EInvoice.Class;
using CommonModel;
using ConstData;
using Soft.Core;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Data;

namespace OnlineBusHos36_EInvoice.BUS
{
    class UpdatePrintStatus
    {
        public static string B_UpdatePrintStatus(string json_in)
        {

            DataReturn dataReturn = new DataReturn();
            try
            {
                UpdatePrintStatus_IN _in = JSONSerializer.Deserialize<UpdatePrintStatus_IN>(json_in);

                string BIZCODE = _in.BIZ_CODE;
                if (string.IsNullOrEmpty(BIZCODE))
                {
                    dataReturn.Code = -1;
                    dataReturn.Msg = "未获取到有效流水号";
                    goto EndPoint;
                }

                string his_rtnxml = "";
                #region UPDATEPRINTSTATUS
                XmlDocument doc = QHXmlMode.GetBaseXml("UPDATEPRINTSTATUS", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "BIZCODE", BIZCODE);
                string inXml = doc.InnerXml;

                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inXml, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "调用院端接口失败：" + his_rtnxml;
                    goto EndPoint;
                }


                XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];
                if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                {
                    dataReturn.Code = -1;
                    dataReturn.Msg = "下载发票数据失败：" + dtrev.Rows[0]["CLJG"].ToString();
                    goto EndPoint;
                }

                #endregion
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.ToString();
            }
        EndPoint:
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
        public static string B_UpdatePrintStatus_b(string json_in)
        {
            UpdatePrintStatus_IN _in = JSONSerializer.Deserialize<UpdatePrintStatus_IN>(json_in);
            DataReturn dataReturn = new DataReturn();
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }

        public class BODY
        {
            public string CLBZ { get; set; }
            public string CLJG { get; set; }
        }
    }
}

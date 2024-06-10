using System;
using System.Collections.Generic;
using System.Text;
using Soft.Core;
using System.Xml;
using System.Data;
using CommonModel;

namespace OnlineBusHos244_YYGH.BUS
{
    class GETREGISTERLIST
    {
        public static string B_GETREGISTERLIST(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            Model.GETREGISTERLIST_M.GETREGISTERLIST_IN _in = JSONSerializer.Deserialize<Model.GETREGISTERLIST_M.GETREGISTERLIST_IN>(json_in);
            Model.GETREGISTERLIST_M.GETREGISTERLIST_OUT _out = new Model.GETREGISTERLIST_M.GETREGISTERLIST_OUT();

            XmlDocument doc = QHXmlMode.GetBaseXml("GETREGISTERLIST", "1");
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", String.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", String.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LTERMINAL_SN", String.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", String.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", String.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE); //String.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", String.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", String.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YY_TYPE", String.IsNullOrEmpty(_in.YY_TYPE) ? "" : _in.YY_TYPE);
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", "1");
            XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", "1000");

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
                DataTable APP = null;
                try
                {
                     APP = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/APPTLIST").Tables[0];
                }
                catch { }
                _out.APPTLIST = new List<Model.GETREGISTERLIST_M.AppTist>();
                if (APP.Equals(null) && APP.Rows.Count == 0)//无记录
                {
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    goto EndPoint;
                }

                DataView dv = APP.DefaultView;
                dv.RowFilter = @"APPT_TYPE = 1 OR APPT_TYPE = 0";
                APP = dv.ToTable();


              
                foreach (DataRow dr in APP.Rows)
                {
                    Model.GETREGISTERLIST_M.AppTist tist = new Model.GETREGISTERLIST_M.AppTist();
                    tist.SCH_DATE = dr["SCH_DATE"].ToString();
                    tist.SCH_TIME = dr["SCH_TIME"].ToString();
                    tist.SCH_TYPE = dr["SCH_TYPE"].ToString();
                    tist.PERIOD_START = dr["PERIOD_START"].ToString();
                    //tist.PERIOD_END = dr["PERIOD_END"].ToString();
                    tist.DEPT_CODE= dr["DEPT_CODE"].ToString();
                    tist.DEPT_NAME= dr["DEPT_NAME"].ToString();
                    tist.DOC_NO = dr["DOC_NO"].ToString();
                    tist.DOC_NAME = dr["DOC_NAME"].ToString();
                    //tist.REGISTER_TYPE = dr["REGISTER_TYPE"].ToString();
                    tist.HOS_SN = dr["HOS_SN"].ToString();
                    tist.APPT_PAY = dr["APPT_PAY"].ToString();
                    tist.APPT_TYPE = dr["APPT_TYPE"].ToString();
                    tist.APPT_ORDER = dr["APPT_ORDER"].ToString();
                    tist.OPT_SN = dr["OPT_SN"].ToString();
                    tist.PRO_TITLE = dr["PRO_TITLE"].ToString();
                    tist.USER_ID = dr["USER_ID"].ToString();
                    tist.YB_INFO = dr["YB_INFO"].ToString();
                    tist.JEALL = dr["JE_ALL"].ToString();
                    tist.APPT_TIME = dr["APPT_TIME"].ToString();

                    _out.APPTLIST.Add(tist);
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
            }
            catch (Exception ex)
            {
                dataReturn.Code = 1;
                dataReturn.Msg = ex.ToString();
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

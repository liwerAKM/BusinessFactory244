using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using DB.Core;

namespace OnlineBusHos133_YYGH.BUS
{
    class GETINSUREGPARA
    {
        public static string B_GETINSUREGPARA(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETINSUREGPARA_M.GETINSUREGPARA_IN _in = JSONSerializer.Deserialize<Model.GETINSUREGPARA_M.GETINSUREGPARA_IN>(json_in);
                Model.GETINSUREGPARA_M.GETINSUREGPARA_OUT _out = new Model.GETINSUREGPARA_M.GETINSUREGPARA_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETINSUREGPARA", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode_NOCHANGE(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PSN_NO", string.IsNullOrEmpty(_in.PSN_NO) ? "" : _in.PSN_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REGISTER_TYPE", string.IsNullOrEmpty(_in.REGISTER_TYPE) ? "" : _in.REGISTER_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.BARCODE) ? "" : _in.BARCODE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YY_LSH", string.IsNullOrEmpty(_in.YY_LSH) ? "" : _in.YY_LSH.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YB_CARD_TYPE", string.IsNullOrEmpty(_in.YB_CARD_TYPE) ? "" : _in.YB_CARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PARAMETERS", string.IsNullOrEmpty(_in.PARAMETERS) ? "" : _in.PARAMETERS.Trim());


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
                    _out.EXPSTRING = dtrev.Columns.Contains("ExpString") ? dtrev.Rows[0]["ExpString"].ToString() : "";
                    #region  平台数据保存
                    /*try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterPay modelregister_pay = new SqlSugarModel.RegisterPay();
                        SqlSugarModel.RegisterAppt modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        int pay_id = 0;//
                        if (!PubFunc.GetSysID("REGISTER_PAY", out pay_id))
                        {
                            goto EndPoint;
                        }

                        modelregister_pay.PAY_ID = pay_id;
                        modelregister_pay.REG_ID = modelregister_appt.REG_ID;
                        modelregister_pay.HOS_ID = _in.HOS_ID;
                        modelregister_pay.PAT_ID = modelregister_appt.PAT_ID;
                        modelregister_pay.CHARGE_TYPE = "";
                        modelregister_pay.QUERYID = _in.QUERYID;
                        modelregister_pay.DEAL_TYPE = _in.DEAL_TYPE;
                        modelregister_pay.SUM_JE = FormatHelper.GetDecimal(_in.JEALL);
                        modelregister_pay.CASH_JE = FormatHelper.GetDecimal(_in.CASH_JE);
                        modelregister_pay.ACCT_JE = 0;
                        modelregister_pay.FUND_JE = modelregister_pay.SUM_JE - modelregister_pay.CASH_JE;
                        modelregister_pay.OTHER_JE = 0;

                        modelregister_pay.HOS_SN = _out.HOS_SN;
                        modelregister_pay.OPT_SN = _out.OPT_SN;
                        modelregister_pay.PRE_NO = "";
                        modelregister_pay.RCPT_NO = _out.RCPT_NO;

                        modelregister_pay.DJ_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy.MM.dd"));
                        modelregister_pay.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                        modelregister_pay.USER_ID = _in.USER_ID;
                        modelregister_pay.SOURCE = "ZZJ";
                        modelregister_pay.lTERMINAL_SN = _in.LTERMINAL_SN;

                        modelregister_pay.IS_TH = false;

                        modelregister_appt.APPT_TYPE = "1";
                        var row1 = db.Updateable(modelregister_appt).ExecuteCommand();
                        var row = db.Insertable(modelregister_pay).ExecuteCommand();
                    }
                    catch (Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "GETINSUREGPARA";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }*/
                    #endregion
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.ToString();

                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.ToString();
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

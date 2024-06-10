using CommonModel;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos44_InHos.BUS
{
    class SAVEINPATYJJ
    {
        public static string B_SAVEINPATYJJ(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out;
            try
            {
                Model.SAVEINPATYJJ_M.SAVEINPATYJJ_IN _in = JSONSerializer.Deserialize<Model.SAVEINPATYJJ_M.SAVEINPATYJJ_IN>(json_in);
                Model.SAVEINPATYJJ_M.SAVEINPATYJJ_OUT _out = new Model.SAVEINPATYJJ_M.SAVEINPATYJJ_OUT();

                //if (string.IsNullOrEmpty(_in.SFZ_NO))
                //{
                //    dataReturn.Code = -1;
                //    dataReturn.Msg = "SFZ_NO不能为空";
                //    goto EndPoint;
                //}

                //校验数据库中患者信息
                var db = new DbMySQLZZJ().Client;
                //SqlSugarModel.PatInfo patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).Single();

                //if (patInfo == null)
                //{
                //    dataReturn.Code = 6;
                //    dataReturn.Msg = "病人还未建档";
                //    dataReturn.Param = JSONSerializer.Serialize(_out);
                //    goto EndPoint;
                //}
                //string PAT_NAME = patInfo.PAT_NAME;
                //int PAT_ID = patInfo.PAT_ID;

                XmlDocument doc = QHXmlMode.GetBaseXml("SAVEINPATYJJ", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/HEADER", "USERNAME", "QHZZJ");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_NO", string.IsNullOrEmpty(_in.HOS_NO) ? "" : _in.HOS_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_PAT_ID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERYID", string.IsNullOrEmpty(_in.QUERYID) ? "" : _in.QUERYID.Trim());

                

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

                    _out.HOSPATID = dtrev.Rows[0]["HOS_PAT_ID"].ToString();
                    _out.JE_PAY = dtrev.Rows[0]["JE_PAY"].ToString();
                    _out.CASH_JE = _in.CASH_JE;
                    _out.JE_REMAIN = dtrev.Rows[0]["JE_REMAIN"].ToString();
                    _out.HOS_PAY_SN = dtrev.Rows[0]["HOS_PAY_SN"].ToString();

                    #region 存库
                    /*
                    try
                    {
                        SqlSugarModel.PatPrePay modelpat_prepay = new SqlSugarModel.PatPrePay();

                        int PAY_ID = 0;//创建支付ID
                        if (!PubFunc.GetSysID("pat_prepay", out PAY_ID))
                        {
                            string s = DateTime.Now.Ticks.ToString();
                            PAY_ID = Convert.ToInt32(s.Substring(s.Length - 6));
                        }

                        modelpat_prepay.PAY_ID = PAY_ID;
                        modelpat_prepay.HOS_ID = _in.HOS_ID;
                        modelpat_prepay.HOS_PAT_ID = _out.HOSPATID;
                        modelpat_prepay.HOS_PAY_SN = _out.HOS_PAY_SN;
                        modelpat_prepay.PAT_ID = PAT_ID;
                        modelpat_prepay.USER_ID = _in.USER_ID;
                        modelpat_prepay.CASH_JE = Convert.ToDecimal(_out.CASH_JE);
                        modelpat_prepay.DJ_TIME = DateTime.Parse(_in.DEAL_TIME);
                        modelpat_prepay.lTERMINAL_SN = _in.LTERMINAL_SN;

                        var row = db.Insertable(modelpat_prepay).ExecuteCommand();
                    }
                    catch(Exception ex)
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "SAVEINPATYJJ";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                    */
                    #endregion
                  
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

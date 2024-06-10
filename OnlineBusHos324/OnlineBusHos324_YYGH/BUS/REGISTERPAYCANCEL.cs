using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
namespace OnlineBusHos324_YYGH.BUS
{
    class REGISTERPAYCANCEL
    {
        public static string B_REGISTERPAYCANCEL(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_IN _in = JSONSerializer.Deserialize<Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_IN>(json_in);
                Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_OUT _out = new Model.REGISTERPAYCANCEL_M.REGISTERPAYCANCEL_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("REGISTERPAYCANCEL", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SNAPPT", string.IsNullOrEmpty(_in.HOS_SNAPPT) ? "" : _in.HOS_SNAPPT.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "CASH_JE", string.IsNullOrEmpty(_in.CASH_JE) ? "" : _in.CASH_JE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TYPE", string.IsNullOrEmpty(_in.DEAL_TYPE) ? "" : _in.DEAL_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_STATES", string.IsNullOrEmpty(_in.DEAL_STATES) ? "" : _in.DEAL_STATES.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEAL_TIME", string.IsNullOrEmpty(_in.DEAL_TIME) ? "" : _in.DEAL_TIME.Trim());
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
                    #region  平台数据保存
                    try
                    {
                        var db = new DbMySQLZZJ().Client;

                        SqlSugarModel.RegisterPay modelregister_pay = db.Queryable<SqlSugarModel.RegisterPay>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        SqlSugarModel.RegisterAppt modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOS_SN == _in.HOS_SN).First();
                        if (modelregister_pay != null && modelregister_appt == null) 
                        {
                            modelregister_appt = db.Queryable<SqlSugarModel.RegisterAppt>().Where(t => t.REG_ID == modelregister_pay.REG_ID).First();
                        }
                        //退费
                        if (modelregister_pay != null) 
                        {
                            modelregister_pay.IS_TH = true;
                            modelregister_pay.TH_DATE = DateTime.Now.Date;
                            modelregister_pay.TH_TIME = DateTime.Now.ToString("HH:mm:ss");
                            modelregister_pay.TH_SOURCE = "ZZJ";
                            modelregister_pay.TH_USER_ID = _in.USER_ID;
                            modelregister_pay.TH_lTERMINAL_SN = _in.LTERMINAL_SN;
                            db.Updateable(modelregister_pay).ExecuteCommand();

                            modelregister_appt.APPT_TYPE = "3";
                            db.Updateable(modelregister_appt).ExecuteCommand();
                        }
                        else if (modelregister_appt != null)//取消预约挂号
                        {
                            modelregister_appt.APPT_TYPE = "5";
                            modelregister_appt.CANCEL_DATE= DateTime.Now.Date;
                            modelregister_appt.CANCEL_TIME= DateTime.Now.ToString("HH:mm:ss");
                            db.Updateable(modelregister_appt).ExecuteCommand();
                        }



                    }
                    catch(Exception ex) 
                    {
                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "REGISTERPAYCANCEL";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                    #endregion

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.Message;
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

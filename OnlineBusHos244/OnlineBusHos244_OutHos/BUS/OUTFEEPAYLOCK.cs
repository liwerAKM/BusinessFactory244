using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos244_OutHos.BUS
{
    class OUTFEEPAYLOCK
    {
        public static string B_OUTFEEPAYLOCK(string json_in)
        {
            return Business(json_in);

        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN _in = JSONSerializer.Deserialize<Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN>(json_in);
                Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT _out = new Model.OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT();
                string YLCARD_TYPE = PubFunc.GETHISYLCARDTYPE(_in.YLCARD_TYPE);

                DataTable dtpre = new DataTable();
                dtpre.Columns.Add("HOS_ID", typeof(string));
                dtpre.Columns.Add("lTERMINAL_SN", typeof(string));
                dtpre.Columns.Add("USER_ID", typeof(string));
                dtpre.Columns.Add("OPT_SN", typeof(string));
                dtpre.Columns.Add("PRE_NO", typeof(string));
                dtpre.Columns.Add("HOS_SN", typeof(string));
                dtpre.Columns.Add("SFZ_NO", typeof(string));
                dtpre.Columns.Add("MB_ID", typeof(string));
                
                DataTable dtMed = new DataTable();
                DataTable dtChkt = new DataTable();
                DataTable dtPre = new DataTable();
                dtPre.Columns.Add("HOS_ID", typeof(string));
                dtPre.Columns.Add("OPT_SN", typeof(string));
                dtPre.Columns.Add("PRE_NO", typeof(string));
                dtPre.Columns.Add("HOS_SN", typeof(string));
                dtPre.Columns.Add("JEALL", typeof(string));
                dtPre.Columns.Add("CASH_JE", typeof(string));
                dtPre.Columns.Add("DJ_DATE", typeof(string));
                dtPre.Columns.Add("DJ_TIME", typeof(string));
                foreach (Model.OUTFEEPAYLOCK_M.PRE pre in _in.PRELIST)
                {
                    DataRow drp = dtpre.NewRow();
                    drp["HOS_ID"] = _in.HOS_ID;
                    drp["lTERMINAL_SN"] = _in.LTERMINAL_SN;
                    drp["USER_ID"] = _in.USER_ID;
                    drp["OPT_SN"] = pre.OPT_SN;
                    drp["PRE_NO"] = pre.PRE_NO;
                    drp["HOS_SN"] = pre.HOS_SN;
                    drp["SFZ_NO"] = _in.SFZ_NO;
                    drp["MB_ID"] = FormatHelper.GetStr(pre.MB_ID);
                    dtpre.Rows.Add(drp);

                    DataRow dr = dtpre.Rows[0];
                    XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAYMX", "0");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", _in.HOS_ID);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", dr["OPT_SN"].ToString().Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", dr["PRE_NO"].ToString().Trim().Replace("#", "|"));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", dr["HOS_SN"].ToString().Trim().Replace("#", "|"));
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", dtpre.Columns.Contains("USER_ID") ? dr["USER_ID"].ToString().Trim() : "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LTERMINAL_SN", dr["lTERMINAL_SN"].ToString().Trim().Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim()); // string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", _in.YLCARD_NO); //dtpre.Columns.Contains("YLCARD_NO") ? dr["YLCARD_NO"].ToString().Trim() : "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", dtpre.Columns.Contains("SFZ_NO") ? dr["SFZ_NO"].ToString().Trim() : "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", _in.HOSPATID);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MB_ID", dtpre.Columns.Contains("MB_ID") ? dr["MB_ID"].ToString().Trim() : "");
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
                        DataTable dt_med = new DataTable();
                        try
                        {
                            dt_med = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DAMEDLIST").Tables[0];
                        }
                        catch
                        {

                        }
                        DataTable dt_chkt = new DataTable();
                        try
                        {
                            dt_chkt = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY/DACHKTLIST").Tables[0];
                        }
                        catch
                        {

                        }
                        if(dtMed.Rows.Count==0)
                        {
                            dtMed = dt_med.Clone();
                        }
                        foreach(DataRow drm in dt_med.Rows)
                        {
                            dtMed.Rows.Add(drm.ItemArray);
                        }
                        if (dtChkt.Rows.Count == 0)
                        {
                            dtChkt = dt_chkt.Clone();
                        }
                        foreach (DataRow drC in dt_chkt.Rows)
                        {
                            dtChkt.Rows.Add(drC.ItemArray);
                        }
                        DataRow drdtp = dtPre.NewRow();
                        drdtp["HOS_ID"] =_in.HOS_ID;
                        drdtp["OPT_SN"] = dr["OPT_SN"].ToString().Trim();
                        drdtp["PRE_NO"] = dr["PRE_NO"].ToString().Trim();
                        drdtp["HOS_SN"] = dr["HOS_SN"].ToString().Trim();
                        drdtp["JEALL"] = dtrev.Rows[0]["JEALL"].ToString().Trim();
                        drdtp["CASH_JE"] = dtrev.Rows[0]["JEALL"].ToString().Trim();
                        drdtp["DJ_DATE"] = DateTime.Now.ToString("yyyy-MM-dd");
                        drdtp["DJ_TIME"] = DateTime.Now.ToString("HH:mm:ss");
                        dtPre.Rows.Add(drdtp);
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                        dataReturn.Param = ex.Message;
                    }

                }
                string PAY_ID = "";
                #region 平台数据保存
                try
                {
                    var db = new DbMySQLZZJ().Client;

                    SqlSugarModel.OptPayLock Modelopt_pay_lock = new SqlSugarModel.OptPayLock();

                    int payid = 0;//
                    if (!PubFunc.GetSysID("opt_pay_lock", out payid))
                    {
                        goto EndPoint;
                    }
                    int pat_id = 0;
                    var pat_info = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.SFZ_NO == _in.SFZ_NO).First();
                    if (pat_info != null)
                    {
                        pat_id = pat_info.PAT_ID;
                    }

                    string PRE_NO = "";
                    decimal CASH_JE = 0;
                    decimal JE_ALL = 0;
                    string HOS_SN = "";
                    foreach (DataRow dr in dtPre.Rows)
                    {
                        PRE_NO += dr["PRE_NO"].ToString().Trim() + "|";
                        CASH_JE = decimal.Parse(dr["CASH_JE"].ToString().Trim());
                        JE_ALL = decimal.Parse(dr["JEALL"].ToString().Trim());
                        HOS_SN += dr["HOS_SN"].ToString().Trim() + "|";
                    }
                    PAY_ID = payid.ToString().PadLeft(10, '0');
                    Modelopt_pay_lock.PAY_ID = PAY_ID;
                    Modelopt_pay_lock.HOS_ID = _in.HOS_ID;
                    Modelopt_pay_lock.PAT_ID = pat_id;
                    Modelopt_pay_lock.PAT_NAME = _in.PAT_NAME;
                    Modelopt_pay_lock.SFZ_NO = _in.SFZ_NO;
                    Modelopt_pay_lock.YLCARD_TYPE =FormatHelper.GetInt(_in.YLCARD_TYPE);
                    Modelopt_pay_lock.YLCARD_NO = _in.YLCARD_NO;
                    Modelopt_pay_lock.HOSPATID = _in.HOSPATID;

                    Modelopt_pay_lock.DEPT_CODE = "";
                    Modelopt_pay_lock.DEPT_NAME = "";
                    Modelopt_pay_lock.DOC_NO = "";
                    Modelopt_pay_lock.DOC_NAME = "";
                    Modelopt_pay_lock.CHARGE_TYPE = "";
                    Modelopt_pay_lock.QUERYID = "";
                    Modelopt_pay_lock.DEAL_TYPE = "";
                    Modelopt_pay_lock.SUM_JE = JE_ALL;
                    Modelopt_pay_lock.CASH_JE = CASH_JE;
                    Modelopt_pay_lock.ACCT_JE = 0;
                    Modelopt_pay_lock.FUND_JE = 0;
                    Modelopt_pay_lock.OTHER_JE = 0;
                    Modelopt_pay_lock.HOS_REG_SN = "";
                    Modelopt_pay_lock.HOS_SN = HOS_SN.TrimEnd('|');
                    Modelopt_pay_lock.OPT_SN = dtPre.Rows[0]["OPT_SN"].ToString().Trim();
                    Modelopt_pay_lock.PRE_NO = PRE_NO.TrimEnd('|');
                    Modelopt_pay_lock.RCPT_NO = "";
                    Modelopt_pay_lock.HOS_PAY_SN = "";
                    Modelopt_pay_lock.DJ_DATE = DateTime.Now.Date;
                    Modelopt_pay_lock.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");

                    Modelopt_pay_lock.USER_ID = _in.USER_ID;
                    Modelopt_pay_lock.lTERMINAL_SN = _in.LTERMINAL_SN;
                    Modelopt_pay_lock.SOURCE = "ZZJ";


                    List<SqlSugarModel.OptPayMxLock> Modelopt_pay_mx_locks = new List<SqlSugarModel.OptPayMxLock>();
                    int item_no = 0;
                    if (dtMed!=null && dtMed.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtMed.Rows.Count; i++)
                        {
                            item_no++;
                            SqlSugarModel.OptPayMxLock Modelopt_pay_mx_lock = new SqlSugarModel.OptPayMxLock();

                            Modelopt_pay_mx_lock.PAY_ID = Modelopt_pay_lock.PAY_ID;
                            Modelopt_pay_mx_lock.ITEM_NO = item_no;
                            Modelopt_pay_mx_lock.DAID = dtMed.Rows[i]["DAID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_TYPE = "0";
                            Modelopt_pay_mx_lock.ITEM_CODE = dtMed.Rows[i]["MED_ID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_NAME = dtMed.Rows[i]["MED_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_SPEC = dtMed.Rows[i]["MED_GG"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_UNITS = dtMed.Rows[i]["AUT_NAMEALL"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_PRICE =FormatHelper.GetDecimal(dtMed.Rows[i]["PRICE"].ToString().Trim());
                            Modelopt_pay_mx_lock.AMOUNT = FormatHelper.GetDecimal(dtMed.Rows[i]["CAMTALL"].ToString().Trim());
                            Modelopt_pay_mx_lock.COSTS = FormatHelper.GetDecimal(dtMed.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.CHARGES = FormatHelper.GetDecimal(dtMed.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.ZFBL = null;

                            Modelopt_pay_mx_locks.Add(Modelopt_pay_mx_lock);
                        }
                      
                    }
                    if (dtChkt.Rows.Count > 0)
                    {
                        for (int i =0; i < dtChkt.Rows.Count; i++)
                        {
                            item_no++;
                            SqlSugarModel.OptPayMxLock Modelopt_pay_mx_lock = new SqlSugarModel.OptPayMxLock();

                            Modelopt_pay_mx_lock.PAY_ID = Modelopt_pay_lock.PAY_ID;
                            Modelopt_pay_mx_lock.ITEM_NO = item_no;
                            Modelopt_pay_mx_lock.DAID = dtChkt.Rows[i]["DAID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_TYPE = "1";
                            Modelopt_pay_mx_lock.ITEM_CODE = dtChkt.Rows[i]["CHKIT_ID"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_NAME = dtChkt.Rows[i]["CHKIT_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_SPEC = "";
                            Modelopt_pay_mx_lock.ITEM_UNITS = dtChkt.Rows[i]["AUT_NAME"].ToString().Trim();
                            Modelopt_pay_mx_lock.ITEM_PRICE = FormatHelper.GetDecimal(dtChkt.Rows[i]["PRICE"].ToString().Trim());
                            Modelopt_pay_mx_lock.AMOUNT = FormatHelper.GetDecimal(dtChkt.Rows[i]["CAMTALL"].ToString().Trim());
                            Modelopt_pay_mx_lock.COSTS = FormatHelper.GetDecimal(dtChkt.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.CHARGES = FormatHelper.GetDecimal(dtChkt.Rows[i]["AMOUNT"].ToString().Trim());
                            Modelopt_pay_mx_lock.ZFBL = null;

                            Modelopt_pay_mx_locks.Add(Modelopt_pay_mx_lock);
                        }
                    }

                    try
                    {
                        db.BeginTran();
                       
                        db.Insertable(Modelopt_pay_lock).ExecuteCommand();
                        db.Insertable(Modelopt_pay_mx_locks).ExecuteCommand();
                        db.CommitTran();

                    }
                    catch (Exception ex)
                    {
                        db.RollbackTran();

                        SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                        sqlerror.TYPE = "OUTFEEPAYLOCK";
                        sqlerror.Exception = ex.Message;
                        sqlerror.DateTime = DateTime.Now;
                        LogHelper.SaveSqlerror(sqlerror);
                    }
                }
                catch (Exception ex)
                {
                    SqlSugarModel.Sqlerror sqlerror = new SqlSugarModel.Sqlerror();
                    sqlerror.TYPE = "OUTFEEPAYLOCK";
                    sqlerror.Exception = ex.Message;
                    sqlerror.DateTime = DateTime.Now;
                    LogHelper.SaveSqlerror(sqlerror);
                }
                #endregion

                _out.PAY_ID = PAY_ID;
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:"+ex.Message;
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

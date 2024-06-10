using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos968_OutHos.Model;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos968_OutHos.BUS
{
    class OUTFEEPAYLOCK
    {
        public static string B_OUTFEEPAYLOCK(string json_in)
        {
            if(GlobalVar.DoBussiness=="0")
            {
                return UnDoBusiness(json_in);
            }
            else
            {
                return DoBusiness(json_in);
            }

        }


        public static string UnDoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN _in = JSONSerializer.Deserialize<OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN>(json_in);
                OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT _out = new OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT();

                DataTable dtpre = new DataTable();
                dtpre.Columns.Add("HOS_ID", typeof(string));
                dtpre.Columns.Add("lTERMINAL_SN", typeof(string));
                dtpre.Columns.Add("USER_ID", typeof(string));
                dtpre.Columns.Add("OPT_SN", typeof(string));
                dtpre.Columns.Add("PRE_NO", typeof(string));
                dtpre.Columns.Add("HOS_SN", typeof(string));
                dtpre.Columns.Add("SFZ_NO", typeof(string));
                dtpre.Columns.Add("YLCARD_TYPE", typeof(string));
                dtpre.Columns.Add("YLCARD_NO", typeof(string));

                foreach (OUTFEEPAYLOCK_M.PRE pre in _in.PRELIST)
                {
                    DataRow dr = dtpre.NewRow();
                    dr["HOS_ID"] = _in.HOS_ID;
                    dr["lTERMINAL_SN"] = _in.LTERMINAL_SN;
                    dr["USER_ID"] = _in.USER_ID;
                    dr["OPT_SN"] = pre.OPT_SN;
                    dr["PRE_NO"] = pre.PRE_NO;
                    dr["HOS_SN"] = pre.HOS_SN;
                    dr["SFZ_NO"] = _in.SFZ_NO;
                    dr["YLCARD_TYPE"] = _in.YLCARD_TYPE;
                    dr["YLCARD_NO"] = _in.SFZ_NO;

                    dtpre.Rows.Add(dr);
                }
                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEEPAYLOCK", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRELIST");
                XMLHelper.X_XmlInsertTable(doc, "ROOT/BODY/PRELIST", dtpre, "PRE");
                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                    try
                    {
                        _out.PAY_ID = dtrev.Columns.Contains("PAY_ID") ? dtrev.Rows[0]["PAY_ID"].ToString() : "";
                    }
                    catch
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,未找到ITEMLIST节点,请检查HIS出参";
                        goto EndPoint;
                    }

                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
        public static string DoBusiness(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN _in = JSONSerializer.Deserialize<OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_IN>(json_in);
                OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT _out = new OUTFEEPAYLOCK_M.OUTFEEPAYLOCK_OUT();

                DataTable dtpre = new DataTable();
                dtpre.Columns.Add("HOS_ID", typeof(string));
                dtpre.Columns.Add("lTERMINAL_SN", typeof(string));
                dtpre.Columns.Add("USER_ID", typeof(string));
                dtpre.Columns.Add("OPT_SN", typeof(string));
                dtpre.Columns.Add("PRE_NO", typeof(string));
                dtpre.Columns.Add("HOS_SN", typeof(string));
                dtpre.Columns.Add("SFZ_NO", typeof(string));
                dtpre.Columns.Add("MB_ID", typeof(string));

                DataTable dtMed = new DataTable();DataTable dtChkt = new DataTable();
                DataTable dtPre = new DataTable();
                dtPre.Columns.Add("HOS_ID", typeof(string));
                dtPre.Columns.Add("OPT_SN", typeof(string));
                dtPre.Columns.Add("PRE_NO", typeof(string));
                dtPre.Columns.Add("HOS_SN", typeof(string));
                dtPre.Columns.Add("JEALL", typeof(string));
                dtPre.Columns.Add("CASH_JE", typeof(string));
                dtPre.Columns.Add("DJ_DATE", typeof(string));
                dtPre.Columns.Add("DJ_TIME", typeof(string));
                foreach (OUTFEEPAYLOCK_M.PRE pre in _in.PRELIST)
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
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "LTERMINAL_SN", dr["lTERMINAL_SN"].ToString().Trim().Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", dtpre.Columns.Contains("YLCARD_NO") ? dr["YLCARD_NO"].ToString().Trim() : "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", dtpre.Columns.Contains("SFZ_NO") ? dr["SFZ_NO"].ToString().Trim() : "");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", _in.HOSPATID);

                    string mb_id = dtpre.Columns.Contains("MB_ID") ? dr["MB_ID"].ToString().Trim() : "";
                    if (mb_id != "")
                    {
                        mb_id = "17298";
                    }
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MB_ID", mb_id);
                    string inxml = doc.InnerXml;
                    string his_rtnxml = "";
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
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
                    }

                }

                #region 业务数据处理
                string SFZ_NO = WCApp.CommonFunction.Convert15to18(_in.SFZ_NO);
                DataTable dtPat_info = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("pat_info", "sfz_no='" + _in.SFZ_NO + "'", "PAT_NAME", "PAT_ID");
                if (dtPat_info.Rows.Count == 0)
                {
                    int pat_id = 0;
                    DataTable dtpat = new Plat.MySQLDAL.BaseFunction().GetList_ZZJ("opt_pay_lock", "SFZ_NO=''", "PAT_ID");
                    if (dtpat == null || dtpat.Rows.Count == 0)//没有门诊号的病人用同一pat_id
                    {
                        if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase("pat_info", out pat_id))//首次创建一个pat_id
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "获取病人ID失败，请重试！";
                            goto EndPoint;
                        }
                        else
                        {
                            SFZ_NO = "";
                            DataRow dr = dtPat_info.NewRow();
                            dr["PAT_NAME"] = FormatHelper.GetStr(_in.PAT_NAME) == "" ? FormatHelper.GetStr(_in.PAT_NAME) : "";
                            dr["PAT_ID"] = pat_id;
                            dtPat_info.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dtPat_info.NewRow();
                        dr["PAT_NAME"] = FormatHelper.GetStr(_in.PAT_NAME) == "" ? FormatHelper.GetStr(_in.PAT_NAME) : "";
                        dr["PAT_ID"] = dtpat.Rows[dtpat.Rows.Count - 1]["pat_id"];
                        dtPat_info.Rows.Add(dr);
                    }
                }
                int PAY_ID;
                if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase_ZZJ("opt_pay_lock", out PAY_ID))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "获取pay_id失败!";
                    goto EndPoint;
                }
                Plat.Model.opt_pay_lock Modelopt_pay_lock = new Plat.Model.opt_pay_lock();
                Modelopt_pay_lock.PAY_ID = PAY_ID.ToString().PadLeft(10, '0');
                Modelopt_pay_lock.HOS_ID = _in.HOS_ID;
                Modelopt_pay_lock.PAT_ID = int.Parse(dtPat_info.Rows[0]["pat_id"].ToString().Trim());
                Modelopt_pay_lock.USER_ID =FormatHelper.GetInt(_in.USER_ID);
                Modelopt_pay_lock.OPT_SN = dtPre.Rows[0]["OPT_SN"].ToString().Trim();
                Modelopt_pay_lock.HOS_REG_SN = "";
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
                Modelopt_pay_lock.HOS_SN = HOS_SN.TrimEnd('|');
                Modelopt_pay_lock.PRE_NO = PRE_NO.TrimEnd('|');
                //病人基本信息
                Modelopt_pay_lock.SFZ_NO = _in.SFZ_NO;
                Modelopt_pay_lock.YLCARD_NO = "";
                Modelopt_pay_lock.YLCARD_TYPE = 0;
                Modelopt_pay_lock.PAT_NAME = dtPat_info.Rows[0]["PAT_NAME"].ToString().Trim();
                //从预约挂号表获取数据
                Modelopt_pay_lock.DEPT_CODE = "";
                Modelopt_pay_lock.DEPT_NAME = "";
                Modelopt_pay_lock.DOC_NO = "";
                Modelopt_pay_lock.DOC_NAME = "";
                Modelopt_pay_lock.DIS_NAME = "";
                //待定数据
                Modelopt_pay_lock.lTERMINAL_SN = _in.LTERMINAL_SN;
                Modelopt_pay_lock.CASH_JE = CASH_JE;
                Modelopt_pay_lock.PAY_TYPE = 0;//待定
                Modelopt_pay_lock.JEALL = JE_ALL;
                Modelopt_pay_lock.JZ_CODE = "";//待定
                Modelopt_pay_lock.ybDJH = "";
                Modelopt_pay_lock.GRZF = 0;
                Modelopt_pay_lock.GRZL = 0;
                Modelopt_pay_lock.TCZF = 0;
                Modelopt_pay_lock.DBZF = 0;
                Modelopt_pay_lock.XJZF = 0;
                Modelopt_pay_lock.ZHZF = 0;
                Modelopt_pay_lock.HM = 0;
                Modelopt_pay_lock.CS = 0;
                Modelopt_pay_lock.ZFY = 0;
                Modelopt_pay_lock.XMFY = 0;
                Modelopt_pay_lock.YF = 0;
                Modelopt_pay_lock.LCL = 0;
                Modelopt_pay_lock.ZHYE = 0;
                Modelopt_pay_lock.XZM = "";
                Modelopt_pay_lock.XZMCH = "";
                Modelopt_pay_lock.man_type = "";
                Modelopt_pay_lock.BZFYY = "";
                Modelopt_pay_lock.YBBZM = "";
                Modelopt_pay_lock.FYLB = "";
                Modelopt_pay_lock.YBBZMC = "";
                Modelopt_pay_lock.DJ_DATE = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                Modelopt_pay_lock.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                Modelopt_pay_lock.IS_TZ = false;
                //Modelopt_pay_lock.TZ_DATE
                //Modelopt_pay_lock.TZ_TIME
                //Modelopt_pay_lock.PAY_ID_IN
                Modelopt_pay_lock.lTERMINAL_SN =_in.LTERMINAL_SN;
                Modelopt_pay_lock.PAY_lTERMINAL_SN = _in.LTERMINAL_SN;
                int FL_NOID = 0;
                if (!new Plat.MySQLDAL.BaseFunction().GetSysIdBase_ZZJ("opt_pay_fl_lock", out FL_NOID))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "获取FL_NOID失败!";
                    goto EndPoint;
                }
                Plat.Model.opt_pay_fl_lock[] Modelopt_pay_fl_locks = new Plat.Model.opt_pay_fl_lock[dtPre.Rows.Count];

                Plat.Model.opt_pay_mx_lock[] Modelopt_pay_mx_locks = null;

                for (int i = 0; i < dtPre.Rows.Count; i++)
                {
                    //副联的值如何确定
                    Modelopt_pay_fl_locks[i] = new Plat.Model.opt_pay_fl_lock();
                    Modelopt_pay_fl_locks[i].PAY_ID = Modelopt_pay_lock.PAY_ID;
                    if (dtPre.Rows[i]["PRE_NO"].ToString().Trim().Length > 255)
                    {
                        Modelopt_pay_fl_locks[i].FL_NO = dtPre.Rows[i]["PRE_NO"].ToString().Trim().Substring(0, 255); //FL_NOID.ToString().PadLeft(10, '0');
                    }
                    else
                    {
                        Modelopt_pay_fl_locks[i].FL_NO = dtPre.Rows[i]["PRE_NO"].ToString().Trim(); //FL_NOID.ToString().PadLeft(10, '0');
                    }
                    Modelopt_pay_fl_locks[i].FL_NAME = "";
                    Modelopt_pay_fl_locks[i].DEPT_CODE = Modelopt_pay_lock.DEPT_CODE;
                    Modelopt_pay_fl_locks[i].DEPT_NAME = Modelopt_pay_lock.DEPT_NAME;
                    Modelopt_pay_fl_locks[i].FL_JE = Convert.ToDecimal(dtPre.Rows[i]["JEALL"]); //Modelopt_pay_lock.JEALL;
                    Modelopt_pay_fl_locks[i].FL_ORDER = i.ToString();
                }
                if (dtMed.Rows.Count > 0 || dtChkt.Rows.Count > 0)
                    Modelopt_pay_mx_locks = new Plat.Model.opt_pay_mx_lock[dtMed.Rows.Count + dtChkt.Rows.Count];
                if (dtMed.Rows.Count > 0)
                {

                    for (int i = 0; i < dtMed.Rows.Count; i++)
                    {
                        Modelopt_pay_mx_locks[i] = new Plat.Model.opt_pay_mx_lock();
                        Modelopt_pay_mx_locks[i].PAY_ID = Modelopt_pay_lock.PAY_ID;
                        if (dtMed.Rows[i]["PRENO"].ToString().Trim().Length > 255)
                        {
                            Modelopt_pay_mx_locks[i].FL_NO = dtMed.Rows[i]["PRENO"].ToString().Trim().Substring(0, 255);
                        }
                        else
                        {
                            Modelopt_pay_mx_locks[i].FL_NO = dtMed.Rows[i]["PRENO"].ToString().Trim();
                        }
                        Modelopt_pay_mx_locks[i].ITEM_TYPE = "0";
                        Modelopt_pay_mx_locks[i].ITEM_NAME = dtMed.Rows[i]["MED_NAME"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].ITEM_ID = dtMed.Rows[i]["MED_ID"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].ITEM_GG = dtMed.Rows[i]["MED_GG"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].ITEM_UNIT = dtMed.Rows[i]["AUT_NAMEALL"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].COUNT = dtMed.Rows[i]["CAMTALL"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].COST = decimal.Parse(dtMed.Rows[i]["PRICE"].ToString().Trim());
                        Modelopt_pay_mx_locks[i].je = decimal.Parse(dtMed.Rows[i]["AMOUNT"].ToString().Trim());
                    }
                }
                int j = 0;
                if (dtChkt.Rows.Count > 0)
                {
                    for (int i = dtMed.Rows.Count; i < dtMed.Rows.Count + dtChkt.Rows.Count; i++)
                    {
                        Modelopt_pay_mx_locks[i] = new Plat.Model.opt_pay_mx_lock();
                        Modelopt_pay_mx_locks[i].PAY_ID = Modelopt_pay_lock.PAY_ID;
                        if (dtChkt.Rows[j]["PRENO"].ToString().Trim().Length > 255)
                        {
                            Modelopt_pay_mx_locks[i].FL_NO = dtChkt.Rows[j]["PRENO"].ToString().Trim().Substring(0, 255);
                        }
                        else
                        {
                            Modelopt_pay_mx_locks[i].FL_NO = dtChkt.Rows[j]["PRENO"].ToString().Trim();
                        }
                        Modelopt_pay_mx_locks[i].ITEM_TYPE = "1";
                        Modelopt_pay_mx_locks[i].ITEM_NAME = dtChkt.Rows[j]["CHKIT_NAME"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].ITEM_ID = dtChkt.Rows[j]["CHKIT_ID"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].ITEM_GG = "";
                        Modelopt_pay_mx_locks[i].ITEM_UNIT = dtChkt.Rows[j]["AUT_NAME"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].COUNT = dtChkt.Rows[j]["CAMTALL"].ToString().Trim();
                        Modelopt_pay_mx_locks[i].COST = decimal.Parse(dtChkt.Rows[j]["PRICE"].ToString().Trim());
                        Modelopt_pay_mx_locks[i].je = decimal.Parse(dtChkt.Rows[j]["AMOUNT"].ToString().Trim());
                        j++;
                    }
                }
                Plat.Model.opt_pay_log log = new Plat.Model.opt_pay_log();
                log.PAY_ID = Modelopt_pay_lock.PAY_ID;
                log.STATES = "1";
                log.HOS_ID = _in.HOS_ID;
                log.PAT_ID = int.Parse(dtPat_info.Rows[0]["PAT_ID"].ToString().Trim());
                log.JEALL = Modelopt_pay_lock.JEALL;
                log.CASH_JE = Modelopt_pay_lock.CASH_JE;
                log.DJ_DATE = DateTime.Now;
                log.DJ_TIME = DateTime.Now.ToString("HH:mm:ss");
                log.lTERMINAL_SN = _in.LTERMINAL_SN;
                log.HSP_SN = Modelopt_pay_lock.HOS_SN;

                if (!new Plat.MySQLDAL.opt_pay_lock().AddByTran_ZZJ(Modelopt_pay_lock, Modelopt_pay_fl_locks, Modelopt_pay_mx_locks, log))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "支付锁定存入后台失败!";
                    goto EndPoint;
                }
                #endregion

                _out.PAY_ID =FormatHelper.GetStr(Modelopt_pay_lock.PAY_ID);
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);
                goto EndPoint;
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:"+ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

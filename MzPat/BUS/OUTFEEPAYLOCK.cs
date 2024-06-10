using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZJMZ.Model;
using Soft.Core;
using System.Xml;
using System.Data;

namespace ZZJMZ.BUS
{
    class OUTFEEPAYLOCK
    {
        public static string B_OUTFEEPAYLOCK(string json_in)
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

                foreach (OUTFEEPAYLOCK_M.PRE pre in _in.PRELIST)
                {
                    DataRow dr = dtpre.NewRow();
                    dr["HOS_ID"] =_in.HOS_ID;
                    dr["lTERMINAL_SN"] =_in.LTERMINAL_SN;
                    dr["USER_ID"] =_in.USER_ID;
                    dr["OPT_SN"] = pre.OPT_SN;
                    dr["PRE_NO"] = pre.PRE_NO;
                    dr["HOS_SN"] = pre.HOS_SN;
                    dr["SFZ_NO"] = _in.SFZ_NO;
                    dtpre.Rows.Add(dr);
                }
                XmlDocument doc = QHXmlMode.GetBaseXml("OUTFEEPAYLOCK", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRELIST");
                XMLHelper.X_XmlInsertTable(doc, "ROOT/BODY/PRELIST", dtpre, "PRE");
                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                if (GlobalVar.DoBussiness == "0")
                {
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                    {

                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }

                }
                else if (GlobalVar.DoBussiness == "1")
                {
                    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }
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
                        _out.PAY_ID =dtrev.Columns.Contains("PAY_ID")? dtrev.Rows[0]["PAY_ID"].ToString():"";
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
    }
}

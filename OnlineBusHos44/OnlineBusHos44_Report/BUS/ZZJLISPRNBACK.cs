using CommonModel;
using Soft.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using OnlineBusHos44_Report.Model;
using MySql.Data.MySqlClient;

namespace OnlineBusHos44_Report.BUS
{
    class ZZJLISPRNBACK
    {
        public static string B_ZZJLISPRNBACK(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                ZZJLISPRNBACK_M.ZZJLISPRNBACK_IN _in = JSONSerializer.Deserialize<ZZJLISPRNBACK_M.ZZJLISPRNBACK_IN>(json_in);
                ZZJLISPRNBACK_M.ZZJLISPRNBACK_OUT _out = new ZZJLISPRNBACK_M.ZZJLISPRNBACK_OUT();
                //XmlDocument doc = QHXmlMode.GetBaseXml("ZZJLISPRNBACK", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "172" : _in.HOS_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "REPORT_SN", string.IsNullOrEmpty(_in.REPORT_SN) ? "" : _in.REPORT_SN.Trim());


                
                #region 不管成功失败,记录打印,用于计数
                try
                {
                    StringBuilder str_reportmx = new StringBuilder();
                    str_reportmx.Append("insert into reportmx(HOS_ID,TYPE,HOS_SN,lTERMINAL_SN,USER_ID,NOW) values (");
                    str_reportmx.Append("@HOS_ID,@TYPE,@HOS_SN,@lTERMINAL_SN,@USER_ID,@NOW);");
                    MySqlParameter[] parameters =
                    {
                        new MySqlParameter("@HOS_ID",MySqlDbType.VarChar,20),
                        new MySqlParameter("@TYPE",MySqlDbType.VarChar,30),
                        new MySqlParameter("@HOS_SN",MySqlDbType.VarChar,100),
                        new MySqlParameter("@lTERMINAL_SN",MySqlDbType.VarChar,30),
                        new MySqlParameter("@USER_ID",MySqlDbType.VarChar,30),
                        new MySqlParameter("@NOW",MySqlDbType.DateTime)
                    };
                    parameters[0].Value = _in.HOS_ID;
                    parameters[1].Value = "检验报告";
                    parameters[2].Value = _in.REPORT_SN;
                    parameters[3].Value = _in.LTERMINAL_SN;
                    parameters[4].Value = _in.USER_ID;
                    parameters[5].Value = DateTime.Now;
                    DB.Core.DbHelperMySQLZZJ.ExecuteSql(str_reportmx.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    Log.Core.Model.ModSqlError logsql = new Log.Core.Model.ModSqlError();
                    logsql.TYPE = "检验";
                    logsql.EXCEPTION = ex.ToString();
                    logsql.time = DateTime.Now;
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(logsql);
                }
                #endregion
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                goto EndPoint;
                
            //    string inxml = doc.InnerXml;
            //    string his_rtnxml = "";
            //    if (!GlobalVar.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
            //    {
            //        dataReturn.Code = 1;
            //        dataReturn.Msg = his_rtnxml;
            //        goto EndPoint;
            //    }

            //    _out.HIS_RTNXML = his_rtnxml;
                
            //    try
            //    {
            //        XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
            //        DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
            //        DataTable dtrev = ds.Tables[0];
            //        if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
            //        {
            //            dataReturn.Code = 1;
            //            dataReturn.Msg = dtrev.Columns.Contains("CLJG") ? dtrev.Rows[0]["CLJG"].ToString() : "";
            //            dataReturn.Param = JSONSerializer.Serialize(_out);
            //            goto EndPoint;
            //        }
            //        dataReturn.Code = 0;
            //        dataReturn.Msg = "SUCCESS";
            //        dataReturn.Param = JSONSerializer.Serialize(_out);

            //    }
            //    catch (Exception ex)
            //    {
            //        dataReturn.Code = 5;
            //        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";

            //    }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常:"+ ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

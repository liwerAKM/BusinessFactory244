using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineBusHos974_EInvoice.Class;
using CommonModel;
using ConstData;
using Soft.Core;
using MySql.Data.MySqlClient;

namespace OnlineBusHos974_EInvoice.BUS
{
    class UpdatePrintStatus
    {
        public static string B_UpdatePrintStatus(string json_in)
        {

            DataReturn dataReturn = new DataReturn();
            try
            {
                UpdatePrintStatus_IN _in = JSONSerializer.Deserialize<UpdatePrintStatus_IN>(json_in);
                Root root = new Root();
                ROOT ROOT = new ROOT();
                HEADER head = new HEADER();
                head.MODULE = "";
                head.TYPE = "UPDATEPRINTSTATUS";
                head.CZLX = "";
                head.SOURCE = "ZZJ";
                ROOT.HEADER = head;
                root.ROOT = ROOT;
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("HOS_ID", FormatHelper.GetStr(_in.HOS_ID));
                dic.Add("invoice_code", FormatHelper.GetStr(_in.INVOICE_CODE));
                dic.Add("invoice_number", FormatHelper.GetStr(_in.INVOICE_NUMBER));
                root.BODY = dic;
                string injson = JSONSerializer.Serialize(root);
                string his_rtnxml = "";
                if (!GlobalVar.CALLSERVICE(_in.HOS_ID, injson, ref his_rtnxml))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }
                #region 不管成功失败,记录打印,用于计数
                /*
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
                    parameters[1].Value = "电子发票";
                    parameters[2].Value = FormatHelper.GetStr(_in.INVOICE_CODE) + "-" + FormatHelper.GetStr(_in.INVOICE_NUMBER);
                    parameters[3].Value = _in.LTERMINAL_SN;
                    parameters[4].Value = _in.USER_ID;
                    parameters[5].Value = DateTime.Now;
                    DB.Core.DbHelperMySQLZZJ.ExecuteSql(str_reportmx.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    Log.Core.Model.ModSqlError logsql = new Log.Core.Model.ModSqlError();
                    logsql.TYPE = "电子发票";
                    logsql.EXCEPTION = ex.ToString();
                    logsql.time = DateTime.Now;
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(logsql);
                }
                */
                #endregion
                Root_rtn root_rtn = JSONSerializer.Deserialize<Root_rtn>(his_rtnxml);
                string json = JSONSerializer.Serialize(root_rtn.ROOT.BODY);
                BODY body = JSONSerializer.Deserialize<BODY>(json);
                if (body.CLBZ != "0")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = body.CLJG;
                    goto EndPoint;
                }
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

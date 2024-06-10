using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using System.Reflection.Emit;
using OnlineBusHos319_YYGH.Model;
using static OnlineBusHos319_YYGH.Model.GETSCHPERIOD_M;
using System.Collections;

namespace OnlineBusHos319_YYGH.BUS
{
    class GETSCHPERIOD
    {
        public static string B_GETSCHPERIOD(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETSCHPERIOD_M.GETSCHPERIOD_IN _in = JSONSerializer.Deserialize<Model.GETSCHPERIOD_M.GETSCHPERIOD_IN>(json_in);
                Model.GETSCHPERIOD_M.GETSCHPERIOD_OUT _out = new Model.GETSCHPERIOD_M.GETSCHPERIOD_OUT();

                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string SCH_DATE = string.IsNullOrEmpty(_in.SCH_DATE) ? DateTime.Today.ToString("yyyy-MM-dd") : _in.SCH_DATE.Trim();
                //XmlDocument doc = QHXmlMode.GetBaseXml("GETSCHPERIOD", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_CODE", string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DOC_NO", string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TYPE", string.IsNullOrEmpty(_in.SCH_TYPE) ? "" : _in.SCH_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_DATE", string.IsNullOrEmpty(_in.SCH_DATE) ? DateTime.Today.ToString("yyyy-MM-dd") : _in.SCH_DATE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SCH_TIME", string.IsNullOrEmpty(_in.SCH_TIME) ? "" : _in.SCH_TIME.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "QUERY_TYPE", "01");


                HISModels.A108.Request request = new HISModels.A108.Request()
                {
                    operCode = _operCode,
                    operName = _operName,
                    hospCode = _hospCode,
                    operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    scheduleNo = _in.REGISTER_TYPE
                };

                //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test1.txt");
                //string his_rtnjson = stream.ReadToEnd();

                string inputjson = JSONSerializer.Serialize(request);
                string his_rtnjson = "";
                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A108", ref his_rtnjson))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnjson;
                    goto EndPoint;
                }


                try
                {
                    HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                    if (baseRsponse.code != "200")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = baseRsponse.message;
                        goto EndPoint;
                    }
                    _out.PERIODLIST = new List<Model.GETSCHPERIOD_M.PERIOD>();
                    List<HISModels.A108.Data> datas = baseRsponse.GetInput<List<HISModels.A108.Data>>();

                    var query = datas.Where(w => (SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd") ? w.sequenceType == "0" : w.sequenceType == "1") && w.state == "0").GroupBy(x => new { x.timePeriod, x.scheduleNo }).Select(q => new
                    {
                        PERIOD_START = q.Key.timePeriod.ToString() == "-" ? DateTime.Now.Hour >= 12 ? "14:00" : "08:00" : q.Key.timePeriod.Substring(0, 5),
                        PERIOD_END = q.Key.timePeriod.ToString() == "-" ? DateTime.Now.Hour >= 12 ? "17:30" : "11:30" : q.Key.timePeriod.Substring(6, 5),
                        REGISTER_TYPE = q.Key.scheduleNo,
                        COUNT_REM = q.Count()
                    }).Distinct();
                    if (false)
                    {
                        Model.GETSCHPERIOD_M.PERIOD period = new Model.GETSCHPERIOD_M.PERIOD();
                        period.PERIOD_START = "08:00";
                        period.PERIOD_END = "17:00";
                        period.REGISTER_TYPE = _in.REGISTER_TYPE;
                        period.COUNT_REM = datas.Count.ToString();
                        _out.PERIODLIST.Add(period);
                    }
                    else
                    {

                        foreach (var q in query.Where(x=>Convert.ToDateTime(x.PERIOD_END)>DateTime.Now))//只展示未结束的时间段
                        {
                            Model.GETSCHPERIOD_M.PERIOD period = new Model.GETSCHPERIOD_M.PERIOD();
                            period.PERIOD_START = q.PERIOD_START;
                            period.PERIOD_END = q.PERIOD_END;
                            period.REGISTER_TYPE = _in.REGISTER_TYPE;
                            period.COUNT_REM = q.COUNT_REM.ToString();
                            _out.PERIODLIST.Add(period);
                        }

                    }




                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败:" + ex.Message;
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

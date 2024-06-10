using System;
using System.Collections.Generic;
using System.Text;
using Soft.Core;
using System.Xml;
using System.Data;
using CommonModel;
using OnlineBusHos319_YYGH.Model;
using System.Linq;


namespace OnlineBusHos319_YYGH.BUS
{
    class GETREGISTERLIST
    {
        public static string B_GETREGISTERLIST(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            Model.GETREGISTERLIST_M.GETREGISTERLIST_IN _in = JSONSerializer.Deserialize<Model.GETREGISTERLIST_M.GETREGISTERLIST_IN>(json_in);
            Model.GETREGISTERLIST_M.GETREGISTERLIST_OUT _out = new Model.GETREGISTERLIST_M.GETREGISTERLIST_OUT();
            string HOSPATID = string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID;
            string _hospCode = "12321283469108887C";
            string _operCode = "zzj01";
            string _operName = "自助机01";
            string HOS_ID = _in.HOS_ID;
            if (string.IsNullOrEmpty(HOSPATID))
            {
                dataReturn.Code = 1;
                dataReturn.Msg = "获取挂号记录院内号不能为空";
                goto EndPoint;
            }

          


            Model.HISModels.A113.A113Request request = new Model.HISModels.A113.A113Request()
            {
                patiId=HOSPATID,
                hospCode = _hospCode,
                operCode = _operCode,
                operName = _operName,
                operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            string inputjson = JSONSerializer.Serialize(request);
            string his_rtnjson = "";
            if (!PubFunc.CallHISService(HOS_ID, inputjson, "A113", ref his_rtnjson))
            {
                dataReturn.Code = 1;
                dataReturn.Msg = his_rtnjson;
                goto EndPoint;
            }
         


            _out.HIS_RTNXML = "";
            try
            {
                HISModels.baseRsponse baseRsponse = JSONSerializer.Deserialize<HISModels.baseRsponse>(his_rtnjson);

                if (baseRsponse.code != "200")
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = baseRsponse.message;
                    goto EndPoint;
                }
                List<HISModels.A113.Data> datas = baseRsponse.GetInput<List<HISModels.A113.Data>>();
                _out.APPTLIST = new List<Model.GETREGISTERLIST_M.AppTist>();

                if (datas==null || datas.Count == 0)//无记录
                {
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    goto EndPoint;
                }

                _out.APPTLIST = (from items in datas
                            where items.seeState == "0"
                            select new GETREGISTERLIST_M.AppTist
                            {
                                SCH_DATE= items.bookingDate,
                                SCH_TIME= items.noonName,
                                SCH_TYPE=string.IsNullOrEmpty(items.doctCode)?"1":"2",
                                PERIOD_START=items.beginTime,
                                PERIOD_END=items.endTime,
                                DEPT_CODE=items.deptCode,
                                DEPT_NAME=items.deptName,
                                DOC_NO=items.doctCode,
                                DOC_NAME=items.doctName,
                                REGISTER_TYPE=items.scheduleNo,
                                HOS_SN=items.bookingNo,
                                APPT_PAY=items.totCost,
                                APPT_TYPE="0",
                                APPT_ORDER=items.sequenceNo,
                                OPT_SN=HOSPATID,
                                PRO_TITLE="",
                                USER_ID="",
                                YB_INFO="",
                                JEALL= items.totCost,
                                APPT_TIME= items.noonName,
                                APPT_PLACE=""
                            }).ToList();         


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

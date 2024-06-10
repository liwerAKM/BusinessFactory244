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

namespace OnlineBusHos319_YYGH.BUS
{
    class GETSCHINFO
    {
        public static string B_GETSCHINFO(string json_in)
        {
            return Business(json_in);
        }

        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETSCHINFO_M.GETSCHINFO_IN _in = JSONSerializer.Deserialize<Model.GETSCHINFO_M.GETSCHINFO_IN>(json_in);
                Model.GETSCHINFO_M.GETSCHINFO_OUT _out = new Model.GETSCHINFO_M.GETSCHINFO_OUT();
                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string DOC_NO = string.IsNullOrEmpty(_in.DOC_NO) ? "" : _in.DOC_NO.Trim();
                string DEPT_CODE = string.IsNullOrEmpty(_in.DEPT_CODE) ? "" : _in.DEPT_CODE.Trim();
                string SCH_DATE = string.IsNullOrEmpty(_in.SCH_DATE) ? "" : _in.SCH_DATE.Trim();
                string beginDate = DateTime.Today.ToString("yyyy-MM-dd");
                string endDate = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
                //排班日期不为空取对应排班
                if (SCH_DATE != "")
                {
                    beginDate = SCH_DATE.Substring(0,10);
                    endDate = SCH_DATE.Substring(0,10);
                }
               
                

                HISModels.A104.A104Reguest a104Reguest = new HISModels.A104.A104Reguest()
                {
                    beginTime = beginDate,
                    deptCode = DEPT_CODE,
                    doctCode = "",
                    endTime = endDate,
                    hospCode = _hospCode,
                    operCode = _operCode,
                    operName = _operName,
                    operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    scheduleType = "2"
                };
                string inputjson = JSONSerializer.Serialize(a104Reguest);
                string his_rtnjson = "";
                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A104", ref his_rtnjson))
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

                    List<HISModels.A104.Data> datas = baseRsponse.GetInput<List<HISModels.A104.Data>>();

                    

                    _out.DEPTLIST = new List<Model.GETSCHINFO_M.DEPT>();
                    _out.DOCLIST = new List<Model.GETSCHINFO_M.DOC>();
                    foreach (HISModels.A104.Data data in datas.Where(x=> Convert.ToDateTime(x.seeDate) > DateTime.Today ||(Convert.ToDateTime(x.seeDate) == DateTime.Today && Convert.ToDateTime(x.endTime) >= DateTime.Now)))//仅展示今日以后 及今日仍在接诊时间排班
                    {
                        #region 下午不显示上午的排班
                        //if (DateTime.Now.Hour >= 12)
                        //{
                        //    if (data.noonName == "上午" && SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd"))
                        //    {
                        //        continue;
                        //    }
                        //}
                        #endregion
                        /*
                        if(Convert.ToDateTime(data.seeDate)<DateTime.Today || (Convert.ToDateTime(data.seeDate) == DateTime.Today&& Convert.ToDateTime(data.endTime) < DateTime.Now))//当日以前及当日超过就诊时间的排班过滤
                        {
                            continue;
                        }
                        */


                        if (string.IsNullOrEmpty(data.doctCode))//普通号
                        {
                            Model.GETSCHINFO_M.DEPT dept = new Model.GETSCHINFO_M.DEPT();
                            dept.DEPT_CODE = data.deptCode;
                            dept.DEPT_NAME = data.deptName;
                            dept.DOC_NO = data.doctCode;
                            dept.DOC_NAME = data.doctName;
                            dept.GH_FEE = "0";
                            dept.ZL_FEE = data.regPrice;
                            dept.SCH_TYPE = "1";
                            dept.SCH_DATE = Convert.ToDateTime(data.seeDate).ToString("yyyy-MM-dd");//执行日期
                            dept.SCH_TIME = data.noonName;
                            dept.PERIOD_START = Convert.ToDateTime(data.beginTime).ToString("HH:mm:ss");//开始时间段例 8：00
                            dept.PERIOD_END = Convert.ToDateTime(data.endTime).ToString("HH:mm:ss");//结束时间例 11：00
                            dept.CAN_WAIT =  "1";
                            dept.REGISTER_TYPE = data.scheduleNo;
                            dept.REGISTER_TYPE_NAME = "";
                            dept.STATUS = "";
                            if (SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd"))//当日
                            {
                                dept.COUNT_REM = data.surplusWindowCount;
                            }
                            else
                            {
                                dept.COUNT_REM = data.surplusAppointmentCount;
                            }
                            
                            dept.YB_CODE = "";
                            dept.PRO_TITLE = "";

                            _out.DEPTLIST.Add(dept);
                        }
                        else//专家号
                        {
                           

                            Model.GETSCHINFO_M.DOC doctor = new Model.GETSCHINFO_M.DOC();
                            doctor.DOC_NO = data.doctCode;
                            doctor.DOC_NAME = data.doctName;
                            doctor.GH_FEE = "0";
                            doctor.ZL_FEE = data.regPrice;
                            doctor.SCH_TYPE ="2";
                            doctor.SCH_DATE = Convert.ToDateTime(data.seeDate).ToString("yyyy-MM-dd");//执行日期
                            doctor.SCH_TIME = data.noonName;
                            doctor.PERIOD_START = Convert.ToDateTime(data.beginTime).ToString("HH:mm:ss");//开始时间段例 8：00
                            doctor.PERIOD_END = Convert.ToDateTime(data.endTime).ToString("HH:mm:ss");//结束时间例 11：00
                            doctor.CAN_WAIT = "1";

                            doctor.REGISTER_TYPE = data.scheduleNo;;
                            doctor.REGISTER_TYPE_NAME = "";
                            doctor.STATUS = "";
                           
                            if (SCH_DATE == DateTime.Now.ToString("yyyy-MM-dd"))//当日
                            {
                                doctor.COUNT_REM = data.surplusWindowCount;
                            }
                            else
                            {
                                doctor.COUNT_REM = data.surplusAppointmentCount;
                            }

                            doctor.YB_CODE = "";
                            doctor.PRO_TITLE = "";

                            _out.DOCLIST.Add(doctor);
                        }

                    }
                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch(Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确:"+ex.Message;

                    goto EndPoint;
                }


              
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

using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using System.IO;
using System.Reflection.Emit;
using OnlineBusHos319_YYGH.Model;
//using PasS.Base.Lib;

namespace OnlineBusHos319_YYGH.BUS
{
    class GETHOSPDEPT
    {
        public static string B_GETHOSPDEPT(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETHOSPDEPT_M.GETHOSPDEPT_IN _in = JSONSerializer.Deserialize<Model.GETHOSPDEPT_M.GETHOSPDEPT_IN>(json_in);
                Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT _out = new Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT();
                //XmlDocument doc = QHXmlMode.GetBaseXml("GETHOSPDEPT", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", string.IsNullOrEmpty(_in.USE_TYPE) ? "" : _in.USE_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_TYPE", string.IsNullOrEmpty(_in.FILT_TYPE) ? "" : _in.FILT_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_VALUE", string.IsNullOrEmpty(_in.FILT_VALUE) ? "" : _in.FILT_VALUE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "RETURN_TYPE", string.IsNullOrEmpty(_in.RETURN_TYPE) ? "" : _in.RETURN_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", string.IsNullOrEmpty(_in.PAGEINDEX) ? "" : _in.PAGEINDEX.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", string.IsNullOrEmpty(_in.PAGESIZE) ? "" : _in.PAGESIZE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "DEPT_USE", string.IsNullOrEmpty(_in.PAGESIZE) ? "" : _in.PAGESIZE.Trim());
                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string FILT_TYPE = _in.FILT_TYPE;
                string FILT_VALUE = _in.FILT_VALUE;
                Model.HISModels.A105.A105Request request = new Model.HISModels.A105.A105Request()
                {
                    beginTime = DateTime.Today.ToString("yyyy-MM-dd"),
                    endTime = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd"),
                    hospCode = _hospCode,
                    operCode = _operCode,
                    operName = _operName,
                    operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string inputjson = JSONSerializer.Serialize(request);
                string his_rtnjson = "";

                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A105", ref his_rtnjson))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnjson;
                    goto EndPoint;
                }




                //StreamReader stream = new StreamReader("D:/test1.txt");
                //his_rtnjson = stream.ReadToEnd();
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


                    List<HISModels.A105.Data> datas = baseRsponse.GetInput<List<HISModels.A105.Data>>();

                    try
                    {
                        _out.DEPTLIST = new List<Model.GETHOSPDEPT_M.DEPT>();

                        if (FILT_TYPE=="01")//取一级科室 科室编号与上级科室一致
                        {
                            //DataTable dtLevel1 = new DataTable();
                            //dtLevel1.Columns.Add("DEPT_CODE", typeof(string));
                            //dtLevel1.Columns.Add("DEPT_NAME", typeof(string));
                            //筛选出一级科室
                            /*
                            DataTable dtLevel1 = (from items in datas
                                                  where items.parentCode == items.deptCode
                                                  group items by new { items.deptCode, items.deptName }
                                                 into newItems
                                                  select new
                                                  {
                                                      DEPT_CODE = newItems.Key.deptCode,
                                                      DEPT_NAME = newItems.Key.deptName,

                                                  }).CopyToDataTable();
                            */
                            List<HISModels.A105.Data> ListLevel1= datas.Where(x => x.parentCode==x.deptCode&&!x.deptName.Contains("润泰")).ToList();
                            
                            /*
                            foreach(DataRow dr in dtLevel1.Rows)
                            {

                                string DEPT_CODE_1= dr["DEPT_CODE"].ToString();
                                string DEPT_NAME_1= dr["DEPT_NAME"].ToString();
                                foreach (HISModels.A105.Data data in datas.Where(x => x.parentCode.Equals(DEPT_CODE_1)))
                                {
                                    Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();

                                    dept.DEPT_CODE = DEPT_CODE_1;
                                    dept.DEPT_NAME = DEPT_NAME_1;
                                    dept.DEPT_INTRO = "";
                                    dept.DEPT_URL = "";
                                    dept.DEPT_ORDER = "";
                                    dept.DEPT_TYPE = "";
                                    dept.CHILDREN = new List<GETHOSPDEPT_M.DEPT>();
                                    dept.DEPT_ADDRESS = "";
                                    _out.DEPTLIST.Add(dept);
                                }


                            }
                            */


                            foreach (HISModels.A105.Data data in ListLevel1)//筛选出一级科室
                            {
                                Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();

                                dept.DEPT_CODE = data.deptCode;
                                dept.DEPT_NAME = data.deptName;
                                dept.DEPT_INTRO = "";
                                dept.DEPT_URL = "";
                                dept.DEPT_ORDER = "";
                                dept.DEPT_TYPE = "";
                                dept.CHILDREN = new List<GETHOSPDEPT_M.DEPT>();
                                dept.DEPT_ADDRESS = "";
                                _out.DEPTLIST.Add(dept);
                            }



                        }
                        else if(FILT_TYPE=="04"&&!string.IsNullOrEmpty(FILT_VALUE))//按一级科室查询
                        {

                            List<HISModels.A105.Data> ListLevel2 = datas.Where(x => x.parentCode==FILT_VALUE && !x.deptName.Contains("润泰")).ToList();

                            foreach (HISModels.A105.Data data in ListLevel2)
                            {
                                Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();

                                dept.DEPT_CODE = data.deptCode;
                                dept.DEPT_NAME = data.deptName;
                                dept.DEPT_INTRO = "";
                                dept.DEPT_URL = "";
                                dept.DEPT_ORDER = "";
                                dept.DEPT_TYPE = "";
                                dept.CHILDREN = new List<GETHOSPDEPT_M.DEPT>();
                                dept.DEPT_ADDRESS = "";
                                _out.DEPTLIST.Add(dept);
                            }
                        }






                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参";
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

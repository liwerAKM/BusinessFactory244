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
using OnlineBusHos319_OutHos.Model;

namespace OnlineBusHos319_OutHos.BUS
{
    class GETOUTFEENOPAY
    {
        public static string B_GETOUTFEENOPAY(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN _in = JSONSerializer.Deserialize<Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN>(json_in);
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT _out = new Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT();
          
                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string HOSPATID = _in.HOSPATID;

                if (string.IsNullOrEmpty(HOSPATID))
                {
                    dataReturn.Code = 1;
                    dataReturn.Msg = "获取处方信息是HOSPATID不能为空";
                    goto EndPoint;
                }

                HISModels.A309.A309Request a309Request = new HISModels.A309.A309Request()
                {
                    operCode = _operCode,
                    operName = _operName,
                    patiId = HOSPATID,
                    hospCode = _hospCode,
                    operTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                string inputjson = JSONSerializer.Serialize(a309Request);
                string his_rtnjson = "";
                if (!PubFunc.CallHISService(HOS_ID, inputjson, "A309", ref his_rtnjson))
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

                    List<HISModels.A309.A309Response.Data> data = baseRsponse.GetInput<List<HISModels.A309.A309Response.Data>>();
                    _out.PRELIST = new List<Model.GETOUTFEENOPAY_M.PRE>();

                    for (int i = 0; i < data.Count; i++)
                    {
                        Model.GETOUTFEENOPAY_M.PRE pre = new Model.GETOUTFEENOPAY_M.PRE();
                        pre.OPT_SN = data[i].recipeSummary.clinicNo;
                        pre.PRE_NO = data[i].recipeSummary.recipeNo;
                        pre.HOS_SN = data[i].recipeSummary.recipeNo;
                        pre.DEPT_CODE = data[i].recipeSummary.execDeptCode;
                        pre.DEPT_NAME = string.IsNullOrEmpty(data[i].recipeSummary.execDeptName) ? "" : data[i].recipeSummary.execDeptName;
                        pre.DOC_NO = data[i].recipeSummary.doctCode;
                        pre.DOC_NAME = data[i].recipeSummary.doctName;
                        pre.JEALL = data[i].recipeSummary.totCost;
                        pre.CASH_JE = data[i].recipeSummary.totCost;
                        pre.YB_PAY = "1";
                        pre.PRE_TYPE = data[i].recipeSummary.recipeType;//处方类别（01,普通处方;04,毒性药品;05,精神I类;06,精神Ⅱ类;07,精神Ⅲ类;08,戒毒药品;09,麻醉处方）
                        pre.SIRECIPETYPE = data[i].recipeSummary.siRecipeType;
                        _out.PRELIST.Add(pre);
                    }





                    #region 处方合并
                    /*
                    DataView dataView1 = dtpre.DefaultView;
                    dataView1.RowFilter = "HOS_SN<>''";//排除为空的数据
                    dtpre = dataView1.ToTable();
                    if (_in.YLCARD_TYPE != "2")//非医保用户,合并为一条
                    {
                        Model.GETOUTFEENOPAY_M.PRE preZf = new Model.GETOUTFEENOPAY_M.PRE();
                        //不需要合并的字段
                        preZf.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dtpre.Rows[0]["OPT_SN"].ToString() : "";
                        preZf.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dtpre.Rows[0]["DEPT_CODE"].ToString() : "";
                        preZf.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dtpre.Rows[0]["DEPT_NAME"].ToString() : "";
                        preZf.DOC_NO = dtpre.Columns.Contains("DOC_NO") ? dtpre.Rows[0]["DOC_NO"].ToString() : "";
                        preZf.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dtpre.Rows[0]["DOC_NAME"].ToString() : "";
                        preZf.YB_PAY = "1";
                        preZf.YB_NOPAY_REASON = "";
                        preZf.YLLB = "";
                        preZf.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dtpre.Rows[0]["DIS_CODE"].ToString() : ""; ;
                        preZf.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dtpre.Rows[0]["DIS_TYPE"].ToString() : "";

                        //需要合并的字段
                        decimal JEALL_ZF = 0;
                        decimal CASH_JE_ZF = 0;
                        string PRE_NO_ZF = "";
                        string HOS_SN_ZF = "";
                        foreach (DataRow dr in dtpre.Rows)
                        {
                            PRE_NO_ZF += dr["PRE_NO"].ToString() + ",";
                            HOS_SN_ZF += dr["HOS_SN"].ToString() + ",";
                            JEALL_ZF += Convert.ToDecimal(dr["JEALL"].ToString());
                            CASH_JE_ZF += Convert.ToDecimal(dr["CASH_JE"].ToString());
                        }
                        preZf.PRE_NO = PRE_NO_ZF.TrimEnd(',');
                        preZf.HOS_SN = HOS_SN_ZF.TrimEnd(',');
                        preZf.JEALL = JEALL_ZF.ToString("0.00");
                        preZf.CASH_JE = CASH_JE_ZF.ToString("0.00");

                        _out.PRELIST.Add(preZf);
                    }
                    else//医保患者
                    {
                        //根据YB_PAY字段进行合并 0纯自费 1普通医保 2特殊医保  
                        DataView dataView = dtpre.DefaultView;
                        dataView.RowFilter = "YB_PAY=0";
                        DataTable dtZfPre = dataView.ToTable();//自费处方容器

                        dataView.RowFilter = "YB_PAY=1";
                        DataTable dtPtYbPre = dataView.ToTable();//普通医保处方容器

                        dataView.RowFilter = "YB_PAY=2";
                        DataTable dtTsYbPre = dataView.ToTable();//特殊医保处方容器，这类处方不进行合并  

                        //自费处方合并
                        if (dtZfPre != null && dtZfPre.Rows.Count > 0)
                        {
                            Model.GETOUTFEENOPAY_M.PRE preZf = new Model.GETOUTFEENOPAY_M.PRE();//合并后的自费处方
                                                                                                //不需要合并的字段
                            preZf.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dtZfPre.Rows[0]["OPT_SN"].ToString() : "";
                            preZf.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dtZfPre.Rows[0]["DEPT_CODE"].ToString() : "";
                            preZf.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dtZfPre.Rows[0]["DEPT_NAME"].ToString() : "";
                            preZf.DOC_NO = dtpre.Columns.Contains("DOC_NO") ? dtZfPre.Rows[0]["DOC_NO"].ToString() : "";
                            preZf.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dtZfPre.Rows[0]["DOC_NAME"].ToString() : "";
                            preZf.YB_PAY = "0";
                            preZf.YB_NOPAY_REASON = "该处方只支持自费缴费";
                            preZf.YLLB = "";
                            preZf.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dtZfPre.Rows[0]["DIS_CODE"].ToString() : ""; ;
                            preZf.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dtZfPre.Rows[0]["DIS_TYPE"].ToString() : "";

                            //需要合并的字段
                            decimal JEALL_ZF = 0;
                            decimal CASH_JE_ZF = 0;
                            string PRE_NO_ZF = "";
                            string HOS_SN_ZF = "";
                            foreach (DataRow dr in dtZfPre.Rows)
                            {
                                PRE_NO_ZF += dr["PRE_NO"].ToString() + ",";
                                HOS_SN_ZF += dr["HOS_SN"].ToString() + ",";
                                JEALL_ZF += Convert.ToDecimal(dr["JEALL"].ToString());
                                CASH_JE_ZF += Convert.ToDecimal(dr["CASH_JE"].ToString());
                            }
                            preZf.PRE_NO = PRE_NO_ZF.TrimEnd(',');
                            preZf.HOS_SN = HOS_SN_ZF.TrimEnd(',');
                            preZf.JEALL = JEALL_ZF.ToString("0.00");
                            preZf.CASH_JE = CASH_JE_ZF.ToString("0.00");

                            _out.PRELIST.Add(preZf);
                        }



                        if (dtPtYbPre != null && dtPtYbPre.Rows.Count > 0)
                        {
                            Model.GETOUTFEENOPAY_M.PRE prePtYB = new Model.GETOUTFEENOPAY_M.PRE();//合并后的普通医保处方
                                                                                                  //不需要合并的字段
                            prePtYB.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dtPtYbPre.Rows[0]["OPT_SN"].ToString() : "";
                            prePtYB.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dtPtYbPre.Rows[0]["DEPT_CODE"].ToString() : "";
                            prePtYB.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dtPtYbPre.Rows[0]["DEPT_NAME"].ToString() : "";
                            prePtYB.DOC_NO = dtpre.Columns.Contains("DOC_NO") ? dtPtYbPre.Rows[0]["DOC_NO"].ToString() : "";
                            prePtYB.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dtPtYbPre.Rows[0]["DOC_NAME"].ToString() : "";
                            prePtYB.YB_PAY = "1";
                            prePtYB.YB_NOPAY_REASON = "";
                            prePtYB.YLLB = dtpre.Columns.Contains("YLLB") ? dtPtYbPre.Rows[0]["YLLB"].ToString() : ""; ;
                            prePtYB.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dtPtYbPre.Rows[0]["DIS_CODE"].ToString() : "";
                            prePtYB.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dtPtYbPre.Rows[0]["DIS_TYPE"].ToString() : "";

                            //需要合并的字段
                            decimal JEALL_PTYB = 0;
                            decimal CASH_JE_PTYB = 0;
                            string PRE_NO_PTYB = "";
                            string HOS_SN_PTYB = "";
                            foreach (DataRow dr in dtPtYbPre.Rows)
                            {
                                PRE_NO_PTYB += dr["PRE_NO"].ToString() + ",";
                                HOS_SN_PTYB += dr["HOS_SN"].ToString() + ",";
                                JEALL_PTYB += Convert.ToDecimal(dr["JEALL"].ToString());
                                CASH_JE_PTYB += Convert.ToDecimal(dr["CASH_JE"].ToString());
                            }
                            prePtYB.PRE_NO = PRE_NO_PTYB.TrimEnd(',');
                            prePtYB.HOS_SN = HOS_SN_PTYB.TrimEnd(',');
                            prePtYB.JEALL = JEALL_PTYB.ToString("0.00");
                            prePtYB.CASH_JE = CASH_JE_PTYB.ToString("0.00");

                            _out.PRELIST.Add(prePtYB);
                        }

                        foreach (DataRow dr in dtTsYbPre.Rows)//不需要合并的特殊处方
                        {
                            Model.GETOUTFEENOPAY_M.PRE pre = new Model.GETOUTFEENOPAY_M.PRE();
                            pre.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dr["OPT_SN"].ToString() : "";
                            pre.PRE_NO = dtpre.Columns.Contains("PRE_NO") ? dr["PRE_NO"].ToString() : "";
                            pre.HOS_SN = dtpre.Columns.Contains("HOS_SN") ? dr["HOS_SN"].ToString() : "";
                            pre.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                            pre.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                            pre.DOC_NO = dtpre.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                            pre.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                            pre.JEALL = dtpre.Columns.Contains("JEALL") ? dr["JEALL"].ToString() : "";
                            pre.CASH_JE = dtpre.Columns.Contains("CASH_JE") ? dr["CASH_JE"].ToString() : "";
                            pre.YB_PAY = "1";//与his的同名字段意思不同
                            pre.YB_NOPAY_REASON = "";

                            pre.YLLB = dtpre.Columns.Contains("YLLB") ? dr["YLLB"].ToString() : "";
                            pre.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dr["DIS_CODE"].ToString() : "";
                            pre.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dr["DIS_TYPE"].ToString() : "";

                            _out.PRELIST.Add(pre);
                        }


                    }

                    */
                    #endregion
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

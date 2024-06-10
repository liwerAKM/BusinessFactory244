using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;

namespace OnlineBusHos172_OutHos.BUS
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
                string MB_ID = string.IsNullOrEmpty(_in.MB_ID) ? "" : _in.MB_ID.Trim();
                if (!MB_ID.Equals(""))//如果为核酸检测缴费则跳过该接口
                {
                    dataReturn.Code = 0;
                    dataReturn.Msg = "核酸检测不适用该接口";
                    dataReturn.Param = JSONSerializer.Serialize(_out);
                    goto EndPoint;
                }
                XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAY", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN","");

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
                    try 
                    {
                        DataTable dtpre = ds.Tables["PRE"];
                        _out.PRELIST = new List<Model.GETOUTFEENOPAY_M.PRE>();
                        if (dtpre.Rows.Count == 0)//判断下是否有处方
                        {
                            dataReturn.Code =0;
                            dataReturn.Msg = "未查询到处方";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }

                        #region 处方合并
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
                            #endregion





                        }
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

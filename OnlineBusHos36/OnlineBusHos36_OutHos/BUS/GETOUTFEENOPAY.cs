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

namespace OnlineBusHos36_OutHos.BUS
{
    class GETOUTFEENOPAY
    {
        /// <summary>
        /// 合并处方方法
        /// </summary>
        /// <param name="dtPre"></param>
        /// <returns></returns>
        private static Model.GETOUTFEENOPAY_M.PRE MergePre(DataTable dtPre)
        {
            Model.GETOUTFEENOPAY_M.PRE preRet = new Model.GETOUTFEENOPAY_M.PRE();
            //不需要合并的字段
            preRet.OPT_SN = dtPre.Columns.Contains("OPT_SN") ? dtPre.Rows[0]["OPT_SN"].ToString() : "";
            preRet.DEPT_CODE = dtPre.Columns.Contains("DEPT_CODE") ? dtPre.Rows[0]["DEPT_CODE"].ToString() : "";
            preRet.DEPT_NAME = dtPre.Columns.Contains("DEPT_NAME") ? dtPre.Rows[0]["DEPT_NAME"].ToString() : "";
            preRet.DOC_NO = dtPre.Columns.Contains("DOC_NO") ? dtPre.Rows[0]["DOC_NO"].ToString() : "";
            preRet.DOC_NAME = dtPre.Columns.Contains("DOC_NAME") ? dtPre.Rows[0]["DOC_NAME"].ToString() : "";
            preRet.YB_PAY = dtPre.Columns.Contains("YB_PAY") ? dtPre.Rows[0]["YB_PAY"].ToString() : "";
            preRet.YB_NOPAY_REASON = "";
            preRet.YLLB = "";
            preRet.DIS_CODE = dtPre.Columns.Contains("DIS_CODE") ? dtPre.Rows[0]["DIS_CODE"].ToString() : ""; ;
            preRet.DIS_TYPE = dtPre.Columns.Contains("DIS_TYPE") ? dtPre.Rows[0]["DIS_TYPE"].ToString() : "";

            //需要合并的字段
            decimal JEALL_ZF = 0;
            decimal CASH_JE_ZF = 0;
            string PRE_NO_ZF = "";
            string HOS_SN_ZF = "";
            foreach (DataRow drTemp in dtPre.Rows)
            {
                PRE_NO_ZF += drTemp["PRE_NO"].ToString() + ",";
                HOS_SN_ZF += drTemp["HOS_SN"].ToString() + ",";
                JEALL_ZF += Convert.ToDecimal(drTemp["JEALL"].ToString());
                CASH_JE_ZF += Convert.ToDecimal(drTemp["CASH_JE"].ToString());
            }
            preRet.PRE_NO = PRE_NO_ZF.TrimEnd(',');
            preRet.HOS_SN = HOS_SN_ZF.TrimEnd(',');
            preRet.JEALL = JEALL_ZF.ToString("0.00");
            preRet.CASH_JE = CASH_JE_ZF.ToString("0.00");


            return preRet;
        }


        public static string B_GETOUTFEENOPAY(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN _in = JSONSerializer.Deserialize<Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_IN>(json_in);
                Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT _out = new Model.GETOUTFEENOPAY_M.GETOUTFEENOPAY_OUT();
                string YLCARD_TYPE = PubFunc.GETHISYLCARDTYPE(_in.YLCARD_TYPE);

                string HOS_SN = string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim();

                if (!string.IsNullOrEmpty(_in.MB_ID))
                {
                    XmlDocument doc1 = QHXmlMode.GetBaseXml("YJHSTRIGGER", "1");
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode_NOCHANGE(doc1, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "YLCARD_TYPE", YLCARD_TYPE);
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "PAT_NAME", string.IsNullOrEmpty(_in.PAT_NAME) ? "" : _in.PAT_NAME.Trim());
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "HOS_PAT_ID", _in.MB_ID == "2" ? _in.HS_HOS_PAT_NO : "");
                    XMLHelper.X_XmlInsertNode(doc1, "ROOT/BODY", "MB_ID", _in.MB_ID);

                    string inxml1 = doc1.InnerXml;
                    string his_rtnxml1 = "";
                    if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml1, ref his_rtnxml1))
                    {

                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml1;
                        goto EndPoint;
                    }

                    _out.HIS_RTNXML = his_rtnxml1;
                    try
                    {
                        XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml1);
                        DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                        DataTable dtrev = ds.Tables[0];
                        if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }
                        else
                        {
                            HOS_SN = dtrev.Rows[0]["HOS_SN"].ToString();

                            if (_in.MB_ID == "3")
                            {
                                _out.HIS_RTNXML = his_rtnxml1;
                                _out.PRELIST = new List<Model.GETOUTFEENOPAY_M.PRE>();
                                Model.GETOUTFEENOPAY_M.PRE pre = new Model.GETOUTFEENOPAY_M.PRE();
                                pre.HOS_SN = HOS_SN;
                                _out.PRELIST.Add(pre);

                                dataReturn.Code = 0;
                                dataReturn.Msg = "SUCCESS";
                                dataReturn.Param = JSONSerializer.Serialize(_out);
                                goto EndPoint;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        dataReturn.Code = 5;
                        dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                        dataReturn.Param = ex.Message;
                    }
                }

                XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAY", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", YLCARD_TYPE); //string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", "");

                string inxml = doc.InnerXml;
                string his_rtnxml = "";

                //测试用
                //StreamReader stream = new StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();
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


                        if (dtpre.Equals(null) || dtpre.Rows.Count == 0)
                        {
                            dataReturn.Code = 1;
                            dataReturn.Msg = "未获取到有效处方信息";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }


                     

                        //处方预处理，先获取所有挂号流水号
                        DataView dv = dtpre.DefaultView;
                        DataTable dt_of_HOS_SN_REG = dv.ToTable(true, "HOS_SN_REG","DOC_NO");//挂号流水号和“医生编码”20230828 医院his有同一流水号转诊的情况

                        foreach(DataRow dr in dt_of_HOS_SN_REG.Rows)
                        {
                            dv.RowFilter = string.Format(@"HOS_SN_REG='{0}' AND DOC_NO='{1}'", dr["HOS_SN_REG"], dr["DOC_NO"]);
                            DataTable dtPreTemp = dv.ToTable();

                            #region 处方合并
                            if (_in.YLCARD_TYPE != "2")//非医保用户需按挂号流水号进行合并
                            {                    
                                _out.PRELIST.Add(MergePre(dtPreTemp));
                            }
                            else
                            {
                                //医保用户先区分出医保个自费的处方
                                DataView dv_of_dtPreTemp = dtPreTemp.DefaultView;

                                //非医保结算处方
                                dv_of_dtPreTemp.RowFilter = "YB_PAY=0";
                                DataTable dtpre_zf = dv_of_dtPreTemp.ToTable();

                                //医保处方
                                dv_of_dtPreTemp.RowFilter = "YB_PAY<>0";
                                DataTable dtpre_yb = dv_of_dtPreTemp.ToTable();

                                //首先无法医保结算的处方单独合并
                                if (dtpre_zf.Rows.Count > 0)
                                {
                                    _out.PRELIST.Add(MergePre(dtpre_zf));
                                }

                                //医保结算处方需根据YLLB和DOC_NO再次分类
                                if (dtpre_yb.Rows.Count > 0)
                                {
                                    DataView dv_of_dtpre_yb = dtpre_yb.DefaultView;
                                    DataTable dt_of_YLLB = dv_of_dtpre_yb.ToTable(true, "YLLB");

                                    foreach(DataRow dr_of_YLLB in dt_of_YLLB.Rows)
                                    {
                                        dv_of_dtpre_yb.RowFilter = string.Format(@"YLLB='{0}'", dr_of_YLLB["YLLB"]);
                                        DataTable dt_of_yb_by_YLLB = dv_of_dtpre_yb.ToTable();


                                        _out.PRELIST.Add(MergePre(dt_of_yb_by_YLLB));

                                    }
                                }
                            }

                            #endregion

                        }
                        




                        //foreach (DataRow dr in dtpre.Rows)
                        //{
                        //    Model.GETOUTFEENOPAY_M.PRE pre = new Model.GETOUTFEENOPAY_M.PRE();
                        //    pre.OPT_SN = dtpre.Columns.Contains("OPT_SN") ? dr["OPT_SN"].ToString() : "";
                        //    pre.PRE_NO = dtpre.Columns.Contains("PRE_NO") ? dr["PRE_NO"].ToString() : "";
                        //    pre.HOS_SN = dtpre.Columns.Contains("HOS_SN") ? dr["HOS_SN"].ToString() : "";
                        //    pre.DEPT_CODE = dtpre.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                        //    pre.DEPT_NAME = dtpre.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                        //    pre.DOC_NO = dtpre.Columns.Contains("DOC_NO") ? dr["DOC_NO"].ToString() : "";
                        //    pre.DOC_NAME = dtpre.Columns.Contains("DOC_NAME") ? dr["DOC_NAME"].ToString() : "";
                        //    pre.JEALL = dtpre.Columns.Contains("JEALL") ? dr["JEALL"].ToString() : "";
                        //    pre.CASH_JE = dtpre.Columns.Contains("CASH_JE") ? dr["CASH_JE"].ToString() : "";
                        //    pre.YB_PAY = dtpre.Columns.Contains("YB_PAY") ? FormatHelper.GetInt(dr["YB_PAY"]).ToString() : "1";
                        //    pre.YB_NOPAY_REASON = dtpre.Columns.Contains("YB_NOPAY_REASON") ? dr["YB_NOPAY_REASON"].ToString() : pre.YB_PAY == "0" ? "该处方只支持自费缴费" : "";

                        //    pre.YLLB = dtpre.Columns.Contains("YLLB") ? dr["YLLB"].ToString() : "";
                        //    pre.DIS_CODE = dtpre.Columns.Contains("DIS_CODE") ? dr["DIS_CODE"].ToString() : "";
                        //    pre.DIS_TYPE = dtpre.Columns.Contains("DIS_TYPE") ? dr["DIS_TYPE"].ToString() : "";

                        //    _out.PRELIST.Add(pre);
                        //}
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

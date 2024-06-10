using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using static OnlineBusHos319_OutHos.Model.GETOUTFEENOPAYMX_M;
using OnlineBusHos319_OutHos.Model;

namespace OnlineBusHos319_OutHos.BUS
{
    class GETOUTFEENOPAYMX
    {
        public static string B_GETOUTFEENOPAYMX(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_IN _in = JSONSerializer.Deserialize<Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_IN>(json_in);
                Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_OUT _out = new Model.GETOUTFEENOPAYMX_M.GETOUTFEENOPAYMX_OUT();

                string _hospCode = "12321283469108887C";
                string _operCode = "zzj01";
                string _operName = "自助机01";
                string HOS_ID = _in.HOS_ID;
                string HOSPATID = _in.HOSPATID;
                string PRE_NO = _in.PRE_NO;

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
                

                //XmlDocument doc = QHXmlMode.GetBaseXml("GETOUTFEENOPAYMX", "1");
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_SN", string.IsNullOrEmpty(_in.HOS_SN) ? "" : _in.HOS_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "OPT_SN", string.IsNullOrEmpty(_in.OPT_SN) ? "" : _in.OPT_SN.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PRE_NO", string.IsNullOrEmpty(_in.PRE_NO) ? "" : _in.PRE_NO.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOSPATID", string.IsNullOrEmpty(_in.HOSPATID) ? "" : _in.HOSPATID.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", string.IsNullOrEmpty(_in.YLCARD_TYPE) ? "" : _in.YLCARD_TYPE.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", string.IsNullOrEmpty(_in.YLCARD_NO) ? "" : _in.YLCARD_NO.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                //XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MB_ID", string.IsNullOrEmpty(_in.MB_ID) ? "" : _in.MB_ID.Trim());




                string inputjson = JSONSerializer.Serialize(a309Request);
                //System.IO.StreamReader stream = new System.IO.StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();
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


                    try
                    {
                        _out.DACHKTLIST = new List<Model.GETOUTFEENOPAYMX_M.CHKT>();
                        _out.DAMEDLIST = new List<Model.GETOUTFEENOPAYMX_M.MED>();
                        string[] pres = PRE_NO.Split('|');
                        decimal JEALL = 0;
                        List<HISModels.A309.A309Response.Data> datas = baseRsponse.GetInput<List<HISModels.A309.A309Response.Data>>();

                        foreach (string pre in pres)
                        {
                            HISModels.A309.A309Response.Data data = datas.Find(q => q.recipeSummary.recipeNo == pre);

                            JEALL += decimal.Parse(data.recipeSummary.totCost);

                            foreach (HISModels.A309.A309Response.recipeDetails recipeDetails in data.recipeDetails)
                            {
                                if (recipeDetails.drugFlag == "1")//药品
                                {
                                    Model.GETOUTFEENOPAYMX_M.MED med = new Model.GETOUTFEENOPAYMX_M.MED();
                                    med.PRENO = data.recipeSummary.recipeNo;
                                    med.DATIME = data.recipeSummary.operDate;
                                    med.DAID = recipeDetails.recipeSeq;
                                    med.MED_ID = recipeDetails.itemCode;
                                    med.MED_NAME = recipeDetails.itemName;
                                    med.MED_GG = recipeDetails.specs;
                                    med.GROUPID = "0";
                                    med.USAGE = "";
                                    med.AUT_NAME = recipeDetails.doseUnit;
                                    med.CAMT = recipeDetails.doseOnce;
                                    med.AUT_NAMEALL = recipeDetails.priceUnit;
                                    med.CAMTALL = recipeDetails.qty.ToString();
                                    med.TIMES = recipeDetails.frequencyCode;
                                    med.PRICE = recipeDetails.unitPrice.ToString();
                                    med.AMOUNT = recipeDetails.totCost.ToString();
                                    //(decimal.Parse(recipeDetails.qty.ToString()) * decimal.Parse(recipeDetails.unitPrice)).ToString();
                                    med.YB_CODE = "";
                                    med.MINAUT_FLAG = "";
                                    _out.DAMEDLIST.Add(med);
                                }
                                else
                                {
                                    Model.GETOUTFEENOPAYMX_M.CHKT chkt = new Model.GETOUTFEENOPAYMX_M.CHKT();
                                    chkt.DATIME = data.recipeSummary.operDate;
                                    chkt.DAID = recipeDetails.recipeSeq;
                                    chkt.CHKIT_ID = recipeDetails.itemCode;
                                    chkt.CHKIT_NAME = recipeDetails.itemName;
                                    chkt.AUT_NAME = recipeDetails.doseUnit;
                                    chkt.CAMTALL = recipeDetails.qty.ToString();
                                    chkt.PRICE = recipeDetails.unitPrice.ToString();
                                    chkt.AMOUNT = recipeDetails.totCost.ToString();
                                    //(decimal.Parse(recipeDetails.qty.ToString()) * decimal.Parse(recipeDetails.unitPrice)).ToString();
                                    chkt.YB_CODE = "";
                                    chkt.MINAUT_FLAG = "";
                                    _out.DACHKTLIST.Add(chkt);
                                }
                            }

                        }
                        }catch(Exception ex) 
                    {
                        dataReturn.Code = 4;
                        dataReturn.Msg = "解析HIS出参失败:"+ex.Message;
                        dataReturn.Param = ex.Message;
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

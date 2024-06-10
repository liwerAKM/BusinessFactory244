using EBPP.DAL;
using EBPP.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib;
using System.Xml;
using EBPP.Log.Model;
using EBPP.Model;
using EBPP.Log.DAL;

namespace EBPP
{
    public class EBPPHelper
    {
        /// <summary>
        /// 保存电子票据
        /// </summary>
        /// <param name="eBPPSaveReq"></param>
        /// <returns></returns>
        public EBPPBaseResp DealEBPPSaveReq(EBPPSaveReq eBPPSaveReq)
        {
            EBPPBaseResp eBPPSaveResp = new EBPPBaseResp();

            try
            {
                //检查入参是否正确
                string result = "", RetMsg = "";
                if (!CheckEBPPSaveReq(eBPPSaveReq, out result, out RetMsg))
                {
                    eBPPSaveResp.Code = result;
                    eBPPSaveResp.Msg = RetMsg;
                    return eBPPSaveResp;
                }
                long EBPPID = DALBaseFun.GetEBPPID();

                Mebppmain mebppmain = new Mebppmain();//主表
                Mebppquery mebppquery = null;//快速查询
                mebppmain.EBPPID = EBPPID;
                List<Mebppinfo> lMebppinfo = new List<Mebppinfo>();
                List<Mebppmx2> lMebppmx = new List<Mebppmx2>();
                DateTime opentime = ConvertToDateTime(eBPPSaveReq.openTime);
                mebppmain.idcardNo = eBPPSaveReq.idcardNo;
                mebppmain.EBPPID = EBPPID;
                mebppmain.billID = eBPPSaveReq.billID;
                mebppmain.billCode = eBPPSaveReq.billCode;
                mebppmain.cancel = false;
                mebppmain.CreatTime = DateTime.Now;
                mebppmain.EBPPType = eBPPSaveReq.eBPPType;
                mebppmain.openTime = opentime;
                mebppmain.PatName = "";
                mebppmain.OrgName = eBPPSaveReq.orgName;
                mebppmain.PDFUrl = eBPPSaveReq.pDFUrl;
                mebppmain.PayType = eBPPSaveReq.payType;
                mebppmain.unifiedOrgCode = eBPPSaveReq.unifiedOrgCode;
                mebppmain.TotalMoney = eBPPSaveReq.totalFee;



                //eBPPSaveReq.proSettleInfo = new ProSettleInfo();
                //eBPPSaveReq.proReceiptDetails = new List<ProReceiptDetail>();
                //ProReceiptDetail proReceipt = new ProReceiptDetail();
                //eBPPSaveReq.proReceiptDetails.Add(proReceipt);

                //eBPPSaveReq.prescriptionDetails = new List<PrescriptionDetail>();
                //PrescriptionDetail prescription = new PrescriptionDetail();
                //eBPPSaveReq.prescriptionDetails.Add(prescription);

                //string ss = JsonConvert.SerializeObject(eBPPSaveReq);
                if (eBPPSaveReq.patInfo != null)
                {
                    mebppmain.PatName = eBPPSaveReq.patInfo.name;
                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 1;
                    mebppinfo.EBPPID = EBPPID;
                    //mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.patInfo); //JsonConvert.SerializeObject(eBPPSaveReq.patInfo);

                    mebppinfo.JSON = ObjToJsonstr(new PatinfoAD(eBPPSaveReq.patInfo));
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);

                }
                if (eBPPSaveReq.collectInfoGenreal != null)
                {
                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 2;
                    mebppinfo.EBPPID = EBPPID;
                    //   mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.collectInfoGenreal);// JsonConvert.SerializeObject(eBPPSaveReq.collectInfoGenreal);
                    mebppinfo.JSON = ObjToJsonstr(new ChargeCollectGenrealAD(eBPPSaveReq.collectInfoGenreal));
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);
                }
                if (eBPPSaveReq.collectInfoCadre != null)
                {
                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 3;
                    mebppinfo.EBPPID = EBPPID;
                    // mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.collectInfoCadre);// JsonConvert.SerializeObject(eBPPSaveReq.collectInfoCadre);
                    mebppinfo.JSON = ObjToJsonstr(eBPPSaveReq.collectInfoCadre);
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);
                }
                if (eBPPSaveReq.proSettleInfo != null)//结算数据
                {
                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 5;
                    mebppinfo.EBPPID = EBPPID;
                    // mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.proSettleInfo);//  JsonConvert.SerializeObject(eBPPSaveReq.proSettleInfo);
                    mebppinfo.JSON = ObjToJsonstr(new ProSettleInfoAD(eBPPSaveReq.proSettleInfo));
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);
                }

                if (eBPPSaveReq.proReceiptDetail != null)//按类别汇总
                {
                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 6;
                    mebppinfo.EBPPID = EBPPID;
                    List<ProReceiptDetailAD> listad = new List<ProReceiptDetailAD>();
                    foreach (ProReceiptDetail pr in eBPPSaveReq.proReceiptDetail)
                    {
                        listad.Add(new ProReceiptDetailAD(pr));
                    }
                    // mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.proReceiptDetail);//JsonConvert.SerializeObject(eBPPSaveReq.proReceiptDetails);
                    mebppinfo.JSON = ObjToJsonstr(listad);
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);
                }
                if (eBPPSaveReq.prescriptionDetails != null)//处方明细
                {

                    Mebppinfo mebppinfo = new Mebppinfo();
                    mebppinfo.DataType = 7;
                    mebppinfo.EBPPID = EBPPID;
                    List<PrescriptionDetailAD> listad = new List<PrescriptionDetailAD>();
                    foreach (PrescriptionDetail pd in eBPPSaveReq.prescriptionDetails)
                    {
                        listad.Add(new PrescriptionDetailAD(pd));
                    }
                    // mebppinfo.JSON = AESHelper.Encrypt(eBPPSaveReq.prescriptionDetails);//JsonConvert.SerializeObject(eBPPSaveReq.proReceiptDetails);
                    mebppinfo.JSON = ObjToJsonstr(listad);
                    mebppinfo.D1 = DateTime.Now;
                    lMebppinfo.Add(mebppinfo);
                }
                if (eBPPSaveReq.items != null)
                {
                    List<ListInfoAD> listad = new List<ListInfoAD>();
                    foreach (ListInfo li in eBPPSaveReq.items)
                    {
                        listad.Add(new ListInfoAD(li));
                    }
                    Mebppmx2 mebppmx = new Mebppmx2("items", "全部项目", eBPPSaveReq.items.Count.ToString());
                    mebppmx.JSON = ObjToJsonstr(listad);
                    // this.JSON = GZipHelper.CompressToBase64(AESHelper.EncryptDefault(JsonConvert.SerializeObject(listInfo)));
                    mebppmx.EBPPID = EBPPID;
                    lMebppmx.Add(mebppmx);
                }
                //foreach (ListInfo info in eBPPSaveReq.items)
                //{
                //    Mebppmx2 mebppmx = new Mebppmx2(info);
                //    mebppmx.JSON = AESHelper.EncryptDefault(GZipHelper.CompressToBase64(JsonConvert.SerializeObject(info))); //AESHelper.Encrypt(listInfo);
                //    // this.JSON = GZipHelper.CompressToBase64(AESHelper.EncryptDefault(JsonConvert.SerializeObject(listInfo)));
                //    mebppmx.EBPPID = EBPPID;
                //    lMebppmx.Add(mebppmx);
                //}
                if (mebppmain.EBPPType == 4)//电子清单要保存对应文件
                {
                    try
                    {
                        SaveBase64(EBPPConfig.GetFileName(mebppmain), eBPPSaveReq.Base64);
                    }
                    catch (Exception ex)
                    {
                        eBPPSaveResp.Code = "101206";
                        eBPPSaveResp.Msg = "保存文件异常:" + ex.Message;
                        return eBPPSaveResp;
                    }
                }
                mebppquery = new Mebppquery(mebppmain);

                mebppquery.OrgName = eBPPSaveReq.orgName;
                if (!DALBaseFun.SaveEBPP(mebppquery, mebppmain, lMebppinfo, lMebppmx, out RetMsg))
                {
                    eBPPSaveResp.Code = "101205";
                    eBPPSaveResp.Msg = RetMsg;
                }
                else
                {
                    eBPPSaveResp.Code = "0";
                    eBPPSaveResp.Msg = "sucess";
                }
            }
            catch (Exception ex)
            {
                eBPPSaveResp.Code = "101204";
                eBPPSaveResp.Msg = "异常错误:" + ex.Message;
            }
            return eBPPSaveResp;
        }

        /// <summary>
        /// 撤销电子票据(删除)
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public EBPPBaseResp DealEBPPRepealReq(EBPPRepealReq eBPPRepealReq)
        {
            EBPPBaseResp eBPPSaveResp = new EBPPBaseResp();
            string result = "", RetMsg = "";
            if (!CheckEBPPRepalReq(eBPPRepealReq, out result, out RetMsg))
            {
                eBPPSaveResp.Code = result;
                eBPPSaveResp.Msg = RetMsg;
                return eBPPSaveResp;
            }

            DataTable dtmebppmain = new DALebppmain().GetModel(eBPPRepealReq.unifiedOrgCode, eBPPRepealReq.BillID, eBPPRepealReq.BillCode);
            if (dtmebppmain == null)
            {
                eBPPSaveResp.Code = "101198";
                eBPPSaveResp.Msg = "不存在该数据或已经撤销";
                return eBPPSaveResp;
            }
            if (dtmebppmain != null && dtmebppmain.Select("cancel=0").Length == 0)
            {
                eBPPSaveResp.Code = "101198";
                eBPPSaveResp.Msg = "不存在该数据或已经撤销";
                return eBPPSaveResp;
            }
            DataRow[] drs = dtmebppmain.Select("cancel=0");
            foreach (DataRow dr in drs)
            {
                Mebppmain mebppmain = new DALebppmain().DataRowToModel(dr);
                if (mebppmain.idcardNo == eBPPRepealReq.idcardNo)
                {
                    try
                    {
                        if (DALBaseFun.RepealeEBPP(mebppmain.EBPPID, mebppmain.idcardNo, mebppmain.openTime))
                        {
                            if (mebppmain.EBPPType == 4)//电子清单要清除PDF文件
                            {
                                string fileName = EBPPConfig.GetFileName(mebppmain);
                                try
                                {
                                    File.Delete(fileName);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            eBPPSaveResp.Code = "0";
                            eBPPSaveResp.Msg = "sucess";
                        }
                    }
                    catch (Exception ex)
                    {
                        eBPPSaveResp.Code = "101205";
                        eBPPSaveResp.Msg = "报错业务出错,请联系管理人员处理:" + ex.Message;
                    }
                }
                else
                {
                    eBPPSaveResp.Code = "101198";
                    eBPPSaveResp.Msg = "idcardNob不一致";
                }
            }

            return eBPPSaveResp;
        }




        /// <summary>
        /// 查询统计每日上传量返回
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public static STATSApplyCountResp DealStatsApplyCount(STATSApplyCountReq countReq)
        {
            STATSApplyCountResp resp = new STATSApplyCountResp();
            if (string.IsNullOrWhiteSpace(countReq.StatType))
            {
                resp.Code = "101211";
                resp.Msg = "StatType不正确 统计类型 'apply' 按上传时间查询; 'open' 按开票时间查询";
                return resp;
            }
            if ((countReq.StartDate - DateTime.Now).TotalDays > 10)
            {
                resp.Code = "101212";
                resp.Msg = "只能统计10天内数据";
            }
            if (string.IsNullOrWhiteSpace(countReq.unifiedOrgCode) || countReq.unifiedOrgCode.Length != 18)
            {
                resp.Code = "101213";
                resp.Msg = "unifiedOrgCode 医院代码不正确";
                return resp;
            }
            if (countReq.StatType.ToLower() == "apply")
            {
                List<MapplycountQ> list = new DALapplycount().GetListQ(countReq.StartDate, DateTime.MinValue, countReq.unifiedOrgCode);
                resp.list = list;
            }
            else if (countReq.StatType.ToLower() == "open")
            {
                List<MapplycountQ> list = new DALapplycount().GetListQOpen(countReq.StartDate, DateTime.MinValue, countReq.unifiedOrgCode);
                resp.list = list;
            }
            resp.Code = "0";
            resp.Msg = "sucess";
            return resp;
        }

        /// <summary>
        /// 查询错误日志
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public static STATSApplyErrorResp DealStatsApplyError(STATSApplyErrorReq countReq)
        {
            STATSApplyErrorResp resp = new STATSApplyErrorResp();
            if (string.IsNullOrWhiteSpace(countReq.unifiedOrgCode))
            {
                resp.Code = "101213";
                resp.Msg = "unifiedOrgCode 不能为空 ";
                return resp;
            }
            if (Math.Abs( (countReq.EndDate - countReq.StartDate).TotalDays) > 1)
            {
                resp.Code = "101214";
                resp.Msg = "StartDate EndDate 时间差不能超过24小时";
                return resp;
            }
            if ((countReq.StartDate - DateTime.Now ).TotalDays > 10|| (countReq.EndDate - DateTime.Now).TotalDays > 10)
            {
                resp.Code = "101212";
                resp.Msg = "只能统计10天内数据";
                return resp;
            }
            List<MapplyerrorQ> list = new DALapplyerror().GetListQ(countReq.StartDate, countReq.EndDate, countReq.unifiedOrgCode, false );
            resp.list = list;
            resp.Code = "0";
            resp.Msg = "sucess";
            return resp;
        }


        /// <summary>
        /// 快速查询票据 通过身份证 和日期
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public EBPPQueryResp DealEBPPQueryReq(EBPPQueryReq eBPPQueryReq)
        {
            EBPPQueryResp eBPPQueryResp = new EBPPQueryResp();
            if (string.IsNullOrWhiteSpace(eBPPQueryReq.idcardNo) || eBPPQueryReq.idcardNo.Length < 15)
            {
                eBPPQueryResp.Code = "101151";
                eBPPQueryResp.Msg = "idcardNo不正确";
                return eBPPQueryResp;
            }
            List<QuertInfo> list = new DALebppquery().GetQueryList(eBPPQueryReq.idcardNo, eBPPQueryReq.limitData);
            eBPPQueryResp.EBPPList = list;
            eBPPQueryResp.Code = "0";
            eBPPQueryResp.Msg = "sucess";
            return eBPPQueryResp;
        }


        /// <summary>
        ///  查询票据明细 通过身份证 和日期
        /// </summary>
        /// <param name="eBPPQueryReq"></param>
        /// <returns></returns>

        public EBPPGetMxResp DealEBPPGetMxsReq(EBPPGetMxReq eBPPGetMxReq)
        {

            EBPPGetMxResp ebPPGetMxResp = new EBPPGetMxResp();
            if (string.IsNullOrWhiteSpace(eBPPGetMxReq.idcardNo) || eBPPGetMxReq.idcardNo.Length < 15)
            {
                ebPPGetMxResp.Code = "101151";
                ebPPGetMxResp.Msg = "idcardNo不正确";
                return ebPPGetMxResp;
            }
            ebPPGetMxResp.eBPPMxList = new List<EBPPSaveReq>();

            List<QuertInfo> list = new DALebppquery().GetQueryList(eBPPGetMxReq.idcardNo, eBPPGetMxReq.limitData);
            if (list != null && list.Count > 0)
            {
                foreach (QuertInfo info in list)
                {
                    EBPPSaveReq eBPP = GetMxInf(info.EBPPID);
                    ebPPGetMxResp.eBPPMxList.Add(eBPP);
                }
            }

            ebPPGetMxResp.Code = "0";
            ebPPGetMxResp.Msg = "sucess";
            return ebPPGetMxResp;
        }
        /// <summary>
        /// 通过ID获取基本信息及明细分类汇总proReceiptDetail
        /// </summary>
        /// <param name="eBPPGetMxByIDReq"></param>
        /// <returns></returns>
        public EBPPGetInfoByIDResp DealGetPatInfoByID(EBPPGetMxByIDReq eBPPGetMxByIDReq)
        {
            EBPPGetInfoByIDResp ebPPGetMxResp = new EBPPGetInfoByIDResp();
            EBPPSaveReq eBPP = GetPatInf(eBPPGetMxByIDReq.EBPPID);
            ebPPGetMxResp.eBPPMx = eBPP;
            ebPPGetMxResp.Code = "0";
            ebPPGetMxResp.Msg = "sucess";
            return ebPPGetMxResp;
        }
        /// <summary>
        /// 通过ID获取明细
        /// </summary>
        /// <param name="eBPPGetMxByIDReq"></param>
        /// <returns></returns>
        public EBPPGetInfoByIDResp DealGetMxByID(EBPPGetMxByIDReq eBPPGetMxByIDReq)
        {
            EBPPGetInfoByIDResp ebPPGetMxResp = new EBPPGetInfoByIDResp();
            EBPPSaveReq eBPP = GetMxInf(eBPPGetMxByIDReq.EBPPID);
            ebPPGetMxResp.eBPPMx = eBPP;
            ebPPGetMxResp.Code = "0";
            ebPPGetMxResp.Msg = "sucess";
            return ebPPGetMxResp;
        }
        /// <summary>
        /// 获取病人基本信息及明细分类汇总proReceiptDetail
        /// </summary>
        /// <param name="EBPPID"></param>
        /// <returns></returns>
        private EBPPSaveReq GetPatInf(long EBPPID)
        {
            EBPPSaveReq eBPP = new EBPPSaveReq();
            DataSet ds = DALBaseFun.GetEBPPInfo(EBPPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Mebppmain mebppmain = new DALebppmain().DataRowToModel(ds.Tables[0].Rows[0]);
                eBPP.openTime = mebppmain.openTime.ToString("yyyyMMddHHmmss");
                eBPP.billCode = mebppmain.billCode;
                eBPP.billID = mebppmain.billID;
                eBPP.eBPPType = mebppmain.EBPPType;
                eBPP.payType = mebppmain.PayType;
                eBPP.idcardNo = mebppmain.idcardNo;
                eBPP.orgName = mebppmain.OrgName;
                eBPP.unifiedOrgCode = mebppmain.unifiedOrgCode;
                eBPP.pDFUrl = mebppmain.PDFUrl;
                eBPP.totalFee = mebppmain.TotalMoney;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                DALebppinfo dALebppinfo = new DALebppinfo();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    Mebppinfo mebppinfo = dALebppinfo.DataRowToModel(dr);
                    if (mebppinfo.DataType == 1)
                    {
                        eBPP.patInfo = GetObjByJsonStr<PatinfoAD>(mebppinfo.JSON).GetPatinfo();
                    }
                    //else if (mebppinfo.DataType == 2)
                    //{
                    //    eBPP.collectInfoGenreal = AESHelper.Decrypt<ChargeCollectGenreal>(mebppinfo.JSON);
                    //}
                    //else if (mebppinfo.DataType == 3)
                    //{
                    //    eBPP.collectInfoCadre = AESHelper.Decrypt<ChargeCollectCadre>(mebppinfo.JSON);
                    //}
                    //else if (mebppinfo.DataType == 5)
                    //{
                    //    eBPP.proSettleInfo = AESHelper.Decrypt<ProSettleInfo>(mebppinfo.JSON);
                    //}
                    else if (mebppinfo.DataType == 6)
                    {
                        List<ProReceiptDetailAD> listad = GetObjByJsonStr<List<ProReceiptDetailAD>>(mebppinfo.JSON);
                        List<ProReceiptDetail> listpr = new List<ProReceiptDetail>();
                        foreach (ProReceiptDetailAD aD in listad)
                        {
                            listpr.Add(aD.GetProReceiptDetail());
                        }
                        eBPP.proReceiptDetail = listpr;
                    }
                    //else if (mebppinfo.DataType == 7)
                    //{
                    //    eBPP.prescriptionDetails = AESHelper.Decrypt<List<PrescriptionDetail>>(mebppinfo.JSON);
                    //}
                }
            }
            return eBPP;
        }
        public static string ObjToJsonstr<T>(T t)
        {
            string json = JsonConvert.SerializeObject(t);
            string jsong = GZipHelper.CompressToBase64(json);
            string jsone = AESHelper.EncryptDefault(jsong);
            return jsone;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strjson"></param>
        /// <returns></returns>
        public static T GetObjByJsonStr<T>(string strjson)
        {
            //  return  AESHelper.Decrypt<T>(strjson);

            string s = AESHelper.DecryptDefault(strjson);
            string t = GZipHelper.DecompressBase64(s);
            return JsonConvert.DeserializeObject<T>(t);

        }

        /// <summary>
        /// 获取整个明细数据 
        /// </summary>
        /// <param name="EBPPID"></param>
        /// <returns></returns>
        private EBPPSaveReq GetMxInf(long EBPPID)
        {
            EBPPSaveReq eBPP = new EBPPSaveReq();
            DataSet ds = DALBaseFun.GetEBPPMx(EBPPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Mebppmain mebppmain = new DALebppmain().DataRowToModel(ds.Tables[0].Rows[0]);
                eBPP.openTime = mebppmain.openTime.ToString("yyyyMMddHHmmss");
                eBPP.billCode = mebppmain.billCode;
                eBPP.billID = mebppmain.billID;
                eBPP.eBPPType = mebppmain.EBPPType;
                eBPP.payType = mebppmain.PayType;
                eBPP.idcardNo = mebppmain.idcardNo;
                eBPP.orgName = mebppmain.OrgName;
                eBPP.unifiedOrgCode = mebppmain.unifiedOrgCode;
                eBPP.pDFUrl = mebppmain.PDFUrl;
                eBPP.totalFee = mebppmain.TotalMoney;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                DALebppinfo dALebppinfo = new DALebppinfo();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    Mebppinfo mebppinfo = dALebppinfo.DataRowToModel(dr);
                    if (mebppinfo.DataType == 1)
                    {
                        eBPP.patInfo = GetObjByJsonStr<PatinfoAD>(mebppinfo.JSON).GetPatinfo();
                    }
                    else if (mebppinfo.DataType == 2)
                    {
                        eBPP.collectInfoGenreal = GetObjByJsonStr<ChargeCollectGenrealAD>(mebppinfo.JSON).GetChargeCollectGenreal();
                    }
                    else if (mebppinfo.DataType == 3)
                    {
                        eBPP.collectInfoCadre = GetObjByJsonStr<ChargeCollectCadre>(mebppinfo.JSON);
                    }
                    else if (mebppinfo.DataType == 5)
                    {
                        eBPP.proSettleInfo = GetObjByJsonStr<ProSettleInfoAD>(mebppinfo.JSON).GetProSettleInfo();
                    }
                    else if (mebppinfo.DataType == 6)
                    {
                        List<ProReceiptDetailAD> listad = GetObjByJsonStr<List<ProReceiptDetailAD>>(mebppinfo.JSON);
                        List<ProReceiptDetail> listpr = new List<ProReceiptDetail>();
                        foreach (ProReceiptDetailAD aD in listad)
                        {
                            listpr.Add(aD.GetProReceiptDetail());
                        }
                        eBPP.proReceiptDetail = listpr;
                    }
                    else if (mebppinfo.DataType == 7)
                    {
                        List<PrescriptionDetailAD> listad = GetObjByJsonStr<List<PrescriptionDetailAD>>(mebppinfo.JSON);
                        List<PrescriptionDetail> listpd = new List<PrescriptionDetail>();
                        foreach (PrescriptionDetailAD aD in listad)
                        {
                            listpd.Add(aD.GetPrescriptionDetail());
                        }
                        eBPP.prescriptionDetails = listpd;
                    }
                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                eBPP.items = new List<ListInfo>();
                DALebppmx2 dALebppmx2 = new DALebppmx2();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    Mebppmx2 mebppmx = dALebppmx2.DataRowToModel(dr);
                    //eBPP.items.Add(GetObjByJsonStr<ListInfo>(mebppmx.JSON));
                    List<ListInfoAD> listad = GetObjByJsonStr<List<ListInfoAD>>(mebppmx.JSON);

                    foreach (ListInfoAD aD in listad)
                    {
                        eBPP.items.Add(aD.GetListInfo());
                    }

                }
            }
            return eBPP;
        }

        /// <summary>
        /// 检查入参是否正确
        /// </summary>
        /// <param name="Req"></param>
        /// <param name="result"></param>
        /// <param name="RetMsg"></param>
        /// <returns></returns>
        Boolean CheckEBPPSaveReq(EBPPSaveReq Req, out string result, out string RetMsg)
        {
            result = "0"; RetMsg = "";
            if (string.IsNullOrWhiteSpace(Req.unifiedOrgCode))
            {
                result = "101101";
                RetMsg = "unifiedOrgCode 医院统一社会信用代码不能为空";
                return false;
            }
            else if (Req.unifiedOrgCode.Trim().Length != Req.unifiedOrgCode.Length || Req.unifiedOrgCode.Contains(' '))
            {
                result = "101101";
                RetMsg = "unifiedOrgCode 医院统一社会信用代码不能有空格";
                return false;
            }
            else if (Req.unifiedOrgCode.Trim().Length != 18)
            {
                result = "101101";
                RetMsg = "unifiedOrgCode 医院统一社会信用代码不正确";
                return false;
            }
            Req.unifiedOrgCode = Req.unifiedOrgCode.Trim();

            if (string.IsNullOrWhiteSpace(Req.orgName))
            {
                result = "101101";
                RetMsg = "orgName 医院名称不能为空";
                return false;
            }
            else if (Req.orgName.Length > 50)
            {
                result = "101101";
                RetMsg = "orgName 医院名称超过允许最大长度50";
                return false;
            }
            if (Req.totalFee <= 0)
            {
                result = "101102";
                RetMsg = "totalFee总金额需大于0";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.openTime))
            {
                result = "101105";
                RetMsg = "OpenTime生成时间不能为空";
                return false;
            }
            else
            {
                try
                {
                    ConvertToDateTime(Req.openTime);
                }
                catch
                {
                    result = "101106";
                    RetMsg = "OpenTime生成时间格式不正确yyyyMMddHHmmss";
                    return false;
                }
            }
            if (Req.payType <= 0 || Req.payType > 3)
            {
                result = "101107";
                RetMsg = "PayType取数范围是1-3（1、商保 2、医保 3、自费）";
                return false;
            }
            if (Req.eBPPType <= 0 || Req.eBPPType > 4)
            {
                result = "101108";
                RetMsg = "EBPPType取数范围是1-4（1:门诊挂号发票信息；2:门诊收费发票信息;3:住院发票信息；4：住院电子清单）";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.billID))
            {
                result = "101109";
                RetMsg = "billID 医院内部票据号不能为空";
                return false;
            }
            else if (Req.billID.Length > 50)
            {
                result = "101101";
                RetMsg = "billID 医院内部票据号超过允许最大长度20";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.idcardNo))
            {
                result = "101110";
                RetMsg = "idcardNo 身份证不能为空";
                return false;
            }
            Req.idcardNo = Req.idcardNo.Trim();
            if (Req.idcardNo.ToLower() == "newborn")
            {
                if (Req.patInfo == null)
                {
                    result = "101120";
                    RetMsg = "PatInfo病人基本信息不能为null";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.fidIdcardNo))
                {
                    result = "101121";
                    RetMsg = "PatInfo.fidIdcardNo 无身份证新生儿监护人身份证不能为空";
                    return false;
                }

            }
            else if (Req.idcardNo.Length < 15 || Req.idcardNo.Length > 18)
            {
                result = "101110";
                RetMsg = "idcardNo 身份证格式不正确，新生儿无身份证请填'newborn'";
                return false;
            }

            if (Req.eBPPType == 4 && string.IsNullOrWhiteSpace(Req.Base64))
            {
                result = "101110";
                RetMsg = "EBPPType为4电子清单时Base64应为电子清单文件Base64";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Req.pDFUrl))
            {
                Req.pDFUrl = "";
                //result = "101111";
                //RetMsg = "pDFUrl PDF电子发票或清单公网查看地址不能为空";
                //return false;
            }
            else if (!URLCheck.UrlIsValid(Req.pDFUrl))
            {
                result = "101111";
                RetMsg = "pDFUrl 请查看PDF电子发票或清单公网地址是否有效";
                return false;
            }

            if (Req.patInfo == null)
            {
                result = "101120";
                RetMsg = "PatInfo病人基本信息不能为null";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.name))
            {
                result = "101121";
                RetMsg = "PatInfo.name病人姓名不能为空";
                return false;
            }
            else if (Req.patInfo.name.Length > 20)
            {
                result = "101121";
                RetMsg = "PatInfo.name病人姓名超过允许最大长度20";
                return false;
            }
            //if (IsChineseChar(Req.patInfo.name) < 2)
            //{
            //    result = "101121";
            //    RetMsg = "PatInfo.name 病人姓名需为真实姓名";
            //    return false;
            //}
            if (string.IsNullOrWhiteSpace(Req.patInfo.age))
            {
                result = "101122";
                RetMsg = "PatInfo.Age病人年龄不能为空";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.sex))
            {
                result = "101123";
                RetMsg = "PatInfo.sex病人性别不能为空(1男,2女,9未知)";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.deptNameIn))
            {
                result = "101124";
                RetMsg = "PatInfo.deptNameIn就诊(入院)科室不能为空";
                return false;
            }
            if (EBPPConfig.EBPPApplyCloseCheck&& IsChineseChar(Req.patInfo.deptNameIn) < 2)
            {
                result = "101124";
                RetMsg = "PatInfo.deptNameIn就诊(入院)科室应为中文名称";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.deptCodeIn))
            {
                result = "101124";
                RetMsg = "PatInfo.deptCodeIn就诊(入院)科室代码（江苏诊疗科目代码表）不能为空";
                return false;
            }
            if (Req.patInfo.deptNameIn == Req.patInfo.deptCodeIn)
            {
                result = "101124";
                RetMsg = "PatInfo.deptNameIn  PatInfo.deptCodeIn 就诊(入院)科室名称与代码不应相同";
                return false;
            }
            //if (string.IsNullOrWhiteSpace(Req.patInfo.idcardNo))
            //{
            //    result = "101124";
            //    RetMsg = "PatInfo.idcardNo 身份证号码不能为空";
            //    return false;
            //}
            //if (Req.patInfo.idcardNo.Length < 15)
            //{
            //    result = "101124";
            //    RetMsg = "PatInfo.idcardNo 身份证格式不正确";
            //    return false;
            //}
            if (!string.IsNullOrEmpty(Req.patInfo.idcardNo))
            {
                Req.patInfo.idcardNo = Req.patInfo.idcardNo.Trim();
                if (Req.idcardNo != Req.patInfo.idcardNo)
                {
                    RetMsg = "PatInfo.idcardNo 与主节点中 idcardNo 身份证号码不一致(PatInfo.idcardNo已经废弃，可以不传)";
                    return false;
                }
            }
            else
            {
                Req.patInfo.idcardNo = Req.idcardNo;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.hosNo))
            {
                result = "101125";
                RetMsg = "PatInfo.hosNo 住院号（门诊号）不能为空";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.lister))
            {
                result = "101126";
                RetMsg = "PatInfo.lister 制表人不能为空";
                return false;
            }
            if (IsChineseChar(Req.patInfo.lister) < 2)
            {
                result = "101126";
                RetMsg = "PatInfo.lister 制表人需为真实姓名";
                return false;
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.formedTime))
            {
                result = "101127";
                RetMsg = "PatInfo.formedTime制表时间不能为空";
                return false;
            }
            else
            {
                try
                {
                    DateTime.Parse(Req.patInfo.formedTime);
                }
                catch
                {
                    result = "101127";
                    RetMsg = "PatInfo.formedTime制表时间格式不正确";
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(Req.patInfo.personnelNature))
            {
                result = "101133";
                RetMsg = "PatInfo.personnelNature 人员性质/医疗待遇类别/医疗人员类别不能为空，自费病人填写'自费'，医保人员按医保返回填写";
                return false;
            }
            //if (string.IsNullOrWhiteSpace(Req.patInfo.credentialType))
            //{
            //    result = "101127";
            //    RetMsg = "PatInfo.credentialType证件类型不能为空，请参见 8.1 身份证件类别代码表（CV02.01.101）";
            //    return false;
            //}

            if (Req.eBPPType != 1)//不是门诊挂号
            {
                if (string.IsNullOrWhiteSpace(Req.patInfo.doc_no))
                {
                    result = "101131";
                    RetMsg = "PatInfo.doc_no 医生编码不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.doc_name))
                {
                    result = "101131";
                    RetMsg = "PatInfo.doc_name 医生姓名不能为空";
                    return false;
                }
                if (IsChineseChar(Req.patInfo.name) < 2)
                {
                    result = "101131";
                    RetMsg = "PatInfo.doc_name 医生姓名需为真实姓名";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.inDisName))
                {
                    result = "101132";
                    RetMsg = "PatInfo.inDisName 入院/门诊诊断不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.inDisCode))
                {
                    result = "101132";
                    RetMsg = "PatInfo.inDisCode 入院/门诊诊断编码IDC10不能为空";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Req.patInfo.jz_time))
                {
                    result = "101132";
                    RetMsg = "PatInfo.jz_time 就诊时间(住院为入院时间)不能为空";
                    return false;
                }
                else
                {
                    try
                    {
                        ConvertToDateTime(Req.patInfo.jz_time);
                    }
                    catch
                    {
                        result = "101106";
                        RetMsg = "patInfo.jz_time 就诊时间(住院为入院时间)格式不正确yyyyMMddHHmmss";
                        return false;
                    }
                }

            }
            if (Req.eBPPType == 3 || Req.eBPPType == 4)//住院发票或清单
            {
                if (string.IsNullOrWhiteSpace(Req.patInfo.bQNameIn))
                {
                    result = "101133";
                    RetMsg = "PatInfo.bQNameIn 住院数据中,病区不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.deptNameOut))
                {
                    result = "101133";
                    RetMsg = "PatInfo.deptNameOut 住院数据中,出院科室不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.deptCodeOut))
                {
                    result = "101133";
                    RetMsg = "PatInfo.deptCodeOut 住院数据中,出院科室编码（江苏诊疗科目代码表）不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.bQNameOut))
                {
                    result = "101133";
                    RetMsg = "PatInfo.bQNameOut 住院数据中,出院病区不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.inDate))
                {
                    result = "101133";
                    RetMsg = "PatInfo.inDate 住院数据中,入院时间不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.outDate))
                {
                    result = "101133";
                    RetMsg = "PatInfo.outDate 住院数据中,出院时间不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.bed_no))
                {
                    result = "101133";
                    RetMsg = "PatInfo.bed_no 住院数据中,床号不能为空";
                    return false;
                }
                if (Req.patInfo.hLOS <= 0)
                {
                    result = "101133";
                    RetMsg = "PatInfo.hLOS 住院数据中,住院时间不正确";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.outDate))
                {
                    result = "101133";
                    RetMsg = "PatInfo.outDate 住院数据中,出院时间不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.outDisName))
                {
                    result = "101133";
                    RetMsg = "PatInfo.outDisName 住院数据中,出院诊断不能为空";
                    return false;
                }
                if (IsChineseChar(Req.patInfo.outDisName.Trim()) <1)
                {
                    result = "101133";
                    RetMsg = "PatInfo.outDisName 住院数据中,出院诊断不合理";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Req.patInfo.outDisCode))
                {
                    result = "101133";
                    RetMsg = "PatInfo.outDisCode 住院数据中,出院诊断编码IDC10不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.outStatus))
                {
                    result = "101133";
                    RetMsg = "PatInfo.outStatus 住院数据中,出院情况不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.inWay))
                {
                    result = "101133";
                    RetMsg = "PatInfo.inWay 住院数据中,入院方式不能为空";
                    return false;
                }

            }
            if (Req.eBPPType != 4)//非住院清单
            {
                if (Req.proSettleInfo == null)
                {
                    result = "101135";
                    RetMsg = "proSettleInfo 发票数据中 结算节点不能为空";
                    return false;
                }
            }
            //一次真正结算总金额(如果一次医保结算分成多张发票 则此totalFee会被改为多张发票金额之和)
            decimal totalFee = Req.totalFee;
            if (Req.payType == 2)//医保
            {
                if (Req.proSettleInfo.billIDS != null && Req.proSettleInfo.billIDS.Count > 1)//一次医保结算对应多张发票
                {
                    totalFee = 0;
                    foreach (BillIDSItem iDSItem in Req.proSettleInfo.billIDS)
                    {
                        totalFee += iDSItem.totalFee;
                    }
                }
                else
                {
                    Req.proSettleInfo.billIDS = null;
                }
            }
            if (Req.payType == 3)//自费
            {
                Req.proSettleInfo.billIDS = null;
            }

            //医保总额 一般为发票增金额 有部分医院发票中部分数据不上传医保 此费用zf需要填写在医保节点的zf里
            decimal totalFeeyb = totalFee;
         
            if (Req.eBPPType != 4)//非住院清单
            {
                
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.payType))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.payType 发票数据中,病人类别不能为空（1自费;2本地医保;9异地医保;11省医保;12省干保;13市干保）";
                    return false;
                }
                if (Req.payType == 2)
                {
                    if (Req.proSettleInfo.payType == "1")
                    {
                        result = "101136";
                        RetMsg = "proSettleInfo.payType 发票数据中,医保病人类别不能为'1'";
                        return false;
                    }
                }
                else if (Req.payType == 1)
                {
                    if (Req.proSettleInfo.payType != "1")
                    {
                        result = "101136";
                        RetMsg = "proSettleInfo.payType 发票数据中自费病人类别需为'1'";
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.medicalBillingNo))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.medicalBillingNo 发票数据中,单据号不能为空(此单据号为医院打印在发票上的变化)";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.invoiceNO))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.invoiceNO 发票数据中,发票号不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.billNum))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.billNum 发票数据中,HIS结算流水号不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.settleDate))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.settleDate 发票数据中,结算日期不能为空";
                    return false;
                }
                else
                {
                    try
                    {
                        ConvertToDateTime(Req.proSettleInfo.settleDate);
                    }
                    catch
                    {
                        result = "101106";
                        RetMsg = "proSettleInfo.settleDate 发票数据中,结算日期格式不正确（yyyyMMddhhMMss）";
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.updatedBy))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.updatedBy 发票数据中,经办人不能为空";
                    return false;
                }
                if (IsChineseChar(Req.proSettleInfo.updatedBy) < 2)
                {
                    result = "101131";
                    RetMsg = "proSettleInfo.updatedBy 发票数据中,经办人需为真实姓名";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.personType))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.personType 发票数据中,人群类型不能为空";
                    return false;
                }
                if (Req.payType == 2)
                {
                    if (string.IsNullOrWhiteSpace(Req.proSettleInfo.treatment))
                    {
                        result = "101136";
                        RetMsg = "proSettleInfo.treatment 发票数据中,人员待遇不能为空，医保人员按照当前对应医保字典上传(一般情况下读卡时可获取)";
                        return false;
                    }
                }
                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.payType))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.payType 发票数据中,支付类型不能为空（1自费;2本地医保;9异地医保;11省医保;12省干保;13市干保）";
                    return false;
                }

                if ((Req.proSettleInfo.payType == "2" || Req.proSettleInfo.payType == "9"))
                {
                    if (Req.collectInfoGenreal == null)
                    {
                        result = "101136";
                        RetMsg = "collectInfoGenreal 发票数据中，医保病人费用必传";
                        return false;
                    }
                    if (Math.Abs(Req.collectInfoGenreal.TotalMoney + Req.collectInfoGenreal.zf - totalFee) > 0.5m)
                    {
                        if (!string.IsNullOrWhiteSpace(Req.patInfo.unitName) && Req.patInfo.unitName.Contains("建档立卡人"))
                        {//建湖 的(建档立卡人) 医保金额和医院金额不一致 扶贫政策 优惠30%
                        }
                        else
                        {
                            result = "101136";
                            RetMsg = "collectInfoGenreal.TotalMoney 发票数据中，医保数据中总金额与总金额TotalFee不一致";
                            return false;
                        }
                    }
                    totalFeeyb = Req.collectInfoGenreal.TotalMoney;
                }
                else if ((Req.proSettleInfo.payType == "11" || Req.proSettleInfo.payType == "12" || Req.proSettleInfo.payType == "13"))
                {
                    if (Req.collectInfoCadre == null)
                    {
                        result = "101136";
                        RetMsg = "collectInfoCadre 发票数据中，医保病人费用必传";
                        return false;
                    }

                    if (Math.Abs(Req.collectInfoCadre.TotalMoney + Req.collectInfoCadre.zf - totalFee) > 0.5m)
                    {
                        result = "101136";
                        RetMsg = "collectInfoCadre.TotalMoney 发票数据中，医保数据中总金额与总金额TotalFee不一致";
                        return false;
                    }
                    totalFeeyb = Req.collectInfoCadre.TotalMoney;
                    if (Math.Abs(Req.collectInfoCadre.TotalMoney - (Req.collectInfoCadre.CommonNum + Req.collectInfoCadre.HelpPay + Req.collectInfoCadre.SupplyPay
                        + Req.collectInfoCadre.OtrPay + Req.collectInfoCadre.MedAccPay + Req.collectInfoCadre.BankAccPay + Req.collectInfoCadre.CashPay)) > 0.5m)
                    {
                        result = "101136";
                        RetMsg = "collectInfoCadre.TotalMoney 发票数据中，医保数据中总金额与分项数据数据平衡关系不一致";
                        return false;
                    }

                }
            }
            else
            {
                Req.collectInfoCadre = null;
                Req.collectInfoGenreal = null;
                Req.items = null;
                Req.prescriptionDetails = null;
                Req.proReceiptDetail = null;
            }
          

           //是否检查[items].sF,[items].dF 项目明细中,医保病人请上传真实的自付金额sF和自理金额dF
            bool Checksfdf = false;
            if (Req.payType == 2)//医保
            {
                if (string.IsNullOrWhiteSpace(Req.patInfo.medicalCardNo))
                {
                    result = "101134";
                    RetMsg = "PatInfo.medicalCardNo 医保病人,医保号码（社会保障卡号）不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.insuredSite))
                {
                    result = "101134";
                    RetMsg = "PatInfo.insuredSite 医保病人,参保地不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.inNumber))
                {
                    result = "101134";
                    RetMsg = "PatInfo.inNumber 医保病人,本年度住院次数不能为空";
                    return false;
                }
                if ((Req.eBPPType == 3 || Req.eBPPType == 4) && string.IsNullOrWhiteSpace(Req.patInfo.hosNoYb))
                {
                    result = "101134";
                    RetMsg = "PatInfo.hosNoYb 医保病人,医保住院号（如不能获取请填住院号）不能为空";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(Req.patInfo.medicalLnsuranceNo))
                {
                    result = "101134";
                    RetMsg = "PatInfo.medicalLnsuranceNo 医保病人,医保结算流水号不能为空";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Req.proSettleInfo.mEDICALLNSURANCE))
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串，医保必传，(省医保、省干保、市干保传挂号传门诊号psRecCode，结算传医保流水号psBillCode)";
                    return false;
                }
                if (Req.proSettleInfo.mEDICALLNSURANCE.Length < 7)
                {
                    result = "101136";
                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串，医保必传，(省医保、省干保、市干保挂号传门诊号psRecCode，结算传医保流水号psBillCode)";
                    return false;
                }
                else  //if(EBPPConfig.EBPPApplyCloseCheck)//针对医保数据特殊判断
                {
                    try
                    {
                        decimal mybzje = 0;
                        if (Req.proSettleInfo.payType == "2" || Req.proSettleInfo.payType == "9")
                        {
                            Req.collectInfoCadre = null;
                            if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("<ZJGYBXML>") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith("</ZJGYBXML>"))
                            {//XML格式 苏州张家港医保5.5.12. 结算结果(BALANCE)
                                string zje = "";
                                try
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(Req.proSettleInfo.mEDICALLNSURANCE);
                                    XmlNode xmlNodeje = doc.SelectSingleNode("ZJGYBXML/BALANCE");
                                    XmlNode xlist = xmlNodeje.FirstChild;
                                    XmlNodeList xmlNodeList = xlist.ChildNodes;
                                    for (int i = 0; i < xmlNodeList.Count;i++)
                                    {
                                        XmlNode xmlNode = xmlNodeList.Item(i);
                                        if (xmlNode.Attributes.GetNamedItem("name").InnerText.ToLower() == "zfy")
                                            zje = xmlNode.InnerText;
                                        break;
                                    }
                                }
                                catch(Exception ex)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传 苏州张家港医保参见 <ZJGYBXML>预结算数据</ZJGYBXML>，错误"+ex.Message ;
                                    return false;
                                }
                             
                                if (!string.IsNullOrEmpty(zje))
                                {
                                   
                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传 苏州张家港医保参见 <ZJGYBXML>预结算数据</ZJGYBXML>";
                                    return false;
                                }
                                if (!decimal.TryParse(zje, out mybzje))
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传苏州张家港医保参见 <ZJGYBXML>预结算数据</ZJGYBXML>";
                                    return false;
                                }
                                if (Math.Abs(mybzje - totalFeeyb) > 0.5m)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传 苏州张家港医保参见 <ZJGYBXML>预结算数据</ZJGYBXML>";
                                    return false;
                                }
                            }
                           else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("<SZYBXML>") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith("</SZYBXML>"))
                            {//XML格式 苏州医保. 结算结果(BALANCE)
                                string zje = "";
                                try
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(Req.proSettleInfo.mEDICALLNSURANCE);
                                    XmlNode xmlLIST = doc.SelectSingleNode("SZYBXML/BUSINESS/BODY/OUT/KC12/LIST");
                                    XmlNode xmlNodeje = xmlLIST.SelectSingleNode("AKC264");
                                    if (xmlNodeje != null)
                                    {
                                        zje = xmlNodeje.InnerText;
                                    }
                                    else
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传 苏州医保参见  <SZYBXML>预结算数据</SZYBXML>";
                                        return false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传 苏州医保参见 <SZYBXML>预结算数据</SZYBXML>" + ex.Message ;
                                    return false;
                                }

                                if (!string.IsNullOrEmpty(zje))
                                {

                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传 苏州医保参见 <SZYBXML>预结算数据</SZYBXML>";
                                    return false;
                                }
                                if (!decimal.TryParse(zje, out mybzje))
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传苏州医保参见 <SZYBXML>预结算数据</SZYBXML>";
                                    return false;
                                }
                                if (Math.Abs(mybzje - totalFeeyb) > 0.5m)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传 苏州医保参见 <SZYBXML>预结算数据</SZYBXML>";
                                    return false;
                                }
                            }
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("<") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith(">"))
                            {//XML格式
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(Req.proSettleInfo.mEDICALLNSURANCE);
                                XmlNode xmlNodeje = doc.SelectSingleNode("Data/CAA006");
                                string zje = "";
                                if (xmlNodeje != null)
                                {
                                    zje = xmlNodeje.InnerText;
                                }
                                else
                                {
                                    xmlNodeje = doc.SelectSingleNode("Data/ELRESULT/CAA012");
                                    if (xmlNodeje != null)
                                    {
                                        zje = xmlNodeje.InnerText;
                                    }
                                }

                                if (!string.IsNullOrEmpty(zje))//苏州园区
                                {
                                    if (!decimal.TryParse(zje, out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                    return false;
                                }
                            }

                            else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("RRRR") && Req.proSettleInfo.mEDICALLNSURANCE.Length > 200)
                            {
                                //镇江医保 124+业务
                                string zje = Req.proSettleInfo.mEDICALLNSURANCE.Substring(124, 10);
                                if (zje[7] == '.')
                                {
                                    if (!decimal.TryParse(zje, out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else
                                {  //扬州医保 124+业务 

                                     string MsgType = Req.proSettleInfo.mEDICALLNSURANCE.Substring(20,4).ToUpper();//消息类型码   前面124位
                                      // if (MsgType == "SC02"||MsgType == "SC04")//SC02门急诊结算  SC04门诊统筹收费确认请求 
                                   if (MsgType == "FC50")// FC50-异地住院费用结算                                        SC04门诊统筹收费确认请求
                                    {
                                        zje = Req.proSettleInfo.mEDICALLNSURANCE.Substring(154, 14);
                                        if (!decimal.TryParse(zje, out mybzje))
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                            return false;
                                        }
                                    }
                                  else  if (MsgType == "FCB0")// FCB0-异地门诊费用结算              
                                    {
                                        zje = Req.proSettleInfo.mEDICALLNSURANCE.Substring(174, 14);
                                        if (!decimal.TryParse(zje, out mybzje))
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                            return false;
                                        }
                                    }
                                    else  {
                                        string s0_124 = Req.proSettleInfo.mEDICALLNSURANCE.Substring(0, 124);
                                        string s124 = Req.proSettleInfo.mEDICALLNSURANCE.Substring(124);
                                        zje = Req.proSettleInfo.mEDICALLNSURANCE.Substring(140, 10);
                                        if (!decimal.TryParse(zje, out mybzje))
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                            return false;
                                        }
                                    }
                                  
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                            }
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("$$~") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith("~$$"))
                            {//$$~A%%B~1%%2%%3~$$
                                //苏州园区会员社保
                                string mE = Req.proSettleInfo.mEDICALLNSURANCE.Replace("%%", "%");
                                string[] mEDs = mE.Split('%');
                                string[] chMs1 = mEDs[0].Split('~');
                                if (!decimal.TryParse(chMs1[chMs1.Length - 1], out mybzje))
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，吴江、大丰医保$$~~0~~IC~~~8000 %% 1$$";
                                    return false;
                                }
                                if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                    return false;
                                }
                            }
                           
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("$$") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith("$$")&& Req.proSettleInfo.mEDICALLNSURANCE.Contains("%%"))
                            {//苏州吴江 $$0~~0~~IC~~~8000 %% 1000 %% 700 %% 3000 %% 0 %% 100 %% 0 %% 0 %% 0 %% 4900 %% 2000 %% 2000 %% 3000 %% 0 %% 0 %% 0 %% 0 %% 0 %% 0 %% 0 %% 0 %% 1300$$
                             //盐城大丰本地医保
                                string mE = Req.proSettleInfo.mEDICALLNSURANCE.Replace("%%", "%");
                                string[] mEDs = mE.Split('%');
                                string[] chMs1 = mEDs[0].Split('~');
                                if (!decimal.TryParse(chMs1[chMs1.Length - 1], out mybzje))
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，吴江、大丰医保$$~~0~~IC~~~8000 %% 1$$";
                                    return false;
                                }
                                if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                    return false;
                                }
                            }
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.StartsWith("$$") && Req.proSettleInfo.mEDICALLNSURANCE.EndsWith("$$") && Req.proSettleInfo.mEDICALLNSURANCE.Contains("|"))
                            {//$$0~~~~~1~0~20191217 091228~6058772275~10|0|1|0|0|9|0|0|0|1|0|1|9|0|9|0|0|0|0|0|0|0|0|195.7|3222.18|0|0|0|0|0^~04|门诊统筹1段|0|0|.7|0|0^07|自费费用|1|0|0|1|0^17|当年账户|9|9|1|0|0^~2|22379651|0|1|1|1|单价限额:9;^$$
                             //丹阳医保
                                string[] mEDs = Req.proSettleInfo.mEDICALLNSURANCE.Split('~');
                                string[] chMs1;
                                if (mEDs.Length == 12)
                                {
                                    chMs1 = mEDs[9].Split('|');
                                }
                                else if (mEDs.Length == 11)
                                {
                                    chMs1 = mEDs[6].Split('|');
                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，丹阳医保 $$$$";
                                    return false;
                                }
                                if (!decimal.TryParse(chMs1[0], out mybzje))
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，丹阳医保 $$$$";
                                    return false;
                                }
                                if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                    return false;
                                }
                            }
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.Split('^').Length == 5)
                            {
                                string[] mEDs = Req.proSettleInfo.mEDICALLNSURANCE.Split('^');
                                string[] chMs1 = mEDs[1].Split('|');
                                string[] chMs2 = mEDs[2].Split('|');
                                if (chMs2.Length >= 75)
                                {//淮安医保

                                    if (!decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                    Checksfdf = false;
                                }
                                else if (chMs2.Length >= 73)
                                {//连云港

                                    if (!decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                   
                                }
                                else if (chMs2.Length >= 64)
                                {

                                    int retType = 0;//1格式正确 2数值正确
                                    //可能是盐城医保chMs2[11]    //常州医保chMs2[0]
                                    if (decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        retType = 1;
                                        //   result = "101136";
                                        //RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        //return false;
                                    }
                                    else if (decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        retType = 1;
                                        //   result = "101136";
                                        //RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        //return false;
                                    }
                                    if (retType == 0)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (decimal.TryParse(chMs2[11], out mybzje)) //盐城医保chMs2[11]
                                    {
                                        if (!string.IsNullOrWhiteSpace(Req.patInfo.unitName) && Req.patInfo.unitName.Contains("建档立卡人"))
                                        {//建湖 的(建档立卡人) 医保金额和医院金额不一致 扶贫政策 优惠30%
                                            retType = 2;
                                        }
                                      else  if (Math.Abs(mybzje - totalFeeyb) < 0.1m)
                                        {
                                            retType = 2;
                                        }
                                    }
                                    if (retType != 2)//上面验证错误 可能是常州医保chMs2[0]
                                    {
                                        if (decimal.TryParse(chMs2[0], out mybzje))
                                        {
                                            if (Math.Abs(mybzje - totalFeeyb) < 0.1m)
                                            {
                                                retType = 2;
                                            }
                                        }
                                    }
                                    if (retType != 2)//金额不正确
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }

                                }
                                ////常州医保
                                //else if (chMs2.Length >= 63)
                                //{
                                //    if (!decimal.TryParse(chMs2[0], out mybzje))
                                //    {
                                //        result = "101136";
                                //        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                //        return false;
                                //    }
                                //    if (Math.Abs(mybzje - Req.totalFee) > 0.1m)
                                //    {
                                //        result = "101136";
                                //        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                //        return false;
                                //    }

                                //}
                                //常州武进医保
                                else if (chMs2.Length >= 40)
                                {

                                    if (!decimal.TryParse(chMs2[1], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                //无锡医保
                                else if (chMs1.Length >= 27)
                                {

                                    if (!decimal.TryParse(chMs1[0], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else if (chMs2.Length== 7)
                                {//盐城挂号
                                  //  1   帐户支出 2   现金支出   3   连续住院标志   4   一般起付标准  5   起付标志自付  6   统筹支付

                                    if (!decimal.TryParse(chMs2[0], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,盐城医保挂号返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    decimal je2= 0,je4=0,je5=0,je6 = 0;
                                    if (!decimal.TryParse(chMs2[1], out je2))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,盐城医保挂号返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    //if (!decimal.TryParse(chMs2[4], out je5))
                                    //{
                                    //    result = "101136";
                                    //    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,盐城医保挂号返回的原始字符串不正确，医保必传";
                                    //    return false;
                                    //}
                                    if (!decimal.TryParse(chMs2[5], out je6))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,盐城医保挂号返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje+ je2  + je6   - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,盐城医保挂号返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                    return false;
                                }
                            }
                            else if (Req.proSettleInfo.mEDICALLNSURANCE.Split('^').Length == 4)
                            {
                                string[] mEDs = Req.proSettleInfo.mEDICALLNSURANCE.Split('^');
                                string[] chMs1 = mEDs[1].Split('|');
                                string[] chMs2 = mEDs[2].Split('|');

                                //南京医保
                                if (chMs1.Length >= 19)
                                {

                                    if (!decimal.TryParse(chMs1[0], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                //泰州医保
                                else if (chMs2.Length >= 94)
                                {

                                    if (!decimal.TryParse(chMs2[15], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else if (chMs2.Length >= 72)
                                {//宿迁医保

                                    if (!decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.5m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }

                                }
                                else if (chMs2.Length >= 67)
                                {
                                    //南通医保  14 结算日期	VARCHAR2(14)		YYYYMMDDHH24MISS
                                    bool isdata14 = false;
                                    try
                                    {
                                        ConvertToDateTime(chMs2[13]);
                                        isdata14 = true;
                                    }
                                    catch
                                    {
                                        isdata14 = false;
                                    }
                                    if (isdata14) //南通医保 
                                    {
                                        if (!decimal.TryParse(chMs2[15], out mybzje))
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                            return false;
                                        }
                                        if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        //可能是盐城医保

                                        if (!decimal.TryParse(chMs2[11], out mybzje))
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                            return false;
                                        }
                                        if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                        {
                                            result = "101136";
                                            RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                            return false;
                                        }
                                    }
                                }
                                //宜兴医保
                                else if (chMs2.Length >= 60)
                                {

                                    if (!decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFeeyb) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                //徐州医保
                                else if (chMs2.Length >= 57)
                                {

                                    if (!decimal.TryParse(chMs2[11], out mybzje))
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，医保必传";
                                        return false;
                                    }
                                    if (Math.Abs(mybzje - totalFee) > 0.1m)
                                    {
                                        result = "101136";
                                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确(非本人)，医保必传";
                                        return false;
                                    }
                                }
                                else
                                {
                                    result = "101136";
                                    RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确。业务数据不正确。";
                                    return false;
                                }

                            }
                            else
                            {
                                result = "101136";
                                RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，^^^(扬州医保以RRRR开头)其他详见当地医保";
                                return false;
                            }





                        }
                    }
                    catch (Exception ex)
                    {
                        result = "101136";
                        RetMsg = "proSettleInfo.mEDICALLNSURANCE 发票数据中,医保结算返回的原始字符串不正确，^^^(扬州医保以RRRR开头)其他详见当地医保";
                        return false;
                    }

                }

            }
            else
            {
                Req.collectInfoCadre = null;
                Req.collectInfoGenreal = null;
            }
           
            int MedCount = 0;//药品数量 用于检查药品是否开处方
            decimal jeMed = 0;//药品价格 用于检查药品是否开处方
            if (Req.eBPPType != 4)
            {
                if (Req.items == null)
                {
                    result = "101140";
                    RetMsg = "items费用明细不能为null";
                    return false;
                }
                decimal TotalitemMoney = 0;
                decimal sumsF = 0;//自付金额合计
                decimal sumdF = 0;//自理金额合计
                int QzfCount = 0;//项目医保代码写全自费的数量
                foreach (ListInfo liinfo in Req.items)
                {
                    sumsF += liinfo.sF;
                    sumdF += liinfo.dF;

                    TotalitemMoney += liinfo.iJ;
                    if (string.IsNullOrWhiteSpace(liinfo.iT))
                    {
                        result = "101141";
                        RetMsg = "[items].iT 项目明细中,项目类别不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(liinfo.iTC))
                    {
                        result = "101141";
                        RetMsg = "[items].iTC 项目明细中,项目类别代码不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(liinfo.iBM))
                    {
                        result = "101141";
                        RetMsg = "[items].iBM 项目明细中,HIS项目自编码不能为空";
                        return false;
                    }
                    else if (liinfo.iBM.Length > 50)
                    {
                        result = "101141";
                        RetMsg = "[items].iBM 项目明细中,HIS项目自编码超过最大允许长度50";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(liinfo.iN))
                    {
                        result = "101141";
                        RetMsg = "[items].iN 项目明细中,项目名称不能为空";
                        return false;
                    }
                    else if (liinfo.iN.Length > 100)
                    {
                        result = "101141";
                        RetMsg = "[items].iN 项目明细中,项目名称超过最大允许长度100";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(liinfo.iU))
                    {
                        result = "101141";
                        RetMsg = "[items].iU 项目明细中,项目单位不能为空";
                        return false;
                    }
                    if (liinfo.iC == 0)
                    {
                        result = "101141";
                        RetMsg = "[items].iC 项目明细中,项目数量不能为空或0";
                        return false;
                    }
                    if (liinfo.listCat == 0)
                    {
                        result = "101141";
                        RetMsg = "[items].listCat 项目明细中,目录类别不能为0   1药品；2 诊疗项目；3 服务设施；4 医用材料";
                        return false;
                    }
                    else if (liinfo.listCat == 1)
                    {
                        jeMed += liinfo.iJ;
                        MedCount++;
                    }
                    if (liinfo.order != null && liinfo.order.Length > 10)
                    {
                        result = "101141";
                        RetMsg = "[items].order 项目明细中,排序标记(如为空则按照上传顺序)超过允许最大长度10";
                        return false;
                    }

                    if (Req.eBPPType == 2 || Req.eBPPType == 3)
                    {
                        if (string.IsNullOrWhiteSpace(liinfo.recipeNum))
                        {
                            result = "101141";
                            RetMsg = "[items].recipeNum 项目明细中,处方/医嘱号不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.recipeSerialNum))
                        {
                            result = "101141";
                            RetMsg = "[items].recipeSerialNum 项目明细中,处方/医嘱流水号不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.prescriptionitemsCode))
                        {
                            result = "101141";
                            RetMsg = "[items].prescriptionitemsCode 项目明细中,处方/医嘱项目代码不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.prescriptionitemsName))
                        {
                            result = "101141";
                            RetMsg = "[items].prescriptionitemsName 项目明细中,处方/医嘱项目名称不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.recipeDate))
                        {
                            result = "101141";
                            RetMsg = "[items].recipeDate 项目明细中,费用日期不能为空（yyyyMMddhhMMss）";
                            return false;
                        }
                        if (liinfo.listCat == 1 && string.IsNullOrWhiteSpace(liinfo.formulation))
                        {
                            result = "101141";
                            RetMsg = "[items].formulation 项目明细中,药品时剂型不能为空:" + liinfo.iN;
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.deptNum))
                        {
                            result = "101141";
                            RetMsg = "[items].deptNum 项目明细中,科室编码不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.deptName))
                        {
                            result = "101141";
                            RetMsg = "[items].deptName 项目明细中,科室名称不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.doctorCode))
                        {
                            result = "101141";
                            RetMsg = "[items].doctorCode 项目明细中,处方/医嘱医生编码不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.updateBy))
                        {
                            result = "101141";
                            RetMsg = "[items].updateBy 项目明细中,经办人不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.execDeptName))
                        {
                            result = "101141";
                            RetMsg = "[items].execDeptName 项目明细中,执行科室不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.itemId))
                        {
                            result = "101141";
                            RetMsg = "[items].itemId 项目明细中,费用明细ID不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.medicalItemCatName))
                        {
                            result = "101141";
                            RetMsg = "[items].medicalItemCatName 项目明细中,收费项目类别名称不能为空";
                            return false;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.billNum))
                        {
                            result = "101141";
                            RetMsg = "[items].billNum 项目明细中,结算流水号不能为空";
                            return false;
                        }
                        else if (Req.proSettleInfo.billNum != liinfo.billNum)
                        {
                            result = "101141";
                            RetMsg = "[items].billNum 项目明细中,结算流水号与proSettleInfo.billNum不一致";
                            return false;
                        }
                        if (liinfo.itemLevel == 0)
                        {
                            result = "101141";
                            RetMsg = "[items].itemLevel 项目明细中,收费项目等级代码不应为0";
                            return false;
                        } 
                        if (liinfo.sR < 0 || liinfo.sR > 1)
                        {
                            result = "101141";
                            RetMsg = "[items].sR 项目明细中,自付比例范围应在0.0-1.0";
                            return false;
                        }
                        //if (liinfo.itemLevel == 2 && liinfo.sR == 0)
                        //{
                        //    result = "101141";
                        //    RetMsg = "[items].itemLevel，[items].sR 项目明细中,乙类项目自付比例不应为0";
                        //    return false;
                        //}
                        if (liinfo.itemLevel == 3 && liinfo.sR == 0)
                        {
                            result = "101141";
                            RetMsg = "[items].itemLevel，[items].sR 项目明细中itemLevel为3的项目自付比例应为1";
                            return false;
                        }
                        //if (liinfo.sR >0   && liinfo.dF + liinfo .sF == 0)
                        //{
                        //    result = "101141";
                        //    RetMsg = "[items].sR liinfo.dF + liinfo .sF  项目明细中目自付比与自付(自理)金额不对应";
                        //    return false;
                        //}



                    }
                    //  if (Req.payType == 2)
                    {
                        if (string.IsNullOrWhiteSpace(liinfo.centreChargeCode))
                        {
                            result = "101141";
                            RetMsg = "[items].centreChargeCode 项目明细中, 对应医保收费项目编码不能为空（如果没有对应关系请协调HIS保存并提供相关参数，如果该项目在医保中未对应填'全自费''QZF'）";
                            return false;
                        }
                        else if (liinfo.centreChargeCode.ToUpper() == "QZF")
                        {
                            QzfCount++;
                        }
                        if (string.IsNullOrWhiteSpace(liinfo.medicareFeeitemName))
                        {
                            result = "101141";
                            RetMsg = "[items].medicareFeeitemName 项目明细中,对应医保病人 医保收费项目名称不能为空（如果没有对应关系请协调HIS保存并提供相关参数，如果该项目在医保中未对应填'全自费'）";
                            return false;
                        }

                    }
                }

                if (Req.items.Count >3 && Req.items.Count == QzfCount)
                {
                    result = "101141";
                    RetMsg = "[items].centreChargeCode, medicareFeeitemName 项目明细中, 医保收费项目编码或名称不允全对应（'QZF'、'全自费'），请上传实际对应代码及名称。";
                    return false;
                }
                if (Math.Abs(TotalitemMoney - Req.totalFee) > 0.5m)
                {
                    result = "101142";
                    RetMsg = "items费用明细总金额与总金额TotalFee不一致";
                    return false;
                }

                if (Req.payType == 2 && Req.eBPPType != 1)
                {
                    if (Req.collectInfoGenreal != null)
                    {
                        //if (Req.collectInfoGenreal.personCare > 0 || Req.collectInfoGenreal.personPay > 0)
                        //{
                        //    if (sumsF + sumdF == 0)
                        //    {
                        //        result = "101144";
                        //        RetMsg = "[items].sF,[items].dF 项目明细中,医保病人请上传真实的自付金额sF和自理金额dF";
                        //        return false;
                        //    }
                        //}

                        if (Checksfdf&&Req.collectInfoGenreal.personCare > 0)
                        {
                            if (sumsF + sumdF == 0)
                            {
                                result = "101144";
                                RetMsg = "[items].sF,[items].dF 项目明细中,医保病人请上传真实的自付金额sF和自理金额dF";
                                return false;
                            }
                        }
                    }

                }

                if (Req.proReceiptDetail == null)
                {
                    result = "101145";
                    RetMsg = "proReceiptDetail 按类别汇总不能为null";
                    return false;
                }
                if (Req.proReceiptDetail.Count == 0)
                {
                    result = "101145";
                    RetMsg = "proReceiptDetail 按类别汇总不能为空";
                    return false;
                }
                decimal TotalMoneyPD = 0;
                foreach (ProReceiptDetail prd in Req.proReceiptDetail)
                {
                    if (string.IsNullOrWhiteSpace(prd.dictionaryCode))
                    {
                        result = "101146";
                        RetMsg = "[proReceiptDetail].dictionaryCode 按类别汇总中,医院his内部项目类别代码不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(prd.dictionaryName))
                    {
                        result = "101146";
                        RetMsg = "[proReceiptDetail].dictionaryName 按类别汇总中,医院his内部项目类别名称不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(prd.money))
                    {
                        result = "101146";
                        RetMsg = "[proReceiptDetail].money 按类别汇总中,项目金额不能为空";
                        return false;
                    }
                    try
                    {
                        decimal money = decimal.Parse(prd.money);
                        TotalMoneyPD += money;
                    }
                    catch
                    {
                        result = "101146";
                        RetMsg = "[proReceiptDetail].money 按类别汇总中,项目金额应为数值";
                        return false;
                    }
                    if (TotalitemMoney<500&&Math.Abs(TotalitemMoney - Req.totalFee) > 0.1m||TotalitemMoney >500 && Math.Abs(TotalitemMoney - Req.totalFee) > 0.5m)
                    {
                        result = "101147";
                        RetMsg = "proReceiptDetail 按类别汇总中合计金额与总金额TotalFee不一致";
                        return false;
                    }
                    //if (string.IsNullOrWhiteSpace(prd.individualPaymentAmount))
                    //{
                    //    result = "101146";
                    //    RetMsg = "[proReceiptDetails].individualPaymentAmount 按类别汇总中,个人支付金额不能为空,发票上显示就必须传";
                    //    return false;
                    //}
                }
            }

            if (Req.eBPPType == 2 && MedCount > 0)//门诊缴费发票
            {
                if (Req.prescriptionDetails == null || Req.prescriptionDetails.Count == 0)
                {
                    result = "101137";
                    RetMsg = "prescriptionDetails 门诊缴费发票数据中 门诊处方节点不能为空";
                    return false;
                }
                decimal TotalMoneyDetails = 0;
                foreach (PrescriptionDetail pd in Req.prescriptionDetails)
                {
                    if (string.IsNullOrWhiteSpace(pd.recipeNum))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].recipeNum 缴费发票数据中,处方中，门诊处方号不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.recipeSerialNum))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].recipeSerialNum 缴费发票数据中,处方中,门诊处方流水号不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.prescriptionitemsCode))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].prescriptionitemsCode 缴费发票数据中,处方中,门诊处方项目代码不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.prescriptionitemsName))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].prescriptionitemsName 缴费发票数据中,处方中,门诊处方项目名称不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.iU))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].iU 缴费发票数据中,处方中,项目单位不能为空";
                        return false;
                    }
                    if (pd.iC == 0)
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].iC 缴费发票数据中,处方中,项目数量不能为0";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.prescriptionDate))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].prescriptionDate 缴费发票数据中,处方中,处方日期不能为空（yyyyMMddhhMMss）";
                        return false;
                    }
                    else
                    {
                        try
                        {
                            ConvertToDateTime(pd.prescriptionDate);
                        }
                        catch
                        {
                            result = "101106";
                            RetMsg = "[prescriptionDetails].prescriptionDate 缴费发票数据中,处方中,处方日期格式不正确（yyyyMMddhhMMss）";
                            return false;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(pd.doctorCode))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].doctorCode 缴费发票数据中,门诊处方中,处方/医嘱医生编码不能为空";
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(pd.doctorName))
                    {
                        result = "101138";
                        RetMsg = "[prescriptionDetails].doctorName 缴费发票数据中,处方中,处方/医嘱医生姓名不能为空";
                        return false;
                    }
                    TotalMoneyDetails += pd.iJ;
                }
                if (Math.Abs(TotalMoneyDetails - Req.totalFee) > 0.1m && Math.Abs(TotalMoneyDetails - jeMed) > 0.1m)
                {
                    result = "101139";
                    RetMsg = "[prescriptionDetails]处方金额汇总与[items]中药品金额不一致";
                    return false;
                }

            }
            else if (Req.eBPPType != 2)
            {
                Req.prescriptionDetails = null;
            }



            //if (Req.eBPPType != 4)
            //{
            //    var TotalMoney = Req.items.Sum(t => t.iJ);
            //    if (Math.Abs(TotalMoney - Req.totalFee) > 0.1m)
            //    {
            //        result = "101141";
            //        RetMsg = "items费用明细总金额与总金额TotalFee不一致";
            //        return false;
            //    }

            //}

            if (string.IsNullOrWhiteSpace(Req.billCode))
            {
                Req.billCode = "";
            }

            if (Req.idcardNo.ToLower() == "newborn")
            {

                Req.idcardNo = "N" + Req.patInfo.fidIdcardNo;
                Req.patInfo.idcardNo = Req.idcardNo;
            }
            DataTable dtmebppmain = new DALebppmain().GetModel(Req.unifiedOrgCode, Req.billID, Req.billCode);
            if (dtmebppmain != null && dtmebppmain.Select("cancel=0").Length > 0)
            {
                result = "101199";
                RetMsg = "该数据已经上传";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断字符串中汉字数量
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        int IsChineseChar(string str)
        {
            int count = 0;
            foreach (char ch in str)
            {
                if (ch >= 0x4e00 && ch <= 0x9fbb)    //判断字符ch是否为汉字字符
                    count++;
            }
            return count;

        }
        /// <summary>
        /// 检查入参是否正确
        /// </summary>
        /// <param name="Req"></param>
        /// <param name="result"></param>
        /// <param name="RetMsg"></param>
        /// <returns></returns>
        Boolean CheckEBPPRepalReq(EBPPRepealReq Req, out string result, out string RetMsg)
        {
            result = "0"; RetMsg = "";
            if (string.IsNullOrWhiteSpace(Req.unifiedOrgCode))
            {
                result = "101101";
                RetMsg = "unifiedOrgCode不能为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Req.BillID))
            {
                result = "101109";
                RetMsg = "BillID不能为空";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Req.BillCode))
            {
                Req.BillCode = "";
            }
            return true;
        }
        /// <summary>
        /// 将yyyyMMddHHmmss转换成DateTime
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        DateTime ConvertToDateTime(string datetime)
        {
            string OpenTimeT = datetime.Substring(0, 4) + "-" + datetime.Substring(4, 2) + "-" + datetime.Substring(6, 2) + " " + datetime.Substring(8, 2) + ":" + datetime.Substring(10, 2) + ":" + datetime.Substring(12, 2);
            DateTime opentime = DateTime.Parse(OpenTimeT);
            return opentime;
        }

        private bool SaveBase64(string filePath, string base64String)
        {

            using (System.IO.FileStream fs = new FileStream(filePath, FileMode.Create))
            {//传文件的路径即可
                System.IO.BinaryReader br = new BinaryReader(fs);
                byte[] bt = Convert.FromBase64String(base64String);
                BinaryWriter sr = new BinaryWriter(fs);
                sr.Write(bt, 0, bt.Length);
                sr.Close();
                fs.Close();
                return true;
            }
        }

    }
}

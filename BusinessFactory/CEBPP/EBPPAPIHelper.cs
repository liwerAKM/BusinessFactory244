using EBPP.DAL;
using EBPP.Log;
using PasS.Base.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 电子票据函数分解类 add by wanglei 20190909
    /// </summary>
    public class EBPPAPIHelper
    {
        public SLBBusinessInfo DealBuiness(SLBBusinessInfo InBusinessInfo)
        {
            //int nextid = 0;

            //if (DbHelperMySQL.GetBusIdBase("W08.20190920", out nextid))
            //{

            //}
            SLBBusinessInfo OutsLBBusinessInfo = new SLBBusinessInfo(InBusinessInfo);

            if (!string.IsNullOrEmpty(InBusinessInfo.BusData))
            {
                SRootEBPP sRootEBPP = null;
                try
                {
                    sRootEBPP = InBusinessInfo.GetBusData<SRootEBPP>();
                }
                catch (Exception ex)
                {
                    OutsLBBusinessInfo.ReslutCode = 0;
                    OutsLBBusinessInfo.ResultMessage = "Error:入参JSON基础格式不规范。The input JSON basic format is not standardized."+ ex.Message ;
                    return OutsLBBusinessInfo;
                }

                EBPPHelper eBPPHelper = new EBPPHelper();
                if (!string.IsNullOrWhiteSpace(sRootEBPP.apiId))
                {
                    if (sRootEBPP.apiId.ToUpper() == "EBPPAPI")//医院调用
                    {
                        switch (sRootEBPP.subId.ToLower())
                        {
                            case "apply"://保存
                                {
                                    EBPPBaseResp eBPPSaveResp = new EBPPBaseResp();
                                    SRootEBPPSaveReq rootEBPPSaveReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        rootEBPPSaveReq = InBusinessInfo.GetBusData<SRootEBPPSaveReq>();
                                    }
                                    catch (Exception ex)
                                    {
                                        eBPPSaveResp.Code = "101200";
                                        eBPPSaveResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串，请确保列表节点[]正确(请参考示例及说明)。" + ex.Message;
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPSaveResp);
                                        DealApplyError.Save(rootEBPPSaveReq, eBPPSaveResp, InBusinessInfo.BusData);
                                        return OutsLBBusinessInfo;
                                    }

                                    eBPPSaveResp = eBPPHelper.DealEBPPSaveReq(rootEBPPSaveReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPSaveResp);
                                    DealApplyError.Save(rootEBPPSaveReq, eBPPSaveResp, InBusinessInfo.BusData);
                                    break;
                                }
                            case "repeal"://撤销
                                {
                                    EBPPBaseResp eBPPBaseResp = new EBPPBaseResp();
                                    SRootEBPPRepealReq eBPPRepealReq = null;

                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPRepealReq = InBusinessInfo.GetBusData<SRootEBPPRepealReq>();
                                    }
                                    catch
                                    {
                                        eBPPBaseResp.Code = "101200";
                                        eBPPBaseResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                        return OutsLBBusinessInfo;
                                    }

                                    eBPPBaseResp = eBPPHelper.DealEBPPRepealReq(eBPPRepealReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }
                            case "applycount"://上传时间统计
                                {
                                    STATSApplyCountResp eBPPQueryResp = new STATSApplyCountResp();
                                    SRootStatsApplyCountReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootStatsApplyCountReq>();
                                    }
                                    catch
                                    {
                                        eBPPQueryResp.Code = "101200";
                                        eBPPQueryResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    eBPPQueryResp = EBPPHelper.DealStatsApplyCount(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                    break;
                                }
                            case "applyerror"://上传错误查询
                                {
                                    STATSApplyErrorResp eBPPQueryResp = new STATSApplyErrorResp();
                                    SRootStatsApplyeErorReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootStatsApplyeErorReq>();
                                    }
                                    catch
                                    {
                                        eBPPQueryResp.Code = "101200";
                                        eBPPQueryResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    eBPPQueryResp = EBPPHelper.DealStatsApplyError(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                    break;
                                }
                            default:
                                {
                                    EBPPBaseResp eBPPBaseResp = new EBPPBaseResp();
                                    eBPPBaseResp.Code = "101202";
                                    eBPPBaseResp.Msg = "入参subId不正确";
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }

                        }
                    }
                    else if (sRootEBPP.apiId.ToUpper() == "EBPPQUERT")//查询调用
                    {
                        switch (sRootEBPP.subId.ToLower())
                        {
                            case "querybase"://快速查询票据 通过身份证 和日期 获取基本信息
                                {
                                    EBPPQueryResp eBPPQueryResp = new EBPPQueryResp();
                                    SRootEBPPQueryReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootEBPPQueryReq>();
                                    }
                                    catch
                                    {
                                        eBPPQueryResp.Code = "101200";
                                        eBPPQueryResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    eBPPQueryResp = eBPPHelper.DealEBPPQueryReq(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                    break;
                                }
                            case "querymx"://// 通过身份证 和日期 获取明细信息
                                {

                                    EBPPGetMxResp eBPPBaseResp = new EBPPGetMxResp();
                                    SRootEBPPGetMxReq sRootEBPPGetMxReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        sRootEBPPGetMxReq = InBusinessInfo.GetBusData<SRootEBPPGetMxReq>();
                                    }
                                    catch
                                    {
                                        eBPPBaseResp.Code = "101200";
                                        eBPPBaseResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                        return OutsLBBusinessInfo;
                                    }

                                    eBPPBaseResp = eBPPHelper.DealEBPPGetMxsReq(sRootEBPPGetMxReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }
                            case "querymxbyid":////  通过id 获取明细信息
                                {

                                    EBPPGetMxResp eBPPBaseResp = new EBPPGetMxResp();
                                    SRootEBPPGetMxReq sRootEBPPGetMxReq = null;

                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        sRootEBPPGetMxReq = InBusinessInfo.GetBusData<SRootEBPPGetMxReq>();
                                    }
                                    catch
                                    {
                                        eBPPBaseResp.Code = "101200";
                                        eBPPBaseResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                        return OutsLBBusinessInfo;
                                    }

                                    eBPPBaseResp = eBPPHelper.DealEBPPGetMxsReq(sRootEBPPGetMxReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }
                            case "queryinfobyid"://// 通过id 获取基本信息及明细分类汇总proReceiptDetail
                                {

                                    EBPPGetInfoByIDResp eBPPBaseResp = new EBPPGetInfoByIDResp();
                                    SRootEBPPGetMxByIDReq sRootEBPPGetMxByIDReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        sRootEBPPGetMxByIDReq = InBusinessInfo.GetBusData<SRootEBPPGetMxByIDReq>();
                                    }
                                    catch
                                    {
                                        eBPPBaseResp.Code = "101200";
                                        eBPPBaseResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                        return OutsLBBusinessInfo;
                                    }

                                    eBPPBaseResp = eBPPHelper.DealGetPatInfoByID(sRootEBPPGetMxByIDReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }
                           
                            default:
                                {
                                    EBPPBaseResp eBPPBaseResp = new EBPPBaseResp();
                                    eBPPBaseResp.Code = "101202";
                                    eBPPBaseResp.Msg = "入参subId不正确";
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }

                        }
                    }
                    else if (sRootEBPP.apiId.ToUpper() == "STATS")//统计查询
                    {
                        switch (sRootEBPP.subId.ToLower())
                        {
                            case "applycount"://上传时间统计
                                {
                                    STATSApplyCountResp eBPPQueryResp = new STATSApplyCountResp();
                                    SRootStatsApplyCountReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootStatsApplyCountReq>();
                                    }
                                    catch
                                    {
                                        eBPPQueryResp.Code = "101200";
                                        eBPPQueryResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    eBPPQueryResp = StatsHelper.DealStatsApplyCount(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                    break;
                                }
                            case "applyerror"://上传错误查询
                                {
                                    STATSApplyErrorResp eBPPQueryResp = new STATSApplyErrorResp();
                                    SRootStatsApplyeErorReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootStatsApplyeErorReq>();
                                    }
                                    catch
                                    {
                                        eBPPQueryResp.Code = "101200";
                                        eBPPQueryResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    eBPPQueryResp = StatsHelper.DealStatsApplyError(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(eBPPQueryResp);
                                    break;
                                }
                            case "addorg"://添加医院id和名称
                                {
                                    STATSAddOrgResp addOrgResp = new STATSAddOrgResp();
                                    SRootAddOrgReq eBPPQueryReq = null;
                                    string ApplyReqJson = InBusinessInfo.BusData;
                                    try
                                    {
                                        eBPPQueryReq = InBusinessInfo.GetBusData<SRootAddOrgReq>();
                                    }
                                    catch
                                    {
                                        addOrgResp.Code = "101200";
                                        addOrgResp.Msg = "入参格式不正确,请确保数值型节点赋值为数值而不是字符串。";
                                        OutsLBBusinessInfo.ReslutCode = 0;
                                        OutsLBBusinessInfo.ResultMessage = "数据格式不正确!";
                                        OutsLBBusinessInfo.SetBusData(addOrgResp);
                                        return OutsLBBusinessInfo;
                                    }
                                    addOrgResp = StatsHelper.DealSAddOrgReq(eBPPQueryReq.param);
                                    OutsLBBusinessInfo.SetBusData(addOrgResp);
                                    break;
                                }

                            default:
                                {
                                    EBPPBaseResp eBPPBaseResp = new EBPPBaseResp();
                                    eBPPBaseResp.Code = "101202";
                                    eBPPBaseResp.Msg = "入参subId不正确";
                                    OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                                    break;
                                }

                        }
                    }
                    else
                    {
                        EBPPBaseResp eBPPBaseResp = new EBPPBaseResp();
                        eBPPBaseResp.Code = "101201";
                        eBPPBaseResp.Msg = "入参apiId不正确";
                        OutsLBBusinessInfo.SetBusData(eBPPBaseResp);
                    }
                    OutsLBBusinessInfo.ReslutCode = 1;
                    OutsLBBusinessInfo.ResultMessage = "sucess";
                }
                else
                {
                    OutsLBBusinessInfo.ReslutCode = 0;
                    OutsLBBusinessInfo.ResultMessage = "Error:入参JSON基础格式不规范。The input JSON basic format is not standardized.";
                    return OutsLBBusinessInfo;
                }
            }
            else
            {
                OutsLBBusinessInfo.ReslutCode = -1;
                OutsLBBusinessInfo.ResultMessage = "业务数据不能为空!";
            }
            return OutsLBBusinessInfo;
        }

    }
    public class SRootEBPP
    {
        public string apiId { get; set; }
        public string subId { get; set; }
    }

    public class SRootEBPPSaveReq : SRootEBPP
    {
        public EBPPSaveReq param { get; set; }
    }

    public class SRootEBPPRepealReq : SRootEBPP
    {
        public EBPPRepealReq param { get; set; }
    }

    public class SRootEBPPQueryReq : SRootEBPP
    {
        public EBPPQueryReq param { get; set; }
    }

    public class SRootEBPPGetMxReq : SRootEBPP
    {
        public EBPPGetMxReq param { get; set; }
    }

    public class SRootEBPPGetMxByIDReq : SRootEBPP
    {
        public EBPPGetMxByIDReq param { get; set; }
    }

    //统计
    public class SRootStatsApplyCountReq : SRootEBPP
    {
        public STATSApplyCountReq param { get; set; }
    }
    public class SRootStatsApplyeErorReq : SRootEBPP
    {
        public STATSApplyErrorReq param { get; set; }
    }

    public class SRootAddOrgReq : SRootEBPP
    {
        public STATSAddOrgReq param { get; set; }
    }
}
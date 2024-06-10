using EBPP.DAL;
using EBPP.Log.DAL;
using EBPP.Log.Model;
using EBPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 统计处理
    /// </summary>
 public    class StatsHelper
    {
        /// <summary>
        /// 查询统计每日上传量返回
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public static STATSApplyCountResp DealStatsApplyCount(STATSApplyCountReq countReq  )
        {
            STATSApplyCountResp resp = new STATSApplyCountResp();
            if (string.IsNullOrWhiteSpace(countReq.StatType))
            {
                resp.Code = "101211";
                resp.Msg = "StatType不正确 统计类型 'apply' 按上传时间查询; 'open' 按开票时间查询";
                return resp;
            }
            if (countReq.StatType.ToLower() == "apply")
            {
                List<MapplycountQ> list = new DALapplycount().GetListQ(countReq.StartDate, countReq.EndDate, countReq.unifiedOrgCode);
                resp.list = list;
            }
            else if (countReq.StatType.ToLower() == "open")
            {
                List<MapplycountQ> list = new DALapplycount().GetListQOpen(countReq.StartDate, countReq.EndDate, countReq.unifiedOrgCode);
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
                resp.Code = "101212";
                resp.Msg = "unifiedOrgCode 不能为空 ";
                return resp;
            }
            if(Math.Abs((countReq.EndDate - countReq.StartDate).TotalDays) > 1)
            {
                resp.Code = "101212";
                resp.Msg = "StartDate EndDate 时间差不能超过24小时 ";
                return resp;
            }
            List<MapplyerrorQ> list = new DALapplyerror().GetListQ(countReq.StartDate, countReq.EndDate, countReq.unifiedOrgCode, countReq.ContainsInData==1);
            resp.list = list;
            resp.Code = "0";
            resp.Msg = "sucess";
            return resp;
        }
        /// <summary>
        /// 添加医院
        /// </summary>
        /// <param name="eBPPRepealReq"></param>
        /// <returns></returns>
        public static STATSAddOrgResp DealSAddOrgReq(STATSAddOrgReq countReq)
        {
            STATSAddOrgResp resp = new STATSAddOrgResp();
          
            if (string.IsNullOrWhiteSpace(countReq.AddKey))
            {
                resp.Code = "101214";
                resp.Msg = "AddKey 不能为空 ";
                return resp;
            }
            else if (countReq.AddKey != "86915958")
            {
                resp.Code = "101215";
                resp.Msg = "AddKey 不正确 ";
                return resp;
            }
            bool canadd = true;//是否可以添加到列表里
            if (string.IsNullOrWhiteSpace(countReq.unifiedOrgCode))
            {
                resp.Code = "101212";
                resp.Msg = "unifiedOrgCode 不能为空 ";
                canadd = false;
            }
            if (string.IsNullOrWhiteSpace(countReq.OrgName))
            {
                resp.Code = "101213";
                resp.Msg = "OrgName 不能为空 ";
                canadd = false;
            }
            if (canadd)
            {
                MOrginfo mOrginfo = new MOrginfo();
                mOrginfo.unifiedOrgCode = countReq.unifiedOrgCode;
                mOrginfo.OrgName = countReq.OrgName;
                try
                {
                    bool OK = new DALorginfo().Add(mOrginfo);
                    if (OK)
                    {
                        resp.Code = "0";
                        resp.Msg = "sucess";
                    }
                    else
                    {
                        resp.Code = "101216";
                        resp.Msg = "添加失败";
                    }
                }
                catch (Exception ex)
                {
                    resp.Code = "101217";
                    resp.Msg = ex.Message;
                }
            }
            if (countReq.RetList == 1)//返回医院列表
            {
                resp.list = new DALorginfo().GetModels();
            }
            return resp;
        }
    }
}

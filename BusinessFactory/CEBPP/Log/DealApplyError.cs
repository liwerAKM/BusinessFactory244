using EBPP.Log.DAL;
using EBPP.Log.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP.Log
{
   public  class DealApplyError
    {
        public static void Save(   SRootEBPPSaveReq rootEBPPSaveReq, EBPPBaseResp eBPPSaveResp,string inData)
        {
            try
            {
                if (eBPPSaveResp.Code != "0")
                {
                    new DALapplyerror().Add(GetMapplyerror(rootEBPPSaveReq, eBPPSaveResp, inData));
                }
                else if(EBPPConfig.EBPPApplySucessLog )
                {
                    new DALapplysucess().Add(GetMapplyerror(rootEBPPSaveReq, eBPPSaveResp, inData));
                }
            }
            catch
            {
            }
        }

        public static Mapplyerror GetMapplyerror(SRootEBPPSaveReq rootEBPPSaveReq, EBPPBaseResp eBPPSaveResp, string inData)
        {
            Mapplyerror mapplyerror = new Mapplyerror();
            mapplyerror.applyTime = DateTime.Now;
            mapplyerror.CreatTime = DateTime.Now;
            if (rootEBPPSaveReq != null)
            {
                if (rootEBPPSaveReq.param != null)
                {
                    mapplyerror.billCode = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.billCode) ? "" : rootEBPPSaveReq.param.billCode;
                    mapplyerror.billID = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.billID) ? "" : rootEBPPSaveReq.param.billID;
                    mapplyerror.CreatTime = DateTime.Now; ;// string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.openTime) ? "" : rootEBPPSaveReq.param.billCode;
                    mapplyerror.EBPPType = rootEBPPSaveReq.param.eBPPType;
                    mapplyerror.idcardNo = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.idcardNo) ? "" : rootEBPPSaveReq.param.idcardNo;
                    mapplyerror.OrgName = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.orgName) ? "" : rootEBPPSaveReq.param.orgName;
                    mapplyerror.PayType = rootEBPPSaveReq.param.payType;
                    mapplyerror.PDFUrl = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.pDFUrl) ? "" : rootEBPPSaveReq.param.pDFUrl;
                    mapplyerror.TotalMoney = rootEBPPSaveReq.param.totalFee;
                    mapplyerror.unifiedOrgCode = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.unifiedOrgCode) ? "" : rootEBPPSaveReq.param.unifiedOrgCode;
                    if (rootEBPPSaveReq.param.patInfo != null)
                    {
                        mapplyerror.PatName = string.IsNullOrWhiteSpace(rootEBPPSaveReq.param.patInfo.name) ? "" : rootEBPPSaveReq.param.patInfo.name;
                    }
                }
            }
            mapplyerror.InData = inData;
            mapplyerror.OutData = JsonConvert.SerializeObject(eBPPSaveResp);
            return mapplyerror;
        }
    }
}

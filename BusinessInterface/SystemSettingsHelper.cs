using PasS.Base.Lib;
using PasS.Base.Lib.DAL;
using PasS.Base.Lib.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInterface
{
    /// <summary>
    /// 系统设置内置类
    /// </summary>
   public  class SystemSettingsHelperIn
    {
       
   
        public static SLBBusinessInfo DealSocketSetting(SLBBusinessInfo sLBBusinessInfo, IPEndPoint endPoint, ref ConcurrentDictionary<string, SAPIUserInfoCKey> cdAPIUser, SpringHttpData springHttpData)
        {
            SLBBusinessInfo sLBBusOut = new SLBBusinessInfo(sLBBusinessInfo);
            sLBBusOut.SubBusID = sLBBusinessInfo.SubBusID;

            try
            {
                if (string.Compare(sLBBusinessInfo.SubBusID, "GetSLatestKey", true) == 0) //获取平台最新RSA公钥
                {
                    SGetSLatestKeyReq sGetSLatest = sLBBusinessInfo.GetBusData<SGetSLatestKeyReq>();
                     
                    if (cdAPIUser.ContainsKey(sGetSLatest.SAPIU_ID))
                    {
                        string SpringRSAPubKey = "";
                        if (!string.IsNullOrEmpty(cdAPIUser[sGetSLatest.SAPIU_ID].SpringRSAPubKey))
                            SpringRSAPubKey = cdAPIUser[sGetSLatest.SAPIU_ID].SpringRSAPubKey;
                        else
                            SpringRSAPubKey = SignEncryptHelper.RSAPublicKey;
                        SGetSLatestKeyResp sGetS = new SGetSLatestKeyResp();
                        sGetS.RSAPublicKey = SpringRSAPubKey;
                        sGetS.AESKey = cdAPIUser[sGetSLatest.SAPIU_ID].AESKey;
                        sGetS.RSA2PublicKey = "";
                        sLBBusOut.BusData = JsonConvert.SerializeObject(sGetS);
                        sLBBusOut.ReslutCode = 1;
                        sLBBusOut.ResultMessage = "sucess";
                    }
                    else
                    {
                        sLBBusOut.ReslutCode = 0;
                        sLBBusOut.ResultMessage = "不存在对应WebSocketAPI用户";
                    }
                }
                else if (string.Compare(sLBBusinessInfo.SubBusID, "SetUserRSAPub", true) == 0) //设置API用户RSA公钥
                {
                    SSetUserRSAPubResp sSetUserRSAPub = sLBBusinessInfo.GetBusData<SSetUserRSAPubResp>();

                    APIUser aPIUser = new APIUser();
                    apiuser api_userOld = aPIUser.GetModel(springHttpData.user_id);
                    if (api_userOld != null)
                    {
                        if (sSetUserRSAPub.RSAPublicKey == api_userOld.RSAPubKey)
                        {// 没有变化
                            sLBBusOut.ReslutCode = 0;
                            sLBBusOut.ResultMessage = "未检测到变化，无需保存!";
                            return sLBBusOut;
                        }
                        try
                        {
                            if (aPIUser.UpdateUserRSA(springHttpData.user_id, sSetUserRSAPub.RSAPublicKey))
                            {
                                sLBBusOut.BusData = $"更新成功，新公钥将于{BusServiceAdapter.UpdateAPIUserInfoTime } 生效。如需要即时更新，不要通过此接口更新，请联系启航项目经理通过系统配置更新。";
                                sLBBusOut.ReslutCode = 1;
                                sLBBusOut.ResultMessage = "sucess";
                            }
                            else
                            {
                                sLBBusOut.ReslutCode = -1;
                                sLBBusOut.ResultMessage = "更新失败";
                            }
                        }
                        catch (Exception ex)
                        {
                            sLBBusOut.ReslutCode = -1;
                            sLBBusOut.ResultMessage = "更新失败:" + ex.Message;
                        }
                    }
                    else
                    {
                        sLBBusOut.ReslutCode = -1;
                        sLBBusOut.ResultMessage = "不存在对应WebSocketAPI用户";
                    }

                }
            }
            catch (Exception ex)
            {
                sLBBusOut.ReslutCode = -1;
                sLBBusOut.ResultMessage = "处理失败:"+ex.Message ;
            }
            return sLBBusOut;
        }

        public class SGetSLatestKeyResp
        {
          
            /// <summary>
            /// 平台AES秘钥
            /// </summary>
            public string AESKey;
            /// <summary>
            /// 平台RSA公钥
            /// </summary>
            public string RSAPublicKey;
            /// <summary>
            /// 平台RSA2公钥
            /// </summary>
            public string RSA2PublicKey;
        }
        public class SGetSLatestKeyReq
        {

            /// <summary>
            /// WebSocket 对应APIUSerID
            /// </summary>
            public string SAPIU_ID;
           
        }

        public class SSetUserRSAPubResp
        {
            /// <summary>
            /// 用户RSA公钥
            /// </summary>
            public string RSAPublicKey;
        }

    }
}

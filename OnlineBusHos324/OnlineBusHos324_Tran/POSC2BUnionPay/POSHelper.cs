using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace POSC2BUnionPay
{
    public class POSC2BunionPayHelper
    {
        public static bool CallService<T>(string TYPE, object inBody,ref T outBody, ref string errmsg)
        {
            try
            {
                Models.QHModel.Root root = new Models.QHModel.Root();
                root.ROOT = new Models.QHModel.ROOT();
                root.ROOT.HEADER = new Models.QHModel.HEADER();
                root.ROOT.HEADER.TYPE = TYPE;

                root.ROOT.BODY = inBody;
                string indata = JsonConvert.SerializeObject(root);
                string outdata = "";
                outdata = EntranceV1NETPAY.DoBusiness(indata);
                /*
                bool flag = OnlineBusHos968_Tran.GlobalVar.CallOtherBus(indata, "PBusPOSC2BunionPay", "V1NETPAY", out outdata);
                if (!flag) 
                {
                    errmsg = outdata;
                    return false;
                }*/
                Models.QHModel.Root response = JsonConvert.DeserializeObject<Models.QHModel.Root>(outdata);
                outBody = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response.ROOT.BODY));

                return true;
            }
            catch(Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }


        }
    }
}

using PasS.Base.Lib;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Xml;

namespace Soft.Core
{
    public class slbCall
    {
        public static string GetPostData(string indata, string BusId, string SubBusID)
        {
            string url = DbHelper.SysInfoGet("CenterSyncDataUrl") + "/SLB/MySpringAES/";
            string user_id = "JSQH";

            string pherText, md5;

            //indata= GZipHelper.CompressToBase64(indata);
            CommonFunction.GetSecretHOS(user_id, indata, out pherText, out md5);

            Root root = new Root();
            root.Param = pherText;
            root.user_id = user_id;
            root.sign = md5;
            root.SubBusID = SubBusID;
            root.Gzip = 0;
            root.ParamType = 1;

            string out_data = "";
            var http = new HttpClient(url + BusId);
            int status = http.SendJson(Soft.Core.JSONSerializer.Serialize(root), Encoding.UTF8, out out_data);
            if (status == 200)
            {
                ReturnClass rerurn_class = Soft.Core.JSONSerializer.Deserialize<ReturnClass>(out_data);
                if (rerurn_class.ReslutCode == 1)
                {
                    out_data = AESExample.Decrypt(rerurn_class.Param, CommonFunction.GetSecretKEY(user_id));
                }
                else
                {
                    out_data = rerurn_class.ResultMessage;
                }
                //out_data = AESExample.Decrypt(rerurn_class.Param, CommonFunction.GetSecretKEY(user_id));
                return out_data;
            }
            else
            {
                return out_data;
            }
        }
    }

    public class ReturnClass
    {
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReslutCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Param { get; set; }

    }



    class Root
    {

        /// <summary>
        /// 入参
        /// </summary>
        public string Param;
        /// <summary>
        /// user_ID
        /// </summary>
        public string user_id;
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;


        /// <summary>
        /// 1 入参为业务数据AES加密 2为RSA加密,3为 为RSA2加密 ;12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
        /// </summary>
        public int ParamType;

        public string BusID;

        /// <summary>
        /// 子业务ID （根据实际业务定义，可空） SUB BUSINESS ID 此ID只参与具体的业务，与Spring无直接关系
        /// </summary>
        public string SubBusID { get; set; }

        /// <summary>
        /// 请求唯一ID 不重复  非必填
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 客户端标记 标记用于灰度发布 非必填
        /// </summary>
        public string CTag { get; set; }

        /// <summary>
        /// 客户端标记 是否启用压缩 0 未压缩 1 压缩 
        /// </summary>
        public int Gzip { get; set; }

    }
}

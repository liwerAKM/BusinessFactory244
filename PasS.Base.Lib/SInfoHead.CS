using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasS.Base.Lib
{

    /// <summary>
    ///通知更新数据的Head部分
    /// </summary>
    public class NotifyUpdateHead
    {
        string mark = "^";
        public NotifyUpdateHead(NotifyUpdateInfo notifyUpdateInfo)
        {
            BusID = notifyUpdateInfo.BusID;
            this.DataID = notifyUpdateInfo.DataID;
            this.UpdateTime = notifyUpdateInfo.UpdateTime;
            this.Scope = notifyUpdateInfo.Scope;
            this.Route = notifyUpdateInfo.Route;
        }

        public NotifyUpdateHead(NotifyUpdateHead sLBInfoHead)
        {
            BusID = sLBInfoHead.BusID;
            this.DataID = sLBInfoHead.DataID;
            this.UpdateTime = sLBInfoHead.UpdateTime;
            this.Scope = sLBInfoHead.Scope;
            this.Route = sLBInfoHead.Route;
            this.BusServerID = sLBInfoHead.BusServerID;
            this.SLBID = sLBInfoHead.SLBID;
            this.TID = sLBInfoHead.TID;

        }
        public NotifyUpdateHead(string strHead)
        {
            if (string.IsNullOrEmpty(strHead) || strHead.Length < 6)
            {
                return;
            }
            string[] plit = strHead.Split('^');
            BusID = int.Parse(plit[0]);
            DataID = plit[1];
            UpdateTime = DateTime.Parse(plit[2]);
            Scope = int.Parse(plit[3]);
            Route = plit[4];

            BusServerID = plit[5];
            SLBID = plit[6];
            TID = plit[7];



        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(BusID);

            builder.Append(mark + DataID);
            builder.Append(mark + UpdateTime.ToString());
            builder.Append(mark + Scope.ToString());
            builder.Append(mark + Route);
            builder.Append(mark + BusServerID);
            builder.Append(mark + SLBID);
            builder.Append(mark + TID);

            return builder.ToString();
        }
        /// <summary>
        ///业务ID 小于50 的是广播通知,大于50 的是客户端单独请求 需要带上客户端标识;
        /// 1 单表<see cref="NotifyUpdateInfo"/>;
        /// 2 多表<see cref="NotifyUpdateInfo"/>;
        /// 51 获取所服务器端定义的所有本地表名称及内容
        /// 52 获取所有或者单个本地表的结构；
        /// 53 获取单个表的全部数据
        /// 54 获取单个表指定时间后的更新数据
        /// </summary>
        public int BusID { get; set; }
        /// <summary>
        /// 数据(表)对应ID或名称
        /// </summary>
        public string DataID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///更新范围类型
        ///1 全部更新
        ///2 只业务服务器更新
        ///3 业务服务器和指定依赖业务更新
        /// </summary>
        public int Scope { get; set; }
        /// <summary>
        /// <see cref="Scope"/>为3 时的指定依赖路由标记
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 发起通知业务的BusServerID
        /// </summary>
        public string BusServerID { get; set; }


        /// <summary>
        /// 指定通知其他BusServer的SLBID
        /// </summary>
        public string SLBID { get; set; }


    }

    /// <summary>
    ///负载均衡和客户端交互的Head部分
    /// </summary>
    public class SLBInfoHead
    {
        string mark = "^";


        public SLBInfoHead()
        {
            BusVersion = "1.0";
        }
        public SLBInfoHead(SLBBusinessInfo sLBBusinessInfo)
        {
            this.BusID = sLBBusinessInfo.BusID;
            // this.TID = sLBBusinessInfo.TID;
            BusVersion = "1.0";
            this.Zip = 0;
        }

        public SLBInfoHead(SLBInfoHeadBusS sLBInfoHead)
        {
            this.Token = sLBInfoHead.Token;
            this.BusID = sLBInfoHead.BusID;
            this.TID = sLBInfoHead.TID;
            this.Zip = sLBInfoHead.Zip;

            this.BusServerID = sLBInfoHead.BusServerID;

            this.StandbyClientID = sLBInfoHead.StandbyClientID;
            this.StandbySLBID = sLBInfoHead.StandbySLBID;
            this.StandbySLBIP_Port = sLBInfoHead.StandbySLBIP_Port;

            this.SignType = sLBInfoHead.SignType;
            this.EncryptType = sLBInfoHead.EncryptType;
            this.SEID = sLBInfoHead.SEID;
            this.BusVersion = sLBInfoHead.BusVersion;
        }
        public SLBInfoHead(string strHead)
        {
            if (string.IsNullOrEmpty(strHead) || strHead.Length < 6)
            {
                return;
            }
            string[] plit = strHead.Split('^');
            Token = plit[0];
            BusID = plit[1];
            TID = plit[2];
            Zip = int.Parse(plit[3]);
            BusServerID = plit.Length > 4 ? plit[4] : "";

            StandbyClientID = plit.Length > 5 ? plit[5] : "";
            StandbySLBID = plit.Length > 6 ? plit[6] : "";
            StandbySLBIP_Port = plit.Length > 7 ? plit[7] : "";

            SignType = plit.Length > 8 ? int.Parse(plit[8]) : 0;

            EncryptType = plit.Length > 9 ? int.Parse(plit[9]) : 0;

            SEID = plit.Length > 10 ? plit[10] : "";
            BusVersion = plit.Length > 11 ? plit[11] : "1.0";
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Token);
            builder.Append(mark + BusID);
            builder.Append(mark + TID);
            builder.Append(mark + Zip.ToString());
            builder.Append(mark + BusServerID);

            builder.Append(mark + StandbyClientID);
            builder.Append(mark + StandbySLBID);
            builder.Append(mark + StandbySLBIP_Port);

            builder.Append(mark + SignType.ToString());
            builder.Append(mark + EncryptType.ToString());
            builder.Append(mark + SEID);
            builder.Append(mark + BusVersion);
            return builder.ToString();
        }
        /// <summary>
        /// BusName任务ID
        /// </summary>
        public string BusID { get; set; }
        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// BusServerID
        /// </summary>
        public string BusServerID { get; set; }


        /// <summary>
        /// 备用ClientID
        /// IP:Port
        /// </summary>
        public string StandbyClientID { get; set; }

        /// <summary>
        ///备用负载均衡服务器ID
        /// </summary>
        public string StandbySLBID { get; set; }

        /// <summary>
        /// 备用负载均衡服务器IP_Port
        /// </summary>
        public string StandbySLBIP_Port { get; set; }

        /// <summary>
        /// 签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID（即后台的APIU_ID）
        /// </summary>
        public string SEID { get; set; }

        /// <summary>
        ///  Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 业务版本 默认1.0
        /// </summary>
        public string BusVersion { get; set; }
    }

    /// <summary>
    ///文件交互的Head部分
    /// </summary>
    public class FileInfoHead
    {
        string mark = "^";
        public FileInfoHead()
        {
        }
        public FileInfoHead(string strHead)
        {
            if (string.IsNullOrEmpty(strHead) || strHead.Length < 6)
            {
                return;
            }
            string[] plit = strHead.Split('^');

            Token = plit[0];
            BusID = plit[1];
            TID = plit[2];
            Zip = int.Parse(plit[3]);
            BusServerID = plit.Length > 4 ? plit[4] : "";

            StandbyClientID = plit.Length > 5 ? plit[5] : "";
            StandbySLBID = plit.Length > 6 ? plit[6] : "";
            StandbySLBIP_Port = plit.Length > 7 ? plit[7] : "";

            SignType = plit.Length > 8 ? int.Parse(plit[8]) : 0;
            EncryptType = plit.Length > 9 ? int.Parse(plit[9]) : 0;
            SEID = plit.Length > 10 ? plit[10] : "";

            LastWriteTime = plit.Length > 11 ? DateTime.Parse(plit[11]) : DateTime.Now;
            FileName = plit.Length > 12 ? plit[12] : DateTime.Now.ToString() + ".File";
            FilePath = plit.Length > 13 ? plit[13] : "\\";

        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Token);
            builder.Append(mark + BusID);
            builder.Append(mark + TID);
            builder.Append(mark + Zip.ToString());
            builder.Append(mark + BusServerID);

            builder.Append(mark + StandbyClientID);
            builder.Append(mark + StandbySLBID);
            builder.Append(mark + StandbySLBIP_Port);

            builder.Append(mark + SignType.ToString());
            builder.Append(mark + EncryptType.ToString());
            builder.Append(mark + SEID);

            builder.Append(mark + LastWriteTime.ToString());
            builder.Append(mark + FileName);
            builder.Append(mark + FilePath);
            return builder.ToString();
        }
        /// <summary>
        /// BusServerID
        /// </summary>
        public string BusServerID { get; set; }

        /// <summary>
        ///  Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// BusName任务ID
        /// </summary>
        public string BusID { get; set; }
        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        ///文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        ///文件相对路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 备用ClientID
        /// IP:Port
        /// </summary>
        public string StandbyClientID { get; set; }

        /// <summary>
        ///备用负载均衡服务器ID
        /// </summary>
        public string StandbySLBID { get; set; }

        /// <summary>
        /// 备用负载均衡服务器IP_Port
        /// </summary>
        public string StandbySLBIP_Port { get; set; }

        /// <summary>
        /// 签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID（即后台的APIU_ID）
        /// </summary>
        public string SEID { get; set; }
    }

    /// <summary>
    ///文件交互的Head部分
    /// </summary>
    public class OMCandZZJInfoHead
    {
        string mark = "^";
        public OMCandZZJInfoHead()
        {
        }
        public OMCandZZJInfoHead(string strHead)
        {
            if (string.IsNullOrEmpty(strHead) || strHead.Length < 6)
            {
                return;
            }
            string[] plit = strHead.Split('^');

            Token = plit[0];
            BusID = plit[1];
            TID = plit[2];
            Zip = int.Parse(plit[3]);
            BusServerID = plit.Length > 4 ? plit[4] : "";

            StandbyClientID = plit.Length > 5 ? plit[5] : "";
            StandbySLBID = plit.Length > 6 ? plit[6] : "";
            StandbySLBIP_Port = plit.Length > 7 ? plit[7] : "";

            SignType = plit.Length > 8 ? int.Parse(plit[8]) : 0;
            EncryptType = plit.Length > 9 ? int.Parse(plit[9]) : 0;
            SEID = plit.Length > 10 ? plit[10] : "";
            BusVersion = plit.Length > 11 ? plit[11] : "1.0";
            ZZJID = plit.Length > 12 ? plit[12] : "";
            OMCID = plit.Length > 13 ? plit[13] : "";

            Extend = plit.Length > 14 ? GZipHelper.DecompressBase64(plit[14]) : "";
            SLBID = plit.Length > 15 ? plit[15] : "";
            OrgID = plit.Length > 16 ? plit[16] : "";

        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Token);
            builder.Append(mark + BusID);
            builder.Append(mark + TID);
            builder.Append(mark + Zip.ToString());
            builder.Append(mark + BusServerID);

            builder.Append(mark + StandbyClientID);
            builder.Append(mark + StandbySLBID);
            builder.Append(mark + StandbySLBIP_Port);

            builder.Append(mark + SignType.ToString());
            builder.Append(mark + EncryptType.ToString());
            builder.Append(mark + SEID);

            builder.Append(mark + BusVersion);
            builder.Append(mark + ZZJID);
            builder.Append(mark + OMCID);
            builder.Append(mark + (string.IsNullOrEmpty(Extend) ? "" : GZipHelper.CompressToBase64(Extend)));
            builder.Append(mark + SLBID);
            builder.Append(mark + OrgID);
            return builder.ToString();
        }
        public string BusVersion { get; set; }



        /// <summary>
        /// BusServerID
        /// </summary>
        public string BusServerID { get; set; }

        /// <summary>
        ///  Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// BusName任务ID
        /// </summary>
        public string BusID { get; set; }
        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int Zip { get; set; }


        /// <summary>
        ///自助机id
        /// </summary>
        public string ZZJID { get; set; }
        /// <summary>
        /// OMCID
        /// </summary>
        public string OMCID { get; set; }

        /// <summary>
        ///医院SLBID  公司连接医院的客户端时需要，其他时为空
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// 客户机构ID，只有公司连接医院的客户端时非空，其他时为空
        /// </summary>
        public string OrgID { get; set; }



        /// <summary>
        /// 备用ClientID
        /// IP:Port
        /// </summary>
        public string StandbyClientID { get; set; }

        /// <summary>
        ///备用负载均衡服务器ID
        /// </summary>
        public string StandbySLBID { get; set; }

        /// <summary>
        /// 备用负载均衡服务器IP_Port
        /// </summary>
        public string StandbySLBIP_Port { get; set; }

        /// <summary>
        /// 签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID（即后台的APIU_ID）
        /// </summary>
        public string SEID { get; set; }

        /// <summary>
        ///扩展参数 根据不同busID 自定义
        /// </summary>
        public string Extend { get; set; }
        /// <summary>
        ///赋值扩展参数 根据不同busID 自定义
        /// </summary>
        public void SetExtend<T>(T t)
        {
            Extend = JsonConvert.SerializeObject(t);
        }
        /// <summary>
        ///获取扩展参数 根据不同busID 自定义
        /// </summary>
        public T GetExtend<T>()
        {
            return JsonConvert.DeserializeObject<T>(Extend);
        }

    }
    /// <summary>
    /// 负载均衡和业务服务器交互的Head部分
    /// </summary>
    public class SLBInfoHeadBusS
    {

        string mark = "^";

        public SLBInfoHeadBusS()
        {
        }
        public SLBInfoHeadBusS(SLBInfoHead sLBInfoHead)
        {
            this.BusID = sLBInfoHead.BusID;
            this.TID = sLBInfoHead.TID;
            this.Zip = sLBInfoHead.Zip;

            this.BusServerID = sLBInfoHead.BusServerID;

            this.StandbyClientID = sLBInfoHead.StandbyClientID;
            this.StandbySLBID = sLBInfoHead.StandbySLBID;
            this.StandbySLBIP_Port = sLBInfoHead.StandbySLBIP_Port;

            this.SignType = sLBInfoHead.SignType;
            this.EncryptType = sLBInfoHead.EncryptType;
            this.SEID = sLBInfoHead.SEID;
            this.Token = sLBInfoHead.Token;
            this.BusVersion = sLBInfoHead.BusVersion;
            this.RealBVer = this.BusVersion;
            //    this.Extend = sLBInfoHead.Extend;
        }
        public SLBInfoHeadBusS(OMCandZZJInfoHead sLBInfoHead)
        {
            this.BusID = sLBInfoHead.BusID;
            this.TID = sLBInfoHead.TID;
            this.Zip = sLBInfoHead.Zip;

            this.BusServerID = sLBInfoHead.BusServerID;

            this.StandbyClientID = sLBInfoHead.StandbyClientID;
            this.StandbySLBID = sLBInfoHead.StandbySLBID;
            this.StandbySLBIP_Port = sLBInfoHead.StandbySLBIP_Port;

            this.SignType = sLBInfoHead.SignType;
            this.EncryptType = sLBInfoHead.EncryptType;
            this.SEID = sLBInfoHead.SEID;
            this.Token = sLBInfoHead.Token;
            this.BusVersion = sLBInfoHead.BusVersion;
            this.RealBVer = this.BusVersion;
            this.Extend = sLBInfoHead.Extend;
        }

        public SLBInfoHeadBusS(SLBInfoHeadBusS sLBInfoHeadS)
        {
            this.BusID = sLBInfoHeadS.BusID;
            this.TID = sLBInfoHeadS.TID;
            this.Zip = sLBInfoHeadS.Zip;
            this.ClientID = sLBInfoHeadS.ClientID;
            this.BusServerID = sLBInfoHeadS.BusServerID;

            this.SLBID = sLBInfoHeadS.SLBID;
            this.SLBIP_Port = sLBInfoHeadS.SLBIP_Port;

            this.StandbyClientID = sLBInfoHeadS.StandbyClientID;
            this.StandbySLBID = sLBInfoHeadS.StandbySLBID;
            this.StandbySLBIP_Port = sLBInfoHeadS.StandbySLBIP_Port;

            this.SignType = sLBInfoHeadS.SignType;
            this.EncryptType = sLBInfoHeadS.EncryptType;
            this.SEID = sLBInfoHeadS.SEID;

            this.HashCodeInData = sLBInfoHeadS.HashCodeInData;
            this.Token = sLBInfoHeadS.Token;
            this.BusVersion = sLBInfoHeadS.BusVersion;
            this.RealBVer = sLBInfoHeadS.RealBVer;
            this.Extend = sLBInfoHeadS.Extend;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Token);
            builder.Append(mark + BusID);
            builder.Append(mark + TID);
            builder.Append(mark + Zip.ToString());
            builder.Append(mark + ClientID);
            builder.Append(mark + BusServerID);

            builder.Append(mark + SLBID);
            builder.Append(mark + SLBIP_Port);

            builder.Append(mark + StandbyClientID);
            builder.Append(mark + StandbySLBID);
            builder.Append(mark + StandbySLBIP_Port);

            builder.Append(mark + SignType.ToString());
            builder.Append(mark + EncryptType.ToString());
            builder.Append(mark + SEID);

            builder.Append(mark + HashCodeInData.ToString());
            builder.Append(mark + BusVersion.ToString());
            builder.Append(mark + RealBVer.ToString());
            builder.Append(mark + (string.IsNullOrEmpty(Extend) ? "" : GZipHelper.CompressToBase64(Extend)));
            return builder.ToString();
        }
        public SLBInfoHeadBusS(string strHead)
        {
            if (string.IsNullOrEmpty(strHead) || strHead.Length < 14)
            {
                return;
            }
            string[] plit = strHead.Split('^');
            Token = plit[0];
            BusID = plit[1];
            TID = plit[2];
            Zip = int.Parse(plit[3]);
            ClientID = plit[4];
            BusServerID = plit[5];

            SLBID = plit[6];
            SLBIP_Port = plit[7];

            StandbyClientID = plit[8];
            StandbySLBID = plit[9];
            StandbySLBIP_Port = plit[10];

            SignType = int.Parse(plit[11]);
            EncryptType = int.Parse(plit[12]);
            SEID = plit[13];
            HashCodeInData = int.Parse(plit[14]);
            if (plit.Length >= 15)
            {
                BusVersion = plit[14];
            }
            else
            {
                BusVersion = "1.0";
            }
            if (plit.Length >= 16)
            {
                RealBVer = plit[15];
            }
            else
            {
                RealBVer = BusVersion;
            }
            Extend = plit.Length > 16 ? GZipHelper.DecompressBase64(plit[16]) : "";


        }
        /// <summary>
        ///  Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        ///BusName任务ID
        /// </summary>
        public string BusID { get; set; }

        /// <summary>
        /// 请求唯一ID
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        ///压缩 0 未压缩；1 GZipStream 压缩；
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// ClientID
        /// IP:Port
        /// </summary>
        public string ClientID { get; set; }
        /// <summary>
        /// 负载均衡服务器ID
        /// </summary>
        public string SLBID { get; set; }
        /// <summary>
        /// 负载均衡服务器IP_Port
        /// </summary>
        public string SLBIP_Port { get; set; }



        /// <summary>
        /// BusServerID
        /// </summary>
        public string BusServerID { get; set; }



        /// <summary>
        /// 备用ClientID
        /// IP:Port
        /// </summary>
        public string StandbyClientID { get; set; }

        /// <summary>
        ///备用负载均衡服务器ID
        /// </summary>
        public string StandbySLBID { get; set; }

        /// <summary>
        /// 备用负载均衡服务器IP_Port
        /// </summary>
        public string StandbySLBIP_Port { get; set; }


        /// <summary>
        /// 签名类别 0:不签名；1:MD5 ; 2:RSA ;3:RSA2  (在非0 情况下与EncryptType对应)
        /// </summary>
        public int SignType { get; set; }

        /// <summary>
        /// 加密类别 0:不加密；1:AES ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)
        /// </summary>
        public int EncryptType { get; set; }
        /// <summary>
        /// 加密或签名的对应系统配置的密钥或公钥ID
        /// </summary>
        public string SEID { get; set; }


        /// <summary>
        /// 记录客户端请求时间,在SLB中AddcdBusServerAndClient赋值，其他地方不用
        /// </summary>
        public DateTime InTime { get; set; }

        /// <summary>
        /// 请求参数的HashCode,主要要用于<see cref="SSLBCacheData"/>的处理
        /// </summary>
        public int HashCodeInData { get; set; }

        /// <summary>
        /// 测试等级0 表示未定义 ;1 表示Level1；2 Level2；3 Level3
        /// 对于测试的<see cref="SSLBCacheData"/>不进行存储
        /// </summary>
        public int TestLevel { get; set; }

        /// <summary>
        /// 业务版本 默认1.0
        /// </summary>
        public string BusVersion { get; set; }
        /// <summary>
        ///实际执行的 业务版本   当SLB获取到兼容版本时 赋值
        /// </summary>
        public string RealBVer { get; set; }

        /// <summary>
        ///扩展参数 根据不同busID 自定义
        /// </summary>
        public string Extend { get; set; }
        /// <summary>
        ///赋值扩展参数 根据不同busID 自定义
        /// </summary>
        public void SetExtend<T>(T t)
        {
            Extend = JsonConvert.SerializeObject(t);
        }
        /// <summary>
        ///获取扩展参数 根据不同busID 自定义
        /// </summary>
        public T GetExtend<T>()
        {
            return JsonConvert.DeserializeObject<T>(Extend);
        }
        /// <summary>
        /// SLBInfoHeadBusS 的标识:  this.BusID + "^" + TID + "^" + this.ClientID 
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return this.BusID + "^" + TID + "^" + this.ClientID;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(SLBInfoHeadBusS))
                return false;

            SLBInfoHeadBusS hcc = obj as SLBInfoHeadBusS;
            return this.GetID() == hcc.GetID();
        }

        public override int GetHashCode()
        {
            return (GetID()).GetHashCode();
        }
    }


}

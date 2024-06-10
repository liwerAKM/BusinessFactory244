using Newtonsoft.Json;
using PasS.Base.Lib.DAL;
using PasS.Base.Lib.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PasS.Base.Lib
{
    /// <summary>
    /// SPring内部交互函数 主要提供给BBusServer调用
    /// </summary>
    public class SpringAPI
    {

        const string SEID = "SpringAPI";
        static int EncryptType = 1;
        /// <summary>
        /// 调用HTTP服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="BusID"></param>
        /// <param name="InData"></param>
        /// <returns></returns>
        public static T Call<T>(string BusID, string InData)
        {
            SpringAPIData springAPIdata = new SpringAPIData();
            springAPIdata.BusID = BusID;
            springAPIdata.user_id = MyPubConstant.BusServerID;
            springAPIdata.Param = SignEncryptHelper.Encrypt(InData, EncryptType, SEID);


            HttpClient httpClient = new HttpClient(MyPubConstant.HttpServerURL + "/SLB/SpringAPI/");
            //// 发送请求获取通信应答
            int status = 0;
            T ret = default(T);

            status = httpClient.SendJson(JsonConvert.SerializeObject(springAPIdata), Encoding.UTF8);
            if (status == 200)
            {
                // 返回结果
                string result = httpClient.Result;
                if (!string.IsNullOrEmpty(result))
                {

                    try
                    {
                        SpringAPIDataOut springHttpDataOut = JsonConvert.DeserializeObject<SpringAPIDataOut>(result);
                        if (springHttpDataOut.ReslutCode == 1)
                        {

                            ret = JsonConvert.DeserializeObject<T>(SignEncryptHelper.Decrypt(springHttpDataOut.Param, EncryptType, SEID));

                        }

                    }
                    catch (Exception ex)
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("HttpAsyncInform:{0}   Error!! ", ex.Message));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
            }
            return ret;
        }

        public static string Call(string BusID, string InData)
        {
            SpringAPIData springAPIdata = new SpringAPIData();
            springAPIdata.BusID = BusID;
            springAPIdata.user_id = MyPubConstant.BusServerID;
            springAPIdata.Param = SignEncryptHelper.Encrypt(InData, EncryptType, SEID);

            string ret = "";
            HttpClient httpClient = new HttpClient(MyPubConstant.HttpServerURL + "/SLB/SpringAPI/");
            //// 发送请求获取通信应答
            int status = 0;
            status = httpClient.SendJson(JsonConvert.SerializeObject(springAPIdata), Encoding.UTF8);
            if (status == 200)
            {
                // 返回结果
                string result = httpClient.Result;
                if (!string.IsNullOrEmpty(result))
                {

                    try
                    {
                        SpringAPIDataOut springHttpDataOut = JsonConvert.DeserializeObject<SpringAPIDataOut>(result);
                        if (springHttpDataOut.ReslutCode == 1)
                        {
                            ret = (SignEncryptHelper.Decrypt(springHttpDataOut.Param, EncryptType, SEID));
                        }

                    }
                    catch (Exception ex)
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(string.Format("HttpAsyncInform:{0}   Error!! ", ex.Message));
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                }
            }
            return ret;
        }

        public static string Remove(SpringAPIData springAPIData)
        {
            SpringAPIDataOut springHttpDataOut = new SpringAPIDataOut();
            try
            {
                //  SpringAPIData springAPIData = JsonConvert.DeserializeObject<SpringAPIData>(springAPIDataJson);

                string Param = SignEncryptHelper.Decrypt(springAPIData.Param, EncryptType, SEID);
                string result = "";
                switch (springAPIData.BusID)
                {
                    case "SysInfoGet":
                        {
                            result = DbHelper.SysInfoGet(Param);
                            break;
                        }
                    case "BusServerTableGet":
                        {
                            DataTable dt = DbHelper.BusServerTableGet(Param);
                            result = JsonConvert.SerializeObject(dt);
                            break;
                        }
                    case "BusServerGet":
                        {
                            BusServerInfo bsi = DbHelper.BusServerGet(Param);
                            result = JsonConvert.SerializeObject(bsi);
                            break;
                        }
                    case "BusServerSetStatus":
                        {
                            SBusServerSetStatus sBusServerSetStatus = JsonConvert.DeserializeObject<SBusServerSetStatus>(Param);
                            bool ret = DbHelper.BusServerSetStatus(sBusServerSetStatus.BusServerID, sBusServerSetStatus.Status);
                            result = JsonConvert.SerializeObject(ret);
                            break;
                        }
                    case "SLBInfoListGetForServer":
                        {
                            List<SLBInfo> list = DbHelper.SLBInfoListGetForServer(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }
                    case "SLBInfoGetForServer"://获取BusServer对应的指定SLB信息（如果有网闸映射，则替换成对应地址）
                        {
                            SSLBInfoGetForServer sfs = JsonConvert.DeserializeObject<SSLBInfoGetForServer>(Param);
                            SLBInfo sLBInfo = DbHelper.SLBInfoGetForServer(sfs.BusServerID, sfs.SLBID);
                            result = JsonConvert.SerializeObject(sLBInfo);
                            break;
                        }

                    case "SLBInfoListGet":
                        {
                            List<SLBInfo> list = DbHelper.SLBInfoListGet(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }
                    case "BusServerBusInfoSingleListByServerGet":
                        {
                            List<SingleBusServerInfo> list = DbHelper.BusServerBusInfoSingleListByServerGet(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }
                    case "BusServerBusInfoSingleVerListByServerGet":
                        {
                            List<SingleBusinessInfoVer> list = DbHelper.BusServerBusInfoSingleVerListByServerGet(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }

                    case "BusinessInfoGet":
                        {
                            BusinessInfo list = DbHelper.BusinessInfoGet(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }

                    case "BusinessInfoVerGet"://add by wanglei 20200214
                        {
                            SingleBusinessInfoVer infoVer = JsonConvert.DeserializeObject<SingleBusinessInfoVer>(Param);
                            BusinessInfoBusVersion busVersion = DbHelper.BusinessInfoVerGet(infoVer.BusID, infoVer.BusVersion);
                            result = JsonConvert.SerializeObject(busVersion);
                            break;
                        }
                    case "BusinessInfoVerGetServer"://add by wanglei 20200214
                        {
                            SingleBusinessInfoVer infoVer = JsonConvert.DeserializeObject<SingleBusinessInfoVer>(Param);
                            BusinessInfoBusVersion busVersion = DbHelper.BusinessInfoVerGet(infoVer.BusServerID,infoVer.BusID, infoVer.BusVersion);
                            result = JsonConvert.SerializeObject(busVersion);
                            break;
                        }
                        
                    case "BusServerUpDownTime":
                        {
                            bool ret = DbHelper.BusServerUpDownTime(Param);
                            result = JsonConvert.SerializeObject(ret);
                            break;
                        }
                    case "FileinfoGetModelWhere":
                        {
                            SFileinfoGetModelWhere sFileinfoGetModelWhere = JsonConvert.DeserializeObject<SFileinfoGetModelWhere>(Param);
                            Fileinfo dalFileinfo = new Fileinfo();
                            List<fileinfo> list = dalFileinfo.GetModelInlyBusSWhere(sFileinfoGetModelWhere.where, sFileinfoGetModelWhere.BusServerID);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }
                    case "FileinfoGetModel":
                        {
                            Fileinfo dalFileinfo = new Fileinfo();
                            fileinfo file = JsonConvert.DeserializeObject<fileinfo>(Param);
                            fileinfo fileret = dalFileinfo.GetModel(file.projectID, file.FilePath, file.Filename, file.Version);
                            result = JsonConvert.SerializeObject(fileret);
                            break;
                        }
                    case "GetModelandImage":
                        {
                            Fileinfo dalFileinfo = new Fileinfo();
                            fileinfo file = JsonConvert.DeserializeObject<fileinfo>(Param);
                            fileinfo fileret = dalFileinfo.GetModelandImage(file.projectID, file.FilePath, file.Filename, file.Version);
                            result = JsonConvert.SerializeObject(fileret);
                            break;
                        }

                    case "DependentFileGet":
                        {
                            SDependentFileGet parm = JsonConvert.DeserializeObject<SDependentFileGet>(Param);
                            DataTable dt = DbHelper.DependentFileGet(parm.BusID, parm.VersionN);
                            result = JsonConvert.SerializeObject(dt);
                            break;
                        }
                    case "GetDefaultStatusAndMarkcd":
                        {
                            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                            ConcurrentDictionary<string, List<string>> concurrent = DbHelper.GetDefaultStatusAndMarkcd();
                            if (concurrent != null)
                            {
                                foreach (string key in concurrent.Keys)
                                {
                                    dic.Add(key, concurrent[key]);
                                }
                            }
                            result = JsonConvert.SerializeObject(dic);
                            break;
                        }
                    case "GetBusStatusAndMarkcd":
                        {
                            SBusIDBusVersion parm = JsonConvert.DeserializeObject<SBusIDBusVersion>(Param);
                            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                            ConcurrentDictionary<string, List<string>> concurrent = DbHelper.GetBusStatusAndMarkcd(parm.BusID, parm.BusVersion);
                            if (concurrent != null)
                            {
                                foreach (string key in concurrent.Keys)
                                {
                                    dic.Add(key, concurrent[key]);
                                }
                            }
                            result = JsonConvert.SerializeObject(dic);
                            break;
                        }
                    case "GetTestLevelListBV":
                        {
                            Dictionary<int, businessinfoversion> dic = new Dictionary<int, businessinfoversion>();
                            ConcurrentDictionary<int, businessinfoversion> concurrent = new BusinessinfoVersion().GetTestLevelList(Param);
                            if (concurrent != null)
                            {
                                foreach (int key in concurrent.Keys)
                                {
                                    dic.Add(key, concurrent[key]);
                                }
                            }
                            result = JsonConvert.SerializeObject(dic);
                            break;
                        }

                    case "BusServerBusHTTPGetBusIDList":
                        {
                            List<string> list = DbHelper.BusServerBusHTTPGetBusIDList(Param);
                            result = JsonConvert.SerializeObject(list);
                            break;
                        }
                    case "BusinessInfoTableGet":
                        {
                            DataTable dt = DbHelper.BusinessInfoTableGet(Param);
                            result = JsonConvert.SerializeObject(dt);
                            break;
                        }
                    case "GetTbNewSysBusSFile":
                        {
                            DateTime dateTime = DateTime.Parse(Param);
                            Fileinfo dalFileinfo = new Fileinfo();
                            DataTable dt = dalFileinfo.GetTbNewSysBusSFile(dateTime);
                            result = JsonConvert.SerializeObject(dt);
                            break;
                        }
                    case "SaveBusLogData":
                        {
                            // MySpringLog.Info("SpringAPI.Remove.SaveBusLogData" , Param);
                            BusLogData busLogData = JsonConvert.DeserializeObject<BusLogData>(Param);
                            bool ret = BusLogHelper.SaveLog(busLogData);
                            result = JsonConvert.SerializeObject(ret);
                            break;
                        }
                    case "SLBInfoGet":
                        {
                            // MySpringLog.Info("SpringAPI.Remove.SaveBusLogData" , Param);
                            BusLogData busLogData = JsonConvert.DeserializeObject<BusLogData>(Param);
                            SLBInfo sLBInfo= DbHelper.SLBInfoGet(Param);
                            result = JsonConvert.SerializeObject(sLBInfo);
                            break;
                        }
                       

                       
                }
                if (!string.IsNullOrEmpty(result))
                {
                    springHttpDataOut.Param = SignEncryptHelper.Encrypt(result, EncryptType, SEID);
                    springHttpDataOut.ReslutCode = 1;
                    springHttpDataOut.ResultMessage = "sucess";
                }
                else
                {
                    springHttpDataOut.Param = "";
                    springHttpDataOut.ReslutCode = -2;
                    springHttpDataOut.ResultMessage = "没有获取到返回值" + JsonConvert.SerializeObject(springAPIData);
                }

            }
            catch (Exception ex)
            {
                springHttpDataOut.ReslutCode = -1;
                springHttpDataOut.ResultMessage = "处理出错" + ex.Message;
                PasSLog.Error("SpringAPI.Remove", ex.ToString() + springAPIData.Param);
            }
            return JsonConvert.SerializeObject(springHttpDataOut);
        }

        public static string SysInfoGet(string Key)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call("SysInfoGet", Key);

            return DbHelper.SysInfoGet(Key);
        }
        public static DataTable BusServerTableGet(string where)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<DataTable>("BusServerTableGet", where);

            return DbHelper.BusServerTableGet(where);
        }
        public static BusServerInfo BusServerGet(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<BusServerInfo>("BusServerGet", BusServerID);

            return DbHelper.BusServerGet(BusServerID); ;
        }

        public static bool BusServerSetStatus(string BusServerID, int Status)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SBusServerSetStatus sBusServerSetStatus = new SBusServerSetStatus();
                sBusServerSetStatus.BusServerID = BusServerID;
                sBusServerSetStatus.Status = Status;
                return Call<bool>("BusServerSetStatus", JsonConvert.SerializeObject(sBusServerSetStatus));
            }
            return DbHelper.BusServerSetStatus(BusServerID, Status);
        }

        public static List<SLBInfo> SLBInfoListGet(string where)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<List<SLBInfo>>("SLBInfoListGet", where);
            return DbHelper.SLBInfoListGet(where);
        }

        /// <summary>
        /// 获取BusServer对应的SLB信息列表（如果有网闸映射，则替换成对应地址）
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public static List<SLBInfo> SLBInfoListGetForServer(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<List<SLBInfo>>("SLBInfoListGetForServer", BusServerID);
            return DbHelper.SLBInfoListGetForServer(BusServerID);
        }

        /// <summary>
        ///获取BusServer对应的指定SLB信息（如果有网闸映射，则替换成对应地址）
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public static SLBInfo SLBInfoGetForServer(string BusServerID, string SLBID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SSLBInfoGetForServer sSLBInfoGetForServer = new SSLBInfoGetForServer();
                sSLBInfoGetForServer.BusServerID = BusServerID;
                sSLBInfoGetForServer.SLBID = SLBID;
                return Call<SLBInfo>("SLBInfoGetForServer", JsonConvert.SerializeObject(sSLBInfoGetForServer));
            }
            return DbHelper.SLBInfoGetForServer(BusServerID, SLBID);
        }


        public static List<SingleBusServerInfo> BusServerBusInfoSingleListByServerGet(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<List<SingleBusServerInfo>>("BusServerBusInfoSingleListByServerGet", BusServerID);

            return DbHelper.BusServerBusInfoSingleListByServerGet(BusServerID);
        }
        public static List<SingleBusinessInfoVer> BusServerBusInfoSingleVerListByServerGet(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<List<SingleBusinessInfoVer>>("BusServerBusInfoSingleVerListByServerGet", BusServerID);

            return DbHelper.BusServerBusInfoSingleVerListByServerGet(BusServerID);
        }
        public static BusinessInfo BusinessInfoGet(string BusID)
        {
            if (MyPubConstant.UseHttpSpringAPI)

                return Call<BusinessInfo>("BusinessInfoGet", BusID);
            return DbHelper.BusinessInfoGet(BusID);
        }
        public static BusinessInfoBusVersion BusinessInfoVerGet(string BusID, string BusVersion)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SingleBusinessInfoVer infoVer = new SingleBusinessInfoVer();
                infoVer.BusID = BusID;
                infoVer.BusVersion = BusVersion;
                return Call<BusinessInfoBusVersion>("BusinessInfoVerGet", JsonConvert.SerializeObject(infoVer));
            }
            return DbHelper.BusinessInfoVerGet(BusID, BusVersion);
        }
        public static BusinessInfoBusVersion BusinessInfoVerGet(string BusServerID, string BusID, string BusVersion)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SingleBusinessInfoVer infoVer = new SingleBusinessInfoVer();
                infoVer.BusID = BusID;
                infoVer.BusVersion = BusVersion;
                infoVer.BusServerID = BusServerID;
                return Call<BusinessInfoBusVersion>("BusinessInfoVerGetServer", JsonConvert.SerializeObject(infoVer));
            }
            return DbHelper.BusinessInfoVerGet(BusServerID,BusID, BusVersion);
        }

        public static bool BusServerUpDownTime(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<bool>("BusServerUpDownTime", BusServerID);

            return DbHelper.BusServerUpDownTime(BusServerID);
        }
        public static List<fileinfo> FileinfoGetModelWhere(string where, string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SFileinfoGetModelWhere sFileinfoGetModelWhere = new SFileinfoGetModelWhere();
                sFileinfoGetModelWhere.BusServerID = BusServerID;
                sFileinfoGetModelWhere.where = where;
                Fileinfo dalFileinf2o = new Fileinfo();
                //    List<fileinfo> list3 = dalFileinf2o.GetModelInlyBusSWhere(sFileinfoGetModelWhere.where, sFileinfoGetModelWhere.BusServerID);
                return Call<List<fileinfo>>("FileinfoGetModelWhere", JsonConvert.SerializeObject(sFileinfoGetModelWhere));
            }

            Fileinfo dalFileinfo = new Fileinfo();
            List<fileinfo> list = dalFileinfo.GetModelInlyBusSWhere(where, BusServerID);
            return list;

        }

        public static fileinfo FileinfoGetModel(string projectID, string FilePath, string Filename, string Version)
        {

            if (MyPubConstant.UseHttpSpringAPI)
            {
                fileinfo fileinfo = new fileinfo();
                fileinfo.projectID = projectID;
                fileinfo.FilePath = FilePath;
                fileinfo.Filename = Filename;
                fileinfo.Version = Version;
                return Call<fileinfo>("FileinfoGetModel", JsonConvert.SerializeObject(fileinfo));
            }

            Fileinfo dalFileinfo = new Fileinfo();
            fileinfo filnfoRet = dalFileinfo.GetModel(projectID, FilePath, Filename, Version);
            return filnfoRet;

        }
        public static fileinfo GetModelandImage(string projectID, string FilePath, string Filename, string Version)
        {

            if (MyPubConstant.UseHttpSpringAPI)
            {
                fileinfo fileinfo = new fileinfo();
                fileinfo.projectID = projectID;
                fileinfo.FilePath = FilePath;
                fileinfo.Filename = Filename;
                fileinfo.Version = Version;
                return Call<fileinfo>("GetModelandImage", JsonConvert.SerializeObject(fileinfo));
            }

            Fileinfo dalFileinfo = new Fileinfo();
            fileinfo filnfoRet = dalFileinfo.GetModelandImage(projectID, FilePath, Filename, Version);
            return filnfoRet;

        }


        public static DataTable DependentFileGet(string BusID, decimal VersionN)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                SDependentFileGet sDependentFileGet = new SDependentFileGet();
                sDependentFileGet.BusID = BusID;
                sDependentFileGet.VersionN = VersionN;
                return Call<DataTable>("DependentFileGet", JsonConvert.SerializeObject(sDependentFileGet));
            }

            return DbHelper.DependentFileGet(BusID, VersionN);

        }

        /// <summary>
        ///  默认测试组
        ///  Status^MarkType  List<MarkValue>
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, List<string>> GetDefaultStatusAndMarkcd()
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                ConcurrentDictionary<string, List<string>> cdic = new ConcurrentDictionary<string, List<string>>();
                Dictionary<string, List<string>> dic = Call<Dictionary<string, List<string>>>("GetDefaultStatusAndMarkcd", "默认测试组");
                if (dic != null)
                {
                    foreach (string key in dic.Keys)
                    {
                        cdic.TryAdd(key, dic[key]);
                    }
                }
                return cdic;

            }
            return DbHelper.GetDefaultStatusAndMarkcd();
        }
        public static ConcurrentDictionary<string, List<string>> GetBusStatusAndMarkcd(SBusIDBusVersion sBusIDBus)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                ConcurrentDictionary<string, List<string>> cdic = new ConcurrentDictionary<string, List<string>>();
                Dictionary<string, List<string>> dic = Call<Dictionary<string, List<string>>>("GetBusStatusAndMarkcd", JsonConvert.SerializeObject(sBusIDBus));
                if (dic != null)
                {
                    foreach (string key in dic.Keys)
                    {
                        cdic.TryAdd(key, dic[key]);
                    }
                }
                return cdic;
            }
            return DbHelper.GetBusStatusAndMarkcd(sBusIDBus.BusID, sBusIDBus.BusVersion);
        }

        public static ConcurrentDictionary<int, businessinfoversion> GetTestLevelListBV(string BusID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
            {
                ConcurrentDictionary<int, businessinfoversion> cdic = new ConcurrentDictionary<int, businessinfoversion>();
                Dictionary<int, businessinfoversion> dic = Call<Dictionary<int, businessinfoversion>>("GetTestLevelListBV", BusID);
                if (dic != null)
                {
                    foreach (int key in dic.Keys)
                    {
                        cdic.TryAdd(key, dic[key]);
                    }
                }
                return cdic;
            }
            return new BusinessinfoVersion().GetTestLevelList(BusID);
        }



        /// <summary>
        /// BusServer允许开放HTTP服务的BusIDList
        /// </summary>
        /// <param name="BusServerID"></param>
        /// <returns></returns>
        public static List<string> BusServerBusHTTPGetBusIDList(string BusServerID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<List<string>>("BusServerBusHTTPGetBusIDList", BusServerID);
            return DbHelper.BusServerBusHTTPGetBusIDList(BusServerID);
        }

        public static DataTable BusinessInfoTableGet(string where)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<DataTable>("BusinessInfoTableGet", where);
            return DbHelper.BusinessInfoTableGet(where);
        }

        public static DataTable GetTbNewSysBusSFile(DateTime lastUpTime)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<DataTable>("GetTbNewSysBusSFile", lastUpTime.ToString());
            Fileinfo dalFileinfo = new Fileinfo();
            return dalFileinfo.GetTbNewSysBusSFile(lastUpTime);
        }

        public static bool SaveBusLogData(BusLogData busLogData)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<bool>("SaveBusLogData", JsonConvert.SerializeObject(busLogData));

            return BusLogHelper.SaveLog(busLogData);
        }
        public static SLBInfo SLBInfoGet(string  SLBID)
        {
            if (MyPubConstant.UseHttpSpringAPI)
                return Call<SLBInfo>("SLBInfoGet", SLBID);

            return DbHelper.SLBInfoGet(SLBID);
        }



        struct SDependentFileGet
        {
            public string BusID; public decimal VersionN;
        }
        struct SBusServerSetStatus
        {
            public string BusServerID; public int Status;
        }
        struct SFileinfoGetModelWhere
        {
            public string BusServerID; public string where;
        }

        struct SSLBInfoGetForServer
        {
            public string BusServerID; public string SLBID;
        }

    }
    /// <summary>
    /// spring API JSON入参结构
    /// </summary>
    public class SpringAPIData
    {
        /// <summary>
        /// 入参
        /// </summary>
        public string Param;
        /// <summary>
        ///API user_ID
        /// </summary>
        public string user_id;
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;


        /// <summary>
        /// 1 入参为业务数据AES加密 12入参为<see cref="SLBBusinessInfo"/>,13入参为业务明文(此模式只限制公司内部网络使用)
        /// </summary>
        internal int ParamType;

        public string BusID;

        /// <summary>
        /// 请求唯一ID 不重复  非必填
        /// </summary>
        public string TID { get; set; }


    }

    /// <summary>
    /// 业务消息
    /// </summary>
    public class SpringAPIDataOut
    {
        /// <summary>
        /// 请求唯一ID 不重复
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        /// 返回结果标识 1成功 成功时 sign，Param才有值
        /// </summary>
        public int ReslutCode { get; set; }
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;

        /// <summary>
        /// 返回具体业务内容
        /// </summary>
        public string Param { get; set; }

    }

}

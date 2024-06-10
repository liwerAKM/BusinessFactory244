using Newtonsoft.Json;
using SuperSocket.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasS.Base.Lib;
using static PasS.Base.Lib.SSysRegisterInfo;

namespace PaaS.Comm
{
    

    /// <summary>
    /// Spring WebSocket 基础类
    /// </summary>
    public abstract class WebSocketSessionClientBase : WebClientBase
    {

        /// <summary>
        /// 压缩阀值，当Bydy大于此长度时进行压缩 1 Gzip压缩
        /// </summary>
        protected int ZipBufferSize = 2000;
        /// <summary>
        ///0 WebSocketServer ; 1 SLB客户端SLBClient；2 SLB服务端SLBBusServer  3 BusServerCallOthter； 7OperationMaintenanceClient ;   BusServerCallOthter = 3,  ServerLoadBalancing = 4,    GSLBServer = 5, AccessServer = 6,  OperationMaintenance = 7;ClientSLB==8
        /// </summary>
        protected int  ConnectType= 0;
        

        /// <summary>
        /// 是否验证Token
        /// </summary>
        protected bool CheckToken;
        /// <summary>
        /// Token
        /// </summary>
        protected string Token;
        /// <summary>
        /// 接收到统消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void ReciveDatasSysInfo(Int32 CCN, string Head, byte[] bInfo);
        /// <summary>
        /// 接收到内部系统控制消息
        /// </summary>
        /// <param name="sSysInfo"></param>
        protected abstract void ReciveDataInteriorSysInf(string strHeadX ,SSysInfo sSysInfo);

        /// <summary>
        /// 接收到服务端主动发给客户端消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void ReciveDataServerCallClientInfo(Int32 CCN, string Head, byte[] bInfo);
        /// <summary>
        ///  接收到客户端主动发给服务端消息数据
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void ReciveDataClientCallServerInfo(Int32 CCN, string Head, byte[] bInfo);

        /// <summary>
        /// 收到二进制文件及信息
        /// </summary>
        /// <param name="sRecFileInfo"></param>
        protected abstract void ReciveFileInfo(SRecFileInfo sRecFileInfo);

        /// <summary>
        /// 是SLB调用自助机管理端400-499
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void ReciveSLBRequestManageInfo(Int32 CCN, string Head, byte[] bInfo);
        /// <summary>
        /// //是自助机管理端调用SLB 500-599
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void ReciveManageRequestSLBInfo(Int32 CCN, string Head, byte[] bInfo);

        /// <summary>
        /// 收到 控制端和自助机之间通讯
        /// </summary>
        /// <param name="CCN"></param>
        /// <param name="Head"></param>
        /// <param name="bInfo"></param>
        protected abstract void OMC_and_Client(Int32 CCN,  OMCandZZJInfoHead Head, byte[] bInfo);

        private void  TaskRaiseReceived(byte[] receivedBytes)
        {
           Task.Run(() =>
            {
                try
                {
                    int ccN = System.BitConverter.ToInt32(receivedBytes, 0);
                    int HccN = ccN / 10000;//高位CCN 
                    int LccN = ccN % 10000;//低位CCN
                    byte[] bHeadX;
                    byte[] bInfo;
                    GetHeadAndInfo(receivedBytes, out ccN, out bHeadX, out bInfo);
                    string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                    SLBInfoHead headX = new SLBInfoHead(strHeadX);
                    if (headX.EncryptType > 0)
                    {
                        bInfo = SignEncryptHelper.Decrypt(bInfo, headX.EncryptType, headX.SEID);
                    }
                    if (headX.Zip == 1)
                    {
                        bInfo = GZipHelper.Decompress(bInfo);
                    }
                    if (CheckToken && ccN != CCNSysValye.Register)//检查token
                    {
                        // string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                        if (string.IsNullOrEmpty(strHeadX) || strHeadX.Length < 6)
                        {
                            return false;
                        }
                        string[] plit = strHeadX.Split('^');
                        if (plit[0] != this.Token)
                        {
                            return false;
                        }
                    }
                    //是系统消息
                    if (HCCNValye.IsSysInfo(HccN))
                    {
                        //系统内部控制消息
                        if (ccN == CCNSysValye.SysInfo)
                        {
                            string sysinfo = this.Encoding.GetString(bInfo);
                            //  GetHeadAndInfo(receivedBytes, out ccN, out sysinfo);
                            if (!string.IsNullOrEmpty(sysinfo))
                            {
                                SSysInfo sSysInfo = JsonConvert.DeserializeObject<SSysInfo>(sysinfo);
                                ReciveDataInteriorSysInf(strHeadX, sSysInfo);
                            }
                            return true;
                        } //系统内部控制消息
                        else if (ccN == CCNSysValye.TestConnect)
                        {
                            string sysinfo = this.Encoding.GetString(bInfo);
                            //  GetHeadAndInfo(receivedBytes, out ccN, out sysinfo);
                            if (!string.IsNullOrEmpty(sysinfo))
                            {
                                SSysInfo sSysInfo = JsonConvert.DeserializeObject<SSysInfo>(sysinfo);
                                ReciveDataInteriorSysInf(strHeadX, sSysInfo);
                            }
                            return true;
                        }
                        else if (ccN == CCNSysValye.SendFile)
                        {
                            // string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                            FileInfoHead fileInfohead = new FileInfoHead(strHeadX);
                            SRecFileInfo sRecFileInfo = new SRecFileInfo(fileInfohead);
                            sRecFileInfo.images = bInfo;
                            ReciveFileInfo(sRecFileInfo);
                            return true;
                        }
                        else if (HccN == HCCNValye.OMC_SLB_Client || HccN == HCCNValye.OMC_SLBR_Client || HccN == HCCNValye.Client_SLBR_OMC || HccN == HCCNValye.Client_SLB_OMC)
                        {
                            //  string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                            OMCandZZJInfoHead head = new OMCandZZJInfoHead(strHeadX);
                            OMC_and_Client(ccN, head, bInfo);
                            return true;
                        }
                        else if (HCCNValye.IsSLBRequestManage(HccN))
                        {
                            ReciveSLBRequestManageInfo(ccN, strHeadX, bInfo);
                        }
                        else if (HCCNValye.IsManageRequestSLB(HccN))//是自助机管理端调用SLB 500-599
                        {
                            ReciveManageRequestSLBInfo(ccN, strHeadX, bInfo);
                        }
                        else
                        {
                            // string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                            ReciveDatasSysInfo(ccN, strHeadX, bInfo);
                        }

                        return true;
                    }
                    //是服务器请求客户端
                    else if (HCCNValye.IsServerRequestClient(HccN))
                    {
                        // GetHeadAndInfo(receivedBytes, out ccN, out bHeadX, out bInfo);
                        // string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                        ReciveDataServerCallClientInfo(ccN, strHeadX, bInfo);
                    }
                    //是客户端请求服务器
                    else if (HCCNValye.IsClientrRequestServer(HccN))
                    {
                        // GetHeadAndInfo(receivedBytes, out ccN, out bHeadX, out bInfo);
                        //  string strHeadX = this.Encoding.GetString(bHeadX, 0, bHeadX.Length);
                        ReciveDataClientCallServerInfo(ccN, strHeadX, bInfo);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    string Message = string.Format("{0}:TaskRaiseReceived数据处理错误:{1} ", DateTime.Now.ToShortDateString() + " "
                 + DateTime.Now.ToShortTimeString(), ex.Message  );
                    ConsoleColor colorold = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Message);
                    Console.ForegroundColor = colorold;
                    PasSLog.Error("TaskRaiseReceived数据处理错误", ex.ToString());
                    return false;
                }
            }).ConfigureAwait(false);
           
        }

        protected  override void RaiseReceived(byte[] receivedBytes)
        {
            TaskRaiseReceived(receivedBytes);
        }

         
                
        #region Send 发送数据
        /// <summary>
        /// 发送系统消息 CCNValye.SysInfo
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <param name="Encrypt"></param>
        /// <returns></returns>
        public bool SendSysInfo(SSysInfo sSysInfo, bool Encrypt, OMCandZZJInfoHead sLBInfo = null)
        {
            try
            {
                if (Encrypt && sSysInfo.ET == 0)
                {
                    sSysInfo.Info = AESHelper.EncryptDefault(sSysInfo.Info);
                    sSysInfo.ET = 1;
                }
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(CCNSysValye.SysInfo, new OMCandZZJInfoHead(), sysinfo);
                //byte[] temp;
                //MergeHeadAndInfo(out temp, CCNValye.SysInfo, sysinfo);
                //Send(temp);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 发送系统消息 CCNValye.SysInfo
        /// </summary>
        /// <param name="sSysInfo"></param>
        /// <returns></returns>
        public bool SendSysInfo(SSysInfo sSysInfo, OMCandZZJInfoHead sLBInfo = null  )
        {
            try
            {
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                if(sLBInfo == null )
                    sLBInfo = new OMCandZZJInfoHead();
                sLBInfo.EncryptType = 1;
                Send(CCNSysValye.SysInfo, sLBInfo, sysinfo);
                ////byte[] temp;
                ////MergeHeadAndInfo(out temp, CCNValye.SysInfo, sysinfo);
                ////Send(temp);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocket4NetSpring.SendSysInfo", ex.Message + sSysInfo.Get_Info());
                return false;
            }
        }
        
        /// <summary>
        /// 测试连接数据
        /// </summary>
        /// <returns></returns>
        public bool TestCennect(string Info= "TestConnection")
        {
            try
            {
                SSysInfo sSysInfo = new SSysInfo(SysInfoType.AddOrUpdateBusinessInfoOnline);
                sSysInfo.Info = Info;
                sSysInfo.ET = 0;
                string sysinfo = JsonConvert.SerializeObject(sSysInfo);
                Send(CCNSysValye.SysInfo, new OMCandZZJInfoHead(), sysinfo);
                //byte[] temp;
                //MergeHeadAndInfo(out temp, CCNValye.TestConnect, JsonConvert.SerializeObject(sSysInfo));
                //Send(temp);
                return true;
            }
            catch (Exception ex)
            {
                PasSLog.Error("WebSocketSessionClientBase.TestCennect", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 生成测试连接数据
        /// </summary>
        /// <returns></returns>
        protected  byte[] GetTestCennect()
        {
           
                string sysinfo = "TestConnect";
                byte[] temp;
            temp= GetSend(CCNSysValye.SysInfo, new SLBInfoHead(), this.Encoding.GetBytes(sysinfo));
           //  MergeHeadAndInfo(out temp, CCNValye.TestConnect, sysinfo);
            return  GetSend(temp);
        }
        /// <summary>
        /// 发送消息，自行处理返回或者不需要同步返回
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="sLBBusinessInfo"></param>
        /// <param name="CCN"></param>
        public void Send(Int32 CCN, OMCandZZJInfoHead sLBInfoHead, string sLBBusinessInfo  )
        {
            sLBInfoHead.Token = this.Token;
            byte[] bInfo = this.Encoding.GetBytes(sLBBusinessInfo);
             Send(CCN,sLBInfoHead, bInfo  );
        }
       
        /// <summary>
        /// 发送消息，自行处理返回或者不需要同步返回
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        /// <param name="CCN"></param>
        public void Send(Int32 CCN,SLBInfoHead sLBInfoHead, byte[] bInfo  )
        {
            sLBInfoHead.Token = this.Token;
            if (bInfo.Length > ZipBufferSize)
            {
                bInfo = GZipHelper.Compress(bInfo);
                sLBInfoHead.Zip = 1;
            }
            else
            {
                sLBInfoHead.Zip = 0;
            }

            string strHead = sLBInfoHead.ToString();// JsonConvert.SerializeObject(tCPAssignInfoHead);
            byte[] bHead = this.Encoding.GetBytes(strHead);

            //4+ Hend + Info
            byte[] tmp;
            if (sLBInfoHead.EncryptType == 0)
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo); }
            else//加密
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo, sLBInfoHead.EncryptType, sLBInfoHead.SEID); }
            Send(tmp);
        }
        private   byte[] GetSend(Int32 CCN, SLBInfoHead sLBInfoHead, byte[] bInfo)
        {
            sLBInfoHead.Token = this.Token;
            if (bInfo.Length > ZipBufferSize)
            {
                bInfo = GZipHelper.Compress(bInfo);
                sLBInfoHead.Zip = 1;
            }
            else
            {
                sLBInfoHead.Zip = 0;
            }

            string strHead = sLBInfoHead.ToString();// JsonConvert.SerializeObject(tCPAssignInfoHead);
            byte[] bHead = this.Encoding.GetBytes(strHead);

            //4+ Hend + Info
            byte[] tmp;
            if (sLBInfoHead.EncryptType == 0)
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo); }
            else//加密
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo, sLBInfoHead.EncryptType, sLBInfoHead.SEID); }
            return tmp;
        }

        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="strHead"></param>
        /// <param name="bInfo"></param>
        /// <param name="CCN"></param>
        public void Transpond(Int32 CCN, string strHead, byte[] bInfo)
        {
            
            byte[] bHead = this.Encoding.GetBytes(strHead);

            //4+ Hend + Info
            byte[] tmp;
            MergeHeadAndInfo(out tmp, CCN, bHead, bInfo);   
            Send(tmp);
        }
        public void SendFile(FileInfoHead sLBInfoHead, byte[] File )
        {
            sLBInfoHead.Token = this.Token;
            Int32 CCN = CCNSysValye.SendFile;
            byte[] bInfo= File;

            string strHead = sLBInfoHead.ToString();// JsonConvert.SerializeObject(tCPAssignInfoHead);
            byte[] bHead = this.Encoding.GetBytes(strHead);

            //4+ Hend + Info
            byte[] tmp;

            if (sLBInfoHead.EncryptType == 0)
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo); }
            else//加密
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo, sLBInfoHead.EncryptType, sLBInfoHead.SEID); }

            Send(tmp);
        }

       
        /// <summary>
        /// 发送消息，自行处理返回或者不需要同步返回
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        /// <param name="CCN"></param>
        public void Send(Int32 CCN, OMCandZZJInfoHead oMCandZZJInfo, byte[] bInfo)
        {
            if (bInfo == null)
            {
                bInfo = new byte[1];
            }
            oMCandZZJInfo.Token = this.Token;
            if (bInfo.Length > ZipBufferSize)
            {
                bInfo = GZipHelper.Compress(bInfo);
                oMCandZZJInfo.Zip = 1;
            }
            else
            {
                oMCandZZJInfo.Zip = 0;
            }

            string strHead = oMCandZZJInfo.ToString();// JsonConvert.SerializeObject(tCPAssignInfoHead);
            byte[] bHead = this.Encoding.GetBytes(strHead);

            //4+ Hend + Info
            byte[] tmp;
            if (oMCandZZJInfo.EncryptType == 0)
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo); }
            else//加密
            { MergeHeadAndInfo(out tmp, CCN, bHead, bInfo, oMCandZZJInfo.EncryptType, oMCandZZJInfo.SEID); }
            Send(tmp);
        }

        /// <summary>
        /// 发生包含Head的数据测试
        /// </summary>
        /// <param name="datagram"></param>
        /// <param name="Head"></param>
        protected  void SendHeadTest(string datagram, string Head, Int32 ccn)
        {
            byte[] bInfo = this.Encoding.GetBytes(datagram);
            byte[] bHead = this.Encoding.GetBytes(Head);

            //2+4+ Hend + Info
            byte[] tmp;
            this.MergeHeadAndInfo(out tmp, ccn, bHead, bInfo);
            Send(tmp);
        }




        public bool Send(Int32 CCN, SLBInfoHeadBusS sLBInfoHeadBusS, string sLBBusinessInfo)
        {
            sLBInfoHeadBusS.Token = this.Token;
            string strInfo = JsonConvert.SerializeObject(sLBBusinessInfo);
            byte[] bInfo = this.Encoding.GetBytes(strInfo);
            return Send(CCN, sLBInfoHeadBusS, sLBBusinessInfo);
        }


        public bool Send(Int32 CCN, SLBInfoHeadBusS sLBInfoHeadBusS, byte[] bInfo)
        {
            sLBInfoHeadBusS.Token = this.Token;
            if (bInfo.Length > ZipBufferSize)
            {
                bInfo = GZipHelper.Compress(bInfo);
                sLBInfoHeadBusS.Zip = 1;
            }
            else
            {
                sLBInfoHeadBusS.Zip = 0;
            }
            string strHeadPT = sLBInfoHeadBusS.ToString();// JsonConvert.SerializeObject(tCPAssignInfoHeadPT);
            byte[] bHeadPT = this.Encoding.GetBytes(strHeadPT);

            byte[] tmp;
            if (sLBInfoHeadBusS.EncryptType == 0)
            { MergeHeadAndInfo(out tmp, CCN, bHeadPT, bInfo); }
            else//加密
            { MergeHeadAndInfo(out tmp, CCN, bHeadPT, bInfo, sLBInfoHeadBusS.EncryptType, sLBInfoHeadBusS.SEID); }

            return Send(tmp);
        }

      



        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(string datagram)
        {
            Send(this.Encoding.GetBytes(datagram));
        }

        #endregion

        #region Send 发送并返回
        /// <summary>
        /// 调用服务器的EventWaitHandle队列
        /// </summary>
        ConcurrentDictionary<string, EventWaitHandle> sdWebSocketClientCallServerBackwh = new ConcurrentDictionary<string, EventWaitHandle>();
        /// <summary>
        /// 调用服务器返回的数据队列
        /// </summary>
        ConcurrentDictionary<string, ResultStore> sdWebSocketClientCallServerBack = new ConcurrentDictionary<string, ResultStore>();
        /// <summary>
        /// 发送数据并接收返回数据
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="sBusInfo"></param>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public string Call(Int32 CCN,SLBInfoHead sLBInfoHead, string sBusInfo  )
        {
            byte[] bRInfo = Call(CCN,new OMCandZZJInfoHead(sLBInfoHead.ToString()), this.Encoding.GetBytes(sBusInfo)  );
            string Outdata = this.Encoding.GetString(bRInfo);
            return Outdata;
        }
        /// <summary>
        /// 发送数据并接收返回数据
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="sBusInfo"></param>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public string Call(Int32 CCN, OMCandZZJInfoHead sLBInfoHead, string sBusInfo)
        {
            byte[] bRInfo = Call(CCN, sLBInfoHead, this.Encoding.GetBytes(sBusInfo));
            string Outdata = this.Encoding.GetString(bRInfo);
            return Outdata;
        }
        /// <summary>
        /// 发送数据并接收返回数据
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bSInfo"></param>
        /// <param name="CCN"></param>
        /// <returns></returns>
        public byte[] Call(Int32 CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bSInfo  )
        {
            string Key = NewIdHelper.NewOrderId24;
            sLBInfoHead.TID = Key;
            EventWaitHandle myHandleT = new EventWaitHandle(false, EventResetMode.ManualReset);
            myHandleT.Reset();
            sdWebSocketClientCallServerBackwh.TryAdd(Key, myHandleT);
            Send( CCN,sLBInfoHead, bSInfo);
            myHandleT.WaitOne();
            myHandleT = null;
            ResultStore resultStoreT;
            if (sdWebSocketClientCallServerBack.ContainsKey(Key))
            {
                sdWebSocketClientCallServerBack.TryRemove(Key, out resultStoreT);
                byte[] bRInfo = resultStoreT.bInfo;
                //if (resultStoreT.sLBInfoHead.EncryptType > 0)//加密数据需要进行解密
                //{
                //    bRInfo = SignEncryptHelper.Decrypt(bRInfo, resultStoreT.sLBInfoHead.EncryptType, resultStoreT.sLBInfoHead.SEID);
                //}
                //if (resultStoreT.sLBInfoHead.Zip == 1)//GZip压缩 需要解压
                //{
                //    bRInfo = GZipHelper.Decompress(bRInfo);
                //}
                resultStoreT = null;
                return bRInfo;
            }
            return null;
        }

        /// <summary>
        /// WebSocket 主动调用对方返回结果
        /// </summary>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        protected  void RetWebSocketForCall(OMCandZZJInfoHead sLBInfoHead, byte[] bInfo)
        {
            if (sdWebSocketClientCallServerBackwh.ContainsKey(sLBInfoHead.TID))
            {
                ResultStore resultStore = new ResultStore(CCNSysValye.BusServerToSCToClinet, sLBInfoHead, bInfo);
                sdWebSocketClientCallServerBack.TryAdd(sLBInfoHead.TID, resultStore);
                EventWaitHandle myHandleT;
                sdWebSocketClientCallServerBackwh.TryRemove(sLBInfoHead.TID, out myHandleT);
                myHandleT.Set();
            }
        }
        #endregion
    }
}

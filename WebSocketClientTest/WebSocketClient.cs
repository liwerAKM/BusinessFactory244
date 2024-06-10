using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasS.Base.Lib;
using PaaS.Comm;
using System.Threading;

namespace WebSocketClientTest
{

    public class WebSocketClient
    {
        WebSocket4NetSpring webSocket4NetSpring;

        public void Start()
        {
            SLBInfo sLBInfo = new SLBInfo();////DbHelper.SLBInfoGet("Test108");
            sLBInfo.IP = "192.168.1.108";
            sLBInfo.WebSocketPort = 8910;
            string WebSocketUrl = MyPubConstant.WebSocketSLBUrl;// $"ws://{sLBInfo.IP }:{sLBInfo.WebSocketPort  }";
            webSocket4NetSpring = new WebSocket4NetSpring(WebSocketUrl, sLBInfo.SLBID);
            webSocket4NetSpring.DataRegisterRetEvent += WebSocket4NetSpring_DataRegisterRetEvent;
            webSocket4NetSpring.DataServerSendClientHeadEvent += WebSocket4NetSpring_DataServerSendClientHeadEvent;
            webSocket4NetSpring.ReciveFileInfoEvent += WebSocket4NetSpring_ReciveFileInfoEvent;
            webSocket4NetSpring.ReciveOMC_and_ClientEvent += WebSocket4NetSpring_ReciveOMC_and_ClientEvent;
            // webSocket4NetSpring.ConnecSLBBusServer();
            string zzjID = MyPubConstant.ZZJID;
            SSysRegisterInfo sSysRegisterInfo = new SSysRegisterInfo(RegisterIdentity.ZZJClient, zzjID);
            sSysRegisterInfo.Info = "WebSocketClient";
            webSocket4NetSpring.ConnectandLogin(sSysRegisterInfo);

        }

        private void WebSocket4NetSpring_ReciveOMC_and_ClientEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead Head, byte[] bInfo)
        {
            int HccN = CCN / 10000;//高位CCN 
            int LccN = CCN % 10000;//低位CCN
            if (HccN == HCCNValye.OMC_SLBR_Client || HccN == HCCNValye.OMC_SLB_Client)
            {
                string message = ("收到 控制端和自助机之间通讯:" + Encoding.UTF8.GetString(bInfo));
                Console.WriteLine(message);

                webSocket4NetClent.Send(HCCNValye.Client_SLBR_OMC * 10000 + LccN, Head, bInfo);
            }
        }



        /// <summary>
        /// 服务端主动发送给客户端的信息 需要根据实际情况判断是否需要返回
        /// </summary>
        /// <param name="webSocket4NetClent"></param>
        /// <param name="CCN"></param>
        /// <param name="sLBInfoHead"></param>
        /// <param name="bInfo"></param>
        private void WebSocket4NetSpring_DataServerSendClientHeadEvent(WebSocket4NetSpring webSocket4NetClent, int CCN, OMCandZZJInfoHead sLBInfoHead, byte[] bInfo)
        {
            if (CCN == 10000000)//测试
            {
                string messagerec = webSocket4NetClent.Encoding.GetString(bInfo);
                Console.WriteLine("recive：  " + messagerec);
                string messageret = "客户端返回数据" + DateTime.Now;
                if (webSocket4NetClent != null && webSocket4NetClent.Connected)
                {
                    webSocket4NetClent.Send(CCN, sLBInfoHead, messageret);//Header 和CCN 不变
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// 连接注册返回结果。可根据结果处理
        /// </summary>
        /// <param name="webSocket4NetClent"></param>
        /// <param name="sSysRegisterInfo"></param>
        private void WebSocket4NetSpring_DataRegisterRetEvent(WebSocket4NetSpring webSocket4NetClent, SSysRegisterRetInfo sSysRegisterInfo)
        {
            // throw new NotImplementedException();
        }
        /// <summary>
        /// 收到文件
        /// </summary>
        /// <param name="webSocket4NetClent"></param>
        /// <param name="sRecFileInfo"></param>
        private void WebSocket4NetSpring_ReciveFileInfoEvent(WebSocket4NetSpring webSocket4NetClent, SRecFileInfo sRecFileInfo)
        {
            Console.WriteLine("reciveFile：  " + sRecFileInfo.FileName);
            sRecFileInfo.SaveFile(System.AppDomain.CurrentDomain.BaseDirectory);
        }
        /// <summary>
        /// 测试
        /// </summary>
        public void CallTest()
        {
            int ccn = 20000000;
            // int ccn = 5009999;
            OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead();
            string sendMessage = "客户端发给发给客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端客户端发给发给服务端服务端" + DateTime.Now;
            //    Console.WriteLine("Send:"+ sendMessage);
            string result = webSocket4NetSpring.Call(ccn, sLBInfoHead, sendMessage);
            //  Console.WriteLine("Recive:" + result);
        }
        /// <summary>
        /// 测试
        /// </summary>
        public void Call2001()
        {
            //OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead();
            //var result = webSocket4NetSpring.Call(5010028, sLBInfoHead,   "{\"HOS_ID\":\"8\",\"PAGE_INDEX\":\"1\",\"PAGE_SIZE\":99,\"SEARCHLIST\":[{\"ID\":\"HOS_ID\",\"VALUE\":\"8\",\"EQUAL\":\"1\"}],\"SOURCE\":\"H091\",\"USER_ID\":\"PC124\"}");
            //   int ccn = 20010000;
            int ccn = 5009999;
            OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead();
            sLBInfoHead.Extend = "Extend";
            string sendMessage = "客户端发给发给服务端" + DateTime.Now;
            Console.WriteLine("Send:" + sendMessage);
            string result = webSocket4NetSpring.Call(ccn, sLBInfoHead, sendMessage);
            Console.WriteLine("Recive:" + result);
        }
        /// <summary>
        /// 测试
        /// </summary>
        public void SendTest()
        {
            int ccn = 20000000;
            OMCandZZJInfoHead sLBInfoHead = new OMCandZZJInfoHead();
            string sendMessage = "客户端发给发给服务端" + DateTime.Now;
            Console.WriteLine("Send:" + sendMessage);
            webSocket4NetSpring.Send(ccn, sLBInfoHead, sendMessage);

        }
        /// <summary>
        /// 测试
        /// </summary>
        public void CallT4()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(50);
                new Thread(() =>
                {
                    CallT4T(); //HTTPSTest();//
                }).Start();

                //Task<long> sss = CountCharactersAsync2(i);
                //    txt_log.Text = sss  + "毫秒" + "\r\n" + txt_log.Text;
            }
        }
        void CallT4T()
        {
            for (int i = 0; i < 10000; i++)
            {
                CallTest();
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PasS.Base.Lib;

namespace BusinessInterface
{
    /// <summary>
    ///  工作任务种类<see cref="BusType.Interactive  "/>，可直接继承此类实现对应接口
    ///   类中已经定义好对应函数Finish、ReturnInfo 可直接调用
    /// </summary>
    public abstract class ProcessingBusinessInteractive : ProcessingBusinessBase
    {
      
        EventWaitHandle evaitHandleInteractive { get; set; }

        /// <summary>
        /// 最后返回的结果
        /// </summary>
        protected SLBBusinessInfo FinallReutnInfo
        {
            get;
            set;
        }
        string Keyid;

        bool InteractiveEnd = false;
        bool isClientInteractiveing = false;
        public bool ProcessingBusiness(SLBBusinessInfo sLBBusinessInfo, out SLBBusinessInfo OutBusinessInfo, string Key, out bool Interactive_End)
        {
            Keyid = Key;
            evaitHandleInteractive = new EventWaitHandle(false, EventResetMode.ManualReset);
            OutBusinessInfo = sLBBusinessInfo;
            if (HandingInitialBusiness(sLBBusinessInfo))
            {
                HandingInitialBusinessReturn = true;
                if (FinishReturn)//如果已经接受到结束命令 则 直接结束
                {
                    if (!isClientInteractiveing && !InteractiveEnd)
                    {
                        // Dealloc();
                        evaitHandleInteractive.Set();
                        evaitHandleInteractive.Close();
                        BusServiceAdapter.InteractiveContinueStoC(Keyid, FinallReutnInfo, out OutBusinessInfo, true);
                    }
                }
                else//否则继续等待下面工作
                {
                    evaitHandleInteractive.WaitOne();
                }
                Console.WriteLine(string.Format("{0} WaitOne complete!\r\n", sLBBusinessInfo.TID));
                OutBusinessInfo = FinallReutnInfo;
                Console.WriteLine(string.Format("{0}  ", sLBBusinessInfo.TID));
                Interactive_End = InteractiveEnd; //this.InteractiveEnd;
                return complete;
            }
            else
            {
                evaitHandleInteractive.Set();
                evaitHandleInteractive.Close();
                OutBusinessInfo = FinallReutnInfo;
                Console.WriteLine(string.Format("{0} HandingInitialBusiness Fail !  ", sLBBusinessInfo.TID));
                BusServiceAdapter.InteractiveContinueStoC(Keyid, FinallReutnInfo, out OutBusinessInfo, true);
                Interactive_End = InteractiveEnd;
                return false;
            }

        }
        /// <summary>
        /// 接收客户端发送数据给服务端（后继业务）
        /// </summary>
        /// <param name="sLBBusinessInfo"></param>
        /// <returns></returns>
        public bool InteractiveContinue(SLBBusinessInfo sLBBusinessInfo, out SLBBusinessInfo OutLBBusinessInfo, out bool IsEndTag)
        {
            isClientInteractiveing = true;
            OutLBBusinessInfo = null;
            IsEndTag = false;
            if (sLBBusinessInfo.BusData.Equals("SYSEND."))
            {
                FinishReturn = true;
                Termination();
                IsEndTag = true;

                InteractiveEnd = true;
                OutLBBusinessInfo = FinallReutnInfo;
                evaitHandleInteractive.Set();
                isClientInteractiveing = false;
                return true;
            }
            else
            {
                bool ret = HandingContinueBusiness(sLBBusinessInfo, out OutLBBusinessInfo);
                if (FinishReturn)
                {
                    InteractiveEnd = true;
                    IsEndTag = true;
                    evaitHandleInteractive.Set();
                    evaitHandleInteractive.Close();
                }
                isClientInteractiveing = false;
                return ret;
            }
        }

        /// <summary>
        /// 发送数据给客户端
        /// </summary>
        /// <param name="InsLBBusinessInfo"></param>
        /// <param name="OutLBBusinessInfo"></param>
        /// <returns></returns>
        protected bool ContinueBusinessToClient(SLBBusinessInfo InsLBBusinessInfo, out SLBBusinessInfo OutLBBusinessInfo)
        {
            return BusServiceAdapter.InteractiveContinueStoC(Keyid, InsLBBusinessInfo, out OutLBBusinessInfo, false);
        }

        /// <summary>
        /// 处理初始业务
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>  
        protected abstract bool HandingInitialBusiness(SLBBusinessInfo sLBBusinessInfo);

        /// <summary>
        /// 处理后继业务
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        protected abstract bool HandingContinueBusiness(SLBBusinessInfo sLBBusinessInfo, out SLBBusinessInfo OutLBBusinessInfo);

        /// <summary>
        /// 被终止执行时需要处理的业务
        /// </summary>
        public abstract void Termination();

        protected void Finish(bool Complete)
        {
            if (!FinishReturn)
            {
                FinishReturn = true;
                complete = Complete;
                if (!isClientInteractiveing)
                {
                    InteractiveEnd = false;
                }
                if (HandingInitialBusinessReturn)//首次执行已经结束，可以结束整个任务；
                {
                    if (!isClientInteractiveing)
                    {
                        SLBBusinessInfo OutLBBusinessInfo;
                        BusServiceAdapter.InteractiveContinueStoC(Keyid, FinallReutnInfo, out OutLBBusinessInfo, true);
                        evaitHandleInteractive.Set();
                        evaitHandleInteractive.Close();
                    }
                }
                Console.WriteLine(string.Format("{0} Finish\r\n", ""));
            }
        }


        bool complete = false;
        bool HandingInitialBusinessReturn = false;
        bool FinishReturn = false;
        /// <summary>
        /// 是否已经终止工作
        /// </summary>
        protected bool HasFinish
        {
            get { return FinishReturn; }
        }


        public override bool ProcessingBusiness(string strJson)
        {
            return true;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="strJsonResult"></param>
        /// <returns></returns>
        public override bool ProcessingBusiness(string strJson, out string strJsonResult)
        {

            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo, out SLBBusinessInfo OutBusinessInfo)
        {
            OutBusinessInfo = InBusinessInfo;
            return true;
        }
        public override bool ProcessingBusiness(SLBBusinessInfo InBusinessInfo)
        {

            return true;
        }
        public override bool ProcessingBusiness(string strJson, out string strJsonResult, ref bool AllowPublish, ref string RoutingKey)
        {
            strJsonResult = "";
            return true;
        }
        public override bool ProcessingBusiness(int CCN, byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS,   byte[] In)
        {
            return true;
        }

        public override bool ProcessingBusiness(int CCN, SLBInfoHeadBusS infoHeadBusS, byte[] In, out byte[] Out)
        {
            Out = null;
            return true;
        }
     
    }
}

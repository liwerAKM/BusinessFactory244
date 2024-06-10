using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using PasS.Base.Lib;

namespace PaaS.Comm
{
    /// <summary>
    /// 侦听到服务器发来数据触发事件
    /// </summary>
    /// <param name="Data"></param>
  //  public delegate void NewDataReceivedEventHandler( byte[] value);


    /// <summary>
    /// Spring WebSocket 基础类
    /// </summary>
    public abstract class WebClientBase
    {
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }
        /// <summary>
        /// 通信使用的编码
        /// </summary>
        protected Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// 最大包长度
        /// </summary>
        public const int MaxRequestLength = 200 * 1024;
        /// <summary>
        /// 分割包长度大小 要小于<see cref="MaxRequestLength"/>
        /// </summary>
        public const int SplitLength = 199 * 1024;//

     //   public event NewDataReceivedEventHandler NewDataReceivedEvent;






        protected abstract void SendData(byte[] datagram);
      

        Dictionary<int, byte[]> dicDataRecBuffer = new Dictionary<int, byte[]>();
       

      
        protected  virtual  void RaiseReceived(byte[] receivedBytes)
        {

        }
        public void DataReceived(byte[] value)
        {
            byte[] retdata = DealReciveData(value);
            if (retdata != null)
            {
                RaiseReceived(retdata);
            }
        }
        public byte[] GetNewDataReceived(byte[] value)
        {
            byte[] retdata = DealReciveData(value);
            return retdata;
        }

        public bool Send(byte[] datagram)
        {
            //标志位 1位: 0 不分包; 1 分包首包;2 分包中间包 ；3 分包结束； + 4位长度（+4位分包数据起始位）+ 数据
            if (datagram.Length <= SplitLength)
            {
                Byte Tag = 0;
                byte[] blen = System.BitConverter.GetBytes(datagram.Length);
                byte[] tmp = new byte[1 + 4 + datagram.Length];
                tmp[0] = Tag;
                System.Buffer.BlockCopy(blen, 0, tmp, 1, blen.Length);
                System.Buffer.BlockCopy(datagram, 0, tmp, 1 + blen.Length, datagram.Length);
                // socketSession.Send(tmp, 0, tmp.Length);
                SendData(tmp);
            }
            else
            {
                Byte Tag = 1;
                byte[] blen = System.BitConverter.GetBytes(datagram.Length);//本数据包长度 记录在低1-4位
                int StartLen = 0;//本次开始点
                while (StartLen + SplitLength < datagram.Length)//循环处理非最后一个包
                {
                    byte[] tmp = new byte[1 + 4 + 4 + SplitLength];
                    tmp[0] = Tag;//0位，标志位
                    byte[] bStartINdex = System.BitConverter.GetBytes(StartLen);
                    System.Buffer.BlockCopy(blen, 0, tmp, 1, blen.Length);//本数据包总长度 记录在第1-4位
                    System.Buffer.BlockCopy(bStartINdex, 0, tmp, 1 + blen.Length, bStartINdex.Length);//本次发送数据起点 记录在第5-8位
                    System.Buffer.BlockCopy(datagram, StartLen, tmp, 1 + blen.Length + bStartINdex.Length, SplitLength);//第9位开始 本次实际分包数据
                    //socketSession.Send(tmp, 0, tmp.Length);
                    SendData(tmp);
                    StartLen = StartLen + SplitLength;
                    Tag = 2;
                }
                if (StartLen < datagram.Length && StartLen + SplitLength >= datagram.Length)//最后一个包处理
                {
                    Tag = 3;
                    int remainLen = datagram.Length - StartLen;//剩余数据长度
                    byte[] tmp = new byte[1 + 4 + 4 + remainLen];
                    tmp[0] = Tag;//0位，标志位
                    byte[] bStartINdex = System.BitConverter.GetBytes(StartLen);
                    System.Buffer.BlockCopy(blen, 0, tmp, 1, blen.Length); ;//本数据包总长度 记录在第1-4位
                    System.Buffer.BlockCopy(bStartINdex, 0, tmp, 1 + blen.Length, bStartINdex.Length);//本次发送数据起点 记录在第5-8位
                    System.Buffer.BlockCopy(datagram, StartLen, tmp, 1 + blen.Length + bStartINdex.Length, remainLen);//第9位开始 本次实际分包数据
                    // socketSession.Send(tmp, 0, tmp.Length);
                    SendData(tmp);
                }
            }
            return true;
        }
        protected  byte [] GetSend(byte[] datagram)
        {
            //标志位 1位: 0 不分包; 1 分包首包;2 分包中间包 ；3 分包结束； + 4位长度（+4位分包数据起始位）+ 数据
           
            {
                Byte Tag = 0;
                byte[] blen = System.BitConverter.GetBytes(datagram.Length);
                byte[] tmp = new byte[1 + 4 + datagram.Length];
                tmp[0] = Tag;
                System.Buffer.BlockCopy(blen, 0, tmp, 1, blen.Length);
                System.Buffer.BlockCopy(datagram, 0, tmp, 1 + blen.Length, datagram.Length);
                // socketSession.Send(tmp, 0, tmp.Length);
                return tmp ;
            }
 
        }

        private byte[] DealReciveData(byte[] value)
        {
            if (value.Length > 5)//小于5 的无数据 不处理
            {
                if (value[0] == 0)//单独包
                {
                    byte[] lenBuffer = new byte[4];
                    Buffer.BlockCopy(value, 1, lenBuffer, 0, 4);
                    int thislong = System.BitConverter.ToInt32(lenBuffer, 0);
                    if (value.Length == thislong + 5)//验证一下数据包是否一致
                    {
                        byte[] busBytes = new byte[thislong];//实际的发送过来的业务包
                        Buffer.BlockCopy(value, 5, busBytes, 0, thislong);
                        return busBytes;
                    }
                }
                else if ((value[0] == 1 || value[0] == 2 || value[0] == 3) && value.Length > 9) //多包首包
                {
                    byte[] lenBuffer = new byte[4];//本数据包总长度 记录在第1-4位
                    Buffer.BlockCopy(value, 1, lenBuffer, 0, 4);
                    int thislong = System.BitConverter.ToInt32(lenBuffer, 0);//本数据包总长度  

                    if (value[0] == 1)// 多包首包
                    {
                        byte[] busBytes = new byte[thislong];//实际的发送过来的业务包
                        Buffer.BlockCopy(value, 9, busBytes, 0, value.Length - 9);
                        dicDataRecBuffer.Add(thislong, busBytes);
                    }
                    else if (value[0] == 2 || value[0] == 3)// 多包
                    {
                        byte[] startindexBuffer = new byte[4];
                        Buffer.BlockCopy(value, 5, startindexBuffer, 0, 4);
                        int startindex = System.BitConverter.ToInt32(startindexBuffer, 0);//起始点
                        if (dicDataRecBuffer.ContainsKey(thislong))
                        {
                            byte[] busBytes = dicDataRecBuffer[thislong];
                            Buffer.BlockCopy(value, 9, busBytes, startindex, value.Length - 9);
                            if (value[0] == 3)//最后一个包
                            {
                                dicDataRecBuffer.Remove(thislong);
                                return busBytes;
                            }
                        }
                    }
                }
            }
            return null;
        }












        /// <summary>
        /// 标志位长度
        /// </summary>
       const int MarkerLen = 4;


        /// <summary>
        ///  将字节流2 + Info 分解 用于系统消息
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="CCN"></param>
        /// <param name="SysInfo"></param>
        /// <returns></returns>
        public bool GetHeadAndInfo(byte[] receivedBytes, out Int32 CCN, out string SysInfo)
        {

            CCN = System.BitConverter.ToInt32(receivedBytes, 0);  
            byte[] bSysInfo = new byte[receivedBytes.Length - MarkerLen];
            Buffer.BlockCopy(receivedBytes, MarkerLen, bSysInfo, 0, receivedBytes.Length - MarkerLen);
            SysInfo = this.Encoding.GetString(bSysInfo);
            return true;
        }

        /// <summary>
        /// 将字节流2 + Info 分解并解密 用于系统消息
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="CCN"></param>
        /// <param name="SysInfo"></param>
        /// <param name="EncryptType">加密类别 0:不加密；1:AES-MD5 ; 2:RSA ;3:RSA2 ；(在非0 情况下与SignType对应)</param>
        /// <param name="SEID">加密或签名的对应系统配置的密钥或公钥ID 空或者'def'为默认</param>
        /// <returns></returns>
        public bool GetHeadAndInfo(byte[] receivedBytes, out Int32 CCN, out string SysInfo, int EncryptType, string SEID)
        {
            CCN = System.BitConverter.ToInt32(receivedBytes, 0);
            byte[] bSysInfo = new byte[receivedBytes.Length - MarkerLen];
            Buffer.BlockCopy(receivedBytes, MarkerLen, bSysInfo, 0, receivedBytes.Length - MarkerLen);
            byte[] bSysInfo2 = SignEncryptHelper.Decrypt(bSysInfo, EncryptType, SEID);
            SysInfo = this.Encoding.GetString(bSysInfo2);
            return true;
        }


        /// <summary>
        /// 将字节流4+ Hend + Info 分解
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>
        public bool _GetHeadAndInfo(byte[] receivedBytes, out byte[] bHead, out byte[] bInfo)
        {
            byte[] blenHead = new byte[4];
            Buffer.BlockCopy(receivedBytes, 0, blenHead, 0, 4);
            int lenHead = System.BitConverter.ToInt32(blenHead, 0);
            bHead = new byte[lenHead];
            Buffer.BlockCopy(receivedBytes, 4, bHead, 0, lenHead);
            int lenInfo = receivedBytes.Length - 4 - lenHead;
            bInfo = new byte[lenInfo];
            Buffer.BlockCopy(receivedBytes, 4 + lenHead, bInfo, 0, lenInfo);
            return true;
        }

        /// <summary>
        /// 将字节流MarkerLen+4+ Hend + Info 分解
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="CCN"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>

        public bool GetHeadAndInfo(byte[] receivedBytes, out Int32 CCN, out byte[] bHead, out byte[] bInfo)
        {
            CCN = System.BitConverter.ToInt32(receivedBytes, 0);
            byte[] blenHead = new byte[4];
            Buffer.BlockCopy(receivedBytes, MarkerLen, blenHead, 0, 4);
            int lenHead = System.BitConverter.ToInt32(blenHead, 0);
            bHead = new byte[lenHead];
            Buffer.BlockCopy(receivedBytes, MarkerLen + 4, bHead, 0, lenHead);
            int lenInfo = receivedBytes.Length - MarkerLen - 4 - lenHead;
            bInfo = new byte[lenInfo];
            Buffer.BlockCopy(receivedBytes, MarkerLen + 4 + lenHead, bInfo, 0, lenInfo);
            return true;
        }

        /// <summary>
        /// 将字节流MarkerLen+4+ Hend + Info 分解
        /// </summary>
        /// <param name="receivedBytes"></param>
        /// <param name="CCN"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>

        public bool GetHeadAndInfo(byte[] receivedBytes, out Int32 CCN, out byte[] bHead, out byte[] bInfo, int EncryptType, string SEID)
        {
            CCN = System.BitConverter.ToInt32 (receivedBytes, 0);
            byte[] blenHead = new byte[4];
            Buffer.BlockCopy(receivedBytes, MarkerLen, blenHead, 0, 4);
            int lenHead = System.BitConverter.ToInt32(blenHead, 0);
            bHead = new byte[lenHead];
            Buffer.BlockCopy(receivedBytes, MarkerLen + 4, bHead, 0, lenHead);
            int lenInfo = receivedBytes.Length - MarkerLen - 4 - lenHead;
            byte[] bInfo0 = new byte[lenInfo];
            Buffer.BlockCopy(receivedBytes, MarkerLen + 4 + lenHead, bInfo0, 0, lenInfo);
            bInfo = SignEncryptHelper.Decrypt(bInfo0, EncryptType, SEID);

            return true;
        }

        /// <summary>
        ///  将字节流2 + SysInfo 合并 系统消息用
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="CCN"></param>
        /// <param name="SysInfo"></param>
        /// <returns></returns>
        public bool MergeHeadAndInfo(out byte[] tmp, Int32 CCN, string SysInfo)
        {
            
            byte[] bSysInfo = this.Encoding.GetBytes(SysInfo);
            //2 + Info
            tmp = new byte[MarkerLen + bSysInfo.Length];
            byte[] bCCN= Int32ToBytes(CCN);
            tmp[0] = bCCN[0];
            tmp[1] = bCCN[1];
            tmp[2] = bCCN[2];
            tmp[3] = bCCN[3];
            System.Buffer.BlockCopy(bSysInfo, 0, tmp, MarkerLen, bSysInfo.Length);
            return true;
        }
        /// <summary>
        ///  将字节流MarkerLen + SysInfo 合并并加密 系统消息用
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="CCN"></param>
        /// <param name="SysInfo"></param>
        /// <returns></returns>
        public bool MergeHeadAndInfo(out byte[] tmp, Int32 CCN, string SysInfo, int EncryptType, string SEID)
        {
            byte[] bSysInfo = this.Encoding.GetBytes(SysInfo);
            byte[] bSysInfo2 = SignEncryptHelper.Encrypt(bSysInfo, EncryptType, SEID);
            //2 + Info
            tmp = new byte[MarkerLen + bSysInfo2.Length];
            byte[] bCCN = Int32ToBytes(CCN);
            tmp[0] = bCCN[0];
            tmp[1] = bCCN[1];
            tmp[2] = bCCN[2];
            tmp[3] = bCCN[3];
            System.Buffer.BlockCopy(bSysInfo2, 0, tmp, MarkerLen, bSysInfo2.Length);
            return true;
        }

        /// <summary>
        /// 将字节流4+ Hend + Info 合并
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>
        public bool _MergeHeadAndInfo(out byte[] tmp, byte[] bHead, byte[] bInfo)
        {
            byte[] blenHead = System.BitConverter.GetBytes(bHead.Length);
            //4+ Hend + Info
            tmp = new byte[4 + bHead.Length + bInfo.Length];
            System.Buffer.BlockCopy(blenHead, 0, tmp, 0, blenHead.Length);
            System.Buffer.BlockCopy(bHead, 0, tmp, blenHead.Length, bHead.Length);
            System.Buffer.BlockCopy(bInfo, 0, tmp, blenHead.Length + bHead.Length, bInfo.Length);
            return true;
        }

        /// <summary>
        ///  将字节流MarkerLen+4+ Hend + Info 合并
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="CCN"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>
        public bool MergeHeadAndInfo(out byte[] tmp, Int32  CCN, byte[] bHead, byte[] bInfo)
        {
            byte[] blenHead = System.BitConverter.GetBytes(bHead.Length);
            //2+4+ Hend + Info
            tmp = new byte[MarkerLen + 4 + bHead.Length + bInfo.Length];
            byte[] bCCN = Int32ToBytes(CCN);
            tmp[0] = bCCN[0];
            tmp[1] = bCCN[1];
            tmp[2] = bCCN[2];
            tmp[3] = bCCN[3];
            System.Buffer.BlockCopy(blenHead, 0, tmp, MarkerLen, blenHead.Length);
            System.Buffer.BlockCopy(bHead, 0, tmp, MarkerLen + blenHead.Length, bHead.Length);
            System.Buffer.BlockCopy(bInfo, 0, tmp, MarkerLen + blenHead.Length + bHead.Length, bInfo.Length);
            return true;
        }

        /// <summary>
        ///  将字节流2+4+ Hend + Info 合并
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="CCN"></param>
        /// <param name="bHead"></param>
        /// <param name="bInfo"></param>
        /// <returns></returns>
        public bool MergeHeadAndInfo(out byte[] tmp, Int32 CCN, byte[] bHead, byte[] bInfo, int EncryptType, string SEID)
        {
            byte[] blenHead = System.BitConverter.GetBytes(bHead.Length);
            byte[] bInfo2 = SignEncryptHelper.Encrypt(bInfo, EncryptType, SEID);
            //2+ Info

            //2+4+ Hend + Info
            tmp = new byte[MarkerLen + 4 + bHead.Length + bInfo2.Length];
            byte[] bCCN = Int32ToBytes(CCN);
            tmp[0] = bCCN[0];
            tmp[1] = bCCN[1];
            tmp[2] = bCCN[2];
            tmp[3] = bCCN[3];
            System.Buffer.BlockCopy(blenHead, 0, tmp, MarkerLen, blenHead.Length);
            System.Buffer.BlockCopy(bHead, 0, tmp, MarkerLen + blenHead.Length, bHead.Length);
            System.Buffer.BlockCopy(bInfo2, 0, tmp, MarkerLen + blenHead.Length + bHead.Length, bInfo2.Length);
            return true;
        }

        public static byte[] ShortToBytes(short value)
        {
            byte[] src = new byte[2];

            src[0] = (byte)(value & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            return src;
        }
        public static byte[] Int32ToBytes(Int32 value)
        {
            byte[] src = new byte[4];

            src[0] = (byte)(value & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[2] = (byte)((value >>16) & 0xFF);
            src[3] = (byte)((value >> 24) & 0xFF);
            return src;
        }
    }
}

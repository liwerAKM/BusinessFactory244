using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DB.Core;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OnlineBusHos44_Tran;
using Soft.Core;

namespace POSC2BUnionPay
{

    public class EntranceV1NETPAY
    {
        //测试地址：https://test-api-open.chinaums.com/v1/netpay/bills/
        //正式地址：生产环境：https://api-mop.chinaums.com/v1/netpay/bills/
        /// <summary>
        /// 银联商务接口地址
        /// </summary>
        //static string apiUrl = "https://api-mop.chinaums.com/v1/netpay/bills/";
        //static string apiUrl = "https://test-api-open.chinaums.com/v1/netpay/bills/";

        //异步通知地址（测试）：https://fbo.ztejsapp.cn/Test/Test/SLB/Notify/POSC2BPay/NOTIFY
        private static string notifyUrl = "https://fbo.ztejsapp.cn/SLB/Notify/POSC2BPay/NOTIFY";

        private static string SuccessCode = "SUCCESS";
        public static string DoBusiness(string injson)
        {
            Models.QHModel.Root request = null;
            Models.QHModel.Root response = null;
            try
            {
                request = JsonConvert.DeserializeObject<Models.QHModel.Root>(injson);

                switch (request.ROOT.HEADER.TYPE)
                {
                    case "BACCOUNTCONFIG":
                        response = BACCOUNTCONFIG(request);
                        break;
                    case "GETQRCODE":
                        response = GETQRCODE(request);
                        break;
                    case "GETORDERSTATUS":
                        response = GETORDERSTATUS(request);
                        break;
                    case "PAYREFUND":
                        response = PAYREFUND(request);
                        break;
                    case "PAYCANCEL":
                        response = PAYCANCEL(request);
                        break;
                    case "NOTIFY":
                        response = NOTIFY(request);
                        break;
                    default:
                        {
                            response = new Models.QHModel.Root();
                            response.ROOT = new Models.QHModel.ROOT();
                            response.ROOT.HEADER = request.ROOT.HEADER;
                            Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                            outBody.CLBZ = "6";
                            outBody.CLJG = "无此交易类型";

                            response.ROOT.BODY = outBody;
                        }
                        break;

                }
            }
            catch (Exception ex)//解析失败返回错误结果
            {
                response = new Models.QHModel.Root();
                response.ROOT = new Models.QHModel.ROOT();
                response.ROOT.HEADER = request.ROOT.HEADER;
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "8";
                outBody.CLJG = ex.Message;

                response.ROOT.BODY = outBody;

            }
            return JsonConvert.SerializeObject(response);
        }

        public static Models.QHModel.Root BACCOUNTCONFIG(Models.QHModel.Root request)
        {
            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            try
            {


                Models.QHModel.BACCOUNTCONFIG_IN inBody = JsonConvert.DeserializeObject<Models.QHModel.BACCOUNTCONFIG_IN>(JsonConvert.SerializeObject(request.ROOT.BODY));

                StringBuilder strSql = new StringBuilder();
                strSql.Append(" insert into baccountposc2b(");
                strSql.Append(" HOS_ID,msgSrc,msgSrcId,md5Key,mid,tid,d1,d2");
                strSql.Append(" ) VALUES (");
                strSql.Append(" @HOS_ID,@msgSrc,@msgSrcId,@md5Key,@mid,@tid,@d1,@d2");
                strSql.Append(" ) ON DUPLICATE KEY UPDATE ");
                strSql.Append(" msgSrc=@msgSrc,msgSrcId=@msgSrcId,md5Key=@md5Key,mid=@mid,=@tid,d1=@d1,d2=@d2");
                MySqlParameter[] parameters = {
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@msgSrc", MySqlDbType.VarChar,20),
                    new MySqlParameter("@msgSrcId", MySqlDbType.VarChar,4),
                    new MySqlParameter("@md5Key", MySqlDbType.VarChar,50),
                    new MySqlParameter("@mid", MySqlDbType.VarChar,32),
                    new MySqlParameter("@tid", MySqlDbType.VarChar,32),
                    new MySqlParameter("@d1", MySqlDbType.DateTime),
                    new MySqlParameter("@d2", MySqlDbType.DateTime)
            };
                parameters[0].Value = inBody.HOS_ID;
                parameters[1].Value = inBody.msgSrc;
                parameters[2].Value = inBody.msgSrcId;
                parameters[3].Value = inBody.md5Key;
                parameters[4].Value = inBody.mid;
                parameters[5].Value = inBody.tid;
                parameters[6].Value = DateTime.Now;
                parameters[7].Value = null;
                int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);

                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "0";
                outBody.CLJG = "处理成功";

                response.ROOT.BODY = outBody;
                return response;
            }
            catch (Exception ex)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "8";
                outBody.CLJG = "处理出错：" + ex.Message;

                response.ROOT.BODY = outBody;
                return response;
            }
        }


        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Models.QHModel.Root GETQRCODE(Models.QHModel.Root request)
        {

            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            Models.QHModel.GETQRCODE_IN inBody = JsonConvert.DeserializeObject<Models.QHModel.GETQRCODE_IN>(JsonConvert.SerializeObject(request.ROOT.BODY));

            BaccountConfig baccountConfig = new BaccountConfig(inBody.HOS_ID);
            if (!baccountConfig.init)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "3";
                outBody.CLJG = baccountConfig.initMsg;

                response.ROOT.BODY = outBody;
                return response;
            }

            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            DateTime nowTime = DateTime.Now;
            string billDate = nowTime.ToString("yyyy-MM-dd");

            requestParams.Add("msgId", CreateGuid());
            requestParams.Add("msgSrc", baccountConfig.msgSrc);
            requestParams.Add("msgType", "bills.getQRCode");
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("srcReserve", "");//请求系统预留字段
            requestParams.Add("mid", baccountConfig.mid);
            string tid = baccountConfig.tid;
            if (!string.IsNullOrEmpty(inBody.TID))
            {
                tid = inBody.TID;
            }
            requestParams.Add("tid", tid);
            requestParams.Add("instMid", "QRPAYDEFAULT");
            string billNo = CreateBillNo(baccountConfig.msgSrcId);
            requestParams.Add("billNo", billNo);
            requestParams.Add("billDate", billDate);
            requestParams.Add("billDesc", inBody.BILLDESC);
            requestParams.Add("totalAmount", (inBody.CASH_JE * 100).ToString("0"));
            requestParams.Add("expireTime", nowTime.AddMinutes(5).ToString("yyyy-MM-dd HH:mm:ss"));//过期时间
            requestParams.Add("notifyUrl", notifyUrl);//支付结果通知地址
            requestParams.Add("returnUrl", "");//网页跳转地址

            //二维码ID,针对需要自行生成二维码的情况
            //不自动生成的qrCodeId与billNo不一致，订单关闭时需传qrCodeId，所以此处采用自行生成，保持与billNo一致
            requestParams.Add("qrCodeId", billNo);
            requestParams.Add("systemId", "");//系统ID
                                              //requestParams.Add("signType", "MD5");//签名算法,值为：MD5或 SHA256；若不上送默认为MD5

            string requestJsonString = JsonConvert.SerializeObject(requestParams);
            DateTime intime = DateTime.Now;
            string tradetype = "get-qrcode";
            string responseJsonString = "";
            bool flag = PostRequest(baccountConfig.appid, baccountConfig.appkey, baccountConfig.apiUrl + tradetype, requestJsonString, ref responseJsonString);

            DateTime outtime = DateTime.Now;

            //增加日志
            SqlSugarModel.Posc2bpaylog modlog = new SqlSugarModel.Posc2bpaylog();
            modlog.HOS_ID = inBody.HOS_ID;
            modlog.tradetype = tradetype;
            modlog.billno = billNo;
            modlog.indata = requestJsonString;
            modlog.intime = intime;
            modlog.outdata = responseJsonString;
            modlog.outtime = outtime;
            AddLog(modlog);

            if (!flag)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = responseJsonString;

                response.ROOT.BODY = outBody;
                return response;
            }

            Models.GetQRCodeResult getQRCodeResult = JsonConvert.DeserializeObject<Models.GetQRCodeResult>(responseJsonString);

            if (getQRCodeResult.errCode == SuccessCode)
            {
                Models.QHModel.GETQRCODE_OUT outBody = new Models.QHModel.GETQRCODE_OUT();
                outBody.CLBZ = "0";
                outBody.CLJG = "";
                outBody.QUERYID = getQRCodeResult.billNo;
                outBody.QRCODE = getQRCodeResult.billQRCode;

                response.ROOT.BODY = outBody;
                return response;
            }
            else
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = getQRCodeResult.errCode + ":" + getQRCodeResult.errMsg;

                response.ROOT.BODY = outBody;
                return response;
            }


        }
        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Models.QHModel.Root GETORDERSTATUS(Models.QHModel.Root request)
        {
            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            Models.QHModel.GETORDERSTATUS_IN inBody = JsonConvert.DeserializeObject<Models.QHModel.GETORDERSTATUS_IN>(JsonConvert.SerializeObject(request.ROOT.BODY));

            BaccountConfig baccountConfig = new BaccountConfig(inBody.HOS_ID);
            if (!baccountConfig.init)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "3";
                outBody.CLJG = baccountConfig.initMsg;

                response.ROOT.BODY = outBody;
                return response;
            }
            string billNo = inBody.QUERYID;
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            DateTime nowTime = DateTime.Now;
            string billDate = nowTime.ToString("yyyy-MM-dd");

            requestParams.Add("msgId", CreateGuid());
            requestParams.Add("msgSrc", baccountConfig.msgSrc);
            requestParams.Add("msgType", "bills.query");
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("srcReserve", "");//请求系统预留字段
            requestParams.Add("mid", baccountConfig.mid);
            string tid = baccountConfig.tid;
            if (!string.IsNullOrEmpty(inBody.TID))
            {
                tid = inBody.TID;
            }
            requestParams.Add("tid", tid);
            requestParams.Add("instMid", "QRPAYDEFAULT");
            requestParams.Add("billNo", inBody.QUERYID);
            //requestParams.Add("refundOrderId", "");//当需要当前帐单退货记录的时候上送
            requestParams.Add("billDate", billDate);

            //requestParams.Add("signType", "MD5");//签名算法,值为：MD5或 SHA256；若不上送默认为MD5

            string requestJsonString = JsonConvert.SerializeObject(requestParams);
            DateTime intime = DateTime.Now;
            string tradetype = "query";
            string responseJsonString = "";
            bool flag = PostRequest(baccountConfig.appid, baccountConfig.appkey, baccountConfig.apiUrl + tradetype, requestJsonString, ref responseJsonString);

            DateTime outtime = DateTime.Now;

            //增加日志
            SqlSugarModel.Posc2bpaylog modlog = new SqlSugarModel.Posc2bpaylog();
            modlog.HOS_ID = inBody.HOS_ID;
            modlog.tradetype = tradetype;
            modlog.billno = billNo;
            modlog.indata = requestJsonString;
            modlog.intime = intime;
            modlog.outdata = responseJsonString;
            modlog.outtime = outtime;
            AddLog(modlog);

            if (!flag)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = responseJsonString;

                response.ROOT.BODY = outBody;
                return response;
            }

            Models.BillsQueryResult billsQueryResult = JsonConvert.DeserializeObject<Models.BillsQueryResult>(responseJsonString);

            if (billsQueryResult.errCode == SuccessCode)
            {
                Models.QHModel.GETORDERSTATUS_OUT outBody = new Models.QHModel.GETORDERSTATUS_OUT();
                outBody.CLBZ = "0";
                outBody.CLJG = billsQueryResult.billStatus;
                /*
billStatus:
PAID
UNPAID
REFUND
CLOSED
UNKNOWN

Status:
NEW_ORDER 	新订单
UNKNOWN 	不明确的交易状态
TRADE_CLOSED 	在指定时间段内未支付时关闭的交易；在交易完成全额退款成功时关闭的交易；支付失败的交易。
WAIT_BUYER_PAY 	交易创建，等待买家付款。
TRADE_SUCCESS	支付成功
TRADE_REFUND	订单转入退货流程
*/
                if (billsQueryResult.billStatus == "PAID")
                {
                    outBody.STATUS = "1";
                    /*
                    try
                    {
                        
                        var db = new DbMySQL().Client;
                        SqlSugarModel.Posc2bTran tran = new SqlSugarModel.Posc2bTran();
                        tran.COMM_SN = billsQueryResult.billNo;
                        tran.TXN_TYPE = "01";
                        tran.HOS_ID = inBody.HOS_ID;
                        tran.mid = billsQueryResult.mid;
                        tran.tid = billsQueryResult.tid;

                        tran.instMid = billsQueryResult.instMid;
                        tran.billNo = billsQueryResult.billNo;
                        tran.billQRCode = billsQueryResult.billQRCode;
                        tran.billDate = billsQueryResult.billDate;
                        tran.createTime = billsQueryResult.createTime;

                        tran.billStatus = billsQueryResult.billStatus;
                        tran.billDesc = billsQueryResult.billDesc;
                        tran.totalAmount = FormatHelper.GetInt(billsQueryResult.totalAmount);
                        tran.merName = billsQueryResult.merName;
                        tran.memo = billsQueryResult.memo;

                        tran.notifyId = "";
                        tran.secureStatus = billsQueryResult.secureStatus;
                        tran.completeAmount = FormatHelper.GetInt(billsQueryResult.completeAmount);
                        tran.merOrderId = billsQueryResult.billPayment == null ? "" : billsQueryResult.billPayment.merOrderId;
                        tran.refundOrderId = "";

                        tran.refundStatus = "";
                        tran.notify_time = DateTime.Now;
                        int rows = db.Insertable<SqlSugarModel.Posc2bTran>(tran).ExecuteCommand();
                    }
                    catch(Exception ex) 
                    {

                    }
                    */

                    StringBuilder strSql = new StringBuilder();
                    strSql = new StringBuilder();
                    strSql.Append("insert into posc2b_tran(");
                    strSql.Append("COMM_SN,TXN_TYPE,HOS_ID,mid,tid,instMid,billNo,billQRCode,billDate,createTime,billStatus,billDesc,totalAmount,merName,memo,notifyId,secureStatus,completeAmount,merOrderId,refundOrderId,refundStatus,notify_time)");//
                    strSql.Append(" values (");
                    strSql.Append("@COMM_SN,@TXN_TYPE,@HOS_ID,@mid,@tid,@instMid,@billNo,@billQRCode,@billDate,@createTime,@billStatus,@billDesc,@totalAmount,@merName,@memo,@notifyId,@secureStatus,@completeAmount,@merOrderId,@refundOrderId,@refundStatus,@notify_time)");
                    strSql.Append("  ON DUPLICATE KEY UPDATE notify_time=@notify_time");
                    MySqlParameter[] parameters1 = {
                     new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
                     new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
                     new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,10),
                     new MySqlParameter("@mid", MySqlDbType.VarChar,10),
                     new MySqlParameter("@tid", MySqlDbType.VarChar,32),

                     new MySqlParameter("@instMid", MySqlDbType.VarChar,30),
                     new MySqlParameter("@billNo",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billQRCode",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billDate", MySqlDbType.VarChar,10),
                     new MySqlParameter("@createTime", MySqlDbType.VarChar, 20),


                      new MySqlParameter("@billStatus", MySqlDbType.VarChar,20),
                     new MySqlParameter("@billDesc", MySqlDbType.VarChar,50),
                      new MySqlParameter("@totalAmount", MySqlDbType.Int32,16),
                     new MySqlParameter("@merName", MySqlDbType.VarChar,50),
                     new MySqlParameter("@memo", MySqlDbType.VarChar,50),

                     new MySqlParameter("@notifyId", MySqlDbType.VarChar,30),
                     new MySqlParameter("@secureStatus",MySqlDbType.VarChar,20),
                     new MySqlParameter("@completeAmount",MySqlDbType.Int32,32),
                     new MySqlParameter("@merOrderId", MySqlDbType.VarChar,50),
                     new MySqlParameter("@refundOrderId",MySqlDbType.VarChar,50),

                     new MySqlParameter("@refundStatus",MySqlDbType.VarChar,20),
                     new MySqlParameter("@notify_time", MySqlDbType.DateTime, 20) };

                    parameters1[0].Value = billsQueryResult.billNo;
                    parameters1[1].Value = "01";
                    parameters1[2].Value = inBody.HOS_ID;
                    parameters1[3].Value = billsQueryResult.mid;
                    parameters1[4].Value = billsQueryResult.tid;

                    parameters1[5].Value = billsQueryResult.instMid;
                    parameters1[6].Value = billsQueryResult.billNo;
                    parameters1[7].Value = billsQueryResult.billQRCode;
                    parameters1[8].Value = billsQueryResult.billDate;
                    parameters1[9].Value = billsQueryResult.createTime;

                    parameters1[9].Value = billsQueryResult.createTime;
                    parameters1[10].Value = billsQueryResult.billStatus;
                    parameters1[11].Value = billsQueryResult.billDesc;
                    parameters1[12].Value = FormatHelper.GetInt(billsQueryResult.totalAmount);
                    parameters1[13].Value = billsQueryResult.merName;
                    parameters1[14].Value = billsQueryResult.memo;

                    parameters1[15].Value = "";
                    parameters1[16].Value = billsQueryResult.secureStatus;
                    parameters1[17].Value = FormatHelper.GetInt(billsQueryResult.completeAmount);
                    parameters1[18].Value = billsQueryResult.billPayment == null ? "" : billsQueryResult.billPayment.merOrderId;
                    parameters1[19].Value = "";

                    parameters1[20].Value = "";
                    parameters1[21].Value = DateTime.Now;

                    int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters1);

                }
                else
                {
                    outBody.STATUS = "0";
                }

                response.ROOT.BODY = outBody;
                return response;
            }
            else
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = billsQueryResult.errCode + ":" + billsQueryResult.errMsg;

                response.ROOT.BODY = outBody;
                return response;
            }

        }
        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Models.QHModel.Root PAYREFUND(Models.QHModel.Root request)
        {
            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            Models.QHModel.PAYREFUND_IN inBody = JsonConvert.DeserializeObject<Models.QHModel.PAYREFUND_IN>(JsonConvert.SerializeObject(request.ROOT.BODY));

            BaccountConfig baccountConfig = new BaccountConfig(inBody.HOS_ID);
            if (!baccountConfig.init)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "3";
                outBody.CLJG = baccountConfig.initMsg;

                response.ROOT.BODY = outBody;
                return response;
            }
            string billNo = inBody.QUERYID;
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            DateTime nowTime = DateTime.Now;
            string billDate = nowTime.ToString("yyyy-MM-dd");

            requestParams.Add("msgId", CreateGuid());
            requestParams.Add("msgSrc", baccountConfig.msgSrc);
            requestParams.Add("msgType", "bills.refund");
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("srcReserve", "");//请求系统预留字段
            requestParams.Add("mid", baccountConfig.mid);
            string tid = baccountConfig.tid;
            if (!string.IsNullOrEmpty(inBody.TID))
            {
                tid = inBody.TID;
            }
            requestParams.Add("tid", tid);
            requestParams.Add("instMid", "QRPAYDEFAULT");
            requestParams.Add("billNo", inBody.QUERYID);
            requestParams.Add("billDate", billDate);
            string refundOrderId = CreateBillNo(baccountConfig.msgSrcId);
            requestParams.Add("refundOrderId", refundOrderId);
            string refundAmount = (inBody.CASH_JE * 100).ToString("0");
            requestParams.Add("refundAmount", refundAmount);
            requestParams.Add("refundDesc", "");//退货说明
            //requestParams.Add("signType", "MD5");//签名算法,值为：MD5或 SHA256；若不上送默认为MD5
            string requestJsonString = JsonConvert.SerializeObject(requestParams);
            DateTime intime = DateTime.Now;
            string tradetype = "refund";
            string responseJsonString = "";
            bool flag = PostRequest(baccountConfig.appid, baccountConfig.appkey, baccountConfig.apiUrl + tradetype, requestJsonString, ref responseJsonString);

            DateTime outtime = DateTime.Now;

            //增加日志
            SqlSugarModel.Posc2bpaylog modlog = new SqlSugarModel.Posc2bpaylog();
            modlog.HOS_ID = inBody.HOS_ID;
            modlog.tradetype = tradetype;
            modlog.billno = billNo;
            modlog.indata = requestJsonString;
            modlog.intime = intime;
            modlog.outdata = responseJsonString;
            modlog.outtime = outtime;
            AddLog(modlog);

            if (!flag)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = responseJsonString;

                response.ROOT.BODY = outBody;
                return response;
            }

            Models.BillRefundResult billRefundResult = JsonConvert.DeserializeObject<Models.BillRefundResult>(responseJsonString);

            if (billRefundResult.errCode == SuccessCode)
            {
                Models.QHModel.PAYREFUND_OUT outBody = new Models.QHModel.PAYREFUND_OUT();
                /*
SUCCESS成功
FAIL失败
PROCESSING处理中
UNKNOWN异常
*/
                if (billRefundResult.refundStatus == "SUCCESS")
                {
                    outBody.CLBZ = "0";
                    try
                    {


                        #region 退款记录保存
                        StringBuilder strSql = new StringBuilder();
                        strSql = new StringBuilder();
                        strSql.Append("insert into posc2b_tran(");
                        strSql.Append("COMM_SN,TXN_TYPE,HOS_ID,mid,tid,instMid,billNo,billQRCode,billDate,createTime,billStatus,billDesc,totalAmount,merName,memo,notifyId,secureStatus,completeAmount,merOrderId,refundOrderId,refundStatus,notify_time)");
                        strSql.Append(" values (");
                        strSql.Append("@COMM_SN,@TXN_TYPE,@HOS_ID,@mid,@tid,@instMid,@billNo,@billQRCode,@billDate,@createTime,@billStatus,@billDesc,@totalAmount,@merName,@memo,@notifyId,@secureStatus,@completeAmount,@merOrderId,@refundOrderId,@refundStatus,@notify_time)");
                        strSql.Append("  ON DUPLICATE KEY UPDATE notify_time=@notify_time");
                        MySqlParameter[] parameters1 = {
                     new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
                     new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
                     new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,10),
                     new MySqlParameter("@mid", MySqlDbType.Decimal,10),
                     new MySqlParameter("@tid", MySqlDbType.VarChar,32),

                     new MySqlParameter("@instMid", MySqlDbType.VarChar,30),
                     new MySqlParameter("@billNo",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billQRCode",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billDate", MySqlDbType.VarChar,10),
                     new MySqlParameter("@createTime", MySqlDbType.VarChar, 20),

                      new MySqlParameter("@billStatus", MySqlDbType.VarChar,20),
                     new MySqlParameter("@billDesc", MySqlDbType.VarChar,50),
                     new MySqlParameter("@totalAmount", MySqlDbType.Int32,16),
                     new MySqlParameter("@merName", MySqlDbType.VarChar,50),
                     new MySqlParameter("@memo", MySqlDbType.VarChar,50),

                     new MySqlParameter("@notifyId", MySqlDbType.VarChar,30),
                     new MySqlParameter("@secureStatus",MySqlDbType.VarChar,20),
                     new MySqlParameter("@completeAmount",MySqlDbType.Int32,32),
                     new MySqlParameter("@merOrderId", MySqlDbType.VarChar,50),
                     new MySqlParameter("@refundOrderId",MySqlDbType.VarChar,50),
                     new MySqlParameter("@refundStatus",MySqlDbType.VarChar,20),
                     new MySqlParameter("@notify_time", MySqlDbType.DateTime, 20) };

                        parameters1[0].Value = billRefundResult.billNo;
                        parameters1[1].Value = "02";
                        parameters1[2].Value = inBody.HOS_ID;
                        parameters1[3].Value = billRefundResult.mid;
                        parameters1[4].Value = billRefundResult.tid;
                        parameters1[5].Value = billRefundResult.instMid;
                        parameters1[6].Value = billRefundResult.billNo;
                        parameters1[7].Value = billRefundResult.billQRCode;
                        parameters1[8].Value = billRefundResult.billDate;
                        parameters1[9].Value = billRefundResult.refundPayTime;
                        parameters1[10].Value = billRefundResult.billStatus;
                        parameters1[11].Value = "";
                        parameters1[12].Value = refundAmount;
                        parameters1[13].Value = null;
                        parameters1[14].Value = null;
                        parameters1[15].Value = billRefundResult.msgId;
                        parameters1[16].Value = null;
                        parameters1[17].Value = null;
                        parameters1[18].Value = billRefundResult.merOrderId;
                        parameters1[19].Value = billRefundResult.refundOrderId;
                        parameters1[20].Value = billRefundResult.refundStatus;
                        parameters1[21].Value = DateTime.Now;

                        int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters1);
                        #endregion
                    }
                    catch (Exception ex)
                    { }
                }
                outBody.CLJG = billRefundResult.refundStatus;


                response.ROOT.BODY = outBody;
                return response;
            }
            else
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = billRefundResult.errCode + ":" + billRefundResult.errMsg;

                response.ROOT.BODY = outBody;
                return response;
            }

        }
        /// <summary>
        /// 订单关闭
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Models.QHModel.Root PAYCANCEL(Models.QHModel.Root request)
        {
            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            Models.QHModel.PAYCANCEL_IN inBody = JsonConvert.DeserializeObject<Models.QHModel.PAYCANCEL_IN>(JsonConvert.SerializeObject(request.ROOT.BODY));

            BaccountConfig baccountConfig = new BaccountConfig(inBody.HOS_ID);
            if (!baccountConfig.init)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "3";
                outBody.CLJG = baccountConfig.initMsg;

                response.ROOT.BODY = outBody;
                return response;
            }
            string billNo = inBody.QUERYID;
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            DateTime nowTime = DateTime.Now;
            string billDate = nowTime.ToString("yyyy-MM-dd");

            requestParams.Add("msgId", CreateGuid());
            requestParams.Add("msgSrc", baccountConfig.msgSrc);
            requestParams.Add("msgType", "bills.closeQRCode");
            requestParams.Add("requestTimestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            requestParams.Add("srcReserve", "");//请求系统预留字段
            requestParams.Add("mid", baccountConfig.mid);
            string tid = baccountConfig.tid;
            if (!string.IsNullOrEmpty(inBody.TID))
            {
                tid = inBody.TID;
            }
            requestParams.Add("tid", tid);
            requestParams.Add("instMid", "QRPAYDEFAULT");

            requestParams.Add("qrCodeId", inBody.QUERYID);
            requestParams.Add("attachRefund", "true");//是否关闭二维码的同时发起退货

            requestParams.Add("systemId", "");//系统ID
            //requestParams.Add("signType", "MD5");//签名算法,值为：MD5或 SHA256；若不上送默认为MD5
            string requestJsonString = JsonConvert.SerializeObject(requestParams);
            DateTime intime = DateTime.Now;
            string tradetype = "close-qrcode";
            string responseJsonString = "";
            bool flag = PostRequest(baccountConfig.appid, baccountConfig.appkey, baccountConfig.apiUrl + tradetype, requestJsonString, ref responseJsonString);

            DateTime outtime = DateTime.Now;

            //增加日志
            SqlSugarModel.Posc2bpaylog modlog = new SqlSugarModel.Posc2bpaylog();
            modlog.HOS_ID = inBody.HOS_ID;
            modlog.tradetype = tradetype;
            modlog.billno = billNo;
            modlog.indata = requestJsonString;
            modlog.intime = intime;
            modlog.outdata = responseJsonString;
            modlog.outtime = outtime;
            AddLog(modlog);

            if (!flag)
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = responseJsonString;

                response.ROOT.BODY = outBody;
                return response;
            }

            Models.BillCloseResult billCloseResult = JsonConvert.DeserializeObject<Models.BillCloseResult>(responseJsonString);

            if (billCloseResult.errCode == SuccessCode)
            {
                Models.QHModel.PAYCANCEL_OUT outBody = new Models.QHModel.PAYCANCEL_OUT();
                outBody.CLBZ = "0";
                outBody.CLJG = "";

                response.ROOT.BODY = outBody;
                return response;
            }
            else
            {
                Models.QHModel.BODY_OUT outBody = new Models.QHModel.BODY_OUT();
                outBody.CLBZ = "2";
                outBody.CLJG = billCloseResult.errCode + ":" + billCloseResult.errMsg;

                response.ROOT.BODY = outBody;
                return response;
            }

        }

        /// <summary>
        /// 异步通知
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Models.QHModel.Root NOTIFY(Models.QHModel.Root request)
        {
            Models.QHModel.Root response = new Models.QHModel.Root();
            response.ROOT = new Models.QHModel.ROOT();
            response.ROOT.HEADER = request.ROOT.HEADER;

            Models.Notify notify = JsonConvert.DeserializeObject<Models.Notify>(JsonConvert.SerializeObject(request.ROOT.BODY));


            StringBuilder strSql0 = new StringBuilder();
            strSql0.Append("select HOS_ID from baccountposc2b where mid=@mid ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@mid", MySqlDbType.VarChar,10)          };
            parameters[0].Value = notify.mid;

            DataTable dt = DbHelperMySQL.Query(strSql0.ToString(), parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                notify.HOS_ID = dt.Rows[0]["HOS_ID"].ToString().Trim();
            }
            notify.COMM_SN = notify.billNo;
            notify.TXN_TYPE = "01";//01收 02退

            /*
            var db = new DbMySQL().Client;
            SqlSugarModel.Posc2bTran tran = new SqlSugarModel.Posc2bTran();
            tran.COMM_SN = notify.billNo;
            tran.TXN_TYPE = notify.TXN_TYPE;
            tran.HOS_ID = notify.HOS_ID;
            tran.mid = notify.mid;
            tran.tid = notify.tid;

            tran.instMid = notify.instMid;
            tran.billNo = notify.billNo;
            tran.billQRCode = notify.billQRCode;
            tran.billDate = notify.billDate;
            tran.createTime = notify.createTime;

            tran.billStatus = notify.billStatus;
            tran.billDesc = notify.billDesc;
            tran.totalAmount = FormatHelper.GetInt(notify.totalAmount);
            tran.merName = notify.merName;
            tran.memo = notify.memo;

            tran.notifyId = notify.notifyId;
            tran.secureStatus = notify.secureStatus;
            tran.completeAmount = FormatHelper.GetInt(notify.completeAmount);
            tran.merOrderId = null;
            tran.refundOrderId = "";

            tran.refundStatus = "";
            tran.notify_time = DateTime.Now;
            int rows = db.Insertable<SqlSugarModel.Posc2bTran>(tran).ExecuteCommand();
            */

            StringBuilder strSql = new StringBuilder();
            strSql = new StringBuilder();
            strSql.Append("insert into posc2b_tran(");
            strSql.Append("COMM_SN,TXN_TYPE,HOS_ID,mid,tid,instMid,billNo,billQRCode,billDate,createTime,billStatus,billDesc,totalAmount,merName,memo,notifyId,secureStatus,completeAmount,notify_time)");
            strSql.Append(" values (");
            strSql.Append("@COMM_SN,@TXN_TYPE,@HOS_ID,@mid,@tid,@instMid,@billNo,@billQRCode,@billDate,@createTime,@billStatus,@billDesc,@totalAmount,@merName,@memo,@notifyId,@secureStatus,@completeAmount,@notify_time)");
            MySqlParameter[] parameters1 = {
                     new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
                     new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
                     new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,10),
                     new MySqlParameter("@mid", MySqlDbType.Decimal,10),
                     new MySqlParameter("@tid", MySqlDbType.VarChar,32),

                     new MySqlParameter("@instMid", MySqlDbType.VarChar,30),
                     new MySqlParameter("@billNo",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billQRCode",MySqlDbType.VarChar,30),
                     new MySqlParameter("@billDate", MySqlDbType.VarChar,10),
                     new MySqlParameter("@createTime", MySqlDbType.VarChar, 20),

                      new MySqlParameter("@billStatus", MySqlDbType.VarChar,20),
                     new MySqlParameter("@billDesc", MySqlDbType.VarChar,50),
                     new MySqlParameter("@totalAmount", MySqlDbType.Int32,16),
                     new MySqlParameter("@merName", MySqlDbType.VarChar,50),
                     new MySqlParameter("@memo", MySqlDbType.VarChar,50),

                     new MySqlParameter("@notifyId", MySqlDbType.VarChar,30),
                     new MySqlParameter("@secureStatus",MySqlDbType.VarChar,20),
                     new MySqlParameter("@completeAmount",MySqlDbType.Int32,32),
                     new MySqlParameter("@notify_time", MySqlDbType.DateTime, 20) };
            parameters1[0].Value = notify.COMM_SN;
            parameters1[1].Value = notify.TXN_TYPE;
            parameters1[2].Value = notify.HOS_ID;
            parameters1[3].Value = notify.mid;
            parameters1[4].Value = notify.tid;
            parameters1[5].Value = notify.instMid;
            parameters1[6].Value = notify.billNo;
            parameters1[7].Value = notify.billQRCode;
            parameters1[8].Value = notify.billDate;
            parameters1[9].Value = notify.createTime;
            parameters1[10].Value = notify.billStatus;
            parameters1[11].Value = notify.billDesc;
            parameters1[12].Value = notify.totalAmount;
            parameters1[13].Value = notify.merName;
            parameters1[14].Value = notify.memo;
            parameters1[15].Value = notify.notifyId;
            parameters1[16].Value = notify.secureStatus;
            parameters1[17].Value = notify.completeAmount;
            parameters1[18].Value = notify.notify_time;
          
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters1);
            
            if (rows > 0)
            {
                Models.QHModel.NOTIFY_OUT outBody = new Models.QHModel.NOTIFY_OUT();
                outBody.CLBZ = "0";
                outBody.CLJG = "";

                response.ROOT.BODY = outBody;
                return response;
            }
            else
            {
                Models.QHModel.NOTIFY_OUT outBody = new Models.QHModel.NOTIFY_OUT();
                outBody.CLBZ = "1";
                outBody.CLJG = "支付数据保存失败";

                response.ROOT.BODY = outBody;
                return response;
            }

        }

        /// <summary>
        /// 生成账单号
        /// </summary>
        /// <returns></returns>
        private static string CreateBillNo(string msgSrcId)
        {
            //{来源编号(4位)}{时间(yyyyMMddmmHHssSSS)(17位)}{7位随机数}
            return msgSrcId + DateTime.Now.ToString("yyyyMMddHHmmssfff") + (new Random().Next(9999999)).ToString("D7");
        }

        private static string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 发送HTTP POST请求
        /// </summary>
        /// <param name="requestJsonString"></param>
        /// <param name="apiUrl"></param>
        /// <returns>请求返回的数据</returns>
        public static bool PostRequest(string appid, string appkey, string apiUrl, string requestdata, ref string responsedata)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string nonce = DateTime.Now.ToString("yyyyMMddHHmmss");

            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] retVal = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(requestdata));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(appkey);
            byte[] messageBytes = encoding.GetBytes(appid + timestamp + nonce + sb.ToString());
            string authorization;
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string signature = Convert.ToBase64String(hashmessage);
                authorization = "OPEN-BODY-SIG AppId=\"" + appid + "\", Timestamp=\"" + timestamp + "\", Nonce=\"" + nonce + "\", Signature=\"" + signature + "\"";
            }

            byte[] strReqStrByte = Encoding.UTF8.GetBytes(requestdata);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.Timeout = 60000;
            request.ContentType = "application/json";
            request.ContentLength = strReqStrByte.Length;
            request.Headers.Add("Authorization", authorization);

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(strReqStrByte, 0, strReqStrByte.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            responsedata = reader.ReadToEnd();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        public static void AddLog(SqlSugarModel.Posc2bpaylog modlog)
        {
            try
            {

                var db = new DbMySQLLog().Client;
                db.Insertable<SqlSugarModel.Posc2bpaylog>(modlog).ExecuteCommand();

            }
            catch (Exception ex)
            {

                try
                {
                    string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "posc2bpaylog");
                    if (!Directory.Exists(LogPath))
                    {
                        Directory.CreateDirectory(LogPath);
                    }
                    StreamWriter sr1 = new StreamWriter(Path.Combine(LogPath, "log" + DateTime.Now.ToString("yyyyMMdd") + ".log"), true);
                    sr1.WriteLine("入参[" + modlog.intime.ToString("yyyy-MM-dd HH:mm:ss") + "]：" + modlog.indata);
                    sr1.WriteLine("出参[" + modlog.outtime.ToString("yyyy-MM-dd HH:mm:ss") + "]：" + modlog.outdata);
                    sr1.WriteLine("--------------------------------------------------");//50个-
                    sr1.Close();
                }
                catch
                {
                }
            }
        }
    }
}

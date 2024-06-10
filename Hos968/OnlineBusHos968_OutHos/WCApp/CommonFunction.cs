using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Net;

namespace OnlineBusHos968_OutHos.WCApp
{
    public struct AgeAndUnit
    {
        public int Age;
        public string AgeUnit;
        public string SEX;
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDay;
        public bool TrueDate;

    }

    /// <summary>
    /// qrcode解析入参实体
    /// </summary>
    public class QrCodeAnalysisData
    {
        public string appSign { get; set; }
        public long timestamp { get; set; }
        public string pack { get; set; }
        public string qrCode { get; set; }

    }

    /// <summary>
    /// 病人基本信息
    /// </summary>
    public class HealthCardInfoData
    {
        public string virtualCardNum { get; set; }
        public string realname { get; set; }//姓名
        public string idNumber { get; set; }//身份证
        public string cellphone { get; set; }//手机号
        public string gender { get; set; }
        public string pin { get; set; }
        public string addr { get; set; }//地址



    }


    /// <summary>
    /// 接口返回（病人信息） 无法使用接口
    /// </summary>
    public class RtnModel
    {

        public string code { get; set; }
        public string message { get; set; }
        public HealthCardInfoData data { get; set; }
    }

    public class SLBRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string Param { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string user_id { get; set; }

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
        public string CTag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubBusID { get; set; }

        public string ReslutCode { get; set; }

        public string ResultMessage { get; set; }

    }

    public class GETSATISURL
    {
        public string GROUP_ID { get; set; }
        public string ITEM_ID { get; set; }
        public string GUID { get; set; }
        public string MODULE_ID { get; set; }
        public string SFZ_NO { get; set; }
        public string PAT_NAME { get; set; }
        public string MOBILE_NO { get; set; }
    }

    public class GETSATISURL_Response
    {
        public string CLBZ { get; set; }
        public string CLJG { get; set; }
        public string URL { get; set; }
        public string EVAL { get; set; }
    }

    /// <summary>
    /// 公用方法
    /// </summary>
    public class CommonFunction
    {
        /// <summary>
        /// 获取电子健康卡信息
        /// </summary>
        /// <param name="qrcode">二维码信息</param>
        /// <returns></returns>
        public static RtnModel GetHealthCardInfo(string qrcode)
        {
            //计算时间戳，是指格林威治时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总秒数，能够唯一地标识某一刻的时间
            long iTmp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            //固定值
            string appKey = "07c59ecff78e4273b16d10564da717da";
            string pack = "cn.ac.sec.doctor";

            string[] arr = { appKey, pack, "" + iTmp };
            System.Array.Sort(arr);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (String param in arr)
            {
                sb.Append(param);
            }

            string cl = sb.ToString();


            //MD5转码
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(cl));
            string pwd = "";
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X2");
            }

            //qrcode = "B6B5BD9DF3714114894865A58863235697FE66A2DEC14E96A8454CA642AD8349:1";
            //qrcode = "BFFD6474929A473EA034FE70E15696E3F1F920FFAD274CBB82CDE8C5BB6AFE9E:1";//潘路强
            //qrcode = "6D65E91EAFE8444AAB213A2F53030E878B83345C16D94E50ABAABF1090B53DDA:1";//杜邦

            QrCodeAnalysisData data = new QrCodeAnalysisData();
            data.appSign = pwd;
            data.timestamp = iTmp;
            data.pack = "cn.ac.sec.doctor";
            data.qrCode = qrcode;
            //string RequestPara = JsonConvert.SerializeObject(data);
            string RequestPara = string.Format("appSign={0}&timestamp={1}&pack={2}&qrCode={3}", pwd, iTmp, "cn.ac.sec.doctor", qrcode);

            /*
{
    "data":{
        "virtualCardNum":"5420120170600000002",
        "realname":"李明",
        "idNumber":"110108188305065433",
        "cellphone":"15833064659",
        "gender":1,
        "pin":1234,
        "addr":"无锡?XX 区 XX 街道"
    },
    "code":0,
    "msg":""
}
             */
            // 创建WebRequest调用对象
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\bin\ServiceBUS.dll.config");
            DataSet ds = XMLHelper.X_GetXmlData(doc, "configuration/appSettings");//请求的数据包

            string Url = ds.Tables[0].Rows[5]["value"].ToString().Trim();
            //string Url = "http://192.168.10.48:13314/hospital/v2/user/qrCodeAnalysis";

            WebRequest request = HttpWebRequest.Create(Url);

            // 数据编码为键值对
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            //request.Timeout = 800;//请求超时时间
            // 将调用数据转换为字节数组
            byte[] buf = Encoding.GetEncoding("UTF-8").GetBytes(RequestPara);
            // 设置HTTP头，提交的数据长度
            request.ContentLength = buf.Length;

            // 写入参数内容
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buf, 0, buf.Length);
                requestStream.Close();

            }



            // 调用返回内容
            string ReturnVal = "";
            // 发起调用操作
            WebResponse response = request.GetResponse();
            // 响应数据流
            Stream stream = response.GetResponseStream();
            // 以UTF-8编码转换为StreamReader
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
            {
                ReturnVal = reader.ReadToEnd();
            }
            HealthCardInfoData healthCardInfoData = new HealthCardInfoData();

            //Newtonsoft.Json.JsonConvert.DeserializeObject<RtnModel>(ReturnVal);

            //ServiceBUS.Log.LogHelper.SaveLogZZJ(DateTime.Now, "1", DateTime.Now, ReturnVal);

            // 读取至结束
            return Newtonsoft.Json.JsonConvert.DeserializeObject<RtnModel>(ReturnVal);

        }






        public static void POSTF()
        {

        }

        //将DataSet转换为xml对象字符串
        public static string ConvertDataSetToXML(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                writer = new XmlTextWriter(stream, Encoding.Unicode);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                UnicodeEncoding utf = new UnicodeEncoding();
                return utf.GetString(arr).Trim();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }


        /// <summary>
        /// 根据身份证号获取年龄
        /// </summary>
        /// <param name="SFZ"></param>
        /// <returns></returns>
        public static AgeAndUnit GetAgeBySFZ(string SFZ)
        {
            AgeAndUnit ageunit = new AgeAndUnit();
            int Age = 0;
            string Ageunit = "";
            string sex = "";
            if (SFZ.Trim().Length == 15 || SFZ.Trim().Length == 18)
            {
                try
                {
                    string NY = "";
                    if (SFZ.Trim().Length == 18)
                    {
                        NY = SFZ.Trim().Substring(6, 8);
                        NY = NY.Substring(0, 4) + "-" + NY.Substring(4, 2) + "-" + NY.Substring(6, 2) + " 00:00:00";
                        sex = SFZ.Substring(14, 3);
                    }
                    else
                    {
                        NY = SFZ.Trim().Substring(6, 6);
                        NY = "19" + NY.Substring(0, 2) + "-" + NY.Substring(2, 2) + "-" + NY.Substring(4, 2) + " 00:00:00";
                        sex = SFZ.Substring(12, 3);
                    }
                    DateTime csny = Convert.ToDateTime(NY);
                    ageunit.BirthDay = csny;
                    Age = DateTime.Now.Year - csny.Year;
                    if (Age > 0)
                    {
                        if (DateTime.Now.Month < csny.Month || (DateTime.Now.Month == csny.Month && DateTime.Now.Day < csny.Day))
                        {
                            Age--;
                        }
                        Ageunit = "岁";
                    }
                    else
                    {
                        Age = DateTime.Now.Month - csny.Month;
                        if (Age > 0)
                        {
                            Ageunit = "月";
                        }
                        else
                        {
                            Age = DateTime.Now.Day - csny.Day;
                            if (Age > 0)
                            {
                                Ageunit = "日";
                            }
                            else
                            {
                                Age = 0;
                                Ageunit = "";
                            }
                        }
                    }
                    ageunit.TrueDate = true;
                }
                catch
                {
                    ageunit.TrueDate = false;
                }

            }
            if (int.Parse(sex) % 2 == 0)
            {
                sex = "女";
            }
            else
            {
                sex = "男";
            }
            ageunit.Age = Age;
            ageunit.AgeUnit = Ageunit;
            ageunit.SEX = sex;
            return ageunit;
        }

        //*********************************************************************************
        //将15位身份证转换成18位时，首先把出生年扩展4位，就是在原来15位号码的第6为数字后
        //增加一个19，然后在第17位数字后添加一位校验码，校验码是由前17位数字本体码加权求
        //和公式，通过计算模，再通过模得到对应的校验码。
        //计算校验码公式
        //（1）17位数字本体码加权求和公式
        //S=Sum(Ai*Wi),i=0,...,16,先对前17位数字的权求和
        //Ai:表示第i位置上的身份证号码数字值
        //Wi:表示第i位置上的加权因子
        //Wi;7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2
        //
        //（2）计算模
        //Y=mod(S,11)
        //（3）通过模得到对应的校验码
        //Y:0 1 2 3 4 5 6 7 8 9 10
        //校验码：1 0 X 9 8 7 6 5 4 3 2
        //*****************************************************************************
        /// <summary>
        /// 将15位的身份证号码转换成18位的身份证好码
        /// </summary>
        /// <param name="idCard">身份证号码</param>
        /// <returns>返回18位身份证号码</returns>
        public static string Convert15to18(string idCard)
        {
            string code = idCard.Trim();//获得身份证号码
            if (code.Length == 15)//如果是15位则转换
            {
                char[] strJY = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
                int[] intJQ = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                string strTemp;
                int intTemp = 0;
                strTemp = code.Substring(0, 6) + "19" + code.Substring(6);
                for (int i = 0; i <= strTemp.Length - 1; i++)
                {
                    intTemp = intTemp + int.Parse(strTemp.Substring(i, 1)) * intJQ[i];
                }
                intTemp = intTemp % 11;
                return strTemp + strJY[intTemp];
            }
            else
            {
                if (code.Length == 18)//如果是18位直接返回
                {
                    return code;
                }
                return string.Empty;//如果即不是15位也不是18位则返回空
            }
        }

        /// <summary>
        /// 判断是否为惠山区医院
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <returns></returns>
        public static bool ISHSYY(string HOS_ID)
        {
            if (int.Parse(HOS_ID) >= 17 && int.Parse(HOS_ID) <= 28)
            {
                return true;
            }
            return false;
        }

        /// <summary>   
        /// 根据条件过滤表   
        /// </summary>   
        /// <param name="dt">未过滤之前的表</param>   
        /// <param name="filter">过滤条件</param>   
        /// <returns>返回过滤后的表</returns>   
        static public DataTable GetNewTable(DataTable dt, string filter)
        {
            #region 根据条件过滤表
            DataTable newTable = dt.Clone();
            DataRow[] drs = dt.Select(filter);
            foreach (DataRow dr in drs)
            {
                object[] arr = dr.ItemArray;
                DataRow newrow = newTable.NewRow();
                for (int i = 0; i < arr.Length; i++)
                    newrow[i] = arr[i];
                newTable.Rows.Add(newrow);
            }
            return newTable;
            #endregion
        }



        public static string GetTips(string FLAG)
        {
            string tips = "";//优化内容
            string Default = "";//原始内容
            switch (FLAG)
            {
                case "GETRISREPORT_NODATA"://记录查询->检查记录->无数据提示
                    tips = "温馨提示：暂无检查报告，请稍后！";
                    Default = "没有找到病人检查报告";
                    break;
                case "GETLISREPORT_NODATA"://记录查询->检验记录->无数据提示
                    tips = "温馨提示：暂无检验报告，请稍后！";
                    Default = "没有找到检验报告单记录";
                    break;
                case "ADDPATINFO_SFZ"://我的卡号->添加持卡人->身份证格式不对
                    tips = "温馨提示：身份证号码输入有误，请您重新输入";
                    Default = "身份证号码格式不正确";
                    break;
                case "ADDPATINFO_SAVE"://我的卡号->添加持卡人->保存失败
                    tips = "温馨提示：添加持卡人失败，请您重试！";
                    Default = "添加持卡人失败";
                    break;
                case "EDITPATINFO_SAVE"://我的卡号->选择持卡人->修改保存失败
                    tips = "温馨提示：修改持卡人失败，请您重试！";
                    Default = "修改持卡人失败";
                    break;
                case "DELPATINFO_RECORD"://我的卡号->选择持卡人->点击删除->今天及以后有预约挂号记录的人不能删除
                    tips = "温馨提示：尚有未就诊记录，您无法删除！";
                    Default = "您有挂号记录未处理";
                    break;
                case "DELPATINFO_SAVE"://我的卡号->选择持卡人->点击删除->删除持卡人失败
                    tips = "温馨提示：删除持卡人失败，请您重试！";
                    Default = "删除持卡人失败";
                    break;
                case "GETPATYLCARD_NODATA"://我的卡号->选择持卡人->没有添加卡时提示
                    tips = "温馨提示：尚未添加就诊卡，请您添加！";
                    Default = "未取到医疗卡列表信息";
                    break;
                case "GETGOODSMX_NODATA"://导医服务->物价公示->选择项目->没有明细
                    tips = "温馨提示：尚未公布物价明细信息，请您等待更新！";
                    Default = "未能获取物价明细列表";
                    break;
                case "GETGOODSCATE_NODATA"://导医服务->物价公示->没有数据
                    tips = "温馨提示：尚未公布物价明细，请您等待更新！";
                    Default = "未能找到物价分类信息";
                    break;
                case "GETSCHPERIOD_NODATA"://预约挂号->选择科室->排班数据为空
                    tips = "温馨提示：尚未公布排班信息，请您等待更新！";
                    Default = "没有排班记录";
                    break;
                case "GETHOSPDEPT_NODATA"://预约挂号->选择科室->科室为空（此处科室根据排班列出来）
                    tips = "温馨提示：尚未公布科室信息，请您等待更新！";
                    Default = "没有排班信息";
                    break;
                case "GETDOCINFO_NODATA"://预约挂号，实时叫号，搜索医生等，传入的条件没有找到对应的医生
                    tips = "温馨提示：尚未公布医生信息，请您等待更新！";
                    Default = "没有找到对应的医生信息";
                    break;
                case "GETHOSPITAL_NODATA"://登录时没有医院
                    tips = "温馨提示：即将上线，请您等待更新！";
                    Default = "没有找到对应的医院信息";
                    break;
                case "GETMIPINFO_NODATA"://导医服务->医保政策
                    tips = "温馨提示：尚未公布医保政策内容，请您等待更新！";
                    Default = "医院对应医保政策不存在";
                    break;
                case "GETMIPINFOMX_NODATA"://导医服务->医保政策->查看内容（目前用不到）
                    tips = "温馨提示：尚未公布医保政策详细内容，请您等待更新！";
                    Default = "对应医保政策内容不存在";
                    break;
                case "GETBULID_NODATA"://信息公告
                    tips = "温馨提示：尚未公布医院公告，请您等待更新！";
                    Default = "没有医院公告信息";
                    break;
                case "GETFB_SAVE"://导医服务>意见反馈->保存失败
                    tips = "温馨提示：反馈失败，请您重试！";
                    Default = "反馈意见保存失败";
                    break;






                //第二批
                case "GETDISCLAIMER_NODATA"://导医服务->免责声明
                    tips = "温馨提示：尚未公布免责声明，请您等待更新！";
                    Default = "医院暂无免责声明";
                    break;
                case "DELYLCARD_RECORD"://我的卡号->选择持卡人->删除医疗卡号，但今天及以后有预约挂号记录的人不能删除
                    tips = "温馨提示：尚有未就诊记录，您无法删除！";
                    Default = "您有挂号记录未处理";
                    break;
                case "DELYLCARD_SAVE"://我的卡号->选择持卡人->删除医疗卡号
                    tips = "温馨提示：删除医疗卡失败，请您重试！";
                    Default = "删除医疗卡失败";
                    break;
                case "ADDYLCARD_CHECK"://我的卡号->选择持卡人->添加医疗卡号 医保卡等卡号设置为只能输入字母和数字
                    tips = "温馨提示：输入卡号有误，请您重试！";
                    Default = "请输入正确的卡号!";
                    break;
                case "ADDYLCARD_SAVE"://我的卡号->选择持卡人->添加医疗卡号->保存
                    tips = "温馨提示：尚未添加成功，请您重试！";
                    Default = "添加医疗卡失败!";
                    break;
                case "GETREISTERMYNJ_USERID"://使用我的南京登录需要更新user_id作为登录唯一标示
                    tips = "温馨提示：同步用户信息失败，请您重试！";
                    Default = "更新User_id失败!";
                    break;
                case "GETREISTERMYNJ_SAVE"://使用我的南京首次登录需要保存用户基本信息
                    tips = "温馨提示：保存用户信息失败，请您重试！";
                    Default = "插入register_pat表失败!";
                    break;
                case "GETREISTERAPP_USERNAME"://单独的APP登录
                    tips = "温馨提示：该手机号还未注册";
                    Default = "用户名不存在!";
                    break;
                case "GETREISTERAPP_PASSWORD"://单独的APP登录
                    tips = "温馨提示：您的密码不匹配，请您重试";
                    Default = "用户密码不匹配!";
                    break;
                case "PATREISTEREDIT_EMAIL"://注册用户基本信息修改 邮箱按照正则表达式判断
                    tips = "温馨提示：邮箱格式有误，请您重试";
                    Default = "邮箱格式不正确!";
                    break;
                case "PATREISTEREDIT_QQ"://注册用户基本信息修改 QQ为纯数字 5-13位
                    tips = "温馨提示：QQ号有误，请您重试";
                    Default = "QQ号码格式不正确!";
                    break;
                case "PATREISTEREDIT_BIRTHDAY": //注册用户基本信息修改 生日限制
                    tips = "温馨提示：生日不能超过当前日期，请您重试";
                    Default = "生日不能超过当前日期!";
                    break;
                case "PATREISTEREDIT_SAVE"://注册用户基本信息修改 保存
                    tips = "温馨提示：修改失败，请您重试！";
                    Default = "用户信息修改失败!";
                    break;
                case "GETPCAPTCHA_SAVE"://发送手机短信验证码 验证码插入后台失败
                    tips = "温馨提示：短信验证码保存异常，请您重试";
                    Default = "插入验证码表失败!";
                    break;
                case "PATPWDFOEGET"://忘记密码 通过短信验证码，APP发送错误的验证码标识符(验证码标识符：表示对应哪个验证码的)
                    tips = "温馨提示：验证码标识符不存在，请您重试";
                    Default = "验证码标标示符不存在!";
                    break;
                case "PATPDWDALTER_CHECK"://修改密码时判断原密码是否一致
                    tips = "温馨提示：原密码输入错误，请您重试";
                    Default = "旧密码输入错误!";
                    break;
                case "PATPDWDALTER_SAVE"://修改密码时保存新密码到后台失败
                    tips = "温馨提示：新密码保存失败，请您重试！";
                    Default = "用户修改密码操作失败!";
                    break;
                case "PATPWDFOEGET_OVERTIME"://验证码过期提示
                    tips = "温馨提示：验证码已失效,请重新获取验证码";
                    Default = "验证码已过期,请重新获取验证码!";
                    break;
                case "PATPWDFOEGET_SAVE"://通过短信重新设置密码,后台保存失败
                    tips = "温馨提示：重新设定密码失败，请您重试";
                    Default = "用户重新设定密码操作失败!";
                    break;




                case "OUTFEEPAYQUERY_NODATA"://在线支付->费用记录->没有数据提示
                    tips = "温馨提示：最近尚无缴费记录";
                    Default = "没有找到相关信息!";
                    break;
                case "OUTFEEDAQUERY_NODATA"://记录查询->处方记录->没有数据提示
                    tips = "温馨提示：最近尚无处方记录";
                    Default = "没有找到相关信息!";
                    break;
                case "GETREGISTERLIST_NOGHDATA"://预约挂号->我的挂号记录->没有数据
                    tips = "";
                    Default = "温馨提示：尚无挂号记录";
                    break;
                case "GETREGISTERLIST_NOYYDATA"://预约挂号->我的预约记录->没有数据
                    tips = "";
                    Default = "温馨提示：尚无预约记录";
                    break;
                case "REGISTERPAYCANCEL_CHECK"://预约挂号->我的挂号记录->详情->取消预约->没有找到对应记录
                    tips = "";
                    Default = "温馨提示：尚无对应记录，请您重试！";
                    break;
                case "REGISTERPAYCANCEL_SAVE"://预约挂号->我的挂号记录->详情->取消预约->数据保存到后台
                    tips = "温馨提示：预约取消保存失败，请您重试！";
                    Default = "预约取消保存失败!";
                    break;
                case "SAVEPATUOTCC_SAVE"://预约挂号->我的挂号记录->记录详情->主病以及病因描述->提交
                    tips = "温馨提示：保存主诉信息失败，请您重试";
                    Default = "保存指定挂号的主诉信息失败!";
                    break;
                case "GETINPATAPPTWAIT_NODATA":
                    tips = "温馨提示：您尚无预约登记信息";
                    Default = "没有找到对应的预约登记信息!";
                    break;
                case "GETPATHOSPAST_NODATA":
                    tips = "温馨提示：您尚无对应住院信息";
                    Default = "没有找到对应的住院信息!";
                    break;
                default:
                    tips = "暂无信息，请重试";
                    Default = "暂无信息，请重试";
                    break;

            }
            if (tips == "")//默认设置为原始内容
            {
                tips = Default;
            }

            return tips;
        }

        /*
        public static string GetTips(string FLAG)
        {
            string tips = "";
            switch (FLAG)
            {
                case "排班信息不存在"://存储过程获取表对应的主键ID失败
                    tips = "";
                    break;
                case "该用户名已经存在":
                    tips = "";
                    break;
                case "该电话号码已经注册过":
                    tips = "";
                    break;
                case "微信号已在该平台注册":
                    tips = "";
                    break;
                case "注册失败":
                    tips = "";
                    break;
                case "验证码错误":
                    tips = "";
                    break;
                case "验证码已过期,请重新获取验证码":
                    tips = "";
                    break;
                case "该身份证已经注册过":
                    tips = "";
                    break;
                case "旧密码输入错误":
                    tips = "";
                    break;
                case "用户修改密码操作失败":
                    tips = "";
                    break;
                case "身份证号码没有记录":
                    tips = "";
                    break;
                case "验证码标标示符不存在":
                    tips = "";
                    break;
                case "用户重新设定密码操作失败":
                    tips = "";
                    break;
                case "未能获取到医院短信接口":
                    tips = "";
                    break;
                case "获取预约标识reg_id失败":
                    tips = "";
                    break;
                case "插入验证码表失败":
                    tips = "";
                    break;
                case "没有找到对应的微信用户信息":
                    tips = "";
                    break;
                case "没有找到对应的注册用户信息":
                    tips = "";
                    break;
                case "用户信息修改失败":
                    tips = "";
                    break;
                case "用户名不存在":
                    tips = "";
                    break;
                case "用户密码不匹配":
                    tips = "";
                    break;
                case "身份证号码不正确":
                    tips = "";
                    break;
                case "更新User_id失败":
                    tips = "";
                    break;
                case "插入register_pat表失败":
                    tips = "";
                    break;
                case "没有找到注册人信息":
                    tips = "";
                    break;
                case "添加持卡人失败":
                    tips = "";
                    break;
                case "该持卡人没有记录":
                    tips = "";
                    break;
                case "身份证号码格式不正确":
                    tips = "";
                    break;

            }
            return "";
              

        }
         */
        static string salt = "0633BA33-8127-42AF-8161-A577B36CC2C0";//加密解密秘钥，不能修改。


        /// <summary>
        /// 获取XML的参数
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetParameters(string XML)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            XmlDocument Doc = XMLHelper.X_GetXmlDocument(XML);
            var dsIRP = XMLHelper.X_GetXmlData(Doc, "ROOT/BODY").Tables[0];//请求的数据包
            foreach (DataColumn dc in dsIRP.Columns)
            {
                dic.Add(dc.ColumnName, dsIRP.Rows[0][dc.ColumnName].ToString().Trim());
            }
            return dic;
        }
        /// <summary>
        /// 获得当前是星期几
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetWeekName(string date)
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = Day[Convert.ToInt32(DateTime.Parse(date).DayOfWeek.ToString("d"))].ToString();
            return week;
        }
        /// <summary>
        /// 返回加密字符串
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns></returns>
        public static string EnSecrt(string password)
        {
            byte[] passwordandSalt = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashButyes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordandSalt);
            string hashString = Convert.ToBase64String(hashButyes);
            return hashString;
        }
        /// <summary>
        /// 判断密码是否一致
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <param name="hashString">加密密码</param>
        /// <returns></returns>
        public static bool DeSecret(string password, string hashString)
        {
            byte[] passwordandSalt = System.Text.Encoding.UTF8.GetBytes(password + salt);
            byte[] hashButyes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordandSalt);

            if (hashString == Convert.ToBase64String(hashButyes))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 验证身份证是否符合
        /// </summary>
        /// <param name="sfz_no"></param>
        /// <returns></returns>
        public static bool isValidSFZ(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日 验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准 
        }
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x 63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "- ");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        /// <summary>
        /// 根据身份证返回性别
        /// </summary>
        /// <param name="sfz_no"></param>
        /// <returns></returns>
        public static string GetSexByIDCard(string sfz_no)
        {
            if (sfz_no.Length == 15)
            {
                return Convert.ToInt32(sfz_no.Substring(14, 1)) % 2 == 1 ? "男" : "女";
            }
            else if (sfz_no.Length == 18)
            {
                return Convert.ToInt32(sfz_no.Substring(16, 1)) % 2 == 1 ? "男" : "女";
            }
            return "";
        }
        /// <summary>
        /// 根据出生日期计算年龄
        /// </summary>
        /// <param name="birthDate">生日</param>
        /// <param name="now">当前日期</param>
        /// <returns></returns>
        public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }

        /// <summary>
        /// 根据身份证获取生日信息
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public static string GetBirthdayByIDCard(string identityCard)
        {
            if (identityCard.Length == 18)
            {
                return identityCard.Substring(6, 4) + "-" + identityCard.Substring(10, 2) + "-" + identityCard.Substring(12, 2);
            }
            else if (identityCard.Length == 15)
            {
                return "19" + identityCard.Substring(6, 2) + "-" + identityCard.Substring(8, 2) + "-" + identityCard.Substring(10, 2);
            }
            return "";
        }
        public static bool ContainsWord(string[] listWords, string findWord)
        {
            foreach (string s in listWords)
            {
                if (s == findWord)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取医院预约挂号未支付的过期时间
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <returns></returns>
        public static string GetYYExpTime(string HOS_ID)
        {
            //string[] FSplit = BusinessWCAppHelper.YYEXPTIME.Split(';');
            //foreach (string s in FSplit)
            //{
            //    string[] split = s.Split('|');
            //    if (split[0] == HOS_ID.Trim())
            //    {
            //        return split[1];
            //    }
            //}
            return "0";
        }
        /// <summary>
        /// 获取医院预约挂号未支付的过期时间
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <returns></returns>
        public static string GetWAITOPENLAST(string HOS_ID)
        {
            //string[] FSplit = BusinessWCAppHelper.WAITOPENLAST.Split(';');
            //foreach (string s in FSplit)
            //{
            //    string[] split = s.Split('|');
            //    if (split[0] == HOS_ID.Trim())
            //    {
            //        return split[1];
            //    }
            //}
            return "0";
        }


        public static string GetConfiguration(string HOS_ID, string column)
        {
            DataTable dt = new Plat.MySQLDAL.BaseFunction().GetList("hos_configuration", "HOS_ID='" + HOS_ID + "'", column);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString().Trim();
            }
            return "";
        }
        /// <summary>
        /// 获取医院预约挂号未支付的过期时间
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <returns></returns>
        public static string GetPAYUNLOCK(string HOS_ID)
        {
            //string[] FSplit = BusinessWCAppHelper.PAYUNLOCK.Split(';');
            //foreach (string s in FSplit)
            //{
            //    string[] split = s.Split('|');
            //    if (split[0] == HOS_ID.Trim())
            //    {
            //        return split[1];
            //    }
            //}
            return "0";
        }
        public static void X_XmlInsertTableDateTime(XmlDocument objXmlDoc, string MailNode, DataTable dataTable, string RowName)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    X_XmlInsertRowDateTime(objXmlDoc, MailNode, RowName, dr);
                }
            }
        }
        /// <summary>
        /// 插入一个节点和此节点的子节点
        /// </summary>
        /// <param name="objXmlDoc">xml</param>
        /// <param name="MailNode">当前节点路径</param>
        /// <param name="ChildNode">新插入节点</param>
        /// <param name="dr">插入节点的子节点集合</param>
        public static void X_XmlInsertRowDateTime(XmlDocument objXmlDoc, string MailNode, string ChildNode, DataRow dr)
        {

            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MailNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode.ToUpper());
            objRootNode.AppendChild(objChildNode);
            if (dr != null && dr.Table.Columns.Count > 0)
            {
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    XmlElement objElement = objXmlDoc.CreateElement(dc.ColumnName.ToUpper());
                    if (dr[dc.ColumnName] != DBNull.Value)
                    {
                        if (dc.DataType.ToString() != "System.DateTime")
                            objElement.InnerText = dr[dc.ColumnName].ToString().ToUpper();
                        else
                            objElement.InnerText = DateTime.Parse(dr[dc.ColumnName].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    objChildNode.AppendChild(objElement);
                }
            }
        }
        /// <summary>
        /// DataTable分页
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="PageIndex">页索引,注意：从1开始</param>
        /// <param name="PageSize">每页大小</param>
        /// <returns></returns>
        public static DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            //判断当前索引
            if (PageIndex == 0)
                return dt;
            //从数据集合拷贝数据
            DataTable newdt = dt.Copy();
            //数据清空
            newdt.Clear();
            //开始数据索引 = 当前页-1 x 每页大小
            int rowbegin = (PageIndex - 1) * PageSize;
            //结束数据索引 = 当前页 x 每页大小
            int rowend = PageIndex * PageSize;
            //开始数据索引 大于等于 当前数据集合大小
            if (rowbegin >= dt.Rows.Count)
                return newdt;
            //结束数据索引 大于 当前数据集合大小
            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            //遍历数据
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }

            return newdt;
        }


        /// <summary>
        /// /*****************************************************************************************
        ///* 类名：FormatHelper                                                  *
        ///* 功能：把金额数据从小写转换为大写                                    *
        /// * 限制条件：金额小于一万亿，且少于两位小数                         *                                       *
        ///* 用法：this.textBox2.Text = FormatHelper.ToUpper(d);                            *
        /// *****************************************************************************************/
        /// 小写金额转换为大写金额，其他条件：金额小于一万亿，最多两位小数
        /// </summary>
        /// <param name="d">源金额，je 《 1000000000000.00(一万亿)，且最多两位小数 </param>
        /// <returns>结果，大写金额</returns>
        static public string ToUpper(decimal InJe)
        {
            decimal d = Math.Abs(InJe);
            if (d == 0)
                return "零元整";

            string je = d.ToString("####.00");
            if (je.Length > 15)
                return "";
            je = new String('0', 15 - je.Length) + je;      //若小于15位长，前面补0

            string stry = je.Substring(0, 4);        //取得'亿'单元
            string strw = je.Substring(4, 4);        //取得'万'单元
            string strg = je.Substring(8, 4);        //取得'元'单元
            string strf = je.Substring(13, 2);        //取得小数部分

            string str1 = "", str2 = "", str3 = "";

            str1 = getupper(stry, "亿");        //亿单元的大写
            str2 = getupper(strw, "万");        //万单元的大写
            str3 = getupper(strg, "元");        //元单元的大写


            string str_y = "", str_w = "";
            if (je[3] == '0' || je[4] == '0')        //亿和万之间是否有0
                str_y = "零";
            if (je[7] == '0' || je[8] == '0')        //万和元之间是否有0
                str_w = "零";

            string ret = str1 + str_y + str2 + str_w + str3;    //亿，万，元的三个大写合并

            for (int i = 0; i < ret.Length; i++)        //去掉前面的"零"   
            {
                if (ret[i] != '零')
                {
                    ret = ret.Substring(i);
                    break;
                }

            }
            for (int i = ret.Length - 1; i > -1; i--)      //去掉最后的"零" 
            {
                if (ret[i] != '零')
                {
                    ret = ret.Substring(0, i + 1);
                    break;
                }
            }

            if (ret[ret.Length - 1] != '元')        //若最后不位不是'元'，则加一个'元'字
                ret = ret + "元";

            if (ret == "零零元")           //若为零元，则去掉"元数"，结果只要小数部分
                ret = "";

            if (strf == "00")            //下面是小数部分的转换
            {
                ret = ret + "整";
            }
            else
            {
                string tmp = "";
                tmp = getint(strf[0]);
                if (tmp == "零")
                    ret = ret + tmp;
                else
                    ret = ret + tmp + "角";

                tmp = getint(strf[1]);
                if (tmp == "零")
                    ret = ret + "整";
                else
                    ret = ret + tmp + "分";
            }

            if (ret[0] == '零')
            {
                ret = ret.Substring(1);          //防止0.03转为"零叁分"，而直接转为"叁分"
            }
            if (InJe < 0)//add by wl 2013.09.22
            {
                ret = "负" + ret;
            }
            return ret;             //完成，返回


        }

        /// <summary>
        /// 把一个单元转为大写，如亿单元，万单元，个单元
        /// </summary>
        /// <param name="str">这个单元的小写数字（4位长，若不足，则前面补零）</param>
        /// <param name="strDW">亿，万，元</param>
        /// <returns>转换结果</returns>
        static private string getupper(string str, string strDW)
        {
            if (str == "0000")
                return "";

            string ret = "";
            string tmp1 = getint(str[0]);
            string tmp2 = getint(str[1]);
            string tmp3 = getint(str[2]);
            string tmp4 = getint(str[3]);
            if (tmp1 != "零")
            {
                ret = ret + tmp1 + "仟";
            }
            else
            {
                ret = ret + tmp1;
            }

            if (tmp2 != "零")
            {
                ret = ret + tmp2 + "佰";
            }
            else
            {
                if (tmp1 != "零")           //保证若有两个零'00'，结果只有一个零，下同
                    ret = ret + tmp2;
            }

            if (tmp3 != "零")
            {
                ret = ret + tmp3 + "拾";
            }
            else
            {
                if (tmp2 != "零")
                    ret = ret + tmp3;
            }

            if (tmp4 != "零")
            {
                ret = ret + tmp4;
            }

            if (ret[0] == '零')            //若第一个字符是'零'，则去掉
                ret = ret.Substring(1);
            if (ret[ret.Length - 1] == '零')        //若最后一个字符是'零'，则去掉
                ret = ret.Substring(0, ret.Length - 1);

            return ret + strDW;            //加上本单元的单位

        }

        /// <summary>
        /// 单个数字转为大写
        /// </summary>
        /// <param name="c">小写阿拉伯数字 0---9</param>
        /// <returns>大写数字</returns>
        static private string getint(char c)
        {
            string str = "";
            switch (c)
            {
                case '0':
                    str = "零";
                    break;
                case '1':
                    str = "壹";
                    break;
                case '2':
                    str = "贰";
                    break;
                case '3':
                    str = "叁";
                    break;
                case '4':
                    str = "肆";
                    break;
                case '5':
                    str = "伍";
                    break;
                case '6':
                    str = "陆";
                    break;
                case '7':
                    str = "柒";
                    break;
                case '8':
                    str = "捌";
                    break;
                case '9':
                    str = "玖";
                    break;
            }
            return str;
        }


        /// <summary>
        /// DataTable转实体类集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DataTableToEntity<T> where T : new()
        {
            /// <summary>
            /// table转实体集合
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public List<T> FillModel_LIST(DataTable dt)
            {
                if (dt == null || dt.Rows.Count == 0)
                    return null;
                List<T> result = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        T res = new T();
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            PropertyInfo propertyInfo = res.GetType().GetProperty(dr.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (propertyInfo != null && dr[i] != DBNull.Value)
                            {
                                var value = dr[i];
                                switch (propertyInfo.PropertyType.FullName)
                                {
                                    case "System.Decimal":
                                        propertyInfo.SetValue(res, Convert.ToDecimal(value), null); break;
                                    case "System.String":
                                        propertyInfo.SetValue(res, value, null); break;
                                    case "System.Int32":
                                        propertyInfo.SetValue(res, Convert.ToInt32(value), null); break;
                                    default:
                                        propertyInfo.SetValue(res, value, null); break;
                                }
                            }
                        }
                        result.Add(res);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                return result;
            }


            /// <summary>
            /// table转实体集合
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public T FillModel(DataTable dt)
            {

                T result = new T();
                if (dt == null || dt.Rows.Count == 0)
                    return result;
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        T res = new T();
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            PropertyInfo propertyInfo = res.GetType().GetProperty(dr.Table.Columns[i].ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (propertyInfo != null && dr[i] != DBNull.Value)
                            {
                                var value = dr[i];
                                switch (propertyInfo.PropertyType.FullName)
                                {
                                    case "System.Decimal":
                                        propertyInfo.SetValue(res, Convert.ToDecimal(value), null); break;
                                    case "System.String":
                                        propertyInfo.SetValue(res, value, null); break;
                                    case "System.Int32":
                                        propertyInfo.SetValue(res, Convert.ToInt32(value), null); break;
                                    default:
                                        propertyInfo.SetValue(res, value, null); break;
                                }
                            }
                        }
                        result = res;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                }
                return result;
            }

            /// <summary>        
            /// /// 实体类转换成DataTable        
            /// /// </summary>        
            /// /// <param name="modelList">实体类列表</param>        
            /// /// <returns></returns>        
            public DataTable FillDataTableList(List<T> modelList)
            {
                if (modelList == null || modelList.Count == 0)
                {
                    return null;
                }
                DataTable dt = CreateData(modelList[0]);
                foreach (T model in modelList)
                {
                    DataRow dataRow = dt.NewRow();
                    foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                    {
                        dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                    }
                    dt.Rows.Add(dataRow);
                }
                return dt;
            }
            /// <summary>        
            /// /// 实体类转换成DataTable        
            /// /// </summary>        
            /// /// <param name="modelList">实体类列表</param>        
            /// /// <returns></returns>        
            public DataTable FillDataTable(T modelList)
            {
                if (modelList == null)
                {
                    return null;
                }
                DataTable dt = CreateData(modelList);
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(modelList, null);
                }
                dt.Rows.Add(dataRow);
                return dt;
            }

            /// <summary>        
            ///  根据实体类得到表结构        
            /// </summary>        
            /// <param name="model">实体类</param>        
            /// <returns></returns>        
            public DataTable CreateData(T model)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
                }
                return dataTable;
            }
        }
    }

    public class ZZJSAVEDATA
    {
        /// <summary>
        /// 自助机状态同步数据
        /// </summary>
        public List<STATEINFO> stateinfo;
    }
    /// <summary>
    /// 自助机数据出参
    /// </summary>
    public class STATEINFO
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HOS_ID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public string DJ_DATE { get; set; }
        /// <summary>
        /// 自助机类型
        /// </summary>
        public string ZZJ_TYPE { get; set; }
        /// <summary>
        /// 终端标识
        /// </summary>
        public string ZZJ_NAME { get; set; }
        /// <summary>
        /// 设备总状态
        /// </summary>
        public string DEV_STATUS { get; set; }
        /// <summary>
        /// 电动磁卡机
        /// </summary>
        public string CARDREADER { get; set; }
        /// <summary>
        /// 发卡器
        /// </summary>
        public string CARDSENDER { get; set; }
        /// <summary>
        /// 国光IC卡读卡器
        /// </summary>
        public string CJ201 { get; set; }
        /// <summary>
        /// 凭条打印机
        /// </summary>
        public string RECEIPTPRINTER { get; set; }
        /// <summary>
        /// 二代证阅读器
        /// </summary>
        public string IDCARDREADER { get; set; }
        /// <summary>
        /// 密码键盘
        /// </summary>
        public string PINPAD { get; set; }
        /// <summary>
        /// 现金模块
        /// </summary>
        public string CASHRECEIVER { get; set; }
        /// <summary>
        /// 激光打印机
        /// </summary>
        public string LASERPRINTER { get; set; }
        /// <summary>
        /// 发票打印机
        /// </summary>
        public string INVOICE { get; set; }
        /// <summary>
        /// 上次修改时间
        /// </summary>
        public string LAST_UPDATE_TIME { get; set; }
        /// <summary>
        /// 上送时间
        /// </summary>
        public string SEND_DATETIME { get; set; }
    }


    public class GOODSIN
    {
        /// <summary>
        /// 物价查询；药品、卫材 、服务项目
        /// </summary>
        public string Order_TYPE { get; set; }

        public string PYM { get; set; }
    }

    public class GOODSOUT
    {
        /// <summary>
        /// 
        /// </summary>
        public string State { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

    }

    public class GOODS
    {
        /// <summary>
        /// 通用名
        /// </summary>
        public string DrugName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 药品剂型
        /// </summary>
        public string DosageForm { get; set; }
        /// <summary>
        /// 门诊计价单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 门诊自费价格
        /// </summary>
        public string Prices { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PYM { get; set; }
        /// <summary>
        /// 医令类型
        /// </summary>
        public string Types { get; set; }
        /// <summary>
        /// 医保自付比例
        /// </summary>
        public string YB_PAYSELF_PERCENT { get; set; }

    }
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Soft.Core
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class CommonFunction
    {
        /// <summary>
        /// 格式化医保结算出参 0^3.1|0.0|0.0|0.0|0.0|0.0|3.1|0.0|0.0|3.1|0.0|3.1|0.0|||3.1|0.0|0.00|13|||2410^H0027^H0027APP^H0027APP^H0027-20180517093217-6199^2^1^
        /// </summary>
        /// <param name="JS_OUT"></param>
        /// <returns></returns>
        public static List<string> GetYBJS_OUT(string JS_OUT)
        {
            //List<string> list = new List<string>();
            //list.Add(JS_OUT);
            //list.Add("");
            //return list;

            List<string> list = new List<string>();
            string result = JS_OUT;
            try
            {
                int index = JS_OUT.IndexOf('^', JS_OUT.IndexOf("^") + 1);
                list.Add(result.Substring(0, index - 5));
                list.Add(result.Substring(index - 4));
            }
            catch
            {

            }
            return list;
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
        /// <summary>
        /// 由分隔符组成的字符串，替换掉其中完全匹配的关键字
        /// </summary>
        /// <param name="original"></param>
        /// <param name="replaceValue"></param>
        /// <param name="c_spilit"></param>
        /// <returns></returns>
        public static string SplitReplace(string original, string replaceValue, char c_spilit)
        {
            string result = "";
            string[] split = original.Split(c_spilit);
            foreach (string s in split)
            {
                if (s == replaceValue)
                {

                }
                else
                {
                    result += s + c_spilit;
                }
            }
            result = result.TrimEnd(c_spilit);
            return result;

        }
        /// <summary>
        /// XML 转化为JSON add by hlw 2018.07.03
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XmlToJson(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.InnerXml = xml;
            return JsonConvert.SerializeXmlNode(document);
        }
        /// <summary>
        /// json转换为xml add by hlw 2018.07.03
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonToXml(string json)
        {
            return JsonConvert.DeserializeXmlNode(json).InnerXml;
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

        public static void GetSecretHOS(string HOS_ID, string inxml, out string pherText, out string signature)
        {
            string key = GetSecretKEY(HOS_ID);
            pherText = AESExample.AESEncrypt(inxml, EncryptionKeyCore.KeyData.AESKEY(HOS_ID));//入参加密
            signature = Encode(pherText, key);//MD5加密
        }
        public static string GetSecretKEY(string HOS_ID)
        {
            return EncryptionKeyCore.KeyData.AESKEY(HOS_ID);
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="encryptKey">加密密钥,最长32位</param>
        /// <returns></returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encryptString + encryptKey, "MD5");
            return Md5Hash(encryptString + encryptKey);

        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }
            return sBuilder.ToString();
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
        /// 返回 某一数据转换成string 并去掉空格  是Null返回 ""
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static string GetStr(object dc)
        {
            if (dc == DBNull.Value || null == dc)
                return "";
            return dc.ToString().Trim();
        }
        /// <summary> 
        /// POST请求与获取结果 
        /// </summary> 
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "text/xml";
            //request.ContentLength = postDataStr.Length;
            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码 
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }

        /// <summary>
        /// 企业微信发送消息
        /// </summary>
        public static void SendQYWXMessage(string message)
        {
            message = HttpUtility.UrlEncode(message);
            //System.Diagnostics.Process.Start("http://wxgzhfw.ztejsapp.cn/CorpNo/SerMessage.aspx?touser=WangDan|HouLiWei&content=" + message);
            //System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            //string myUrl = "http://wxgzhfw.ztejsapp.cn/CorpNo/SerMessage.aspx?touser=WangDan|HouLiWei&content=" + message;
            //myProcess.StartInfo.FileName = "iexplore.exe";
            //myProcess.StartInfo.Arguments = myUrl;
            //myProcess.Start(); 


            var request = (HttpWebRequest)WebRequest.Create("http://wxgzhfw.ztejsapp.cn/CorpNo/SerMessage.aspx?touser=WangDan|HouLiWei&content=" + message);
            var response = (HttpWebResponse)request.GetResponse();
        }

        public static string GetTips(string FLAG, string HOS_ID)
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
                case "GETHOSPDEPTGH_NODATA"://预约挂号->选择科室->科室为空（此处科室根据排班列出来）挂号提示
                    switch (HOS_ID)
                    {
                        case "7":
                            tips = "温馨提示：当日挂号功能暂未开放，敬请期待！";
                            Default = "没有排班信息";
                            break;
                        case "35":
                            tips = "温馨提示：无可在线挂号的排班！";
                            Default = "没有排班信息";
                            break;
                        default:
                            tips = "温馨提示：尚未公布科室信息，请您等待更新！";
                            Default = "没有排班信息";
                            break;
                    }
                    break;
                case "GETHOSPDEPTYY_NODATA"://预约挂号->选择科室->科室为空（此处科室根据排班列出来）预约提示
                    switch (HOS_ID)
                    {
                        case "7":
                            //case "35":
                            tips = "温馨提示：预约功能暂未开放，敬请期待！";
                            Default = "没有排班信息";
                            break;
                        case "35":
                            tips = "温馨提示:此科室尚未公布排班信息，请您等待更新！";
                            Default = "没有排班信息";
                            break;
                        default:
                            tips = "温馨提示：尚未公布科室信息，请您等待更新！";
                            Default = "没有排班信息";
                            break;
                    }
                    break;
                case "GETHOSPDEPT_NODATA"://预约挂号->选择科室->科室为空（此处科室根据排班列出来）挂号提示
                    tips = "温馨提示：尚未公布科室信息，请您等待更新！";
                    Default = "没有排班信息";
                    break;
                case "GETDOCINFO_NODATA"://预约挂号，实时叫号，搜索医生等，传入的条件没有找到对应的医生
                    switch (HOS_ID)
                    {
                        case "7":
                            tips = "温馨提示：预约功能暂未开放，敬请期待！";
                            Default = "没有排班信息";
                            break;

                        default:
                            tips = "温馨提示：尚未公布医生信息，请您等待更新！";
                            Default = "没有找到对应的医生信息";
                            break;
                    }
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
                            objElement.InnerText = dr[dc.ColumnName].ToString();
                        else
                            objElement.InnerText = DateTime.Parse(dr[dc.ColumnName].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    objChildNode.AppendChild(objElement);
                }
            }
        }
        /// <summary>
        /// <summary>
        /// 返回某一Int数据是Null或0
        /// </summary>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static int GetInt(object dc)
        {

            if (dc == DBNull.Value)
                return 0;
            try
            {
                return int.Parse(dc.ToString());
            }
            catch
            {
                return 0;
            }
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
        /// 返回字符串对应的日期格式，如果转换错误，转换为当前时间返回
        /// </summary>
        /// <param name="datetimeValue"></param>
        /// <returns></returns>
        public static DateTime ValidDateTime(string datetimeValue)
        {
            DateTime result;
            if (DateTime.TryParse(datetimeValue, out result))
            {
                return result;
            }
            else
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDigit(object value)
        {
            string s_value = Convert.ToString(value);
            int result;
            if (int.TryParse(s_value, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
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
        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="identityCard"></param>
        /// <returns></returns>
        public static string GetSexByIDCard(string identityCard)
        {
            if (identityCard.Length == 18)
            {
                return int.Parse(identityCard.Substring(16, 1))%2== 0 ? "女" : "男";
            }
            else if (identityCard.Length == 15)
            {
                return int.Parse(identityCard.Substring(14, 1)) % 2 == 0 ? "女" : "男";
            }
            return "";
        }

        /// <summary>
        /// 手机号码,身份证特定位置换成*号
        /// </summary>
        /// <param name="OriginString"></param>
        /// <returns></returns>
        public static string SetPatInfoSTAR(string OriginString)
        {

            //return OriginString;
            //138****0156
            if (OriginString.Length < 7)
            {
                return OriginString;
            }
            string m_star = "";
            for (int i = 0; i < OriginString.Length; i++)
            {
                if (i >= 1 && i <= OriginString.Length - 2)
                {
                    m_star += "*";
                    continue;
                }
                m_star += OriginString[i];
            }
            return m_star;
        }
        static string salt = "0633BA33-8127-42AF-8161-A577B36CC2C0";//加密解密秘钥，不能修改。

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
        /// 获取XML的参数
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetParameters(string XML)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument Doc = XMLHelper.X_GetXmlDocument(XML);
            var dsIRP = XMLHelper.X_GetXmlData(Doc, "ROOT/BODY").Tables[0];//请求的数据包
            foreach (DataColumn dc in dsIRP.Columns)
            {
                dic.Add(dc.ColumnName, dsIRP.Rows[0][dc.ColumnName].ToString().Trim());
            }
            return dic;
        }

        /// <summary>
        /// 验证字符串是否为纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str) //接收一个string类型的参数,保存到str里
        {
            if (str == null || str.Length == 0)    //验证这个参数是否为空
                return false;                           //是，就返回False
            ASCIIEncoding ascii = new ASCIIEncoding();//new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里

            foreach (byte c in bytestr)                   //遍历这个数组里的内容
            {
                if (c < 48 || c > 57)                          //判断是否为数字
                {
                    return false;                              //不是，就返回False
                }
            }
            return true;                                        //是，就返回True
        }

                #region 获取Json字符串某节点的值
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            if (key == "mediInsuOutpam")
            {
                return GetJsonValue_YB(jsonStr, key);
            }
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue_YB(string jsonStr, string key)
        {
            YBJS yBJS = JSONSerializer.Deserialize<YBJS>(jsonStr);
            string mediInsuOutpam = yBJS.mediInsuOutpam;
            return mediInsuOutpam;

        }

        /// <summary>
        /// 是否为11位有效电话号码
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_handset, @"^[1]+\d{10}");
        }
    }

    public class YBJS
    {
        /// <summary>
        /// 
        /// </summary>
        public string cardNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string cardType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string citizenCardPhoto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 0^0001466918|00018538|南京脑科医院|320105196312270421|徐蔚霞|2|11|0|320101|3723.57|0|0|1||9402,9123,6101,6601|1|6101,6601|1|9402|0||0||0||1|9123|0|||0||0||0|||0||0|||1|||4000.0|19919.0|0|||0|2000.0|0|
        /// </summary>
        public string mediInsuOutpam { get; set; }

        /// <summary>
        /// 获取成功!
        /// </summary>
        public string msg { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Soft.Core
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
    public class QHZZJCommonFunction
    {
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
    }
}

using CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soft.Core;
using System.Xml;
using System.Data;
using Org.BouncyCastle.Utilities.Encoders;
using System.IO;
using System.Text.RegularExpressions;
//using System.IO;

namespace OnlineBusHos36_YYGH.BUS
{
    class GETHOSPDEPT
    {
        /// <summary>
        /// 验证输入字符串为18位的身份证号码
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsIDCard18(string strln)
        {
            long n = 0;
            if (long.TryParse(strln.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(strln.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(strln.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = strln.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = strln.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != strln.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        public static DateTime getBirthday(string cardId)
        {
            if (cardId.Length == 18)
            {
                return Convert.ToDateTime(cardId.Substring(6, 8).Insert(4, "-").Insert(7, "-"));
            }
            else if (cardId.Length == 15)
            {
                return Convert.ToDateTime(("19" + cardId.Substring(6, 6)).Insert(4, "-").Insert(7, "-"));
            }
            else
            {
                return Convert.ToDateTime("1900-01-01");
            }
        }






        /// <summary>
        /// 验证输入字符串为15位的身份证号码
        /// </summary>
        /// <param name="strln">输入的字符</param>
        /// <returns></returns>
        public static bool IsIDCard15(string strln)
        {
            long n = 0;
            if (long.TryParse(strln, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(strln.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = strln.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        public static int GetAge(string cardId)
        {
            DateTime dtBir = getBirthday(cardId);
            return GetAgeByBirthdate(dtBir);
        }
        public static int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }


        /// <summary>
        /// 验证身份证是否有效
        /// </summary>
        /// <param name="strln"></param>
        /// <returns></returns>
        public static bool IsIDCard(string strln)
        {
            if (strln.Length == 18)
            {
                bool check = IsIDCard18(strln);
                return check;
            }
            else if (strln.Length == 15)
            {
                bool check = IsIDCard15(strln);
                return check;
            }
            else
            {
                return false;
            }
        }

        public static string getSex(string cardId)
        {
            string rtn;
            string tmp = "";

            if (cardId.Length ==18)
            {
                //return Convert.ToInt16(cardId.Substring(16, 1)) % 2 == 0 ? "女" : "男";
                tmp = cardId.Substring(cardId.Length - 4);
                tmp = tmp.Substring(0, 3);
            }
            else if(cardId.Length == 15)
            {
                tmp = cardId.Substring(cardId.Length - 3);
            }
            else
            {
                return "";
            }
            int sx = int.Parse(tmp);
            int outNum;
            Math.DivRem(sx, 2, out outNum);
            if (outNum == 0)
            {
                rtn = "女";
            }
            else
            {
                rtn = "男";
            }
            return rtn;
        }

        public static string B_GETHOSPDEPT(string json_in)
        {
            return Business(json_in);
        }
        public static string Business(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.GETHOSPDEPT_M.GETHOSPDEPT_IN _in = JSONSerializer.Deserialize<Model.GETHOSPDEPT_M.GETHOSPDEPT_IN>(json_in);
                Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT _out = new Model.GETHOSPDEPT_M.GETHOSPDEPT_OUT();
                XmlDocument doc = QHXmlMode.GetBaseXml("GETDEPTINFO", "1");
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", string.IsNullOrEmpty(_in.USER_ID) ? "" : _in.USER_ID.Trim());

                string USE_TYPE = _in.USE_TYPE;
                switch (USE_TYPE)
                {
                    case "09"://当日
                        USE_TYPE = "01";
                        break;
                    case "08"://预约
                        USE_TYPE = "02";
                        break;
                    default:
                        USE_TYPE = "";
                        break;
                }

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USE_TYPE", USE_TYPE);
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_TYPE", string.IsNullOrEmpty(_in.FILT_TYPE) ? "" : _in.FILT_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "FILT_VALUE", string.IsNullOrEmpty(_in.FILT_VALUE) ? "" : _in.FILT_VALUE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "RETURN_TYPE", string.IsNullOrEmpty(_in.RETURN_TYPE) ? "" : _in.RETURN_TYPE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGEINDEX", string.IsNullOrEmpty(_in.PAGEINDEX) ? "" : _in.PAGEINDEX.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAGESIZE", string.IsNullOrEmpty(_in.PAGESIZE) ? "" : _in.PAGESIZE.Trim());
                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                string SFZ_NO = _in.SFZ_NO;
                string MODEL_TYPE = string.IsNullOrEmpty(_in.MODEL_TYPE) ? "" : _in.MODEL_TYPE.Trim();

                XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "MODEL_TYPE", MODEL_TYPE);

                string inxml = doc.InnerXml;
                string his_rtnxml = "";
                //System.IO.StreamReader stream = new StreamReader("D:/test.txt");
                //string his_rtnxml = stream.ReadToEnd();

                if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                {

                    dataReturn.Code = 1;
                    dataReturn.Msg = his_rtnxml;
                    goto EndPoint;
                }

                //_out.HIS_RTNXML = his_rtnxml;
                try
                {

                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataSet ds = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY");
                    DataTable dtrev = ds.Tables[0];
                    if (dtrev.Rows[0]["CLBZ"].ToString() != "0")
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = dtrev.Rows[0]["CLJG"].ToString();
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }

                    if (MODEL_TYPE == "1")//新模式
                    {
                        string jzDepts = dtrev.Rows[0]["jzDepts"].ToString(); //急诊科室 | 分割
                        string deptNotUse = dtrev.Rows[0]["deptNotUse"].ToString();//不使用的科室 | 分割
                        string kidDepts = dtrev.Rows[0]["kidDepts"].ToString();//儿科科室  | 分割
                        string maleDepts = dtrev.Rows[0]["maleDepts"].ToString();//男性科室 | 分割
                        string femaleDepts = dtrev.Rows[0]["femaleDepts"].ToString();//女性科室 | 分割
                        string JZMAC = dtrev.Rows[0]["JZMAC"].ToString();//急诊设备 | 分割
                        DataTable dtdept = ds.Tables["DEPT"];//这里为his原始出参
                        #region 筛选并重组出参


                        //不使用的科室直接删除
                        try
                        {
                            //foreach (DataRow row in dtdept.AsEnumerable().Where(row => jzdepts.Contains(FormatHelper.GetStr(row["DEPT_CODE"]))))//这样写不行 会因为对集合进行了操作在第二次循环时报错
                            foreach (DataRow row in dtdept.Select("DEPT_CODE IN (" + deptNotUse.Replace("|", ",") + ")"))
                            {
                                if (null != row)
                                {
                                    row.Delete();
                                    dtdept.AcceptChanges();//提交修改
                                }

                            }
                        }
                        catch { }

                        //急诊设备过滤
                        //确定设备是否为急诊设别
                        string[] jzMac =JZMAC.Split('|');
                        bool isJzMac = jzMac.Contains(_in.LTERMINAL_SN.Trim());

                        //急诊设备当日挂号才能获取急诊科室 
                        //筛掉急诊的,急诊没有预约
                        if (USE_TYPE == "02" || !isJzMac)
                        {

                            try
                            {
                                //foreach (DataRow row in dtdept.AsEnumerable().Where(row => jzdepts.Contains(FormatHelper.GetStr(row["DEPT_CODE"]))))//这样写不行 会因为对集合进行了操作在第二次循环时报错
                                foreach (DataRow row in dtdept.Select("DEPT_CODE IN (" + jzDepts.Replace("|", ",") + ")"))
                                {
                                    if (null != row)
                                    {
                                        row.Delete();
                                        dtdept.AcceptChanges();//提交修改
                                    }

                                }
                            }
                            catch { }

                        }

                        

                        //判断是否为外国人
                        bool is_wg = false;
                        bool containsLetter = Regex.IsMatch(_in.SFZ_NO, @"[a-zA-Z]");//包含字母,永居证
                        if (SFZ_NO.Length < 15|| containsLetter)//外国人不进行年龄性别判断
                        {
                            is_wg = true;
                        }
                        else
                        {
                            //20240219 处理15位身份证报错的问题
                            int age = GetAge(SFZ_NO);
                            string sex = getSex(SFZ_NO);

                            if(age >14)//筛掉儿童科室
                            {
                                //foreach (var row in dtdept.AsEnumerable().Where(row => kidDepts.Contains(FormatHelper.GetStr(row["DEPT_CODE"]) ) ))
                                //    row.Delete();

                                try
                                {
                                    foreach (DataRow row in dtdept.Select("DEPT_CODE IN (" + kidDepts.Replace("|", ",") + ")"))
                                    {
                                        if (null != row)
                                        {
                                            row.Delete();
                                            dtdept.AcceptChanges();//提交修改
                                        }

                                    }
                                }
                                catch { }
                            }

                            if(sex =="男")//筛掉女性科室
                            {
                                //foreach (var row in dtdept.AsEnumerable().Where(row => femaleDepts.Contains(FormatHelper.GetStr(row["DEPT_CODE"]))))
                                //    row.Delete();


                                try
                                {
                                    foreach (DataRow row in dtdept.Select("DEPT_CODE IN (" + femaleDepts.Replace("|", ",") + ")"))
                                    {
                                        if (null != row)
                                        {
                                            row.Delete();
                                            dtdept.AcceptChanges();//提交修改
                                        }

                                    }
                                }
                                catch { }
                            }

                            if(sex =="女")//筛掉男性科室
                            {
                                //foreach (var row in dtdept.AsEnumerable().Where(row => maleDepts.Contains(FormatHelper.GetStr(row["DEPT_CODE"]))))
                                //    row.Delete();
                                try
                                {
                                    foreach (DataRow row in dtdept.Select("DEPT_CODE IN (" + maleDepts.Replace("|", ",") + ")"))
                                    {
                                        if (null != row)
                                        {
                                            row.Delete();
                                            dtdept.AcceptChanges();//提交修改
                                        }

                                    }
                                }
                                catch { }

                            }

                         

                            
                        }


                        //大科室
                        _out.DEPTLIST = new List<Model.GETHOSPDEPT_M.DEPT>();
                        DataView dataView = dtdept.DefaultView;
                        DataTable DistTable = dataView.ToTable("Dist", true, "DEPT_CLASSNEW", "DEPT_CLASSNEWORDER");


                        foreach (DataRow dr in DistTable.Rows)
                        {
                            Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();
                            dept.DEPT_CODE = dr["DEPT_CLASSNEW"].ToString();
                            dept.DEPT_NAME = dr["DEPT_CLASSNEW"].ToString();
                            dept.DEPT_INTRO = "";
                            dept.DEPT_URL = "";
                            dept.DEPT_ORDER = dr["DEPT_CLASSNEWORDER"].ToString();
                            dept.DEPT_TYPE = "";
                            dept.DEPT_ADDRESS = "";
                            dept.DEPT_USE = "";//大科室为空
                                               //子科室
                                               //dept.CHILDREN = new List<Model.GETHOSPDEPT_M.CHILDRENDEPT>();
                            dept.CHILDREN = (from a in dtdept.AsEnumerable()
                                             where FormatHelper.GetStr(a["DEPT_CLASSNEW"]) == dept.DEPT_CODE
                                             select new Model.GETHOSPDEPT_M.CHILDRENDEPT
                                             {
                                                 DEPT_CODE = FormatHelper.GetStr(a["DEPT_CODE"]),
                                                 DEPT_NAME = FormatHelper.GetStr(a["DEPT_NAME"]),
                                                 DEPT_INTRO = FormatHelper.GetStr(a["DEPT_INTRO"]),
                                                 DEPT_URL = FormatHelper.GetStr(a["DEPT_URL"]),
                                                 DEPT_ORDER = FormatHelper.GetStr(a["DEPT_NEWORDER"]),
                                                 DEPT_TYPE = FormatHelper.GetStr(a["DEPT_TYPE"]),
                                                 DEPT_ADDRESS = FormatHelper.GetStr(a["DEPT_ADDRESS"]),
                                                 DEPT_USE = jzDepts.Contains(a["DEPT_CODE"].ToString()) ? "03" : ""
                                             }).ToList();
                            dept.CHILDREN = dept.CHILDREN.OrderBy(x => x.DEPT_ORDER).ToList();//重新排序
                            _out.DEPTLIST.Add(dept);

                        }
                        _out.DEPTLIST = _out.DEPTLIST.OrderBy(x => Convert.ToInt32(x.DEPT_ORDER)).ToList();//大科室重新排序

                        //dataReturn.Code = 0;
                        //dataReturn.Msg = "SUCCESS";
                        //dataReturn.Param = JSONSerializer.Serialize(_out);


                        #endregion



                    }
                    else
                    {
                        try
                        {
                            DataTable dtdept = ds.Tables["DEPT"];
                            if (dtdept.Rows.Count > 0)
                            {
                                dtdept.DefaultView.Sort = "DEPT_NAME";
                                dtdept = dtdept.DefaultView.ToTable();

                                foreach (DataRow dr in dtdept.Rows)
                                {
                                    if (string.IsNullOrEmpty(dr["DEPT_ORDER"].ToString()))
                                    {
                                        dr["DEPT_ORDER"] = "9999";
                                    }
                                }

                                dtdept = System.Data.DataTableExtensions.CopyToDataTable(dtdept.Rows.Cast<DataRow>().OrderBy(r => Convert.ToDecimal(r["DEPT_ORDER"])));

                            }

                            _out.DEPTLIST = new List<Model.GETHOSPDEPT_M.DEPT>();
                            foreach (DataRow dr in dtdept.Rows)
                            {
                                Model.GETHOSPDEPT_M.DEPT dept = new Model.GETHOSPDEPT_M.DEPT();

                                dept.DEPT_CODE = dtdept.Columns.Contains("DEPT_CODE") ? dr["DEPT_CODE"].ToString() : "";
                                dept.DEPT_NAME = dtdept.Columns.Contains("DEPT_NAME") ? dr["DEPT_NAME"].ToString() : "";
                                dept.DEPT_INTRO = dtdept.Columns.Contains("DEPT_INTRO") ? dr["DEPT_INTRO"].ToString() : "";
                                dept.DEPT_URL = dtdept.Columns.Contains("DEPT_URL") ? dr["DEPT_URL"].ToString() : "";
                                dept.DEPT_ORDER = dtdept.Columns.Contains("DEPT_ORDER") ? dr["DEPT_ORDER"].ToString() : "";
                                dept.DEPT_TYPE = dtdept.Columns.Contains("DEPT_TYPE") ? dr["DEPT_TYPE"].ToString() : "";
                                dept.DEPT_ADDRESS = dtdept.Columns.Contains("DEPT_ADDRESS") ? dr["DEPT_ADDRESS"].ToString() : "";
                                dept.DEPT_USE = dtdept.Columns.Contains("DEPT_USE") ? dr["DEPT_USE"].ToString() : "";
                                if (dept.DEPT_CODE.Equals("急诊科") || dept.DEPT_CODE.Equals("发热门诊"))
                                {
                                    dept.DEPT_USE = "03";
                                }

                                _out.DEPTLIST.Add(dept);
                            }
                        }
                        catch
                        {
                            //    dataReturn.Code = 5;
                            //    dataReturn.Msg = "解析HIS出参失败,未找到DEPTLIST节点,请检查HIS出参";
                            dataReturn.Code = 0;
                            dataReturn.Msg = "SUCCESS";
                            dataReturn.Param = JSONSerializer.Serialize(_out);
                            goto EndPoint;
                        }

                    }




                    dataReturn.Code = 0;
                    dataReturn.Msg = "SUCCESS";
                    dataReturn.Param = JSONSerializer.Serialize(_out);

                }
                catch (Exception ex)
                {
                    dataReturn.Code = 5;
                    dataReturn.Msg = "解析HIS出参失败,请检查HIS出参是否正确";
                    dataReturn.Param = ex.Message;
                }
            }
            catch (Exception ex)
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "程序处理异常";
                dataReturn.Param = ex.Message;
            }
        EndPoint:
            json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;

        }
    }
}

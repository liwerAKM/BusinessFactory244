using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:register_pay
    /// </summary>
    public partial class register_pay 
    {
        public register_pay()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQLZZJ.GetMaxID("PAY_ID", "register_pay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PAY_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from register_pay");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAY_ID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.register_pay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_pay(");
            strSql.Append("PAY_ID,REG_ID,HOS_ID,PAT_ID,REGPAT_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@REG_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@OPT_SN,@HOS_SN,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@SEX,@DEPT_CODE,@BIRTHDAY,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_GH_TYPE,@HOS_GH_NAME,@ZL_FEE,@GH_FEE,@CASH_JE,@JZ_CODE,@PAY_TYPE,@YB_SN,@YB_TYPE,@YB_FYLB,@YB_GH_ORDER,@YB_ZLFEE,@YB_GHFEE,@XJZF,@ZHZF,@ZHYE,@XZM,@XZMCH,@YBBZM,@YBBZMC,@JE_ALL,@APPT_ORDER,@APPT_SN,@IS_DZ,@DJ_DATE,@DJ_TIME,@IS_TH,@RH_DATE,@TH_TIME)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_GH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_GH_NAME", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,5),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YB_SN", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_GH_ORDER", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_ZLFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@YB_GHFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@JE_ALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_DZ", MySqlDbType.Bit),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TH", MySqlDbType.Bit),
					new MySqlParameter("@RH_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.REG_ID;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.REGPAT_ID;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_SN;
            parameters[7].Value = model.SFZ_NO;
            parameters[8].Value = model.YLCARD_TYPE;
            parameters[9].Value = model.YLCARD_NO;
            parameters[10].Value = model.PAT_NAME;
            parameters[11].Value = model.SEX;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.BIRTHDAY;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME;
            parameters[18].Value = model.GH_TYPE;
            parameters[19].Value = model.HOS_GH_TYPE;
            parameters[20].Value = model.HOS_GH_NAME;
            parameters[21].Value = model.ZL_FEE;
            parameters[22].Value = model.GH_FEE;
            parameters[23].Value = model.CASH_JE;
            parameters[24].Value = model.JZ_CODE;
            parameters[25].Value = model.PAY_TYPE;
            parameters[26].Value = model.YB_SN;
            parameters[27].Value = model.YB_TYPE;
            parameters[28].Value = model.YB_FYLB;
            parameters[29].Value = model.YB_GH_ORDER;
            parameters[30].Value = model.YB_ZLFEE;
            parameters[31].Value = model.YB_GHFEE;
            parameters[32].Value = model.XJZF;
            parameters[33].Value = model.ZHZF;
            parameters[34].Value = model.ZHYE;
            parameters[35].Value = model.XZM;
            parameters[36].Value = model.XZMCH;
            parameters[37].Value = model.YBBZM;
            parameters[38].Value = model.YBBZMC;
            parameters[39].Value = model.JE_ALL;
            parameters[40].Value = model.APPT_ORDER;
            parameters[41].Value = model.APPT_SN;
            parameters[42].Value = model.IS_DZ;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.IS_TH;
            parameters[46].Value = model.RH_DATE;
            parameters[47].Value = model.TH_TIME;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Plat.Model.register_pay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update register_pay set ");
            strSql.Append("REG_ID=@REG_ID,");
            strSql.Append("HOS_ID=@HOS_ID,");
            strSql.Append("PAT_ID=@PAT_ID,");
            strSql.Append("REGPAT_ID=@REGPAT_ID,");
            strSql.Append("OPT_SN=@OPT_SN,");
            strSql.Append("HOS_SN=@HOS_SN,");
            strSql.Append("SFZ_NO=@SFZ_NO,");
            strSql.Append("YLCARD_TYPE=@YLCARD_TYPE,");
            strSql.Append("YLCARD_NO=@YLCARD_NO,");
            strSql.Append("PAT_NAME=@PAT_NAME,");
            strSql.Append("SEX=@SEX,");
            strSql.Append("DEPT_CODE=@DEPT_CODE,");
            strSql.Append("BIRTHDAY=@BIRTHDAY,");
            strSql.Append("DEPT_NAME=@DEPT_NAME,");
            strSql.Append("DOC_NO=@DOC_NO,");
            strSql.Append("DOC_NAME=@DOC_NAME,");
            strSql.Append("DIS_NAME=@DIS_NAME,");
            strSql.Append("GH_TYPE=@GH_TYPE,");
            strSql.Append("HOS_GH_TYPE=@HOS_GH_TYPE,");
            strSql.Append("HOS_GH_NAME=@HOS_GH_NAME,");
            strSql.Append("ZL_FEE=@ZL_FEE,");
            strSql.Append("GH_FEE=@GH_FEE,");
            strSql.Append("CASH_JE=@CASH_JE,");
            strSql.Append("JZ_CODE=@JZ_CODE,");
            strSql.Append("PAY_TYPE=@PAY_TYPE,");
            strSql.Append("YB_SN=@YB_SN,");
            strSql.Append("YB_TYPE=@YB_TYPE,");
            strSql.Append("YB_FYLB=@YB_FYLB,");
            strSql.Append("YB_GH_ORDER=@YB_GH_ORDER,");
            strSql.Append("YB_ZLFEE=@YB_ZLFEE,");
            strSql.Append("YB_GHFEE=@YB_GHFEE,");
            strSql.Append("XJZF=@XJZF,");
            strSql.Append("ZHZF=@ZHZF,");
            strSql.Append("ZHYE=@ZHYE,");
            strSql.Append("XZM=@XZM,");
            strSql.Append("XZMCH=@XZMCH,");
            strSql.Append("YBBZM=@YBBZM,");
            strSql.Append("YBBZMC=@YBBZMC,");
            strSql.Append("JE_ALL=@JE_ALL,");
            strSql.Append("APPT_ORDER=@APPT_ORDER,");
            strSql.Append("APPT_SN=@APPT_SN,");
            strSql.Append("IS_DZ=@IS_DZ,");
            strSql.Append("DJ_DATE=@DJ_DATE,");
            strSql.Append("DJ_TIME=@DJ_TIME,");
            strSql.Append("IS_TH=@IS_TH,");
            strSql.Append("RH_DATE=@RH_DATE,");
            strSql.Append("TH_TIME=@TH_TIME");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_GH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_GH_NAME", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,5),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YB_SN", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_GH_ORDER", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_ZLFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@YB_GHFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@JE_ALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_DZ", MySqlDbType.Bit),
					new MySqlParameter("@DJ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TH", MySqlDbType.Bit),
					new MySqlParameter("@RH_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11)};
            parameters[0].Value = model.REG_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.OPT_SN;
            parameters[5].Value = model.HOS_SN;
            parameters[6].Value = model.SFZ_NO;
            parameters[7].Value = model.YLCARD_TYPE;
            parameters[8].Value = model.YLCARD_NO;
            parameters[9].Value = model.PAT_NAME;
            parameters[10].Value = model.SEX;
            parameters[11].Value = model.DEPT_CODE;
            parameters[12].Value = model.BIRTHDAY;
            parameters[13].Value = model.DEPT_NAME;
            parameters[14].Value = model.DOC_NO;
            parameters[15].Value = model.DOC_NAME;
            parameters[16].Value = model.DIS_NAME;
            parameters[17].Value = model.GH_TYPE;
            parameters[18].Value = model.HOS_GH_TYPE;
            parameters[19].Value = model.HOS_GH_NAME;
            parameters[20].Value = model.ZL_FEE;
            parameters[21].Value = model.GH_FEE;
            parameters[22].Value = model.CASH_JE;
            parameters[23].Value = model.JZ_CODE;
            parameters[24].Value = model.PAY_TYPE;
            parameters[25].Value = model.YB_SN;
            parameters[26].Value = model.YB_TYPE;
            parameters[27].Value = model.YB_FYLB;
            parameters[28].Value = model.YB_GH_ORDER;
            parameters[29].Value = model.YB_ZLFEE;
            parameters[30].Value = model.YB_GHFEE;
            parameters[31].Value = model.XJZF;
            parameters[32].Value = model.ZHZF;
            parameters[33].Value = model.ZHYE;
            parameters[34].Value = model.XZM;
            parameters[35].Value = model.XZMCH;
            parameters[36].Value = model.YBBZM;
            parameters[37].Value = model.YBBZMC;
            parameters[38].Value = model.JE_ALL;
            parameters[39].Value = model.APPT_ORDER;
            parameters[40].Value = model.APPT_SN;
            parameters[41].Value = model.IS_DZ;
            parameters[42].Value = model.DJ_DATE;
            parameters[43].Value = model.DJ_TIME;
            parameters[44].Value = model.IS_TH;
            parameters[45].Value = model.RH_DATE;
            parameters[46].Value = model.TH_TIME;
            parameters[47].Value = model.PAY_ID;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from register_pay ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAY_ID;

            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string PAY_IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from register_pay ");
            strSql.Append(" where PAY_ID in (" + PAY_IDlist + ")  ");
            int rows = DbHelperMySQLZZJ.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Plat.Model.register_pay GetModel(int PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,REG_ID,HOS_ID,PAT_ID,REGPAT_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME from register_pay ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = PAY_ID;

            Plat.Model.register_pay model = new Plat.Model.register_pay();
            DataSet ds = DbHelperMySQLZZJ.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Plat.Model.register_pay DataRowToModel(DataRow row)
        {
            Plat.Model.register_pay model = new Plat.Model.register_pay();
            if (row != null)
            {
                if (row["PAY_ID"] != null && row["PAY_ID"].ToString() != "")
                {
                    model.PAY_ID = int.Parse(row["PAY_ID"].ToString());
                }
                if (row["REG_ID"] != null && row["REG_ID"].ToString() != "")
                {
                    model.REG_ID = int.Parse(row["REG_ID"].ToString());
                }
                if (row["HOS_ID"] != null)
                {
                    model.HOS_ID = row["HOS_ID"].ToString();
                }
                if (row["PAT_ID"] != null && row["PAT_ID"].ToString() != "")
                {
                    model.PAT_ID = int.Parse(row["PAT_ID"].ToString());
                }
                if (row["REGPAT_ID"] != null && row["REGPAT_ID"].ToString() != "")
                {
                    model.REGPAT_ID = int.Parse(row["REGPAT_ID"].ToString());
                }
                if (row["OPT_SN"] != null)
                {
                    model.OPT_SN = row["OPT_SN"].ToString();
                }
                if (row["HOS_SN"] != null)
                {
                    model.HOS_SN = row["HOS_SN"].ToString();
                }
                if (row["SFZ_NO"] != null)
                {
                    model.SFZ_NO = row["SFZ_NO"].ToString();
                }
                if (row["YLCARD_TYPE"] != null && row["YLCARD_TYPE"].ToString() != "")
                {
                    model.YLCARD_TYPE = int.Parse(row["YLCARD_TYPE"].ToString());
                }
                if (row["YLCARD_NO"] != null)
                {
                    model.YLCARD_NO = row["YLCARD_NO"].ToString();
                }
                if (row["PAT_NAME"] != null)
                {
                    model.PAT_NAME = row["PAT_NAME"].ToString();
                }
                if (row["SEX"] != null)
                {
                    model.SEX = row["SEX"].ToString();
                }
                if (row["DEPT_CODE"] != null)
                {
                    model.DEPT_CODE = row["DEPT_CODE"].ToString();
                }
                if (row["BIRTHDAY"] != null)
                {
                    model.BIRTHDAY = row["BIRTHDAY"].ToString();
                }
                if (row["DEPT_NAME"] != null)
                {
                    model.DEPT_NAME = row["DEPT_NAME"].ToString();
                }
                if (row["DOC_NO"] != null)
                {
                    model.DOC_NO = row["DOC_NO"].ToString();
                }
                if (row["DOC_NAME"] != null)
                {
                    model.DOC_NAME = row["DOC_NAME"].ToString();
                }
                if (row["DIS_NAME"] != null)
                {
                    model.DIS_NAME = row["DIS_NAME"].ToString();
                }
                if (row["GH_TYPE"] != null)
                {
                    model.GH_TYPE = row["GH_TYPE"].ToString();
                }
                if (row["HOS_GH_TYPE"] != null)
                {
                    model.HOS_GH_TYPE = row["HOS_GH_TYPE"].ToString();
                }
                if (row["HOS_GH_NAME"] != null)
                {
                    model.HOS_GH_NAME = row["HOS_GH_NAME"].ToString();
                }
                if (row["ZL_FEE"] != null && row["ZL_FEE"].ToString() != "")
                {
                    model.ZL_FEE = decimal.Parse(row["ZL_FEE"].ToString());
                }
                if (row["GH_FEE"] != null && row["GH_FEE"].ToString() != "")
                {
                    model.GH_FEE = decimal.Parse(row["GH_FEE"].ToString());
                }
                if (row["CASH_JE"] != null && row["CASH_JE"].ToString() != "")
                {
                    model.CASH_JE = decimal.Parse(row["CASH_JE"].ToString());
                }
                if (row["JZ_CODE"] != null)
                {
                    model.JZ_CODE = row["JZ_CODE"].ToString();
                }
                if (row["PAY_TYPE"] != null && row["PAY_TYPE"].ToString() != "")
                {
                    model.PAY_TYPE = int.Parse(row["PAY_TYPE"].ToString());
                }
                if (row["YB_SN"] != null)
                {
                    model.YB_SN = row["YB_SN"].ToString();
                }
                if (row["YB_TYPE"] != null)
                {
                    model.YB_TYPE = row["YB_TYPE"].ToString();
                }
                if (row["YB_FYLB"] != null)
                {
                    model.YB_FYLB = row["YB_FYLB"].ToString();
                }
                if (row["YB_GH_ORDER"] != null)
                {
                    model.YB_GH_ORDER = row["YB_GH_ORDER"].ToString();
                }
                if (row["YB_ZLFEE"] != null && row["YB_ZLFEE"].ToString() != "")
                {
                    model.YB_ZLFEE = decimal.Parse(row["YB_ZLFEE"].ToString());
                }
                if (row["YB_GHFEE"] != null && row["YB_GHFEE"].ToString() != "")
                {
                    model.YB_GHFEE = decimal.Parse(row["YB_GHFEE"].ToString());
                }
                if (row["XJZF"] != null && row["XJZF"].ToString() != "")
                {
                    model.XJZF = decimal.Parse(row["XJZF"].ToString());
                }
                if (row["ZHZF"] != null && row["ZHZF"].ToString() != "")
                {
                    model.ZHZF = decimal.Parse(row["ZHZF"].ToString());
                }
                if (row["ZHYE"] != null && row["ZHYE"].ToString() != "")
                {
                    model.ZHYE = decimal.Parse(row["ZHYE"].ToString());
                }
                if (row["XZM"] != null)
                {
                    model.XZM = row["XZM"].ToString();
                }
                if (row["XZMCH"] != null)
                {
                    model.XZMCH = row["XZMCH"].ToString();
                }
                if (row["YBBZM"] != null)
                {
                    model.YBBZM = row["YBBZM"].ToString();
                }
                if (row["YBBZMC"] != null)
                {
                    model.YBBZMC = row["YBBZMC"].ToString();
                }
                if (row["JE_ALL"] != null && row["JE_ALL"].ToString() != "")
                {
                    model.JE_ALL = decimal.Parse(row["JE_ALL"].ToString());
                }
                if (row["APPT_ORDER"] != null)
                {
                    model.APPT_ORDER = row["APPT_ORDER"].ToString();
                }
                if (row["APPT_SN"] != null)
                {
                    model.APPT_SN = row["APPT_SN"].ToString();
                }
                if (row["IS_DZ"] != null && row["IS_DZ"].ToString() != "")
                {
                    if ((row["IS_DZ"].ToString() == "1") || (row["IS_DZ"].ToString().ToLower() == "true"))
                    {
                        model.IS_DZ = true;
                    }
                    else
                    {
                        model.IS_DZ = false;
                    }
                }
                if (row["DJ_DATE"] != null && row["DJ_DATE"].ToString() != "")
                {
                    model.DJ_DATE = DateTime.Parse(row["DJ_DATE"].ToString());
                }
                if (row["DJ_TIME"] != null)
                {
                    model.DJ_TIME = row["DJ_TIME"].ToString();
                }
                if (row["IS_TH"] != null && row["IS_TH"].ToString() != "")
                {
                    if ((row["IS_TH"].ToString() == "1") || (row["IS_TH"].ToString().ToLower() == "true"))
                    {
                        model.IS_TH = true;
                    }
                    else
                    {
                        model.IS_TH = false;
                    }
                }
                if (row["RH_DATE"] != null && row["RH_DATE"].ToString() != "")
                {
                    model.RH_DATE = DateTime.Parse(row["RH_DATE"].ToString());
                }
                if (row["TH_TIME"] != null)
                {
                    model.TH_TIME = row["TH_TIME"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,REG_ID,HOS_ID,PAT_ID,REGPAT_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME ");
            strSql.Append(" FROM register_pay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM register_pay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLZZJ.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.PAY_ID desc");
            }
            strSql.Append(")AS Row, T.*  from register_pay T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        /// <summary>
        /// 自助机预约支付保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="info"></param>
        /// <param name="zfb"></param>
        /// <param name="wc"></param>
        /// <param name="bank"></param>
        /// <param name="upcap"></param>
        /// <param name="ccb"></param>
        /// <returns></returns>
        public bool AddByTran_ZZJ(Plat.Model.register_pay model, Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Plat.Model.pay_info_wc wc, Plat.Model.pay_info_bank bank, Plat.Model.pay_info_upcap upcap, Plat.Model.pay_info_ccb ccb, Plat.Model.pay_info_yb yb)
        {
            //门诊预约挂号支付记录表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_pay(");
            strSql.Append("PAY_ID,REG_ID,HOS_ID,PAT_ID,USER_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME,SOURCE,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@REG_ID,@HOS_ID,@PAT_ID,@USER_ID,@OPT_SN,@HOS_SN,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@SEX,@DEPT_CODE,@BIRTHDAY,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_GH_TYPE,@HOS_GH_NAME,@ZL_FEE,@GH_FEE,@CASH_JE,@JZ_CODE,@PAY_TYPE,@YB_SN,@YB_TYPE,@YB_FYLB,@YB_GH_ORDER,@YB_ZLFEE,@YB_GHFEE,@XJZF,@ZHZF,@ZHYE,@XZM,@XZMCH,@YBBZM,@YBBZMC,@JE_ALL,@APPT_ORDER,@APPT_SN,@IS_DZ,@DJ_DATE,@DJ_TIME,@IS_TH,@RH_DATE,@TH_TIME,@SOURCE,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@USER_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_GH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_GH_NAME", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,5),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YB_SN", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_GH_ORDER", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_ZLFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@YB_GHFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@JE_ALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_DZ", MySqlDbType.Bit),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TH", MySqlDbType.Bit),
					new MySqlParameter("@RH_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@lTERMINAL_SN",MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.REG_ID;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.USER_ID;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_SN;
            parameters[7].Value = model.SFZ_NO;
            parameters[8].Value = model.YLCARD_TYPE;
            parameters[9].Value = model.YLCARD_NO;
            parameters[10].Value = model.PAT_NAME;
            parameters[11].Value = model.SEX;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.BIRTHDAY;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME;
            parameters[18].Value = model.GH_TYPE;
            parameters[19].Value = model.HOS_GH_TYPE;
            parameters[20].Value = model.HOS_GH_NAME;
            parameters[21].Value = model.ZL_FEE;
            parameters[22].Value = model.GH_FEE;
            parameters[23].Value = model.CASH_JE;
            parameters[24].Value = model.JZ_CODE;
            parameters[25].Value = model.PAY_TYPE;
            parameters[26].Value = model.YB_SN;
            parameters[27].Value = model.YB_TYPE;
            parameters[28].Value = model.YB_FYLB;
            parameters[29].Value = model.YB_GH_ORDER;
            parameters[30].Value = model.YB_ZLFEE;
            parameters[31].Value = model.YB_GHFEE;
            parameters[32].Value = model.XJZF;
            parameters[33].Value = model.ZHZF;
            parameters[34].Value = model.ZHYE;
            parameters[35].Value = model.XZM;
            parameters[36].Value = model.XZMCH;
            parameters[37].Value = model.YBBZM;
            parameters[38].Value = model.YBBZMC;
            parameters[39].Value = model.JE_ALL;
            parameters[40].Value = model.APPT_ORDER;
            parameters[41].Value = model.APPT_SN;
            parameters[42].Value = model.IS_DZ;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.IS_TH;
            parameters[46].Value = model.RH_DATE;
            parameters[47].Value = model.TH_TIME;
            parameters[48].Value = model.SOURCE;
            parameters[49].Value = model.lTERMINAL_SN;


            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql, parameters);



            //现金交易记录表
            strSql = new StringBuilder();
            strSql.Append("insert into pay_info(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,USER_ID,BIZ_TYPE,BIZ_SN,CASH_JE,SFZ_NO,DJ_DATE,DJ_TIME,DEAL_TYPE,DEAL_STATES,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@USER_ID,@BIZ_TYPE,@BIZ_SN,@CASH_JE,@SFZ_NO,@DJ_DATE,@DJ_TIME,@DEAL_TYPE,@DEAL_STATES,@lTERMINAL_SN)");
            MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@USER_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEAL_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters1[0].Value = info.PAY_ID;
            parameters1[1].Value = info.HOS_ID;
            parameters1[2].Value = info.PAT_ID;
            parameters1[3].Value = info.USER_ID;
            parameters1[4].Value = info.BIZ_TYPE;
            parameters1[5].Value = info.BIZ_SN;
            parameters1[6].Value = info.CASH_JE;
            parameters1[7].Value = info.SFZ_NO;
            parameters1[8].Value = info.DJ_DATE;
            parameters1[9].Value = info.DJ_TIME;
            parameters1[10].Value = info.DEAL_TYPE;
            parameters1[11].Value = info.DEAL_STATES;
            parameters1[12].Value = info.lTERMINAL_SN;

            table.Add(strSql, parameters1);

            if (zfb != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_zfb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BIZ_SN,SELLER_ID,COMM_SN,JE,DEAL_STATES,TXN_TYPE,DEAL_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BIZ_SN,@SELLER_ID,@COMM_SN,@JE,@DEAL_STATES,@TXN_TYPE,@DEAL_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SELLER_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,20),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEAL_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
                parameters2[0].Value = zfb.PAY_ID;
                parameters2[1].Value = zfb.BIZ_TYPE;
                parameters2[2].Value = zfb.BIZ_SN;
                parameters2[3].Value = zfb.SELLER_ID;
                parameters2[4].Value = zfb.COMM_SN;
                parameters2[5].Value = zfb.JE;
                parameters2[6].Value = zfb.DEAL_STATES;
                parameters2[7].Value = zfb.TXN_TYPE;
                parameters2[8].Value = zfb.DEAL_TIME;
                parameters2[9].Value = zfb.lTERMINAL_SN;

                table.Add(strSql, parameters2);
            }
            else if (wc != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_wc(");
                strSql.Append("PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TXN_TYPE)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@WECHAT,@PAY_TYPE,@BIZ_TYPE,@BIZ_SN,@COMM_SN,@JE,@COMM_NAME,@DEAL_STATES,@DEAL_TIME,@DEAL_SN,@lTERMINAL_SN,@TXN_TYPE)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@WECHAT", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@COMM_NAME", MySqlDbType.VarChar,50),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEAL_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DEAL_SN", MySqlDbType.VarChar,50),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10)};
                parameters2[0].Value = wc.PAY_ID;
                parameters2[1].Value = wc.WECHAT;
                parameters2[2].Value = wc.PAY_TYPE;
                parameters2[3].Value = wc.BIZ_TYPE;
                parameters2[4].Value = wc.BIZ_SN;
                parameters2[5].Value = wc.COMM_SN;
                parameters2[6].Value = wc.JE;
                parameters2[7].Value = wc.COMM_NAME;
                parameters2[8].Value = wc.DEAL_STATES;
                parameters2[9].Value = wc.DEAL_TIME;
                parameters2[10].Value = wc.DEAL_SN;
                parameters2[11].Value = wc.lTERMINAL_SN;
                parameters2[12].Value = wc.TNX_TYPE;

                table.Add(strSql, parameters2);
            }
            else if (bank != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_bank(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,BANK_TYPE,RETURN_CODE,BANK_CARD,CARD_TYPE,SEARCH_CODE,REFCODE,TERMCODE,CARD_COMPAY,COMM_NAME,COMM_SN,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@BANK_TYPE,@RETURN_CODE,@BANK_CARD,@CARD_TYPE,@SEARCH_CODE,@REFCODE,@TERMCODE,@CARD_COMPAY,@COMM_NAME,@COMM_SN,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@BANK_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@RETURN_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BANK_CARD", MySqlDbType.VarChar,19),
					new MySqlParameter("@CARD_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEARCH_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
					new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
					new MySqlParameter("@CARD_COMPAY", MySqlDbType.VarChar,20),
					new MySqlParameter("@COMM_NAME", MySqlDbType.VarChar,50),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,50),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime)};
                parameters2[0].Value = bank.PAY_ID;
                parameters2[1].Value = bank.BIZ_TYPE;
                parameters2[2].Value = bank.BDj_id;
                parameters2[3].Value = bank.JE;
                parameters2[4].Value = bank.BANK_TYPE;
                parameters2[5].Value = bank.RETURN_CODE;
                parameters2[6].Value = bank.BANK_CARD;
                parameters2[7].Value = bank.CARD_TYPE;
                parameters2[8].Value = bank.SEARCH_CODE;
                parameters2[9].Value = bank.REFCODE;
                parameters2[10].Value = bank.TERMCODE;
                parameters2[11].Value = bank.CARD_COMPAY;
                parameters2[12].Value = bank.COMM_NAME;
                parameters2[13].Value = bank.COMM_SN;
                parameters2[14].Value = bank.SFZ_NO;
                parameters2[15].Value = bank.DJ_TIME;
                table.Add(strSql, parameters2);
            }
            else if (upcap != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_upcap(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,TXN_TYPE,MERID,ORDERID,TN,QUERYID,REFCODE,TERMCODE,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@TXN_TYPE,@MERID,@ORDERID,@TN,@QUERYID,@REFCODE,@TERMCODE,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters6 = {
                    new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
                    new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
                    new MySqlParameter("@JE", MySqlDbType.Decimal,10),
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@MERID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@TN", MySqlDbType.VarChar,21),
                    new MySqlParameter("@QUERYID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
                    new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
                    new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
                    new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime)};
                parameters6[0].Value = upcap.PAY_ID;
                parameters6[1].Value = upcap.BIZ_TYPE;
                parameters6[2].Value = upcap.BDj_id;
                parameters6[3].Value = upcap.JE;
                parameters6[4].Value = upcap.TXN_TYPE;
                parameters6[5].Value = upcap.MERID;
                parameters6[6].Value = upcap.ORDERID;
                parameters6[7].Value = upcap.TN;
                parameters6[8].Value = upcap.QUERYID;
                parameters6[9].Value = upcap.REFCODE;
                parameters6[10].Value = upcap.TERMCODE;
                parameters6[11].Value = upcap.SFZ_NO;
                parameters6[12].Value = upcap.DJ_TIME;

                table.Add(strSql, parameters6);
            }
            else if (ccb != null)
            {

                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_ccb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE,MerchantID)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE,@MerchantID)");
                MySqlParameter[] parameters7 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@POSID", MySqlDbType.VarChar,20),
					new MySqlParameter("@BRANCHID", MySqlDbType.VarChar,20),
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@MerchantID", MySqlDbType.VarChar,20)};
                parameters7[0].Value = ccb.PAY_ID;
                parameters7[1].Value = ccb.BIZ_TYPE;
                parameters7[2].Value = ccb.BDj_id;
                parameters7[3].Value = ccb.JE;
                parameters7[4].Value = ccb.POSID;
                parameters7[5].Value = ccb.BRANCHID;
                parameters7[6].Value = ccb.ORDERID;
                parameters7[7].Value = ccb.SFZ_NO;
                parameters7[8].Value = ccb.DJ_TIME;
                parameters7[9].Value = ccb.TXN_TYPE;
                parameters7[10].Value = ccb.MerchantID;
                table.Add(strSql, parameters7);
            }
            //预约表修改支付状态
            strSql = new StringBuilder();
            strSql.Append("update register_appt set PAY_STATUS=1,APPT_TYPE='1' where REG_ID=@REG_ID");
            MySqlParameter[] parameters3 = {
							new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)};
            parameters3[0].Value = model.REG_ID;
            table.Add(strSql, parameters3);

            #region
            if (yb != null)
            {

                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_yb(");
                strSql.Append("PAY_ID,DJ_ID,TRADELSH,MZLSH,DJLSH,YL_TYPE,JS_DATE,CY_DATE,CY_REASON,DIS_CODE,YJSTYPE,ZTYJSTYPE,USR_NAME,FM_DATE,CC,TAIER_AMOUNT,CARDID,ZYYBBH,DEPT_CODE,DOC_NO,IS_GH,ZSRCARDID,SS_ISSUCCESS,BC_ZJE,BC_TCJE,BC_DBJZ,BC_DBBX,BC_MZBZ,BC_ZHZC,BC_XJZC,BC_ZHZCZF,BC_ZHZCZL,BC_XJZCZF,BC_XJZCZL,YBFWNJE,ZHYE,DBZ_CODE,INSTRUCTION,MED_JE,CHK_JE,BB_JE,BY6,lTERMINAL_SN,DEAL_TIME,YWZQH)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@DJ_ID,@TRADELSH,@MZLSH,@DJLSH,@YL_TYPE,@JS_DATE,@CY_DATE,@CY_REASON,@DIS_CODE,@YJSTYPE,@ZTYJSTYPE,@USR_NAME,@FM_DATE,@CC,@TAIER_AMOUNT,@CARDID,@ZYYBBH,@DEPT_CODE,@DOC_NO,@IS_GH,@ZSRCARDID,@SS_ISSUCCESS,@BC_ZJE,@BC_TCJE,@BC_DBJZ,@BC_DBBX,@BC_MZBZ,@BC_ZHZC,@BC_XJZC,@BC_ZHZCZF,@BC_ZHZCZL,@BC_XJZCZF,@BC_XJZCZL,@YBFWNJE,@ZHYE,@DBZ_CODE,@INSTRUCTION,@MED_JE,@CHK_JE,@BB_JE,@BY6,@lTERMINAL_SN,@DEAL_TIME,@YWZQH)");
                MySqlParameter[] parameters8 = {
                    new MySqlParameter("@PAY_ID", MySqlDbType.Int16,4),
                    new MySqlParameter("@DJ_ID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@TRADELSH", MySqlDbType.VarChar,30),
                    new MySqlParameter("@MZLSH", MySqlDbType.VarChar,30),
                    new MySqlParameter("@DJLSH", MySqlDbType.VarChar,30),
                    new MySqlParameter("@YL_TYPE", MySqlDbType.VarChar,20),
                    new MySqlParameter("@JS_DATE", MySqlDbType.VarChar,30),
                    new MySqlParameter("@CY_DATE", MySqlDbType.VarChar,18),
                    new MySqlParameter("@CY_REASON", MySqlDbType.VarChar,1000),
                    new MySqlParameter("@DIS_CODE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@YJSTYPE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ZTYJSTYPE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@USR_NAME", MySqlDbType.VarChar,10),
                    new MySqlParameter("@FM_DATE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@CC", MySqlDbType.VarChar,10),
                    new MySqlParameter("@TAIER_AMOUNT", MySqlDbType.VarChar,10),
                    new MySqlParameter("@CARDID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@ZYYBBH", MySqlDbType.VarChar,10),
                    new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
                    new MySqlParameter("@IS_GH", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ZSRCARDID", MySqlDbType.VarChar,10),
                    new MySqlParameter("@SS_ISSUCCESS", MySqlDbType.VarChar,10),
                    new MySqlParameter("@BC_ZJE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_TCJE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_DBJZ", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_DBBX", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_MZBZ", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_ZHZC", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_XJZC", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_ZHZCZF", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_ZHZCZL", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_XJZCZF", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BC_XJZCZL", MySqlDbType.VarChar,16),
                    new MySqlParameter("@YBFWNJE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@ZHYE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@DBZ_CODE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@INSTRUCTION", MySqlDbType.VarChar,4000),
                    new MySqlParameter("@MED_JE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@CHK_JE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BB_JE", MySqlDbType.VarChar,16),
                    new MySqlParameter("@BY6", MySqlDbType.VarChar,20),
                    new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,10),
                    new MySqlParameter("@DEAL_TIME", MySqlDbType.Datetime),
                   new MySqlParameter("@YWZQH", MySqlDbType.VarChar,100), };
                parameters8[0].Value = yb.PAY_ID;
                parameters8[1].Value = yb.DJ_ID;
                parameters8[2].Value = yb.TRADELSH;
                parameters8[3].Value = yb.MZLSH;
                parameters8[4].Value = yb.DJLSH;
                parameters8[5].Value = yb.YL_TYPE;
                parameters8[6].Value = yb.JS_DATE;
                parameters8[7].Value = yb.CY_DATE;
                parameters8[8].Value = yb.CY_REASON;
                parameters8[9].Value = yb.DIS_CODE;
                parameters8[10].Value = yb.YJSTYPE;
                parameters8[11].Value = yb.ZTYJSTYPE;
                parameters8[12].Value = yb.USR_NAME;
                parameters8[13].Value = yb.FM_DATE;
                parameters8[14].Value = yb.CC;
                parameters8[15].Value = yb.TAIER_AMOUNT;
                parameters8[16].Value = yb.CARDID;
                parameters8[17].Value = yb.ZYYBBH;
                parameters8[18].Value = yb.DEPT_CODE;
                parameters8[19].Value = yb.DOC_NO;
                parameters8[20].Value = yb.IS_GH;
                parameters8[21].Value = yb.ZSRCARDID;
                parameters8[22].Value = yb.SS_ISSUCCESS;
                parameters8[23].Value = yb.BC_ZJE;
                parameters8[24].Value = yb.BC_TCJE;
                parameters8[25].Value = yb.BC_DBJZ;
                parameters8[26].Value = yb.BC_DBBX;
                parameters8[27].Value = yb.BC_MZBZ;
                parameters8[28].Value = yb.BC_ZHZC;
                parameters8[29].Value = yb.BC_XJZC;
                parameters8[30].Value = yb.BC_ZHZCZF;
                parameters8[31].Value = yb.BC_ZHZCZL;
                parameters8[32].Value = yb.BC_XJZCZF;
                parameters8[33].Value = yb.BC_XJZCZL;
                parameters8[34].Value = yb.YBFWNJE;
                parameters8[35].Value = yb.ZHYE;
                parameters8[36].Value = yb.DBZ_CODE;
                parameters8[37].Value = yb.INSTRUCTION;
                parameters8[38].Value = yb.MED_JE;
                parameters8[39].Value = yb.CHK_JE;
                parameters8[40].Value = yb.BB_JE;
                parameters8[41].Value = yb.BY6;
                parameters8[42].Value = yb.lTERMINAL_SN;
                parameters8[43].Value = yb.DEAL_TIME;
                parameters8[44].Value = yb.YWZQH;
                table.Add(strSql, parameters8);
            }
            #endregion
            try
            {
                DbHelperMySQLZZJ.ExecuteSqlTran(table);
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    ModSqlError modSqlError = new ModSqlError();
                    modSqlError.TYPE = "挂号支付收费保存";
                    modSqlError.time = DateTime.Now;
                    modSqlError.EXCEPTION = ex.ToString().Replace("'", "\"");
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(modSqlError);
                }
                catch
                {

                }
                return false;
            }

        }



        public bool AddByTran(Plat.Model.register_pay model, Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Plat.Model.pay_info_wc wc, Plat.Model.pay_info_bank bank, Plat.Model.pay_info_upcap upcap, Plat.Model.pay_info_ccb ccb)
        {
            //门诊预约挂号支付记录表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_pay(");
            strSql.Append("PAY_ID,REG_ID,HOS_ID,PAT_ID,REGPAT_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME,SOURCE)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@REG_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@OPT_SN,@HOS_SN,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@SEX,@DEPT_CODE,@BIRTHDAY,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_GH_TYPE,@HOS_GH_NAME,@ZL_FEE,@GH_FEE,@CASH_JE,@JZ_CODE,@PAY_TYPE,@YB_SN,@YB_TYPE,@YB_FYLB,@YB_GH_ORDER,@YB_ZLFEE,@YB_GHFEE,@XJZF,@ZHZF,@ZHYE,@XZM,@XZMCH,@YBBZM,@YBBZMC,@JE_ALL,@APPT_ORDER,@APPT_SN,@IS_DZ,@DJ_DATE,@DJ_TIME,@IS_TH,@RH_DATE,@TH_TIME,@SOURCE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_GH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_GH_NAME", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,5),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YB_SN", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YB_GH_ORDER", MySqlDbType.VarChar,15),
					new MySqlParameter("@YB_ZLFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@YB_GHFEE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,6),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,6),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@JE_ALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_DZ", MySqlDbType.Bit),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TH", MySqlDbType.Bit),
					new MySqlParameter("@RH_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.REG_ID;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.REGPAT_ID;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_SN;
            parameters[7].Value = model.SFZ_NO;
            parameters[8].Value = model.YLCARD_TYPE;
            parameters[9].Value = model.YLCARD_NO;
            parameters[10].Value = model.PAT_NAME;
            parameters[11].Value = model.SEX;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.BIRTHDAY;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME;
            parameters[18].Value = model.GH_TYPE;
            parameters[19].Value = model.HOS_GH_TYPE;
            parameters[20].Value = model.HOS_GH_NAME;
            parameters[21].Value = model.ZL_FEE;
            parameters[22].Value = model.GH_FEE;
            parameters[23].Value = model.CASH_JE;
            parameters[24].Value = model.JZ_CODE;
            parameters[25].Value = model.PAY_TYPE;
            parameters[26].Value = model.YB_SN;
            parameters[27].Value = model.YB_TYPE;
            parameters[28].Value = model.YB_FYLB;
            parameters[29].Value = model.YB_GH_ORDER;
            parameters[30].Value = model.YB_ZLFEE;
            parameters[31].Value = model.YB_GHFEE;
            parameters[32].Value = model.XJZF;
            parameters[33].Value = model.ZHZF;
            parameters[34].Value = model.ZHYE;
            parameters[35].Value = model.XZM;
            parameters[36].Value = model.XZMCH;
            parameters[37].Value = model.YBBZM;
            parameters[38].Value = model.YBBZMC;
            parameters[39].Value = model.JE_ALL;
            parameters[40].Value = model.APPT_ORDER;
            parameters[41].Value = model.APPT_SN;
            parameters[42].Value = model.IS_DZ;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.IS_TH;
            parameters[46].Value = model.RH_DATE;
            parameters[47].Value = model.TH_TIME;
            parameters[48].Value = model.SOURCE;


            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql, parameters);



            //现金交易记录表
            strSql = new StringBuilder();
            strSql.Append("insert into pay_info(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,BIZ_TYPE,BIZ_SN,CASH_JE,SFZ_NO,DJ_DATE,DJ_TIME,DEAL_TYPE,DEAL_STATES,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@BIZ_TYPE,@BIZ_SN,@CASH_JE,@SFZ_NO,@DJ_DATE,@DJ_TIME,@DEAL_TYPE,@DEAL_STATES,@lTERMINAL_SN)");
            MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEAL_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters1[0].Value = info.PAY_ID;
            parameters1[1].Value = info.HOS_ID;
            parameters1[2].Value = info.PAT_ID;
            parameters1[3].Value = info.REGPAT_ID;
            parameters1[4].Value = info.BIZ_TYPE;
            parameters1[5].Value = info.BIZ_SN;
            parameters1[6].Value = info.CASH_JE;
            parameters1[7].Value = info.SFZ_NO;
            parameters1[8].Value = info.DJ_DATE;
            parameters1[9].Value = info.DJ_TIME;
            parameters1[10].Value = info.DEAL_TYPE;
            parameters1[11].Value = info.DEAL_STATES;
            parameters1[12].Value = info.lTERMINAL_SN;

            table.Add(strSql, parameters1);

            if (zfb != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_zfb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BIZ_SN,SELLER_ID,COMM_SN,JE,DEAL_STATES,TXN_TYPE,DEAL_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BIZ_SN,@SELLER_ID,@COMM_SN,@JE,@DEAL_STATES,@TXN_TYPE,@DEAL_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@SELLER_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,20),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEAL_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
                parameters2[0].Value = zfb.PAY_ID;
                parameters2[1].Value = zfb.BIZ_TYPE;
                parameters2[2].Value = zfb.BIZ_SN;
                parameters2[3].Value = zfb.SELLER_ID;
                parameters2[4].Value = zfb.COMM_SN;
                parameters2[5].Value = zfb.JE;
                parameters2[6].Value = zfb.DEAL_STATES;
                parameters2[7].Value = zfb.TXN_TYPE;
                parameters2[8].Value = zfb.DEAL_TIME;
                parameters2[9].Value = zfb.lTERMINAL_SN;

                table.Add(strSql, parameters2);
            }
            else if (wc != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_wc(");
                strSql.Append("PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TXN_TYPE)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@WECHAT,@PAY_TYPE,@BIZ_TYPE,@BIZ_SN,@COMM_SN,@JE,@COMM_NAME,@DEAL_STATES,@DEAL_TIME,@DEAL_SN,@lTERMINAL_SN,@TXN_TYPE)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@WECHAT", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BIZ_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@COMM_NAME", MySqlDbType.VarChar,50),
					new MySqlParameter("@DEAL_STATES", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEAL_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@DEAL_SN", MySqlDbType.VarChar,50),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10)};
                parameters2[0].Value = wc.PAY_ID;
                parameters2[1].Value = wc.WECHAT;
                parameters2[2].Value = wc.PAY_TYPE;
                parameters2[3].Value = wc.BIZ_TYPE;
                parameters2[4].Value = wc.BIZ_SN;
                parameters2[5].Value = wc.COMM_SN;
                parameters2[6].Value = wc.JE;
                parameters2[7].Value = wc.COMM_NAME;
                parameters2[8].Value = wc.DEAL_STATES;
                parameters2[9].Value = wc.DEAL_TIME;
                parameters2[10].Value = wc.DEAL_SN;
                parameters2[11].Value = wc.lTERMINAL_SN;
                parameters2[12].Value = wc.TNX_TYPE;

                table.Add(strSql, parameters2);
            }
            else if (bank != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_bank(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,BANK_TYPE,RETURN_CODE,BANK_CARD,CARD_TYPE,SEARCH_CODE,REFCODE,TERMCODE,CARD_COMPAY,COMM_NAME,COMM_SN,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@BANK_TYPE,@RETURN_CODE,@BANK_CARD,@CARD_TYPE,@SEARCH_CODE,@REFCODE,@TERMCODE,@CARD_COMPAY,@COMM_NAME,@COMM_SN,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@BANK_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@RETURN_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@BANK_CARD", MySqlDbType.VarChar,19),
					new MySqlParameter("@CARD_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEARCH_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
					new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
					new MySqlParameter("@CARD_COMPAY", MySqlDbType.VarChar,20),
					new MySqlParameter("@COMM_NAME", MySqlDbType.VarChar,50),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,50),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime)};
                parameters2[0].Value = bank.PAY_ID;
                parameters2[1].Value = bank.BIZ_TYPE;
                parameters2[2].Value = bank.BDj_id;
                parameters2[3].Value = bank.JE;
                parameters2[4].Value = bank.BANK_TYPE;
                parameters2[5].Value = bank.RETURN_CODE;
                parameters2[6].Value = bank.BANK_CARD;
                parameters2[7].Value = bank.CARD_TYPE;
                parameters2[8].Value = bank.SEARCH_CODE;
                parameters2[9].Value = bank.REFCODE;
                parameters2[10].Value = bank.TERMCODE;
                parameters2[11].Value = bank.CARD_COMPAY;
                parameters2[12].Value = bank.COMM_NAME;
                parameters2[13].Value = bank.COMM_SN;
                parameters2[14].Value = bank.SFZ_NO;
                parameters2[15].Value = bank.DJ_TIME;
                table.Add(strSql, parameters2);
            }
            else if (upcap != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_upcap(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,TXN_TYPE,MERID,ORDERID,TN,QUERYID,REFCODE,TERMCODE,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@TXN_TYPE,@MERID,@ORDERID,@TN,@QUERYID,@REFCODE,@TERMCODE,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters6 = {
                    new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
                    new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
                    new MySqlParameter("@JE", MySqlDbType.Decimal,10),
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@MERID", MySqlDbType.VarChar,50),
                    new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@TN", MySqlDbType.VarChar,21),
                    new MySqlParameter("@QUERYID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
                    new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
                    new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
                    new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime)};
                parameters6[0].Value = upcap.PAY_ID;
                parameters6[1].Value = upcap.BIZ_TYPE;
                parameters6[2].Value = upcap.BDj_id;
                parameters6[3].Value = upcap.JE;
                parameters6[4].Value = upcap.TXN_TYPE;
                parameters6[5].Value = upcap.MERID;
                parameters6[6].Value = upcap.ORDERID;
                parameters6[7].Value = upcap.TN;
                parameters6[8].Value = upcap.QUERYID;
                parameters6[9].Value = upcap.REFCODE;
                parameters6[10].Value = upcap.TERMCODE;
                parameters6[11].Value = upcap.SFZ_NO;
                parameters6[12].Value = upcap.DJ_TIME;

                table.Add(strSql, parameters6);
            }
            else if (ccb != null)
            {
                //strSql = new StringBuilder();
                //strSql.Append("insert into pay_info_ccb(");
                //strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE)");
                //strSql.Append(" values (");
                //strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE)");
                //MySqlParameter[] parameters7 = {
                //    new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
                //    new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
                //    new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
                //    new MySqlParameter("@JE", MySqlDbType.Decimal,10),
                //    new MySqlParameter("@POSID", MySqlDbType.VarChar,9),
                //    new MySqlParameter("@BRANCHID", MySqlDbType.VarChar,9),
                //    new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
                //    new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
                //    new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
                //    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10)};
                //parameters7[0].Value = ccb.PAY_ID;
                //parameters7[1].Value = ccb.BIZ_TYPE;
                //parameters7[2].Value = ccb.BDj_id;
                //parameters7[3].Value = ccb.JE;
                //parameters7[4].Value = ccb.POSID;
                //parameters7[5].Value = ccb.BRANCHID;
                //parameters7[6].Value = ccb.ORDERID;
                //parameters7[7].Value = ccb.SFZ_NO;
                //parameters7[8].Value = ccb.DJ_TIME;
                //parameters7[9].Value = ccb.TXN_TYPE;

                //table.Add(strSql, parameters7);


                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_ccb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE,MerchantID)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE,@MerchantID)");
                MySqlParameter[] parameters7 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@BIZ_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@BDj_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@POSID", MySqlDbType.VarChar,20),
					new MySqlParameter("@BRANCHID", MySqlDbType.VarChar,20),
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@MerchantID", MySqlDbType.VarChar,20)};
                parameters7[0].Value = ccb.PAY_ID;
                parameters7[1].Value = ccb.BIZ_TYPE;
                parameters7[2].Value = ccb.BDj_id;
                parameters7[3].Value = ccb.JE;
                parameters7[4].Value = ccb.POSID;
                parameters7[5].Value = ccb.BRANCHID;
                parameters7[6].Value = ccb.ORDERID;
                parameters7[7].Value = ccb.SFZ_NO;
                parameters7[8].Value = ccb.DJ_TIME;
                parameters7[9].Value = ccb.TXN_TYPE;
                parameters7[10].Value = ccb.MerchantID;
                table.Add(strSql, parameters7);
            }
            //预约表修改支付状态
            strSql = new StringBuilder();
            strSql.Append("update register_appt set PAY_STATUS=1,APPT_TYPE='1' where REG_ID=@REG_ID");
            MySqlParameter[] parameters3 = {
							new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)};
            parameters3[0].Value = model.REG_ID;
            table.Add(strSql, parameters3);

            try
            {
                DbHelperMySQLZZJ.ExecuteSqlTran(table);
                return true;
            }
            catch(Exception ex)
            {
                try
                {
                    ModSqlError modSqlError = new ModSqlError();
                    modSqlError.TYPE = "挂号支付收费保存";
                    modSqlError.time = DateTime.Now;
                    modSqlError.EXCEPTION = ex.ToString().Replace("'", "\"");
                    new Log.Core.MySQLDAL.DalSqlERRROR().Add(modSqlError);
                }
                catch
                {

                }
                return false;
            }

        }


        public Plat.Model.register_pay GetModeByIDs(int REG_ID, string HOS_ID, int PAT_ID, int REGPAT_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,REG_ID,HOS_ID,PAT_ID,REGPAT_ID,OPT_SN,HOS_SN,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,SEX,DEPT_CODE,BIRTHDAY,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,GH_TYPE,HOS_GH_TYPE,HOS_GH_NAME,ZL_FEE,GH_FEE,CASH_JE,JZ_CODE,PAY_TYPE,YB_SN,YB_TYPE,YB_FYLB,YB_GH_ORDER,YB_ZLFEE,YB_GHFEE,XJZF,ZHZF,ZHYE,XZM,XZMCH,YBBZM,YBBZMC,JE_ALL,APPT_ORDER,APPT_SN,IS_DZ,DJ_DATE,DJ_TIME,IS_TH,RH_DATE,TH_TIME from register_pay ");
            strSql.Append(" where REG_ID=@REG_ID and  HOS_ID=@HOS_ID and PAT_ID=@PAT_ID and REGPAT_ID=@REGPAT_ID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11)};
            parameters[0].Value = REG_ID;
            parameters[1].Value = HOS_ID;
            parameters[2].Value = PAT_ID;
            parameters[3].Value = REGPAT_ID;

            Plat.Model.register_pay model = new Plat.Model.register_pay();
            DataSet ds = DbHelperMySQLZZJ.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            MySqlParameter[] parameters = {
                    new MySqlParameter("@tblName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@fldName", MySqlDbType.VarChar, 255),
                    new MySqlParameter("@PageSize", MySqlDbType.Int32),
                    new MySqlParameter("@PageIndex", MySqlDbType.Int32),
                    new MySqlParameter("@IsReCount", MySqlDbType.Bit),
                    new MySqlParameter("@OrderType", MySqlDbType.Bit),
                    new MySqlParameter("@strWhere", MySqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "register_pay";
            parameters[1].Value = "PAY_ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperMySQLZZJ.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        /// <summary>
        /// 医院查询收费信息
        /// </summary>
        /// <param name="HOS_ID">医院代码</param>
        /// <param name="BEGIN_DATE">开始日期</param>
        /// <param name="END_DATE">结束日期</param>
        /// <param name="PZ_NO">凭证号 门诊号或发票号 为空查询所有</param>
        /// <param name="TYPE">1 预约挂号 2 门诊 3住院</param>
        /// <returns></returns>
        public DataTable GetSFINFO(string HOS_ID, string BEGIN_DATE, string END_DATE, string PZ_NO, string TYPE)
        {
            string sqlcmd = "";
            string where = " ";
            string sqlcmd0 = "";
            string sqlcmd1 = "";
            string sqlcmd2 = "";
            string sqlcmd3 = "";
            //if (TYPE == "1")
            {
                where = PZ_NO == "" ? "" : " and b.OPT_SN='" + PZ_NO + "'";
                sqlcmd1 = string.Format(@"select DISTINCT '0' as PAY_ID, a.PAT_NAME,c.je as JE_ALL,b.OPT_SN as PZ_NO,CONCAT(b.DJ_DATE,' ',b.DJ_TIME) as DJ_TIME,c.ORDERID as QUERYID,
(case when b.IS_TH=0 then '1' else '2' end) as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'5' as DEAL_TYPE,c.txn_Type,1 as BIZ_TYPE   from register_appt a,register_pay b,pay_info_ccb c,pat_info d
where a.REG_ID=b.reg_id and b.PAY_ID=c.BDj_id and a.pat_id=d.pat_id
and a.appt_type in(1,3) and a.HOS_ID='{0}' and b.dj_date between '{1}' and '{2}' {3}
union
select DISTINCT '0' as PAY_ID,a.PAT_NAME,c.je as JE_ALL,b.OPT_SN as PZ_NO,CONCAT(b.DJ_DATE,' ',b.DJ_TIME) as DJ_TIME,c.COMM_SN as QUERYID,
(case when b.IS_TH=0 then '1' else '2' end) as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO ,'2'as DEAL_TYPE,c.txn_Type,1 as BIZ_TYPE
from register_appt a,register_pay b,pay_info_zfb c,pat_info d
where a.REG_ID=b.reg_id and b.PAY_ID=c.BIZ_SN and a.pat_id=d.pat_id
and a.appt_type in(1,3) and a.HOS_ID='{0}' and b.dj_date between '{1}' and '{2}' {3}
union
select DISTINCT '0' as PAY_ID,a.PAT_NAME,c.je/100 as JE_ALL,b.OPT_SN as PZ_NO,CONCAT(b.DJ_DATE,' ',b.DJ_TIME) as DJ_TIME,c.orderid as QUERYID,
(case when b.IS_TH=0 then '1' else '2' end) as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO ,'3'as DEAL_TYPE,c.txn_Type,1 as BIZ_TYPE
from register_appt a,register_pay b,pay_info_upcap c,pat_info d
where a.REG_ID=b.reg_id and b.PAY_ID=c.bdj_id and a.pat_id=d.pat_id
and a.appt_type in(1,3) and a.HOS_ID='{0}' and b.dj_date between '{1}' and '{2}' {3}
", HOS_ID, BEGIN_DATE, END_DATE, where);


                //if (PZ_NO != "")
                //{
                //    sqlcmd += " and b.OPT_SN='" + PZ_NO + "'";
                //}
            }
            //else if (TYPE == "2")
            {
                where = PZ_NO == "" ? "" : " and a.RCPT_NO='" + PZ_NO + "'";
                sqlcmd2 = string.Format(@"select  a.PAY_ID,a.PAT_NAME,a.CASH_JE as JE_ALL,a.RCPT_NO as PZ_NO,CONCAT(a.DJ_DATE,' ',a.DJ_TIME) as DJ_TIME, b.ORDERID as QUERYID,1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'5' as DEAL_TYPE,b.txn_Type,2 as BIZ_TYPE 
from opt_pay a,pay_info_ccb b,pat_info d where IS_TZ=0 and  a.pay_id=b.bdj_id and a.pat_id=d.pat_id AND a.HOS_ID='{0}' and a.dj_date between '{1}' and '{2}' {3}
union
select  a.PAY_ID,a.PAT_NAME,a.CASH_JE as JE_ALL,a.RCPT_NO as PZ_NO,CONCAT(a.DJ_DATE,' ',a.DJ_TIME) as DJ_TIME, 
b.COMM_SN as QUERYID,1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'2' as DEAL_TYPE,b.txn_Type,2 as BIZ_TYPE 
from opt_pay a,pay_info_zfb b,pat_info d where IS_TZ=0 and  a.pay_id=b.biz_sn and a.pat_id=d.pat_id AND a.HOS_ID='{0}' and a.dj_date between '{1}' and '{2}' {3}
union
select  a.PAY_ID,a.PAT_NAME,a.CASH_JE/100 as JE_ALL,a.RCPT_NO as PZ_NO,CONCAT(a.DJ_DATE,' ',a.DJ_TIME) as DJ_TIME, 
b.orderid as QUERYID,1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'3' as DEAL_TYPE,b.txn_Type,2 as BIZ_TYPE   
from opt_pay a,pay_info_upcap b,pat_info d where IS_TZ=0 and  a.pay_id=b.bdj_id and a.pat_id=d.pat_id  AND a.HOS_ID='{0}' and a.dj_date between '{1}' and '{2}' {3}
", HOS_ID, BEGIN_DATE, END_DATE, where);

                //if (PZ_NO != "")
                //{
                //    sqlcmd += " and a.RCPT_NO='" + PZ_NO + "'";
                //}
            }
            //else if (TYPE == "3")
            {
                where = PZ_NO == "" ? "" : " and a.HOS_PAY_SN='" + PZ_NO + "'";
                sqlcmd3 = string.Format(@"
                    select '0' as PAY_ID,b.PAT_NAME,a.CASH_JE as JE_ALL,a.HOS_PAY_SN as PZ_NO,a.DJ_TIME,c.ORDERID as QUERYID, 1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'5' as DEAL_TYPE,c.txn_Type,3 as BIZ_TYPE   
from platform.pat_prepay a,pat_info b,pay_info_ccb c,pat_info d
where a.PAY_ID=c.BDj_id and a.pat_id=b.pat_id and a.pat_id=d.pat_id and  a.HOS_ID='{0}' and a.dj_time between '{1}' and '{2}' {3}
union
select '0' as PAY_ID,b.PAT_NAME,a.CASH_JE as JE_ALL,a.HOS_PAY_SN as PZ_NO,a.DJ_TIME,c.COMM_SN as QUERYID, 1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'2' as DEAL_TYPE,c.txn_Type,3 as BIZ_TYPE   
from platform.pat_prepay a,pat_info b,pay_info_zfb c,pat_info d
where a.PAY_ID=c.BIZ_SN and a.pat_id=b.pat_id and a.pat_id=d.pat_id and  a.HOS_ID='{0}' and a.dj_time between '{1}' and '{2}' {3}
union
select '0' as PAY_ID,b.PAT_NAME,a.CASH_JE/100 as JE_ALL,a.HOS_PAY_SN as PZ_NO,a.DJ_TIME,c.orderid as QUERYID, 1 as FY_STATE,d.SEX,d.BIRTHDAY,d.SFZ_NO,'3' as DEAL_TYPE,c.txn_Type ,3 as BIZ_TYPE  
from platform.pat_prepay a,pat_info b,pay_info_upcap c,pat_info d
where a.PAY_ID=c.bdj_id and a.pat_id=b.pat_id and a.pat_id=d.pat_id and a.HOS_ID='{0}' and a.dj_time between '{1}' and '{2}' {3}

", HOS_ID, BEGIN_DATE + " 00:00:00", END_DATE + " 23:59:59", where);

                //if (PZ_NO != "")
                //{
                //    sqlcmd += " and a.HOS_PAY_SN='" + PZ_NO + "'";
                //}
            }
            if (TYPE == "0")
            {
                sqlcmd = sqlcmd1 + " union " + sqlcmd2 + " union " + sqlcmd3;
            }
            else if (TYPE == "1")
            {
                sqlcmd = sqlcmd1;
            }
            else if (TYPE == "2")
            {
                sqlcmd = sqlcmd2;
            }
            else if (TYPE == "3")
            {
                sqlcmd = sqlcmd3;
            }
            else
            {
                return new DataTable();

            }
            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        #endregion  ExtensionMethod
    }
}


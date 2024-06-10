using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:opt_pay
    /// </summary>
    public partial class opt_pay 
    {
        public opt_pay()
        { }
        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string PAY_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from opt_pay");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30)};
            parameters[0].Value = PAY_ID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.opt_pay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@RCPT_NO,@HOS_PAY_SN,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_REG_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAY_lTERMINAL_SN", MySqlDbType.VarChar,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ybDJH", MySqlDbType.VarChar,15),
					new MySqlParameter("@GRZL", MySqlDbType.Decimal,10),
					new MySqlParameter("@GRZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@TCZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@DBZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@HM", MySqlDbType.Decimal,10),
					new MySqlParameter("@CS", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@YF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XMFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@LCL", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,10),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@man_type", MySqlDbType.VarChar,10),
					new MySqlParameter("@BZFYY", MySqlDbType.VarChar,10),
					new MySqlParameter("@FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@RCPT_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_PAY_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.HOS_SN;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_REG_SN;
            parameters[7].Value = model.PRE_NO;
            parameters[8].Value = model.SFZ_NO;
            parameters[9].Value = model.YLCARD_TYPE;
            parameters[10].Value = model.YLCARD_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.DEPT_NAME;
            parameters[14].Value = model.DOC_NO;
            parameters[15].Value = model.DOC_NAME;
            parameters[16].Value = model.DIS_NAME;
            parameters[17].Value = model.PAY_lTERMINAL_SN;
            parameters[18].Value = model.CASH_JE;
            parameters[19].Value = model.PAY_TYPE;
            parameters[20].Value = model.JEALL;
            parameters[21].Value = model.JZ_CODE;
            parameters[22].Value = model.ybDJH;
            parameters[23].Value = model.GRZL;
            parameters[24].Value = model.GRZF;
            parameters[25].Value = model.TCZF;
            parameters[26].Value = model.DBZF;
            parameters[27].Value = model.XJZF;
            parameters[28].Value = model.ZHZF;
            parameters[29].Value = model.HM;
            parameters[30].Value = model.CS;
            parameters[31].Value = model.ZFY;
            parameters[32].Value = model.YF;
            parameters[33].Value = model.XMFY;
            parameters[34].Value = model.LCL;
            parameters[35].Value = model.ZHYE;
            parameters[36].Value = model.XZM;
            parameters[37].Value = model.XZMCH;
            parameters[38].Value = model.man_type;
            parameters[39].Value = model.BZFYY;
            parameters[40].Value = model.FYLB;
            parameters[41].Value = model.YBBZM;
            parameters[42].Value = model.YBBZMC;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.RCPT_NO;
            parameters[46].Value = model.HOS_PAY_SN;
            parameters[47].Value = model.IS_TZ;
            parameters[48].Value = model.TZ_DATE;
            parameters[49].Value = model.TZ_TIME;
            parameters[50].Value = model.PAY_ID_IN;
            parameters[51].Value = model.lTERMINAL_SN;


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
        public bool Update(Plat.Model.opt_pay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update opt_pay set ");
            strSql.Append("HOS_ID=@HOS_ID,");
            strSql.Append("PAT_ID=@PAT_ID,");
            strSql.Append("REGPAT_ID=@REGPAT_ID,");
            strSql.Append("HOS_SN=@HOS_SN,");
            strSql.Append("OPT_SN=@OPT_SN,");
            strSql.Append("HOS_REG_SN=@HOS_REG_SN,");
            strSql.Append("PRE_NO=@PRE_NO,");
            strSql.Append("SFZ_NO=@SFZ_NO,");
            strSql.Append("YLCARD_TYPE=@YLCARD_TYPE,");
            strSql.Append("YLCARD_NO=@YLCARD_NO,");
            strSql.Append("PAT_NAME=@PAT_NAME,");
            strSql.Append("DEPT_CODE=@DEPT_CODE,");
            strSql.Append("DEPT_NAME=@DEPT_NAME,");
            strSql.Append("DOC_NO=@DOC_NO,");
            strSql.Append("DOC_NAME=@DOC_NAME,");
            strSql.Append("DIS_NAME=@DIS_NAME,");
            strSql.Append("PAY_lTERMINAL_SN=@PAY_lTERMINAL_SN,");
            strSql.Append("CASH_JE=@CASH_JE,");
            strSql.Append("PAY_TYPE=@PAY_TYPE,");
            strSql.Append("JEALL=@JEALL,");
            strSql.Append("JZ_CODE=@JZ_CODE,");
            strSql.Append("ybDJH=@ybDJH,");
            strSql.Append("GRZL=@GRZL,");
            strSql.Append("GRZF=@GRZF,");
            strSql.Append("TCZF=@TCZF,");
            strSql.Append("DBZF=@DBZF,");
            strSql.Append("XJZF=@XJZF,");
            strSql.Append("ZHZF=@ZHZF,");
            strSql.Append("HM=@HM,");
            strSql.Append("CS=@CS,");
            strSql.Append("ZFY=@ZFY,");
            strSql.Append("YF=@YF,");
            strSql.Append("XMFY=@XMFY,");
            strSql.Append("LCL=@LCL,");
            strSql.Append("ZHYE=@ZHYE,");
            strSql.Append("XZM=@XZM,");
            strSql.Append("XZMCH=@XZMCH,");
            strSql.Append("man_type=@man_type,");
            strSql.Append("BZFYY=@BZFYY,");
            strSql.Append("FYLB=@FYLB,");
            strSql.Append("YBBZM=@YBBZM,");
            strSql.Append("YBBZMC=@YBBZMC,");
            strSql.Append("DJ_DATE=@DJ_DATE,");
            strSql.Append("DJ_TIME=@DJ_TIME,");
            strSql.Append("RCPT_NO=@RCPT_NO,");
            strSql.Append("HOS_PAY_SN=@HOS_PAY_SN,");
            strSql.Append("IS_TZ=@IS_TZ,");
            strSql.Append("TZ_DATE=@TZ_DATE,");
            strSql.Append("TZ_TIME=@TZ_TIME,");
            strSql.Append("PAY_ID_IN=@PAY_ID_IN,");
            strSql.Append("lTERMINAL_SN=@lTERMINAL_SN");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_REG_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAY_lTERMINAL_SN", MySqlDbType.VarChar,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ybDJH", MySqlDbType.VarChar,15),
					new MySqlParameter("@GRZL", MySqlDbType.Decimal,10),
					new MySqlParameter("@GRZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@TCZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@DBZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@HM", MySqlDbType.Decimal,10),
					new MySqlParameter("@CS", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@YF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XMFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@LCL", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,10),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@man_type", MySqlDbType.VarChar,10),
					new MySqlParameter("@BZFYY", MySqlDbType.VarChar,10),
					new MySqlParameter("@FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@RCPT_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_PAY_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.PAT_ID;
            parameters[2].Value = model.REGPAT_ID;
            parameters[3].Value = model.HOS_SN;
            parameters[4].Value = model.OPT_SN;
            parameters[5].Value = model.HOS_REG_SN;
            parameters[6].Value = model.PRE_NO;
            parameters[7].Value = model.SFZ_NO;
            parameters[8].Value = model.YLCARD_TYPE;
            parameters[9].Value = model.YLCARD_NO;
            parameters[10].Value = model.PAT_NAME;
            parameters[11].Value = model.DEPT_CODE;
            parameters[12].Value = model.DEPT_NAME;
            parameters[13].Value = model.DOC_NO;
            parameters[14].Value = model.DOC_NAME;
            parameters[15].Value = model.DIS_NAME;
            parameters[16].Value = model.PAY_lTERMINAL_SN;
            parameters[17].Value = model.CASH_JE;
            parameters[18].Value = model.PAY_TYPE;
            parameters[19].Value = model.JEALL;
            parameters[20].Value = model.JZ_CODE;
            parameters[21].Value = model.ybDJH;
            parameters[22].Value = model.GRZL;
            parameters[23].Value = model.GRZF;
            parameters[24].Value = model.TCZF;
            parameters[25].Value = model.DBZF;
            parameters[26].Value = model.XJZF;
            parameters[27].Value = model.ZHZF;
            parameters[28].Value = model.HM;
            parameters[29].Value = model.CS;
            parameters[30].Value = model.ZFY;
            parameters[31].Value = model.YF;
            parameters[32].Value = model.XMFY;
            parameters[33].Value = model.LCL;
            parameters[34].Value = model.ZHYE;
            parameters[35].Value = model.XZM;
            parameters[36].Value = model.XZMCH;
            parameters[37].Value = model.man_type;
            parameters[38].Value = model.BZFYY;
            parameters[39].Value = model.FYLB;
            parameters[40].Value = model.YBBZM;
            parameters[41].Value = model.YBBZMC;
            parameters[42].Value = model.DJ_DATE;
            parameters[43].Value = model.DJ_TIME;
            parameters[44].Value = model.RCPT_NO;
            parameters[45].Value = model.HOS_PAY_SN;
            parameters[46].Value = model.IS_TZ;
            parameters[47].Value = model.TZ_DATE;
            parameters[48].Value = model.TZ_TIME;
            parameters[49].Value = model.PAY_ID_IN;
            parameters[50].Value = model.lTERMINAL_SN;
            parameters[51].Value = model.PAY_ID;

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
        public bool Delete(string PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from opt_pay ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30)};
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
            strSql.Append("delete from opt_pay ");
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
        public Plat.Model.opt_pay GetModel(string PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN from opt_pay ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30)};
            parameters[0].Value = PAY_ID;

            Plat.Model.opt_pay model = new Plat.Model.opt_pay();
            DataSet ds = DbHelperMySQLZZJ.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["PAY_ID"] != null && ds.Tables[0].Rows[0]["PAY_ID"].ToString() != "")
                {
                    model.PAY_ID = ds.Tables[0].Rows[0]["PAY_ID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["HOS_ID"] != null && ds.Tables[0].Rows[0]["HOS_ID"].ToString() != "")
                {
                    model.HOS_ID = ds.Tables[0].Rows[0]["HOS_ID"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PAT_ID"] != null && ds.Tables[0].Rows[0]["PAT_ID"].ToString() != "")
                {
                    model.PAT_ID = int.Parse(ds.Tables[0].Rows[0]["PAT_ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["REGPAT_ID"] != null && ds.Tables[0].Rows[0]["REGPAT_ID"].ToString() != "")
                {
                    model.REGPAT_ID = int.Parse(ds.Tables[0].Rows[0]["REGPAT_ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HOS_SN"] != null && ds.Tables[0].Rows[0]["HOS_SN"].ToString() != "")
                {
                    model.HOS_SN = ds.Tables[0].Rows[0]["HOS_SN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["OPT_SN"] != null && ds.Tables[0].Rows[0]["OPT_SN"].ToString() != "")
                {
                    model.OPT_SN = ds.Tables[0].Rows[0]["OPT_SN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["HOS_REG_SN"] != null && ds.Tables[0].Rows[0]["HOS_REG_SN"].ToString() != "")
                {
                    model.HOS_REG_SN = ds.Tables[0].Rows[0]["HOS_REG_SN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PRE_NO"] != null && ds.Tables[0].Rows[0]["PRE_NO"].ToString() != "")
                {
                    model.PRE_NO = ds.Tables[0].Rows[0]["PRE_NO"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SFZ_NO"] != null && ds.Tables[0].Rows[0]["SFZ_NO"].ToString() != "")
                {
                    model.SFZ_NO = ds.Tables[0].Rows[0]["SFZ_NO"].ToString();
                }
                if (ds.Tables[0].Rows[0]["YLCARD_TYPE"] != null && ds.Tables[0].Rows[0]["YLCARD_TYPE"].ToString() != "")
                {
                    model.YLCARD_TYPE = int.Parse(ds.Tables[0].Rows[0]["YLCARD_TYPE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["YLCARD_NO"] != null && ds.Tables[0].Rows[0]["YLCARD_NO"].ToString() != "")
                {
                    model.YLCARD_NO = ds.Tables[0].Rows[0]["YLCARD_NO"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PAT_NAME"] != null && ds.Tables[0].Rows[0]["PAT_NAME"].ToString() != "")
                {
                    model.PAT_NAME = ds.Tables[0].Rows[0]["PAT_NAME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DEPT_CODE"] != null && ds.Tables[0].Rows[0]["DEPT_CODE"].ToString() != "")
                {
                    model.DEPT_CODE = ds.Tables[0].Rows[0]["DEPT_CODE"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DEPT_NAME"] != null && ds.Tables[0].Rows[0]["DEPT_NAME"].ToString() != "")
                {
                    model.DEPT_NAME = ds.Tables[0].Rows[0]["DEPT_NAME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DOC_NO"] != null && ds.Tables[0].Rows[0]["DOC_NO"].ToString() != "")
                {
                    model.DOC_NO = ds.Tables[0].Rows[0]["DOC_NO"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DOC_NAME"] != null && ds.Tables[0].Rows[0]["DOC_NAME"].ToString() != "")
                {
                    model.DOC_NAME = ds.Tables[0].Rows[0]["DOC_NAME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DIS_NAME"] != null && ds.Tables[0].Rows[0]["DIS_NAME"].ToString() != "")
                {
                    model.DIS_NAME = ds.Tables[0].Rows[0]["DIS_NAME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PAY_lTERMINAL_SN"] != null && ds.Tables[0].Rows[0]["PAY_lTERMINAL_SN"].ToString() != "")
                {
                    model.PAY_lTERMINAL_SN = ds.Tables[0].Rows[0]["PAY_lTERMINAL_SN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["CASH_JE"] != null && ds.Tables[0].Rows[0]["CASH_JE"].ToString() != "")
                {
                    model.CASH_JE = decimal.Parse(ds.Tables[0].Rows[0]["CASH_JE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PAY_TYPE"] != null && ds.Tables[0].Rows[0]["PAY_TYPE"].ToString() != "")
                {
                    model.PAY_TYPE = int.Parse(ds.Tables[0].Rows[0]["PAY_TYPE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["JEALL"] != null && ds.Tables[0].Rows[0]["JEALL"].ToString() != "")
                {
                    model.JEALL = decimal.Parse(ds.Tables[0].Rows[0]["JEALL"].ToString());
                }
                if (ds.Tables[0].Rows[0]["JZ_CODE"] != null && ds.Tables[0].Rows[0]["JZ_CODE"].ToString() != "")
                {
                    model.JZ_CODE = ds.Tables[0].Rows[0]["JZ_CODE"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ybDJH"] != null && ds.Tables[0].Rows[0]["ybDJH"].ToString() != "")
                {
                    model.ybDJH = ds.Tables[0].Rows[0]["ybDJH"].ToString();
                }
                if (ds.Tables[0].Rows[0]["GRZL"] != null && ds.Tables[0].Rows[0]["GRZL"].ToString() != "")
                {
                    model.GRZL = decimal.Parse(ds.Tables[0].Rows[0]["GRZL"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GRZF"] != null && ds.Tables[0].Rows[0]["GRZF"].ToString() != "")
                {
                    model.GRZF = decimal.Parse(ds.Tables[0].Rows[0]["GRZF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TCZF"] != null && ds.Tables[0].Rows[0]["TCZF"].ToString() != "")
                {
                    model.TCZF = decimal.Parse(ds.Tables[0].Rows[0]["TCZF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DBZF"] != null && ds.Tables[0].Rows[0]["DBZF"].ToString() != "")
                {
                    model.DBZF = decimal.Parse(ds.Tables[0].Rows[0]["DBZF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["XJZF"] != null && ds.Tables[0].Rows[0]["XJZF"].ToString() != "")
                {
                    model.XJZF = decimal.Parse(ds.Tables[0].Rows[0]["XJZF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ZHZF"] != null && ds.Tables[0].Rows[0]["ZHZF"].ToString() != "")
                {
                    model.ZHZF = decimal.Parse(ds.Tables[0].Rows[0]["ZHZF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["HM"] != null && ds.Tables[0].Rows[0]["HM"].ToString() != "")
                {
                    model.HM = decimal.Parse(ds.Tables[0].Rows[0]["HM"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CS"] != null && ds.Tables[0].Rows[0]["CS"].ToString() != "")
                {
                    model.CS = decimal.Parse(ds.Tables[0].Rows[0]["CS"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ZFY"] != null && ds.Tables[0].Rows[0]["ZFY"].ToString() != "")
                {
                    model.ZFY = decimal.Parse(ds.Tables[0].Rows[0]["ZFY"].ToString());
                }
                if (ds.Tables[0].Rows[0]["YF"] != null && ds.Tables[0].Rows[0]["YF"].ToString() != "")
                {
                    model.YF = decimal.Parse(ds.Tables[0].Rows[0]["YF"].ToString());
                }
                if (ds.Tables[0].Rows[0]["XMFY"] != null && ds.Tables[0].Rows[0]["XMFY"].ToString() != "")
                {
                    model.XMFY = decimal.Parse(ds.Tables[0].Rows[0]["XMFY"].ToString());
                }
                if (ds.Tables[0].Rows[0]["LCL"] != null && ds.Tables[0].Rows[0]["LCL"].ToString() != "")
                {
                    model.LCL = decimal.Parse(ds.Tables[0].Rows[0]["LCL"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ZHYE"] != null && ds.Tables[0].Rows[0]["ZHYE"].ToString() != "")
                {
                    model.ZHYE = decimal.Parse(ds.Tables[0].Rows[0]["ZHYE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["XZM"] != null && ds.Tables[0].Rows[0]["XZM"].ToString() != "")
                {
                    model.XZM = ds.Tables[0].Rows[0]["XZM"].ToString();
                }
                if (ds.Tables[0].Rows[0]["XZMCH"] != null && ds.Tables[0].Rows[0]["XZMCH"].ToString() != "")
                {
                    model.XZMCH = ds.Tables[0].Rows[0]["XZMCH"].ToString();
                }
                if (ds.Tables[0].Rows[0]["man_type"] != null && ds.Tables[0].Rows[0]["man_type"].ToString() != "")
                {
                    model.man_type = ds.Tables[0].Rows[0]["man_type"].ToString();
                }
                if (ds.Tables[0].Rows[0]["BZFYY"] != null && ds.Tables[0].Rows[0]["BZFYY"].ToString() != "")
                {
                    model.BZFYY = ds.Tables[0].Rows[0]["BZFYY"].ToString();
                }
                if (ds.Tables[0].Rows[0]["FYLB"] != null && ds.Tables[0].Rows[0]["FYLB"].ToString() != "")
                {
                    model.FYLB = ds.Tables[0].Rows[0]["FYLB"].ToString();
                }
                if (ds.Tables[0].Rows[0]["YBBZM"] != null && ds.Tables[0].Rows[0]["YBBZM"].ToString() != "")
                {
                    model.YBBZM = ds.Tables[0].Rows[0]["YBBZM"].ToString();
                }
                if (ds.Tables[0].Rows[0]["YBBZMC"] != null && ds.Tables[0].Rows[0]["YBBZMC"].ToString() != "")
                {
                    model.YBBZMC = ds.Tables[0].Rows[0]["YBBZMC"].ToString();
                }
                if (ds.Tables[0].Rows[0]["DJ_DATE"] != null && ds.Tables[0].Rows[0]["DJ_DATE"].ToString() != "")
                {
                    model.DJ_DATE = DateTime.Parse(ds.Tables[0].Rows[0]["DJ_DATE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["DJ_TIME"] != null && ds.Tables[0].Rows[0]["DJ_TIME"].ToString() != "")
                {
                    model.DJ_TIME = ds.Tables[0].Rows[0]["DJ_TIME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["RCPT_NO"] != null && ds.Tables[0].Rows[0]["RCPT_NO"].ToString() != "")
                {
                    model.RCPT_NO = ds.Tables[0].Rows[0]["RCPT_NO"].ToString();
                }
                if (ds.Tables[0].Rows[0]["HOS_PAY_SN"] != null && ds.Tables[0].Rows[0]["HOS_PAY_SN"].ToString() != "")
                {
                    model.HOS_PAY_SN = ds.Tables[0].Rows[0]["HOS_PAY_SN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["IS_TZ"] != null && ds.Tables[0].Rows[0]["IS_TZ"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IS_TZ"].ToString() == "1") || (ds.Tables[0].Rows[0]["IS_TZ"].ToString().ToLower() == "true"))
                    {
                        model.IS_TZ = true;
                    }
                    else
                    {
                        model.IS_TZ = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["TZ_DATE"] != null && ds.Tables[0].Rows[0]["TZ_DATE"].ToString() != "")
                {
                    model.TZ_DATE = DateTime.Parse(ds.Tables[0].Rows[0]["TZ_DATE"].ToString());
                }
                if (ds.Tables[0].Rows[0]["TZ_TIME"] != null && ds.Tables[0].Rows[0]["TZ_TIME"].ToString() != "")
                {
                    model.TZ_TIME = ds.Tables[0].Rows[0]["TZ_TIME"].ToString();
                }
                if (ds.Tables[0].Rows[0]["PAY_ID_IN"] != null && ds.Tables[0].Rows[0]["PAY_ID_IN"].ToString() != "")
                {
                    model.PAY_ID_IN = ds.Tables[0].Rows[0]["PAY_ID_IN"].ToString();
                }
                if (ds.Tables[0].Rows[0]["lTERMINAL_SN"] != null && ds.Tables[0].Rows[0]["lTERMINAL_SN"].ToString() != "")
                {
                    model.lTERMINAL_SN = ds.Tables[0].Rows[0]["lTERMINAL_SN"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN ");
            strSql.Append(" FROM opt_pay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }
        public DataTable GetPreInfo(string HOS_ID, int PAT_ID, int REGPAT_ID, string BEGIN_DATE, string END_DATE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            string sqlcmd = string.Format(@"
    SELECT a.pay_id, a.pat_id,a.hos_id,a.opt_sn,a.pre_no,a.hos_sn,b.pat_name,a.dj_date,a.dj_time from opt_pay a,pat_info b
            where a.pat_id=b.pat_id
            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
            and a.pat_id='{2}' and a.dj_date between '{3}' and '{4}' and is_tz=0 order by a.dj_date desc,a.dj_time desc ", HOS_ID, REGPAT_ID, PAT_ID, BEGIN_DATE, END_DATE);

            if (BEGIN_DATE.Trim() == "" && END_DATE.Trim() == "")
            {
                sqlcmd = string.Format(@"
    SELECT a.pay_id, a.pat_id,a.hos_id,a.opt_sn,a.pre_no,a.hos_sn,b.pat_name,a.dj_date,a.dj_time from opt_pay a,pat_info b
            where a.pat_id=b.pat_id
            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
            and a.pat_id='{2}' and is_tz=0 order by a.dj_date desc,a.dj_time desc ", HOS_ID, REGPAT_ID, PAT_ID);

            }
            PAGECOUNT = BaseFunction.PAGECount(sqlcmd);
            PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
				
            //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
            sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
            return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
        }
        /// <summary>
        /// 历史诊间处方查询
        /// </summary>
        /// <param name="HOS_ID"></param>
        /// <param name="PAT_ID"></param>
        /// <param name="REGPAT_ID"></param>
        /// <param name="BEGIN_DATE"></param>
        /// <param name="END_DATE"></param>
        /// <param name="PAGEINDEX"></param>
        /// <param name="PAGESIZE"></param>
        /// <param name="PAGECOUNT"></param>
        /// <returns></returns>
        public DataTable GetFYInfo(string HOS_ID, int PAT_ID, int REGPAT_ID, string BEGIN_DATE, string END_DATE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
//            string sqlcmd = string.Format(@"
//    SELECT a.pay_id,a.pat_id,a.hos_id,a.opt_sn,
//		a.pre_no,a.hos_sn,b.pat_name,a.cash_je,a.pay_type,
//a.jeall,a.jz_code,a.grzl,a.grzf,a.tczf,a.dbzf,
//a.xjzf,a.zhzf,a.hm,a.cs,a.zfy,a.yf,a.xmfy,a.lcl,
//a.zhye,a.dj_date,a.dj_time,a.is_tz
//     from opt_pay a,pat_info b	
//     where a.pat_id=b.pat_id
//            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
//            and a.pat_id='{2}' and a.dj_date between '{3}' and '{4}' and PAY_ID_IN is null order by a.dj_date desc,a.dj_time desc", HOS_ID, REGPAT_ID, PAT_ID, BEGIN_DATE, END_DATE);
//            if (BEGIN_DATE.Trim() == "" && END_DATE.Trim() == "")
//            {
//                sqlcmd = string.Format(@"
//    SELECT a.pay_id,a.pat_id,a.hos_id,a.opt_sn,
//		a.pre_no,a.hos_sn,b.pat_name,a.cash_je,a.pay_type,
//a.jeall,a.jz_code,a.grzl,a.grzf,a.tczf,a.dbzf,
//a.xjzf,a.zhzf,a.hm,a.cs,a.zfy,a.yf,a.xmfy,a.lcl,
//a.zhye,a.dj_date,a.dj_time,a.is_tz
//     from opt_pay a,pat_info b	
//     where a.pat_id=b.pat_id
//            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
//            and a.pat_id='{2}' and PAY_ID_IN is null order by a.dj_date desc,a.dj_time desc", HOS_ID, REGPAT_ID, PAT_ID);
//            }

            string sqlcmd = string.Format(@"
    SELECT a.pay_id,a.pat_id,a.hos_id,a.opt_sn,
		a.hos_sn as pre_no,a.hos_sn,b.pat_name,a.cash_je,a.pay_type,
a.jeall,a.jz_code,a.grzl,a.grzf,a.tczf,a.dbzf,
a.xjzf,a.zhzf,a.hm,a.cs,a.zfy,a.yf,a.xmfy,a.lcl,
a.zhye,a.dj_date,a.dj_time,a.is_tz,a.ylcard_no
     from opt_pay a,pat_info b	
     where a.pat_id=b.pat_id
            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
            and a.pat_id='{2}' and a.dj_date between '{3}' and '{4}' and PAY_ID_IN is null order by a.dj_date desc,a.dj_time desc", HOS_ID, REGPAT_ID, PAT_ID, BEGIN_DATE, END_DATE);
            if (BEGIN_DATE.Trim() == "" && END_DATE.Trim() == "")
            {
                sqlcmd = string.Format(@"
    SELECT a.pay_id,a.pat_id,a.hos_id,a.opt_sn,
		a.hos_sn as pre_no,a.hos_sn,b.pat_name,a.cash_je,a.pay_type,
a.jeall,a.jz_code,a.grzl,a.grzf,a.tczf,a.dbzf,
a.xjzf,a.zhzf,a.hm,a.cs,a.zfy,a.yf,a.xmfy,a.lcl,
a.zhye,a.dj_date,a.dj_time,a.is_tz,a.ylcard_no
     from opt_pay a,pat_info b	
     where a.pat_id=b.pat_id
            and a.hos_id='{0}' and a.REGPAT_ID='{1}'
            and a.pat_id='{2}' and PAY_ID_IN is null order by a.dj_date desc,a.dj_time desc", HOS_ID, REGPAT_ID, PAT_ID);
            }
                PAGECOUNT = BaseFunction.PAGECount(sqlcmd);
                PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
				
            //sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
                sqlcmd += " limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + PAGESIZE;
            return DbHelperMySQLZZJ.Query(sqlcmd.ToString()).Tables[0];
        }
        public DataTable GetCFZFMX(string FILT_TYPE, string PAY_ID,int REGPAT_ID,  int PAT_ID, string HOS_ID, string OPT_SN, string PRE_NO, string HOS_SN)
        {
            return null;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddByTran(Plat.Model.opt_pay model, Plat.Model.opt_pay_fl[] fl, Plat.Model.opt_pay_mx[] mx,Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Plat.Model.pay_info_wc wc, Plat.Model.pay_info_bank bank,Plat.Model.opt_pay_log log,Plat.Model.pay_info_upcap upcap,Plat.Model.pay_info_ccb ccb)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN,SOURCE)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@RCPT_NO,@HOS_PAY_SN,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN,@SOURCE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_REG_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAY_lTERMINAL_SN", MySqlDbType.VarChar,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ybDJH", MySqlDbType.VarChar,15),
					new MySqlParameter("@GRZL", MySqlDbType.Decimal,10),
					new MySqlParameter("@GRZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@TCZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@DBZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@HM", MySqlDbType.Decimal,10),
					new MySqlParameter("@CS", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@YF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XMFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@LCL", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,10),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@man_type", MySqlDbType.VarChar,10),
					new MySqlParameter("@BZFYY", MySqlDbType.VarChar,10),
					new MySqlParameter("@FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@RCPT_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_PAY_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10)
                                          };
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.HOS_SN;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_REG_SN;
            parameters[7].Value = model.PRE_NO;
            parameters[8].Value = model.SFZ_NO;
            parameters[9].Value = model.YLCARD_TYPE;
            parameters[10].Value = model.YLCARD_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.DEPT_NAME;
            parameters[14].Value = model.DOC_NO;
            parameters[15].Value = model.DOC_NAME;
            parameters[16].Value = model.DIS_NAME;
            parameters[17].Value = model.PAY_lTERMINAL_SN;
            parameters[18].Value = model.CASH_JE;
            parameters[19].Value = model.PAY_TYPE;
            parameters[20].Value = model.JEALL;
            parameters[21].Value = model.JZ_CODE;
            parameters[22].Value = model.ybDJH;
            parameters[23].Value = model.GRZL;
            parameters[24].Value = model.GRZF;
            parameters[25].Value = model.TCZF;
            parameters[26].Value = model.DBZF;
            parameters[27].Value = model.XJZF;
            parameters[28].Value = model.ZHZF;
            parameters[29].Value = model.HM;
            parameters[30].Value = model.CS;
            parameters[31].Value = model.ZFY;
            parameters[32].Value = model.YF;
            parameters[33].Value = model.XMFY;
            parameters[34].Value = model.LCL;
            parameters[35].Value = model.ZHYE;
            parameters[36].Value = model.XZM;
            parameters[37].Value = model.XZMCH;
            parameters[38].Value = model.man_type;
            parameters[39].Value = model.BZFYY;
            parameters[40].Value = model.FYLB;
            parameters[41].Value = model.YBBZM;
            parameters[42].Value = model.YBBZMC;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.RCPT_NO;
            parameters[46].Value = model.HOS_PAY_SN;
            parameters[47].Value = model.IS_TZ;
            parameters[48].Value = model.TZ_DATE;
            parameters[49].Value = model.TZ_TIME;
            parameters[50].Value = model.PAY_ID_IN;
            parameters[51].Value = model.lTERMINAL_SN;
            parameters[52].Value = model.SOURCE;

            table.Add(strSql.ToString(), parameters);

            for (int i = 0; i < fl.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_fl(");
                strSql.Append("PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@FL_NAME,@DEPT_CODE,@DEPT_NAME,@FL_JE,@FL_ORDER)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10)};
                parameters1[0].Value = fl[i].PAY_ID;
                parameters1[1].Value = fl[i].FL_NO;
                parameters1[2].Value = fl[i].FL_NAME;
                parameters1[3].Value = fl[i].DEPT_CODE;
                parameters1[4].Value = fl[i].DEPT_NAME;
                parameters1[5].Value = fl[i].FL_JE;
                parameters1[6].Value = fl[i].FL_ORDER;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }

            for (int i = 0; i < mx.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_mx(");
                strSql.Append("PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@ITEM_TYPE,@ITEM_ID,@ITEM_NAME,@ITEM_GG,@COUNT,@ITEM_UNIT,@COST,@je)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10)};
                parameters1[0].Value = mx[i].PAY_ID;
                parameters1[1].Value = mx[i].FL_NO;
                parameters1[2].Value = mx[i].ITEM_TYPE;
                parameters1[3].Value = mx[i].ITEM_ID;
                parameters1[4].Value = mx[i].ITEM_NAME;
                parameters1[5].Value = mx[i].ITEM_GG;
                parameters1[6].Value = mx[i].COUNT;
                parameters1[7].Value = mx[i].ITEM_UNIT;
                parameters1[8].Value = mx[i].COST;
                parameters1[9].Value = mx[i].je;


                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_mx_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters2[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters2);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_fl_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters3[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters3);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters4[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters4);



            //现金交易记录表
            strSql = new StringBuilder();
            strSql.Append("insert into pay_info(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,BIZ_TYPE,BIZ_SN,CASH_JE,SFZ_NO,DJ_DATE,DJ_TIME,DEAL_TYPE,DEAL_STATES,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@BIZ_TYPE,@BIZ_SN,@CASH_JE,@SFZ_NO,@DJ_DATE,@DJ_TIME,@DEAL_TYPE,@DEAL_STATES,@lTERMINAL_SN)");
            MySqlParameter[] parameters5 = {
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
            parameters5[0].Value = info.PAY_ID;
            parameters5[1].Value = info.HOS_ID;
            parameters5[2].Value = info.PAT_ID;
            parameters5[3].Value = info.REGPAT_ID;
            parameters5[4].Value = info.BIZ_TYPE;
            parameters5[5].Value = info.BIZ_SN;
            parameters5[6].Value = info.CASH_JE;
            parameters5[7].Value = info.SFZ_NO;
            parameters5[8].Value = info.DJ_DATE;
            parameters5[9].Value = info.DJ_TIME;
            parameters5[10].Value = info.DEAL_TYPE;
            parameters5[11].Value = info.DEAL_STATES;
            parameters5[12].Value = info.lTERMINAL_SN;

            table.Add(strSql, parameters5);

            if (zfb != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_zfb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BIZ_SN,SELLER_ID,COMM_SN,JE,DEAL_STATES,TXN_TYPE,DEAL_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BIZ_SN,@SELLER_ID,@COMM_SN,@JE,@DEAL_STATES,@TXN_TYPE,@DEAL_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters6 = {
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
                parameters6[0].Value = zfb.PAY_ID;
                parameters6[1].Value = zfb.BIZ_TYPE;
                parameters6[2].Value = zfb.BIZ_SN;
                parameters6[3].Value = zfb.SELLER_ID;
                parameters6[4].Value = zfb.COMM_SN;
                parameters6[5].Value = zfb.JE;
                parameters6[6].Value = zfb.DEAL_STATES;
                parameters6[7].Value = zfb.TXN_TYPE;
                parameters6[8].Value = zfb.DEAL_TIME;
                parameters6[9].Value = zfb.lTERMINAL_SN;

                table.Add(strSql, parameters6);
            }
            else if (wc != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_wc(");
                strSql.Append("PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TXN_TYPE)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@WECHAT,@PAY_TYPE,@BIZ_TYPE,@BIZ_SN,@COMM_SN,@JE,@COMM_NAME,@DEAL_STATES,@DEAL_TIME,@DEAL_SN,@lTERMINAL_SN,@TXN_TYPE)");
                MySqlParameter[] parameters7 = {
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
                parameters7[0].Value = wc.PAY_ID;
                parameters7[1].Value = wc.WECHAT;
                parameters7[2].Value = wc.PAY_TYPE;
                parameters7[3].Value = wc.BIZ_TYPE;
                parameters7[4].Value = wc.BIZ_SN;
                parameters7[5].Value = wc.COMM_SN;
                parameters7[6].Value = wc.JE;
                parameters7[7].Value = wc.COMM_NAME;
                parameters7[8].Value = wc.DEAL_STATES;
                parameters7[9].Value = wc.DEAL_TIME;
                parameters7[10].Value = wc.DEAL_SN;
                parameters7[11].Value = wc.lTERMINAL_SN;
                parameters7[12].Value = wc.TNX_TYPE;

                table.Add(strSql, parameters7);
            }
            else if (bank != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_bank(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,BANK_TYPE,RETURN_CODE,BANK_CARD,CARD_TYPE,SEARCH_CODE,REFCODE,TERMCODE,CARD_COMPAY,COMM_NAME,COMM_SN,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@BANK_TYPE,@RETURN_CODE,@BANK_CARD,@CARD_TYPE,@SEARCH_CODE,@REFCODE,@TERMCODE,@CARD_COMPAY,@COMM_NAME,@COMM_SN,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters8 = {
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
                parameters8[0].Value = bank.PAY_ID;
                parameters8[1].Value = bank.BIZ_TYPE;
                parameters8[2].Value = bank.BDj_id;
                parameters8[3].Value = bank.JE;
                parameters8[4].Value = bank.BANK_TYPE;
                parameters8[5].Value = bank.RETURN_CODE;
                parameters8[6].Value = bank.BANK_CARD;
                parameters8[7].Value = bank.CARD_TYPE;
                parameters8[8].Value = bank.SEARCH_CODE;
                parameters8[9].Value = bank.REFCODE;
                parameters8[10].Value = bank.TERMCODE;
                parameters8[11].Value = bank.CARD_COMPAY;
                parameters8[12].Value = bank.COMM_NAME;
                parameters8[13].Value = bank.COMM_SN;
                parameters8[14].Value = bank.SFZ_NO;
                parameters8[15].Value = bank.DJ_TIME;
                table.Add(strSql, parameters8);
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
                //strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE,MerchantID)");
                //strSql.Append(" values (");
                //strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE,@MerchantID)");
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

            strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters9 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HSP_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters9[0].Value = log.PAY_ID;
            parameters9[1].Value = log.STATES;
            parameters9[2].Value = log.HOS_ID;
            parameters9[3].Value = log.PAT_ID;
            parameters9[4].Value = log.HSP_SN;
            parameters9[5].Value = log.JEALL;
            parameters9[6].Value = model.CASH_JE;
            parameters9[7].Value = log.DJ_DATE;
            parameters9[8].Value = log.DJ_TIME;
            parameters9[9].Value = log.lTERMINAL_SN;

            table.Add(strSql, parameters9);


            strSql = new StringBuilder();
            strSql.Append("update register_appt set APPT_TYPE=4 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            MySqlParameter[] parameters10 = {
            new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
            new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30)};

            parameters10[0].Value = model.HOS_ID;
            parameters10[1].Value = model.HOS_REG_SN;
            table.Add(strSql.ToString(), parameters10);


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
                    modSqlError.TYPE = "诊间支付保存";
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
        /// <summary>
        /// 诊间退费保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="info"></param>
        /// <param name="zfb"></param>
        /// <param name="wc"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        public bool ZJTFByTran(Plat.Model.opt_pay model, Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Plat.Model.pay_info_wc wc, Plat.Model.pay_info_bank bank,Plat.Model.opt_pay_log log,Plat.Model.pay_info_upcap upcap,Plat.Model.unionpay_tran tran,Plat.Model.alipay_tran alitran,Plat.Model.pay_info_ccb ccb)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN,SOURCE)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@RCPT_NO,@HOS_PAY_SN,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN,@SOURCE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_REG_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAY_lTERMINAL_SN", MySqlDbType.VarChar,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ybDJH", MySqlDbType.VarChar,15),
					new MySqlParameter("@GRZL", MySqlDbType.Decimal,10),
					new MySqlParameter("@GRZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@TCZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@DBZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@HM", MySqlDbType.Decimal,10),
					new MySqlParameter("@CS", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@YF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XMFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@LCL", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,10),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@man_type", MySqlDbType.VarChar,10),
					new MySqlParameter("@BZFYY", MySqlDbType.VarChar,10),
					new MySqlParameter("@FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@RCPT_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_PAY_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10)                     
                    };
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.HOS_SN;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_REG_SN==null?"":model.HOS_REG_SN;
            parameters[7].Value = model.PRE_NO;
            parameters[8].Value = model.SFZ_NO;
            parameters[9].Value = model.YLCARD_TYPE;
            parameters[10].Value = model.YLCARD_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.DEPT_NAME;
            parameters[14].Value = model.DOC_NO == null ? "" : model.DOC_NO;
            parameters[15].Value = model.DOC_NAME == null ? "" : model.DOC_NAME;
            parameters[16].Value = model.DIS_NAME == null ? "" : model.DIS_NAME;
            parameters[17].Value = model.PAY_lTERMINAL_SN == null ? "" : model.PAY_lTERMINAL_SN;
            parameters[18].Value = model.CASH_JE;
            parameters[19].Value = model.PAY_TYPE;
            parameters[20].Value = model.JEALL;
            parameters[21].Value = model.JZ_CODE == null ? "" : model.JZ_CODE;
            parameters[22].Value = model.ybDJH == null ? "" : model.ybDJH;
            parameters[23].Value = model.GRZL;
            parameters[24].Value = model.GRZF;
            parameters[25].Value = model.TCZF;
            parameters[26].Value = model.DBZF;
            parameters[27].Value = model.XJZF;
            parameters[28].Value = model.ZHZF;
            parameters[29].Value = model.HM;
            parameters[30].Value = model.CS;
            parameters[31].Value = model.ZFY;
            parameters[32].Value = model.YF;
            parameters[33].Value = model.XMFY;
            parameters[34].Value = model.LCL;
            parameters[35].Value = model.ZHYE;
            parameters[36].Value = model.XZM == null ? "" : model.XZM;
            parameters[37].Value = model.XZMCH == null ? "" : model.XZMCH;
            parameters[38].Value = model.man_type == null ? "" : model.man_type;
            parameters[39].Value = model.BZFYY;
            parameters[40].Value = model.FYLB == null ? "" : model.FYLB;
            parameters[41].Value = model.YBBZM == null ? "" : model.YBBZM;
            parameters[42].Value = model.YBBZMC == null ? "" : model.YBBZMC;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.RCPT_NO;
            parameters[46].Value = model.HOS_PAY_SN;
            parameters[47].Value = model.IS_TZ;
            parameters[48].Value = model.TZ_DATE;
            parameters[49].Value = model.TZ_TIME;
            parameters[50].Value = model.PAY_ID_IN;
            parameters[51].Value = model.lTERMINAL_SN == null ? "" : model.lTERMINAL_SN;
            parameters[52].Value = model.SOURCE;

            table.Add(strSql.ToString(), parameters);

            if (info != null)
            {
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
            }

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
                strSql.Append("PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TNX_TYPE)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@WECHAT,@PAY_TYPE,@BIZ_TYPE,@BIZ_SN,@COMM_SN,@JE,@COMM_NAME,@DEAL_STATES,@DEAL_TIME,@DEAL_SN,@lTERMINAL_SN,@TNX_TYPE)");
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
					new MySqlParameter("@TNX_TYPE", MySqlDbType.VarChar,10)};
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
            if (tran != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into unionpay_tran(");
                strSql.Append("ORDERID,MERID,QUERYID,TN,JE,currencyCode,TXN_TYPE,txnSubType,bizType,channelType,accessType,eserved,reqReserved,REFCODE,orderDesc,TERMCODE,DJ_TIME,txnTime,AT_Time,ATrespCode,ATrespMsg)");
                strSql.Append(" values (");
                strSql.Append("@ORDERID,@MERID,@QUERYID,@TN,@JE,@currencyCode,@TXN_TYPE,@txnSubType,@bizType,@channelType,@accessType,@eserved,@reqReserved,@REFCODE,@orderDesc,@TERMCODE,@DJ_TIME,@txnTime,@AT_Time,@ATrespCode,@ATrespMsg)");
                MySqlParameter[] parameters5 = {
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30),
					new MySqlParameter("@MERID", MySqlDbType.VarChar,50),
					new MySqlParameter("@QUERYID", MySqlDbType.VarChar,30),
					new MySqlParameter("@TN", MySqlDbType.VarChar,21),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@currencyCode", MySqlDbType.VarChar,5),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@txnSubType", MySqlDbType.VarChar,2),
					new MySqlParameter("@bizType", MySqlDbType.VarChar,10),
					new MySqlParameter("@channelType", MySqlDbType.VarChar,2),
					new MySqlParameter("@accessType", MySqlDbType.VarChar,1),
					new MySqlParameter("@eserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@reqReserved", MySqlDbType.VarChar,10),
					new MySqlParameter("@REFCODE", MySqlDbType.VarChar,6),
					new MySqlParameter("@orderDesc", MySqlDbType.VarChar,20),
					new MySqlParameter("@TERMCODE", MySqlDbType.VarChar,15),
					new MySqlParameter("@DJ_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@txnTime", MySqlDbType.DateTime),
					new MySqlParameter("@AT_Time", MySqlDbType.DateTime),
					new MySqlParameter("@ATrespCode", MySqlDbType.VarChar,10),
					new MySqlParameter("@ATrespMsg", MySqlDbType.VarChar,10)};
                parameters5[0].Value = tran.ORDERID;
                parameters5[1].Value = tran.MERID;
                parameters5[2].Value = tran.QUERYID;
                parameters5[3].Value = tran.TN;
                parameters5[4].Value = tran.JE;
                parameters5[5].Value = tran.currencyCode;
                parameters5[6].Value = tran.TXN_TYPE;
                parameters5[7].Value = tran.txnSubType;
                parameters5[8].Value = tran.bizType;
                parameters5[9].Value = tran.channelType;
                parameters5[10].Value = tran.accessType;
                parameters5[11].Value = tran.eserved;
                parameters5[12].Value = tran.reqReserved;
                parameters5[13].Value = tran.REFCODE;
                parameters5[14].Value = tran.orderDesc;
                parameters5[15].Value = tran.TERMCODE;
                parameters5[16].Value = tran.DJ_TIME;
                parameters5[17].Value = tran.txnTime;
                parameters5[18].Value = tran.AT_Time;
                parameters5[19].Value = tran.ATrespCode;
                parameters5[20].Value = tran.ATrespMsg;
                table.Add(strSql, parameters5);
            }
            if (upcap != null)
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
            if (alitran != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into alipay_tran(");
                strSql.Append("COMM_SN,TRADE_NO,TRADE_STATUS,JE,TXN_TYPE,gmt_create,gmt_payment,notify_time,notify_type,notify_id,payment_type,seller_id,seller_email,buyer_id,buyer_email,body,subject,refund_status,gmt_refund,batch_no)");
                strSql.Append(" values (");
                strSql.Append("@COMM_SN,@TRADE_NO,@TRADE_STATUS,@JE,@TXN_TYPE,@gmt_create,@gmt_payment,@notify_time,@notify_type,@notify_id,@payment_type,@seller_id,@seller_email,@buyer_id,@buyer_email,@body,@subject,@refund_status,@gmt_refund,@batch_no)");
                MySqlParameter[] parameters8 = {
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@TRADE_NO", MySqlDbType.VarChar,64),
					new MySqlParameter("@TRADE_STATUS", MySqlDbType.VarChar,20),
					new MySqlParameter("@JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@gmt_create", MySqlDbType.DateTime),
					new MySqlParameter("@gmt_payment", MySqlDbType.DateTime),
					new MySqlParameter("@notify_time", MySqlDbType.DateTime),
					new MySqlParameter("@notify_type", MySqlDbType.VarChar,20),
					new MySqlParameter("@notify_id", MySqlDbType.VarChar,50),
					new MySqlParameter("@payment_type", MySqlDbType.VarChar,4),
					new MySqlParameter("@seller_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@seller_email", MySqlDbType.VarChar,100),
					new MySqlParameter("@buyer_id", MySqlDbType.VarChar,30),
					new MySqlParameter("@buyer_email", MySqlDbType.VarChar,100),
					new MySqlParameter("@body", MySqlDbType.VarChar,100),
					new MySqlParameter("@subject", MySqlDbType.VarChar,50),
					new MySqlParameter("@refund_status", MySqlDbType.VarChar,20),
					new MySqlParameter("@gmt_refund", MySqlDbType.DateTime),
					new MySqlParameter("@batch_no", MySqlDbType.VarChar,20)};
                parameters8[0].Value = alitran.COMM_SN;
                parameters8[1].Value = alitran.TRADE_NO;
                parameters8[2].Value = alitran.TRADE_STATUS;
                parameters8[3].Value = alitran.JE;
                parameters8[4].Value = alitran.TXN_TYPE;
                parameters8[5].Value = alitran.gmt_create;
                parameters8[6].Value = alitran.gmt_payment;
                parameters8[7].Value = alitran.notify_time;
                parameters8[8].Value = alitran.notify_type;
                parameters8[9].Value = alitran.notify_id;
                parameters8[10].Value = alitran.payment_type;
                parameters8[11].Value = alitran.seller_id;
                parameters8[12].Value = alitran.seller_email;
                parameters8[13].Value = alitran.buyer_id;
                parameters8[14].Value = alitran.buyer_email;
                parameters8[15].Value = alitran.body;
                parameters8[16].Value = alitran.subject;
                parameters8[17].Value = alitran.refund_status;
                parameters8[18].Value = alitran.gmt_refund;
                parameters8[19].Value = alitran.batch_no;

                table.Add(strSql, parameters8);
            }
            if (ccb != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_ccb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE,MerchantID)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE,@MerchantID)");
                MySqlParameter[] parameters10 = {
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
                parameters10[0].Value = ccb.PAY_ID;
                parameters10[1].Value = ccb.BIZ_TYPE;
                parameters10[2].Value = ccb.BDj_id;
                parameters10[3].Value = ccb.JE;
                parameters10[4].Value = ccb.POSID;
                parameters10[5].Value = ccb.BRANCHID;
                parameters10[6].Value = ccb.ORDERID;
                parameters10[7].Value = ccb.SFZ_NO;
                parameters10[8].Value = ccb.DJ_TIME;
                parameters10[9].Value = ccb.TXN_TYPE;
                parameters10[10].Value = ccb.MerchantID;

                table.Add(strSql, parameters10);
            }

            strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters9 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HSP_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters9[0].Value = log.PAY_ID;
            parameters9[1].Value = log.STATES;
            parameters9[2].Value = log.HOS_ID;
            parameters9[3].Value = log.PAT_ID;
            parameters9[4].Value = log.HSP_SN;
            parameters9[5].Value = log.JEALL;
            parameters9[6].Value = model.CASH_JE;
            parameters9[7].Value = log.DJ_DATE;
            parameters9[8].Value = log.DJ_TIME;
            parameters9[9].Value = log.lTERMINAL_SN==null?"": log.lTERMINAL_SN;

            table.Add(strSql, parameters9);

            //strSql = new StringBuilder();
            //strSql.Append("update register_appt set APPT_TYPE=1 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            //MySqlParameter[] parameters10 = {
            //new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
            //new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30)};

            //parameters10[0].Value = model.HOS_ID;
            //parameters10[1].Value = model.HOS_REG_SN;
            //table.Add(strSql.ToString(), parameters10);//退费后状态为 已就诊 hlw mod 2016.03.23
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
                    modSqlError.TYPE = "诊间退费保存";
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

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddByTran_ZZJ(Plat.Model.opt_pay model, Plat.Model.opt_pay_fl[] fl, Plat.Model.opt_pay_mx[] mx, Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Plat.Model.pay_info_wc wc, Plat.Model.pay_info_bank bank, Plat.Model.opt_pay_log log, Plat.Model.pay_info_upcap upcap, Plat.Model.pay_info_ccb ccb,Plat.Model.pay_info_yb yb,string yjs_in,string yjs_out)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,RCPT_NO,HOS_PAY_SN,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN,SOURCE)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@USER_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@RCPT_NO,@HOS_PAY_SN,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN,@SOURCE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@USER_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@OPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_REG_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,18),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAY_lTERMINAL_SN", MySqlDbType.VarChar,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@PAY_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@JZ_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ybDJH", MySqlDbType.VarChar,15),
					new MySqlParameter("@GRZL", MySqlDbType.Decimal,10),
					new MySqlParameter("@GRZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@TCZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@DBZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XJZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHZF", MySqlDbType.Decimal,10),
					new MySqlParameter("@HM", MySqlDbType.Decimal,10),
					new MySqlParameter("@CS", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@YF", MySqlDbType.Decimal,10),
					new MySqlParameter("@XMFY", MySqlDbType.Decimal,10),
					new MySqlParameter("@LCL", MySqlDbType.Decimal,10),
					new MySqlParameter("@ZHYE", MySqlDbType.Decimal,10),
					new MySqlParameter("@XZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@XZMCH", MySqlDbType.VarChar,20),
					new MySqlParameter("@man_type", MySqlDbType.VarChar,10),
					new MySqlParameter("@BZFYY", MySqlDbType.VarChar,10),
					new MySqlParameter("@FYLB", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZM", MySqlDbType.VarChar,10),
					new MySqlParameter("@YBBZMC", MySqlDbType.VarChar,20),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@RCPT_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@HOS_PAY_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10)
                                          };
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.USER_ID;
            parameters[4].Value = model.HOS_SN;
            parameters[5].Value = model.OPT_SN;
            parameters[6].Value = model.HOS_REG_SN;
            parameters[7].Value = model.PRE_NO;
            parameters[8].Value = model.SFZ_NO;
            parameters[9].Value = model.YLCARD_TYPE;
            parameters[10].Value = model.YLCARD_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.DEPT_CODE;
            parameters[13].Value = model.DEPT_NAME;
            parameters[14].Value = model.DOC_NO;
            parameters[15].Value = model.DOC_NAME;
            parameters[16].Value = model.DIS_NAME;
            parameters[17].Value = model.PAY_lTERMINAL_SN;
            parameters[18].Value = model.CASH_JE;
            parameters[19].Value = model.PAY_TYPE;
            parameters[20].Value = model.JEALL;
            parameters[21].Value = model.JZ_CODE;
            parameters[22].Value = model.ybDJH;
            parameters[23].Value = model.GRZL;
            parameters[24].Value = model.GRZF;
            parameters[25].Value = model.TCZF;
            parameters[26].Value = model.DBZF;
            parameters[27].Value = model.XJZF;
            parameters[28].Value = model.ZHZF;
            parameters[29].Value = model.HM;
            parameters[30].Value = model.CS;
            parameters[31].Value = model.ZFY;
            parameters[32].Value = model.YF;
            parameters[33].Value = model.XMFY;
            parameters[34].Value = model.LCL;
            parameters[35].Value = model.ZHYE;
            parameters[36].Value = model.XZM;
            parameters[37].Value = model.XZMCH;
            parameters[38].Value = model.man_type;
            parameters[39].Value = model.BZFYY;
            parameters[40].Value = model.FYLB;
            parameters[41].Value = model.YBBZM;
            parameters[42].Value = model.YBBZMC;
            parameters[43].Value = model.DJ_DATE;
            parameters[44].Value = model.DJ_TIME;
            parameters[45].Value = model.RCPT_NO;
            parameters[46].Value = model.HOS_PAY_SN;
            parameters[47].Value = model.IS_TZ;
            parameters[48].Value = model.TZ_DATE;
            parameters[49].Value = model.TZ_TIME;
            parameters[50].Value = model.PAY_ID_IN;
            parameters[51].Value = model.lTERMINAL_SN;
            parameters[52].Value = model.SOURCE;

            table.Add(strSql.ToString(), parameters);

            for (int i = 0; i < fl.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_fl(");
                strSql.Append("PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@FL_NAME,@DEPT_CODE,@DEPT_NAME,@FL_JE,@FL_ORDER)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10)};
                parameters1[0].Value = fl[i].PAY_ID;
                parameters1[1].Value = fl[i].FL_NO;
                parameters1[2].Value = fl[i].FL_NAME;
                parameters1[3].Value = fl[i].DEPT_CODE;
                parameters1[4].Value = fl[i].DEPT_NAME;
                parameters1[5].Value = fl[i].FL_JE;
                parameters1[6].Value = fl[i].FL_ORDER;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }

            for (int i = 0; i < mx.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_mx(");
                strSql.Append("PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@ITEM_TYPE,@ITEM_ID,@ITEM_NAME,@ITEM_GG,@COUNT,@ITEM_UNIT,@COST,@je)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,30),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10)};
                parameters1[0].Value = mx[i].PAY_ID;
                parameters1[1].Value = mx[i].FL_NO;
                parameters1[2].Value = mx[i].ITEM_TYPE;
                parameters1[3].Value = mx[i].ITEM_ID;
                parameters1[4].Value = mx[i].ITEM_NAME;
                parameters1[5].Value = mx[i].ITEM_GG;
                parameters1[6].Value = mx[i].COUNT;
                parameters1[7].Value = mx[i].ITEM_UNIT;
                parameters1[8].Value = mx[i].COST;
                parameters1[9].Value = mx[i].je;


                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_mx_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters2[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters2);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_fl_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters3[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters3);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters4[0].Value = model.LockPAY_ID;
            table.Add(strSql.ToString(), parameters4);



            //现金交易记录表
            strSql = new StringBuilder();
            strSql.Append("insert into pay_info(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,USER_ID,BIZ_TYPE,BIZ_SN,CASH_JE,SFZ_NO,DJ_DATE,DJ_TIME,DEAL_TYPE,DEAL_STATES,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@USER_ID,@BIZ_TYPE,@BIZ_SN,@CASH_JE,@SFZ_NO,@DJ_DATE,@DJ_TIME,@DEAL_TYPE,@DEAL_STATES,@lTERMINAL_SN)");
            MySqlParameter[] parameters5 = {
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
            parameters5[0].Value = info.PAY_ID;
            parameters5[1].Value = info.HOS_ID;
            parameters5[2].Value = info.PAT_ID;
            parameters5[3].Value = info.USER_ID;
            parameters5[4].Value = info.BIZ_TYPE;
            parameters5[5].Value = info.BIZ_SN;
            parameters5[6].Value = info.CASH_JE;
            parameters5[7].Value = info.SFZ_NO;
            parameters5[8].Value = info.DJ_DATE;
            parameters5[9].Value = info.DJ_TIME;
            parameters5[10].Value = info.DEAL_TYPE;
            parameters5[11].Value = info.DEAL_STATES;
            parameters5[12].Value = info.lTERMINAL_SN;

            table.Add(strSql, parameters5);

            if (zfb != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_zfb(");
                strSql.Append("PAY_ID,BIZ_TYPE,BIZ_SN,SELLER_ID,COMM_SN,JE,DEAL_STATES,TXN_TYPE,DEAL_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BIZ_SN,@SELLER_ID,@COMM_SN,@JE,@DEAL_STATES,@TXN_TYPE,@DEAL_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters6 = {
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
                parameters6[0].Value = zfb.PAY_ID;
                parameters6[1].Value = zfb.BIZ_TYPE;
                parameters6[2].Value = zfb.BIZ_SN;
                parameters6[3].Value = zfb.SELLER_ID;
                parameters6[4].Value = zfb.COMM_SN;
                parameters6[5].Value = zfb.JE;
                parameters6[6].Value = zfb.DEAL_STATES;
                parameters6[7].Value = zfb.TXN_TYPE;
                parameters6[8].Value = zfb.DEAL_TIME;
                parameters6[9].Value = zfb.lTERMINAL_SN;

                table.Add(strSql, parameters6);
            }
            else if (wc != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_wc(");
                strSql.Append("PAY_ID,WECHAT,PAY_TYPE,BIZ_TYPE,BIZ_SN,COMM_SN,JE,COMM_NAME,DEAL_STATES,DEAL_TIME,DEAL_SN,lTERMINAL_SN,TXN_TYPE)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@WECHAT,@PAY_TYPE,@BIZ_TYPE,@BIZ_SN,@COMM_SN,@JE,@COMM_NAME,@DEAL_STATES,@DEAL_TIME,@DEAL_SN,@lTERMINAL_SN,@TXN_TYPE)");
                MySqlParameter[] parameters7 = {
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
                parameters7[0].Value = wc.PAY_ID;
                parameters7[1].Value = wc.WECHAT;
                parameters7[2].Value = wc.PAY_TYPE;
                parameters7[3].Value = wc.BIZ_TYPE;
                parameters7[4].Value = wc.BIZ_SN;
                parameters7[5].Value = wc.COMM_SN;
                parameters7[6].Value = wc.JE;
                parameters7[7].Value = wc.COMM_NAME;
                parameters7[8].Value = wc.DEAL_STATES;
                parameters7[9].Value = wc.DEAL_TIME;
                parameters7[10].Value = wc.DEAL_SN;
                parameters7[11].Value = wc.lTERMINAL_SN;
                parameters7[12].Value = wc.TNX_TYPE;

                table.Add(strSql, parameters7);
            }
            else if (bank != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_bank(");
                strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,BANK_TYPE,RETURN_CODE,BANK_CARD,CARD_TYPE,SEARCH_CODE,REFCODE,TERMCODE,CARD_COMPAY,COMM_NAME,COMM_SN,SFZ_NO,DJ_TIME)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@BANK_TYPE,@RETURN_CODE,@BANK_CARD,@CARD_TYPE,@SEARCH_CODE,@REFCODE,@TERMCODE,@CARD_COMPAY,@COMM_NAME,@COMM_SN,@SFZ_NO,@DJ_TIME)");
                MySqlParameter[] parameters8 = {
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
                parameters8[0].Value = bank.PAY_ID;
                parameters8[1].Value = bank.BIZ_TYPE;
                parameters8[2].Value = bank.BDj_id;
                parameters8[3].Value = bank.JE;
                parameters8[4].Value = bank.BANK_TYPE;
                parameters8[5].Value = bank.RETURN_CODE;
                parameters8[6].Value = bank.BANK_CARD;
                parameters8[7].Value = bank.CARD_TYPE;
                parameters8[8].Value = bank.SEARCH_CODE;
                parameters8[9].Value = bank.REFCODE;
                parameters8[10].Value = bank.TERMCODE;
                parameters8[11].Value = bank.CARD_COMPAY;
                parameters8[12].Value = bank.COMM_NAME;
                parameters8[13].Value = bank.COMM_SN;
                parameters8[14].Value = bank.SFZ_NO;
                parameters8[15].Value = bank.DJ_TIME;
                table.Add(strSql, parameters8);
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
                //strSql.Append("PAY_ID,BIZ_TYPE,BDj_id,JE,POSID,BRANCHID,ORDERID,SFZ_NO,DJ_TIME,TXN_TYPE,MerchantID)");
                //strSql.Append(" values (");
                //strSql.Append("@PAY_ID,@BIZ_TYPE,@BDj_id,@JE,@POSID,@BRANCHID,@ORDERID,@SFZ_NO,@DJ_TIME,@TXN_TYPE,@MerchantID)");
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

            strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters9 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@STATES", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HSP_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@JEALL", MySqlDbType.Decimal,10),
					new MySqlParameter("@CASH_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@DJ_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters9[0].Value = log.PAY_ID;
            parameters9[1].Value = log.STATES;
            parameters9[2].Value = log.HOS_ID;
            parameters9[3].Value = log.PAT_ID;
            parameters9[4].Value = log.HSP_SN;
            parameters9[5].Value = log.JEALL;
            parameters9[6].Value = model.CASH_JE;
            parameters9[7].Value = log.DJ_DATE;
            parameters9[8].Value = log.DJ_TIME;
            parameters9[9].Value = log.lTERMINAL_SN;

            table.Add(strSql, parameters9);


            strSql = new StringBuilder();
            strSql.Append("update register_appt set APPT_TYPE=4 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            MySqlParameter[] parameters10 = {
            new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
            new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30)};

            parameters10[0].Value = model.HOS_ID;
            parameters10[1].Value = model.HOS_REG_SN;
            table.Add(strSql.ToString(), parameters10);

            #region
            if (yb != null)
            {

                strSql = new StringBuilder();
                strSql.Append("insert into pay_info_yb(");
                strSql.Append("PAY_ID,DJ_ID,TRADELSH,MZLSH,DJLSH,YL_TYPE,JS_DATE,CY_DATE,CY_REASON,DIS_CODE,YJSTYPE,ZTYJSTYPE,USR_NAME,FM_DATE,CC,TAIER_AMOUNT,CARDID,ZYYBBH,DEPT_CODE,DOC_NO,IS_GH,ZSRCARDID,SS_ISSUCCESS,BC_ZJE,BC_TCJE,BC_DBJZ,BC_DBBX,BC_MZBZ,BC_ZHZC,BC_XJZC,BC_ZHZCZF,BC_ZHZCZL,BC_XJZCZF,BC_XJZCZL,YBFWNJE,ZHYE,DBZ_CODE,INSTRUCTION,MED_JE,CHK_JE,BB_JE,BY6,lTERMINAL_SN,DEAL_TIME,YWZQH)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@DJ_ID,@TRADELSH,@MZLSH,@DJLSH,@YL_TYPE,@JS_DATE,@CY_DATE,@CY_REASON,@DIS_CODE,@YJSTYPE,@ZTYJSTYPE,@USR_NAME,@FM_DATE,@CC,@TAIER_AMOUNT,@CARDID,@ZYYBBH,@DEPT_CODE,@DOC_NO,@IS_GH,@ZSRCARDID,@SS_ISSUCCESS,@BC_ZJE,@BC_TCJE,@BC_DBJZ,@BC_DBBX,@BC_MZBZ,@BC_ZHZC,@BC_XJZC,@BC_ZHZCZF,@BC_ZHZCZL,@BC_XJZCZF,@BC_XJZCZL,@YBFWNJE,@ZHYE,@DBZ_CODE,@INSTRUCTION,@MED_JE,@CHK_JE,@BB_JE,@BY6,@lTERMINAL_SN,@DEAL_TIME,@YWZQH)");
                MySqlParameter[] parameters11 = {
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
                parameters11[0].Value = yb.PAY_ID;
                parameters11[1].Value = yb.DJ_ID;
                parameters11[2].Value = yb.TRADELSH;
                parameters11[3].Value = yb.MZLSH;
                parameters11[4].Value = yb.DJLSH;
                parameters11[5].Value = yb.YL_TYPE;
                parameters11[6].Value = yb.JS_DATE;
                parameters11[7].Value = yb.CY_DATE;
                parameters11[8].Value = yb.CY_REASON;
                parameters11[9].Value = yb.DIS_CODE;
                parameters11[10].Value = yb.YJSTYPE;
                parameters11[11].Value = yb.ZTYJSTYPE;
                parameters11[12].Value = yb.USR_NAME;
                parameters11[13].Value = yb.FM_DATE;
                parameters11[14].Value = yb.CC;
                parameters11[15].Value = yb.TAIER_AMOUNT;
                parameters11[16].Value = yb.CARDID;
                parameters11[17].Value = yb.ZYYBBH;
                parameters11[18].Value = yb.DEPT_CODE;
                parameters11[19].Value = yb.DOC_NO;
                parameters11[20].Value = yb.IS_GH;
                parameters11[21].Value = yb.ZSRCARDID;
                parameters11[22].Value = yb.SS_ISSUCCESS;
                parameters11[23].Value = yb.BC_ZJE;
                parameters11[24].Value = yb.BC_TCJE;
                parameters11[25].Value = yb.BC_DBJZ;
                parameters11[26].Value = yb.BC_DBBX;
                parameters11[27].Value = yb.BC_MZBZ;
                parameters11[28].Value = yb.BC_ZHZC;
                parameters11[29].Value = yb.BC_XJZC;
                parameters11[30].Value = yb.BC_ZHZCZF;
                parameters11[31].Value = yb.BC_ZHZCZL;
                parameters11[32].Value = yb.BC_XJZCZF;
                parameters11[33].Value = yb.BC_XJZCZL;
                parameters11[34].Value = yb.YBFWNJE;
                parameters11[35].Value = yb.ZHYE;
                parameters11[36].Value = yb.DBZ_CODE;
                parameters11[37].Value = yb.INSTRUCTION;
                parameters11[38].Value = yb.MED_JE;
                parameters11[39].Value = yb.CHK_JE;
                parameters11[40].Value = yb.BB_JE;
                parameters11[41].Value = yb.BY6;
                parameters11[42].Value = yb.lTERMINAL_SN;
                parameters11[43].Value = yb.DEAL_TIME;
                parameters11[44].Value = yb.YWZQH;
                table.Add(strSql, parameters11);
            }
            //医保明细入参
            //if (yjs_in != "")
            //{
            //    /* <YJS_IN>2310^H0027^920001^0027-00920001-20180629042027^20180703164235-H0027-920001^0^0^ZZJjs20180703164233|3|7000145382|1|20180703164235|q5624|14.0000|1|14.0000|1|00719|207|ZZJ01||Z85.300|||||$ZZJjs20180703164233|2|7000145382|2|20180703164235|4233|15.00|1|15.00|1|00719|207|ZZJ01||Z85.300|||||$ZZJjs20180703164233|2|7000145382|3|20180703164235|4199a|70.00|1|70.00|1|00719|207|ZZJ01||Z85.300|||||$ZZJjs20180703164233|2|7000145382|4|20180703164235|4216a|55.00|1|55.00|1|00719|207|ZZJ01||Z85.300|||||^</YJS_IN>*/
            //    string strYJSIN = yjs_in.Split('^').Length >= 7 ? yjs_in.Split('^')[7] : "";
            //    string ywzqh = yjs_in.Split('^')[3];
            //}
            //医保明细出参
            if (yjs_out != "")
            {
                yjs_out = yjs_out.Replace("$$$", "");
                string[] stryjsout = yjs_out.Split('$');
                for (int i = 0; i <= stryjsout.Length - 1; i++)
                {

                    string[] stryjsmxlog = stryjsout[i].Split('|');
                    string preno = "";
                    if (i == 0)
                    {
                        preno = stryjsmxlog[0].Split('^')[1].Trim();
                    }
                    else
                    {
                        preno = stryjsmxlog[0];
                    }
                    string da_id = stryjsmxlog[1];
                    string dj_date = stryjsmxlog[2];
                    string xmzbm = stryjsmxlog[3];
                    string zje = stryjsmxlog[4];
                    string zfje = stryjsmxlog[5];
                    string zlje = stryjsmxlog[6];
                    string zfbl = stryjsmxlog[7];
                    string zfsx = stryjsmxlog[8];
                    string sfxmdj = stryjsmxlog[9];
                    string smxx = stryjsmxlog[10];
                    string by2 = stryjsmxlog[11];
                    string by3 = stryjsmxlog[12];
                    string by4 = stryjsmxlog[13];
                    string by5 = stryjsmxlog[14];
                    string by6 = stryjsmxlog[15];
                    string itemtype = "0";
                    
                    strSql = new StringBuilder();
                    strSql.Append("insert into pay_info_ybjsmxlog(");
                    strSql.Append("PAY_ID,DJ_ID,PRE_NO,DA_ID,DJ_DATE,ITEMTYPE,XMZBM,ZJE,ZFJE,ZLJE,ZFBL,ZFSX,SFXMDJ,SMXX,BY2,BY3,BY4,BY5,BY6)");
                    strSql.Append(" values (");
                    strSql.Append("@PAY_ID,@DJ_ID,@PRE_NO,@DA_ID,@DJ_DATE,@ITEMTYPE,@XMZBM,@ZJE,@ZFJE,@ZLJE,@ZFBL,@ZFSX,@SFXMDJ,@SMXX,@BY2,@BY3,@BY4,@BY5,@BY6)");
                    MySqlParameter[] parameters11 = {
                    new MySqlParameter("@PAY_ID", MySqlDbType.Int16,4),
                    new MySqlParameter("@DJ_ID", MySqlDbType.VarChar,30),
                    new MySqlParameter("@PRE_NO", MySqlDbType.VarChar,30),
                    new MySqlParameter("@DA_ID", MySqlDbType.Int16,4),
                    new MySqlParameter("@DJ_DATE", MySqlDbType.VarChar,30),
                    new MySqlParameter("@ITEMTYPE", MySqlDbType.VarChar,20),
                    new MySqlParameter("@XMZBM", MySqlDbType.VarChar,30),
                    new MySqlParameter("@ZJE", MySqlDbType.VarChar,18),
                    new MySqlParameter("@ZFJE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ZLJE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ZFBL", MySqlDbType.VarChar,10),
                    new MySqlParameter("@ZFSX", MySqlDbType.VarChar,10),
                    new MySqlParameter("@SFXMDJ", MySqlDbType.VarChar,10),
                    new MySqlParameter("@SMXX", MySqlDbType.VarChar,10),
                    new MySqlParameter("@BY2", MySqlDbType.VarChar,100),
                    new MySqlParameter("@BY3", MySqlDbType.VarChar,100),
                    new MySqlParameter("@BY4", MySqlDbType.VarChar,100),
                    new MySqlParameter("@BY5", MySqlDbType.VarChar,100),
                    new MySqlParameter("@BY6", MySqlDbType.VarChar,100), };
                    parameters11[0].Value = yb.PAY_ID;
                    parameters11[1].Value = yb.DJ_ID;
                    parameters11[2].Value = preno;
                    parameters11[3].Value = da_id;
                    parameters11[4].Value = dj_date;
                    parameters11[5].Value = itemtype;
                    parameters11[6].Value = xmzbm;
                    parameters11[7].Value = zje;
                    parameters11[8].Value = zfje;
                    parameters11[9].Value = zlje;
                    parameters11[10].Value = zfbl;
                    parameters11[11].Value = zfsx;
                    parameters11[12].Value = sfxmdj;
                    parameters11[13].Value = smxx;
                    parameters11[14].Value = by2;
                    parameters11[15].Value = by3;
                    parameters11[16].Value = by4;
                    parameters11[17].Value = by5;
                    parameters11[18].Value = by6;
                    table.Add(strSql, parameters11);

                }
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
                    modSqlError.TYPE = "诊间支付保存";
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
        #endregion  Method
    }
}


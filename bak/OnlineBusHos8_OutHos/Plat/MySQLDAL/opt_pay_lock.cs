using System;
using System.Data;
using System.Text;
using DB.Core;
using Log.Core.Model;
using MySql.Data.MySqlClient;
namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:opt_pay_lock
    /// </summary>
    public partial class opt_pay_lock 
    {
        public opt_pay_lock()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string PAY_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from opt_pay_lock");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.opt_pay_lock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_lock(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@USER_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
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
					new MySqlParameter("@DJ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.USER_ID;
            parameters[5].Value = model.HOS_SN;
            parameters[6].Value = model.OPT_SN;
            parameters[7].Value = model.HOS_REG_SN;
            parameters[8].Value = model.PRE_NO;
            parameters[9].Value = model.SFZ_NO;
            parameters[10].Value = model.YLCARD_TYPE;
            parameters[11].Value = model.YLCARD_NO;
            parameters[12].Value = model.PAT_NAME;
            parameters[13].Value = model.DEPT_CODE;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME;
            parameters[18].Value = model.PAY_lTERMINAL_SN;
            parameters[19].Value = model.CASH_JE;
            parameters[20].Value = model.PAY_TYPE;
            parameters[21].Value = model.JEALL;
            parameters[22].Value = model.JZ_CODE;
            parameters[23].Value = model.ybDJH;
            parameters[24].Value = model.GRZL;
            parameters[25].Value = model.GRZF;
            parameters[26].Value = model.TCZF;
            parameters[27].Value = model.DBZF;
            parameters[28].Value = model.XJZF;
            parameters[29].Value = model.ZHZF;
            parameters[30].Value = model.HM;
            parameters[31].Value = model.CS;
            parameters[32].Value = model.ZFY;
            parameters[33].Value = model.YF;
            parameters[34].Value = model.XMFY;
            parameters[35].Value = model.LCL;
            parameters[36].Value = model.ZHYE;
            parameters[37].Value = model.XZM;
            parameters[38].Value = model.XZMCH;
            parameters[39].Value = model.man_type;
            parameters[40].Value = model.BZFYY;
            parameters[41].Value = model.FYLB;
            parameters[42].Value = model.YBBZM;
            parameters[43].Value = model.YBBZMC;
            parameters[44].Value = model.DJ_DATE;
            parameters[45].Value = model.DJ_TIME;
            parameters[46].Value = model.IS_TZ;
            parameters[47].Value = model.TZ_DATE;
            parameters[48].Value = model.TZ_TIME;
            parameters[49].Value = model.PAY_ID_IN;
            parameters[50].Value = model.lTERMINAL_SN;

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
        public bool Update(Plat.Model.opt_pay_lock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update opt_pay_lock set ");
            strSql.Append("HOS_ID=@HOS_ID,");
            strSql.Append("PAT_ID=@PAT_ID,");
            strSql.Append("REGPAT_ID=@REGPAT_ID,");
            strSql.Append("USER_ID=@USER_ID,");
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
					new MySqlParameter("@DJ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.PAT_ID;
            parameters[2].Value = model.REGPAT_ID;
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
            parameters[45].Value = model.IS_TZ;
            parameters[46].Value = model.TZ_DATE;
            parameters[47].Value = model.TZ_TIME;
            parameters[48].Value = model.PAY_ID_IN;
            parameters[49].Value = model.lTERMINAL_SN;
            parameters[50].Value = model.PAY_ID;

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
            strSql.Append("delete from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)			};
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
            strSql.Append("delete from opt_pay_lock ");
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
        public Plat.Model.opt_pay_lock GetModel(string PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;

            Plat.Model.opt_pay_lock model = new Plat.Model.opt_pay_lock();
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
        public Plat.Model.opt_pay_lock GetModel_ZZJ(string PAY_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)			};
            parameters[0].Value = PAY_ID;

            Plat.Model.opt_pay_lock model = new Plat.Model.opt_pay_lock();
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
        public Plat.Model.opt_pay_lock DataRowToModel(DataRow row)
        {
            Plat.Model.opt_pay_lock model = new Plat.Model.opt_pay_lock();
            if (row != null)
            {
                if (row["PAY_ID"] != null)
                {
                    model.PAY_ID = row["PAY_ID"].ToString();
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
                if (row["USER_ID"] != null && row["USER_ID"].ToString() != "")
                {
                    model.USER_ID = int.Parse(row["USER_ID"].ToString());
                }
                if (row["HOS_SN"] != null)
                {
                    model.HOS_SN = row["HOS_SN"].ToString();
                }
                if (row["OPT_SN"] != null)
                {
                    model.OPT_SN = row["OPT_SN"].ToString();
                }
                if (row["HOS_REG_SN"] != null)
                {
                    model.HOS_REG_SN = row["HOS_REG_SN"].ToString();
                }
                if (row["PRE_NO"] != null)
                {
                    model.PRE_NO = row["PRE_NO"].ToString();
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
                if (row["DEPT_CODE"] != null)
                {
                    model.DEPT_CODE = row["DEPT_CODE"].ToString();
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
                if (row["PAY_lTERMINAL_SN"] != null)
                {
                    model.PAY_lTERMINAL_SN = row["PAY_lTERMINAL_SN"].ToString();
                }
                if (row["CASH_JE"] != null && row["CASH_JE"].ToString() != "")
                {
                    model.CASH_JE = decimal.Parse(row["CASH_JE"].ToString());
                }
                if (row["PAY_TYPE"] != null && row["PAY_TYPE"].ToString() != "")
                {
                    model.PAY_TYPE = int.Parse(row["PAY_TYPE"].ToString());
                }
                if (row["JEALL"] != null && row["JEALL"].ToString() != "")
                {
                    model.JEALL = decimal.Parse(row["JEALL"].ToString());
                }
                if (row["JZ_CODE"] != null)
                {
                    model.JZ_CODE = row["JZ_CODE"].ToString();
                }
                if (row["ybDJH"] != null)
                {
                    model.ybDJH = row["ybDJH"].ToString();
                }
                if (row["GRZL"] != null && row["GRZL"].ToString() != "")
                {
                    model.GRZL = decimal.Parse(row["GRZL"].ToString());
                }
                if (row["GRZF"] != null && row["GRZF"].ToString() != "")
                {
                    model.GRZF = decimal.Parse(row["GRZF"].ToString());
                }
                if (row["TCZF"] != null && row["TCZF"].ToString() != "")
                {
                    model.TCZF = decimal.Parse(row["TCZF"].ToString());
                }
                if (row["DBZF"] != null && row["DBZF"].ToString() != "")
                {
                    model.DBZF = decimal.Parse(row["DBZF"].ToString());
                }
                if (row["XJZF"] != null && row["XJZF"].ToString() != "")
                {
                    model.XJZF = decimal.Parse(row["XJZF"].ToString());
                }
                if (row["ZHZF"] != null && row["ZHZF"].ToString() != "")
                {
                    model.ZHZF = decimal.Parse(row["ZHZF"].ToString());
                }
                if (row["HM"] != null && row["HM"].ToString() != "")
                {
                    model.HM = decimal.Parse(row["HM"].ToString());
                }
                if (row["CS"] != null && row["CS"].ToString() != "")
                {
                    model.CS = decimal.Parse(row["CS"].ToString());
                }
                if (row["ZFY"] != null && row["ZFY"].ToString() != "")
                {
                    model.ZFY = decimal.Parse(row["ZFY"].ToString());
                }
                if (row["YF"] != null && row["YF"].ToString() != "")
                {
                    model.YF = decimal.Parse(row["YF"].ToString());
                }
                if (row["XMFY"] != null && row["XMFY"].ToString() != "")
                {
                    model.XMFY = decimal.Parse(row["XMFY"].ToString());
                }
                if (row["LCL"] != null && row["LCL"].ToString() != "")
                {
                    model.LCL = decimal.Parse(row["LCL"].ToString());
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
                if (row["man_type"] != null)
                {
                    model.man_type = row["man_type"].ToString();
                }
                if (row["BZFYY"] != null)
                {
                    model.BZFYY = row["BZFYY"].ToString();
                }
                if (row["FYLB"] != null)
                {
                    model.FYLB = row["FYLB"].ToString();
                }
                if (row["YBBZM"] != null)
                {
                    model.YBBZM = row["YBBZM"].ToString();
                }
                if (row["YBBZMC"] != null)
                {
                    model.YBBZMC = row["YBBZMC"].ToString();
                }
                if (row["DJ_DATE"] != null && row["DJ_DATE"].ToString() != "")
                {
                    model.DJ_DATE = DateTime.Parse(row["DJ_DATE"].ToString());
                }
                if (row["DJ_TIME"] != null)
                {
                    model.DJ_TIME = row["DJ_TIME"].ToString();
                }
                if (row["IS_TZ"] != null && row["IS_TZ"].ToString() != "")
                {
                    if ((row["IS_TZ"].ToString() == "1") || (row["IS_TZ"].ToString().ToLower() == "true"))
                    {
                        model.IS_TZ = true;
                    }
                    else
                    {
                        model.IS_TZ = false;
                    }
                }
                if (row["TZ_DATE"] != null && row["TZ_DATE"].ToString() != "")
                {
                    model.TZ_DATE = DateTime.Parse(row["TZ_DATE"].ToString());
                }
                if (row["TZ_TIME"] != null)
                {
                    model.TZ_TIME = row["TZ_TIME"].ToString();
                }
                if (row["PAY_ID_IN"] != null)
                {
                    model.PAY_ID_IN = row["PAY_ID_IN"].ToString();
                }
                if (row["lTERMINAL_SN"] != null)
                {
                    model.lTERMINAL_SN = row["lTERMINAL_SN"].ToString();
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
            strSql.Append("select PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN ");
            strSql.Append(" FROM opt_pay_lock ");
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
            strSql.Append("select count(1) FROM opt_pay_lock ");
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
            strSql.Append(")AS Row, T.*  from opt_pay_lock T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLZZJ.Query(strSql.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool AddByTran(Plat.Model.opt_pay_lock model, Plat.Model.opt_pay_fl_lock[] fl_lock, Plat.Model.opt_pay_mx_lock[] mx_lock,Plat.Model.opt_pay_log log)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_lock(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@USER_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
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
					new MySqlParameter("@DJ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.USER_ID;
            parameters[5].Value = model.HOS_SN;
            parameters[6].Value = model.OPT_SN;
            parameters[7].Value = model.HOS_REG_SN == null ? "" : model.HOS_REG_SN;
            parameters[8].Value = model.PRE_NO;
            parameters[9].Value = model.SFZ_NO;
            parameters[10].Value = model.YLCARD_TYPE;
            parameters[11].Value = model.YLCARD_NO;
            parameters[12].Value = model.PAT_NAME;
            parameters[13].Value = model.DEPT_CODE;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME==null?"":model.DIS_NAME;
            parameters[18].Value = model.PAY_lTERMINAL_SN;
            parameters[19].Value = model.CASH_JE;
            parameters[20].Value = model.PAY_TYPE;
            parameters[21].Value = model.JEALL;
            parameters[22].Value = model.JZ_CODE == null ? "" : model.JZ_CODE;
            parameters[23].Value = model.ybDJH == null ? "" : model.ybDJH;
            parameters[24].Value = model.GRZL;
            parameters[25].Value = model.GRZF;
            parameters[26].Value = model.TCZF;
            parameters[27].Value = model.DBZF;
            parameters[28].Value = model.XJZF;
            parameters[29].Value = model.ZHZF;
            parameters[30].Value = model.HM;
            parameters[31].Value = model.CS;
            parameters[32].Value = model.ZFY;
            parameters[33].Value = model.YF;
            parameters[34].Value = model.XMFY;
            parameters[35].Value = model.LCL;
            parameters[36].Value = model.ZHYE;
            parameters[37].Value = model.XZM == null ? "" : model.XZM;
            parameters[38].Value = model.XZMCH == null ? "" : model.XZMCH;
            parameters[39].Value = model.man_type == null ? "" : model.man_type;
            parameters[40].Value = model.BZFYY == null ? "" : model.BZFYY;
            parameters[41].Value = model.FYLB == null ? "" : model.FYLB;
            parameters[42].Value = model.YBBZM == null ? "" : model.YBBZM;
            parameters[43].Value = model.YBBZMC == null ? "" : model.YBBZMC;
            parameters[44].Value = model.DJ_DATE;
            parameters[45].Value = model.DJ_TIME;
            parameters[46].Value = model.IS_TZ;
            parameters[47].Value = model.TZ_DATE;
            parameters[48].Value = model.TZ_TIME;
            parameters[49].Value = model.PAY_ID_IN;
            parameters[50].Value = model.lTERMINAL_SN;

            table.Add(strSql.ToString(), parameters);
            for (int i = 0; i < fl_lock.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_fl_lock(");
                strSql.Append("PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@FL_NAME,@DEPT_CODE,@DEPT_NAME,@FL_JE,@FL_ORDER)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10)};
                parameters1[0].Value = fl_lock[i].PAY_ID;
                parameters1[1].Value = fl_lock[i].FL_NO;
                parameters1[2].Value = fl_lock[i].FL_NAME;
                parameters1[3].Value = fl_lock[i].DEPT_CODE;
                parameters1[4].Value = fl_lock[i].DEPT_NAME;
                parameters1[5].Value = fl_lock[i].FL_JE;
                parameters1[6].Value = fl_lock[i].FL_ORDER;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }
            for (int i = 0; i < mx_lock.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_mx_lock(");
                strSql.Append("PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@ITEM_TYPE,@ITEM_ID,@ITEM_NAME,@ITEM_GG,@COUNT,@ITEM_UNIT,@COST,@je)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10)};
                parameters1[0].Value = mx_lock[i].PAY_ID;
                parameters1[1].Value = mx_lock[i].FL_NO;
                parameters1[2].Value = mx_lock[i].ITEM_TYPE;
                parameters1[3].Value = mx_lock[i].ITEM_ID;
                parameters1[4].Value = mx_lock[i].ITEM_NAME;
                parameters1[5].Value = mx_lock[i].ITEM_GG;
                parameters1[6].Value = mx_lock[i].COUNT;
                parameters1[7].Value = mx_lock[i].ITEM_UNIT;
                parameters1[8].Value = mx_lock[i].COST;
                parameters1[9].Value = mx_lock[i].je;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);

            }
            strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters2 = {
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
            parameters2[0].Value = log.PAY_ID;
            parameters2[1].Value = log.STATES;
            parameters2[2].Value = log.HOS_ID;
            parameters2[3].Value = log.PAT_ID;
            parameters2[4].Value = log.HSP_SN;
            parameters2[5].Value = log.JEALL;
            parameters2[6].Value = log.CASH_JE;
            parameters2[7].Value = log.DJ_DATE;
            parameters2[8].Value = log.DJ_TIME;
            parameters2[9].Value = log.lTERMINAL_SN;


            table.Add(strSql.ToString(), parameters2);

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
                    modSqlError.TYPE = "诊间支付锁定";
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
        public bool DeleteByTran(Plat.Model.opt_pay_log []model)
        {
            StringBuilder strSql = new StringBuilder(); 
            System.Collections.Hashtable table = new System.Collections.Hashtable();


            for (int i = 0; i < model.Length; i++)
            {
                if (model[i] == null)
                    continue;
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_log(");
                strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters = {
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
                parameters[0].Value = model[i].PAY_ID;
                parameters[1].Value = model[i].STATES;
                parameters[2].Value = model[i].HOS_ID;
                parameters[3].Value = model[i].PAT_ID;
                parameters[4].Value = model[i].HSP_SN;
                parameters[5].Value = model[i].JEALL;
                parameters[6].Value = model[i].CASH_JE;
                parameters[7].Value = model[i].DJ_DATE;
                parameters[8].Value = model[i].DJ_TIME;
                parameters[9].Value = model[i].lTERMINAL_SN;


                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_mx_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters2[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters2);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_fl_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters3[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters3);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters4[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters4);
            }

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
                    modSqlError.TYPE = "诊间锁定解锁";
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

        public bool DeleteByTran_ZZJ(Plat.Model.opt_pay_log[] model)
        {
            StringBuilder strSql = new StringBuilder();
            System.Collections.Hashtable table = new System.Collections.Hashtable();


            for (int i = 0; i < model.Length; i++)
            {
                if (model[i] == null)
                    continue;
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_log(");
                strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
                MySqlParameter[] parameters = {
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
                parameters[0].Value = model[i].PAY_ID;
                parameters[1].Value = model[i].STATES;
                parameters[2].Value = model[i].HOS_ID;
                parameters[3].Value = model[i].PAT_ID;
                parameters[4].Value = model[i].HSP_SN;
                parameters[5].Value = model[i].JEALL;
                parameters[6].Value = model[i].CASH_JE;
                parameters[7].Value = model[i].DJ_DATE;
                parameters[8].Value = model[i].DJ_TIME;
                parameters[9].Value = model[i].lTERMINAL_SN;


                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_mx_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters2[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters2);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_fl_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters3[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters3);

                strSql = new StringBuilder();
                strSql.Append("delete from opt_pay_lock ");
                strSql.Append(" where PAY_ID=@PAY_ID");
                MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
                parameters4[0].Value = model[i].PAY_ID;
                j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters4);
            }

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
                    modSqlError.TYPE = "诊间锁定解锁";
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

        public bool DeleteByTran(Plat.Model.opt_pay_log model)
        {
            StringBuilder strSql = new StringBuilder();
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
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
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.STATES;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.HSP_SN;
            parameters[5].Value = model.JEALL;
            parameters[6].Value = model.CASH_JE;
            parameters[7].Value = model.DJ_DATE;
            parameters[8].Value = model.DJ_TIME;
            parameters[9].Value = model.lTERMINAL_SN;

            table.Add(strSql.ToString(), parameters);


            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_mx_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters2[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters2);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_fl_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters3[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters3);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters4[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters4);

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
                    modSqlError.TYPE = "诊间锁定解锁";
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
        public bool DeleteByTran_ZZJ(Plat.Model.opt_pay_log model)
        {
            StringBuilder strSql = new StringBuilder();
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
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
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.STATES;
            parameters[2].Value = model.HOS_ID;
            parameters[3].Value = model.PAT_ID;
            parameters[4].Value = model.HSP_SN;
            parameters[5].Value = model.JEALL;
            parameters[6].Value = model.CASH_JE;
            parameters[7].Value = model.DJ_DATE;
            parameters[8].Value = model.DJ_TIME;
            parameters[9].Value = model.lTERMINAL_SN;

            table.Add(strSql.ToString(), parameters);


            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_mx_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters2[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters2);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_fl_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters3[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters3);

            strSql = new StringBuilder();
            strSql.Append("delete from opt_pay_lock ");
            strSql.Append(" where PAY_ID=@PAY_ID");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10)};
            parameters4[0].Value = model.PAY_ID;
            table.Add(strSql.ToString(), parameters4);

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
                    modSqlError.TYPE = "诊间锁定解锁";
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
        public bool AddByTran_ZZJ(Plat.Model.opt_pay_lock model, Plat.Model.opt_pay_fl_lock[] fl_lock, Plat.Model.opt_pay_mx_lock[] mx_lock, Plat.Model.opt_pay_log log)
        {
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_lock(");
            strSql.Append("PAY_ID,HOS_ID,PAT_ID,REGPAT_ID,USER_ID,HOS_SN,OPT_SN,HOS_REG_SN,PRE_NO,SFZ_NO,YLCARD_TYPE,YLCARD_NO,PAT_NAME,DEPT_CODE,DEPT_NAME,DOC_NO,DOC_NAME,DIS_NAME,PAY_lTERMINAL_SN,CASH_JE,PAY_TYPE,JEALL,JZ_CODE,ybDJH,GRZL,GRZF,TCZF,DBZF,XJZF,ZHZF,HM,CS,ZFY,YF,XMFY,LCL,ZHYE,XZM,XZMCH,man_type,BZFYY,FYLB,YBBZM,YBBZMC,DJ_DATE,DJ_TIME,IS_TZ,TZ_DATE,TZ_TIME,PAY_ID_IN,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@HOS_ID,@PAT_ID,@REGPAT_ID,@USER_ID,@HOS_SN,@OPT_SN,@HOS_REG_SN,@PRE_NO,@SFZ_NO,@YLCARD_TYPE,@YLCARD_NO,@PAT_NAME,@DEPT_CODE,@DEPT_NAME,@DOC_NO,@DOC_NAME,@DIS_NAME,@PAY_lTERMINAL_SN,@CASH_JE,@PAY_TYPE,@JEALL,@JZ_CODE,@ybDJH,@GRZL,@GRZF,@TCZF,@DBZF,@XJZF,@ZHZF,@HM,@CS,@ZFY,@YF,@XMFY,@LCL,@ZHYE,@XZM,@XZMCH,@man_type,@BZFYY,@FYLB,@YBBZM,@YBBZMC,@DJ_DATE,@DJ_TIME,@IS_TZ,@TZ_DATE,@TZ_TIME,@PAY_ID_IN,@lTERMINAL_SN)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
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
					new MySqlParameter("@DJ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@DJ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@IS_TZ", MySqlDbType.Bit),
					new MySqlParameter("@TZ_DATE", MySqlDbType.Datetime),
					new MySqlParameter("@TZ_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_ID_IN", MySqlDbType.VarChar,10),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30)};
            parameters[0].Value = model.PAY_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.REGPAT_ID;
            parameters[4].Value = model.USER_ID;
            parameters[5].Value = model.HOS_SN;
            parameters[6].Value = model.OPT_SN;
            parameters[7].Value = model.HOS_REG_SN == null ? "" : model.HOS_REG_SN;
            parameters[8].Value = model.PRE_NO;
            parameters[9].Value = model.SFZ_NO;
            parameters[10].Value = model.YLCARD_TYPE;
            parameters[11].Value = model.YLCARD_NO;
            parameters[12].Value = model.PAT_NAME;
            parameters[13].Value = model.DEPT_CODE;
            parameters[14].Value = model.DEPT_NAME;
            parameters[15].Value = model.DOC_NO;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME == null ? "" : model.DIS_NAME;
            parameters[18].Value = model.PAY_lTERMINAL_SN;
            parameters[19].Value = model.CASH_JE;
            parameters[20].Value = model.PAY_TYPE;
            parameters[21].Value = model.JEALL;
            parameters[22].Value = model.JZ_CODE == null ? "" : model.JZ_CODE;
            parameters[23].Value = model.ybDJH == null ? "" : model.ybDJH;
            parameters[24].Value = model.GRZL;
            parameters[25].Value = model.GRZF;
            parameters[26].Value = model.TCZF;
            parameters[27].Value = model.DBZF;
            parameters[28].Value = model.XJZF;
            parameters[29].Value = model.ZHZF;
            parameters[30].Value = model.HM;
            parameters[31].Value = model.CS;
            parameters[32].Value = model.ZFY;
            parameters[33].Value = model.YF;
            parameters[34].Value = model.XMFY;
            parameters[35].Value = model.LCL;
            parameters[36].Value = model.ZHYE;
            parameters[37].Value = model.XZM == null ? "" : model.XZM;
            parameters[38].Value = model.XZMCH == null ? "" : model.XZMCH;
            parameters[39].Value = model.man_type == null ? "" : model.man_type;
            parameters[40].Value = model.BZFYY == null ? "" : model.BZFYY;
            parameters[41].Value = model.FYLB == null ? "" : model.FYLB;
            parameters[42].Value = model.YBBZM == null ? "" : model.YBBZM;
            parameters[43].Value = model.YBBZMC == null ? "" : model.YBBZMC;
            parameters[44].Value = model.DJ_DATE;
            parameters[45].Value = model.DJ_TIME;
            parameters[46].Value = model.IS_TZ;
            parameters[47].Value = model.TZ_DATE;
            parameters[48].Value = model.TZ_TIME;
            parameters[49].Value = model.PAY_ID_IN;
            parameters[50].Value = model.lTERMINAL_SN;

            table.Add(strSql.ToString(), parameters);
            for (int i = 0; i < fl_lock.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_fl_lock(");
                strSql.Append("PAY_ID,FL_NO,FL_NAME,DEPT_CODE,DEPT_NAME,FL_JE,FL_ORDER)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@FL_NAME,@DEPT_CODE,@DEPT_NAME,@FL_JE,@FL_ORDER)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@FL_JE", MySqlDbType.Decimal,10),
					new MySqlParameter("@FL_ORDER", MySqlDbType.VarChar,10)};
                parameters1[0].Value = fl_lock[i].PAY_ID;
                parameters1[1].Value = fl_lock[i].FL_NO;
                parameters1[2].Value = fl_lock[i].FL_NAME;
                parameters1[3].Value = fl_lock[i].DEPT_CODE;
                parameters1[4].Value = fl_lock[i].DEPT_NAME;
                parameters1[5].Value = fl_lock[i].FL_JE;
                parameters1[6].Value = fl_lock[i].FL_ORDER;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);
            }
            for (int i = 0; i < mx_lock.Length; i++)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into opt_pay_mx_lock(");
                strSql.Append("PAY_ID,FL_NO,ITEM_TYPE,ITEM_ID,ITEM_NAME,ITEM_GG,COUNT,ITEM_UNIT,COST,je)");
                strSql.Append(" values (");
                strSql.Append("@PAY_ID,@FL_NO,@ITEM_TYPE,@ITEM_ID,@ITEM_NAME,@ITEM_GG,@COUNT,@ITEM_UNIT,@COST,@je)");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("@PAY_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@FL_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@ITEM_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_NAME", MySqlDbType.VarChar,30),
					new MySqlParameter("@ITEM_GG", MySqlDbType.VarChar,30),
					new MySqlParameter("@COUNT", MySqlDbType.VarChar,10),
					new MySqlParameter("@ITEM_UNIT", MySqlDbType.VarChar,10),
					new MySqlParameter("@COST", MySqlDbType.Decimal,10),
					new MySqlParameter("@je", MySqlDbType.Decimal,10)};
                parameters1[0].Value = mx_lock[i].PAY_ID;
                parameters1[1].Value = mx_lock[i].FL_NO;
                parameters1[2].Value = mx_lock[i].ITEM_TYPE;
                parameters1[3].Value = mx_lock[i].ITEM_ID;
                parameters1[4].Value = mx_lock[i].ITEM_NAME;
                parameters1[5].Value = mx_lock[i].ITEM_GG;
                parameters1[6].Value = mx_lock[i].COUNT;
                parameters1[7].Value = mx_lock[i].ITEM_UNIT;
                parameters1[8].Value = mx_lock[i].COST;
                parameters1[9].Value = mx_lock[i].je;

                int j = i;
                while (j-- > 0)
                {
                    strSql.Append(" ");
                }
                table.Add(strSql.ToString(), parameters1);

            }
            strSql = new StringBuilder();
            strSql.Append("insert into opt_pay_log(");
            strSql.Append("PAY_ID,STATES,HOS_ID,PAT_ID,HSP_SN,JEALL,CASH_JE,DJ_DATE,DJ_TIME,lTERMINAL_SN)");
            strSql.Append(" values (");
            strSql.Append("@PAY_ID,@STATES,@HOS_ID,@PAT_ID,@HSP_SN,@JEALL,@CASH_JE,@DJ_DATE,@DJ_TIME,@lTERMINAL_SN)");
            MySqlParameter[] parameters2 = {
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
            parameters2[0].Value = log.PAY_ID;
            parameters2[1].Value = log.STATES;
            parameters2[2].Value = log.HOS_ID;
            parameters2[3].Value = log.PAT_ID;
            parameters2[4].Value = log.HSP_SN;
            parameters2[5].Value = log.JEALL;
            parameters2[6].Value = log.CASH_JE;
            parameters2[7].Value = log.DJ_DATE;
            parameters2[8].Value = log.DJ_TIME;
            parameters2[9].Value = log.lTERMINAL_SN;


            table.Add(strSql.ToString(), parameters2);

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
                    modSqlError.TYPE = "诊间支付锁定";
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
        #endregion  ExtensionMethod
    }
}


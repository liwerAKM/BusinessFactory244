using System;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using System.Linq;
using DB.Core;
using Log.Core.Model;

namespace Plat.MySQLDAL
{
    /// <summary>
    /// 数据访问类:register_appt
    /// </summary>
    public partial class register_appt 
    {
        public register_appt()
        { }
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQLZZJ.GetMaxID("REG_ID", "register_appt");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int REG_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from register_appt");
            strSql.Append(" where REG_ID=@REG_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = REG_ID;

            return DbHelperMySQLZZJ.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Plat.Model.register_appt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_appt(");
            strSql.Append("REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,IN_PAY_STATE)");
            strSql.Append(" values (");
            strSql.Append("@REG_ID,@HOS_ID,@PAT_ID,@SCH_DATE,@SCH_TIME,@DEPT_CODE,@DOC_NO,@REGPAT_ID,@TIME_BUCKET,@TIME_POINT,@SFZ_NO,@PAT_NAME,@BIRTHDAY,@SEX,@YLCARD_TYPE,@YLCARD_NO,@DEPT_NAME,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_SN,@HOS_FH_TYPE,@ZL_FEE,@GH_FEE,@APPT_TYPE,@APPT_ORDER,@PAY_STATUS,@APPT_SN,@APPT_PAY,@IS_FZ,@APPT_TATE,@APPT_TIME,@PAY_EXPIRATION,@APPT_EXPIRATION,@lTERMINAL_SN,@APPT_WAY,@PERIOD_START,@IN_PAY_STATE)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@TIME_BUCKET", MySqlDbType.VarChar,30),
					new MySqlParameter("@TIME_POINT", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_FH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_STATUS", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_PAY", MySqlDbType.Decimal,6),
					new MySqlParameter("@IS_FZ", MySqlDbType.Bit),
					new MySqlParameter("@APPT_TATE", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_WAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
					new MySqlParameter("@IN_PAY_STATE", MySqlDbType.Bit)};
            parameters[0].Value = model.REG_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.SCH_DATE;
            parameters[4].Value = model.SCH_TIME;
            parameters[5].Value = model.DEPT_CODE;
            parameters[6].Value = model.DOC_NO;
            parameters[7].Value = model.REGPAT_ID;
            parameters[8].Value = model.TIME_BUCKET;
            parameters[9].Value = model.TIME_POINT;
            parameters[10].Value = model.SFZ_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.BIRTHDAY;
            parameters[13].Value = model.SEX;
            parameters[14].Value = model.YLCARD_TYPE;
            parameters[15].Value = model.YLCARD_NO;
            parameters[16].Value = model.DEPT_NAME;
            parameters[17].Value = model.DOC_NAME;
            parameters[18].Value = model.DIS_NAME;
            parameters[19].Value = model.GH_TYPE;
            parameters[20].Value = model.HOS_SN;
            parameters[21].Value = model.HOS_FH_TYPE;
            parameters[22].Value = model.ZL_FEE;
            parameters[23].Value = model.GH_FEE;
            parameters[24].Value = model.APPT_TYPE;
            parameters[25].Value = model.APPT_ORDER;
            parameters[26].Value = model.PAY_STATUS;
            parameters[27].Value = model.APPT_SN;
            parameters[28].Value = model.APPT_PAY;
            parameters[29].Value = model.IS_FZ;
            parameters[30].Value = model.APPT_TATE;
            parameters[31].Value = model.APPT_TIME;
            parameters[32].Value = model.PAY_EXPIRATION;
            parameters[33].Value = model.APPT_EXPIRATION;
            parameters[34].Value = model.lTERMINAL_SN;
            parameters[35].Value = model.APPT_WAY;
            parameters[36].Value = model.PERIOD_START;
            parameters[37].Value = model.IN_PAY_STATE;

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
        public bool Update(Plat.Model.register_appt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update register_appt set ");
            strSql.Append("HOS_ID=@HOS_ID,");
            strSql.Append("PAT_ID=@PAT_ID,");
            strSql.Append("SCH_DATE=@SCH_DATE,");
            strSql.Append("SCH_TIME=@SCH_TIME,");
            strSql.Append("DEPT_CODE=@DEPT_CODE,");
            strSql.Append("DOC_NO=@DOC_NO,");
            strSql.Append("REGPAT_ID=@REGPAT_ID,");
            strSql.Append("TIME_BUCKET=@TIME_BUCKET,");
            strSql.Append("TIME_POINT=@TIME_POINT,");
            strSql.Append("SFZ_NO=@SFZ_NO,");
            strSql.Append("PAT_NAME=@PAT_NAME,");
            strSql.Append("BIRTHDAY=@BIRTHDAY,");
            strSql.Append("SEX=@SEX,");
            strSql.Append("YLCARD_TYPE=@YLCARD_TYPE,");
            strSql.Append("YLCARD_NO=@YLCARD_NO,");
            strSql.Append("DEPT_NAME=@DEPT_NAME,");
            strSql.Append("DOC_NAME=@DOC_NAME,");
            strSql.Append("DIS_NAME=@DIS_NAME,");
            strSql.Append("GH_TYPE=@GH_TYPE,");
            strSql.Append("HOS_SN=@HOS_SN,");
            strSql.Append("HOS_FH_TYPE=@HOS_FH_TYPE,");
            strSql.Append("ZL_FEE=@ZL_FEE,");
            strSql.Append("GH_FEE=@GH_FEE,");
            strSql.Append("APPT_TYPE=@APPT_TYPE,");
            strSql.Append("APPT_ORDER=@APPT_ORDER,");
            strSql.Append("PAY_STATUS=@PAY_STATUS,");
            strSql.Append("APPT_SN=@APPT_SN,");
            strSql.Append("APPT_PAY=@APPT_PAY,");
            strSql.Append("IS_FZ=@IS_FZ,");
            strSql.Append("APPT_TATE=@APPT_TATE,");
            strSql.Append("APPT_TIME=@APPT_TIME,");
            strSql.Append("PAY_EXPIRATION=@PAY_EXPIRATION,");
            strSql.Append("APPT_EXPIRATION=@APPT_EXPIRATION,");
            strSql.Append("lTERMINAL_SN=@lTERMINAL_SN,");
            strSql.Append("APPT_WAY=@APPT_WAY,");
            strSql.Append("PERIOD_START=@PERIOD_START,");
            strSql.Append("IN_PAY_STATE=@IN_PAY_STATE");
            strSql.Append(" where REG_ID=@REG_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@TIME_BUCKET", MySqlDbType.VarChar,30),
					new MySqlParameter("@TIME_POINT", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_FH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_STATUS", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_PAY", MySqlDbType.Decimal,6),
					new MySqlParameter("@IS_FZ", MySqlDbType.Bit),
					new MySqlParameter("@APPT_TATE", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_WAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
					new MySqlParameter("@IN_PAY_STATE", MySqlDbType.Bit),
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)};
            parameters[0].Value = model.HOS_ID;
            parameters[1].Value = model.PAT_ID;
            parameters[2].Value = model.SCH_DATE;
            parameters[3].Value = model.SCH_TIME;
            parameters[4].Value = model.DEPT_CODE;
            parameters[5].Value = model.DOC_NO;
            parameters[6].Value = model.REGPAT_ID;
            parameters[7].Value = model.TIME_BUCKET;
            parameters[8].Value = model.TIME_POINT;
            parameters[9].Value = model.SFZ_NO;
            parameters[10].Value = model.PAT_NAME;
            parameters[11].Value = model.BIRTHDAY;
            parameters[12].Value = model.SEX;
            parameters[13].Value = model.YLCARD_TYPE;
            parameters[14].Value = model.YLCARD_NO;
            parameters[15].Value = model.DEPT_NAME;
            parameters[16].Value = model.DOC_NAME;
            parameters[17].Value = model.DIS_NAME;
            parameters[18].Value = model.GH_TYPE;
            parameters[19].Value = model.HOS_SN;
            parameters[20].Value = model.HOS_FH_TYPE;
            parameters[21].Value = model.ZL_FEE;
            parameters[22].Value = model.GH_FEE;
            parameters[23].Value = model.APPT_TYPE;
            parameters[24].Value = model.APPT_ORDER;
            parameters[25].Value = model.PAY_STATUS;
            parameters[26].Value = model.APPT_SN;
            parameters[27].Value = model.APPT_PAY;
            parameters[28].Value = model.IS_FZ;
            parameters[29].Value = model.APPT_TATE;
            parameters[30].Value = model.APPT_TIME;
            parameters[31].Value = model.PAY_EXPIRATION;
            parameters[32].Value = model.APPT_EXPIRATION;
            parameters[33].Value = model.lTERMINAL_SN;
            parameters[34].Value = model.APPT_WAY;
            parameters[35].Value = model.PERIOD_START;
            parameters[36].Value = model.IN_PAY_STATE;
            parameters[37].Value = model.REG_ID;

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
        public bool Delete(int REG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from register_appt ");
            strSql.Append(" where REG_ID=@REG_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = REG_ID;

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
        public bool DeleteList(string REG_IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from register_appt ");
            strSql.Append(" where REG_ID in (" + REG_IDlist + ")  ");
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
        public Plat.Model.register_appt GetModel(int REG_ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,IN_PAY_STATE,HOS_SN_HIS from register_appt ");
            strSql.Append(" where REG_ID=@REG_ID ");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11)			};
            parameters[0].Value = REG_ID;

            Plat.Model.register_appt model = new Plat.Model.register_appt();
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
        public Plat.Model.register_appt DataRowToModel(DataRow row)
        {
            Plat.Model.register_appt model = new Plat.Model.register_appt();
            if (row != null)
            {
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
                if (row["SCH_DATE"] != null)
                {
                    model.SCH_DATE = row["SCH_DATE"].ToString();
                }
                if (row["SCH_TIME"] != null)
                {
                    model.SCH_TIME = row["SCH_TIME"].ToString();
                }
                if (row["DEPT_CODE"] != null)
                {
                    model.DEPT_CODE = row["DEPT_CODE"].ToString();
                }
                if (row["DOC_NO"] != null)
                {
                    model.DOC_NO = row["DOC_NO"].ToString();
                }
                if (row["REGPAT_ID"] != null && row["REGPAT_ID"].ToString() != "")
                {
                    model.REGPAT_ID = int.Parse(row["REGPAT_ID"].ToString());
                }
                if (row["TIME_BUCKET"] != null)
                {
                    model.TIME_BUCKET = row["TIME_BUCKET"].ToString();
                }
                if (row["TIME_POINT"] != null)
                {
                    model.TIME_POINT = row["TIME_POINT"].ToString();
                }
                if (row["SFZ_NO"] != null)
                {
                    model.SFZ_NO = row["SFZ_NO"].ToString();
                }
                if (row["PAT_NAME"] != null)
                {
                    model.PAT_NAME = row["PAT_NAME"].ToString();
                }
                if (row["BIRTHDAY"] != null)
                {
                    model.BIRTHDAY = row["BIRTHDAY"].ToString();
                }
                if (row["SEX"] != null)
                {
                    model.SEX = row["SEX"].ToString();
                }
                if (row["YLCARD_TYPE"] != null && row["YLCARD_TYPE"].ToString() != "")
                {
                    model.YLCARD_TYPE = int.Parse(row["YLCARD_TYPE"].ToString());
                }
                if (row["YLCARD_NO"] != null)
                {
                    model.YLCARD_NO = row["YLCARD_NO"].ToString();
                }
                if (row["DEPT_NAME"] != null)
                {
                    model.DEPT_NAME = row["DEPT_NAME"].ToString();
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
                if (row["HOS_SN"] != null)
                {
                    model.HOS_SN = row["HOS_SN"].ToString();
                }
                if (row["HOS_FH_TYPE"] != null)
                {
                    model.HOS_FH_TYPE = row["HOS_FH_TYPE"].ToString();
                }
                if (row["ZL_FEE"] != null && row["ZL_FEE"].ToString() != "")
                {
                    model.ZL_FEE = decimal.Parse(row["ZL_FEE"].ToString());
                }
                if (row["GH_FEE"] != null && row["GH_FEE"].ToString() != "")
                {
                    model.GH_FEE = decimal.Parse(row["GH_FEE"].ToString());
                }
                if (row["APPT_TYPE"] != null)
                {
                    model.APPT_TYPE = row["APPT_TYPE"].ToString();
                }
                if (row["APPT_ORDER"] != null)
                {
                    model.APPT_ORDER = row["APPT_ORDER"].ToString();
                }
                if (row["PAY_STATUS"] != null)
                {
                    model.PAY_STATUS = row["PAY_STATUS"].ToString();
                }
                if (row["APPT_SN"] != null)
                {
                    model.APPT_SN = row["APPT_SN"].ToString();
                }
                if (row["APPT_PAY"] != null && row["APPT_PAY"].ToString() != "")
                {
                    model.APPT_PAY = decimal.Parse(row["APPT_PAY"].ToString());
                }
                if (row["IS_FZ"] != null && row["IS_FZ"].ToString() != "")
                {
                    if ((row["IS_FZ"].ToString() == "1") || (row["IS_FZ"].ToString().ToLower() == "true"))
                    {
                        model.IS_FZ = 1;
                    }
                    else
                    {
                        model.IS_FZ = 0;
                    }
                }
                if (row["APPT_TATE"] != null && row["APPT_TATE"].ToString() != "")
                {
                    model.APPT_TATE = DateTime.Parse(row["APPT_TATE"].ToString());
                }
                if (row["APPT_TIME"] != null)
                {
                    model.APPT_TIME = row["APPT_TIME"].ToString();
                }
                if (row["PAY_EXPIRATION"] != null && row["PAY_EXPIRATION"].ToString() != "")
                {
                    model.PAY_EXPIRATION = DateTime.Parse(row["PAY_EXPIRATION"].ToString());
                }
                if (row["APPT_EXPIRATION"] != null && row["APPT_EXPIRATION"].ToString() != "")
                {
                    model.APPT_EXPIRATION = DateTime.Parse(row["APPT_EXPIRATION"].ToString());
                }
                if (row["lTERMINAL_SN"] != null)
                {
                    model.lTERMINAL_SN = row["lTERMINAL_SN"].ToString();
                }
                if (row["APPT_WAY"] != null)
                {
                    model.APPT_WAY = row["APPT_WAY"].ToString();
                }
                if (row["PERIOD_START"] != null)
                {
                    model.PERIOD_START = row["PERIOD_START"].ToString();
                }
                if (row["IN_PAY_STATE"] != null && row["IN_PAY_STATE"].ToString() != "")
                {
                    if ((row["IN_PAY_STATE"].ToString() == "1") || (row["IN_PAY_STATE"].ToString().ToLower() == "true"))
                    {
                        model.IN_PAY_STATE = true;
                    }
                    else
                    {
                        model.IN_PAY_STATE = false;
                    }
                }
                if (row["HOS_SN_HIS"] != null)
                {
                    model.HOS_SN_HIS = row["HOS_SN_HIS"].ToString();
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
            strSql.Append("select REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,IN_PAY_STATE ");
            strSql.Append(" FROM register_appt ");
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
            strSql.Append("select count(1) FROM register_appt ");
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
                strSql.Append("order by T.REG_ID desc");
            }
            strSql.Append(")AS Row, T.*  from register_appt T ");
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
        public Plat.Model.register_appt GetModelByHOS_SNAndHOSID(string HOS_SN, string HOS_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,IN_PAY_STATE,HOS_SN_HIS from register_appt ");
            strSql.Append(" where HOS_SN=@HOS_SN and HOS_ID=@HOS_ID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20)};
            parameters[0].Value = HOS_SN;
            parameters[1].Value = HOS_ID;


            Plat.Model.register_appt model = new Plat.Model.register_appt();
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
        public Plat.Model.register_appt GetModelByAPPT_SNAndHOSID(string APPT_SN, string HOS_ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,IN_PAY_STATE from register_appt ");
            strSql.Append(" where APPT_SN=@APPT_SN and HOS_ID=@HOS_ID");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20)};
            parameters[0].Value = APPT_SN;
            parameters[1].Value = HOS_ID;


            Plat.Model.register_appt model = new Plat.Model.register_appt();
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


        public DataTable GetModelByIdS(int REGPAT_ID, int PAT_ID, string HOS_ID, string APPT_TYPE, int YY_TYPE, int PAGEINDEX, int PAGESIZE, ref int PAGECOUNT)
        {
            //为空不限制APPT_TYPE的值
            /*
0：预约未支付
1：预约已支付
2：预约已失效（预约未支付自动取消，不影响其他人预约）
3：预约已取消
4：预约已就诊
5：预约违约（违约为在规定时间内未支付 也未取消的预约信息，与2区别）
 */
            //            string sqlcmd = string.Format(@"SELECT a.YLCARD_NO,a.pat_name, a.appt_sn,a.hos_sn,a.hos_id,b.hos_name,a.dept_code,a.dept_name,a.doc_no,a.doc_name,a.sch_date,a.sch_time,a.gh_type as sch_type,
            //PERIOD_start, appt_pay,''as opt_sn,appt_type,appt_tate,appt_time FROM register_appt a,hospital  b
            //where a.HOS_ID=b.HOS_ID and a.REGPAT_ID={0} and a.PAT_ID={1} and a.HOS_ID='{2}' and a.APPT_TYPE='{3}'", REGPAT_ID, PAT_ID, HOS_ID,APPT_TYPE);

            //            if (APPT_TYPE == "1" || APPT_TYPE == "4")
            //            {
            //                sqlcmd = string.Format(@"SELECT a.YLCARD_NO,a.pat_name, a.appt_sn,a.hos_sn,a.hos_id,b.hos_name,a.dept_code,a.dept_name,a.doc_no,a.doc_name,a.sch_date,a.sch_time,a.gh_type as sch_type,
            //PERIOD_start, appt_pay,d.opt_sn,appt_type,appt_tate,appt_time FROM register_appt a,hospital  b,register_pay d
            //where a.HOS_ID=b.HOS_ID and a.reg_id=d.reg_id and d.is_th=0  and a.REGPAT_ID={0} and a.PAT_ID={1} and a.HOS_ID='{2}' and a.APPT_TYPE='{3}'", REGPAT_ID, PAT_ID, HOS_ID, APPT_TYPE);
            //            }

            string where = "";
            if (YY_TYPE == 1)
            {
                where = " and a.appt_tate<a.sch_date";
            }
            else
            {
                where = " and a.appt_tate=a.sch_date";
            }
            string sqlcmd = string.Format(@"SELECT IFNULL(pay_id,0)as pay_id, a.YLCARD_NO, a.pat_name,a.reg_id as appt_sn,a.hos_sn,a.hos_id,b.hos_name,a.dept_code,a.dept_name,a.doc_no,a.doc_name,a.sch_date,a.sch_time,a.gh_type as sch_type,
PERIOD_start, appt_pay,d.hos_sn as opt_sn,'' as  show_sn,(case when d.HOS_SN='ERROR' and appt_type=1 then '7' else appt_type end)appt_type,appt_tate,appt_time,a.APPT_ORDER,a.PAY_EXPIRATION FROM register_appt a LEFT JOIN hospital  b
ON a.hos_id=b.hos_id
left join register_pay d
on a.reg_id=d.reg_id
where a.REGPAT_ID={0} and a.PAT_ID={1} and a.HOS_ID='{2}' and a.appt_type='{3}' {4} order by appt_tate desc,appt_time desc ", REGPAT_ID, PAT_ID, HOS_ID, APPT_TYPE, where);
            if (APPT_TYPE.Trim() == "")
            {
                sqlcmd = string.Format(@"SELECT IFNULL(pay_id,0)as pay_id, a.YLCARD_NO, a.pat_name,a.reg_id as appt_sn,a.hos_sn,a.hos_id,b.hos_name,a.dept_code,a.dept_name,a.doc_no,a.doc_name,a.sch_date,a.sch_time,a.gh_type as sch_type,
PERIOD_start, appt_pay,d.hos_sn as opt_sn,'' as  show_sn,(case when d.HOS_SN='ERROR' and appt_type=1 then '7' else appt_type end)appt_type,appt_tate,appt_time,a.APPT_ORDER,a.PAY_EXPIRATION FROM register_appt a LEFT JOIN hospital  b
ON a.hos_id=b.hos_id
left join register_pay d
on a.reg_id=d.reg_id
where a.REGPAT_ID={0} and a.PAT_ID={1} and a.HOS_ID='{2}' {3} order by appt_tate desc,appt_time desc ", REGPAT_ID, PAT_ID, HOS_ID, where);
            }

            DataTable dt = DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                int max =Convert.ToInt32(dt.Compute("max(PAY_ID)", "appt_sn='" + dt.Rows[i]["appt_sn"].ToString().Trim() + "'"));
                if (dt.Rows[i]["PAY_ID"].ToString().Trim()!=max.ToString())
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            PAGECOUNT = dt.Rows.Count;
            PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
            return BaseFunction.GetPagedTable(dt, PAGEINDEX, PAGESIZE);

            //PAGECOUNT = BaseFunction.PAGECount(sqlcmd);
            //PAGECOUNT = (PAGECOUNT - 1) / PAGESIZE + 1;
            //sqlcmd += "limit " + (PAGESIZE * PAGEINDEX - PAGESIZE) + "," + (PAGESIZE * PAGEINDEX);
            //return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }

         /// <summary>
        /// 预约挂号保存
        /// </summary>
        /// <param name="appt"></param>
        /// <param name="schedule"></param>
        /// <param name="period"></param>
        /// <param name="old"></param>
        /// <param name="p_old"></param>
        /// <returns></returns>
        public bool AddByTran_ZZJ(Plat.Model.register_appt model)
        {
            //门诊预约挂号信息表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_appt(");
            strSql.Append("REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,USER_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,SOURCE,HOS_SN_HIS)");
            strSql.Append(" values (");
            strSql.Append("@REG_ID,@HOS_ID,@PAT_ID,@SCH_DATE,@SCH_TIME,@DEPT_CODE,@DOC_NO,@USER_ID,@TIME_BUCKET,@TIME_POINT,@SFZ_NO,@PAT_NAME,@BIRTHDAY,@SEX,@YLCARD_TYPE,@YLCARD_NO,@DEPT_NAME,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_SN,@HOS_FH_TYPE,@ZL_FEE,@GH_FEE,@APPT_TYPE,@APPT_ORDER,@PAY_STATUS,@APPT_SN,@APPT_PAY,@IS_FZ,@APPT_TATE,@APPT_TIME,@PAY_EXPIRATION,@APPT_EXPIRATION,@lTERMINAL_SN,@APPT_WAY,@PERIOD_START,@SOURCE,@HOS_SN_HIS)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@USER_ID", MySqlDbType.VarChar,10),
					new MySqlParameter("@TIME_BUCKET", MySqlDbType.VarChar,30),
					new MySqlParameter("@TIME_POINT", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_FH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_STATUS", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_PAY", MySqlDbType.Decimal,6),
					new MySqlParameter("@IS_FZ", MySqlDbType.Bit),
					new MySqlParameter("@APPT_TATE", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_WAY", MySqlDbType.VarChar,10),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@HOS_SN_HIS",MySqlDbType.VarChar,20)
                                          };
            parameters[0].Value = model.REG_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.SCH_DATE;
            parameters[4].Value = model.SCH_TIME;
            parameters[5].Value = model.DEPT_CODE;
            parameters[6].Value = model.DOC_NO;
            parameters[7].Value = model.USER_ID;
            parameters[8].Value = model.TIME_BUCKET;
            parameters[9].Value = model.TIME_POINT;
            parameters[10].Value = model.SFZ_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.BIRTHDAY;
            parameters[13].Value = model.SEX;
            parameters[14].Value = model.YLCARD_TYPE;
            parameters[15].Value = model.YLCARD_NO;
            parameters[16].Value = model.DEPT_NAME;
            parameters[17].Value = model.DOC_NAME;
            parameters[18].Value = model.DIS_NAME;
            parameters[19].Value = model.GH_TYPE;
            parameters[20].Value = model.HOS_SN;
            parameters[21].Value = model.HOS_FH_TYPE;
            parameters[22].Value = model.ZL_FEE;
            parameters[23].Value = model.GH_FEE;
            parameters[24].Value = model.APPT_TYPE;
            parameters[25].Value = model.APPT_ORDER;
            parameters[26].Value = model.PAY_STATUS;
            parameters[27].Value = model.APPT_SN;
            parameters[28].Value = model.APPT_PAY;
            parameters[29].Value = model.IS_FZ;
            parameters[30].Value = model.APPT_TATE;
            parameters[31].Value = model.APPT_TIME;
            parameters[32].Value = model.PAY_EXPIRATION;
            parameters[33].Value = model.APPT_EXPIRATION;
            parameters[34].Value = model.lTERMINAL_SN;
            parameters[35].Value = model.APPT_WAY;
            parameters[36].Value = model.PERIOD_START;
            parameters[37].Value = model.SOURCE;
            parameters[38].Value = model.HOS_SN_HIS;


            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql.ToString(), parameters);

            //排班表
            strSql = new StringBuilder();
            strSql.Append("update schedule set COUNT_REM=COUNT_REM-1 where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters1 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters1[0].Value = model.HOS_ID;
            parameters1[1].Value = model.DEPT_CODE;
            parameters1[2].Value = model.DOC_NO;
            parameters1[3].Value = model.SCH_DATE;
            parameters1[4].Value = model.SCH_TIME;
            table.Add(strSql.ToString(), parameters1);

            //排班时段表
            strSql = new StringBuilder();
            strSql.Append("update schedule_period set COUNT_YET=COUNT_YET+1  where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)};
            parameters2[0].Value = model.HOS_ID;
            parameters2[1].Value = model.DEPT_CODE;
            parameters2[2].Value = model.DOC_NO;
            parameters2[3].Value = model.SCH_DATE;
            parameters2[4].Value = model.SCH_TIME;
            parameters2[5].Value = model.PERIOD_START;
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
                    modSqlError.TYPE = "预约挂号保存";
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
        /// 预约挂号保存
        /// </summary>
        /// <param name="appt"></param>
        /// <param name="schedule"></param>
        /// <param name="period"></param>
        /// <param name="old"></param>
        /// <param name="p_old"></param>
        /// <returns></returns>
        public bool AddByTran(Plat.Model.register_appt model, Plat.Model.schedule_period period, Plat.Model.register_wait wait, Plat.Model.pat_card_bind bind)
        {
            //门诊预约挂号信息表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into register_appt(");
            strSql.Append("REG_ID,HOS_ID,PAT_ID,SCH_DATE,SCH_TIME,DEPT_CODE,DOC_NO,REGPAT_ID,TIME_BUCKET,TIME_POINT,SFZ_NO,PAT_NAME,BIRTHDAY,SEX,YLCARD_TYPE,YLCARD_NO,DEPT_NAME,DOC_NAME,DIS_NAME,GH_TYPE,HOS_SN,HOS_FH_TYPE,ZL_FEE,GH_FEE,APPT_TYPE,APPT_ORDER,PAY_STATUS,APPT_SN,APPT_PAY,IS_FZ,APPT_TATE,APPT_TIME,PAY_EXPIRATION,APPT_EXPIRATION,lTERMINAL_SN,APPT_WAY,PERIOD_START,SOURCE,HOS_SN_HIS)");
            strSql.Append(" values (");
            strSql.Append("@REG_ID,@HOS_ID,@PAT_ID,@SCH_DATE,@SCH_TIME,@DEPT_CODE,@DOC_NO,@REGPAT_ID,@TIME_BUCKET,@TIME_POINT,@SFZ_NO,@PAT_NAME,@BIRTHDAY,@SEX,@YLCARD_TYPE,@YLCARD_NO,@DEPT_NAME,@DOC_NAME,@DIS_NAME,@GH_TYPE,@HOS_SN,@HOS_FH_TYPE,@ZL_FEE,@GH_FEE,@APPT_TYPE,@APPT_ORDER,@PAY_STATUS,@APPT_SN,@APPT_PAY,@IS_FZ,@APPT_TATE,@APPT_TIME,@PAY_EXPIRATION,@APPT_EXPIRATION,@lTERMINAL_SN,@APPT_WAY,@PERIOD_START,@SOURCE,@HOS_SN_HIS)");
            MySqlParameter[] parameters = {
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@TIME_BUCKET", MySqlDbType.VarChar,30),
					new MySqlParameter("@TIME_POINT", MySqlDbType.VarChar,30),
					new MySqlParameter("@SFZ_NO", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@BIRTHDAY", MySqlDbType.VarChar,10),
					new MySqlParameter("@SEX", MySqlDbType.VarChar,6),
					new MySqlParameter("@YLCARD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@DEPT_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DOC_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@DIS_NAME", MySqlDbType.VarChar,20),
					new MySqlParameter("@GH_TYPE", MySqlDbType.VarChar,2),
					new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@HOS_FH_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@ZL_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@GH_FEE", MySqlDbType.Decimal,10),
					new MySqlParameter("@APPT_TYPE", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_ORDER", MySqlDbType.VarChar,10),
					new MySqlParameter("@PAY_STATUS", MySqlDbType.VarChar,1),
					new MySqlParameter("@APPT_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_PAY", MySqlDbType.Decimal,6),
					new MySqlParameter("@IS_FZ", MySqlDbType.Bit),
					new MySqlParameter("@APPT_TATE", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_TIME", MySqlDbType.VarChar,8),
					new MySqlParameter("@PAY_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@APPT_EXPIRATION", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@APPT_WAY", MySqlDbType.VarChar,10),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10),
                    new MySqlParameter("@SOURCE", MySqlDbType.VarChar,10),
                    new MySqlParameter("@HOS_SN_HIS",MySqlDbType.VarChar,20) 
                                          };
            parameters[0].Value = model.REG_ID;
            parameters[1].Value = model.HOS_ID;
            parameters[2].Value = model.PAT_ID;
            parameters[3].Value = model.SCH_DATE;
            parameters[4].Value = model.SCH_TIME;
            parameters[5].Value = model.DEPT_CODE;
            parameters[6].Value = model.DOC_NO;
            parameters[7].Value = model.REGPAT_ID;
            parameters[8].Value = model.TIME_BUCKET;
            parameters[9].Value = model.TIME_POINT;
            parameters[10].Value = model.SFZ_NO;
            parameters[11].Value = model.PAT_NAME;
            parameters[12].Value = model.BIRTHDAY;
            parameters[13].Value = model.SEX;
            parameters[14].Value = model.YLCARD_TYPE;
            parameters[15].Value = model.YLCARD_NO;
            parameters[16].Value = model.DEPT_NAME;
            parameters[17].Value = model.DOC_NAME;
            parameters[18].Value = model.DIS_NAME;
            parameters[19].Value = model.GH_TYPE;
            parameters[20].Value = model.HOS_SN;
            parameters[21].Value = model.HOS_FH_TYPE;
            parameters[22].Value = model.ZL_FEE;
            parameters[23].Value = model.GH_FEE;
            parameters[24].Value = model.APPT_TYPE;
            parameters[25].Value = model.APPT_ORDER;
            parameters[26].Value = model.PAY_STATUS;
            parameters[27].Value = model.APPT_SN;
            parameters[28].Value = model.APPT_PAY;
            parameters[29].Value = model.IS_FZ;
            parameters[30].Value = model.APPT_TATE;
            parameters[31].Value = model.APPT_TIME;
            parameters[32].Value = model.PAY_EXPIRATION;
            parameters[33].Value = model.APPT_EXPIRATION;
            parameters[34].Value = model.lTERMINAL_SN;
            parameters[35].Value = model.APPT_WAY;
            parameters[36].Value = model.PERIOD_START;
            parameters[37].Value = model.SOURCE;
            parameters[38].Value = model.HOS_SN_HIS;

            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql.ToString(), parameters);

            //排班表
            strSql = new StringBuilder();
            strSql.Append("update schedule set COUNT_REM=COUNT_REM-1 where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters1 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters1[0].Value = period.HOS_ID;
            parameters1[1].Value = period.DEPT_CODE;
            parameters1[2].Value = period.DOC_NO;
            parameters1[3].Value = period.SCH_DATE;
            parameters1[4].Value = period.SCH_TIME;
            table.Add(strSql.ToString(), parameters1);

            //排班时段表
            strSql = new StringBuilder();
            strSql.Append("update schedule_period set COUNT_YET=COUNT_YET+1  where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START");
            MySqlParameter[] parameters2 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)};
            parameters2[0].Value = period.HOS_ID;
            parameters2[1].Value = period.DEPT_CODE;
            parameters2[2].Value = period.DOC_NO;
            parameters2[3].Value = period.SCH_DATE;
            parameters2[4].Value = period.SCH_TIME;
            parameters2[5].Value = period.PERIOD_START;
            table.Add(strSql.ToString(), parameters2);

            // 预约等待表更新
            if (wait != null)
            {
                strSql = new StringBuilder();
                strSql.Append("update register_wait set ");
                strSql.Append("IS_APPT=@IS_APPT,");
                strSql.Append("REG_ID=@REG_ID,");
                strSql.Append("APP_TIME=@APP_TIME,");
                strSql.Append("lTERMINAL_SN=@lTERMINAL_SN");
                strSql.Append(" where WAIT_ID=@WAIT_ID ");

                MySqlParameter[] parameters3 = {
					new MySqlParameter("@IS_APPT", MySqlDbType.Bit),
					new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@APP_TIME", MySqlDbType.DateTime),
					new MySqlParameter("@lTERMINAL_SN", MySqlDbType.VarChar,30),
					new MySqlParameter("@WAIT_ID", MySqlDbType.VarChar,10)};

                parameters3[0].Value = wait.IS_APPT;
                parameters3[1].Value = wait.REG_ID;
                parameters3[2].Value = wait.APP_TIME;
                parameters3[3].Value = wait.lTERMINAL_SN;
                parameters3[4].Value = wait.WAIT_ID;


                table.Add(strSql.ToString(), parameters3);
            }
            //病人和医疗卡绑定
            if (bind != null)
            {
                strSql = new StringBuilder();
                strSql.Append("insert into pat_card_bind(");
                strSql.Append("HOS_ID,PAT_ID,YLCARTD_TYPE,YLCARD_NO,MARK_BIND,BAND_TIME)");
                strSql.Append(" values (");
                strSql.Append("@HOS_ID,@PAT_ID,@YLCARTD_TYPE,@YLCARD_NO,@MARK_BIND,@BAND_TIME)");
                MySqlParameter[] parameters4 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@YLCARTD_TYPE", MySqlDbType.Int16,4),
					new MySqlParameter("@YLCARD_NO", MySqlDbType.VarChar,30),
					new MySqlParameter("@MARK_BIND", MySqlDbType.Int16,4),
					new MySqlParameter("@BAND_TIME", MySqlDbType.DateTime)};
                parameters4[0].Value = bind.HOS_ID;
                parameters4[1].Value = bind.PAT_ID;
                parameters4[2].Value = bind.YLCARTD_TYPE;
                parameters4[3].Value = bind.YLCARD_NO;
                parameters4[4].Value = bind.MARK_BIND;
                parameters4[5].Value = bind.BAND_TIME;

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
                    modSqlError.TYPE = "预约挂号保存";
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
        public DataTable GetInfoByIndex(int PAGEINDEX, int PAGESIZE)
        {
            //string sqlcmd = string.Format(@"select * from register_appt limit {0},{1}", PAGESIZE * PAGEINDEX - PAGESIZE + 1, PAGESIZE * PAGEINDEX);
            string sqlcmd = string.Format(@"select * from register_appt limit {0},{1}", PAGESIZE * PAGEINDEX - PAGESIZE + 1, PAGESIZE);
            return DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
        }
        /// <summary>
        /// 获取有效数量提示信息
        /// </summary>
        /// <param name="TYPE"></param>
        /// <returns></returns>
        public bool GetYYGHNUM(int TYPE, int REGPAT_ID, string HOS_ID, ref int NUM_YYGH, ref int NUM_YY, ref int NUM_GH, ref int NUM_CARD)
        {
            string sqlcmd = "";
            DataTable dtReturn = new DataTable();
            if (TYPE == 1)//主界面
            {
                sqlcmd = string.Format(@"select count(1) from register_appt where appt_type in(0,1) and REGPAT_ID={0} and HOS_ID='{1}' and sch_date>=curdate()",REGPAT_ID,HOS_ID);
                DataTable dt = DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
                NUM_YYGH = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0][0]);

                sqlcmd = string.Format(@"select count(a.YLCARD_NO) from pat_card a,regtopat b where a.pat_id=b.pat_id and a.mark_del=0 and b.regpat_id={0}", REGPAT_ID);
                dt = DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
                NUM_CARD = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0][0]);
            }
            else if (TYPE == 2)
            {
                sqlcmd = string.Format(@"select count(1) from register_appt where appt_type in(0,1) and REGPAT_ID={0} and HOS_ID='{1}' and sch_date>appt_tate and sch_date>=curdate()", REGPAT_ID, HOS_ID);
                DataTable dt = DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
                NUM_YY = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0][0]);

                sqlcmd = string.Format(@"select count(1) from register_appt where appt_type in(0,1) and REGPAT_ID={0} and HOS_ID='{1}' and sch_date=appt_tate and sch_date>=curdate()", REGPAT_ID, HOS_ID);
                dt = DbHelperMySQLZZJ.Query(sqlcmd).Tables[0];
                NUM_GH = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Rows[0][0]);

            }
            else
            {
                NUM_YYGH = 0;
                NUM_YY = 0;
                NUM_GH = 0;
                NUM_CARD = 0;
            }
            return true;
        }
        /// <summary>
        /// 预约取消 不含支付
        /// </summary>
        /// <param name="appt"></param>
        /// <returns></returns>
        public bool YYCancel(Plat.Model.register_appt appt)
        {
            string APPT_TYPE = appt.APPT_TYPE;
            //预约表
            StringBuilder strSql = new StringBuilder();
            if (APPT_TYPE == "0")
            {
                strSql.Append("update register_appt set PAY_STATUS=0,APPT_TYPE=5 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            }
            else
            {
                strSql.Append("update register_appt set PAY_STATUS=0,APPT_TYPE=3 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            }
            MySqlParameter[] parameters = {
            new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
            new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30)};

            parameters[0].Value = appt.HOS_ID;
            parameters[1].Value = appt.HOS_SN;
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql.ToString(), parameters);

            if (APPT_TYPE == "1")
            {
                strSql = new StringBuilder();
                strSql.Append("update register_pay set IS_TH=1,RH_DATE=@RH_DATE,TH_TIME=@TH_TIME where HOS_ID=@HOS_ID and REG_ID=@REG_ID and PAT_ID=@PAT_ID and REGPAT_ID=@REGPAT_ID");
                MySqlParameter[] parameters6 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
                    new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
                   	new MySqlParameter("@RH_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8)};
                parameters6[0].Value = appt.HOS_ID;
                parameters6[1].Value = appt.REG_ID;
                parameters6[2].Value = appt.PAT_ID;
                parameters6[3].Value = appt.REGPAT_ID;
                parameters6[4].Value = DateTime.Now.ToString("yyyy-MM-dd");
                parameters6[5].Value = DateTime.Now.ToString("HH:mm:ss");
                table.Add(strSql.ToString(), parameters6);
            }


            //排班表
            strSql = new StringBuilder();
            strSql.Append("update schedule set COUNT_REM=COUNT_REM+1 where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters3[0].Value = appt.HOS_ID;
            parameters3[1].Value = appt.DEPT_CODE;
            parameters3[2].Value = appt.DOC_NO;
            parameters3[3].Value = appt.SCH_DATE;
            parameters3[4].Value = appt.SCH_TIME;
            table.Add(strSql.ToString(), parameters3);

            //排班时段表
            strSql = new StringBuilder();
            strSql.Append("update schedule_period set COUNT_YET=COUNT_YET-1  where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)};
            parameters4[0].Value = appt.HOS_ID;
            parameters4[1].Value = appt.DEPT_CODE;
            parameters4[2].Value = appt.DOC_NO;
            parameters4[3].Value = appt.SCH_DATE;
            parameters4[4].Value = appt.SCH_TIME;
            parameters4[5].Value = appt.PERIOD_START;
            table.Add(strSql.ToString(), parameters4);


            try
            {
                DbHelperMySQLZZJ.ExecuteSqlTran(table);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool YYCancel(Plat.Model.register_appt appt, Plat.Model.pay_info info, Plat.Model.pay_info_zfb zfb, Model.pay_info_wc wc, Model.pay_info_bank bank, Plat.Model.pay_info_upcap upcap, Plat.Model.unionpay_tran tran,Plat.Model.alipay_tran alitran,Plat.Model.pay_info_ccb ccb)
        {
            string APPT_TYPE = appt.APPT_TYPE;
            //预约表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update register_appt set PAY_STATUS=0,APPT_TYPE=3 where HOS_ID=@HOS_ID and HOS_SN=@HOS_SN");
            MySqlParameter[] parameters = {
            new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
            new MySqlParameter("@HOS_SN", MySqlDbType.VarChar,30)};

            parameters[0].Value = appt.HOS_ID;
            parameters[1].Value = appt.HOS_SN;
            System.Collections.Hashtable table = new System.Collections.Hashtable();
            table.Add(strSql.ToString(), parameters);
            //预约支付

            if (APPT_TYPE == "1")
            {
                strSql = new StringBuilder();
                strSql.Append("update register_pay set IS_TH=1,RH_DATE=@RH_DATE,TH_TIME=@TH_TIME where HOS_ID=@HOS_ID and REG_ID=@REG_ID and PAT_ID=@PAT_ID and REGPAT_ID=@REGPAT_ID");
                MySqlParameter[] parameters6 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@REG_ID", MySqlDbType.Int32,11),
					new MySqlParameter("@PAT_ID", MySqlDbType.Int32,11),
                    new MySqlParameter("@REGPAT_ID", MySqlDbType.Int32,11),
                   	new MySqlParameter("@RH_DATE", MySqlDbType.DateTime),
					new MySqlParameter("@TH_TIME", MySqlDbType.VarChar,8)};
                parameters6[0].Value = appt.HOS_ID;
                parameters6[1].Value = appt.REG_ID;
                parameters6[2].Value = appt.PAT_ID;
                parameters6[3].Value = appt.REGPAT_ID;
                parameters6[4].Value = info.DJ_DATE;
                parameters6[5].Value = info.DJ_TIME;
                table.Add(strSql.ToString(), parameters6);
            }


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


                strSql = new StringBuilder();
                strSql.Append("update pay_info_zfb set ");
                strSql.Append("TXN_TYPE=@TXN_TYPE");
                strSql.Append(" where COMM_SN=@COMM_SN and TXN_TYPE='05'");
                MySqlParameter[] parameters10 = {
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30)};

                parameters10[0].Value = "01";
                parameters10[1].Value = zfb.COMM_SN;

                table.Add(strSql, parameters10);


                strSql = new StringBuilder();
                strSql.Append("update alipay_tran set ");
                strSql.Append("TXN_TYPE=@TXN_TYPE");
                strSql.Append(" where COMM_SN=@COMM_SN and TXN_TYPE='05'");
                MySqlParameter[] parameters11 = {
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,10),
					new MySqlParameter("@COMM_SN", MySqlDbType.VarChar,30)};

                parameters11[0].Value = "01";
                parameters11[1].Value = zfb.COMM_SN;

                table.Add(strSql, parameters11);
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
                MySqlParameter[] parameters9 = {
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
                parameters9[0].Value = ccb.PAY_ID;
                parameters9[1].Value = ccb.BIZ_TYPE;
                parameters9[2].Value = ccb.BDj_id;
                parameters9[3].Value = ccb.JE;
                parameters9[4].Value = ccb.POSID;
                parameters9[5].Value = ccb.BRANCHID;
                parameters9[6].Value = ccb.ORDERID;
                parameters9[7].Value = ccb.SFZ_NO;
                parameters9[8].Value = ccb.DJ_TIME;
                parameters9[9].Value = ccb.TXN_TYPE;
                parameters9[10].Value = ccb.MerchantID;

                table.Add(strSql, parameters9);

                strSql = new StringBuilder();
                strSql.Append("update pay_info_ccb set ");
                strSql.Append("TXN_TYPE=@TXN_TYPE");
                strSql.Append(" where ORDERID=@ORDERID and TXN_TYPE='05'");
                MySqlParameter[] parameters10 = {
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)};

                parameters10[0].Value = "01";
                parameters10[1].Value = ccb.ORDERID;

                table.Add(strSql, parameters10);

                strSql = new StringBuilder();
                strSql.Append("update ccbpay_tran set ");
                strSql.Append("TXN_TYPE=@TXN_TYPE");
                strSql.Append(" where ORDERID=@ORDERID and TXN_TYPE='05'");
                MySqlParameter[] parameters11 = {
                    new MySqlParameter("@TXN_TYPE", MySqlDbType.VarChar,30),
					new MySqlParameter("@ORDERID", MySqlDbType.VarChar,30)};

                parameters11[0].Value = "01";
                parameters11[1].Value = ccb.ORDERID;

                table.Add(strSql, parameters11);
            }
            //排班表
            strSql = new StringBuilder();
            strSql.Append("update schedule set COUNT_REM=COUNT_REM+1 where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME ");
            MySqlParameter[] parameters3 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8)			};
            parameters3[0].Value = appt.HOS_ID;
            parameters3[1].Value = appt.DEPT_CODE;
            parameters3[2].Value = appt.DOC_NO;
            parameters3[3].Value = appt.SCH_DATE;
            parameters3[4].Value = appt.SCH_TIME;
            table.Add(strSql.ToString(), parameters3);

            //排班时段表
            strSql = new StringBuilder();
            strSql.Append("update schedule_period set COUNT_YET=COUNT_YET-1  where HOS_ID=@HOS_ID and DEPT_CODE=@DEPT_CODE and DOC_NO=@DOC_NO and SCH_DATE=@SCH_DATE and SCH_TIME=@SCH_TIME and PERIOD_START=@PERIOD_START");
            MySqlParameter[] parameters4 = {
					new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,20),
					new MySqlParameter("@DEPT_CODE", MySqlDbType.VarChar,10),
					new MySqlParameter("@DOC_NO", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_DATE", MySqlDbType.VarChar,10),
					new MySqlParameter("@SCH_TIME", MySqlDbType.VarChar,8),
                    new MySqlParameter("@PERIOD_START", MySqlDbType.VarChar,10)};
            parameters4[0].Value = appt.HOS_ID;
            parameters4[1].Value = appt.DEPT_CODE;
            parameters4[2].Value = appt.DOC_NO;
            parameters4[3].Value = appt.SCH_DATE;
            parameters4[4].Value = appt.SCH_TIME;
            parameters4[5].Value = appt.PERIOD_START;
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
                    modSqlError.TYPE = "预约支付取消";
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


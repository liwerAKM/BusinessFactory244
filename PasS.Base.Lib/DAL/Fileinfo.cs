
using System.Text;
using PasS.Base.Lib.Model;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System;

namespace PasS.Base.Lib.DAL
{
    /// <summary>
    /// 数据访问类:fileinfo
    /// </summary>
    public partial class Fileinfo
    {
        public Fileinfo()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from fileinfo");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)          };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;
            return DbHelperMySQLMySpring.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否被使用 如果被使用则不允许删除
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="FilePath"></param>
        /// <param name="Filename"></param>
        /// <param name="Version"></param>
        /// <returns></returns>
        public bool IsUsed(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from businessinfo");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and DllName=@Filename and Version=@Version ;");
            strSql.Append("select count(1) from BusinessInfoVersion");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and DllName=@Filename and Version=@Version ;");
            strSql.Append("select count(1) from dependentfile");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ;");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)          };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return (int.Parse(ds.Tables[0].Rows[0][0].ToString()) + int.Parse(ds.Tables[1].Rows[0][0].ToString()) + int.Parse(ds.Tables[2].Rows[0][0].ToString())) > 0;
        }

        public int SaveFileImage(string MD5, Byte[] images)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ImageID from Fileimage where md5=@MD5");
            MySqlParameter[] parameters0 = {
                    new MySqlParameter("@MD5", MySqlDbType.VarChar,32) };
            parameters0[0].Value = MD5;
            object obj = DbHelperMySQLMySpring.GetSingle(strSql.ToString(), parameters0);
            if (obj == null)
            {
                strSql = new StringBuilder();
                int ImageID = 0;
                if (DbHelper.GetSysIdBase("Fileimage", out ImageID))
                {
                    strSql.Append("insert into  Fileimage (ImageID,MD5,images ,AddTime) values (@ImageID,@MD5,@images ,@AddTime) ");
                    MySqlParameter[] parameters = {
                    new MySqlParameter("@ImageID", MySqlDbType.Int32),
                      new MySqlParameter("@MD5",  MySqlDbType.VarChar,32),
                  new MySqlParameter("@images", MySqlDbType.LongBlob),
                    new MySqlParameter("@AddTime", MySqlDbType.DateTime) };
                    parameters[0].Value = ImageID;
                    parameters[1].Value = MD5;
                    parameters[2].Value = images;
                    parameters[3].Value = DateTime.Now;
                    int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
                    if (rows > 0)
                    {
                        return ImageID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return (int)obj;
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(fileinfo model)
        {
            model.ImageID = SaveFileImage(model.MD5, model.images);
             StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into fileinfo(");
            strSql.Append("projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,Note,CompanyName,FileSize,ComMinVersion,ComMaxVersion,cTime,MTime,upTime,images,ImageID,MD5)");
            strSql.Append(" values (");
            strSql.Append("@projectID,@FilePath,@Filename,@Version,@isdirect,@Extension,@FileDescription,@Note,@CompanyName,@FileSize,@ComMinVersion,@ComMaxVersion,@cTime,@MTime,@upTime,@images,@ImageID,@MD5)");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@isdirect", MySqlDbType.Bit),
                    new MySqlParameter("@Extension", MySqlDbType.VarChar,10),
                    new MySqlParameter("@FileDescription", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Note", MySqlDbType.VarChar,100),
                    new MySqlParameter("@CompanyName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@FileSize", MySqlDbType.VarChar,100),
                    new MySqlParameter("@ComMinVersion", MySqlDbType.VarChar,23),
                    new MySqlParameter("@ComMaxVersion", MySqlDbType.VarChar,23),
                    new MySqlParameter("@cTime", MySqlDbType.DateTime),
                    new MySqlParameter("@MTime", MySqlDbType.DateTime),
                    new MySqlParameter("@upTime", MySqlDbType.DateTime),
                    new MySqlParameter("@images", MySqlDbType.LongBlob),
              new MySqlParameter("@ImageID", MySqlDbType.Int32),
              new MySqlParameter("@MD5",  MySqlDbType.VarChar,32),};
            parameters[0].Value = model.projectID;
            parameters[1].Value = model.FilePath;
            parameters[2].Value = model.Filename;
            parameters[3].Value = model.Version;
            parameters[4].Value = model.isdirect;
            parameters[5].Value = model.Extension;
            parameters[6].Value = model.FileDescription;
            parameters[7].Value = model.Note;
            parameters[8].Value = model.CompanyName;
            parameters[9].Value = model.FileSize;
            parameters[10].Value = model.ComMinVersion;
            parameters[11].Value = model.ComMaxVersion;
            parameters[12].Value = model.cTime;
            parameters[13].Value = model.MTime;
            parameters[14].Value = model.upTime;
           // parameters[15].Value = model.images;
            parameters[16].Value = model.ImageID;
            parameters[17].Value = model.MD5;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
		public bool Update(fileinfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update fileinfo set ");
            strSql.Append("isdirect=@isdirect,");
            strSql.Append("Extension=@Extension,");
            strSql.Append("FileDescription=@FileDescription,");
            strSql.Append("Note=@Note,");
            strSql.Append("CompanyName=@CompanyName,");
            strSql.Append("FileSize=@FileSize,");
            strSql.Append("ComMinVersion=@ComMinVersion,");
            strSql.Append("ComMaxVersion=@ComMaxVersion,");
            strSql.Append("cTime=@cTime,");
            strSql.Append("MTime=@MTime,");
            strSql.Append("upTime=@upTime,");
            strSql.Append("images=@images");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@isdirect", MySqlDbType.Bit),
                    new MySqlParameter("@Extension", MySqlDbType.VarChar,10),
                    new MySqlParameter("@FileDescription", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Note", MySqlDbType.VarChar,100),
                    new MySqlParameter("@CompanyName", MySqlDbType.VarChar,100),
                    new MySqlParameter("@FileSize", MySqlDbType.VarChar,100),
                    new MySqlParameter("@ComMinVersion", MySqlDbType.VarChar,23),
                    new MySqlParameter("@ComMaxVersion", MySqlDbType.VarChar,23),
                    new MySqlParameter("@cTime", MySqlDbType.DateTime),
                    new MySqlParameter("@MTime", MySqlDbType.DateTime),
                    new MySqlParameter("@upTime", MySqlDbType.DateTime),
                    new MySqlParameter("@images", MySqlDbType.LongBlob),
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)};
            parameters[0].Value = model.isdirect;
            parameters[1].Value = model.Extension;
            parameters[2].Value = model.FileDescription;
            parameters[3].Value = model.Note;
            parameters[4].Value = model.CompanyName;
            parameters[5].Value = model.FileSize;
            parameters[6].Value = model.ComMinVersion;
            parameters[7].Value = model.ComMaxVersion;
            parameters[8].Value = model.cTime;
            parameters[9].Value = model.MTime;
            parameters[10].Value = model.upTime;
            parameters[11].Value = model.images;
            parameters[12].Value = model.projectID;
            parameters[13].Value = model.FilePath;
            parameters[14].Value = model.Filename;
            parameters[15].Value = model.Version;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }

            StringBuilder strSql = new StringBuilder();
            //strSql.Append(" select imageid into @imageid  from fileinfo where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version;");
            strSql.Append(" select imageid  from fileinfo where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version;");


            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20),
                    new MySqlParameter("@imageid", MySqlDbType.Int32)};
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;
            parameters[4].Value = 0;

            object obj = DbHelperMySQLMySpring.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return false;
            }

            int imageid = (int)obj;
            strSql = new StringBuilder();

            strSql.Append("delete from fileinfo ");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ;");

            strSql.Append("delete from fileimage where imageid = @imageid and not  EXISTS(select * from fileinfo where imageid = @imageid );");

            parameters[4].Value = imageid;

            //MySqlParameter[] parameters = {
            //        new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
            //        new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
            //        new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
            //        new MySqlParameter("@Version", MySqlDbType.VarChar,20),
            //        new MySqlParameter("@imageid", MySqlDbType.Int32) };
            //parameters[0].Value = projectID;
            //parameters[1].Value = FilePath;
            //parameters[2].Value = Filename;
            //parameters[3].Value = Version;

            int rows = DbHelperMySQLMySpring.ExecuteSql(strSql.ToString(), parameters);
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
        /// 获取直接提供业务服务的DLL
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetFileInfo(string projectID)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' ,
projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID   ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20) }
                   ;
            parameters[0].Value = projectID;


            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取直接提供业务服务的DLL
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public DataTable GetDirect(string projectID)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' ,
projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID    and isdirect=1");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20) }
                   ;
            parameters[0].Value = projectID;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取直接提供业务服务的DLL
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="DefFilename"></param>
        /// <returns></returns>
        public DataTable GetDirect(string projectID, string DefFilename)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";

            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select * from (
select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' ,
projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note,ComMinVersion,ComMaxVersion,0 as 'Ord'
from fileinfo 
where projectID=@projectID    and isdirect=1 and Filename =@DefFilename
ORDER BY 
Filename, MTime desc)
as aa
union 
select * from (
select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' ,
projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime ,Note,ComMinVersion,ComMaxVersion,1 as 'Ord'
from fileinfo 
where projectID=@projectID    and isdirect=1 and Filename != @DefFilename
ORDER BY 
Filename, MTime desc)as bb
ORDER BY Ord,Filename, MTime DESC
 ");

            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
            new MySqlParameter("@DefFilename", MySqlDbType.VarChar,50)}
                   ;
            parameters[0].Value = projectID;
            parameters[1].Value = DefFilename;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }

        public DataTable GetTable(string projectID)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' ,projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime ,Note,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID   ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20) }
                   ;
            parameters[0].Value = projectID;


            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }
        public DataTable GetTablewhere(string where)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CONCAT( 'ProjectID:',a.projectID, ',Path:',a.FilePath,',Name:',a.Filename,',Version:',a.Version) as'FileID' ,a.projectID,a.FilePath,a.Filename,a.Version,
a.isdirect, a.Extension, a.FileDescription, a.CompanyName, a.FileSize, a.cTime, a.MTime, a.upTime, a.Note,a.ComMinVersion,a.ComMaxVersion
 from fileinfo a");
            if (!string.IsNullOrWhiteSpace(where))
            {
                strSql.Append(" where   " + where);
            }

            strSql.Append("  order by upTime asc   ");



            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 获取BusServer需要更新文件
        /// </summary>
        /// <param name="lastupTime"></param>
        /// <returns></returns>
        public DataTable GetTbNewSysBusSFile(DateTime lastupTime)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CONCAT( 'ProjectID:',a.projectID, ',Path:',a.FilePath,',Name:',a.Filename,',Version:',a.Version) as'FileID' ,a.projectID,a.FilePath,a.Filename,a.Version,
a.isdirect, a.Extension, a.FileDescription, a.CompanyName, a.FileSize, a.cTime, a.MTime, a.upTime, a.Note ,a.ComMinVersion,a.ComMaxVersion
 from fileinfo a");

            strSql.Append(" where a.projectID ='SysBusSFile' and  a.upTime >'" + lastupTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            return ds.Tables[0];
        }

        public DataTable GetTableOnlyBusSwhere(string where, string BusServerID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"select CONCAT( 'ProjectID:',a.projectID, ',Path:',a.FilePath,',Name:',a.Filename,',Version:',a.Version) as'FileID' ,a.projectID,a.FilePath,a.Filename,a.Version,
a.isdirect, a.Extension, a.FileDescription, a.CompanyName, a.FileSize, a.cTime, a.MTime, a.upTime, a.Note,a.ComMinVersion,a.ComMaxVersion
 from fileinfo a
join businessinfo  b on  b.projectID=a.projectID and b.FilePath=a.FilePath and b.DllName =a.Filename  and b.Version=a.Version
join busserverbusinfo c on b.BusID=c.BusID and c.BusServerID='{0}'", BusServerID));
            if (!string.IsNullOrWhiteSpace(where))
            {
                strSql.Append(" where   " + where);
            }

            strSql.Append("  order by a.upTime asc   ");



            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString());
            return ds.Tables[0];
        }
        /// <summary>
        /// 得到一个Table
        /// </summary>
        public DataTable GetTable(string projectID, string FilePath, string Filename)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  CONCAT( 'ProjectID:',projectID, ',Path:',FilePath,',Name:',Filename,',Version:',Version) as'FileID' , projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename   ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,50) };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 得到一个List对象实体
        /// </summary>
        public List<fileinfo> GetModel(string projectID)
        {
            DataTable dt = GetTable(projectID);
            if (dt.Rows.Count > 0)
            {
                List<fileinfo> list = new List<fileinfo>();
                foreach (DataRow dr in dt.Rows)
                    list.Add(DataRowToModel(dr));

                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个List对象实体
        /// </summary>
        public List<fileinfo> GetModelWhere(string where)
        {
            DataTable dt = GetTablewhere(where);
            if (dt.Rows.Count > 0)
            {
                List<fileinfo> list = new List<fileinfo>();
                foreach (DataRow dr in dt.Rows)
                    list.Add(DataRowToModel(dr));

                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个List对象实体(只允许授权对应)
        /// </summary>
        public List<fileinfo> GetModelInlyBusSWhere(string where, string BusServerID)
        {
            DataTable dt = GetTableOnlyBusSwhere(where, BusServerID);
            if (dt.Rows.Count > 0)
            {
                List<fileinfo> list = new List<fileinfo>();
                foreach (DataRow dr in dt.Rows)
                    list.Add(DataRowToModel(dr));

                return list;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个List对象实体
        /// </summary>
        public List<fileinfo> GetModel(string projectID, string FilePath, string Filename)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename   ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,50) };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;

            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                List<fileinfo> list = new List<fileinfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                    list.Add(DataRowToModel(dr));

                return list;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public fileinfo GetModel(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,Note ,ComMinVersion,ComMaxVersion from fileinfo ");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)          };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;

            fileinfo model = new fileinfo();
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
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
        public fileinfo GetModelandImage(string projectID, string FilePath, string Filename, string Version)
        {
            if (string.IsNullOrWhiteSpace(projectID))
            {
                projectID = "default";
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime, ifnull(b.images,a.images) as'images',Note,ComMinVersion,ComMaxVersion  
from fileinfo a
left outer  join FileImage b on a.ImageID = b.ImageID  ");
            strSql.Append(" where projectID=@projectID and FilePath=@FilePath and Filename=@Filename and Version=@Version ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@projectID", MySqlDbType.VarChar,20),
                    new MySqlParameter("@FilePath", MySqlDbType.VarChar,100),
                    new MySqlParameter("@Filename", MySqlDbType.VarChar,50),
                    new MySqlParameter("@Version", MySqlDbType.VarChar,20)          };
            parameters[0].Value = projectID;
            parameters[1].Value = FilePath;
            parameters[2].Value = Filename;
            parameters[3].Value = Version;

            fileinfo model = new fileinfo();
            DataSet ds = DbHelperMySQLMySpring.Query(strSql.ToString(), parameters);
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
        public fileinfo DataRowToModel(DataRow row)
        {
            fileinfo model = new fileinfo();

            if (row != null)
            {
                if (row["projectID"] != null)
                {
                    model.projectID = row["projectID"].ToString();
                }
                if (row["FilePath"] != null)
                {
                    model.FilePath = row["FilePath"].ToString();
                }
                if (row["Filename"] != null)
                {
                    model.Filename = row["Filename"].ToString();
                }
                if (row["Version"] != null)
                {
                    model.Version = row["Version"].ToString();
                }
                if (row["isdirect"] != null && row["isdirect"].ToString() != "")
                {
                    if ((row["isdirect"].ToString() == "1") || (row["isdirect"].ToString().ToLower() == "true"))
                    {
                        model.isdirect = true;
                    }
                    else
                    {
                        model.isdirect = false;
                    }
                }
                if (row["Extension"] != null)
                {
                    model.Extension = row["Extension"].ToString();
                }
                if (row["FileDescription"] != null)
                {
                    model.FileDescription = row["FileDescription"].ToString();
                }
                if (row["Note"] != null)
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["CompanyName"] != null)
                {
                    model.CompanyName = row["CompanyName"].ToString();
                }
                if (row["FileSize"] != null)
                {
                    model.FileSize = row["FileSize"].ToString();
                }
                if (row["cTime"] != null && row["cTime"].ToString() != "")
                {
                    model.cTime = DateTime.Parse(row["cTime"].ToString());
                }
                if (row["MTime"] != null && row["MTime"].ToString() != "")
                {
                    model.MTime = DateTime.Parse(row["MTime"].ToString());
                }
                if (row["upTime"] != null && row["upTime"].ToString() != "")
                {
                    model.upTime = DateTime.Parse(row["upTime"].ToString());
                }
                if (row["ComMinVersion"] != null)
                {
                    model.ComMinVersion = row["ComMinVersion"].ToString();
                }
                if (row["ComMaxVersion"] != null)
                {
                    model.ComMaxVersion = row["ComMaxVersion"].ToString();
                }
                if (row.Table.Columns.Contains("images"))
                {
                    if (row["images"] != null)
                    {
                        model.images = (Byte[])row["images"];
                    }
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
            strSql.Append("select projectID,FilePath,Filename,Version,isdirect,Extension,FileDescription,CompanyName,FileSize,cTime,MTime,upTime,images,Note,ComMinVersion,ComMaxVersion ");
            strSql.Append(" FROM fileinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM fileinfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQLMySpring.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.Version desc");
            }
            strSql.Append(")AS Row, T.*  from fileinfo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DbHelperMySQLMySpring.Query(strSql.ToString());
        }


        #endregion  BasicMethod

    }
}

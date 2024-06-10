using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Core;
using MySql.Data.MySqlClient;

namespace POSC2BUnionPay
{
    public class BaccountConfig
    {
        public BaccountConfig(string HOS_ID)
        {
            if (HOS_ID == "test")
            {
                init = true;
                initMsg = "初始化成功";
                msgSrc = "WWW.TEST.COM";
                msgSrcId = "1017";
                md5Key = "impARTxrQcfwmRijpDNCw6hPxaWCddKEpYxjaKXDhCaTCXJ6";
                mid = "898310148160568";
                tid = "38557688";
                appid = "10037e6f6823b20801682b6a5e5a0006";
                appkey = "1c4e3b16066244ae9b236a09e5b312e8";
                return;
            } 
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from baccountposc2b where hos_id=@HOS_ID ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@HOS_ID", MySqlDbType.VarChar,10)          };
            parameters[0].Value = HOS_ID;

            DataTable dt = DbHelperMySQL.Query(strSql.ToString(), parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                init = true;
                initMsg = "初始化成功";
                msgSrc = dt.Rows[0]["msgSrc"].ToString();
                msgSrcId = dt.Rows[0]["msgSrcId"].ToString();
                md5Key = dt.Rows[0]["md5Key"].ToString();
                mid = dt.Rows[0]["mid"].ToString();
                tid = dt.Rows[0]["tid"].ToString();
                appkey = dt.Rows[0]["appkey"].ToString();
                appid = dt.Rows[0]["appid"].ToString();
                apiUrl = dt.Rows[0]["apiUrl"].ToString();
            }
            else
            {
                init = false;
                initMsg = "平台账户信息未配置";
            }
        }

        private bool _init = false;
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool init
        {
            get { return _init; }
            set { _init = value; }
        }

        private string _initMsg = "账户配置尚未初始化";
        /// <summary>
        /// 消息来源
        /// </summary>
        public string initMsg
        {
            get { return _initMsg; }
            set { _initMsg = value; }
        }


        private string _msgSrc = "JSQH";
        /// <summary>
        /// 消息来源
        /// </summary>
        public string msgSrc
        {
            get { return _msgSrc; }
            set { _msgSrc = value; }
        }


        private string _msgSrcId = "";
        /// <summary>
        /// 来源编号-银商分配
        /// </summary>
        public string msgSrcId
        {
            get { return _msgSrcId; }
            set { _msgSrcId = value; }
        }

        private string _md5Key = "";
        /// <summary>
        /// MD5秘钥
        /// </summary>
        public string md5Key
        {
            get { return _md5Key; }
            set { _md5Key = value; }
        }

        private string _mid = "";
        /// <summary>
        /// 商户号
        /// </summary>
        public string mid
        {
            get { return _mid; }
            set { _mid = value; }
        }

        private string _tid = "";
        /// <summary>
        /// 终端号
        /// </summary>
        public string tid
        {
            get { return _tid; }
            set { _tid = value; }
        }

        private string _appid = "";
        /// <summary>
        /// 
        /// </summary>
        public string appid
        {
            get { return _appid; }
            set { _appid = value; }
        }

        private string _appkey = "";
        /// <summary>
        /// MD5秘钥
        /// </summary>
        public string appkey
        {
            get { return _appkey; }
            set { _appkey = value; }
        }
        private string _apiUrl = "";
        public string apiUrl
        {
            get { return _apiUrl; }
            set { _apiUrl = value; }
        }
        
    }
}

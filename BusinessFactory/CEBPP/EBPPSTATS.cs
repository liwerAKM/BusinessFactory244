using EBPP.Log.Model;
using EBPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{

    /// <summary>
    /// 查询统计每日上传量请求
    /// </summary>
    public class STATSApplyCountReq
    {
        /// <summary>
        ///统计类型 apply/open
        /// </summary>
        public string StatType;
        /// <summary>
        /// 开始日期 yyyy-MM-dd
        /// </summary>
        public DateTime StartDate;
        /// <summary>
        /// 结束日期yyyy-MM-dd
        /// </summary>
        public DateTime EndDate;
        /// <summary>
        /// 医院代码
        /// </summary>
        public string unifiedOrgCode;
    }
    /// <summary>
    /// 查询统计每日上传量返回
    /// </summary>
    public class STATSApplyCountResp
    {
        /// <summary>
        /// 返回结果代码 "0"时成功
        /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public List<MapplycountQ> list;
    }


    /// <summary>
    /// 查询统计错误请求
    /// </summary>
    public class STATSApplyErrorReq
    {
        /// <summary>
        ///是否包含上传数据
        /// </summary>
        public int ContainsInData;
        /// <summary>
        /// 开始日期 yyyy-MM-dd
        /// </summary>
        public DateTime StartDate;
        /// <summary>
        /// 结束日期yyyy-MM-dd
        /// </summary>
        public DateTime EndDate;
        /// <summary>
        /// 医院代码
        /// </summary>
        public string unifiedOrgCode;
    }

    /// <summary>
    /// 查询统计错误返回
    /// </summary>
    public class STATSApplyErrorResp
    {
        /// <summary>
        /// 返回结果代码 "0"时成功
        /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public List<MapplyerrorQ> list;
    }


    /// <summary>
    /// 加医院请求
    /// </summary>
    public class STATSAddOrgReq
    {    /// <summary>
         ///Key
         /// </summary>
        public string AddKey;

        /// <summary>
        ///医院名称
        /// </summary>
        public string  OrgName;
       
        /// <summary>
        /// 医院代码
        /// </summary>
        public string unifiedOrgCode;

        /// <summary>
        /// 是否返回医院列表 1 返回
        /// </summary>
        public int RetList;
    }

    /// <summary>
    /// 加医院请求返回
    /// </summary>
    public class STATSAddOrgResp
    {
        /// <summary>
        /// 返回结果代码 "0"时成功
        /// </summary>
        public string Code;
        /// <summary>
        /// 返回结果说明
        /// </summary>
        public string Msg;

        public List<MOrginfo > list;
    }

}
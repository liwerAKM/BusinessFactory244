using CommonModel;
using OnlineBusHos153_GJYB.Model;
using Soft.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace OnlineBusHos153_GJYB.BUS
{
    /// <summary>
    /// A方案，HIS封装调用医保接口
    /// </summary>
    public class ABusiness : IBusiness
    {
        public virtual DataReturn PSNQUERY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN _in = JSONSerializer.Deserialize<GJYB_PSNQUERY_M.GJYB_PSNQUERY_IN>(injson);
            GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT _out = new GJYB_PSNQUERY_M.GJYB_PSNQUERY_OUT();

            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;

            Models.T1101.Root t1101 = new Models.T1101.Root();
            t1101.data = new Models.T1101.Data();
            t1101.data = new Models.T1101.Data();
            t1101.data.mdtrt_cert_type = _in.MDTRT_CERT_TYPE;
            t1101.data.mdtrt_cert_no = _in.MDTRT_CERT_NO;
            t1101.data.card_sn = _in.CARD_SN;
            t1101.data.begntime = _in.BEGNTIME;
            t1101.data.psn_cert_type = _in.PSN_CERT_TYPE;
            t1101.data.certno = _in.CERTNO;
            t1101.data.psn_name = _in.PSN_NAME;

            string insuplc_admdvs = ""; //_in.INSUPLC_ADMDVS

            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
            //需要返回的字段
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "psn_no", rt1101.baseinfo.psn_no);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "certno", rt1101.baseinfo.certno);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "psn_name", rt1101.baseinfo.psn_name);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "gend", rt1101.baseinfo.gend);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "naty", rt1101.baseinfo.naty);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "brdy", rt1101.baseinfo.brdy);
            //string balc = FormatHelper.GetStr(insurinfo.balc);
            //string insutype = FormatHelper.GetStr(insurinfo.insutype);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "balc", balc);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "insutype", insutype);
        }

        public virtual DataReturn REGTRY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_REGTRY_M.GJYB_REGTRY_IN _in = JSONSerializer.Deserialize<GJYB_REGTRY_M.GJYB_REGTRY_IN>(injson);
            GJYB_REGTRY_M.GJYB_REGTRY_OUT _out = new GJYB_REGTRY_M.GJYB_REGTRY_OUT();
            string HOS_SN = _in.HOS_SN;
            string psn_no = _in.PSN_NO;
            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;


            //需要返回的字段
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "mdtrt_id", rt2206.setlinfo.mdtrt_id);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "medfee_sumamt", FormatHelper.GetStr(rt2206.setlinfo.medfee_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "acct_pay", FormatHelper.GetStr(rt2206.setlinfo.acct_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "psn_cash_pay", FormatHelper.GetStr(rt2206.setlinfo.psn_cash_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "fund_pay_sumamt", FormatHelper.GetStr(rt2206.setlinfo.fund_pay_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "oth_pay", FormatHelper.GetStr(rt2206.setlinfo.oth_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "balc", FormatHelper.GetStr(rt2206.setlinfo.balc));

        }

        public virtual DataReturn OUTPTRY(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_OUTPTRY_M.GJYB_OUTPTRY_IN _in = JSONSerializer.Deserialize<GJYB_OUTPTRY_M.GJYB_OUTPTRY_IN>(injson);
            GJYB_OUTPTRY_M.GJYB_OUTPTRY_OUT _out = new GJYB_OUTPTRY_M.GJYB_OUTPTRY_OUT();
            string HOS_SN = _in.HOS_SN;
            string psn_no = _in.PSN_NO;
            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
            //需要返回的字段
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "mdtrt_id", rt2206.setlinfo.mdtrt_id);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "medfee_sumamt", FormatHelper.GetStr(rt2206.setlinfo.medfee_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "acct_pay", FormatHelper.GetStr(rt2206.setlinfo.acct_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "psn_cash_pay", FormatHelper.GetStr(rt2206.setlinfo.psn_cash_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "fund_pay_sumamt", FormatHelper.GetStr(rt2206.setlinfo.fund_pay_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "oth_pay", FormatHelper.GetStr(rt2206.setlinfo.oth_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "balc", FormatHelper.GetStr(rt2206.setlinfo.balc));
        }

        public virtual DataReturn SETTLE(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_SETTLE_M.GJYB_SETTLE_IN _in = JSONSerializer.Deserialize<GJYB_SETTLE_M.GJYB_SETTLE_IN>(injson);
            GJYB_SETTLE_M.GJYB_SETTLE_OUT _out = new GJYB_SETTLE_M.GJYB_SETTLE_OUT();

            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;
            string HOS_SN = _in.HOS_SN;
            string ISGH = _in.ISGH;
            string TRAN_ID = HOS_ID + "_" + (ISGH == "1" ? "01" : "02") + "_" + HOS_SN;
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            dataReturn.Param = JSONSerializer.Serialize(_out);
            return dataReturn;
            //需要返回的字段
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "mdtrt_id", rt2207.setlinfo.mdtrt_id);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "setl_id", rt2207.setlinfo.setl_id);
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "medfee_sumamt", FormatHelper.GetStr(rt2207.setlinfo.medfee_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "acct_pay", FormatHelper.GetStr(rt2207.setlinfo.acct_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "psn_cash_pay", FormatHelper.GetStr(rt2207.setlinfo.psn_cash_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "fund_pay_sumamt", FormatHelper.GetStr(rt2207.setlinfo.fund_pay_sumamt));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "oth_pay", FormatHelper.GetStr(rt2207.setlinfo.oth_pay));
            //XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "balc", FormatHelper.GetStr(rt2207.setlinfo.balc));
        }

        public virtual DataReturn REFUND(string injson)
        {
            DataReturn dataReturn = new DataReturn();
            GJYB_REFUND_M.GJYB_REFUND_IN _in = JSONSerializer.Deserialize<GJYB_REFUND_M.GJYB_REFUND_IN>(injson);
            GJYB_REFUND_M.GJYB_REFUND_OUT _out = new GJYB_REFUND_M.GJYB_REFUND_OUT();

            string HOS_ID = _in.HOS_ID;
            string opter_no = _in.USER_ID;

            string mdtrt_id = _in.MDTRT_ID;
            string setl_id = _in.SETL_ID;
            dataReturn.Code = 0;
            dataReturn.Msg = "SUCCESS";
            return dataReturn;

            /*
            Models.RT2208.Root rt2208 = outputRoot2208.GetOutput<Models.RT2208.Root>();
            //需要返回的字段
            XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "mdtrt_id", rt2208.setlinfo.mdtrt_id);
            XMLHelper.X_XmlInsertNode(rtndoc, "ROOT/BODY", "setl_id", rt2208.setlinfo.setl_id);
            */
        }

        public virtual DataReturn COMMON(string injson)
        {
            throw new NotImplementedException();
        }
    }
}

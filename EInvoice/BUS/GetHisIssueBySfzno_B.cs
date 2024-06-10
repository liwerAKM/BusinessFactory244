using CommonModel;
using ConstData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EInvoice.Model;
using Soft.Core;

namespace EInvoice.BUS
{
    class GetHisIssueBySfzno_B
    {
        public static string GetHisIssueBySfzno(string json_in)
        {
            GetHisIssueBySfzno_IN _in = JSONSerializer.Deserialize<GetHisIssueBySfzno_IN>(json_in);
            GetHisIssueBySfzno_OUT _out = new GetHisIssueBySfzno_OUT();
            DataReturn dataReturn = new DataReturn();
            if (_in.BARCODE == "1")
            {
                dataReturn.Code = 6;
                dataReturn.Msg = "无明细";
            }
            else
            {

                _out.HISISSUELISTS = new List<GetHisIssueBySfzno_OUT.Hisissuelist>();
                GetHisIssueBySfzno_OUT.Hisissuelist list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                list.INVOICE_CODE = "32060121";
                list.INVOICE_NUMBER = "0212521549";
                list.INVOICING_PARTY_NAME = "南京市江宁区湖熟街道社区卫生服务中心";
                list.PAYER_PARTY_NAME = "金淑暧";
                list.TOTAL_AMOUNT = "67.00";
                list.SFTYPENAME = "门诊";
                list.STATUS = "已红冲";
                list.SAVEDDATE_TIME = "2021-07-09 17:21";
                list.ISPRINT = "0";
                _out.HISISSUELISTS.Add(list);

                list = new GetHisIssueBySfzno_OUT.Hisissuelist();
                list.INVOICE_CODE = "32060121";
                list.INVOICE_NUMBER = "0212521548";
                list.INVOICING_PARTY_NAME = "南京市江宁区湖熟街道社区卫生服务中心";
                list.PAYER_PARTY_NAME = "金淑暧";
                list.TOTAL_AMOUNT = "10.00";
                list.SFTYPENAME = "门诊";
                list.STATUS = "";
                list.SAVEDDATE_TIME = "2021-07-09 17:06";
                list.ISPRINT = "0";
                _out.HISISSUELISTS.Add(list);


                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);


            }
            string json_out = JSONSerializer.Serialize(dataReturn);
            return json_out;
        }
    }
}

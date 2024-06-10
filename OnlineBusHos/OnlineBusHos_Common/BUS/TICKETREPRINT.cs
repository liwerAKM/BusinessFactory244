using CommonModel;
using Soft.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos_Common.BUS
{
    class TICKETREPRINT
    {
        public static string B_TICKETREPRINT(string json_in)
        {
            DataReturn dataReturn = new DataReturn();
            string json_out = "";
            try
            {
                Model.TICKETREPRINT.TICKETREPRINT_IN _in = JSONSerializer.Deserialize<Model.TICKETREPRINT.TICKETREPRINT_IN>(json_in);
                Model.TICKETREPRINT.TICKETREPRINT_OUT _out = new Model.TICKETREPRINT.TICKETREPRINT_OUT();

                var db = new DbMySQLZZJ().Client;

                if (string.IsNullOrEmpty(_in.TYPE)) 
                {
                    _in.TYPE = "1";
                }
                if (_in.TYPE == "1")
                {
                   
                    int SERIAL_NO = 0;//预约标识
                    if (!PubFunc.GetSysID("TICKETREPRINT", out SERIAL_NO))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = "取[TICKETREPRINT]系统ID失败";
                        dataReturn.Param = JSONSerializer.Serialize(_out);
                        goto EndPoint;
                    }
                    SqlSugarModel.Ticketreprint model = new SqlSugarModel.Ticketreprint();
                    model.SERIAL_NO = SERIAL_NO;
                    model.HOS_ID = _in.HOS_ID;
                    model.BIZ_TYPE = _in.PT_TYPE;
                    model.HOS_SN = _in.DJ_ID;
                    model.TEXT = _in.TEXT;
                    model.lTERMINAL_SN = _in.LTERMINAL_SN;
                    model.NOW = DateTime.Now;
                    model.SFZ_NO = _in.SFZ_NO;
                    model.print_times = 1;
                    int row = db.Insertable(model).ExecuteCommand();
                }
                else if (_in.TYPE == "2") 
                {
                    int TICKETREPRINTDAYS = 7;
                    int TICKETREALLOWPRINTTIMES = 2;//允许打印次数
                    SqlSugarModel.SysConfig model = db.Queryable<SqlSugarModel.SysConfig>().Where(t => t.HOS_ID == _in.HOS_ID && t.config_key == "TICKETREPRINTDAYS").First();
                    if (model != null)
                    {
                        TICKETREPRINTDAYS =FormatHelper.GetInt(model.config_value);
                    }
                    model = db.Queryable<SqlSugarModel.SysConfig>().Where(t => t.HOS_ID == _in.HOS_ID && t.config_key == "TICKETREALLOWPRINTTIMES").First();
                    if (model != null)
                    {
                        TICKETREALLOWPRINTTIMES = FormatHelper.GetInt(model.config_value);
                    }
                    var list=  db.Queryable<SqlSugarModel.Ticketreprint>().Where(t => t.HOS_ID == _in.HOS_ID && t.SFZ_NO == _in.SFZ_NO && SqlFunc.Between(t.NOW, DateTime.Now.AddDays(-1 * TICKETREPRINTDAYS), DateTime.Now)).ToList();
                    _out.ITEMLIST = new List<Model.TICKETREPRINT.ITEM>();
                    foreach (var ptitem in list)
                    {
                        Model.TICKETREPRINT.ITEM item = new Model.TICKETREPRINT.ITEM();
                        item.CAN_PRINT = ptitem.print_times >= TICKETREALLOWPRINTTIMES ? "0" : "1";
                        item.DJ_ID = ptitem.HOS_SN;
                        item.TEXT = ptitem.TEXT;
                        item.PRINT_TIMES = ptitem.print_times.ToString();
                        _out.ITEMLIST.Add(item);
                    }
                }
                else if (_in.TYPE == "3")
                {
                    var model = db.Queryable<SqlSugarModel.Ticketreprint>().Where(t => t.HOS_ID == _in.HOS_ID && t.SFZ_NO == _in.SFZ_NO && t.HOS_SN==_in.DJ_ID && t.BIZ_TYPE==_in.PT_TYPE).First();
                    model.print_times++;
                    db.Updateable(model);
                }
                dataReturn.Code = 0;
                dataReturn.Msg = "SUCCESS";
                dataReturn.Param = JSONSerializer.Serialize(_out);

               
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

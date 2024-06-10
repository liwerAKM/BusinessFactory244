using CommonModel;
using Soft.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;

namespace OnlineBusHos133_Common.BUS
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

                //入参校验
                if (string.IsNullOrEmpty(_in.SFZ_NO)&& string.IsNullOrEmpty(_in.YY_LSH))
                {
                    dataReturn.Code = 4;
                    dataReturn.Msg = "身份证号与院内号不能同时为空";
                    goto EndPoint;
                }
                var db = new DbMySQLZZJ().Client;

                if (string.IsNullOrEmpty(_in.YY_LSH))//20230809 天长中医院以流水号为准
                {
                    XmlDocument doc = QHXmlMode.GetBaseXml("GETPATINFO", "1");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "HOS_ID", string.IsNullOrEmpty(_in.HOS_ID) ? "" : _in.HOS_ID.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "lTERMINAL_SN", string.IsNullOrEmpty(_in.LTERMINAL_SN) ? "" : _in.LTERMINAL_SN.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "USER_ID", "qhzzj001");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_TYPE", "4");
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "YLCARD_NO", _in.SFZ_NO);
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "SFZ_NO", string.IsNullOrEmpty(_in.SFZ_NO) ? "" : _in.SFZ_NO.Trim());
                    XMLHelper.X_XmlInsertNode(doc, "ROOT/BODY", "PAT_NAME","");

                    string inxml = doc.InnerXml;
                    string his_rtnxml = "";
                    if (!PubFunc.CALLSERVICE(_in.HOS_ID, inxml, ref his_rtnxml))
                    {
                        dataReturn.Code = 1;
                        dataReturn.Msg = his_rtnxml;
                        goto EndPoint;
                    }

                    XmlDocument xmldoc = XMLHelper.X_GetXmlDocument(his_rtnxml);
                    DataTable dtrev = XMLHelper.X_GetXmlData(xmldoc, "ROOT/BODY").Tables[0];

                    if (dtrev.Rows[0]["CLBZ"].ToString() == "0")
                    {
                        _in.YY_LSH = dtrev.Rows[0]["YY_LSH"].ToString();//
                    }

                   
                }



                //if (string.IsNullOrEmpty(_in.SFZ_NO))
                //{
                //    SqlSugarModel.PatInfo patInfo = new SqlSugarModel.PatInfo();
                //    SqlSugarModel.PatCard patCard = new SqlSugarModel.PatCard();
                //    SqlSugarModel.PatCardBind patCardBind = new SqlSugarModel.PatCardBind();
                //    if (!string.IsNullOrEmpty(_in.YLCARD_NO)&&_in.CARD_TYPE=="1")
                //    {
                //        patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.YY_LSH == _in.YLCARD_NO).First();
                //    }
                //    else if (!string.IsNullOrEmpty(_in.HOSPATID))
                //    {
                //        patCardBind = db.Queryable<SqlSugarModel.PatCardBind>().Where(t => t.HOS_ID == _in.HOS_ID && t.HOSPATID == _in.HOSPATID).First();//条码就是院内号
                //    }
                 
           

                //    if (!(patCardBind==null))
                //    {
                //        patInfo = db.Queryable<SqlSugarModel.PatInfo>().Where(t => t.PAT_ID == patCardBind.PAT_ID).First();                     
                //    }
                //    else
                //    {
                //        patInfo = null;
                //    }

                //    if (patInfo==null || string.IsNullOrEmpty(patInfo.SFZ_NO))
                //    {
                //        dataReturn.Code = 5;
                //        dataReturn.Msg = "未查询到用户信息，请使用身份证或者医保卡操作";
                //        goto EndPoint;
                //    }
                //    else
                //    {
                //        _in.SFZ_NO = patInfo.SFZ_NO;
                //    }

                //}

                

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
                    model.YY_LSH = _in.YY_LSH;
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

                    var list=  db.Queryable<SqlSugarModel.Ticketreprint>().Where(t => t.HOS_ID == _in.HOS_ID && t.BIZ_TYPE==_in.PT_TYPE && SqlFunc.Between(t.NOW, DateTime.Now.AddDays(-1 * TICKETREPRINTDAYS), DateTime.Now) && (t.YY_LSH == _in.YY_LSH || t.SFZ_NO==_in.SFZ_NO)).ToList();
                    _out.ITEMLIST = new List<Model.TICKETREPRINT.ITEM>();
                    foreach (var ptitem in list)
                    {
                        Model.TICKETREPRINT.ITEM item = new Model.TICKETREPRINT.ITEM();
                        item.CAN_PRINT = ptitem.print_times >= TICKETREALLOWPRINTTIMES ? "0" : "1";
                        item.DJ_ID = ptitem.HOS_SN;
                        item.TEXT = ptitem.TEXT;
                        item.PT_TYPE = ptitem.BIZ_TYPE;
                        item.PRINT_TIMES = ptitem.print_times.ToString();
                        _out.ITEMLIST.Add(item);
                    }
                }
                else if (_in.TYPE == "3")
                {
                    var model = db.Queryable<SqlSugarModel.Ticketreprint>().Where(t => t.HOS_ID == _in.HOS_ID && t.YY_LSH == _in.YY_LSH && t.HOS_SN==_in.DJ_ID && t.BIZ_TYPE==_in.PT_TYPE).First();
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

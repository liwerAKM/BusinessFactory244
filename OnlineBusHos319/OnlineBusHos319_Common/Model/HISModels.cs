using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineBusHos319_Common.Model
{
    public class HISModels
    {
        public class baseRsponse
        {
            /// <summary>
            /// 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string message { get; set; }

            public object data { get; set; }

            public T GetInput<T>()
            {
                if (data == null)
                {
                    return default(T);
                }
                if (data.GetType() == typeof(string))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data.ToString());
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                }
            }
        }

        public class BaseRequest
        { /// <summary>
          /// 医疗机构代码
          /// </summary>
            public string hospCode { get; set; }
            /// <summary>
            /// 操作员工号
            /// </summary>
            public string operCode { get; set; }
            /// <summary>
            /// 操作员姓名
            /// </summary>
            public string operName { get; set; }
            public string operTime { get; set; }

        }

        public class BasesiInfo
        {
            /// <summary>
            /// 就诊凭证类型（按国家医保标准字典）
            /// </summary>
            public string mdtrt_cert_type { get; set; }
            /// <summary>
            /// 就诊凭证编号
            /// </summary>
            public string mdtrt_cert_no { get; set; }
            /// <summary>
            /// 卡识别号（就诊凭证类型为“03-社会保障卡”时必填）
            /// </summary>
            public string card_sn { get; set; }
            /// <summary>
            /// 开始时间（获取历史参保信息时传入）
            /// </summary>
            public string begntime { get; set; }
            /// <summary>
            /// 人员证件类型
            /// </summary>
            public string psn_cert_type { get; set; }
            /// <summary>
            /// 证件号码（为“准新生儿”时必填，填写“准生儿社会保障卡号
            /// </summary>
            public string certno { get; set; }
            /// <summary>
            /// 人员姓名
            /// </summary>
            public string psn_name { get; set; }
        }

        /// <summary>
        /// 病人唯一号查询病人信息
        /// </summary>
        public class A101
        {
            public class A101Request: BaseRequest
            {
                /// <summary>
                /// 病人唯一号(HIS内码，全院唯一)
                /// </summary>
                public string patiId { get; set; }
            }
        }
        /// <summary>
        /// 身份证号查询病人信息
        /// </summary>
        public class A102
        {
            public class A102Request : BaseRequest
            {
                /// <summary>
                /// 身份证号
                /// </summary>
                public string idCardNo { get; set; }
            }

        }

        public class PatinfoData
        {
            /// <summary>
            /// 院内账户余额
            /// </summary>
            public string balance { get; set; }
            /// <summary>
            /// 出生日期
            /// </summary>
            public string birthDate { get; set; }
            /// <summary>
            /// 性别编码
            /// </summary>
            public string genderCode { get; set; }//(M,男;F,女;U,未知;O,其他)
            /// <summary>
            /// 性别名称
            /// </summary>
            public string enderName { get; set; }//(M,男;F,女;U,未知;O,其他)
            /// <summary>
            ///身份证号
            /// </summary>
            public string idCardNo { get; set; }
            /// <summary>
            /// 联系地址
            /// </summary>
            public string linkAddress { get; set; }
            /// <summary>
            /// 联系电话
            /// </summary>
            public string linkTel { get; set; }
            /// <summary>
            /// 婚姻状况
            /// </summary>
            public string mari { get; set; }//(1,未婚;2,已婚;3,丧偶;4,离婚;9,未说明的婚姻状况;21,初婚;22,再婚;23,复婚;)
            /// <summary>
            /// 创建日期
            /// </summary>
            public string operDate { get; set; }
            /// <summary>
            /// 籍贯
            /// </summary>
            public string origo { get; set; }
            /// <summary>
            /// 患者费别编码
            /// </summary>
            public string pactCode { get; set; }
            /// <summary>
            /// 患者费别说明
            /// </summary>
            public string pactName { get; set; }
            /// <summary>
            /// 病人唯一号
            /// </summary>
            public string patiId { get; set; }
            /// <summary>
            /// 姓名
            /// </summary>
            public string patiName { get; set; }
        }
        /// <summary>
        /// 查询科室医生排班信息
        /// </summary>
        public class A104
        {
            public class A104Reguest: BaseRequest
            {
                /// <summary>
                /// 起始时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 科室编码，若为空，查询所有科室
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 医生编码，若为空，查询所有医生
                /// </summary>
                public string doctCode { get; set; }
                /// <summary>
                /// 截止时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string endTime { get; set; }
               
                /// <summary>
                /// 排班类型(0,科室;1,医生;2,全部)
                /// </summary>
                public string scheduleType { get; set; }
            }

            public class Data
            {
                /// <summary>
                /// 开始看诊时间
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 科室简称
                /// </summary>
                public string deptAbbr { get; set; }
                /// <summary>
                /// 科室编码
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 科室位置
                /// </summary>
                public string deptLoc { get; set; }
                /// <summary>
                /// 科室名称
                /// </summary>
                public string deptName { get; set; }
                /// <summary>
                /// 科室简介
                /// </summary>
                public string deptProfile { get; set; }
                /// <summary>
                /// 医生编码
                /// </summary>
                public string doctCode { get; set; }
                /// <summary>
                /// 医生姓名
                /// </summary>
                public string doctName { get; set; }
                /// <summary>
                /// 结束看诊时间
                /// </summary>
                public string endTime { get; set; }
                /// <summary>
                /// 午别代码
                /// </summary>
                public string noonCode { get; set; }
                /// <summary>
                /// 午别名称
                /// </summary>
                public string noonName { get; set; }
                /// <summary>
                /// 上级科室代码
                /// </summary>
                public string parentCode { get; set; }
                /// <summary>
                /// 上级科室名称
                /// </summary>
                public string parentName { get; set; }
                /// <summary>
                /// 挂号级别
                /// </summary>
                public string regLevelCode { get; set; }
                /// <summary>
                /// 挂号级别说明
                /// </summary>
                public string regLevelName { get; set; }
                /// <summary>
                /// 排班序号
                /// </summary>
                public string scheduleNo { get; set; }
                /// <summary>
                /// 看诊日期
                /// </summary>
                public string seeDate { get; set; }
                /// <summary>
                /// 挂号费(单位：元)
                /// </summary>
                public string regPrice { get; set; }
                /// <summary>
                /// 预约号源总数
                /// </summary>
                public string sumAppointmentCount { get; set; }
                /// <summary>
                /// 现场号源总数
                /// </summary>
                public string sumWindowCount { get; set; }
                /// <summary>
                /// 预约挂号剩余号源
                /// </summary>
                public string surplusAppointmentCount { get; set; }
                /// <summary>
                /// 现场挂号剩余号源
                /// </summary>
                public string surplusWindowCount { get; set; }
                /// <summary>
                /// 周
                /// </summary>
                public string week { get; set; }
                /// <summary>
                /// 是否启用分时段号序 1:是 0:否 为1时可以调用A108或者L007
                /// </summary>
                public string sequenceFlag { get; set; }
                /// <summary>
                /// 号别
                /// </summary>
                public string regType { get; set; }
                /// <summary>
                /// 复诊号别标识 0否 1是
                /// </summary>
                public string firstFlag { get; set; }
                /// <summary>
                /// 是否停止挂号 0否 1 是
                /// </summary>
                public string stopRegFlag { get; set; }

                public string checkName { get; set; }
            }
        }
        /// <summary>
        /// 查询门诊排班科室信息
        /// </summary>
        public class A105
        {
            public class A105Request:BaseRequest
            {
                /// <summary>
                /// 起始时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 截止时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string endTime { get; set; }
            }

            public class Data
            {
                /// <summary>
                /// 科室简称
                /// </summary>
                public string deptAbbr { get; set; }
                /// <summary>
                /// 科室编码
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 科室名称
                /// </summary>
                public string deptName { get; set; }
                /// <summary>
                /// 科室简介
                /// </summary>
                public string deptProfile { get; set; }
                /// <summary>
                /// 上级科室代码
                /// </summary>
                public string parentCode { get; set; }
                /// <summary>
                /// 学科分类
                /// </summary>
                public string subjectType { get; set; }
                /// <summary>
                /// 上级科室名称
                /// </summary>
                public string parentName { get; set; }
                /// <summary>
                /// 急诊标识
                /// </summary>
                public string emcFlag { get; set; }
            }
        }
        /// <summary>
        /// 查询门诊排班医生信息
        /// </summary>
        public class A106
        {
            public class A106Request:BaseRequest
            {
                /// <summary>
                /// 起始时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 科室编码,传空查询所有医生，不为空则查科室内的医生
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 截止时间（含）,北京时间。yyyy-MM-dd格式
                /// </summary>
                public string endTime { get; set; }
            }


            public class Data
            {
                /// <summary>
                /// 出生日期
                /// </summary>
                public string birthdate { get; set; }
                /// <summary>
                /// 科室编码
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 科室名称
                /// </summary>
                public string deptName { get; set; }
                /// <summary>
                /// 医生编码
                /// </summary>
                public string doctCode { get; set; }
                /// <summary>
                /// 医生姓名
                /// </summary>
                public string doctName { get; set; }
                /// <summary>
                /// 身份证号
                /// </summary>
                public string idCardNo { get; set; }
                /// <summary>
                /// 电话
                /// </summary>
                public string tel { get; set; }
                /// <summary>
                /// 职称编码(231,主任医师;232,副主任医师;233,主治医师;234,医师;235,医士)
                /// </summary>
                public string titleCode { get; set; }
                /// <summary>
                /// 性别编码(1,男;2,女)
                /// </summary>
                public string userSex { get; set; }
            }


        }
        /// <summary>
        /// 通过排班序号查询排班可用号序池信息
        /// </summary>
        public class A108
        {
            public class Request:BaseRequest
            {
                /// <summary>
                /// 排班序号
                /// </summary>
                public string scheduleNo { get; set; }
            }
            public class Data
            {
                /// <summary>
                /// 是否是加号的号源(1,是;0,否)
                /// </summary>
                public string addFlag { get; set; }
                /// <summary>
                /// 排班实体的ID
                /// </summary>
                public string scheduleNo { get; set; }
                /// <summary>
                /// 号序
                /// </summary>
                public string sequenceNo { get; set; }
                /// <summary>
                /// 号序类型(0,现场号;1,预约号)
                /// </summary>
                public string sequenceType { get; set; }
                /// <summary>
                /// 号序排序规则（0,现场号靠前;1,交叉排序;2,预约号靠前）
                /// </summary>
                public string sortType { get; set; }
                /// <summary>
                /// 号序状态(0,有效;1,已预约;2,挂号确认;3,爽约;4,无效);
                /// </summary>
                public string state { get; set; }
                /// <summary>
                /// 号序时间点
                /// </summary>
                public string timePoint { get; set; }
                /// <summary>
                /// 归属时间段 格式 08:00-08:59
                /// </summary>
                public string timePeriod { get; set; }

            }
        }
        /// <summary>
        /// 创建新病人信息
        /// </summary>
        public class A201
        {
            public class A201Request:BaseRequest
            {
                /// <summary>
                /// 出生日期,yyyy-MM-dd
                /// </summary>
                public string birthdate { get; set; }
                /// <summary>
                /// 物理卡号
                /// </summary>
                public string cardNo { get; set; }
                /// <summary>
                /// 卡类型(1,院内就诊卡;2,医保卡;3,健康卡)
                /// </summary>
                public string cardType { get; set; }
                /// <summary>
                /// 国家编码
                /// </summary>
                public string countryCode { get; set; }
                /// <summary>
                /// 性别编码(M,男;F,女;U,未知;O,其他)
                /// </summary>
                public string genderCode { get; set; }
                /// <summary>
                /// 地级市编码
                /// </summary>
                public string homeCity { get; set; }
                /// <summary>
                /// 区县编码
                /// </summary>
                public string homeDistrict { get; set; }
                /// <summary>
                /// 省编码
                /// </summary>
                public string homeProvince { get; set; }
                /// <summary>
                /// 身份证号
                /// </summary>
                public string idcardNo { get; set; }
                /// <summary>
                /// 详细地址
                /// </summary>
                public string linkAddress { get; set; }
                /// <summary>
                /// 联系电话
                /// </summary>
                public string linkTel { get; set; }
                /// <summary>
                /// 民族编码
                /// </summary>
                public string nationCode { get; set; }
                /// <summary>
                /// 费别编码
                /// </summary>
                public string pactCode { get; set; }
                /// <summary>
                /// 费别名称
                /// </summary>
                public string pactName { get; set; }
                /// <summary>
                /// 姓名
                /// </summary>
                public string patiName { get; set; }
            }
        }
        /// <summary>
        /// 挂号登记预算
        /// </summary>
        public class A202
        {
            public class A202Request : BaseRequest
            {
                /// <summary>
                /// 病历号
                /// </summary>
                public string patiId { get; set; }
                /// <summary>
                /// 排班序号
                /// </summary>
                public string scheduleNo { get; set; }
                /// <summary>
                /// 强制自费标识0 按病人费别结算 1按全自费结算当地社保未开通线上报销的情况下，此标识需要传1
                /// </summary>
                public string selfPay { get; set; }
                //01 自费 03医保
                public string pactCode { get; set; }

                public BasesiInfo siInfo { get; set; }
            }
        }
        /// <summary>
        /// 挂号登记结算
        /// </summary>
        public class A203
        {
            public class A203Request : BaseRequest
            {        /// <summary>
                     /// 交易金额，两位小数
                     /// </summary>
                public decimal? amonCost { get; set; }
                /// <summary>
                /// 支付流水号（银行/微信/支付宝等）
                /// </summary>
                public string bankTradeNo { get; set; }
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 支付方式编码(ZFB,支付宝;WX,微信)
                /// </summary>
                public string payTypeId { get; set; }
                /// <summary>
                /// 第三方支付平台生成的订单号/流水号
                /// </summary>
                public string openTradeNo { get; set; }
                /// <summary>
                /// 交易时间
                /// </summary>
                public string payTime { get; set; }
                /// <summary>
                /// ZFB支付宝 WX 微信 YH 银行卡
                /// </summary>
                public string paySource { get; set; }
                /// <summary>
                /// 终端来源 3 自助机 4公众号
                /// </summary>
                public string regSource { get; set; }
                /// <summary>
                /// 号序开始时间,格式08:00,为空时系统自动分配号序
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 号序结束时间,格式13:00,为空时系统自动分配号序
                /// </summary>
                public string endTime { get; set; }
                /// <summary>
                /// 项目代码,挂号时自动划价收费项目代码
                /// </summary>
                public string itemCode { get; set; }

                public string selfPay { get; set; }

                public siInfo siInfo { get; set; }

            }

            public class siInfo : BasesiInfo
            {
                /// <summary>
                /// 总金额
                /// </summary>
                public string totCost { get; set; }
                /// <summary>
                /// 结算唯一号
                /// </summary>
                public string invoiceNo { get; set; }
                /// <summary>
                /// 结算类型（1挂号 2门诊收费 3住院登记 4住院结算）
                /// </summary>
                public string finType { get; set; }
            }
        }

        public class A204
        {
            public class A204Request : BaseRequest
            {
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 第三方流水号
                /// </summary>
                public string tradeNo { get; set; }
            }
        }
        /// <summary>
        /// 预约挂号
        /// </summary>
        public class A205
        {
            public class A205Request : BaseRequest
            {
                /// <summary>
                /// 号序开始时间,格式08:00,为空时系统自动分配号序
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 号序结束时间,格式13:00,为空时系统自动分配号序
                /// </summary>
                public string endTime { get; set; }
                /// <summary>
                /// 病历号
                /// </summary>
                public string patiId { get; set; }
                /// <summary>
                /// 排班序号
                /// </summary>
                public string scheduleNo { get; set; }
                /// <summary>
                /// 预约号序,为空时系统自动分配号序
                /// </summary>
                public string sequenceNo { get; set; }
            }
        }
        /// <summary>
        /// 预约取号预算
        /// </summary>
        public class A207
        {
            public class A207Request : BaseRequest
            {
                public string patiId { get; set; }

                public string selfPay { get; set; }

                public string pactCode { get; set; }
                /// <summary>
                /// 预约记录号
                /// </summary>
                public string bookingNo { get; set; }

                public BasesiInfo siInfo { get; set; }
            }
        }
        /// <summary>
        /// 预约取号结算
        /// </summary>
        public class A208
        {
            public class A208Request : BaseRequest
            {
                /// <summary>
                /// 交易金额，两位小数
                /// </summary>
                public decimal? amonCost { get; set; }
                /// <summary>
                /// 支付流水号（银行/微信/支付宝等）
                /// </summary>
                public string bankTradeNo { get; set; }
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 支付方式编码(ZFB,支付宝;WX,微信)
                /// </summary>
                public string payTypeId { get; set; }
                /// <summary>
                /// 第三方支付平台生成的订单号/流水号
                /// </summary>
                public string openTradeNo { get; set; }
                /// <summary>
                /// 交易时间
                /// </summary>
                public string payTime { get; set; }
                /// <summary>
                /// ZFB支付宝 WX 微信 YH 银行卡
                /// </summary>
                public string paySource { get; set; }
                /// <summary>
                /// 终端来源 3 自助机 4公众号
                /// </summary>
                public string regSource { get; set; }
                /// <summary>
                /// 号序开始时间,格式08:00,为空时系统自动分配号序
                /// </summary>
                public string beginTime { get; set; }
                /// <summary>
                /// 号序结束时间,格式13:00,为空时系统自动分配号序
                /// </summary>
                public string endTime { get; set; }
                /// <summary>
                /// 项目代码,挂号时自动划价收费项目代码
                /// </summary>
                public string itemCode { get; set; }

                public string selfPay { get; set; }
                public siInfo siInfo { get; set; }
            }

            public class siInfo : BasesiInfo
            {
                /// <summary>
                /// 总金额
                /// </summary>
                public string totCost { get; set; }
                /// <summary>
                /// 结算唯一号
                /// </summary>
                public string invoiceNo { get; set; }
                /// <summary>
                /// 结算类型（1挂号 2门诊收费 3住院登记 4住院结算）
                /// </summary>
                public string finType { get; set; }
            }
        }

        public class A302
        {
            public class A302Request : BaseRequest
            {
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 收费的处方号集合,多个处方号中间用（英文逗号,）隔开
                /// </summary>
                public List<string> recipeNos { get; set; }
                /// <summary>
                /// 强制自费标识0 按国家医保结算(自助机可用) 1按全自费结算 2按国家线上医保结算(我的南京可用)当地社保未开通线上报销的情况下，此标识需要传1
                /// </summary>
                public string selfPay { get; set; }

                public BasesiInfo siInfo { get; set; }
            }


            public class A302Response
            {
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 优惠金额
                /// </summary>
                public string ecoCost { get; set; }
                /// <summary>
                /// 结算唯一号
                /// </summary>
                public string invoiceNo { get; set; }
                /// <summary>
                /// 自费金额
                /// </summary>
                public string ownCost { get; set; }
                /// <summary>
                /// 报销金额
                /// </summary>
                public string pubCost { get; set; }
                /// <summary>
                /// 总金额（TotCost= EcoCost +OwnCost+ PubCost）
                /// </summary>
                public string totCost { get; set; }

                public List<recipeDetails> recipeDetail { get; set; }

                public object siOutput { get; set; }
            }
            public class recipeDetails
            {
                /// <summary>
                /// 付数（天数）
                /// </summary>
                public string days { get; set; }
                /// <summary>
                /// 每次用量
                /// </summary>
                public string doseOnce { get; set; }
                /// <summary>
                /// 每次用量单位
                /// </summary>
                public string doseUnit { get; set; }
                /// <summary>
                /// 项目类别(1,药品;0,非药品)
                /// </summary>
                public string drugFlag { get; set; }
                /// <summary>
                /// 执行科室代码
                /// </summary>
                public string execDeptCode { get; set; }
                /// <summary>
                /// 执行科室名称
                /// </summary>
                public string execDeptName { get; set; }
                /// <summary>
                /// 最小费用代码
                /// </summary>
                public string feeCode { get; set; }
                /// <summary>
                /// 最小费用名称
                /// </summary>
                public string feeName { get; set; }
                /// <summary>
                /// 频次代码
                /// </summary>
                public string frequencyCode { get; set; }
                /// <summary>
                /// 项目代码
                /// </summary>
                public string itemCode { get; set; }
                /// <summary>
                /// 项目名称
                /// </summary>
                public string itemName { get; set; }
                /// <summary>
                /// 单位
                /// </summary>
                public string priceUnit { get; set; }
                /// <summary>
                /// 数量
                /// </summary>
                public string qty { get; set; }
                /// <summary>
                /// 处方内项目流水号
                /// </summary>
                public string recipeSeq { get; set; }
                /// <summary>
                /// 规格
                /// </summary>
                public string specs { get; set; }
                /// <summary>
                /// 单价
                /// </summary>
                public string unitPrice { get; set; }
                /// <summary>
                /// 用法代码
                /// </summary>
                public string usageCode { get; set; }
                /// <summary>
                /// 用法名称
                /// </summary>
                public string usageName { get; set; }
                /// <summary>
                /// 总金额
                /// </summary>
                public string totCost { get; set; }
                ///// <summary>
                ///// 1 没有对码，无法按国家医保报销，结算本张处方时A302的selfPay必须是1 强制自费
                ///// </summary>
                //public string ownFlag { get; set; }
            }
        }

        public class A303
        {
            public class A303Request:BaseRequest
            {
                /// <summary>
                /// 交易金额，两位小数
                /// </summary>
                public string amonCost { get; set; }
                /// <summary>
                /// 支付流水号（银行/微信/支付宝等）
                /// </summary>
                public string bankTradeNo { get; set; }
                /// <summary>
                /// 挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 结算唯一号
                /// </summary>
                public string invoiceNo { get; set; }
                /// <summary>
                /// 第三方支付平台生成的订单号/流水号
                /// </summary>
                public string openTradeNo { get; set; }
                /// <summary>
                /// 交易时间
                /// </summary>
                public string payTime { get; set; }
                /// <summary>
                /// 支付方式编码(ZFB,支付宝;WX,微信)
                /// </summary>
                public string payTypeId { get; set; }
                /// <summary>
                /// 收费的处方号集合,多个处方号中间用（英文逗号,）隔开
                /// </summary>
                public List<string> recipeNos { get; set; }

                public string selfPay { get; set; }

                public siInfo siInfo { get; set; }
            }

            public class siInfo : BasesiInfo
            {
                /// <summary>
                /// 总金额
                /// </summary>
                public string totCost { get; set; }
                /// <summary>
                /// 结算唯一号
                /// </summary>
                public string invoiceNo { get; set; }
                /// <summary>
                /// 结算类型（1挂号 2门诊收费 3住院登记 4住院结算）
                /// </summary>
                public string finType { get; set; }
            }
        }

        public class A309
        {
            public class A309Request : BaseRequest
            {
                /// <summary>
                /// 病人唯一号(HIS内码，全院唯一)
                /// </summary>
                public string patiId { get; set; }
            }


            public class A309Response
            {
                public class Data
                {
                    public List<recipeDetails> recipeDetails { get; set; }
                    public recipeSummary recipeSummary { get; set; }
                }
                public class recipeDetails
                {
                    /// <summary>
                    /// 付数（天数）
                    /// </summary>
                    public string days { get; set; }
                    /// <summary>
                    /// 每次用量
                    /// </summary>
                    public string doseOnce { get; set; }
                    /// <summary>
                    /// 每次用量单位
                    /// </summary>
                    public string doseUnit { get; set; }
                    /// <summary>
                    /// 项目类别(1,药品;0,非药品)
                    /// </summary>
                    public string drugFlag { get; set; }
                    /// <summary>
                    /// 执行科室代码
                    /// </summary>
                    public string execDeptCode { get; set; }
                    /// <summary>
                    /// 执行科室名称
                    /// </summary>
                    public string execDeptName { get; set; }
                    /// <summary>
                    /// 最小费用代码
                    /// </summary>
                    public string feeCode { get; set; }
                    /// <summary>
                    /// 最小费用名称
                    /// </summary>
                    public string feeName { get; set; }
                    /// <summary>
                    /// 频次代码
                    /// </summary>
                    public string frequencyCode { get; set; }
                    /// <summary>
                    /// 项目代码
                    /// </summary>
                    public string itemCode { get; set; }
                    /// <summary>
                    /// 项目名称
                    /// </summary>
                    public string itemName { get; set; }
                    /// <summary>
                    /// 单位
                    /// </summary>
                    public string priceUnit { get; set; }
                    /// <summary>
                    /// 数量
                    /// </summary>
                    public string qty { get; set; }
                    /// <summary>
                    /// 处方内项目流水号
                    /// </summary>
                    public string recipeSeq { get; set; }
                    /// <summary>
                    /// 规格
                    /// </summary>
                    public string specs { get; set; }
                    /// <summary>
                    /// 单价
                    /// </summary>
                    public string unitPrice { get; set; }
                    /// <summary>
                    /// 用法代码
                    /// </summary>
                    public string usageCode { get; set; }
                    /// <summary>
                    /// 用法名称
                    /// </summary>
                    public string usageName { get; set; }
                    /// <summary>
                    /// 总金额
                    /// </summary>
                    public string totCost { get; set; }
                    /// <summary>
                    /// 1 没有对码，无法按国家医保报销，结算本张处方时A302的selfPay必须是1 强制自费
                    /// </summary>
                    public string ownFlag { get; set; }
                }
                public class recipeSummary
                {
                    /// <summary>
                    /// 门诊号
                    /// </summary>
                    public string clinicNo { get; set; }
                    /// <summary>
                    /// 处方诊断编码
                    /// </summary>
                    public string diagnoseCode { get; set; }
                    /// <summary>
                    /// 处方诊断名称
                    /// </summary>
                    public string diagnoseName { get; set; }
                    /// <summary>
                    /// 开立医生工号
                    /// </summary>
                    public string doctCode { get; set; }
                    /// <summary>
                    /// 开立医生名称
                    /// </summary>
                    public string doctName { get; set; }
                    /// <summary>
                    /// 执行科室编码
                    /// </summary>
                    public string execDeptCode { get; set; }
                    /// <summary>
                    /// 执行科室名称
                    /// </summary>
                    public string execDeptName { get; set; }
                    /// <summary>
                    /// 处方开立时间，格式为yyyy-MM-dd HH:mm:ss
                    /// </summary>
                    public string operDate { get; set; }
                    /// <summary>
                    /// 患者唯一号
                    /// </summary>
                    public string patiId { get; set; }
                    /// <summary>
                    /// 处方类型（1,西药处方;2,中草药处方;3,其他）
                    /// </summary>
                    public string recipeClassType { get; set; }
                    /// <summary>
                    /// 处方号
                    /// </summary>
                    public string recipeNo { get; set; }
                    /// <summary>
                    /// 处方类别（01,普通处方;04,毒性药品;05,精神I类;06,精神Ⅱ类;07,精神Ⅲ类;08,戒毒药品;09,麻醉处方）
                    /// </summary>
                    public string recipeType { get; set; }
                    /// <summary>
                    /// 开立科室代码
                    /// </summary>
                    public string regDeptCode { get; set; }
                    /// <summary>
                    /// 开立科室名称
                    /// </summary>
                    public string regDeptName { get; set; }
                    /// <summary>
                    /// 总金额
                    /// </summary>
                    public string totCost { get; set; }
                    /// <summary>
                    /// 1 没有对码，无法按国家医保报销，结算本张处方时A302的selfPay必须是1 强制自费
                    /// </summary>
                    public string ownFlag { get; set; }
                    /// <summary>
                    /// 医保处方类别
                    /// </summary>
                    public string siRecipeType { get; set; }
                }

            }
        }

        public class TZ112
        {
            public class TZ112Request : BaseRequest
            {
                /// <summary>
                /// 病历号
                /// </summary>
                public string patiId { get; set; }
                /// <summary>
                /// 强制自费标识（0按病人费别结算，1按全自费结算。当地社保未开通线上报销的情况下，此标识需要传1）
                /// </summary>
                public string selfPay { get; set; }
                /// <summary>
                /// 床位号
                /// </summary>
                public string bedNo { get; set; }
                /// <summary>
                /// 卡片号
                /// </summary>
                public string cardNo { get; set; }
                /// <summary>
                /// 卡片类型 1磁卡 2医保卡
                /// </summary>
                public string cardType { get; set; }
                /// <summary>
                /// 转入预交金额（未结)
                /// </summary>
                public string changePrepayCost { get; set; }
                /// <summary>
                /// 主治医师代码
                /// </summary>
                public string chargeDoctCode { get; set; }
                /// <summary>
                /// 主治医师名称
                /// </summary>
                public string chargeDoctName { get; set; }
                /// <summary>
                /// 门诊诊断代码
                /// </summary>
                public string clinicDiagNoseCode { get; set; }
                /// <summary>
                /// 门诊诊断名称
                /// </summary>
                public string clinicDiagNoseName { get; set; }
                /// <summary>
                /// 门诊挂号流水号
                /// </summary>
                public string clinicNo { get; set; }
                /// <summary>
                /// 科室编码
                /// </summary>
                public string deptCode { get; set; }
                /// <summary>
                /// 科室名称
                /// </summary>
                public string deptName { get; set; }
                /// <summary>
                /// 住院医师代码
                /// </summary>
                public string houseDoctCode { get; set; }
                /// <summary>
                /// 住院医师名称
                /// </summary>
                public string houseDoctName { get; set; }
                /// <summary>
                /// 入院途径 1:门诊，2:急诊，3:转科，4:转院
                /// </summary>
                public string inAvenue { get; set; }
                /// <summary>
                /// 床位号
                /// </summary>
                public string inBedNo { get; set; }
                /// <summary>
                /// 入院情况
                /// </summary>
                public string inCircs { get; set; }
                /// <summary>
                /// 入院日期
                /// </summary>
                public string inDate { get; set; }
                /// <summary>
                /// 科室编码
                /// </summary>
                public string inDeptCode { get; set; }
                /// <summary>
                /// 科室名称
                /// </summary>
                public string inDeptName { get; set; }
                /// <summary>
                /// 住院诊断代码
                /// </summary>
                public string inDiagNoseCode { get; set; }
                /// <summary>
                /// 住院诊断名称
                /// </summary>
                public string inDiagNoseName { get; set; }
                /// <summary>
                /// 住院次数
                /// </summary>
                public string inTimes { get; set; }
                /// <summary>
                /// 病区编码
                /// </summary>
                public string inWardCode { get; set; }
                /// <summary>
                /// 入病区时间
                /// </summary>
                public string inWardDate { get; set; }
                /// <summary>
                /// 病区名称
                /// </summary>
                public string inWardName { get; set; }
                /// <summary>
                /// 警戒线
                /// </summary>
                public string moneyAlert { get; set; }
                /// <summary>
                /// 预约登记号
                /// </summary>
                public string preRegNo { get; set; }
                /// <summary>
                /// 预交金额(未结)
                /// </summary>
                public string prepayCost { get; set; }

                public string wardCode { get; set; }

                public string wardName { get; set; }

                public SiInfo siInfo { get; set; }
            }

            public class SiInfo: BasesiInfo
            {
          
                /// <summary>
                /// 费用累计
                /// </summary>
                public string addTotCost { get; set; }
                /// <summary>
                /// 结算时间
                /// </summary>
                public string balanceDate { get; set; }
                /// <summary>
                /// 个人起付金额
                /// </summary>
                public string baseCost { get; set; }
 
 
                /// <summary>
                /// 门诊诊断
                /// </summary>
                public string clinicDiagNose { get; set; }
                /// <summary>
                /// 就诊ID
                /// </summary>
                public string clinicNo { get; set; }
  
                /// <summary>
                /// 人员类别 1-在职、2-退休
                /// </summary>
                public string emplType { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string feeTimes { get; set; }
                /// <summary>
                /// 结算类型(1挂号 2门诊收费 3住院登记 4住院结算)
                /// </summary>
                public string finType { get; set; }
                /// <summary>
                /// 医药机构分担金额
                /// </summary>
                public string hosCost { get; set; }
                /// <summary>
                /// 医疗机构代码
                /// </summary>
                public string hospCode { get; set; }
                /// <summary>
                /// 入院诊断信息
                /// </summary>
                public string inDiagnose { get; set; }
                /// <summary>
                /// 入院诊断日期
                /// </summary>
                public string inDiagnoseDate { get; set; }
                /// <summary>
                /// 部分项目自付金额 
                /// </summary>
                public string itemPayCost { get; set; }
                /// <summary>
                /// 个人自付金额（乙类自付部分）
                /// </summary>
                public string itemYLCost { get; set; }

                /// <summary>
                /// 病区编码
                /// </summary>
                public string wardCode { get; set; }
                /// <summary>
                /// 病区名称
                /// </summary>
                public string wardName { get; set; }
            }
        }


        public class Query_Wjcx
        {
            public string ITEMCODE { get; set; }

            public string ITEMNAME { get; set; }

            public string SPELLCODE { get; set; }

            public string PRICE { get; set; }

            public string UNIT { get; set; }

            public string SPECS { get; set; }

            public string PRODUCERNAME { get; set; }

            public string LX { get; set; }
        }
    }
}

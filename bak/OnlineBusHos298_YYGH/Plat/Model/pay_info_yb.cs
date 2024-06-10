using System;
namespace Plat.Model
{
    /// <summary>
    /// register_pay:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class pay_info_yb
    {
        public pay_info_yb()
        { }
        #region Model
        private int _pay_id;
        private string _dj_id;
        private string _tradelsh;
        private string _mzlsh;
        private string _regpat_id;
        private string _djlsh;
        private string _yl_type;
        private string _sfz_no;
        private string _js_date;
        private string _cy_date;
        private string _cy_reason;
        private string _dis_code;
        private string _yjstype;
        private string _ztjstype;
        private string _usr_name;
        private string _fm_date;
        private string _cc;
        private string _taier_amount;
        private string _cardid;
        private string _zyybbh;
        private string _dept_code;
        private string _doc_no;
        private string _is_gh;
        private string _zsrcardid;
        private string _ss_issuccess;
        private string _bc_zje;
        private string _bc_tcje;
        private string _bc_dbjz;
        private string _bc_dbbx;
        private string _bc_mzbz;
        private string _bc_zhzc;
        private string _bc_xjzc;
        private string _bc_zhzczf;
        private string _bc_zhzczl;
        private string _bc_xjzczf;
        private string _bc_xjzczl;
        private string _ybfwnje;
        private string _zhye;
        private string _dbz_code;
        private string _instruction;
        private string _med_je;
        private string _chk_je;
        private string _bb_je;
        private string _by6;
        private string _ywzqh;
        private DateTime _deal_time;

        /// <summary>
        /// 支付标示
        /// </summary>
        public int PAY_ID
        {
            set { _pay_id = value; }
            get { return _pay_id; }
        }
        /// <summary>
        /// 院内唯一流水号
        /// </summary>
        public string DJ_ID
        {
            set { _dj_id = value; }
            get { return _dj_id; }
        }
        /// <summary>
        /// 交易流水号
        /// </summary>
        public string TRADELSH
        {
            set { _tradelsh = value; }
            get { return _tradelsh; }
        }
        /// <summary>
        /// 门诊/住院流水号
        /// </summary>
        public string MZLSH
        {
            set { _mzlsh = value; }
            get { return _mzlsh; }
        }
        /// <summary>
        /// 单据号
        /// </summary>
        public string DJLSH
        {
            set { _djlsh = value; }
            get { return _djlsh; }
        }
        /// <summary>
        ///医疗类别
        /// </summary>
        public string YL_TYPE
        {
            set { _yl_type = value; }
            get { return _yl_type; }
        }
        /// <summary>
        /// 结算日期
        /// </summary>
        public string JS_DATE
        {
            set { _js_date = value; }
            get { return _js_date; }
        }
        /// <summary>
        /// 出院日期
        /// </summary>
        public string CY_DATE
        {
            set { _cy_date = value; }
            get { return _cy_date; }
        }
        /// <summary>
        ///出院原因
        /// </summary>
        public string CY_REASON
        {
            set { _cy_reason = value; }
            get { return _cy_reason; }
        }
        /// <summary>
        /// 出院诊断疾病编码
        /// </summary>
        public string DIS_CODE
        {
            set { _dis_code = value; }
            get { return _dis_code; }
        }
        /// <summary>
        /// 月结算类别
        /// </summary>
        public string YJSTYPE
        {
            set { _yjstype = value; }
            get { return _yjstype; }
        }
        /// <summary>
        /// 中途结算标志
        /// </summary>
        public string ZTYJSTYPE
        {
            set { _ztjstype = value; }
            get { return _ztjstype; }
        }
        /// <summary>
        /// 经办人
        /// </summary>
        public string USR_NAME
        {
            set { _usr_name = value; }
            get { return _usr_name; }
        }
        /// <summary>
        /// 分娩日期
        /// </summary>
        public string FM_DATE
        {
            set { _fm_date = value; }
            get { return _fm_date; }
        }
        /// <summary>
        /// 产次
        /// </summary>
        public string CC
        {
            set { _cc = value; }
            get { return _cc; }
        }
        /// <summary>
        /// 胎儿数
        /// </summary>
        public string TAIER_AMOUNT
        {
            set { _taier_amount = value; }
            get { return _taier_amount; }
        }
        /// <summary>
        /// 社会保障卡号
        /// </summary>
        public string CARDID
        {
            set { _cardid = value; }
            get { return _cardid; }
        }
        /// <summary>
        /// 转院医院编号
        /// </summary>
        public string ZYYBBH
        {
            set { _zyybbh = value; }
            get { return _zyybbh; }
        }
        /// <summary>
        /// 科室编码
        /// </summary>
        public string DEPT_CODE
        {
            set { _dept_code = value; }
            get { return _dept_code; }
        }
        /// <summary>
        /// 医生编码
        /// </summary>
        public string DOC_NO
        {
            set { _doc_no = value; }
            get { return _doc_no; }
        }
        /// <summary>
        /// 是否为挂号费结算
        /// </summary>
        public string IS_GH
        {
            set { _is_gh = value; }
            get { return _is_gh; }
        }
        /// <summary>
        /// 准生儿社会保障卡号
        /// </summary>
        public string ZSRCARDID
        {
            set { _zsrcardid = value; }
            get { return _zsrcardid; }
        }
        /// <summary>
        /// 手术是否成功标志
        /// </summary>
        public string SS_ISSUCCESS
        {
            set { _ss_issuccess = value; }
            get { return _ss_issuccess; }
        }
        /// <summary>
        /// 本次医疗费总额
        /// </summary>
        public string BC_ZJE
        {
            set { _bc_zje = value; }
            get { return _bc_zje; }
        }
        /// <summary>
        /// 本次统筹支付金额
        /// </summary>
        public string BC_TCJE
        {
            set { _bc_tcje = value; }
            get { return _bc_tcje; }
        }
        /// <summary>
        /// 本次大病救助支付
        /// </summary>
        public string BC_DBJZ
        {
            set { _bc_dbjz = value; }
            get { return _bc_dbjz; }
        }
        /// <summary>
        /// 本次大病保险支付
        /// </summary>
        public string BC_DBBX
        {
            set { _bc_dbbx = value; }
            get { return _bc_dbbx; }
        }
        /// <summary>
        /// 本次民政补助支付
        /// </summary>
        public string BC_MZBZ
        {
            set { _bc_mzbz = value; }
            get { return _bc_mzbz; }
        }
        /// <summary>
        /// 本次帐户支付总额
        /// </summary>
        public string BC_ZHZC
        {
            set { _bc_zhzc = value; }
            get { return _bc_zhzc; }
        }
        /// <summary>
        /// 本次现金支付总额
        /// </summary>
        public string BC_XJZC
        {
            set { _bc_xjzc = value; }
            get { return _bc_xjzc; }
        }
        /// <summary>
        /// 本次帐户支付自付
        /// </summary>
        public string BC_ZHZCZF
        {
            set { _bc_zhzczf = value; }
            get { return _bc_zhzczf; }
        }
        /// <summary>
        /// 本次帐户支付自理
        /// </summary>
        public string BC_ZHZCZL
        {
            set { _bc_zhzczl = value; }
            get { return _bc_zhzczl; }
        }
        /// <summary>
        /// 本次现金支付自付
        /// </summary>
        public string BC_XJZCZF
        {
            set { _bc_xjzczf = value; }
            get { return _bc_xjzczf; }
        }
        /// <summary>
        /// 本次现金支付自理
        /// </summary>
        public string BC_XJZCZL
        {
            set { _bc_xjzczl = value; }
            get { return _bc_xjzczl; }
        }
        /// <summary>
        /// 医保范围内费用
        /// </summary>
        public string YBFWNJE
        {
            set { _ybfwnje = value; }
            get { return _ybfwnje; }
        }
        /// <summary>
        /// 帐户消费后余额
        /// </summary>
        public string ZHYE
        {
            set { _zhye = value; }
            get { return _zhye; }
        }
        /// <summary>
        /// 单病种病种编码
        /// </summary>
        public string DBZ_CODE
        {
            set { _dbz_code = value; }
            get { return _dbz_code; }
        }
        /// <summary>
        /// 说明信息
        /// </summary>
        public string INSTRUCTION
        {
            set { _instruction = value; }
            get { return _instruction; }
        }
        /// <summary>
        /// 药费合计
        /// </summary>
        public string MED_JE
        {
            set { _med_je = value; }
            get { return _med_je; }
        }
        /// <summary>
        /// 诊疗项目费合计
        /// </summary>
        public string CHK_JE
        {
            set { _chk_je = value; }
            get { return _chk_je; }
        }
        /// <summary>
        /// 补保支付
        /// </summary>
        public string BB_JE
        {
            set { _bb_je = value; }
            get { return _bb_je; }
        }
        /// <summary>
        /// 备用6
        /// </summary>
        public string BY6
        {
            set { _by6 = value; }
            get { return _by6; }
        }

        /// <summary>
        /// 业务周期号
        /// </summary>
        public string YWZQH
        {
            set { _ywzqh = value; }
            get { return _ywzqh; }
        }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime DEAL_TIME
        {
            set { _deal_time = value; }
            get { return _deal_time; }
        }
        public string SOURCE { get; set; }
        public string USER_ID { get; set; }
        public string lTERMINAL_SN { get; set; }
        #endregion Model
    }
}

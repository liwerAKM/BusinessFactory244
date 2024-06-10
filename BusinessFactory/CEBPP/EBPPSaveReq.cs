using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBPP
{
    /// <summary>
    /// 提交保存电子票据入参
    /// </summary>
    public class EBPPSaveReq
    {

        public EBPPSaveReq()
        {

        }
        public EBPPSaveReq(bool Ini)
        {
            if (Ini)
            {
                patInfo = new Patinfo();
                items = new List<ListInfo>();
                collectInfoGenreal = new ChargeCollectGenreal();
                collectInfoCadre = new ChargeCollectCadre();
                proSettleInfo = new ProSettleInfo();
                proReceiptDetail = new List<ProReceiptDetail>();
                prescriptionDetails = new List<PrescriptionDetail>();
            }

        }
        /// <summary>
        /// String	18	是 医院编号 统一社会信用代码
        /// </summary>
        public string unifiedOrgCode;
        /// <summary>
        /// String	50	是 医院名称
        /// </summary>
        public string orgName;
        /// <summary>
        ///     票据类型	1:门诊挂号电子发票；2:门诊收费电子发票3住院电子发票；4住院电子清单
        /// </summary>
        public int eBPPType;
        /// <summary>
        /// Int 是   支付类型	1、商保 2、医保 3、自费
        /// </summary>
        public int payType;
        /// <summary>
        /// String	30	是 医院内部票据号 发票时为发票号，清单时为住院号
        /// </summary>
        public string billID;
        /// <summary>
        /// String	10	是 发票时为票据代码， 清单时为住院次数
        /// </summary>
        public string billCode;
        /// <summary>
        ///  String	24	是 证件号码(身份证) 新生儿无身份证时填"Newborn"
        /// </summary>
        public string idcardNo;
        /// <summary>
        /// String	14	是 开票时间    yyyyMMddhhMMss
        /// </summary>
        public string openTime;
        /// <summary>
        /// String	200	是 PDF电子发票或清单公网查看地址
        /// </summary>
        public string pDFUrl;
        /// <summary>
        /// String      否 电子清单时必须将电子清单转换为base64上传
        /// </summary>
        private string base64;

        /// <summary>
        ///本次医疗费总额   
        /// </summary>
        public decimal totalFee;
        /// <summary>
        ///1 病人基本信息
        /// </summary>
        public Patinfo patInfo;
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<ListInfo> items;
        /// <summary>
        /// 2 医保（非省医保、市干保、省干保）时必填
        /// </summary>
        public ChargeCollectGenreal collectInfoGenreal;
        /// <summary>
        /// 3 省医保、市干保、省干保时必填，其他时不填
        /// </summary>
        public ChargeCollectCadre collectInfoCadre;

        /// <summary>
        ///  5.proSettleInfo为结算数据
        /// </summary>
        public ProSettleInfo proSettleInfo;
        /// <summary>
        /// 6 proReceiptDetail为按类别汇总
        /// </summary>
        public List<ProReceiptDetail> proReceiptDetail;

        /// <summary>
        /// 7.proReceiptDetail处方
        /// </summary>
        public List<PrescriptionDetail> prescriptionDetails;
     
        public string Base64 { get => base64; set => base64 = value; }
    }
   


    public class Patinfo
    {

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string name;

        /// <summary>
        /// 病人性别
        /// </summary>
        public string sex;

        /// <summary>
        /// 病人年龄
        /// </summary>
        public string age;

        /// <summary>
        ///身份证
        /// </summary>
        public string idcardNo;

        /// <summary>
        ///监护人身份证(当病人为新生儿时无身份证时，监护人身份证不能为空)
        /// </summary>
        public string fidIdcardNo;

        /// <summary>
        ///住院号
        /// </summary>
        public string hosNo;

        /// <summary>
        ///入院科室
        /// </summary>
        public string deptNameIn;

        /// <summary>
        ///入院科室代码
        /// </summary>
        public string deptCodeIn;

        /// <summary>
        ///入院病区
        /// </summary>
        public string bQNameIn;

        /// <summary>
        ///出院科室
        /// </summary>
        public string deptNameOut;

        /// <summary>
        ///出院科室代码
        /// </summary>
        public string deptCodeOut;

        /// <summary>
        ///出院病区
        /// </summary>
        public string bQNameOut;


        /// <summary>
        /// 床号
        /// </summary>
        public string bed_no;


        /// <summary>
        ///入院时间
        /// </summary>
        public string inDate;

        /// <summary>
        ///出院时间
        /// </summary>
        public string outDate;

        /// <summary>
        ///住院天数
        /// </summary>
        public int hLOS;


        /// <summary>
        ///入院诊断
        /// </summary>
        public string inDisName;
        /// <summary>
        ///入院诊断代码
        /// </summary>
        public string inDisCode;

        /// <summary>
        ///出院诊断
        /// </summary>
        public string outDisName;

        /// <summary>
        ///出院诊断代码
        /// </summary>
        public string outDisCode;

        /// <summary>
        ///出院次要诊断
        /// </summary>
        public string outDisName2;

        /// <summary>
        ///出院诊断2代码
        /// </summary>
        public string outDisCode2;

        /// <summary>
        ///出院情况(性质)
        /// </summary>
        public string outStatus;

        /// <summary>
        ///入院方式
        /// </summary>
        public string inWay;

        /// <summary>
        /// 人员性质
        /// </summary>
        public string personnelNature;
        /// <summary>
        /// 单位名称
        /// </summary>
        public string unitName;

        /// <summary>
        ///险种类型
        /// </summary>
        public string insuranceType;

        /// <summary>
        ///医保号码（社会保障卡号）
        /// </summary>
        public string medicalCardNo;

        /// <summary>
        ///参保地
        /// </summary>
        public string insuredSite;


        /// <summary>
        ///本年度住院次数
        /// </summary>
        public string inNumber;


        /// <summary>
        ///医保住院号 仅医保及异地就医用（如不能获取请填住院号）
        /// </summary>
        public string hosNoYb;


        /// <summary>
        ///制表人
        /// </summary>
        public string lister;

        /// <summary>
        /// 制表时间
        /// </summary>
        public string formedTime;

        /// <summary>
        /// string	30	是 医生编码
        /// </summary>
        public string doc_no;

        /// <summary>
        /// string	30	是 医生姓名
        /// </summary>
        public string doc_name;

        /// <summary>
        ///  string	10	是 证件类型
        /// </summary>
        public string credentialType;


        /// <summary>
        ///  string	50	是 证件号码
        /// </summary>
        public string credentialNum;

        /// <summary>
        ///  String	50	是 就诊时间（住院为入院时间）
        /// </summary>
        public string jz_time;

        /// <summary>
        /// String 	50	否 医保结算流水号
        /// </summary>
        public string medicalLnsuranceNo;

        /// <summary>
        /// string	50	否 医保结算出参
        /// </summary>
        public string medical_out;

        /// <summary>
        ///  String	30	否 社保类别原始值
        /// </summary>
        public string originalSocialInsurType;


    }
    public class PatinfoAD
    {

        public PatinfoAD()
        {
        }

        public PatinfoAD(Patinfo patinfo)
        {
            this.a = patinfo.age;
            this.b = patinfo.bed_no;
            this.bI = patinfo.bQNameIn;
            this.bO = patinfo.bQNameOut;
            this.cN = patinfo.credentialNum;
            this.cT = patinfo.credentialType;
            this.dI = patinfo.deptCodeIn;
            this.dC = patinfo.deptCodeOut;
            this.d = patinfo.deptNameIn;
            this.dO = patinfo.deptNameOut;
            this.dn = patinfo.doc_name;
            this.dc = patinfo.doc_no;
            this.f = patinfo.fidIdcardNo;
            this.fT = patinfo.formedTime;
            this.hL = patinfo.hLOS;
            this.h = patinfo.hosNo;
            this.hY = patinfo.hosNoYb;
            this.i = patinfo.idcardNo;
            this.iD = patinfo.inDate;
            this.iC = patinfo.inDisCode;
            this.iN = patinfo.inDisName;
            this.ir = patinfo.inNumber;
            this.iT = patinfo.insuranceType;
            this.ii = patinfo.insuredSite;
            this.iW = patinfo.inWay;
            this.j = patinfo.jz_time;
            this.li = patinfo.lister;
            this.m = patinfo.medicalCardNo;
            this.mo = patinfo.medicalLnsuranceNo;
            this.mt = patinfo.medical_out;
            this.n = patinfo.name;
            this.oT = patinfo.originalSocialInsurType;
            this.oD = patinfo.outDate;
            this.oC = patinfo.outDisCode;
            this.oC2 = patinfo.outDisCode2;
            this.oN = patinfo.outDisName;
            this.oN2 = patinfo.outDisName2;
            this.oS = patinfo.outStatus;
            this.pN = patinfo.personnelNature;
            this.s = patinfo.sex;
            this.uN = patinfo.unitName;
        }
        public Patinfo GetPatinfo()
        {
            Patinfo patinfo = new Patinfo();
            patinfo.age = this.a;
            patinfo.bed_no = this.b;
            patinfo.bQNameIn = this.bI;
            patinfo.bQNameOut = this.bO;
            patinfo.credentialNum = this.cN;
            patinfo.credentialType = this.cT;
            patinfo.deptCodeIn = this.dI;
            patinfo.deptCodeOut = this.dC;
            patinfo.deptNameIn = this.d;
            patinfo.deptNameOut = this.dO;
            patinfo.doc_name = this.dn;
            patinfo.doc_no = this.dc;
            patinfo.fidIdcardNo = this.f;
            patinfo.formedTime = this.fT;
            patinfo.hLOS = this.hL;
            patinfo.hosNo = this.h;
            patinfo.hosNoYb = this.hY;
            patinfo.idcardNo = this.i;
            patinfo.inDate = this.iD;
            patinfo.inDisCode = this.iC;
            patinfo.inDisName = this.iN;
            patinfo.inNumber = this.ir;
            patinfo.insuranceType = this.iT;
            patinfo.insuredSite = this.ii;
            patinfo.inWay = this.iW;
            patinfo.jz_time = this.j;
            patinfo.lister = this.li;
            patinfo.medicalCardNo = this.m;
            patinfo.medicalLnsuranceNo = this.mo;
            patinfo.medical_out = this.mt;
            patinfo.name = this.n;
            patinfo.originalSocialInsurType = this.oT;
            patinfo.outDate = this.oD;
            patinfo.outDisCode = this.oC;
            patinfo.outDisCode2 = this.oC2;
            patinfo.outDisName = this.oN;
            patinfo.outDisName2 = this.oN2;
            patinfo.outStatus = this.oS;
            patinfo.personnelNature = this.pN;
            patinfo.sex = this.s;
            patinfo.unitName = this.uN;

            return patinfo;
        }

        /// <summary>
        /// 病人姓名name
        /// </summary>
        public string n;

        /// <summary>
        /// 病人性别sex
        /// </summary>
        public string s;

        /// <summary>
        /// 病人年龄age
        /// </summary>
        public string a;

        /// <summary>
        ///身份证idcardNo
        /// </summary>
        public string i;

        /// <summary>
        ///监护人身份证(当病人为新生儿时无身份证时，监护人身份证不能为空)fidIdcardNo
        /// </summary>
        public string f;

        /// <summary>
        ///住院号hosNo
        /// </summary>
        public string h;

        /// <summary>
        ///入院科室deptNameIn
        /// </summary>
        public string d;

        /// <summary>
        ///入院科室代码deptCodeIn
        /// </summary>
        public string dI;

        /// <summary>
        ///入院病区bQNameIn
        /// </summary>
        public string bI;

        /// <summary>
        ///出院科室deptNameOut
        /// </summary>
        public string dO;

        /// <summary>
        ///出院科室代码deptCodeOut
        /// </summary>
        public string dC;

        /// <summary>
        ///出院病区bQNameOut
        /// </summary>
        public string bO;


        /// <summary>
        /// 床号bed_no
        /// </summary>
        public string b;


        /// <summary>
        ///入院时间inDate
        /// </summary>
        public string iD;

        /// <summary>
        ///出院时间outDate
        /// </summary>
        public string oD;

        /// <summary>
        ///住院天数hLOS
        /// </summary>
        public int hL;


        /// <summary>
        ///入院诊断inDisName
        /// </summary>
        public string iN;
        /// <summary>
        ///入院诊断代码inDisCode
        /// </summary>
        public string iC;

        /// <summary>
        ///出院诊断outDisName
        /// </summary>
        public string oN;

        /// <summary>
        ///出院诊断代码outDisCode
        /// </summary>
        public string oC;

        /// <summary>
        ///出院次要诊断outDisName2
        /// </summary>
        public string oN2;

        /// <summary>
        ///出院诊断2代码outDisCode2
        /// </summary>
        public string oC2;

        /// <summary>
        ///出院情况(性质)outStatus
        /// </summary>
        public string oS;

        /// <summary>
        ///入院方式inWay
        /// </summary>
        public string iW;

        /// <summary>
        /// 人员性质personnelNature
        /// </summary>
        public string pN;
        /// <summary>
        /// 单位名称unitName
        /// </summary>
        public string uN;

        /// <summary>
        ///险种类型insuranceType
        /// </summary>
        public string iT;

        /// <summary>
        ///医保号码（社会保障卡号）medicalCardNo
        /// </summary>
        public string m;

        /// <summary>
        ///参保地insuredSite
        /// </summary>
        public string ii;


        /// <summary>
        ///本年度住院次数inNumber
        /// </summary>
        public string ir;


        /// <summary>
        ///医保住院号 仅医保及异地就医用（如不能获取请填住院号）hosNoYb
        /// </summary>
        public string hY;


        /// <summary>
        ///制表人hosNoYb
        /// </summary>
        public string li;

        /// <summary>
        /// 制表时间formedTime
        /// </summary>
        public string fT;

        /// <summary>
        /// string	30	是 医生编码doc_no
        /// </summary>
        public string dc;

        /// <summary>
        /// string	30	是 医生姓名doc_name
        /// </summary>
        public string dn;

        /// <summary>
        ///  string	10	是 证件类型credentialType
        /// </summary>
        public string cT;


        /// <summary>
        ///  string	50	是 证件号码credentialNum
        /// </summary>
        public string cN;

        /// <summary>
        ///  String	50	是 就诊时间（住院为入院时间）jz_time
        /// </summary>
        public string j;

        /// <summary>
        /// String 	50	否 医保结算流水号medicalLnsuranceNo
        /// </summary>
        public string mo;

        /// <summary>
        /// string	50	否 医保结算出参medical_out
        /// </summary>
        public string mt;

        /// <summary>
        ///  String	30	否 社保类别原始值originalSocialInsurType
        /// </summary>
        public string oT;


    }

    public class ListInfo
    {
        /// <summary>
        /// 排序标记(如为空则按照上传顺序)
        /// </summary>
        public string order;
        /// <summary>
        /// 项目类别 ItemType
        /// </summary>
        public string iT;

        /// <summary>
        /// 项目类别 ItemTypeCode 江苏医疗收费财务分类代码表
        /// </summary>
        public string iTC;

        /// <summary>
        /// HIS项目自编码
        /// </summary>
        public string iBM;


        /// <summary>
        /// 项目名称ItemName
        /// </summary>
        public string iN;

        /// <summary>
        /// 项目规格ItemGG
        /// </summary>
        public string iG;

        /// <summary>
        /// 项目单位 ItemUnit
        /// </summary>
        public string iU;

        /// <summary>
        /// 项目单价ItemPrice
        /// </summary>
        public decimal iP;

        /// <summary>
        /// 项目数量ItemCount
        /// </summary>
        public decimal iC;
        /// <summary>
        /// 项目金额ItemJe
        /// </summary>
        public decimal iJ;

        /// <summary>
        /// 自付比例SelfRate(自费病人可不提供)
        /// </summary>
        public decimal sR;

        /// <summary>
        /// 支付上限 UpLimit(自费病人或省干部、省医保、市干保可不提供)
        /// </summary>
        public string uL;


        /// <summary>
        ///自付金额SelfFee(自费病人可不提供)
        /// </summary>
        public decimal sF;

        /// <summary>
        ///自理金额DeductFee(自费病人可不提供)
        /// </summary>
        public decimal dF;
        /// <summary>
        /// Int     是 目录类别	1	药品2	诊疗项目3	服务设施4	医用材料
        /// </summary>
        public int listCat;
        /// <summary>
        /// String	20	是 处方号
        /// </summary>
        public string recipeNum;
        /// <summary>
        /// String	20	是 处方流水号
        /// </summary>
        public string recipeSerialNum;
        /// <summary>
        ///  String	20	是 处方/医嘱项目代码	00323
        /// </summary>
        public string prescriptionitemsCode;
        /// <summary>
        /// String	50	是 处方/医嘱项目名称 肝功能
        /// </summary>
        public string prescriptionitemsName;

        /// <summary>
        /// String	20	是 费用日期
        /// </summary>
        public string recipeDate;
        /// <summary>
        ///  String	20	是 医保收费项目编码
        /// </summary>
        public string centreChargeCode;
        /// <summary>
        ///  String	100	是 医保收费项目名称
        /// </summary>
        public string medicareFeeitemName;
        /// <summary>
        ///  String	10	是 剂型  药品非空
        /// </summary>
        public string formulation;
        /// <summary>
        /// String	10	是 科室编码    费用开单科室编码
        /// </summary>
        public string deptNum;
        /// <summary>
        ///    String	15	是 科室名称    费用开单科室名称
        /// </summary>
        public string deptName;
        /// <summary>
        ///   String	10	是 处方医生编码  费用开单医生编码
        /// </summary>
        public string doctorCode;
        /// <summary>
        ///  String	10	是 处方医生姓名  费用开单医生名称
        /// </summary>
        public string doctorName;
        /// <summary>
        ///    String	10	是 经办人 记账操作员姓名，为空时取开单医生姓名
        /// </summary>
        public string updateBy;

        /// <summary>
        ///String	15 是 执行科室
        /// </summary>
        public string execDeptName;

        /// <summary>
        ///   Int 是   收费项目等级	
        /// </summary>
        public int itemLevel;
        /// <summary>
        /// 	是 费用明细ID  对应HIS收费明细ID，his系统中收费明细的唯一主键；
        /// </summary>
        public string itemId;
        /// <summary>
        ///  是   医嘱类别	1长期 2临时 ；默认为2
        /// </summary>
        public int orderType;
        /// <summary>
        /// String	20	是 收费项目类别名称    HIS发票分类项目名称，必须与发票一致
        /// </summary>
        public string medicalItemCatName;
        /// <summary>
        /// String	20	是 结算流水号
        /// </summary>
        public string billNum;

    }

    public class ListInfoAD
    {
        public ListInfoAD()
        {
        }

        public ListInfoAD(ListInfo info)
        {
            this.bn = info.billNum;
            this.cc = info.centreChargeCode;
            this.da = info.deptName;
            this.du = info.deptNum;
            this.dF = info.dF;
            this.dc = info.doctorCode;
            this.dm = info.doctorName;
            this.ed = info.execDeptName;
            this.f = info.formulation;
            this.iBM = info.iBM;
            this.iC = info.iC;
            this.iG = info.iG;
            this.iJ = info.iJ;
            this.iN = info.iN;
            this.iP = info.iP;
            this.iT = info.iT;
            this.iTC = info.iTC;
            this.iU = info.iU;
            this.sF = info.sF;
            this.sR = info.sR;
            this.uL = info.uL;
            this.id = info.itemId;
            this.il = info.itemLevel;
            this.listCat = info.listCat;
            this.mi = info.medicalItemCatName;
            this.mN = info.medicareFeeitemName;
            this.o = info.order;
            this.ot = info.orderType;
            this.pc = info.prescriptionitemsCode;
            this.pn = info.prescriptionitemsName;
            this.rd = info.recipeDate;
            this.recipeNum = info.recipeNum;
            this.r = info.recipeSerialNum;
            this.ub = info.updateBy;

        }

        public ListInfo GetListInfo()
        {
            ListInfo info = new ListInfo();
            info.billNum = this.bn;
            info.centreChargeCode = this.cc;
            info.deptName = this.da;
            info.deptNum = this.du;
            info.dF = this.dF;
            info.doctorCode = this.dc;
            info.doctorName = this.dm;
            info.execDeptName = this.ed;
            info.formulation = this.f;
            info.iBM = this.iBM;
            info.iC = this.iC;
            info.iG = this.iG;
            info.iJ = this.iJ;
            info.iN = this.iN;
            info.iP = this.iP;
            info.iT = this.iT;
            info.iTC = this.iTC;
            info.iU = this.iU;
            info.sF = this.sF;
            info.sR = this.sR;
            info.uL = this.uL;
            info.itemId = this.id;
            info.itemLevel = this.il;
            info.listCat = this.listCat;
            info.medicalItemCatName = this.mi;
            info.medicareFeeitemName = this.mN;
            info.order = this.o;
            info.orderType = this.ot;
            info.prescriptionitemsCode = this.pc;
            info.prescriptionitemsName = this.pn;
            info.recipeDate = this.rd;
            info.recipeNum = this.recipeNum;
            info.recipeSerialNum = this.r;
            info.updateBy = this.ub;
            return info;
        }

        /// <summary>
        /// 排序标记(如为空则按照上传顺序) order
        /// </summary>
        public string o;
        /// <summary>
        /// 项目类别 ItemType
        /// </summary>
        public string iT;

        /// <summary>
        /// 项目类别 ItemTypeCode 江苏医疗收费财务分类代码表
        /// </summary>
        public string iTC;

        /// <summary>
        /// HIS项目自编码
        /// </summary>
        public string iBM;


        /// <summary>
        /// 项目名称ItemName
        /// </summary>
        public string iN;

        /// <summary>
        /// 项目规格ItemGG
        /// </summary>
        public string iG;

        /// <summary>
        /// 项目单位 ItemUnit
        /// </summary>
        public string iU;

        /// <summary>
        /// 项目单价ItemPrice
        /// </summary>
        public decimal iP;

        /// <summary>
        /// 项目数量ItemCount
        /// </summary>
        public decimal iC;
        /// <summary>
        /// 项目金额ItemJe
        /// </summary>
        public decimal iJ;

        /// <summary>
        /// 自付比例SelfRate(自费病人可不提供)
        /// </summary>
        public decimal sR;

        /// <summary>
        /// 支付上限 UpLimit(自费病人或省干部、省医保、市干保可不提供)
        /// </summary>
        public string uL;


        /// <summary>
        ///自付金额SelfFee(自费病人可不提供)
        /// </summary>
        public decimal sF;

        /// <summary>
        ///自理金额DeductFee(自费病人可不提供)
        /// </summary>
        public decimal dF;
        /// <summary>
        /// Int     是 目录类别	1	药品2	诊疗项目3	服务设施4	医用材料
        /// </summary>
        public int listCat;
        /// <summary>
        /// String	20	是 处方号
        /// </summary>
        public string recipeNum;
        /// <summary>
        /// String	20	是 处方流水号recipeSerialNum
        /// </summary>
        public string r;
        /// <summary>
        ///  String	20	是 处方/医嘱项目代码	00323prescriptionitemsCode
        /// </summary>
        public string pc;
        /// <summary>
        /// String	50	是 处方/医嘱项目名称 肝功能prescriptionitemsName
        /// </summary>
        public string pn;

        /// <summary>
        /// String	20	是 费用日期recipeDate
        /// </summary>
        public string rd;
        /// <summary>
        ///  String	20	是 医保收费项目编码centreChargeCode
        /// </summary>
        public string cc;
        /// <summary>
        ///  String	100	是 医保收费项目名称medicareFeeitemName
        /// </summary>
        public string mN;
        /// <summary>
        ///  String	10	是 剂型  药品非空formulation
        /// </summary>
        public string f;
        /// <summary>
        /// String	10	是 科室编码    费用开单科室编码deptNum
        /// </summary>
        public string du;
        /// <summary>
        ///    String	15	是 科室名称    费用开单科室名称deptName
        /// </summary>
        public string da;
        /// <summary>
        ///   String	10	是 处方医生编码  费用开单医生编码doctorCode
        /// </summary>
        public string dc;
        /// <summary>
        ///  String	10	是 处方医生姓名  费用开单医生名称doctorName
        /// </summary>
        public string dm;
        /// <summary>
        ///    String	10	是 经办人 记账操作员姓名，为空时取开单医生姓名updateBy
        /// </summary>
        public string ub;

        /// <summary>
        ///String	15 是 执行科室execDeptName
        /// </summary>
        public string ed;

        /// <summary>
        ///   Int 是   收费项目等级	itemLevel
        /// </summary>
        public int il;
        /// <summary>
        /// 	是 费用明细ID  对应HIS收费明细ID，his系统中收费明细的唯一主键；itemId
        /// </summary>
        public string id;
        /// <summary>
        ///  是   医嘱类别	1长期 2临时 ；默认为2orderType
        /// </summary>
        public int ot;
        /// <summary>
        /// String	20	是 收费项目类别名称    HIS发票分类项目名称，必须与发票一致medicalItemCatName
        /// </summary>
        public string mi;
        /// <summary>
        /// String	20	是 结算流水号billNum
        /// </summary>
        public string bn;

    }

    /// <summary>
    ///普通医保
    /// </summary>
    public class ChargeCollectGenreal
    {

        /// <summary>
        ///本次医疗费总额
        /// </summary>
        public decimal TotalMoney;
        /// <summary>
        ///统筹支付金额
        /// </summary>
        public decimal CommonNum;
        /// <summary>
        ///大病支付
        /// </summary>
        public decimal seriousNum;
        /// <summary>
        ///大病保险
        /// </summary>
        public decimal seriousInsuranceNum;
        /// <summary>
        ///民政补助金额（民政救助）
        /// </summary>
        public decimal civilAssisNum;
        /// <summary>
        /// 公务员补助支付金额(异地)
        /// </summary>
        public decimal civilServantNum;

        /// <summary>
        ///个人帐户支付总额=<see cref="personAccountPayNum"/>+<see cref="personAccountCareNum"/>
        /// </summary>
        public decimal personAccountNum;

        /// <summary>
        ///现金支付总额=<see cref="CashPayNum"/>+<see cref="CashCareNum"/>
        /// </summary>
        public decimal CashNum;

        /// <summary>
        ///本次帐户支付自付
        /// </summary>
        public decimal personAccountPayNum;

        /// <summary>
        ///本次帐户支付自理
        /// </summary>
        public decimal personAccountCareNum;

        /// <summary>
        ///本次现金支付自付
        /// </summary>
        public decimal CashPayNum;
        /// <summary>
        ///本次现金支付自理
        /// </summary>
        public decimal CashCareNum;

        /// <summary>
        ///医保范围内费用 
        /// </summary>
        public decimal CoverageNum;

        /// <summary>
        ///个人自付金额=<see cref="personAccountPayNum"/>+<see cref="CashPayNum"/>
        /// </summary>
        public decimal personPay;

        /// <summary>
        ///个人自理金额=<see cref="personAccountCareNum"/>+<see cref="CashCareNum"/>
        /// </summary>
        public decimal personCare;

        /// <summary>
        /// 报销费用合计=<see cref="TotalMoney"/>-<see cref="personAccountNum"/>-<see cref="CashNum"/>
        /// </summary>
        /// <returns></returns>
        public decimal ReimburseNum()
        {
            return TotalMoney - personAccountNum - CashNum;
        }
        /// <summary>
        /// 入院时账户余额
        /// </summary>
        public decimal InBalance;

        /// <summary>
        /// 出院时账户余额
        /// </summary>
        public decimal OutBalance;

        /// <summary>
        /// 起付标准（异地就医）
        /// </summary>
        public decimal PayLevel;


        /// <summary>
        /// 如果发票上部分项目不上传医保，请将对应金额填在此处.关系zf+ totalMoney= 根节点totalFee
        /// </summary>
        public decimal zf;

        /// <summary>
        /// 扩展数据
        /// </summary>
        public ChargeCollectGenrealExtend Extend;

    }


    /// <summary>
    ///普通医保
    /// </summary>
    public class ChargeCollectGenrealAD
    {

        public ChargeCollectGenrealAD()
        {
        }

        public ChargeCollectGenrealAD(ChargeCollectGenreal charge)
        {
            this.CC = charge.CashCareNum;
            this.ca = charge.CashNum;
            this.CP = charge.CashPayNum;
            this.ci = charge.civilAssisNum;
            this.cs = charge.civilServantNum;
            this.co = charge.CommonNum;
            this.CN = charge.CoverageNum;
            if (charge.Extend != null)
                this.Ex = new ChargeCollectGenrealExtendAD(charge.Extend);
            this.IB = charge.InBalance;
            this.OB = charge.OutBalance;
            this.pl = charge.PayLevel;
            this.pAC = charge.personAccountCareNum;
            this.pA = charge.personAccountNum;
            this.pAP = charge.personAccountPayNum;
            this.pC = charge.personCare;
            this.pP = charge.personPay;
            this.si = charge.seriousInsuranceNum;
            this.sn = charge.seriousNum;
            this.tm = charge.TotalMoney;
            this.zf  = charge.zf;

        }

        public ChargeCollectGenreal GetChargeCollectGenreal()
        {
            ChargeCollectGenreal charge = new ChargeCollectGenreal();
            charge.CashCareNum = this.CC;
            charge.CashNum = this.ca;
            charge.CashPayNum = this.CP;
            charge.civilAssisNum = this.ci;
            charge.civilServantNum = this.cs;
            charge.CommonNum = this.co;
            charge.CoverageNum = this.CN;
            if (this.Ex != null)
                charge.Extend = this.Ex.GetChargeCollectGenrealExtend();
            charge.InBalance = this.IB;
            charge.OutBalance = this.OB;
            charge.PayLevel = this.pl;
            charge.personAccountCareNum = this.pAC;
            charge.personAccountNum = this.pA;
            charge.personAccountPayNum = this.pAP;
            charge.personCare = this.pC;
            charge.personPay = this.pP;
            charge.seriousInsuranceNum = this.si;
            charge.seriousNum = this.sn;
            charge.TotalMoney = this.tm;
            charge.zf = this.zf;
            return charge;
        }

        /// <summary>
        ///本次医疗费总额TotalMoney
        /// </summary>
        public decimal tm;
        /// <summary>
        ///统筹支付金额CommonNum
        /// </summary>
        public decimal co;
        /// <summary>
        ///大病支付seriousNum
        /// </summary>
        public decimal sn;
        /// <summary>
        ///大病保险seriousInsuranceNum
        /// </summary>
        public decimal si;
        /// <summary>
        ///民政补助金额（民政救助）civilAssisNum
        /// </summary>
        public decimal ci;
        /// <summary>
        /// 公务员补助支付金额(异地)civilServantNum
        /// </summary>
        public decimal cs;

        /// <summary>
        ///个人帐户支付总额=<see cref="pAP"/>+<see cref="pAC"/>personAccountNum
        /// </summary>
        public decimal pA;

        /// <summary>
        ///现金支付总额=<see cref="CP"/>+<see cref="CC"/>  CashNum
        /// </summary>
        public decimal ca;

        /// <summary>
        ///本次帐户支付自付personAccountPayNum
        /// </summary>
        public decimal pAP;

        /// <summary>
        ///本次帐户支付自理personAccountCareNum
        /// </summary>
        public decimal pAC;

        /// <summary>
        ///本次现金支付自付CashPayNum
        /// </summary>
        public decimal CP;
        /// <summary>
        ///本次现金支付自理CashCareNum
        /// </summary>
        public decimal CC;

        /// <summary>
        ///医保范围内费用 CoverageNum
        /// </summary>
        public decimal CN;

        /// <summary>
        ///个人自付金额=<see cref="pAP"/>+<see cref="CP"/>personPay
        /// </summary>
        public decimal pP;

        /// <summary>
        ///个人自理金额=<see cref="pAC"/>+<see cref="CC"/>personCare
        /// </summary>
        public decimal pC;

        /// <summary>
        /// 报销费用合计=<see cref="tm"/>-<see cref="pA"/>-<see cref="ca"/>
        /// </summary>
        /// <returns></returns>
        public decimal ReimburseNum()
        {
            return tm - pA - ca;
        }
        /// <summary>
        /// 入院时账户余额InBalance
        /// </summary>
        public decimal IB;

        /// <summary>
        /// 出院时账户余额OutBalance
        /// </summary>
        public decimal OB;

        /// <summary>
        /// 起付标准（异地就医）PayLevel
        /// </summary>
        public decimal pl;
        /// <summary>
        /// 如果发票上部分项目不上传医保，请将对应金额填在此处.关系zf+ totalMoney= 根节点totalFee
        /// </summary>
        public decimal zf;

        /// <summary>
        /// 扩展数据
        /// </summary>
        public ChargeCollectGenrealExtendAD Ex;

    }
    /// <summary>
    /// 本地普通医保扩展数据
    /// </summary>
    public class ChargeCollectGenrealExtend
    {

        /// <summary>
        ///年度累计已支付统筹支付金额
        /// </summary>
        public decimal YearCommonNum;
        /// <summary>
        ///年度累计已支付大病救助支付
        /// </summary>
        public decimal YearseriousNum;
        /// <summary>
        ///年度累计已支付大病保险
        /// </summary>
        public decimal YearseriousInsuranceNum;
        /// <summary>
        ///年度累计已支付民政补助金额（民政救助）
        /// </summary>
        public decimal YearcivilAssisNum;

        /// <summary>
        ///年度累计已支付个人自付金额
        /// </summary>
        public decimal personPay;
        public List<ChargeCollectGenrealExtendStandard> StandardList;
    }
    /// <summary>
    /// 本地普通医保扩展数据
    /// </summary>
    public class ChargeCollectGenrealExtendAD
    {

        public ChargeCollectGenrealExtendAD()
        {
        }
        public ChargeCollectGenrealExtendAD(ChargeCollectGenrealExtend charge)
        {
            if (charge != null)
            {
                this.p = charge.personPay;
                this.li = charge.StandardList;
                this.a = charge.YearcivilAssisNum;
                this.c = charge.YearCommonNum;
                this.i = charge.YearseriousInsuranceNum;
                this.s = charge.YearseriousNum;
            }

        }

        public ChargeCollectGenrealExtend GetChargeCollectGenrealExtend()
        {
            ChargeCollectGenrealExtend charge = new ChargeCollectGenrealExtend();
            charge.personPay = this.p;
            charge.StandardList = this.li;
            charge.YearcivilAssisNum = this.a;
            charge.YearCommonNum = this.c;
            charge.YearseriousInsuranceNum = this.i;
            charge.YearseriousNum = this.s;
            return charge;
        }

        /// <summary>
        ///年度累计已支付统筹支付金额YearCommonNum
        /// </summary>
        public decimal c;
        /// <summary>
        ///年度累计已支付大病救助支付YearseriousNum
        /// </summary>
        public decimal s;
        /// <summary>
        ///年度累计已支付大病保险YearseriousInsuranceNum
        /// </summary>
        public decimal i;
        /// <summary>
        ///年度累计已支付民政补助金额（民政救助）YearcivilAssisNum
        /// </summary>
        public decimal a;

        /// <summary>
        ///年度累计已支付个人自付金额personPay
        /// </summary>
        public decimal p;
        public List<ChargeCollectGenrealExtendStandard> li;
    }
    /// <summary>
    /// 本地普通医保扩展数据 费用情况
    /// </summary>
    public class ChargeCollectGenrealExtendStandard
    {
        /// <summary>
        /// 名称编码 A 起付标准；B 统筹基金最高支付限额 ;C 统筹基金最高支付限额以上部分;D 范围内个人自付超过0万元以上部分...
        /// </summary>
        public string NameCode;

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Je;

        /// <summary>
        /// 报销比例
        /// </summary>
        public decimal ReimRate;

        /// <summary>
        /// 报销费用
        /// </summary>
        public decimal ReimJe;
    }

    /// <summary>
    ///干部医保(省医保、南京市干保、江苏省干保)
    /// </summary>
    public class ChargeCollectCadre
    {

        /// <summary>
        ///本次医疗费总额   = <see cref="CommonNum"/>+<see cref="HelpPay"/> +<see cref="SupplyPay"/>+ <see cref="OtrPay"/> + <see cref="MedAccPay"/>  + <see cref="BankAccPay"/> + <see cref="CashPay"/>   
        /// </summary>
        public decimal TotalMoney;
        /// <summary>
        ///统筹支付
        /// </summary>
        public decimal CommonNum;
        /// <summary>
        ///3个范围内费用
        /// </summary>
        public decimal GenFee;

        /// <summary>
        ///自付费用
        /// </summary>
        public decimal FirstPay;

        /// <summary>
        ///自费费用
        /// </summary>
        public decimal SelfFee;
        /// <summary>
        ///统筹个人自付
        /// </summary>
        public decimal PubSelf;

        /// <summary>
        ///大病救助基金支付
        /// </summary>
        public decimal HelpPay;

        /// <summary>
        ///大病救助基金个人自付
        /// </summary>
        public decimal HelpSelf;

        /// <summary>
        ///公务员补充支付/企业补充支付
        /// </summary>
        public decimal SupplyPay;

        /// <summary>
        ///公务员补充支付/企业补充支付 个人自付
        /// </summary>
        public decimal SupplySelf;

        /// <summary>
        ///其它基金支付
        /// </summary>
        public decimal OtrPay;


        /// <summary>
        ///个人医疗帐户支付
        /// </summary>
        public decimal MedAccPay;

        /// <summary>
        ///个人储蓄帐户支付
        /// </summary>
        public decimal BankAccPay;

        /// <summary>
        ///现金支付
        /// </summary>
        public decimal CashPay;


        /// <summary>
        /// 起付标准
        /// </summary>
        public decimal PayLevel;


        /// <summary>
        ///个人负担
        /// </summary>
        public decimal PersonBurden()
        {
            return MedAccPay + BankAccPay + CashPay;
        }
        /// <summary>
        ///报销范围内容
        /// </summary>
        public decimal Peimbursable()
        {
            return TotalMoney - PersonBurden();
        }


        /// <summary>
        /// 如果发票上部分项目不上传医保，请将对应金额填在此处.关系zf+ totalMoney= 根节点totalFee
        /// </summary>
        public decimal zf;
    }

    /// <summary>
    /// 5.proSettleInfo为结算数据
    /// </summary>
    public class ProSettleInfo
    {
        /// <summary>
        /// String	20	是 单据号
        /// </summary>
        public string medicalBillingNo;
        /// <summary>
        /// String	20	是 发票号
        /// </summary>
        public string invoiceNO;
        /// <summary>
        /// 	20	是 HIS结算流水号
        /// </summary>
        public string billNum;
        /// <summary>
        /// 是 中途结算标志	0 非 1为中途结算
        /// </summary>
        public int isHalfSettle;
        /// <summary>
        ///   String	20	是 结算日期
        /// </summary>
        public string settleDate;
        /// <summary>
        /// String	15	是 经办人
        /// </summary>
        public string updatedBy;
        /// <summary>
        /// String	20	是 医保结算流水号
        /// </summary>
        public string medicalLnsuranceNo;
        /// <summary>
        /// String	20	是 业务流水号
        /// </summary>
        public string businessNo;
        /// <summary>
        ///  String      是 人群类型	1	普通门诊2	门重门慢
        ///3	门诊急救
        ///4	职保
        ///5	工伤
        ///6	生育 
        ///7	离休
        ///8	居保
        ///9	生育检查
        ///10	生育流产
        ///11	大学生
        ///12	男生育
        ///13	职工公费医疗
        ///21	普通住院
        /// </summary>
        public string personType;
        /// <summary>
        /// 22	是 支付类型	1自费;2本地医保;9异地医保;11省医保;12省干保;13市干保
        /// </summary>
        public string payType;

        /// <summary>
        ///  20	是 人员待遇
        /// </summary>
        public string treatment;


        /// <summary>
        ///  String	1000		医保结算数据 医保结算返回的原始字符串，医保必
        /// </summary>
        public string mEDICALLNSURANCE;

       
        /// <summary>
        /// 一次医保分成多张发票时，发票列表
        /// </summary>
        public List<BillIDSItem> billIDS { get; set; }
    }

    /// <summary>
    /// 5.proSettleInfo为结算数据
    /// </summary>
    public class ProSettleInfoAD
    {

        public ProSettleInfoAD()
        {
        }
        public ProSettleInfoAD(ProSettleInfo info)
        {
            this.b = info.billNum;
            this.bn = info.businessNo;
            this.i = info.invoiceNO;
            this.ih = info.isHalfSettle;
            this.m = info.medicalBillingNo;
            this.mE = info.mEDICALLNSURANCE;
            this.mm = info.medicalLnsuranceNo;
            this.pt = info.payType;
            this.pe = info.personType;
            this.sd = info.settleDate;
            this.t = info.treatment;
            this.ub = info.updatedBy;
            this.bS = info.billIDS;
        }

        public ProSettleInfo GetProSettleInfo()
        {
            ProSettleInfo info = new ProSettleInfo();
            info.billNum = this.b;
            info.businessNo = this.bn;
            info.invoiceNO = this.i;
            info.isHalfSettle = this.ih;
            info.medicalBillingNo = this.m;
            info.mEDICALLNSURANCE = this.mE;
            info.medicalLnsuranceNo = this.mm;
            info.payType = this.pt;
            info.personType = this.pe;
            info.settleDate = this.sd;
            info.treatment = this.t;
            info.updatedBy = this.ub;
            info.billIDS = this.bS;
           
            return info;
        }
        /// <summary>
        /// String	20	是 单据号medicalBillingNo
        /// </summary>
        public string m;
        /// <summary>
        /// String	20	是 发票号invoiceNO
        /// </summary>
        public string i;
        /// <summary>
        /// 	20	是 HIS结算流水号billNum
        /// </summary>
        public string b;
        /// <summary>
        /// 是 中途结算标志	0 非 1为中途结算isHalfSettle
        /// </summary>
        public int ih;
        /// <summary>
        ///   String	20	是 结算日期settleDate
        /// </summary>
        public string sd;
        /// <summary>
        /// String	15	是 经办人updatedBy
        /// </summary>
        public string ub;
        /// <summary>
        /// String	20	是 医保结算流水号medicalLnsuranceNo
        /// </summary>
        public string mm;
        /// <summary>
        /// String	20	是 业务流水号businessNo
        /// </summary>
        public string bn;
        /// <summary>
        ///  String      是 人群类型	1	普通门诊2	门重门慢personType
        ///4	职保
        ///5	工伤
        ///6	生育 
        ///7	离休
        ///8	居保
        ///9	生育检查
        ///10	生育流产
        ///11	大学生
        ///12	男生育
        ///13	职工公费医疗
        ///21	普通住院
        /// </summary>
        public string pe;
        /// <summary>
        /// 22	是 支付类型	1自费;2本地医保;9异地医保;11省医保;12省干保;13市干保payType
        /// </summary>
        public string pt;

        /// <summary>
        ///  20	是 人员待遇treatment
        /// </summary>
        public string t;


        /// <summary>
        ///  String	1000		医保结算数据 医保结算返回的原始字符串，医保必mEDICALLNSURANCE
        /// </summary>
        public string mE;
        /// <summary>
        /// 一次医保分成多张发票时，发票列表
        /// </summary>
        public List<BillIDSItem> bS { get; set; }

    }

    /// <summary>
    /// 一次医保分成多张发票时，发票列表
    /// </summary>
    public class BillIDSItem
    {
        /// <summary>
        /// String	30	是 医院内部票据号 发票时为发票号 
        /// </summary>
        public string billID { get; set; }
        /// <summary>
        ///票据代码
        /// </summary>
        public string billCode { get; set; }

        /// <summary>
        ///本发票对应金额   
        /// </summary>
        public decimal totalFee;
    }

 

    /// <summary>
    /// 6.proReceiptDetail为按类别汇总
    /// </summary>
    public class ProReceiptDetail
    {/// <summary>
     /// String	20	是 医院his内部项目类别代码	0001
     /// </summary>
        public string dictionaryCode;
        /// <summary>
        /// String	20	是 医院his内部项目类别名称   西药费
        /// </summary>
        public string dictionaryName;
        /// <summary>
        /// String	15	是 项目金额    必须与发票类别金额一致  120
        /// </summary>
        public string money;

        /// <summary>
        ///  String	15	否 个人支付金额  发票上显示就必须传  60
        /// </summary>
        public string IndividualPaymentAmount;

    }

    /// <summary>
    /// 6.proReceiptDetail为按类别汇总
    /// </summary>
    public class ProReceiptDetailAD
    {
        public ProReceiptDetailAD()
        {
        }
        public ProReceiptDetailAD(ProReceiptDetail pro)
        {
            this.d = pro.dictionaryCode;
            this.i = pro.dictionaryName;
            this.n = pro.IndividualPaymentAmount;
            this.m = pro.money;
        }
        public ProReceiptDetail GetProReceiptDetail()
        {
            ProReceiptDetail pro = new ProReceiptDetail();
            pro.dictionaryCode = this.d;
            pro.dictionaryName = this.i;
            pro.IndividualPaymentAmount = this.n;
            pro.money = this.m;
            return pro;
        }
        /// <summary>
        /// String	20	是 医院his内部项目类别代码	0001 dictionaryCode
        /// </summary>
        public string d;
        /// <summary>
        /// String	20	是 医院his内部项目类别名称   西药费dictionaryName
        /// </summary>
        public string i;
        /// <summary>
        /// String	15	是 项目金额    必须与发票类别金额一致  120money
        /// </summary>
        public string m;

        /// <summary>
        ///  String	15	否 个人支付金额  发票上显示就必须传  60IndividualPaymentAmount
        /// </summary>
        public string n;

    }

    /// <summary>
    /// 7.proReceiptDetail处方
    /// </summary>
    public class PrescriptionDetail
    {
        /// <summary>
        /// 20	是 门诊处方号
        /// </summary>
        public string recipeNum;

        /// <summary>
        /// String	20	是 门诊处方流水号
        /// </summary>
        public string recipeSerialNum;

        /// <summary>
        ///  门诊处方项目代码	00323
        /// </summary>
        public string prescriptionitemsCode;

        /// <summary>
        ///门诊处方项目名称    肝功能
        /// </summary>
        public string prescriptionitemsName;

        /// <summary>
        /// 项目单位 ItemUnit
        /// </summary>
        public string iU;

        /// <summary>
        /// 项目单价ItemPrice
        /// </summary>
        public decimal iP;

        /// <summary>
        /// 项目数量ItemCount
        /// </summary>
        public decimal iC;

        /// <summary>
        /// 项目金额ItemJe
        /// </summary>
        public decimal iJ;

        /// <summary>
        /// String	20	是 处方日期	20190501111122（yyyyMMddhhMMss）
        /// </summary>
        public string prescriptionDate;

        /// <summary>
        /// String	10	是 处方/医嘱医生编码
        /// </summary>
        public string doctorCode;


        /// <summary>
        ///  String	10	是 处方/医嘱医生姓名
        /// </summary>
        public string doctorName;

    }

    /// <summary>
    /// 7.proReceiptDetail处方
    /// </summary>
    public class PrescriptionDetailAD
    {
        public PrescriptionDetailAD()
        {

        }
        public PrescriptionDetailAD(PrescriptionDetail detail)
        {
            this.d = detail.doctorCode;
            this.o = detail.doctorName;
            this.iC = detail.iC;
            this.iJ = detail.iJ;
            this.iP = detail.iP;
            this.iU = detail.iU;
            this.c = detail.prescriptionDate;
            this.p = detail.prescriptionitemsCode;
            this.s = detail.prescriptionitemsName;
            this.r = detail.recipeNum;
            this.e = detail.recipeSerialNum;

        }

        public PrescriptionDetail GetPrescriptionDetail()
        {
            PrescriptionDetail detail = new PrescriptionDetail();
            detail.doctorCode = this.d;
            detail.doctorName = this.o;
            detail.iC = this.iC;
            detail.iJ = this.iJ;
            detail.iP = this.iP;
            detail.iU = this.iU;
            detail.prescriptionDate = this.c;
            detail.prescriptionitemsCode = this.p;
            detail.prescriptionitemsName = this.s;
            detail.recipeNum = this.r;
            detail.recipeSerialNum = this.e;
            return detail;
        }


        /// <summary>
        /// 20	是 门诊处方号
        /// </summary>
        public string r;

        /// <summary>
        /// String	20	是 门诊处方流水号
        /// </summary>
        public string e;

        /// <summary>
        ///  门诊处方项目代码	00323
        /// </summary>
        public string p;

        /// <summary>
        ///门诊处方项目名称    肝功能prescriptionitemsName
        /// </summary>
        public string s;

        /// <summary>
        /// 项目单位 ItemUnit
        /// </summary>
        public string iU;

        /// <summary>
        /// 项目单价ItemPrice
        /// </summary>
        public decimal iP;

        /// <summary>
        /// 项目数量ItemCount
        /// </summary>
        public decimal iC;

        /// <summary>
        /// 项目金额ItemJe
        /// </summary>
        public decimal iJ;

        /// <summary>
        /// String	20	是 处方日期	20190501111122（yyyyMMddhhMMss）prescriptionDate
        /// </summary>
        public string c;

        /// <summary>
        /// String	10	是 处方/医嘱医生编码
        /// </summary>
        public string d;


        /// <summary>
        ///  String	10	是 处方/医嘱医生姓名
        /// </summary>
        public string o;

    }
    

}


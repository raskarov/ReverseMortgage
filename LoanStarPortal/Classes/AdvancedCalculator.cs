using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using RevMortLib;

namespace LoanStar.Common
{
    public enum RateRoundingMethod
    {
        Down = 1,
        Up = 2,
        Nearest 
    }

    public enum ProductFlag
    {
        None = 0,
        HECM_Monthly = 1,
        HECM_Annual = 2,
        FNMA = 4,
        HECM_FIXED = 5

    }

    #region ZMath addition
    public class TalcInfo
    {
        public TalcInfo(double _talc, int _term)
        {
            talc = _talc;
            term = _term;
        }

        private double talc = 0;
        public double Talc
        {
            get { return talc; }
            set { talc = value; }
        }

        private int term = 0;
        public int Term
        {
            get { return term; }
            set { term = value; }
        }
    }

    public class Talc
    {
        public Talc(TalcItem talcItem)
        {
            talcInfo = new List<TalcInfo>();
            for (int i = 0; i < 4; i++)
                talcInfo.Add(new TalcInfo(talcItem.Talc[i], talcItem.Term[i]));

            apprec = talcItem.Apprec;
        }

        private readonly List<TalcInfo> talcInfo = null;
        public List<TalcInfo> TalcInfo
        {
            get { return talcInfo; }
        }

        private readonly double apprec = 0;
        public double Apprec
        {
            get { return apprec; }
        }
    }

    public class Schedule
    {
        public Schedule(ScheduleItem scheduleItem)
        {
            pmtNo = scheduleItem.PmtNo;
		    scheduleDate = scheduleItem.ScheduleDate;
		    age = scheduleItem.Age;
		    serviceFee = scheduleItem.ServiceFee;
		    pmt = scheduleItem.Pmt;
		    prem = scheduleItem.Prem;
		    interest = scheduleItem.Interest;
		    balance = scheduleItem.Balance;
		    lineOfCredit = scheduleItem.LineOfCredit;
		    principalLimit = scheduleItem.PrincipalLimit;
		    propertyValue = scheduleItem.PropertyValue;
		    potentialSharedApp = scheduleItem.PotentialSharedApp;
		    actualSharedApp = scheduleItem.ActualSharedApp;
		    revisedBalance = scheduleItem.RevisedBalance;
        }

        public int pmtNo;
        public int PmtNo
        {
            get { return pmtNo; }
            set { pmtNo = value; }
        }

        public DateTime scheduleDate;
        public DateTime ScheduleDate
        {
            get { return scheduleDate; }
            set { scheduleDate = value; }
        }

        public double age;
        public double Age
        {
            get { return age; }
            set { age = value; }
        }

        public double serviceFee;
        public double ServiceFee
        {
            get { return serviceFee; }
            set { serviceFee = value; }
        }

        public double pmt;
        public double Pmt
        {
            get { return pmt; }
            set { pmt = value; }
        }

        public double prem;
        public double Prem
        {
            get { return prem; }
            set { prem = value; }
        }

        public double interest;
        public double Interest
        {
            get { return interest; }
            set { interest = value; }
        }

        public double balance;
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public double lineOfCredit;
        public double LineOfCredit
        {
            get { return lineOfCredit; }
            set { lineOfCredit = value; }
        }

        public double principalLimit;
        public double PrincipalLimit
        {
            get { return principalLimit; }
            set { principalLimit = value; }
        }

        public double propertyValue;
        public double PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }

        public double potentialSharedApp;
        public double PotentialSharedApp
        {
            get { return potentialSharedApp; }
            set { potentialSharedApp = value; }
        }

        public double actualSharedApp;
        public double ActualSharedApp
        {
            get { return actualSharedApp; }
            set { actualSharedApp = value; }
        }

        public double revisedBalance;
        public double RevisedBalance
        {
            get { return revisedBalance; }
            set { revisedBalance = value; }
        }
    }

    public class YearEnd
    {
        public YearEnd(YearEndItem yearEndItem)
        {
            yearNo = yearEndItem.YearNo;
		    age = yearEndItem.Age;
		    serviceFee = yearEndItem.ServiceFee;
		    pmt = yearEndItem.Pmt;
		    prem = yearEndItem.Prem;
		    interest = yearEndItem.Interest;
		    balance = yearEndItem.Balance;
		    lineOfCredit = yearEndItem.LineOfCredit;
		    principalLimit = yearEndItem.PrincipalLimit;
		    propertyValue = yearEndItem.PropertyValue;
		    potentialSharedApp = yearEndItem.PotentialSharedApp;
		    actualSharedApp = yearEndItem.ActualSharedApp;
		    revisedBalance = yearEndItem.RevisedBalance;
        }

        private int yearNo;
        public int YearNo
        {
            get { return yearNo; }
            set { yearNo = value; }
        }

        private double age;
        public double Age
        {
            get { return age; }
            set { age = value; }
        }

        private double serviceFee;
        public double ServiceFee
        {
            get { return serviceFee; }
            set { serviceFee = value; }
        }

        private double pmt;
        public double Pmt
        {
            get { return pmt; }
            set { pmt = value; }
        }

        private double prem;
        public double Prem
        {
            get { return prem; }
            set { prem = value; }
        }

        private double interest;
        public double Interest
        {
            get { return interest; }
            set { interest = value; }
        }

        private double balance;
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        private double lineOfCredit;
        public double LineOfCredit
        {
            get { return lineOfCredit; }
            set { lineOfCredit = value; }
        }

        private double principalLimit;
        public double PrincipalLimit
        {
            get { return principalLimit; }
            set { principalLimit = value; }
        }

        private double propertyValue;
        public double PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }

        private double potentialSharedApp;
        public double PotentialSharedApp
        {
            get { return potentialSharedApp; }
            set { potentialSharedApp = value; }
        }

        private double actualSharedApp;
        public double ActualSharedApp
        {
            get { return actualSharedApp; }
            set { actualSharedApp = value; }
        }

        private double revisedBalance;
        public double RevisedBalance
        {
            get { return revisedBalance; }
            set { revisedBalance = value; }
        }
    }

    public class MortgageBuffer : RevMortBuffer
    {
        #region Private fields
        private Product.ProductRate curProdRate = null;
        private Product.ProductRate closingDateProdRate = null;
        private bool needInitialDraw = false;
        private bool needCreditLine = false;
        private bool needMonthlyIncome = false;
        private bool needTerm = false;
        private double? publishedExpectedIndex = null;
        private double? publishedExpectedRate;
        private double productExpectedFloorRate;
        #endregion
        public double ProductExpectedFloorRate
        {
            get { return productExpectedFloorRate; }
        }
        #region Properties
        public DateTime EffectiveDate
        {
            get { return HecmFnmaFlag == 2 ? DateTime.Now.Date : ClosingDate.Date; }
        }
        public double ExpectedMargin
        {
            get { return curProdRate == null ? 0 : curProdRate.ExpectedMargin; }
        }
        public double PublishedInitialIndex
        {
//            get { return closingDateProdRate == null ? 0 : Math.Round(closingDateProdRate.PublishedInitialIndex, 2); }
            get { return closingDateProdRate == null ? 0 : closingDateProdRate.PublishedInitialIndex; }
        }
        public double PublishedInitialMargin
        {
            get { return closingDateProdRate == null ? 0 : closingDateProdRate.Margin; }
        }
        public double PublishedInitialRate
        {
//            get { return closingDateProdRate == null ? 0 : Math.Round(closingDateProdRate.PublishedInitialRate, 2); }
            get { return closingDateProdRate == null ? 0 : closingDateProdRate.PublishedInitialRate; }
        }

        public double PublishedExpectedRate
        {
            get
            {
//                return curProdRate == null ? 0 : Math.Round(curProdRate.PublishedExpectedRate, 2);
                return curProdRate == null ? 0 : curProdRate.PublishedExpectedRate;
            }
            set { publishedExpectedRate = value; }
        }
        public double PublishedExpectedIndex
        {
            get
            {
//                return curProdRate == null ? 0 : Math.Round(curProdRate.PublishedExpectedIndex, 2);
                return curProdRate == null ? 0 : curProdRate.PublishedExpectedIndex;
            }
            set { publishedExpectedIndex = value; }
        }

        public double CreditLineGrowthRate
        {
//            get { return HecmFnmaFlag == 2 || curProdRate == null ? 0 : Math.Round(curProdRate.PublishedInitialRate + 0.5, 2); }
            get { return HecmFnmaFlag == 2 || curProdRate == null ? 0 : curProdRate.PublishedInitialRate+0.5; }
        }
        public bool NeedMonthlyIncome
        {
            get { return needMonthlyIncome; }
            set { needMonthlyIncome = value; }
        }
        public bool NeedTerm
        {
            get { return needTerm; }
            set { needTerm = value; }
        }
        public bool NeedInitialDraw
        {
            get { return needInitialDraw; }
            set { needInitialDraw = value; }
        }
        public bool NeedCreditLine
        {
            get { return needCreditLine; }
            set { needCreditLine = value; }
        }
        public decimal PledgedValueOrLimit
        {
            get
            {
                if (HomeValue < LendingLimit)
                    return Convert.ToDecimal(HomeValue);
                else
                    return Convert.ToDecimal(LendingLimit);
            }
        }
        public decimal UpFrontPremium
        {
            get { return HecmFnmaFlag == 2 ? 0 : Convert.ToDecimal(UpfrontPremium); }
        }
        #endregion
        #region Constructors
        public MortgageBuffer()
        {
            Borrower2Birthdate = DateTime.MinValue;
            PortionMiFinanced = 1;
            NetProceeds = 0;
        }
        #endregion

        #region Methods
        public MortgageBuffer GetStartCopy()
        {
            MortgageBuffer startCopyBuf = new MortgageBuffer();

            startCopyBuf.HecmFnmaFlag = HecmFnmaFlag;
            startCopyBuf.MonthlyServiceFee = MonthlyServiceFee;
            startCopyBuf.EomFlag = EomFlag;
            startCopyBuf.Precision = Precision;
            startCopyBuf.RateRoundingMethod = RateRoundingMethod;
            startCopyBuf.Basis = Basis;
            startCopyBuf.PaymentsPerYear = PaymentsPerYear;
            startCopyBuf.PropertyAppreciation = PropertyAppreciation;
            startCopyBuf.UpfrontMiRate = UpfrontMiRate;
            startCopyBuf.RenewalMiRate = RenewalMiRate;
            startCopyBuf.EffectiveRateCap = EffectiveRateCap;
            startCopyBuf.Margin = Margin;
            startCopyBuf.Index = Index;
            startCopyBuf.FirstYrPropChgs = FirstYrPropChgs;

            startCopyBuf.InitialDraw = InitialDraw;
            startCopyBuf.CreditLine = CreditLine;
            startCopyBuf.PaymentAmount = PaymentAmount;
            startCopyBuf.Term = Term;
            startCopyBuf.CalcLineOfCredit = CalcLineOfCredit;
            startCopyBuf.OtherCashOut = OtherCashOut;
            startCopyBuf.Repairs = Repairs;
            startCopyBuf.MortgageBalance = MortgageBalance;
            startCopyBuf.LenderFees = LenderFees;
            startCopyBuf.OtherClosingCosts = OtherClosingCosts;
            startCopyBuf.LendingLimit = LendingLimit;
            startCopyBuf.SharedAppreciation = SharedAppreciation;
            startCopyBuf.HomeValue = HomeValue;
            startCopyBuf.LifeExpectancy = LifeExpectancy;
            startCopyBuf.Borrower1Birthdate = Borrower1Birthdate;
            startCopyBuf.ClosingDate = ClosingDate;

            return startCopyBuf;
        }

        public void LoadFromProduct(Product product,int lenderId, DateTime closingDate)
        {
            curProdRate = product.GetNearestProductRate(EffectiveDate);
            closingDateProdRate = product.GetNearestProductRate(closingDate);

            MonthlyServiceFee = product.GetCompanyServiceFee(lenderId);
            EomFlag = product.EndOfMonthFlag ? "Y" : "N";

            Precision = Convert.ToDouble(product.RateRoundingPrecision);
            RateRoundingMethod = product.RateRoundingMethodId;
            Basis = product.BasisId;
            PaymentsPerYear = product.PaymentsPerYear;
            PropertyAppreciation = Convert.ToDouble(product.PropertyAppreciation);
            UpfrontMiRate = Convert.ToDouble(product.UpfrontMortgageInsuranceRate);
            RenewalMiRate = Convert.ToDouble(product.RenewalMortgageInsuranceRate);
            productExpectedFloorRate = product.ExpectedFloorRate;
            SetRatesDependableValues(product);
            FirstYrPropChgs = product.FirstYearPropertyCharges;
        }
        private void SetRatesDependableValues(Product product)
        {
            EffectiveRateCap = PublishedInitialRate + Convert.ToDouble(product.RelativeRateCap);
            Margin = ExpectedMargin;
            Index = PublishedExpectedRate < productExpectedFloorRate ? productExpectedFloorRate - Margin : PublishedExpectedIndex;
        }
        public void LoadRatesForSpecificDate(Product product, DateTime rateDate,out DateTime realdate)
        {
            curProdRate = product.GetNearestProductRate(rateDate);
            realdate = DateTime.MinValue;
            if(curProdRate!=null) realdate = curProdRate.Period;
            SetRatesDependableValues(product);
        }
        public void SetInitialRate(Product product, DateTime rateDate, out DateTime realdate)
        {
            Product.ProductRate rate = product.GetNearestProductRate(rateDate);
            realdate = DateTime.MinValue;
            if (rate != null)
            {
                realdate = rate.Period;
//                InitialRate = Math.Round(rate.PublishedInitialRate,2);
                InitialRate = rate.PublishedInitialRate;
            }
        }

        #endregion



        #region Talcs
        private List<Talc> talcList = null;
        public List<Talc> Talcs
        {
            get
            {
                if (talcList == null)
                    talcList = GetTalcList();

                return talcList;
            }
        }

        private List<Talc> GetTalcList()
        {
            List<Talc> talcList_ = new List<Talc>();
            foreach (TalcItem talcItem in Talc)
                talcList_.Add(new Talc(talcItem));

            return talcList_;
        }
        #endregion

        #region Schedules
        private List<Schedule> scheduleList = null;
        public List<Schedule> Schedules
        {
            get
            {
                if (scheduleList == null)
                    scheduleList = GetScheduleList();

                return scheduleList;
            }
        }

        private List<Schedule> GetScheduleList()
        {
            List<Schedule> scheduleList_ = new List<Schedule>();
            for (int i = 0; i < Term; i++)
                scheduleList_.Add(new Schedule(Schedule[i]));

            return scheduleList_;
        }
        #endregion

        #region YearEnds
        private List<YearEnd> yearEndList = null;
        public List<YearEnd> YearEnds
        {
            get
            {
                if (yearEndList == null)
                    yearEndList = GetYearEndList();

                return yearEndList;
            }
        }

        private List<YearEnd> GetYearEndList()
        {
            List<YearEnd> yearEndList_ = new List<YearEnd>();
            int yearEndCount = 99 - (int)YoungBorrowerAge + 1;
            if (yearEndCount > MAX_YEAR_END)
                yearEndCount = MAX_YEAR_END;
            for (int i = 0; i < yearEndCount/*base.Term / 12*/; i++)
                yearEndList_.Add(new YearEnd(YearEnd[i]));

            return yearEndList_;
        }
        #endregion
    }
    #endregion

    #region Calculator launches
    public enum LaunchStep
    {
        Launch1 = 1,
        Launch2 = 2,
        Launch3 = 3,
        Launch4 = 4,
        Launch5 = 5,
        Launch6 = 6,
        Finish  = 7
    }

    public class AdvancedCalculatorLauncher
    {
        private delegate void Launch();
        private Launch Calculate;
        private LaunchStep currentStep;
        private readonly AdvancedCalculator advancedCalculator = null;
        private string errorMessage = String.Empty;

        public AdvancedCalculatorLauncher(AdvancedCalculator advCalc)
        {
            advancedCalculator = advCalc;
            currentStep = LaunchStep.Launch1;
        }

        #region Properties
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        #endregion

        #region Manage Launches
        public void StartLaunches()
        {
            while (NextLaunch() != LaunchStep.Finish)
                Calculate();
        }

        private LaunchStep NextLaunch()
        {
            switch (currentStep)
            {
                case LaunchStep.Launch1:
                    Calculate = Launch1;
                    break;
                case LaunchStep.Launch2:
                    Calculate = Launch2;
                    break;
                case LaunchStep.Launch3:
                    Calculate = Launch3;
                    break;
                case LaunchStep.Launch4:
                    Calculate = Launch4;
                    break;
                case LaunchStep.Launch5:
                    Calculate = Launch5;
                    break;
                case LaunchStep.Launch6:
                    Calculate = Launch6;
                    break;
                default:
                    break;
            }

            return currentStep;
        }
        #endregion

        #region Launches
        private void Launch1()
        {
            errorMessage = advancedCalculator.Calculate(LaunchStep.Launch1);
            currentStep = !String.IsNullOrEmpty(errorMessage) || advancedCalculator.UnallocatedFunds <= 0 ?
                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch2;
        }
        private void Launch2()
        {
            MortgageBuffer launch2Buf = advancedCalculator.GetStartCopyMortgageBuffer();
            launch2Buf.InitialDraw = Convert.ToDouble(advancedCalculator.UnallocatedFunds) + 
                Convert.ToDouble(advancedCalculator.InitialDraw);
            
            errorMessage = advancedCalculator.Calculate(launch2Buf, LaunchStep.Launch2);
//            AdvancedCalculator advCalcLaunch2 = advancedCalculator[LaunchStep.Launch2];
            
//            currentStep = !String.IsNullOrEmpty(errorMessage) || advCalcLaunch2.UnallocatedFunds <= 0 ? 
//                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch3;
            currentStep = !String.IsNullOrEmpty(errorMessage) ?
                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch3;

//            if (String.IsNullOrEmpty(errorMessage) && advCalcLaunch2.UnallocatedFunds <= 0)
//                advancedCalculator.MPInitialDraw = advCalcLaunch2.InitialDraw;
        }
        private void Launch3()
        {
            MortgageBuffer launch3Buf = advancedCalculator.GetStartCopyMortgageBuffer();
            launch3Buf.CreditLine = Convert.ToDouble(advancedCalculator.UnallocatedFunds) + 
                2 * Convert.ToDouble(advancedCalculator.InitialDraw) - 
                Convert.ToDouble(advancedCalculator.CreditLine);

            errorMessage = advancedCalculator.Calculate(launch3Buf, LaunchStep.Launch3);
//            AdvancedCalculator advCalcLaunch3 = advancedCalculator[LaunchStep.Launch3];

//            currentStep = !String.IsNullOrEmpty(errorMessage) || advCalcLaunch3.UnallocatedFunds <= 0 ? 
//                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch4;
            currentStep = !String.IsNullOrEmpty(errorMessage) ? 
                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch4;

//            if (String.IsNullOrEmpty(errorMessage) && advCalcLaunch3.UnallocatedFunds <= 0)
//                advancedCalculator.MPCreditLine = advCalcLaunch3.CreditLine;
        }
        private void Launch4()
        {
            MortgageBuffer launch4Buf = advancedCalculator.GetStartCopyMortgageBuffer();
            launch4Buf.CalcLineOfCredit = 0;
            launch4Buf.PaymentAmount = 0;
            launch4Buf.Term = 0;

            errorMessage = advancedCalculator.Calculate(launch4Buf, LaunchStep.Launch4);
//            AdvancedCalculator advCalcLaunch4 = advancedCalculator[LaunchStep.Launch4];

//            currentStep = !String.IsNullOrEmpty(errorMessage) || !advCalcLaunch4.NeedMonthlyIncome || 
//                !advCalcLaunch4.NeedTerm || advCalcLaunch4.UnallocatedFunds <= 0 ?
//                currentStep = LaunchStep.Finish : currentStep = LaunchStep.Launch5;
            
            currentStep = String.IsNullOrEmpty(errorMessage) && 
                advancedCalculator.NeedMonthlyIncome && advancedCalculator.NeedTerm && advancedCalculator.MPTerm > 0 ?
                currentStep = LaunchStep.Launch5 : currentStep = LaunchStep.Finish;

//            if (String.IsNullOrEmpty(errorMessage) && advCalcLaunch4.UnallocatedFunds <= 0)
//            {
//                advancedCalculator.NeedMonthlyIncome = true;
//                advancedCalculator.NeedTerm = false;
//                advancedCalculator.MPPaymentAmount = 0;
//                advancedCalculator.MPTerm = 0;
//            }
        }
        private void Launch5()
        {
            MortgageBuffer launch5Buf = advancedCalculator.GetStartCopyMortgageBuffer();
            launch5Buf.CalcLineOfCredit = 0;
            launch5Buf.PaymentAmount = 0;

            errorMessage = advancedCalculator.Calculate(launch5Buf, LaunchStep.Launch5);
            currentStep = LaunchStep.Finish;
        }
        private void Launch6()
        {
            
        }
        #endregion
    }
    #endregion

    public class AdvancedCalculator : BaseObject
    {
        #region Private fields
        private readonly MortgageProfile mp = null;
        private MortgageBuffer mortgageBuffer = null;

        private bool needInitialDraw = false;
        private bool needCreditLine = false;
        private bool needMonthlyIncome = false;
        private bool needTerm = false;

        private readonly Hashtable productHash = new Hashtable();
        private readonly SortedList launchHash = new SortedList();

        private decimal reserveTotalAmount = 0;
        private Hashtable retailCalculatorProducts;
        #endregion

        #region Constructors
        public AdvancedCalculator(AdvancedCalculator advCalc, LaunchStep launchStep)
        {
            mp = advCalc.mp;
            MortgageBuffer = (MortgageBuffer)advCalc.launchHash[launchStep];

            productHash[ProductFlag.HECM_Monthly] = advCalc.productHash[ProductFlag.HECM_Monthly];
            productHash[ProductFlag.HECM_Annual] = advCalc.productHash[ProductFlag.HECM_Annual];
            productHash[ProductFlag.FNMA] = advCalc.productHash[ProductFlag.FNMA];

            ID = advCalc.ID;
            NeedInitialDraw = advCalc.NeedInitialDraw;
            NeedCreditLine = advCalc.NeedCreditLine;
            NeedMonthlyIncome = advCalc.NeedMonthlyIncome;
            NeedTerm = advCalc.NeedTerm;
        }

        public AdvancedCalculator(MortgageProfile _mp)
        {
            mp = _mp;
            MortgageBuffer = new MortgageBuffer();

            productHash[ProductFlag.HECM_Monthly] = new Product(ProductFlag.HECM_Monthly);
            productHash[ProductFlag.HECM_Annual] = new Product(ProductFlag.HECM_Annual);
            productHash[ProductFlag.FNMA] = new Product(ProductFlag.FNMA);

            ID = _mp.ID;
            if (_mp.ID < 0)
                return;

            DataTable tblAdvCalc = db.GetDataTable("GetAdvancedCalculatorByID", _mp.ID);
            if (tblAdvCalc.Rows.Count == 0)
                return;

            LoadFromDataRow(tblAdvCalc.DefaultView[0]);
        }
        #endregion

        #region Properties
        public AdvancedCalculator this[LaunchStep launchStep]
        {
            get { return new AdvancedCalculator(this, launchStep); }
        }

        public MortgageBuffer GetStartCopyMortgageBuffer()
        {
            return MortgageBuffer.GetStartCopy();
        }

        public bool ContainsLaunch(LaunchStep launchStep)
        {
            return launchHash.ContainsKey(launchStep);
        }

        private MortgageBuffer MortgageBuffer
        {
            set
            {
                mortgageBuffer = value ?? new MortgageBuffer();
                ReserveTotalAmount = mp.ReserveTotalAmount;

                #region unused code
                //            buf.Borrower1Birthdate = DateTime.Parse("00/00/0000");//DateTime.Parse("09/20/1902");
                //            buf.Borrower2Birthdate = DateTime.Parse("00/00/0000");//DateTime.MinValue;
                //            buf.ClosingDate = DateTime.Parse("00/00/0000");//DateTime.Parse("11/04/2007");


                // The following fields apply to both HECM and HomeKeeper plans.
                //            buf.HecmFnmaFlag = 0;	/* 1 = hecm, 2 = fnma	*/
                //            buf.HomeValue = 0;
                //            buf.MortgageBalance = 0;
                //            buf.LendingLimit = 0;
                //            buf.LenderFees = 0;
                //            buf.OtherClosingCosts = 0;
                //MonthlyServiceFee = mp.MortgageInfo.Product.PeriodicServiceFee;//buf.MonthlyServiceFee = 0;

                //EomFlag = mp.MortgageInfo.Product.EndOfMonthFlag;//buf.EomFlag = "N";
                //            buf.InitialDraw = 0.00;
                //            buf.Repairs = 0.00;
                //            buf.OtherCashOut = 0.00;
                //            buf.CreditLine = 0.00;
                //            buf.CalcLineOfCredit = 1;

                // The following fields are HECM specific 
                //buf.Index = 4.74;
                //Margin = mp.MortgageInfo.Product.Margin;//1.25;
                //buf.Precision = Convert.ToDouble(mp.MortgageInfo.Product.RateRoundingPrecision);//0.125;OK!!
                //buf.RateRoundingMethod = mp.MortgageInfo.Product.RateRoundingMethodId;//3;
                //buf.Basis = mp.MortgageInfo.Product.BasisId;//1;
                //buf.PaymentsPerYear = (double)mp.MortgageInfo.Product.PaymentsPerYear;//12;
                //            buf.Term = 0;
                //PropertyAppreciation = mp.MortgageInfo.Product.PropertyAppreciation;//buf.PropertyAppreciation = 8;
                //UpfrontMiRate = mp.MortgageInfo.Product.UpfrontMortgageInsuranceRate;//buf.UpfrontMiRate = 2.0;
                //RenewalMiRate = mp.MortgageInfo.Product.RenewalMortgageInsuranceRate;//buf.RenewalMiRate = 0.5;
                
                //            buf.PaymentAmount = 0.00;
                //            buf.LifeExpectancy = 2;
                
                //            SharedAppreciation = mp.MortgageInfo.SharedAppreciation;//0;!!!!
                //EffectiveRateCap = mp.MortgageInfo.Product.EffectiveRateCap;//buf.EffectiveRateCap = 0;

                // The following field is HomeKeeper specific 
                //buf.FirstYrPropChgs = (double)mp.MortgageInfo.Product.FirstYearPropertyCharges;//0;
                #endregion
            }
            get { return mortgageBuffer; }
        }

        public MortgageProfile Owner
        {
            get { return mp; }
        }

        public decimal ReserveTotalAmount
        {
            get { return reserveTotalAmount; }
            set { reserveTotalAmount = value; }
        }

        public decimal MPInitialDraw
        {
            get { return mp.InitialDraw == null ? 0 : (decimal)mp.InitialDraw; }
            set { mp.InitialDraw = value; }
        }
        public decimal MPCreditLine
        {
            get { return mp.CreditLine == null ? 0 : (decimal)mp.CreditLine; }
            set { mp.CreditLine = value; }
        }

        public decimal MPPaymentAmount
        {
            get { return mp.PaymentAmount ?? 0; }
            set { mp.PaymentAmount = value; }
        }

        public decimal MPTerm
        {
            get { return mp.MortgageInfo.Term ?? 0; }
            set { mp.MortgageInfo.Term = value; }
        }

        public ProductFlag MPProduct
        {
            get { return mp.ProductCalcType; }
        }

        public int MPProductID
        {
            get { return mp.MortgageInfo.ProductId; }
            set { mp.MortgageInfo.ProductId = value; }
        }

        public Hashtable MPProdTypeHash
        {
            get { return mp.MortgageInfo.ProdTypeHash; }
        }
        public Hashtable RetailCalculatorProducts
        { 
            get
            {
                if (retailCalculatorProducts == null)
                {
                    retailCalculatorProducts = new Hashtable();
                    Product prod = new Product(ProductFlag.HECM_Monthly);
                    retailCalculatorProducts[prod.CalculationType] = prod;
                    prod = new Product(ProductFlag.HECM_Annual);
                    retailCalculatorProducts[prod.CalculationType] = prod;
                    prod = new Product(ProductFlag.FNMA);
                    retailCalculatorProducts[prod.CalculationType] = prod;
                }
                return retailCalculatorProducts;
            }
        }
        public bool NeedMonthlyIncome
        {
            get { return needMonthlyIncome; }
            set { needMonthlyIncome = value; }
        }

        public bool NeedTerm
        {
            get { return needTerm; }
            set { needTerm = value; }
        }

        public bool NeedInitialDraw
        {
            get { return needInitialDraw; }
            set { needInitialDraw = value; }
        }

        public bool NeedCreditLine
        {
            get { return needCreditLine; }
            set { needCreditLine = value; }
        }
        #endregion

        #region Mortgage buffer properties
        public decimal FinalTerm
        {
            get { return Convert.ToDecimal(mortgageBuffer.FinalTerm); }
        }

        public bool CalcLineOfCredit
        {
            get { return Convert.ToBoolean(mortgageBuffer.CalcLineOfCredit); }
            set { mortgageBuffer.CalcLineOfCredit = Convert.ToInt32(value); }
        }

        public decimal LendingLimit
        {
            get { return Convert.ToDecimal(mortgageBuffer.LendingLimit); }
            set { mortgageBuffer.LendingLimit = Convert.ToDouble(value); }
        }

        public decimal Term
        {
            get { return Convert.ToDecimal(mortgageBuffer.Term); }
            set { mortgageBuffer.Term = Convert.ToDouble(value); }
        }

        public decimal PaymentAmount
        {
            get { return Convert.ToDecimal(mortgageBuffer.PaymentAmount); }
            set { mortgageBuffer.PaymentAmount = Convert.ToDouble(value); }
        }

        public decimal PeriodicPayment
        {
            get { return Convert.ToDecimal(mortgageBuffer.PeriodicPayment); }
        }

        public decimal CreditLine
        {
            get { return Convert.ToDecimal(mortgageBuffer.CreditLine); }
            set { mortgageBuffer.CreditLine = Convert.ToDouble(value); }
        }

        public decimal LineOfCredit
        {
            get { return Convert.ToDecimal(mortgageBuffer.LineOfCredit); }
        }

        public decimal InitialDraw
        {
            get { return Convert.ToDecimal(mortgageBuffer.InitialDraw); }
            set { mortgageBuffer.InitialDraw = Convert.ToDouble(value); }
        }

        public decimal UnallocatedFunds
        {
            get { return LineOfCredit - CreditLine; }
        }

        public decimal OtherCashOut
        {
            get { return Convert.ToDecimal(mortgageBuffer.OtherCashOut); }
            set { mortgageBuffer.OtherCashOut = Convert.ToDouble(value); }
        }

        public decimal Repairs
        {
            get { return Convert.ToDecimal(mortgageBuffer.Repairs); }
            set { mortgageBuffer.Repairs = Convert.ToDouble(value); }
        }

        public decimal MortgageBalance
        {
            get { return Convert.ToDecimal(mortgageBuffer.MortgageBalance); }
            set { mortgageBuffer.MortgageBalance = Convert.ToDouble(value); }
        }

        public decimal OtherClosingCosts
        {
            get { return Convert.ToDecimal(mortgageBuffer.OtherClosingCosts); }
            set { mortgageBuffer.OtherClosingCosts = Convert.ToDouble(value); }
        }

        public decimal TotalOtherClosingCosts
        {
            get { return Convert.ToDecimal(mortgageBuffer.TotalOtherClosingCosts); }
        }

        public decimal OtherFinancedCosts
        {
            get { return TotalOtherClosingCosts - LoanOriginationFees; }
        }

        public decimal TotalClosingCosts
        {
            get { return Convert.ToDecimal(mortgageBuffer.TotalClosingCosts); }
        }

        public decimal LoanOriginationFees
        {
            get { return mp.MortgageInfo.OriginationFees; }
        }

        public decimal LenderFees
        {
            get { return Convert.ToDecimal(mortgageBuffer.LenderFees); }
            set { mortgageBuffer.LenderFees = Convert.ToDouble(value); }
        }

        public decimal HomeValue
        {
            get { return Convert.ToDecimal(mortgageBuffer.HomeValue); }
            set { mortgageBuffer.HomeValue = Convert.ToDouble(value); }
        }

        public ProductFlag HecmFnmaFlag
        {
            set
            {
                switch (value)
                {
                    case ProductFlag.HECM_Monthly:
                        mortgageBuffer.HecmFnmaFlag = 1;
                        break;
                    case ProductFlag.HECM_Annual:
                        mortgageBuffer.HecmFnmaFlag = 1;
                        break;
                    case ProductFlag.HECM_FIXED:
                        mortgageBuffer.HecmFnmaFlag = 1;
                        break;
                    case ProductFlag.FNMA:
                        mortgageBuffer.HecmFnmaFlag = 2;
                        break;
                    default:
                        mortgageBuffer.HecmFnmaFlag = 0;
                        break;
                }
            }
        }

        public RateRoundingMethod RateRoundingMethod
        {
            get { return (RateRoundingMethod)mortgageBuffer.RateRoundingMethod; }
        }

        public string Basis
        {
            get
            {
                switch (mortgageBuffer.Basis)
                {
                    case 1:
                        return "30/360";
                    case 2:
                        return "Actual/365";
                    case 3:
                        return "30/365";
                    case 6:
                        return "30/Actual";
                    default:
                        return String.Empty;
                }
            }
        }

        public bool EomFlag
        {
            get
            {
                switch (mortgageBuffer.EomFlag)
                {
                    case "y":
                    case "Y":
                        return true;
                    case "n":
                    case "N":
                    default:
                        return false;
                }
            }
        }

        public double PublishedIndex
        {
//            get { return mortgageBuffer.PublishedIndex; }
            get { return 0; }
        }

        public double PublishedExpectedIndex
        {
            get { return mortgageBuffer.PublishedExpectedIndex; }
        }

        public double Margin
        {
            get { return mortgageBuffer.Margin; }
        }

        public double Index
        {
            get { return mortgageBuffer.Index; }
        }

        public double ExpectedIndexRate
        {
            get { return mortgageBuffer.ExpectedIntRate; }
        }
/* 
        public decimal Margin
        {
            get
            {
                return Convert.ToDecimal(mortgageBuffer.Margin);
            }
            set
            {
                mortgageBuffer.Margin = Convert.ToDouble(value);
            }
        }

        public decimal IndexRate
        {
            get
            {
                return Convert.ToDecimal(mortgageBuffer.Index);
            }
            set
            {
                mortgageBuffer.Index = Convert.ToDouble(value);
            }
        }

        public double ExpectedIndexRate
        {
            get
            {
                if (mortgageBuffer.HecmFnmaFlag == 2)
                    return mortgageBuffer.Index;

                return mortgageBuffer.ExpectedIntRate;
            }
        }
*/

        public decimal MonthlyServiceFee
        {
            get { return Convert.ToDecimal(mortgageBuffer.MonthlyServiceFee); }
        }

        public decimal PledgedValueOrLimit
        {
            get { return mortgageBuffer.PledgedValueOrLimit; }
        }

        public double Persentage
        {
            get
            {
                if (PledgedValueOrLimit == 0)
                    return -1;

                return (double)HomeValue / (double)PledgedValueOrLimit;
            }
        }

        public double CreditLineGrowthRate
        {
            get { return mortgageBuffer.CreditLineGrowthRate; }
        }

        public decimal PrincipalLimit
        {
            get { return Convert.ToDecimal(mortgageBuffer.PrincipalLimit); }
        }

        public decimal ServiceSetAside
        {
            get { return mortgageBuffer.HecmFnmaFlag == 2 ? 0 : Convert.ToDecimal(mortgageBuffer.PresentValueServiceFee); }
        }

        public decimal UpFrontPremium
        {
            get { return mortgageBuffer.UpFrontPremium; }
        }

        public decimal NetPrincipalLimit
        {
            get { return Convert.ToDecimal(mortgageBuffer.NetPrincipalLimit); }
        }

        public decimal NetPrincipalLimitSchedPmt
        {
            get { return Convert.ToDecimal(mortgageBuffer.NetPrincipalLimitSchedPmt); }
        }

        public decimal FirstYrPropChgs
        {
            get { return Convert.ToDecimal(mortgageBuffer.FirstYrPropChgs); }
        }

        public decimal NetAvailable
        {
            get
            {
                //return NetPrincipalLimit - MortgageBalance - FirstYrPropChgs;
                return NetPrincipalLimit + InitialDraw - Repairs - OtherCashOut;
            }
        }

        public decimal TotalInitialCharges
        {
            get { return UpFrontPremium + LoanOriginationFees + OtherFinancedCosts; }
        }

        public decimal PropertyAppreciation
        {
            get { return Convert.ToDecimal(mortgageBuffer.PropertyAppreciation); }
        }

        public decimal UpfrontMiRate
        {
            get { return Convert.ToDecimal(mortgageBuffer.UpfrontMiRate); }
        }
        public decimal RenewalMiRate
        {
            get { return Convert.ToDecimal(mortgageBuffer.RenewalMiRate); }
        }
        public decimal PortionMiFinanced
        {
            get { return Convert.ToDecimal(mortgageBuffer.PortionMiFinanced); }
            set { mortgageBuffer.PortionMiFinanced = Convert.ToDouble(value); }
        }
        public decimal NetProceeds
        {
            get { return Convert.ToDecimal(mortgageBuffer.NetProceeds); }
            set { mortgageBuffer.NetProceeds = Convert.ToDouble(value); }
        }
        public decimal SharedAppreciation
        {
            get { return Convert.ToDecimal(mortgageBuffer.SharedAppreciation); }
            set { mortgageBuffer.SharedAppreciation = Convert.ToDouble(value); }
        }
        public double EffectiveRateCap
        {
            get { return mortgageBuffer.EffectiveRateCap; }
        }
        public double Age
        {
            get
            {
/*                Borrower youngestBorrower = mp.YoungestBorrower;
                if (mortgageBuffer.HecmFnmaFlag == 1)
                    return youngestBorrower.NearestAge == null ? 0 : (int)youngestBorrower.NearestAge;
                else if (mortgageBuffer.HecmFnmaFlag == 2)
                    return youngestBorrower.ActualAge == null ? 0 : (int)youngestBorrower.ActualAge;
                else
                    return 0;*/
                return mortgageBuffer.YoungBorrowerAge;
            }
        }

        public string InputParameters
        {
            get
            {
                StringBuilder paramBuilder = new StringBuilder();
                paramBuilder.AppendLine(String.Format("Mortgage Youngest Borrower : {0}", mp.YoungestBorrower.FullName));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Product specific parameters");
                paramBuilder.AppendLine(String.Format("HecmFnmaFlag : {0}", mortgageBuffer.HecmFnmaFlag));
                paramBuilder.AppendLine(String.Format("ZMathIndex : {0}", mortgageBuffer.Index));
                paramBuilder.AppendLine(String.Format("Margin : {0}", Margin));
                paramBuilder.AppendLine(String.Format("MonthlyServiceFee : {0}", MonthlyServiceFee));
                paramBuilder.AppendLine(String.Format("EomFlag : {0}", mortgageBuffer.EomFlag));
                paramBuilder.AppendLine(String.Format("Precision : {0}", mortgageBuffer.Precision));
                paramBuilder.AppendLine(String.Format("RateRoundingMethod : {0}", mortgageBuffer.RateRoundingMethod));
                paramBuilder.AppendLine(String.Format("Basis : {0}", mortgageBuffer.Basis));
                paramBuilder.AppendLine(String.Format("PaymentsPerYear : {0}", mortgageBuffer.PaymentsPerYear));
                paramBuilder.AppendLine(String.Format("PropertyAppreciation : {0}", PropertyAppreciation));
                paramBuilder.AppendLine(String.Format("UpfrontMiRate : {0}", UpfrontMiRate));
                paramBuilder.AppendLine(String.Format("RenewalMiRate : {0}", RenewalMiRate));
                paramBuilder.AppendLine(String.Format("EffectiveRateCap : {0}", EffectiveRateCap));
                paramBuilder.AppendLine(String.Format("FirstYrPropChgs : {0}", mortgageBuffer.FirstYrPropChgs));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Mortgage specific parameters");
                paramBuilder.AppendLine(String.Format("InitialDraw : {0}", InitialDraw));
                paramBuilder.AppendLine(String.Format("CreditLine : {0}", CreditLine));
                paramBuilder.AppendLine(String.Format("PaymentAmount : {0}", PaymentAmount));
                paramBuilder.AppendLine(String.Format("Term : {0}", Term));
                paramBuilder.AppendLine(String.Format("CalcLineOfCredit : {0}", mortgageBuffer.CalcLineOfCredit));
                paramBuilder.AppendLine(String.Format("OtherCashOut : {0}", OtherCashOut));
                paramBuilder.AppendLine(String.Format("Repairs : {0}", Repairs));
                paramBuilder.AppendLine(String.Format("MortgageBalance : {0}", MortgageBalance));
                paramBuilder.AppendLine(String.Format("LenderFees : {0}", LenderFees));
                paramBuilder.AppendLine(String.Format("OtherClosingCosts : {0}", OtherClosingCosts));
                paramBuilder.AppendLine(String.Format("LendingLimit : {0}", LendingLimit));
                paramBuilder.AppendLine(String.Format("SharedAppreciation : {0}", SharedAppreciation));
                paramBuilder.AppendLine(String.Format("HomeValue : {0}", HomeValue));
                paramBuilder.AppendLine(String.Format("LifeExpectancy : {0}", mortgageBuffer.LifeExpectancy));
                paramBuilder.AppendLine(String.Format("Borrower1Birthdate : {0}", mortgageBuffer.Borrower1Birthdate));
                paramBuilder.AppendLine(String.Format("ClosingDate : {0}", mortgageBuffer.ClosingDate));
                paramBuilder.AppendLine(String.Format("ReserveTotalAmount : {0}", ReserveTotalAmount));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Not changed parameters");
                paramBuilder.AppendLine(String.Format("Borrower2Birthdate : {0}", mortgageBuffer.Borrower2Birthdate));
                paramBuilder.AppendLine(String.Format("PortionMiFinanced : {0}", mortgageBuffer.PortionMiFinanced));
                paramBuilder.AppendLine(String.Format("NetProceeds : {0}", mortgageBuffer.NetProceeds));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Output");
                paramBuilder.AppendLine(String.Format("TotalClosingCosts : {0}", TotalClosingCosts));
                paramBuilder.AppendLine(String.Format("TotalOtherClosingCosts : {0}", TotalOtherClosingCosts));
                paramBuilder.AppendLine(String.Format("UpfrontPremium : {0}", UpFrontPremium));

                return paramBuilder.ToString();
            }
        }
        #endregion

        #region Load methods
        private void LoadFromDataRow(DataRowView rowAdvCalc)
        {
            ID = ConvertToInt(rowAdvCalc["MPID"], -1);
            NeedInitialDraw = ConvertToBool(rowAdvCalc["NeedInitialDraw"], true);
            NeedCreditLine = ConvertToBool(rowAdvCalc["NeedCreditLine"], true);
            NeedMonthlyIncome = ConvertToBool(rowAdvCalc["NeedMonthlyIncome"], false);
            NeedTerm = ConvertToBool(rowAdvCalc["NeedTerm"], false);
        }

        public string ValidateFirst()
        {
            string resMessage = String.Empty;

            if (mp.YoungestBorrower == null || mp.YoungestBorrower.LifeExpectancy == null)
            {
                string msg = "You need to have at least one borrower birthday to obtain LifeExpectancy";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            if (mp.Property.StateId == 0 || mp.Property.CountyID == 0)
            {
                string msg = "You need to set both State and County fields for Mortgage property in public area";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }
            else if (mp.Property.LendingLimit == 0)
            {
                string msg = "Can not obtain LendingLimit value from DB";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            if (mp.Property.SPValue == null || (decimal)mp.Property.SPValue <= 0)
            {
                string msg = "You need to enter HomeValue (MarketValue)";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            return resMessage;
        }

        public string Validate()
        {
            string resMessage = String.Empty;
            int yearAge;

            Borrower youngestBorrower = mp.YoungestBorrower;
            DateTime borrower1Birthdate = youngestBorrower != null ?
                ((DateTime)youngestBorrower.DateOfBirth).Date : DateTime.MinValue.Date;
            if (borrower1Birthdate == DateTime.MinValue.Date)
                resMessage = "Borrower1 birthdate is not valid";
            else if (borrower1Birthdate > DateTime.Now.Date)
                resMessage = String.Format("Borrower1 birthdate {0} can not be more then Now {1}", borrower1Birthdate, DateTime.Now.Date);
            else
            {
                yearAge = new DateTime((DateTime.Now.Date - borrower1Birthdate).Ticks).Year;
                if (yearAge > 100 || yearAge < 62) //was 65 : 100
                    resMessage = String.Format("Borrower1 birthdate {0} must be between {1} and {2} years", borrower1Birthdate, 62, 100);
            }

            DateTime closingDate = mp.MortgageInfo.ClosingDate != null ? 
                ((DateTime)mp.MortgageInfo.ClosingDate).Date : DateTime.Now.Date;
            if (borrower1Birthdate > closingDate)
            {
                string msg = String.Format("Borrower1 birthdate {0} can not be more then Mortgage closing date {1}", borrower1Birthdate, closingDate);
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }
            else
            {
                yearAge = new DateTime((closingDate - borrower1Birthdate).Ticks).Year;
                if (yearAge < 62 || yearAge > 100) //was 65 : 95
                {
                    string msg = String.Format("Difference between ClosingDate {0} and Borrower1 birthdate {1} must be between {2} and {3} years", closingDate, borrower1Birthdate, 62, 100);
                    resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
                }
            }

            if (NeedMonthlyIncome && NeedTerm && MPTerm > 456)
            {
                string msg = "Term cannot exceed 456 - the number of months between age 62 and 100";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            if (NeedMonthlyIncome && NeedTerm && MPPaymentAmount <= 0 && MPTerm <= 0)
            {
                string msg = "At least one of the two variables (PaymentAmount or Term) must be entered";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            return resMessage;
        }

        private void LoadFromMortgageProfile()
        {
            mortgageBuffer.NeedCreditLine = NeedCreditLine;
            mortgageBuffer.NeedInitialDraw = NeedInitialDraw;
            mortgageBuffer.NeedMonthlyIncome = NeedMonthlyIncome;
            mortgageBuffer.NeedTerm = NeedTerm;

            InitialDraw = NeedInitialDraw && MPInitialDraw >= 0 ? MPInitialDraw : 0;
            CreditLine = NeedCreditLine && MPCreditLine >= 0 ? MPCreditLine : 0;
            PaymentAmount = NeedMonthlyIncome && NeedTerm ? MPPaymentAmount : 0;
            Term = NeedMonthlyIncome && NeedTerm ? MPTerm : 0;

            //CalcLineOfCredit = !NeedMonthlyIncome || NeedTerm;  //is equal to !(NeedMonthlyIncome && !NeedTerm);
            CalcLineOfCredit = true;
            if (NeedMonthlyIncome && ((NeedTerm && MPPaymentAmount == 0 && MPTerm > 0) || !NeedTerm))
                CalcLineOfCredit = false;

            OtherCashOut = mp.OtherCashOut;
            Repairs = mp.Repairs;
            MortgageBalance = mp.MortgageBalance;
            LenderFees = mp.LenderFees;
            OtherClosingCosts = mp.OtherClosingCosts;
            SharedAppreciation = mp.MortgageInfo.SharedAppreciation ?? 0;
            switch (mortgageBuffer.HecmFnmaFlag)
            {
                case 2:
                    LendingLimit = ((Product)productHash[ProductFlag.FNMA]).ConformingLendingLimit;
                    break;
                case 1:
                    LendingLimit = mp.Property.LendingLimit;
                    break;
                default:
                    LendingLimit = 0;
                    break;
            }

            HomeValue = mp.Property.SPValue == null ? 0 : (decimal)mp.Property.SPValue;
            mortgageBuffer.LifeExpectancy = (int)mp.YoungestBorrower.LifeExpectancy;

            Borrower youngestBorrower = mp.YoungestBorrower;
            mortgageBuffer.Borrower1Birthdate = youngestBorrower != null ? 
                ((DateTime)youngestBorrower.DateOfBirth).Date : DateTime.MinValue.Date;

            mortgageBuffer.ClosingDate = DateTime.Now.Date;
            if (mp.MortgageInfo.ClosingDate != null)
                mortgageBuffer.ClosingDate = ((DateTime)mp.MortgageInfo.ClosingDate).Date;
        }

        private void LoadFromProduct(ProductFlag prodFlag)
        {
            Product product = (Product)productHash[prodFlag];
            mortgageBuffer.LoadFromProduct(product,mp.Lender.CompanyId,DateTime.Now);
        }
        #endregion

        #region Public methods
        public string Calculate(LaunchStep launchStep)
        {
            MortgageBuffer = new MortgageBuffer();
            HecmFnmaFlag = mp.ProductCalcType;
            LoadFromMortgageProfile();
            LoadFromProduct(mp.ProductCalcType);

            string resMessage = Validate();
            if (String.IsNullOrEmpty(resMessage))
            {
                RevMort revmort = new RevMort();
                try
                {
                    revmort.ReverseMortgageCalc(MortgageBuffer);
                }
                catch (Exception ex)
                {
                    resMessage = String.Format("{0} calculation error occured \nError : {1}", launchStep, ex.Message);
                }
            }

            launchHash[launchStep] = MortgageBuffer;
            return resMessage;
        }

        public string Calculate(MortgageBuffer mortBuf, LaunchStep launchStep)
        {
            string resMessage = String.Empty;
            RevMort revmort = new RevMort();
            try
            {
                revmort.ReverseMortgageCalc(mortBuf);
            }
            catch (Exception ex)
            {
                resMessage = String.Format("{0} calculation error occured \nError : {1}", launchStep, ex.Message);
            }

            launchHash[launchStep] = mortBuf;
            return resMessage;
        }

        public string Calculate(ProductFlag prodFlag)
        {
            MortgageBuffer = new MortgageBuffer();
            HecmFnmaFlag = prodFlag;
            LoadFromMortgageProfile();
            LoadFromProduct(prodFlag);

            string resMessage = Validate();
            if (String.IsNullOrEmpty(resMessage))
            {
                RevMort revmort = new RevMort();
                try
                {
                    revmort.ReverseMortgageCalc(MortgageBuffer);
                }
                catch (Exception ex)
                {
                    resMessage = String.Format("{0} product calculation error occured \nError : {1}", prodFlag, ex.Message);
                }
            }

//            if (mp.Product.CalculationType == prodFlag)
//                mp.Calculator = MortgageBuffer;
//                mp.Calculator = this;
            return resMessage;
        }

        public string Calculate()
        {
            MortgageBuffer = new MortgageBuffer();
//            mp.Calculator = MortgageBuffer;
//            mp.Calculator = this;
            
            
            if (mp.ProductCalcType == ProductFlag.None)
                return "You need to select a product";

            HecmFnmaFlag = mp.ProductCalcType;
            LoadFromMortgageProfile();
            LoadFromProduct(mp.ProductCalcType);

            string resMessage = ValidateFirst();
            if (String.IsNullOrEmpty(resMessage))
                resMessage = Validate();

            if (String.IsNullOrEmpty(resMessage))
            {
                RevMort revmort = new RevMort();
                try
                {
                    revmort.ReverseMortgageCalc(MortgageBuffer);
                }
                catch (Exception ex)
                {
                    resMessage = String.Format("{0} product calculation error occured \nError : {1}", mp.ProductCalcType, ex.Message);
                }
            }

            return resMessage;
        }

        public string Save()
        {
            return Save(null, null, null, null);
        }
        public string Save(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine)
        {
            return Save(mpPaymentAmount, mpTerm, mpInitialDraw, mpCreditLine, null);
        }
        public string Save(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine, int? mpProductID)
        {
            string errMessage;
            bool resAction;
            try
            {
                int resSave = db.ExecuteScalarInt("SaveAdvancedCalculator", 
                                            ID, 
                                            NeedInitialDraw, 
                                            NeedCreditLine, 
                                            NeedMonthlyIncome, 
                                            NeedTerm);
                resAction = Convert.ToBoolean(resSave);
            }
            catch (Exception)
            {
                resAction = false;
            }

            MortgageInfo mortgageInfo = mp.MortgageInfo;
            if (mpPaymentAmount != null)
                resAction &= mp.UpdateObject("MortgageInfo.PaymentAmount", ((decimal)mpPaymentAmount).ToString(), mortgageInfo.ID, out errMessage);
            if (mpTerm != null)
                resAction &= mp.UpdateObject("MortgageInfo.Term", ((decimal)mpTerm).ToString(), mortgageInfo.ID, out errMessage);
            if (mpInitialDraw != null)
                resAction &= mp.UpdateObject("MortgageInfo.InitialDraw", ((decimal)mpInitialDraw).ToString(), mortgageInfo.ID, out errMessage);
            if (mpCreditLine != null)
                resAction &= mp.UpdateObject("MortgageInfo.CreditLine", ((decimal)mpCreditLine).ToString(), mortgageInfo.ID, out errMessage);
            if (mpProductID != null)
            {
                int lenderAffiliateID = mp.MortgageInfo.LenderAffiliateID;
                DataView dvLenderProduct = mp.CurrentPage.GetDictionary("vwLenderProduct");
                dvLenderProduct.RowFilter = String.Format("id={0} AND LenderId={1}", (decimal)mpProductID, lenderAffiliateID);
                if (dvLenderProduct.Count > 0)
                    resAction &= mp.UpdateObject("MortgageInfo.ProductId", ((decimal)mpProductID).ToString(), mortgageInfo.ID, out errMessage);
                else
                {
                    dvLenderProduct.RowFilter = String.Format("id={0} AND LenderId<>{1}", (decimal)mpProductID, mp.CurrentPage.CurrentUser.CompanyId);
                    dvLenderProduct.Sort = "LenderId";
                    if (dvLenderProduct.Count > 0)
                    {
                        lenderAffiliateID = Convert.ToInt32(dvLenderProduct[0]["LenderId"]);
                        resAction &= mp.UpdateObject("MortgageInfo.LenderAffiliateID", lenderAffiliateID.ToString(), mortgageInfo.ID, out errMessage);
                        resAction &= mp.UpdateObject("MortgageInfo.ProductId", ((decimal)mpProductID).ToString(), mortgageInfo.ID, out errMessage);
                    }
                }
            }

            return resAction ? String.Empty : "Database error occured";
        }

        public string ValidateClosingDate()
        {
/*            DateTime nearestTuesday = EffectiveDate;
            nearestTuesday = nearestTuesday.DayOfWeek >= DayOfWeek.Tuesday ?
                    nearestTuesday.AddDays(DayOfWeek.Tuesday - nearestTuesday.DayOfWeek) :
                    nearestTuesday.AddDays(-(int)nearestTuesday.DayOfWeek - 1 - (int)DayOfWeek.Saturday + (int)DayOfWeek.Tuesday);

            if (Product.ValidateProductsRate(nearestTuesday) < 3)
                return  "The closing date you provided is too far into the future. " + System.Environment.NewLine +
                        "We do not have rates available at this time for that date. " + System.Environment.NewLine +
                        "The calculations returned reflect today’s rates. ";*/

            return String.Empty;
        }
        #endregion
    }
}

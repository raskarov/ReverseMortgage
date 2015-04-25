namespace LoanStar.Common
{

    public class LeadCalculator 
    {

        #region constants

        private const decimal DEFAULTDESIREDGROSSINCOME = 500000;
        private const decimal DEFAULTAVERAGEMAXCLAIM = 250000;
        private const float DEFAULTSTANDARDCOMMISSION = 1;
        private const float DEFAULTESTIMATEDFALLOUT = 10;

        private const float DEFAULTSELFSOURCEDCLOSING = 100;
        private const float DEFAULTNETCOMMISSION1 = 100;
        private const float DEFAULTPERCENTLEADSTOAPPS1 = 5;

        private const float DEFAULTRMCREFERREDCLOSING = 0;
        private const float DEFAULTNETCOMMISSION2 = 75;
        private const float DEFAULTPERCENTLEADSTOAPPS2 = 10;

        private const float DEFAULTBROKERINCLOSING1 = 0;
        private const float DEFAULTNETCOMMISSION3 = 100;
        private const float DEFAULTPERCENTLEADSTOAPPS3 = 5;

        private const float DEFAULTBROKERINCLOSING2 = 0;
        private const float DEFAULTNETCOMMISSION4 = 65;        
        private const float DEFAULTPERCENTLEADSTOAPPS4 = 5;      
        #endregion

        #region fields
        private decimal desiredGrossIncom = DEFAULTDESIREDGROSSINCOME;
        private decimal productionNeeded;
        private float standardCommission = DEFAULTSTANDARDCOMMISSION;
        private decimal averageMaxClaim = DEFAULTAVERAGEMAXCLAIM;
        private float estimatedFallOut = DEFAULTESTIMATEDFALLOUT;
        private decimal goalForYearEnd;
        private decimal averagePerMonth;
        private decimal averagePerWeek;
        private Closing selfSourcedClosing;
        private Closing rmcreferredClosing;
        private Closing brokerInClosingl;
        private Closing brokerInClosing2;

        #region unused
        //private float selfSourcedClosings = DEFAULTSELFSOURCEDCLOSING;
        //private float netCommission1 = DEFAULTNETCOMMISSION1;
        //private float perMonthUnitVolume1;
        //private decimal incomeMonth1;
        //private decimal incomeYear1;
        //private float percentOfLeadsToApps1 = DEFAULTPERCENTLEADSTOAPPS1;
        //private decimal reqLeadsPerMonth1;
        //private decimal reqLeadsPerWeek1;
        //private decimal reqLeadsPerWeekDay1;

        //private float rmcRefferedClosings = DEFAULTRMCREFERREDCLOSING;
        //private float netCommission2 = DEFAULTNETCOMMISSION2;
        //private float perMonthUnitVolume2;
        //private decimal incomeMonth2;
        //private decimal incomeYear2;
        //private float percentOfLeadsToApps2 = DEFAULTPERCENTLEADSTOAPPS2;
        //private decimal reqLeadsPerMonth2;
        //private decimal reqLeadsPerWeek2;

        //private float brokerInClosings1 = DEFAULTBROKERINCLOSING1;
        //private float netCommission3 = DEFAULTNETCOMMISSION3;
        //private float perMonthUnitVolume3;
        //private decimal incomeMonth3;
        //private decimal incomeYear3;
        //private float percentOfLeadsToApps3 = DEFAULTPERCENTLEADSTOAPPS3;
        //private decimal reqLeadsPerMonth3;
        //private decimal reqLeadsPerWeek3;
        //private decimal reqLeadsPerWeekDay3;

        //private float brokerInClosings2 = DEFAULTBROKERINCLOSING2;
        //private float netCommission4 = DEFAULTNETCOMMISSION3;
        //private float perMonthUnitVolume4;
        //private decimal incomeMonth4;
        //private decimal incomeYear4;
        //private float percentOfLeadsToApps4 = DEFAULTPERCENTLEADSTOAPPS3;
        //private decimal reqLeadsPerMonth4;
        //private decimal reqLeadsPerWeek4;
        #endregion

        #endregion

        #region properties
        public decimal DesiredGrossIncom
        {
            get { return desiredGrossIncom;}
            set { desiredGrossIncom = value; }
        }
        public decimal ProductionNeeded
        {
            get { return productionNeeded; }
            set { productionNeeded = value; }
        }
        public float StandardCommission
        {
            get { return standardCommission; }
            set { standardCommission = value; }
        }
        public decimal AverageMaxClaim
        {
            get { return averageMaxClaim; }
            set { averageMaxClaim = value; }
        }
        public float EstimatedFallOut
        {
            get { return estimatedFallOut; }
            set { estimatedFallOut = value; }
        }
        public decimal GoalForYearEnd
        {
            get { return goalForYearEnd; }
            set { goalForYearEnd = value; }
        }
        public decimal AveragePerMonth
        {
            get { return averagePerMonth; }
            set { averagePerMonth = value; }
        }
        public decimal AveragePerWeek
        {
            get { return averagePerWeek; }
            set { averagePerWeek = value; }
        }
        public Closing SelfSourcedClosing
        {
            get { return selfSourcedClosing; }
            set { selfSourcedClosing = value; }
        }
        public Closing RMCreferredClosing
        {
            get { return rmcreferredClosing; }
            set { rmcreferredClosing = value; }
        }
        public Closing BrokerInClosingl
        {
            get { return brokerInClosingl; }
            set { brokerInClosingl = value; }
        }
        public Closing BrokerInClosing2
        {
            get { return brokerInClosing2; }
            set { brokerInClosing2 = value; }
        }
        #region unused
        //private float SelfSourcedClosings
        //{
        //    get { return selfSourcedClosings; }
        //    set { selfSourcedClosings = value; }
        //}
        //private float NetCommission1
        //{
        //    get { return netCommission1; }
        //    set { netCommission1= value; }
        //}
        //private float PerMonthUnitVolume1
        //{
        //    get { return perMonthUnitVolume1; }
        //    set { perMonthUnitVolume1= value; }
        //}
        //private decimal IncomeMonth1
        //{
        //    get { return incomeMonth1; }
        //    set { incomeMonth1 = value; }
        //}
        //private decimal IncomeYear1
        //{
        //    get { return incomeYear1; }
        //    set { incomeYear1 = value; }
        //}
        //private float PercentOfLeadsToApps1
        //{
        //    get { return percentOfLeadsToApps1; }
        //    set { percentOfLeadsToApps1 = value; }
        //}
        //private decimal ReqLeadsPerMonth1
        //{
        //    get { return reqLeadsPerMonth1; }
        //    set { reqLeadsPerMonth1 = value; }
        //}
        //private decimal ReqLeadsPerWeek1
        //{
        //    get { return reqLeadsPerWeek1; }
        //    set { reqLeadsPerWeek1 = value; }
        //}
        //private decimal ReqLeadsPerWeekDay1
        //{
        //    get { return reqLeadsPerWeekDay1; }
        //    set { reqLeadsPerWeekDay1 = value; }
        //}
        //private float RmcRefferedClosings
        //{
        //    get { return rmcRefferedClosings; }
        //    set { rmcRefferedClosings = value; }
        //}
        //private float NetCommission2
        //{
        //    get { return netCommission2; }
        //    set { netCommission2 = value; }
        //}
        //private float PerMonthUnitVolume2
        //{
        //    get { return perMonthUnitVolume2; }
        //    set { perMonthUnitVolume2 = value; }
        //}
        //private decimal IncomeMonth2
        //{
        //    get { return incomeMonth2; }
        //    set { incomeMonth2 = value; }
        //}
        //private decimal IncomeYear2
        //{
        //    get { return incomeYear2; }
        //    set { incomeYear2 = value; }
        //}
        //private float PercentOfLeadsToApps2
        //{
        //    get { return percentOfLeadsToApps2; }
        //    set { percentOfLeadsToApps2 = value; }
        //}
        //private decimal ReqLeadsPerMonth2
        //{
        //    get { return reqLeadsPerMonth2; }
        //    set { reqLeadsPerMonth2 = value; }
        //}
        //private decimal ReqLeadsPerWeek2
        //{
        //    get { return reqLeadsPerWeek2; }
        //    set { reqLeadsPerWeek2 = value; }
        //}
        //private float BrokerInClosings1
        //{
        //    get { return brokerInClosings1; }
        //    set { brokerInClosings1 = value; }
        //}
        //private float NetCommission3
        //{
        //    get { return netCommission3; }
        //    set { netCommission3 = value; }
        //}
        //private float PerMonthUnitVolume3
        //{
        //    get { return perMonthUnitVolume3; }
        //    set { perMonthUnitVolume3 = value; }
        //}
        //private decimal IncomeMonth3
        //{
        //    get { return incomeMonth3; }
        //    set { incomeMonth3 = value; }
        //}
        //private decimal IncomeYear3
        //{
        //    get { return incomeYear3; }
        //    set { incomeYear3 = value; }
        //}
        //private float PercentOfLeadsToApps3
        //{
        //    get { return percentOfLeadsToApps3; }
        //    set { percentOfLeadsToApps3 = value; }
        //}
        //private decimal ReqLeadsPerMonth3
        //{
        //    get { return reqLeadsPerMonth3; }
        //    set { reqLeadsPerMonth3 = value; }
        //}
        //private decimal ReqLeadsPerWeek3
        //{
        //    get { return reqLeadsPerWeek3; }
        //    set { reqLeadsPerWeek3 = value; }
        //}
        //private decimal ReqLeadsPerWeekDay3
        //{
        //    get { return reqLeadsPerWeekDay3; }
        //    set { reqLeadsPerWeekDay3 = value; }
        //}
        //private float BrokerInClosings2
        //{
        //    get { return brokerInClosings2; }
        //    set { brokerInClosings2 = value; }
        //}
        //private float NetCommission4
        //{
        //    get { return netCommission4; }
        //    set { netCommission4 = value; }
        //}
        //private float PerMonthUnitVolume4
        //{
        //    get { return perMonthUnitVolume4; }
        //    set { perMonthUnitVolume4 = value; }
        //}
        //private decimal IncomeMonth4
        //{
        //    get { return incomeMonth4; }
        //    set { incomeMonth4 = value; }
        //}
        //private decimal IncomeYear4
        //{
        //    get { return incomeYear4; }
        //    set { incomeYear4 = value; }
        //}
        //private float PercentOfLeadsToApps4
        //{
        //    get { return percentOfLeadsToApps4; }
        //    set { percentOfLeadsToApps4 = value; }
        //}
        //private decimal ReqLeadsPerMonth4
        //{
        //    get { return reqLeadsPerMonth4; }
        //    set { reqLeadsPerMonth4 = value; }
        //}
        //private decimal ReqLeadsPerWeek4
        //{
        //    get { return reqLeadsPerWeek4; }
        //    set { reqLeadsPerWeek4 = value; }
        //}
        #endregion

        #endregion

        #region constructor
        public LeadCalculator()
            : this(DEFAULTDESIREDGROSSINCOME, DEFAULTSTANDARDCOMMISSION, DEFAULTAVERAGEMAXCLAIM, DEFAULTESTIMATEDFALLOUT)
        {
        }
        public LeadCalculator(decimal grossIncome, float standardCommission, decimal averageMaxClaim, float estimatedFallOut)
        {
            this.standardCommission = standardCommission;
            this.averageMaxClaim = averageMaxClaim;
            desiredGrossIncom = grossIncome;
            this.estimatedFallOut = estimatedFallOut;
            selfSourcedClosing = new Closing(DEFAULTSELFSOURCEDCLOSING, DEFAULTNETCOMMISSION1, DEFAULTPERCENTLEADSTOAPPS1);
            rmcreferredClosing = new Closing(DEFAULTRMCREFERREDCLOSING,DEFAULTNETCOMMISSION2,DEFAULTPERCENTLEADSTOAPPS2);
            brokerInClosingl = new Closing(DEFAULTBROKERINCLOSING1,DEFAULTNETCOMMISSION3,DEFAULTPERCENTLEADSTOAPPS3);
            brokerInClosing2 = new Closing(DEFAULTBROKERINCLOSING2,DEFAULTNETCOMMISSION4,DEFAULTPERCENTLEADSTOAPPS4);
            productionNeeded = desiredGrossIncom * 100;
        }

        #endregion

        #region methods
        public void Calculate()
        {
            selfSourcedClosing.Calculate(desiredGrossIncom, averageMaxClaim, standardCommission, estimatedFallOut);
            rmcreferredClosing.Calculate(desiredGrossIncom, averageMaxClaim, standardCommission, estimatedFallOut);
            brokerInClosingl.Calculate(desiredGrossIncom, averageMaxClaim, standardCommission, estimatedFallOut);
            brokerInClosing2.Calculate(desiredGrossIncom, averageMaxClaim, standardCommission, estimatedFallOut);
            averagePerMonth = selfSourcedClosing.PerMonthUnitValue + rmcreferredClosing.PerMonthUnitValue + brokerInClosingl.PerMonthUnitValue;
            goalForYearEnd = averagePerMonth * Closing.MONTHNUMBER;
            averagePerWeek = goalForYearEnd / Closing.WEEKNUMBER;
        }
        #endregion
    }
    public class Closing
    {
        #region constants
        public const int MONTHNUMBER = 12;
        public const int WEEKNUMBER = 52;
        private const int DAYNUMBER = 261;
        #endregion

        #region fields
        private float closingPercent;
        private float netCommission;
        private decimal perMonthUnitValue;
        private decimal incomeMonth;
        private decimal incomeYear;
        private float percentLeadsToApps;
        private decimal reqLeadsperMonth;
        private decimal reqLeadsperWeek;
        private decimal reqLeadsperWeekDay;
        #endregion

        #region properties
        public float ClosingPercent
        {
            get { return closingPercent; }
            set { closingPercent = value; }
        }
        public float NetCommission
        {
            get { return netCommission; }
            set { netCommission = value; }
        }
        public decimal PerMonthUnitValue
        {
            get { return perMonthUnitValue; }
        }
        public decimal IncomeMonth
        {
            get { return incomeMonth; }
        }
        public decimal IncomeYear
        {
            get { return incomeYear; }
        }
        public decimal ReqLeadsperMonth
        {
            get { return reqLeadsperMonth; }
        }
        public decimal ReqLeadsperWeek
        {
            get { return reqLeadsperWeek; }
        }
        public decimal ReqLeadsperWeekDay
        {
            get { return reqLeadsperWeekDay; }
        }
        public float PercentLeadsToApps
        {
            get { return percentLeadsToApps; }
            set { percentLeadsToApps = value; }
        }
        #endregion

        #region constructor
        public Closing():this(0,100,5)
        { 
        }
        public Closing(float closing, float netCommission, float percentLeadsToApps)
        {
            closingPercent = closing;
            this.netCommission = netCommission;
            this.percentLeadsToApps = percentLeadsToApps;
        }
        #endregion

        #region methods
        public void Calculate(decimal grossIncome, decimal averageMaxClaim, float standardCommission, float estimatedFallOut)
        {
            incomeYear = grossIncome * (decimal)(closingPercent/100);
            incomeMonth = incomeYear / MONTHNUMBER;
            perMonthUnitValue = incomeMonth / (averageMaxClaim * (decimal)(standardCommission / 100) * (decimal)(netCommission / 100));
            reqLeadsperMonth = (perMonthUnitValue * (decimal)(estimatedFallOut / 100)) / (decimal)(percentLeadsToApps / 100) + (perMonthUnitValue /(decimal) (percentLeadsToApps / 100));
            reqLeadsperWeek = (reqLeadsperMonth * MONTHNUMBER) / WEEKNUMBER;
            reqLeadsperWeekDay = (reqLeadsperMonth * MONTHNUMBER) / DAYNUMBER;
        }
        #endregion
    }
}

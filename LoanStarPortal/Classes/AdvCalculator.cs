using System;
using System.Data;
using System.Text;
using System.Reflection;
using System.Collections;
using RevMortLib;

namespace LoanStar.Common
{
    
    public class AdvCalculator : BaseObject
    {
        private const string GETFIELDS = "GetAdvancedCalculatorFields";
        private const string SAVE = "SaveAdvCalculator";

        private readonly MortgageProfile mp = null;
        private MortgageBuffer advCalculatorBuffer = null;

        private bool needInitialDraw = false;
        private bool needCreditLine = false;
        private bool needMonthlyIncome = false;
        private bool needTerm = false;
        private readonly bool needSaveInMortgage = false;
        private DateTime dtRates1;
        private DateTime dtRates2;

        private Hashtable products;
        private ArrayList productsOrder;
        private Hashtable scenarioValues;
        private Product currentProduct;

        private readonly DataView dvProducts;
        private int selectedProductId = -1;
        private decimal financedOriginationFee;
        private decimal reserveTotalAmount = 0;
        private decimal loanOriginatorFees = 0;
        private decimal initialDraw = 0;
        private decimal creditLine = 0;
        private decimal paymentAmount = 0;
        private decimal term = 0;
        private decimal totalClosingCostsOther;
        private decimal totalClosingCostsInvoice;
        private string calcError = String.Empty;
        private Hashtable serviceFeeSelection;
        private string feeSelection = String.Empty;
        private readonly Logger log;
        private bool useAppDateRates = false;
        private bool isNetAvavilableNegative = false;

        #region poperties
        public Product CurrentProduct
        {
            get { return currentProduct; }
        }
        public decimal MaxYieldSpreadPremium
        {
            get
            {
                return NetPrincipleLimit * (decimal)(mp.MortgageInfo.YieldSpreadRate ?? 0)/100 ;
            }
        }
        public decimal ProductLendingLimit
        {
            get
            {
                decimal res = 0;
                if(currentProduct!=null)
                {
                    res = currentProduct.LendingLimit;
                }
                return res;
            }
        }
        public string IncomeType
        {
            get
            {
                string res = String.Empty;
                if(!needTerm)
                {
                    res = "Tenure";
                }
                else if(needMonthlyIncome)
                {
                    res = "Term";
                }
                return res;
            }
        }
        public int TermWholeYears
        {
            get
            {
                return (int)(FinalTerm/12);
            }
        }
        public int RemainderMonths
        {
            get
            {
                return (int)FinalTerm - TermWholeYears;
            } 
        }
        public decimal Percentage
        {
            get
            {
                decimal res = 0;
                if(mp.Property.SPValue!=null)
                {
                    res = (PledgedValueOrLimit / (decimal)mp.Property.SPValue) * 100;
                }
                return res;
            }
        }
        public decimal TotalPayoffPOC
        {
            get { return mp.Property.TotalPayoffPOC; }
        }
        public decimal CashAvailForMonthlyPayments
        {
            get { return NetAvailable - CreditLine - InitialDraw; }
        }
        public bool IsNetAvavilableNegative
        {
            get { return isNetAvavilableNegative; }
        }
        public bool UseAppDateRates
        {
            get { return useAppDateRates; }
            set { useAppDateRates = value; }
        }
        public decimal TotalClosingCostsPlusSetAside
        {
            get { return TotalClosingCosts + ServiceSetAside; }
        }
        public decimal HECMRefiNetAvailableAfterPayoff
        {
            get { return PrincipalLimit - TotalClosingCostsPlusSetAside - mp.MortgageInfo.PayoffOldHECMAmount; }
        }
        public decimal PrincipleLimitIncrease
        {
            get { return PrincipalLimit - (mp.MortgageInfo.HECMRefiPrincipleLimitOldLoan ?? 0); }
        }
        public string FeeSelection
        {
            get { return feeSelection;}
        }
        public decimal NetPrincipleLimit
        {
            get { return PrincipalLimit - ServiceSetAside; }
        }
        public Hashtable ServiceeFeeSelection
        {
            get 
            {
                if (serviceFeeSelection == null)
                {
                    serviceFeeSelection = new Hashtable();
                }
                return serviceFeeSelection; 
            }
        }
        public string CalculatorError
        {
            get { return calcError; }
            set { calcError = value; }
        }
        public Hashtable ScenarioValues
        {
            get { return scenarioValues; }
            set { scenarioValues = value; }
        }
        public int SelectedProductId
        {
            get { return selectedProductId; }
            set
            {
                selectedProductId = value;
                SetSelectedProduct();
            }
        }
        public decimal FinancedOriginationFee
        {
            get { return financedOriginationFee; }
            set { financedOriginationFee = value; }
        }
        public decimal AmountHeldBack
        {
            get
            {
                decimal res = MonthlyPaymentInsTaxReduction;
                if (mp != null)
                {
                    if (mp.MortgageInfo != null)
                    {
                        res += mp.MortgageInfo.TotalRepairSetAsides;
                    }
                }
                return res;
            }
        }
        public decimal MonthlyPaymentInsTaxReduction
        {
            get
            {
                decimal res = 0;
                if (mp != null)
                {
                    decimal hp = 0;
                    decimal fp = 0;
                    if (mp.Property != null)
                    {
                        if (mp.Property.HazPremium != null) hp = (decimal)mp.Property.HazPremium;
                        if (mp.Property.FldPremium != null) fp = (decimal)mp.Property.FldPremium;
                    }
                    int years = 0;
                    if (mp.MortgageInfo != null)
                    {
                        if (mp.MortgageInfo.EscrowYears != null) years = (int)mp.MortgageInfo.EscrowYears;
                    }
                    res = (hp + fp) * years;
                }
                return res;
            }
        }
        public decimal LessMIPPoints
        {
            get
            {
                return (TotalClosingCosts - UpFrontPremium);
            }
        }
        public decimal InitialDrawOnPrintScreen
        {
            get
            {
                decimal res = 0;
                if (mp != null)
                {
                    res = mp.MortgageInfo.TotalPayoffs + ((mp.MortgageInfo.InitialDraw == null) ? 0 : (decimal)mp.MortgageInfo.InitialDraw);
                }
                return res;
            }
        }
        public decimal UPB
        {
            get
            {
                decimal res = 0;
                if (mp != null)
                {
                    res = mp.MortgageInfo.TotalClosingCosts1400 + mp.MortgageInfo.TotalPayoffs + ((mp.MortgageInfo.InitialDraw == null) ? 0 : (decimal)mp.MortgageInfo.InitialDraw);
                }
                return res;
            }
        }
        public decimal TotalClosingCostsOther
        {
            get { return totalClosingCostsOther; }
            set { totalClosingCostsOther = value; }
        }
        public bool NeedTerm
        {
            get { return needTerm; }
            set { needTerm = value; }
        }
        public bool NeedMonthlyIncome
        {
            get { return needMonthlyIncome; }
            set { needMonthlyIncome = value; }
        }
        public bool NeedCreditLine
        {
            get { return needCreditLine; }
            set { needCreditLine = value; }
        }
        public bool NeedInitialDraw
        {
            get { return needInitialDraw; }
            set { needInitialDraw = value; }
        }
        public decimal ReserveTotalAmount
        {
            get { return reserveTotalAmount; }
            set { reserveTotalAmount = value; }
        }
        public decimal MPTerm
        {
            get { return term;}
        }
        public decimal MPPaymentAmount
        {            
            get { return paymentAmount; }
        }
        public decimal MPInitialDraw
        {
            get { return initialDraw; }
        }
        public decimal MPCreditLine
        {
            get { return creditLine; }
            set { creditLine = value; }
        }
        public decimal LoanOriginationFees
        {
            get { return loanOriginatorFees; }
            set { loanOriginatorFees = value; }
        }
        public decimal OtherFinancedCosts
        {
            get { return TotalClosingCostsInvoice - LoanOriginationFees - UpFrontPremium; }
        }
        public decimal TotalInitialCharges
        {
            get { return UpFrontPremium + LoanOriginationFees + OtherFinancedCosts; }
//            get { return LoanOriginationFees + OtherFinancedCosts; }
        }
        public string Tenure_Term
        {
            get
            {
                return !needTerm ? "Tenure":FinalTerm+" month(s)";
            }
        }
        public decimal NetAvailable
        {
            get
            {
                return NetPrincipleLimit - TotalClosingCosts - MortgageBalance - Repairs;
            }
        }
        public double BorrowerAge
        {
            get { return advCalculatorBuffer.BorrowerAge; }
        }
        public double CoBorrowerAge
        {
            get { return advCalculatorBuffer.CoBorrowerAge; }
        }
        public double InitialBalance
        {
            get { return advCalculatorBuffer.InitialBalance; }
        }
        public double NetPrincipalLimitSchedPmt
        {
            get { return advCalculatorBuffer.NetPrincipalLimitSchedPmt; }
        }
        public double YoungBorrowerAge
        {
            get { return advCalculatorBuffer.YoungBorrowerAge; }
        }
        public decimal UnallocatedFunds
        {
            get { return LineOfCredit - CreditLine; }
        }
        public MortgageBuffer AdvCalculatorBuffer
        {
            get { return advCalculatorBuffer; }
        }
        #region data from buffer
        public double PublishedInitialRate
        {
//            get { return advCalculatorBuffer.PublishedInitialRate; }
            get { return advCalculatorBuffer.InitialRate; }
        }
        public double PublishedExpectedRate
        {
            get { return advCalculatorBuffer.PublishedExpectedRate; }
            set
            {
                advCalculatorBuffer.PublishedExpectedRate = value;
                advCalculatorBuffer.Index = (advCalculatorBuffer.PublishedExpectedRate < advCalculatorBuffer.ProductExpectedFloorRate ? advCalculatorBuffer.ProductExpectedFloorRate : advCalculatorBuffer.PublishedExpectedRate) - advCalculatorBuffer.Margin;
            }
        }
        public double EffectiveRateCap
        {
            get { return advCalculatorBuffer.EffectiveRateCap; }
            set { advCalculatorBuffer.EffectiveRateCap = value; }
        }
        public decimal MonthlyServiceFee
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.MonthlyServiceFee); }
            set { advCalculatorBuffer.MonthlyServiceFee = Convert.ToDouble(value); }
        }
        public decimal HomeValue
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.HomeValue); }
            set { advCalculatorBuffer.HomeValue = Convert.ToDouble(value); }
        }
        public decimal PledgedValueOrLimit
        {
            get { return advCalculatorBuffer.PledgedValueOrLimit; }
        }
        public double CreditLineGrowthRate
        {
            get { return advCalculatorBuffer.CreditLineGrowthRate; }
        }
        public double Age
        {
            get
            {
                return advCalculatorBuffer.YoungBorrowerAge;
            }
            set { advCalculatorBuffer.YoungBorrowerAge = value; }
        }
        public decimal PrincipalLimit
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.PrincipalLimit); }
            set { advCalculatorBuffer.PrincipalLimit = Convert.ToDouble(value); }
        }
        public decimal ServiceSetAside
        {
            get { return advCalculatorBuffer.HecmFnmaFlag == 2 ? 0 : Convert.ToDecimal(advCalculatorBuffer.PresentValueServiceFee); }
        }
        public decimal UpFrontPremium
        {
            get { return advCalculatorBuffer.UpFrontPremium; }
        }
        public decimal TotalOtherClosingCosts
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.TotalOtherClosingCosts); }
        }
        public decimal MortgageBalance
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.MortgageBalance); }
            set { advCalculatorBuffer.MortgageBalance = Convert.ToDouble(value); }
        }
        public decimal NetPrincipalLimit
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.NetPrincipalLimit); }
        }
        public decimal InitialDraw
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.InitialDraw); }
            set { advCalculatorBuffer.InitialDraw = Convert.ToDouble(value); }
        }
        public decimal Repairs
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.Repairs); }
            set { advCalculatorBuffer.Repairs = Convert.ToDouble(value); }
        }
        public decimal OtherCashOut
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.OtherCashOut); }
            set { advCalculatorBuffer.OtherCashOut = Convert.ToDouble(value); }
        }
        public decimal CreditLine
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.CreditLine); }
            set { advCalculatorBuffer.CreditLine = Convert.ToDouble(value); }
        }
        public decimal LineOfCredit
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.LineOfCredit); }
        }
        public decimal PeriodicPayment
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.PeriodicPayment); }
        }
        public decimal FinalTerm
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.FinalTerm); }
            set { advCalculatorBuffer.FinalTerm = Convert.ToDouble(value); }
        }
        public decimal PaymentAmount
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.PaymentAmount); }
            set { advCalculatorBuffer.PaymentAmount = Convert.ToDouble(value); }
        }
        public decimal Term
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.Term); }
            set { advCalculatorBuffer.Term = Convert.ToDouble(value); }
        }
        public bool CalcLineOfCredit
        {
            get { return Convert.ToBoolean(advCalculatorBuffer.CalcLineOfCredit); }
            set { advCalculatorBuffer.CalcLineOfCredit = Convert.ToInt32(value); }
        }
        public decimal LenderFees
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.LenderFees); }
            set { advCalculatorBuffer.LenderFees = Convert.ToDouble(value); }
        }
        public decimal OtherClosingCosts
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.OtherClosingCosts); }
            set { advCalculatorBuffer.OtherClosingCosts = Convert.ToDouble(value); }
        }
        public decimal SharedAppreciation
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.SharedAppreciation); }
            set { advCalculatorBuffer.SharedAppreciation = Convert.ToDouble(value); }
        }
        public decimal LendingLimit
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.LendingLimit); }
            set { advCalculatorBuffer.LendingLimit = Convert.ToDouble(value); }
        }
        public double Margin
        {
            get { return advCalculatorBuffer.PublishedInitialMargin; }
        }
        public double ExpectedMargin
        {
            get { return advCalculatorBuffer.Margin; }
        }
        public decimal PropertyAppreciation
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.PropertyAppreciation); }
        }
        public decimal UpfrontMiRate
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.UpfrontMiRate); }
        }
        public decimal RenewalMiRate
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.RenewalMiRate); }
        }
        public decimal TotalClosingCosts
        {
            get { return Convert.ToDecimal(advCalculatorBuffer.TotalClosingCosts); }
        }
        public decimal TotalClosingCostsInvoice
        {
            get { return totalClosingCostsInvoice; }
            set { totalClosingCostsInvoice = value; }
        }
        public double Index
        {
            get { return advCalculatorBuffer.Index; }
            set { advCalculatorBuffer.Index = value; }
        }
        public double PublishedInitialIndex
        {
            get { return advCalculatorBuffer.PublishedInitialIndex; }
        }
        public double ExpectedIntRate
        {
            get { return advCalculatorBuffer.ExpectedIntRate; }
        }
        public double PresentValueServiceFee
        {
            get { return advCalculatorBuffer.PresentValueServiceFee; }
        }
        public double NetLineOfCredit
        {
            get { return advCalculatorBuffer.NetLineOfCredit; }
        }
        public double TotalAnnualLoanCost
        {
            get { return advCalculatorBuffer.TotalAnnualLoanCost; }
        }
        #endregion

        public string ProductName
        {
            get { return currentProduct.Name; }
        }
        #endregion

        #region Constructors
        public AdvCalculator(MortgageProfile _mp, bool needSave)
        {
            needSaveInMortgage = needSave;
            mp = _mp;
            if (mp != null)
            {
                ID = mp.ID;
            }
            advCalculatorBuffer = new MortgageBuffer();
            RestoreSavedValues();

        }
        public AdvCalculator(MortgageProfile _mp)
            : this(_mp,true)
        {
        }
        public AdvCalculator(MortgageProfile _mp, DataView _dvProducts, bool needSave, Logger log_)
        {
            log = log_;
            needSaveInMortgage = needSave;
            mp = _mp;
            if (mp != null)
            {
                ID = mp.ID;
                if(log!=null) WriteToLog(String.Format("Calculator created for Loan (Id={0}, Borrower: {1}, {2})", mp.ID, mp.YoungestBorrower == null ? "n/a" : mp.YoungestBorrower.FullName, mp.YoungestBorrower.DateOfBirth == null ? "n/a" : mp.YoungestBorrower.DateOfBirth.ToString()));
            }
            dvProducts = _dvProducts;
            InitProducts();
            advCalculatorBuffer = new MortgageBuffer();
            RestoreSavedValues();
        }
        public AdvCalculator(MortgageProfile _mp, DataView _dvProducts,bool needSave):this(_mp,_dvProducts,needSave,null)
        {
        }
        public AdvCalculator(MortgageProfile _mp, bool needInitialDraw_, bool needCreditLine_, bool needMonthlyIncome_, bool needTerm_, string feeSelection_)
        {
            mp = _mp;
            needSaveInMortgage = false;
            if (mp != null)
            {
                ID = mp.ID;
                paymentAmount = mp.PaymentAmount ?? 0;
                term = mp.MortgageInfo.Term ?? 0; 
            }
            advCalculatorBuffer = new MortgageBuffer();
            needInitialDraw = needInitialDraw_;
            needCreditLine = needCreditLine_;
            needMonthlyIncome = needMonthlyIncome_;
            needTerm = needTerm_;
            feeSelection = feeSelection_;
            if (!String.IsNullOrEmpty(feeSelection))
            {
                SetServiceFee();
            }
        }
        #endregion
        #region methods
        
        #region public
        public bool CalculateSingleProduct(int productId, double serviceFee)
        {
            Product product = new Product(productId);
            currentProduct = product;
            string errMessage;
            SetBuffer(product);
            advCalculatorBuffer.MonthlyServiceFee = serviceFee;
            CalculateProduct(product, out errMessage);
            calcError = errMessage;
            return String.IsNullOrEmpty(errMessage);
        }
        public bool CalculatedForSelectedProduct()
        {
            Product product = mp.Product;
            currentProduct = product;
            string errMessage;
            if (product != null)
            {
                SetBuffer(product);
                advCalculatorBuffer.MonthlyServiceFee = GetServiceFee();
                CalculateProduct(product, out errMessage);
            }
            else
            {
                errMessage = "Product not selected";
            }
            calcError = errMessage;
            return String.IsNullOrEmpty(errMessage);
        }
        public string ValidateFirst()
        {
            string resMessage = String.Empty;

            if (mp.YoungestBorrower == null || mp.YoungestBorrower.LifeExpectancy == null)
            {
                string msg = "You need to have at least one borrower birthday to obtain LifeExpectancy";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }
            if (mp.Property.SPValue == null || (decimal)mp.Property.SPValue <= 0)
            {
                string msg = "You need to enter HomeValue (MarketValue)";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            return resMessage;
        }
        public int[] GetProductOrder(int count)
        {
            int cnt = Math.Min(dvProducts.Count, count);
            int[] res = new int[cnt];
            for (int i = 0; i < cnt; i++) res[i] = 0;
            if ((productsOrder == null) ||(productsOrder.Count!=cnt))
            {
                productsOrder = new ArrayList();
                for (int i = 0; i < cnt; i++)
                {
                    productsOrder.Add(int.Parse(dvProducts[i]["id"].ToString()));
                }
            }
            //if(mp.ProductID>0)
            //{
            //    string filter = dvProducts.RowFilter;
            //    dvProducts.RowFilter = String.Format("id={0}", mp.ProductID);
            //    bool hasSelected = dvProducts.Count > 0;
            //    dvProducts.RowFilter = filter;
            //    if(!hasSelected)
            //    {
            //        mp.MortgageInfo.ProductId = -1;
            //    }
            //}
            selectedProductId = mp.ProductID;
            if (selectedProductId > 0)
            {
                res[0] = selectedProductId;
                int i = 1;
                int k = 0;
                while (k<cnt)
                {
                    if ((int)productsOrder[k] != selectedProductId)
                    {
                        res[i] = (int)productsOrder[k];
                        i++;
                        if (i == cnt) break;
                    }
                    k++;
                }
            }
            else
            {
                for (int i = 0; i < cnt; i++)
                {
                    res[i] = (int)productsOrder[i];
                }
            }
            return res;
        }
        public void UpdateFeeSelection(string selection)
        {
            feeSelection = selection;
            SetServiceFee();
        }
        public bool Save(int[] productOrder)
        {
            string order = GetOrderField(productOrder);
            return db.ExecuteScalarInt(SAVE, ID, needInitialDraw, needCreditLine, needMonthlyIncome, needTerm, order, feeSelection) == 1;
        }
        public Hashtable GetValues(int productId, string[] properties, decimal serviceFee, out string errMessage, out string parameters)
        {
            Hashtable tbl;
            Product prod = (Product)products[productId];
            currentProduct = prod;
            SetBuffer(prod);
            advCalculatorBuffer.MonthlyServiceFee = (double)serviceFee;
            CalculateProduct(prod, out errMessage);
            parameters = CalculatorParameters;
            tbl = new Hashtable();
            for (int i = 0; i < properties.Length; i++)
            {
                Object o = GetPropertyValue(properties[i]);
                tbl.Add(properties[i], o);
            }
            return tbl;
        }
        public string GetName(int productId)
        {
            string res = "";
            if (products.Contains(productId))
            {
                Product p = (Product)products[productId];
                res  = p.Name;
            }
            return res;
        }
        public string GetParameters(int productId, out string productName)
        {
            productName = ""; 
            string res = "";
            if (products.Contains(productId))
            { 
                Product p = (Product)products[productId];
                productName = p.Name;
                string errMessage;
                SetBuffer(p);
                if (CalculateProduct(p, out errMessage))
                {
                    res = CalculatorParameters;
                }
            }
            return res;
        }
        public bool UpdateHomeValue(decimal homeValue)
        {
            Property property = mp.Property;
            string errMessage;
            bool res;
            res = mp.UpdateObject("Property.SPValue", homeValue.ToString(), property.ID, out errMessage);
            return res;
        }
        public bool UpdateCreditLine(decimal mpCreditLine)
        {
            MortgageInfo mortgageInfo = mp.MortgageInfo;
            string errMessage;
            creditLine = mpCreditLine;
            return mp.UpdateObject("MortgageInfo.CreditLine", mpCreditLine.ToString(), mortgageInfo.ID, out errMessage);
        }
        public bool UpdateTerm(decimal mpTerm)
        {
            MortgageInfo mortgageInfo = mp.MortgageInfo;
            string errMessage;
            term = mpTerm;
            return mp.UpdateObject("MortgageInfo.Term", mpTerm.ToString(), mortgageInfo.ID, out errMessage);
        }
        public bool UpdateMortgageData(decimal? mpPaymentAmount, decimal? mpTerm, decimal? mpInitialDraw, decimal? mpCreditLine)
        {
            MortgageInfo mortgageInfo = mp.MortgageInfo;
            string errMessage;
            bool resAction = true;
            if (mpPaymentAmount != null)
            {
                paymentAmount = (decimal)mpPaymentAmount;
                resAction &= mp.UpdateObject("MortgageInfo.PaymentAmount", ((decimal)mpPaymentAmount).ToString(), mortgageInfo.ID, out errMessage);
            }

            if (mpTerm != null)
            {
                term = (decimal)mpTerm;
                resAction &= mp.UpdateObject("MortgageInfo.Term", ((decimal)mpTerm).ToString(), mortgageInfo.ID, out errMessage);
            }
            if (mpInitialDraw != null)
            {
                initialDraw = (decimal)mpInitialDraw;
                resAction &= mp.UpdateObject("MortgageInfo.InitialDraw", ((decimal)mpInitialDraw).ToString(), mortgageInfo.ID, out errMessage);
            }
            if (mpCreditLine != null)
            {
                creditLine = (decimal)mpCreditLine;
                resAction &= mp.UpdateObject("MortgageInfo.CreditLine", ((decimal)mpCreditLine).ToString(), mortgageInfo.ID, out errMessage);
            }                
            return resAction;
        }
        public Object GetScenarioValue(string propertyName, string propertyValue)
        {
            PropertyInfo pi = GetType().GetProperty(propertyName);
            Type t = pi.PropertyType;
            if (IsNullableType(t))
            {
                t = Nullable.GetUnderlyingType(t);
            }
            return Convert.ChangeType(propertyValue, t);
        }
        public bool HasSetter(string propertyName)
        {
            return GetType().GetProperty(propertyName).CanWrite;
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

            if (needMonthlyIncome && needTerm && MPTerm > 456)
            {
                string msg = "Term cannot exceed 456 - the number of months between age 62 and 100";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }
            if (needMonthlyIncome && needTerm && MPTerm <= 0)
            {
                string msg = "If term is selected, 'Months' must be greater than 0";
                resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            }

            //if (needMonthlyIncome && needTerm && MPPaymentAmount <= 0 && MPTerm <= 0)
            //{
            //    string msg = "At least one of the two variables (PaymentAmount or Term) must be entered";
            //    resMessage += String.IsNullOrEmpty(resMessage) ? msg : Environment.NewLine + msg;
            //}
            return resMessage;
        }
        public void LoadServiceeFee()
        {
            DataView dv = ServiceFee.GetAllDefaultServiceFee();
            dv.RowFilter = String.Format("companyid={0}", mp.Lender.ID);
            if (dv.Count > 0)
            {
                AddServiceFeeValues(dv);
            }
            dv.RowFilter = String.Format("companyid={0}", 1);
            if (dv.Count > 0)
            {
                AddServiceFeeValues(dv);
            }
        }
        #endregion

        #region private
        private void WriteToLog(string message)
        {
            if(log!=null)
            {
                log.WriteLine(message);
            }
        }
        private void AddServiceFeeValues(DataView dv)
        {
            for(int i=0;i<dv.Count;i++)
            {
                int productId = int.Parse(dv[i]["productId"].ToString());
                decimal fee = Convert.ToDecimal(dv[i]["fee"].ToString());
                if (!ServiceeFeeSelection.Contains(productId))
                {
                    ServiceeFeeSelection.Add(productId,fee);
                }
            }
        }
        private double GetServiceFee()
        {
            decimal res = 0;
            DataView dv = Product.GetLenderServiceFee(mp.Product.ID,mp.Lender.ID);
            if (ServiceeFeeSelection.Contains(mp.Product.ID))
            {
                decimal fee = (decimal)ServiceeFeeSelection[mp.Product.ID];
                dv.RowFilter = String.Format("fee={0}", fee);
                if (dv.Count == 0)
                {
                    dv.RowFilter = "Isdefault=1";
                }
                if (dv.Count > 0)
                {
                    res = Convert.ToDecimal(dv[0]["fee"].ToString());
                }
            }
            else
            {
                dv.RowFilter = "Isdefault=1";
                if (dv.Count > 0)
                {
                    res = Convert.ToDecimal(dv[0]["fee"].ToString());
                }
            }
            return (double)res;
        }
        private void SetSelectedProduct()
        {
            if (mp != null)
            {
                MortgageInfo mortgageInfo = mp.MortgageInfo;
                int lenderAffiliateID = mp.MortgageInfo.LenderAffiliateID;
                DataView dvLenderProduct = Product.GetAllProductsWithLendersForOriginator(mp.CompanyID);
                dvLenderProduct.RowFilter = String.Format("productid={0} AND LenderId={1}", selectedProductId, lenderAffiliateID);
                string errMessage;
                if (dvLenderProduct.Count > 0)
                    mp.UpdateObject("MortgageInfo.ProductId", selectedProductId.ToString(), mortgageInfo.ID, out errMessage);
                else
                {
                    dvLenderProduct.RowFilter = String.Format("productid={0}", selectedProductId);
                    dvLenderProduct.Sort = "LenderId";
                    if (dvLenderProduct.Count > 0)
                    {
                        lenderAffiliateID = Convert.ToInt32(dvLenderProduct[0]["LenderId"]);
                        mp.UpdateObject("MortgageInfo.LenderAffiliateID", lenderAffiliateID.ToString(), mortgageInfo.ID, out errMessage);
                        mp.UpdateObject("MortgageInfo.ProductId", selectedProductId.ToString(), mortgageInfo.ID, out errMessage);
                    }
                }
            }
        }
        private void SetBuffer(Product prod)
        {
            advCalculatorBuffer = new MortgageBuffer();
            advCalculatorBuffer.NeedCreditLine = needCreditLine;
            advCalculatorBuffer.NeedInitialDraw = needInitialDraw;
            advCalculatorBuffer.NeedMonthlyIncome = needMonthlyIncome;
            advCalculatorBuffer.NeedTerm = needTerm;
            LoadMortgageData();
            advCalculatorBuffer.HecmFnmaFlag = 1; // always HECM
            CalcLineOfCredit = needCreditLine;
            LendingLimit = mp.Property.LendingLimit;
            advCalculatorBuffer.LoadFromProduct(prod, mp.Lender.CompanyId, mp.MortgageInfo.ClosingDate ?? DateTime.Now);
            advCalculatorBuffer.PreviousMiCredit = (double) (mp.MortgageInfo.HECMRefiOrigMIPAmt ?? 0);
            UpdateBufferWithScenarioValues();
        }
        private void UpdateBufferWithScenarioValues()
        {
            if (scenarioValues != null)
            {
                foreach (Object key in scenarioValues.Keys)
                {
                    string propertyName = key.ToString();
                    Object propertyValue = scenarioValues[propertyName];
                    if (propertyName == "Age")
                    {
                        int fullYears = GetYears(advCalculatorBuffer.Borrower1Birthdate.AddMonths(-7));
                        double d = Convert.ToDouble(propertyValue.ToString());
                        int diffYears = fullYears - Convert.ToInt32(d);
                        advCalculatorBuffer.Borrower1Birthdate = advCalculatorBuffer.Borrower1Birthdate.AddYears(diffYears);
                    }
                    else if (propertyName == "FinancedOriginationFee")
                    {
                        PropertyInfo pi = GetType().GetProperty(propertyName);
                        pi.SetValue(this, propertyValue, null);
                        LenderFees = mp.LenderFees - mp.OriginationFees + decimal.Parse(propertyValue.ToString());
                    }
                    else
                    {
                        PropertyInfo pi = GetType().GetProperty(propertyName);
                        pi.SetValue(this, propertyValue, null);
                    }
                }
            }
        }
        private static int GetYears(DateTime dt)
        { 
            DateTime d1 = new DateTime(DateTime.Now.Year,dt.Month,dt.Day);            
            int res = DateTime.Now.Year - dt.Year;
            if (d1 > DateTime.Now)
            {
                res--;
            }
            return res;            
        }
        private void LoadMortgageData()
        { 
            if (mp != null)
            {
                RestoreSavedValues();
                initialDraw = mp.InitialDraw == null ? 0 : (decimal)mp.InitialDraw;
                creditLine = mp.CreditLine == null ? 0 : (decimal)mp.CreditLine;
                paymentAmount = mp.PaymentAmount ?? 0; 
                term =  mp.MortgageInfo.Term ?? 0; 

                InitialDraw = MPInitialDraw >= 0 ? MPInitialDraw : 0;
                CreditLine = MPCreditLine >= 0 ? MPCreditLine : 0;

                PaymentAmount = needMonthlyIncome && needTerm ? MPPaymentAmount : 0;
                Term = needMonthlyIncome && needTerm ? MPTerm : 0;

                reserveTotalAmount = mp.ReserveTotalAmount;
                loanOriginatorFees = mp.OriginationFees;
                financedOriginationFee = loanOriginatorFees;
                OtherCashOut = mp.OtherCashOut;
                Repairs = mp.Repairs;
                MortgageBalance = mp.MortgageBalance;
                LenderFees = mp.LenderFees;
                OtherClosingCosts = mp.OtherClosingCosts;
                totalClosingCostsInvoice = mp.TotalClosingCosts;
//                    - mp.UpFrontPremiumInvoiceValue;
                SharedAppreciation = mp.MortgageInfo.SharedAppreciation ?? 0;
                HomeValue = mp.Property.SPValue == null ? 0 : (decimal)mp.Property.SPValue;
                advCalculatorBuffer.LifeExpectancy = (int)mp.YoungestBorrower.LifeExpectancy;
                advCalculatorBuffer.Repairs = (double)(mp.Property.RepairSetAside ?? 0) + (double)(mp.Property.RepairSetAsideTaxesAndInsurance ?? 0);

                Borrower youngestBorrower = mp.YoungestBorrower;
                advCalculatorBuffer.Borrower1Birthdate = youngestBorrower != null ?
                    ((DateTime)youngestBorrower.DateOfBirth).Date : DateTime.MinValue.Date;

                advCalculatorBuffer.ClosingDate = DateTime.Now.Date;
                if (mp.MortgageInfo.ClosingDate != null)
                    advCalculatorBuffer.ClosingDate = ((DateTime)mp.MortgageInfo.ClosingDate).Date;
                selectedProductId = mp.ProductID > 0 ? mp.ProductID : -1;

            }
        }
        private static string GetOrderField(int[] order)
        {
            string res = "";
            if (order.Length > 0)
            {
                for (int i = 0; i < order.Length; i++)
                {
                    int id = order[i];
                    res += id.ToString();
                    if (i < (order.Length - 1)) res += ";";

                }
            }
            return res;
        }
        private string CalculatorParameters
        {
            get
            {
                StringBuilder paramBuilder = new StringBuilder();
                paramBuilder.AppendLine(String.Format("Mortgage Youngest Borrower : {0}", mp.YoungestBorrower.FullName));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Product specific parameters");
                paramBuilder.AppendLine(String.Format("HecmFnmaFlag : {0}", advCalculatorBuffer.HecmFnmaFlag));
                paramBuilder.AppendLine(String.Format("ZMathIndex : {0}", advCalculatorBuffer.Index));
                paramBuilder.AppendLine(String.Format("Margin : {0}", Margin));
                paramBuilder.AppendLine(String.Format("MonthlyServiceFee : {0}", MonthlyServiceFee));
                paramBuilder.AppendLine(String.Format("EomFlag : {0}", advCalculatorBuffer.EomFlag));
                paramBuilder.AppendLine(String.Format("Precision : {0}", advCalculatorBuffer.Precision));
                paramBuilder.AppendLine(String.Format("RateRoundingMethod : {0}", advCalculatorBuffer.RateRoundingMethod));
                paramBuilder.AppendLine(String.Format("Basis : {0}", advCalculatorBuffer.Basis));
                paramBuilder.AppendLine(String.Format("PaymentsPerYear : {0}", advCalculatorBuffer.PaymentsPerYear));
                paramBuilder.AppendLine(String.Format("PropertyAppreciation : {0}", PropertyAppreciation));
                paramBuilder.AppendLine(String.Format("UpfrontMiRate : {0}", UpfrontMiRate));
                paramBuilder.AppendLine(String.Format("RenewalMiRate : {0}", RenewalMiRate));
                paramBuilder.AppendLine(String.Format("EffectiveRateCap : {0}", EffectiveRateCap));
                paramBuilder.AppendLine(String.Format("FirstYrPropChgs : {0}", advCalculatorBuffer.FirstYrPropChgs));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Mortgage specific parameters");
                paramBuilder.AppendLine(String.Format("InitialDraw : {0}", InitialDraw));
                paramBuilder.AppendLine(String.Format("CreditLine : {0}", CreditLine));
                paramBuilder.AppendLine(String.Format("PaymentAmount : {0}", PaymentAmount));
                paramBuilder.AppendLine(String.Format("Term : {0}", Term));
                paramBuilder.AppendLine(String.Format("CalcLineOfCredit : {0}", advCalculatorBuffer.CalcLineOfCredit));
                paramBuilder.AppendLine(String.Format("OtherCashOut : {0}", OtherCashOut));
                paramBuilder.AppendLine(String.Format("Repairs : {0}", Repairs));
                paramBuilder.AppendLine(String.Format("MortgageBalance : {0}", MortgageBalance));
                paramBuilder.AppendLine(String.Format("LenderFees : {0}", LenderFees));
                paramBuilder.AppendLine(String.Format("OtherClosingCosts : {0}", OtherClosingCosts));
                paramBuilder.AppendLine(String.Format("LendingLimit : {0}", LendingLimit));
                paramBuilder.AppendLine(String.Format("SharedAppreciation : {0}", SharedAppreciation));
                paramBuilder.AppendLine(String.Format("HomeValue : {0}", HomeValue));
                paramBuilder.AppendLine(String.Format("LifeExpectancy : {0}", advCalculatorBuffer.LifeExpectancy));
                paramBuilder.AppendLine(String.Format("Borrower1Birthdate : {0}", advCalculatorBuffer.Borrower1Birthdate));
                paramBuilder.AppendLine(String.Format("ClosingDate : {0}", advCalculatorBuffer.ClosingDate));
                paramBuilder.AppendLine(String.Format("ReserveTotalAmount : {0}", ReserveTotalAmount));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Not changed parameters");
                paramBuilder.AppendLine(String.Format("Borrower2Birthdate : {0}", advCalculatorBuffer.Borrower2Birthdate));
                paramBuilder.AppendLine(String.Format("PortionMiFinanced : {0}", advCalculatorBuffer.PortionMiFinanced));
                paramBuilder.AppendLine(String.Format("NetProceeds : {0}", advCalculatorBuffer.NetProceeds));
                paramBuilder.AppendLine(String.Empty);

                paramBuilder.AppendLine("Output");
                paramBuilder.AppendLine(String.Format("TotalClosingCosts : {0}", TotalClosingCosts));
                paramBuilder.AppendLine(String.Format("TotalOtherClosingCosts : {0}", TotalOtherClosingCosts));
                paramBuilder.AppendLine(String.Format("UpfrontPremium : {0}", UpFrontPremium));

                return paramBuilder.ToString();
            }
        }
        private void InitProducts() 
        {
            products = new Hashtable();
            for (int i = 0; i < dvProducts.Count; i++)
            {
                int id = Convert.ToInt32(dvProducts[i]["id"].ToString());
                products.Add(id, new Product(id));
            }
        }
        private object GetPropertyValue(string propertyName)
        {
            PropertyInfo pi = GetType().GetProperty(propertyName);
            Object res = pi.GetValue(this, null);
            return res;
        }
        private bool FixedRateLockUsed(Product prod)
        {
            if (log != null) WriteToLog("Checking if fixed rate lock is used for the product...");
            if (log != null) WriteToLog(String.Format("FixedRateLocksYN = {0}", prod.FixedRateLocksYN));
            return prod.FixedRateLocksYN;
        }

        private bool PrincipalLimitProtectionUsed(Product prod)
        {
            if (log != null) WriteToLog("Checking if principal limit protection is used for the product...");
            bool res = false;
            if (log != null) WriteToLog(String.Format("PrincipleLimitProtectionYN = {0}", prod.PrincipleLimitProtectionYN));
            if(prod.PrincipleLimitProtectionYN)
            {
                if (log != null) WriteToLog(String.Format("MortgageInfo.ApplicationDate = {0}", mp.MortgageInfo.ApplicationDate));
                if (mp.MortgageInfo.ApplicationDate==null)
                {
                    if (log != null) WriteToLog("returning due to MortgageInfo.ApplicationDate is null");
                    return res;
                }
                if (log != null) WriteToLog(String.Format("MortgageInfo.CaseNumAssignDate = {0}, DaysToLock= {1}", mp.MortgageInfo.CaseNumAssignDate, prod.DaysToLock));
                DateTime dt1 = Holidays.GetWorkDate((mp.MortgageInfo.CaseNumAssignDate ?? DateTime.Now).AddDays(prod.DaysToLock),mp.CompanyID);
                DateTime dt2 = mp.MortgageInfo.ClosingDate ?? DateTime.Now;
                if (log != null) WriteToLog(String.Format("Checking if {0} >= {1}", dt1,dt2));
                if (log != null) WriteToLog(String.Format("Real date used {0} (weekend and holidays taken into account)", dt1));
                if(dt1>=dt2)
                {

                    dtRates1 = (DateTime) mp.MortgageInfo.ApplicationDate;
                    dtRates2 = (useAppDateRates&&prod.ID==mp.MortgageInfo.ProductId)? (DateTime) mp.MortgageInfo.ApplicationDate: mp.MortgageInfo.ClosingDate ?? DateTime.Now;
                    res = true;
                    if (useAppDateRates && prod.ID == mp.MortgageInfo.ProductId)
                    {
                        if (log != null) WriteToLog("Application date is used for product");
                    }
                    if (log != null) WriteToLog(String.Format("Principal limit protection is used for product. MortgageInfo.ApplicationDate = {0}, MortgageInfo.ClosingDate = {1}, First date for rates = {0}, Second date for rates = {2}", mp.MortgageInfo.ApplicationDate, mp.MortgageInfo.ClosingDate, dtRates2));
                }
            }
            if(!res)
            {
                if (log != null) WriteToLog("Principal limit protection not used for product");
            }
            return res;
        }
        private bool CalculateProduct(Product prod, out string errMessage)
        {
            if (log != null) WriteToLog(System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine);
            if (log != null) WriteToLog(String.Format("======== Calculation for product {0} (Id={1}) started ================= ", prod.Name, prod.ID));
            if (log != null) WriteToLog(String.Format("PublishedExpectedRate = {0} ProductExpectedFloorRate = {1}, Index(Zmath) = {2}({3})", PublishedExpectedRate, prod.ExpectedFloorRate, (PublishedExpectedRate < prod.ExpectedFloorRate ? prod.ExpectedFloorRate - Margin : advCalculatorBuffer.PublishedExpectedIndex), (PublishedExpectedRate < prod.ExpectedFloorRate ? "prod.ExpectedFloorRate - Margin" : "PublishedExpectedIndex")));

            errMessage = Validate();
            if (!String.IsNullOrEmpty(errMessage))
            {
                if (log != null) WriteToLog(String.Format("Validation failed. Error - {0}", errMessage));
            }
            DateTime initialRateDate = (DateTime) (mp.MortgageInfo.ClosingDate ?? DateTime.Now);
            if (String.IsNullOrEmpty(errMessage))
            {
                if (IsTwoRunNeeded())
                {
                    TestRun(out errMessage, prod.Name, prod.ID == selectedProductId);
                }
                if(PrincipalLimitProtectionUsed(prod))
                {
                    double limit1 = RunForDate(dtRates1,prod);
                    double limit2 = RunForDate(dtRates2, prod);
                    if (log != null) WriteToLog(String.Format("Limit for first run = {0}, for second run = {1}", limit1, limit2));
                    DateTime realDate;
                    if (log != null) WriteToLog("Checking if limit1 > limit2");
                    if (limit1 > limit2)
                    {
                        advCalculatorBuffer.LoadRatesForSpecificDate(prod, dtRates1, out realDate);
                        if (log != null) WriteToLog(String.Format("Limit1 is higher, rates for date {0} will be used (real date = {1})", dtRates1, realDate));
                    }
                    else
                    {
                        advCalculatorBuffer.LoadRatesForSpecificDate(prod, dtRates2, out realDate);
                        if (log != null) WriteToLog(String.Format("Limit1 is not higher, rates for date {0} will be used (real date = {1})", dtRates2, realDate));
                    }
                }
                else if (FixedRateLockUsed(prod))
                {
                    if (mp.MortgageInfo.RateLockStartDate!=null)
                    {
                        DateTime dt1 = Holidays.GetWorkDate(((DateTime)mp.MortgageInfo.RateLockStartDate).AddDays(prod.FixedRateLockDays),mp.CompanyID);
                        if (log != null) WriteToLog(String.Format("MortgageInfo.RateLockStartDate = {0}, FixedRateLockDays = {1}, LockEndDate(with holidays and weekends) = {2}"  , mp.MortgageInfo.RateLockStartDate, prod.FixedRateLockDays, dt1));
                        if (dt1 < Utils.RemoveTime(DateTime.Now))
                        {
                            if (log != null) WriteToLog("Current date will be used");
                        }
                        else
                        {
                            if (log != null) WriteToLog(String.Format("LockEndDate = {0} is > then current date = {1}", dt1, DateTime.Now));
                            if (log != null) WriteToLog(String.Format("Date = {0} will be used", (DateTime)mp.MortgageInfo.RateLockStartDate));
                            DateTime realDate;
                            advCalculatorBuffer.LoadRatesForSpecificDate(prod, (DateTime)mp.MortgageInfo.RateLockStartDate, out realDate);
                            initialRateDate = (DateTime)mp.MortgageInfo.RateLockStartDate;
                            if (log != null) WriteToLog(String.Format("Using rates for date {0} (real date = {1})", (DateTime)mp.MortgageInfo.RateLockStartDate, realDate));
                        }
                    }
                    else
                    {
                        if (log != null) WriteToLog("MortgageInfo.RateLockStartDate is NULL, current date will be used");
                    }
                }
                if (!String.IsNullOrEmpty(errMessage))
                {
                    return false;
                }
                DateTime rd;
                advCalculatorBuffer.SetInitialRate(prod,initialRateDate,out rd);
                if (log != null) WriteToLog(String.Format("Date used for InitialPublishedRate = {0}, (real date = {1}", initialRateDate,rd));
                RevMort revmort = new RevMort();
                try
                {
                    if (log != null) WriteToLog(String.Format("Zmath values - Index={0}, Margin={1}, InitialRate={2}", advCalculatorBuffer.Index, advCalculatorBuffer.Margin,advCalculatorBuffer.InitialRate));
                    if (log != null) DumpBuffer(advCalculatorBuffer);
                    revmort.ReverseMortgageCalc(advCalculatorBuffer);
                    CheckNetAvailable(prod, out errMessage);
                }
                catch (Exception ex)
                {
                    errMessage = String.Format("{0} product calculation error occured \nError : {1}",prod.Name, ex.Message);
                    calcError += errMessage + Environment.NewLine; ;
                }
            }
            if (mp.Product.ID  == prod.ID)
            {
                if(needSaveInMortgage)
                {
                    mp.MortgageInfo.SetCalculator(this);
                }
                mp.CalculatorUpdateNeeded = false;
            }
            return String.IsNullOrEmpty(errMessage);
        }
        private void CheckNetAvailable(Product prod, out string errMessage)
        {
            errMessage = "";
            isNetAvavilableNegative = false;
            if(prod.ID == mp.MortgageInfo.ProductId)
            {
                if (NetAvailable < 0)
                {
                    if (log != null) WriteToLog(String.Format("NetAvailabe = {0}", NetAvailable));
                    isNetAvavilableNegative = true;
                }
                mp.MortgageInfo.MoneyShortFlagUpdate = isNetAvavilableNegative;
            }
            if(isNetAvavilableNegative)
            {
                advCalculatorBuffer.InitialDraw = 0;
                mp.InitialDraw = 0;
                needInitialDraw = false;
                advCalculatorBuffer.CreditLine = 0;
                mp.CreditLine = 0;
                needCreditLine = false;
                needMonthlyIncome = false;
                advCalculatorBuffer.NeedMonthlyIncome = false;
                RevMort revmort = new RevMort();
                try
                {
                    revmort.ReverseMortgageCalc(advCalculatorBuffer);
                }
                catch (Exception ex)
                {
                    errMessage = String.Format("{0} product calculation error occured \nError : {1}", prod.Name, ex.Message);
                    calcError += errMessage + Environment.NewLine; ;
                }
            }
        }

        private void DumpBuffer(MortgageBuffer buffer)
        {
            WriteToLog("-------------- ZMath buffer: --------------------");
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] properties = buffer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            for(int i=0;i<properties.Length; i++)
            {
                sb.AppendLine(String.Format("Property ({0}) {1} = {2}", properties[i].PropertyType.Name, properties[i].Name,
                                        properties[i].GetValue(buffer, null)));
                //WriteToLog(
                //    String.Format("Property ({0}) {1} = {2}", properties[i].PropertyType.Name, properties[i].Name,
                //                  properties[i].GetValue(buffer, null)));
            }
            WriteToLog(sb.ToString());
        }

        private double RunForDate(DateTime rateDate, Product product)
        {
            double res = 0;
            DateTime realDate;
            advCalculatorBuffer.LoadRatesForSpecificDate(product, rateDate, out realDate);
            if (log != null) WriteToLog(String.Format("Run calculator for rates date={0}(real date={1})", rateDate, realDate));
            RevMort revmort = new RevMort();
            try
            {
                if (log != null) WriteToLog(String.Format("Zmath values - Index={0}, Margin={1}", advCalculatorBuffer.Index, advCalculatorBuffer.Margin));
                if (log != null) DumpBuffer(advCalculatorBuffer);
                revmort.ReverseMortgageCalc(advCalculatorBuffer);
                res = advCalculatorBuffer.PrincipalLimit;
            }
            catch (Exception ex)
            {
                string errMessage = String.Format("{0} product calculation error occured \nError : {1}", product.Name, ex.Message);
                calcError += errMessage + Environment.NewLine; ;
            }
            return res;
        }
        private bool IsTwoRunNeeded()
        { 
            bool res = false;
            if (log != null) WriteToLog("Checking if 2 runs needed...");
            if (log != null) WriteToLog(String.Format("Needterm={0} Payment amount={1} term={2}",needTerm,paymentAmount,term));
            if (needTerm)
            {
                if ((paymentAmount > 0) && (term ==0))
                {
                    res = true;
                }
            }
            if (log != null) WriteToLog(String.Format("Two runs {0}",res?"needed":"not needed"));
            return res;
        }
        private void TestRun(out string errMessage, string productName, bool showerror)
        {
            if (log != null) WriteToLog("---- Test run ----");
            errMessage = String.Empty;
            int calLineOfCredit_ = advCalculatorBuffer.CalcLineOfCredit;
            advCalculatorBuffer.CalcLineOfCredit = 0;
            double paymentAmount_ = advCalculatorBuffer.PaymentAmount;
            advCalculatorBuffer.PaymentAmount = 0;
            double term_ = advCalculatorBuffer.Term;
            advCalculatorBuffer.Term = 0;
            if (log != null) WriteToLog("CalcLineOfCredit,PaymentAmount,Term are set to 0");
            RevMort revmort = new RevMort();
            try
            {
                if (log != null) WriteToLog(String.Format("Zmath values - Index={0}, Margin={1}", advCalculatorBuffer.Index, advCalculatorBuffer.Margin));
                if (log != null) DumpBuffer(advCalculatorBuffer);
                revmort.ReverseMortgageCalc(advCalculatorBuffer);
            }
            catch (Exception ex)
            {
                errMessage = String.Format("{0} product calculation error occured \nError : {1}", productName, ex.Message);
            }
            if (paymentAmount_ > advCalculatorBuffer.PeriodicPayment)
            {
                if (log != null) WriteToLog(String.Format("{0}: Monthly payment is too high. It cannot be higher than tenure payment.", productName));
                errMessage = String.Format("{0}: Monthly payment is too high. It cannot be higher than tenure payment.",productName);
            }
            else 
            {
                if (log != null) WriteToLog("Test run pass successfully");
                advCalculatorBuffer.CalcLineOfCredit = calLineOfCredit_;
                advCalculatorBuffer.PaymentAmount = paymentAmount_;
                advCalculatorBuffer.Term = term_;
            }
            if(!String.IsNullOrEmpty(errMessage) && showerror)
            {
                calcError += errMessage + Environment.NewLine; ;
            }            
        }
        private void RestoreSavedValues()
        {
            DataView dv = db.GetDataView("GetAdvancedCalculatorByID", mp.ID);
            if (dv.Count != 1)
                return;
            ID = ConvertToInt(dv[0]["MPID"], -1);
            needInitialDraw = ConvertToBool(dv[0]["NeedInitialDraw"], true);
            needCreditLine = ConvertToBool(dv[0]["NeedCreditLine"], true);
            needMonthlyIncome = ConvertToBool(dv[0]["NeedMonthlyIncome"], false);
            needTerm = ConvertToBool(dv[0]["NeedTerm"], false);
            SetProductsOrder(dv[0]["productOrder"].ToString());
            if (dv[0]["ServiceFeeSelection"] != DBNull.Value)
            {
                feeSelection = dv[0]["ServiceFeeSelection"].ToString(); 
                SetServiceFee();
            }
            paymentAmount = mp.PaymentAmount ?? 0;
            term = mp.MortgageInfo.Term ?? 0; 
        }
        private void SetServiceFee()
        {
            serviceFeeSelection = new Hashtable();
            string[] tmp = feeSelection.Split(';');
            for (int i = 0; i < tmp.Length; i++)
            {
                int productId;
                decimal fee;
                if (ParseFeeForProduct(tmp[i],out productId, out fee))
                {
                    if (!serviceFeeSelection.Contains(productId))
                    {
                        serviceFeeSelection.Add(productId, fee);
                    }
                }
            }
        }
        private static bool ParseFeeForProduct(string data, out int productId, out decimal fee)
        {
            bool res = false;
            productId = -1;
            fee = 0;
            string[] tmp = data.Split('=');
            if (tmp.Length == 2)
            {
                try
                {
                    productId = int.Parse(tmp[0]);
                    fee = decimal.Parse(tmp[1]);
                    res = true;
                }
                catch 
                {
                }
            }
            return res;
        }
        private void SetProductsOrder(string order)
        {
            productsOrder = new ArrayList();
            if (!String.IsNullOrEmpty(order))
            {
                string[] s = order.Split(';');
                for (int k = 0; k < s.Length; k++)
                {
                    try
                    {
                        int id = Math.Abs(int.Parse(s[k]));
                        if(products.Contains(id))
                        {
                            if (!productsOrder.Contains(id))
                            {
                                productsOrder.Add(id);
                            }
                        }
                    }
                    catch { }
                }
            }
            if (selectedProductId <= 0)
            {
                selectedProductId = mp.ProductID;
            }
        }
        #endregion

        #region static
        public static DataView GetFields()
        {
            return db.GetDataView(GETFIELDS);
        }
        #endregion
        
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Xml;


namespace LoanStar.Common
{
    public class Invoice : BaseObject, IInvoiceData
    {
        /// <summary>
        /// Any method of Invoice class must throw custom exceptions of this type only
        /// </summary>
        public class InvoiceException : BaseObjectException
        {
            public InvoiceException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public InvoiceException(string message)
                : base(message)
            {
            }

            public InvoiceException()
            {
            }
        }
        
        #region constants
        private const int RELEASETYPE = 5;
        public const int STATUSPAYATSETTLEMENT = 7;
        public const int VENDORPROVIDERTYPEID = 1;
        public const int ORIGINATORPROVIDERTYPEID = 2;
        public const int LENDERPROVIDERTYPEID = 3;
        public const int OTHERPROVIDERTYPEID = 4;
        public const int COUNTYRECORDERTYPEID = 5;
        public const int PREFFEREDVENDORTYPEID = 6;
        private const int POCSTATUSID = 2;
        private const string DELETE = "DeleteInvoice";
        
        public const int RECORDINGFEETYPEID = 4;
        public const int CITYCOUNTYSTAMPSFEETYPEID = 6;
        public const int STATETAXSTAMPSFEETYPEID = 7;
        public const int TITLEINSURANCEFEETYPEID = 31;
        public const int TITLEENDORSMENTSFEETYPEID = 37;
        private const string ROOTELEMENT = "root";
        public const int MAXCLAIMID = 1;
        public const int MAXPRINCIPLELIMITID = 2;
        public const int PRINCIPLELIMITID = 3;
        public const int ROUNDUPID = 1;
        public const int ROUNDNEARESTID = 2;


        public const int ADDITIONALCHARGECATEGORYID = 1;
        public const int GOVERNMENTCHARGECATEGORYID = 2;
        public const int LENDERCHARGECATEGORYID = 3;
        public const int TITLECHARGECATEGORYID = 4;
        public const int ITEMSPAIDINADVACNCECHARGECATEGORYID = 5;

        #region formula constants
        public const int RECORDINGFEETOTALCOSTFORMULAID = 1;
        public const int RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID = 2;
        public const int RECORDINGFEESAMEPRICEPAGEFORMULAID = 3;
        public const int STAMPSTOTALCOSTFORMULAID = 1;
        public const int STAMPSPRICEPERUNITFORMULAID = 2;
        public const int STAMPSPERCENTAGEFORMULAID = 3;
        public const int TITLEINSURANCETOTALCOSTFORMULAID = 1;
        public const int TITLEINSURANCETWOTIERFORMULAID = 2;
        public const int TITLEINSURANCEFOURTIERFORMULAID = 3;


        public const string FORMULAID = "FormulaId";
        public const string DEEDFIRST = "DeedFirst";
        public const string DEEDNEXT = "DeedNext";
        public const string DEEDPAGES = "DeedPages";
        public const string MORTGAGEFIRST = "MortgageFirst";
        public const string MORTGAGENEXT = "MortgageNext";
        public const string MORTGAGEPAGES = "MortgagePages";
        public const string RELEASEFIRST = "ReleaseFirst";
        public const string RELEASENEXT = "ReleaseNext";
        public const string RELEASEPAGES = "ReleasePages";
        #endregion

        #endregion

        #region fields
        public MortgageProfile mp;
        private int mortgageid;
        private int typeid;
        private string typename;
        private int feecategoryid;
        private string feecategoryname;
        private string description;
        private int providerId;
        private int statusid = 2;
        private string statusname;
        private decimal amount = 0;
        private DateTime? duedate;
        private int chargetoid=1;
        private string chargetoname;
        private int createdby;
        private DateTime created;
        private bool calculateFee = false;
        private int providerTypeId = 1;
        private string otherProviderName;
        private string otherTypeDescription;
        private decimal pocAmount = 0;
        private decimal deedAmount = 0;
        private decimal mortgageAmount = 0;
        private decimal releaseAmount = 0;
        private string listendorsements;
        private string vendorName;
        private Hashtable formulaData;
        private decimal lenderCreditAmount = 0;
        private decimal thirdPartyPaidAmount = 0;
        private bool showAmounts = false;
        private string vendorAffiliation;
        #endregion

        #region  properties
        public string VendorAffiliation
        {
            get
            {
                return vendorAffiliation;
            }
        }
        public bool ShowAmounts
        {
            get { return showAmounts; }
            set { showAmounts = value; }
        }
        public decimal LenderCreditAmount
        {
            get { return lenderCreditAmount; }
            set { lenderCreditAmount = value; }
        }
        public decimal ThirdPartyPaidAmount
        {
            get { return thirdPartyPaidAmount; }
            set { thirdPartyPaidAmount = value; }
        }

        public int FormulaId
        {
            get 
            {
                int res = -1;
                if(FormulaData.ContainsKey(FORMULAID))
                {
                    try
                    {
                        res = int.Parse(FormulaData[FORMULAID].ToString());
                    }
                    catch
                    {
                    }
                }
                return res; 
            }
            set
            {
                if (FormulaData.ContainsKey(FORMULAID))
                {
                    FormulaData[FORMULAID] = value;
                }
                else
                {
                    FormulaData.Add(FORMULAID,value);
                }
            }
        }
        public Hashtable FormulaData
        {
            get
            {
                if(formulaData==null)
                {
                    formulaData = new Hashtable();
                }
                return formulaData;
            }
        }
        public bool HasFormula
        {
            get
            {
                return (typeid == RECORDINGFEETYPEID) || (typeid == CITYCOUNTYSTAMPSFEETYPEID) ||
                       (typeid == STATETAXSTAMPSFEETYPEID) || (typeid == TITLEINSURANCEFEETYPEID);
            }
        }
        public bool HasOrders
        {
            get { return false; }
        }
        public string Provider
        {
            get
            {
                string res = string.Empty;
                switch (providerTypeId)
                {
                    case LENDERPROVIDERTYPEID :
                        if (mp.MortgageInfo.LenderAffiliateID>0)
                        {
                            res = mp.MortgageInfo.LenderAffiliate.Name;
                        }
                        break;
                    case ORIGINATORPROVIDERTYPEID:
                        res = mp.CurrentUser.CompanyName;
                        break;
                    case OTHERPROVIDERTYPEID:
                        res = otherProviderName;
                        break;
                    case COUNTYRECORDERTYPEID:
                        if(HasFormula)
                        {
                            int officerTitleId = 0;
                            if (FormulaData.ContainsKey("OfficerTitleId"))
                            {
                                try
                                {
                                    officerTitleId = int.Parse(FormulaData["OfficerTitleId"].ToString());
                                }
                                catch
                                {
                                }
                            }
                            if(officerTitleId>0)
                            {
                                if (formulaData.ContainsKey("OfficerTitle"))
                                {
                                    res = FormulaData["OfficerTitle"].ToString();
                                }
                            }
                            if(String.IsNullOrEmpty(res))
                            {
                                res = otherProviderName;
                            }
                        }
                        else
                        {
                            if (mp.Property.CountyID > 0)
                            {
                                res = String.Format("{0} county recorder", mp.Property.County);
                            }
                            else
                            {
                                res = "County recorder";
                            }
                        }
                        break;
                    case VENDORPROVIDERTYPEID:
                        if(providerId >0)
                        {
                            res = vendorName;
                        }
                        break;
                }
                return res;
            }
        }
        public string Listendorsements
        {
            get { return listendorsements; }
            set { listendorsements = value; }
        }
        public bool IsPOC
        {
            get { return (statusid == POCSTATUSID); }
        }
        public int MortgageID
        {
            set { mortgageid = value; }
            get { return mortgageid; }
        }
        public int TypeID
        {
            set { typeid = value; }
            get { return typeid; }
        }
        public string TypeName
        {
            set { typename = value; }
            get
            {
                return typename;
            }
        }
        public int FeeCategoryID
        {
            set { feecategoryid = value; }
            get { return feecategoryid; }
        }
        public string FeeCategoryName
        {
            set { feecategoryname = value; }
            get { return feecategoryname; }
        }
        public string Description
        {
            set { description = value; }
            get { return description; }
        }
        public int ProviderId
        {
            set { providerId = value; }
            get { return providerId; }
        }
        public int StatusID
        {
            set { statusid = value; }
            get { return statusid; }
        }
        public string StatusName
        {
            set { statusname = value; }
            get { return statusname; }
        }
        public decimal InvoiceDeedAmount
        {
            get
            {
                decimal res = 0;
                if ((mp.Property.IsDeedState != null) && ((bool)mp.Property.IsDeedState))
                {
                    res = amount;
                }
                return res;
            }
        }
        public decimal InvoiceMortgageAmount
        {
            get
            {
                decimal res = 0;
                if ((mp.Property.IsDeedState != null) && (!(bool)mp.Property.IsDeedState))
                {
                    res = amount;
                }
                return res;
            }
        }
        public decimal InvoiceReleaseAmount
        {
            get
            {
                decimal res = 0;
                if (typeid==RELEASETYPE)
                {
                    res = amount;
                }
                return res;
            }
        }
        public decimal TotalFees
        {
            get { return InvoiceDeedAmount + InvoiceMortgageAmount + InvoiceReleaseAmount; }
        }
        
        public DateTime? DueDate
        {
            set { duedate = value; }
            get { return duedate; }
        }
        public int ChargeToId
        {
            set { chargetoid = value; }
            get { return chargetoid; }
        }
        public string ChargeToName
        {
            set { chargetoname = value; }
            get { return chargetoname; }
        }
        public int CreatedBy
        {
            set { createdby = value; }
            get { return createdby; }
        }
        public DateTime Created
        {
            set { created = value; }
            get { return created; }
        }
        public bool CalculateFee
        {
            get { return calculateFee; }
            set { calculateFee = value; }
        }
        public int ProviderTypeId
        {
            get { return providerTypeId; }
            set { providerTypeId = value; }
        }
        public string OtherProviderName
        {
            get { return otherProviderName; }
            set { otherProviderName = value; }
        }
        public string OtherTypeDescription
        {
            get { return otherTypeDescription; }
            set { otherTypeDescription = value; }
        }
        public decimal POCAmount
        {
            get { return pocAmount; }
            set { pocAmount = value; }
        }       
        public decimal DeedAmount
        {
            get { return deedAmount; }
            set { deedAmount = value; }
        }
        public decimal MortgageAmount
        {
            get { return mortgageAmount; }
            set { mortgageAmount = value; }
        }
        public decimal ReleaseAmount
        {
            get { return releaseAmount; }
            set { releaseAmount = value; }
        }
        public decimal Amount
        {
            get
            {
                decimal res = amount;
                if (HasFormula)
                {
                    switch (typeid)
                    {
                        case RECORDINGFEETYPEID:
                            if (FormulaId == RECORDINGFEETOTALCOSTFORMULAID)
                            {
                                res = mortgageAmount + deedAmount + releaseAmount;
                            }
                            else if (FormulaId == RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID)
                            {
                                res = GetAmountForFirstAdditional("Mortgage") + GetAmountForFirstAdditional("Deed") + GetAmountForFirstAdditional("Release");
                            }
                            else if (FormulaId == RECORDINGFEESAMEPRICEPAGEFORMULAID)
                            {
                                res = GetAmountForSamePrice("Mortgage") + GetAmountForSamePrice("Deed") + GetAmountForSamePrice("Release");
                            }
                            break;
                        case STATETAXSTAMPSFEETYPEID:
                        case CITYCOUNTYSTAMPSFEETYPEID:
                            if (FormulaId == STAMPSTOTALCOSTFORMULAID)
                            {
                                res = mortgageAmount + deedAmount;
                            }
                            else if (FormulaId == STAMPSPRICEPERUNITFORMULAID)
                            {
                                res = CalculateTotal();
                            }
                            else if(FormulaId == STAMPSPERCENTAGEFORMULAID)
                            {
                                res = CalculateTotal();
                            }
                            break;
                        case TITLEINSURANCEFEETYPEID:
                            if(FormulaId == TITLEINSURANCETOTALCOSTFORMULAID)
                            {
                                res = GetFormulaDataDecimal("Totalprice");
                            }
                            else
                            {
                                res = CalculateTotal();
                            }
                            break;
                    }
                }
                else
                {
                    if (feecategoryid == GOVERNMENTCHARGECATEGORYID)
                    {
                        res = deedAmount + mortgageAmount;
                    }
                    
                }
                return res;
            }
            set { amount = value; }
        }
        public decimal AmountFinanced
        {
            get 
            {
                return Amount - (pocAmount + lenderCreditAmount + thirdPartyPaidAmount);
            }
        }
        public decimal MaxClaimAmount
        {
            get
            {
                decimal res = 0;
                if(mp!=null)
                {
                    res = mp.Property.MaxClaimAmount;
                }
                return res;
            }
        }
        public decimal PrincipalLimit
        {
            get
            {
                decimal res = 0;
                if(mp!=null&&mp.ProductID>0)
                {
                    res = mp.Calculator.PrincipalLimit;
                }
                return res;
            }
        }
        #endregion

        #region constructors
        public Invoice(): this(-1)
	    {
	    }
        public Invoice(int id)
        {
            ID = id;
            if (id <= 0)
                return;
            DataView dv = db.GetDataView("GetInvoiceById", id);
            if (dv.Count == 1)
            {
                LoadByDataRow(dv[0]);
            }
        }
        public Invoice(DataRowView dr,MortgageProfile mp_) : this(dr)
        {
            mp = mp_;
        }
        public Invoice(DataRowView dr)
        {
            LoadByDataRow(dr);
        }
        #endregion

        #region methods
        public void SetPrincipalLimit()
        {
            if(mp.IsCalculatorReady)
            {
                SetFormulaData("PrincipalLimit",mp.Calculator.PrincipalLimit.ToString());
            }
        }

        private decimal GetAmountForSamePrice(string name)
        {
            decimal u = GetFormulaDataDecimal(name + "Pages");
            decimal p = GetFormulaDataDecimal(name + "PricePerPageUnit");
            return u*p;
        }

        private decimal GetAmountForFirstAdditional(string name)
        {
            decimal p1 = GetFormulaDataDecimal(name + "FirstPage");
            decimal p2 = GetFormulaDataDecimal(name + "AdditionalPage");
            int n = GetFormulaDataInt(name + "TotalPages");
            return p1+p2*(n-1);
        }

        public decimal CalculateTotal()
        {
            decimal res = 0;
            switch(typeid)
            {
                case CITYCOUNTYSTAMPSFEETYPEID:
                case STATETAXSTAMPSFEETYPEID:
                    if (FormulaId == STAMPSPRICEPERUNITFORMULAID)
                    {
                        decimal pu = GetFormulaDataDecimal("PricePerUnit");
                        decimal u = GetFormulaDataDecimal("Units");
                        decimal r = pu * u;
                        if (r > 0)
                        {
                            res = RoundTotal(GetMaxAmount(), pu, u);
                        }
                    }
                    else if (FormulaId == STAMPSPERCENTAGEFORMULAID)
                    {
                        res = GetMaxAmount() * (GetFormulaDataDecimal("Percentage")/(decimal) 100.0);
                    }
                    break;
                case TITLEINSURANCEFEETYPEID:
                    if(FormulaId == TITLEINSURANCETWOTIERFORMULAID)
                    {
                        decimal total = GetMaxAmount();
                        decimal t1 = GetFormulaDataDecimal("Upto");
                        if (total < t1)
                        {
                            t1 = total;
                        }
                        total = total - t1;
                        res = GetTierUnitFee(t1, GetFormulaDataDecimal("Per"), GetFormulaDataDecimal("UnitFee"));
                        if (total > 0)
                        {
                            res += GetTierUnitFee(total, GetFormulaDataDecimal("Peradditional"), GetFormulaDataDecimal("Thenunitfee"));
                        }
                    }
                    else if(FormulaId == TITLEINSURANCEFOURTIERFORMULAID)
                    {
                        decimal total = GetMaxAmount();
                        res = CalculateTiers(total, 4);
                    }
                    break;
            }
            return res;
        }
        private decimal CalculateTiers(decimal total, int tiersCount)
        {
            decimal res = 0;
            decimal ut1 = 0;
            for (int i = 0; i < tiersCount; i++ )
            {
                decimal ut = total;
                if(i<(tiersCount-1))
                {
                    ut = GetFormulaDataDecimal(String.Format("Upto{0}", i));
                }
                decimal uf = GetFormulaDataDecimal(String.Format("UnitFee{0}", i));
                decimal per = GetFormulaDataDecimal(String.Format("Per{0}", i));
                if(total<=ut)
                {
                    ut = total;
                }
                res += GetTierUnitFee(ut-ut1, per, uf);
                ut1 = ut;
            }
            return res;
        }

        private static decimal GetTierUnitFee(decimal t,decimal per, decimal unitfee)
        {
            decimal res = 0;
            if((t>0)&&(per!=0)&&(unitfee!=0))
            {
                res = (t/per)*unitfee;
            }
            return res;
        }

        private decimal RoundTotal(decimal d, decimal pu, decimal u)
        {
            decimal res = 0;
            int i = GetFormulaDataInt("RoundSelect");
            if( i== ROUNDUPID)
            {
                decimal k = Math.Round(d/u);
                if(k*u<d)
                {
                    k++;
                }
                res = k*pu;
            }
            else if(i==ROUNDNEARESTID)
            {
                res = Math.Round(d/u)*pu;
            }
            return res;
        }
        private decimal GetFormulaDataDecimal(string name)
        {
            decimal res = 0;
            string s = GetFormulaData(name);
            if (!String.IsNullOrEmpty(s))
            {
                try
                {
                    res = decimal.Parse(s);
                }
                catch
                {
                }
            }
            return res;
        }
        private decimal GetMaxAmount()
        {
            decimal res;
            int i = GetFormulaDataInt("MaxSelect");
            res = mp.Property.MaxClaimAmount;
            if((i==PRINCIPLELIMITID)&&(mp.ProductID<1))
            {
                i = MAXCLAIMID;
            }
            if (i == MAXPRINCIPLELIMITID)
            {
                res *= (decimal) 1.5;
            }
            else if (i==PRINCIPLELIMITID)
            {
                if(mp.IsCalculatorReady)
                {
                    res = mp.Calculator.PrincipalLimit;
                }
                else
                {
                    res = GetFormulaDataDecimal("PrincipalLimit");
                }
            }
            return res;
        }
        public int GetFormulaDataInt(string name)
        {
            int res = 0;
            string s = GetFormulaData(name);
            if(!String.IsNullOrEmpty(s))
            {
                try
                {
                    res = int.Parse(s);
                }
                catch
                {
                }
            }
            return res;
        }
        public string GetFormulaData()
        {
            string res = String.Empty;
            if(HasFormula)
            {
                if(FormulaData.Count>0)
                {
                    XmlDocument d = new XmlDocument();
                    XmlNode root = d.CreateElement(ROOTELEMENT);
                    foreach (Object key in FormulaData.Keys)
                    {
                        XmlNode n = d.CreateElement(ITEMELEMENT);
                        XmlAttribute a = d.CreateAttribute(key.ToString());
                        a.Value = FormulaData[key].ToString();
                        n.Attributes.Append(a);
                        root.AppendChild(n);
                    }
                    d.AppendChild(root);
                    res = d.OuterXml;
                }
            }
            return res;
        }
        private void LoadByDataRow(DataRowView dr)
        {
            ID = ConvertToInt(dr["ID"], -1);
            mortgageid = Convert.ToInt32(dr["MortgageID"]);
            typeid = Convert.ToInt32(dr["TypeID"]);
            typename = dr["TypeName"].ToString();
            feecategoryid = Convert.ToInt32(dr["FeeCategoryId"]);
            feecategoryname = dr["CategoryName"].ToString();
            description = dr["Description"].ToString();
            providerId = Convert.ToInt32(dr["Provider"]);
            statusid = Convert.ToInt32(dr["StatusID"]);
            statusname = dr["StatusName"].ToString();
            amount = Convert.ToDecimal(dr["Amount"]);
            if (dr["DueDate"] != DBNull.Value)
            {
                if (!String.IsNullOrEmpty(dr["DueDate"].ToString()))
                {
                    duedate = Convert.ToDateTime(dr["DueDate"]);
                }                
            }                
            chargetoid = Convert.ToInt32(dr["chargetoid"]);
            chargetoname = dr["ChargeName"].ToString();
            if (!String.IsNullOrEmpty(dr["CreatedBy"].ToString()))
            {
                createdby = Convert.ToInt32(dr["CreatedBy"]);
            }
            created = Convert.ToDateTime(dr["Created"]);
            calculateFee = bool.Parse(dr["calculateFee"].ToString());
            otherProviderName = dr["otherProviderName"].ToString();
            if (dr["providerTypeId"] != DBNull.Value) providerTypeId = int.Parse(dr["providerTypeId"].ToString());
            pocAmount = Convert.ToDecimal(dr["POCAmount"]);
            deedAmount = Convert.ToDecimal(dr["DeedAmount"]);
            mortgageAmount = Convert.ToDecimal(dr["mortgageAmount"]);
            releaseAmount = Convert.ToDecimal(dr["releaseAmount"]);
            listendorsements = dr["Listendorsements"].ToString();
            vendorName = dr["VendorName"].ToString();
            if (dr["InvoiceData"] != DBNull.Value)
            {
                PrepareFormulaData(dr["InvoiceData"].ToString());
            } 
            if (dr["lenderCreditAmount"] != DBNull.Value)
            {
                lenderCreditAmount = Convert.ToDecimal(dr["lenderCreditAmount"]);
            } 
            if (dr["thirdPartyPaidAmount"] != DBNull.Value)
            {
                thirdPartyPaidAmount = Convert.ToDecimal(dr["thirdPartyPaidAmount"]);
            }
            vendorAffiliation = dr["vendorAffiliation"].ToString();
        }
        private void PrepareFormulaData(string data)
        {
            if(!String.IsNullOrEmpty(data))
            {
                formulaData = new Hashtable();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                for(int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++ )
                {
                    XmlNode n = doc.DocumentElement.ChildNodes[i];
                    string name = n.Attributes[0].Name;
                    string val = n.Attributes[0].Value;
                    if(!formulaData.ContainsKey(name))
                    {
                        formulaData.Add(name,val);
                    }
                }
            }
        }

        public int Save()
        {
            int res = db.ExecuteScalarInt("SaveInvoice",
                                        ID
                                        ,MortgageID
                                        ,TypeID
                                        ,Description
                                        ,ProviderId
                                        ,StatusID
                                        ,Amount
                                        ,pocAmount
                                        ,deedAmount
                                        ,mortgageAmount
                                        ,releaseAmount
                                        ,lenderCreditAmount
                                        ,thirdPartyPaidAmount
                                        ,DueDate ?? (object)DBNull.Value
                                        ,ChargeToId
                                        ,CreatedBy
                                        ,calculateFee
                                        ,listendorsements
                                        ,GetFormulaData()
                                        );

            if (res <= 0)
                throw new InvoiceException("Invoice was not updated succesfully");

            ID = (ID <= 0)?res:ID;
            return res;
        }
        public string GetFormulaData(string name)
        {
            string res = String.Empty;
            if (FormulaData.ContainsKey(name))
            {
                res = FormulaData[name].ToString();
            }
            return res;
        }
        public bool NeedOrder(DataView dv)
        {
            bool res = false;
            if(providerTypeId == VENDORPROVIDERTYPEID)
            {
                dv.RowFilter = String.Format("FeeTypeId={0}", TypeID);
                res = dv.Count == 1;
                dv.RowFilter = "";
            }
            return res;
        }

        public void SetFormulaData(string name, string val)
        {
            if (FormulaData.ContainsKey(name))
            {
                FormulaData[name] = val;
            }
            else
            {
                FormulaData.Add(name, val);
            }
        }
        #endregion

        #region Static methods
        public static bool IsOtherFeeType(int feetypeId)
        {
            return (feetypeId == 3) || (feetypeId == 8) || (feetypeId == 21) || (feetypeId == 27) || (feetypeId == 35);
        }
        public static DataView GetInvoiceList(int MortgageID)
        {
            return db.GetDataView("GetInvoiceList", MortgageID);
        }
        public static List<Invoice> GetInvoiceObjectList(MortgageProfile mp)
        {
            DataView invoiceDV = db.GetDataView("GetInvoicesByMortgageID", mp.ID);

            List<Invoice> invoiceList = new List<Invoice>();
            foreach (DataRowView invoiceRow in invoiceDV)
            {
                Invoice invoiceObj = new Invoice(invoiceRow,mp);
                if(invoiceObj.FeeCategoryID != ITEMSPAIDINADVACNCECHARGECATEGORYID)
                {
                    invoiceList.Add(invoiceObj);
                }
            }
            return invoiceList;
        }
        public static DataView GetInvoiceFeeCategoryList()
        {
            return db.GetDataView("GetInvoiceFeeCategoryList");
        }
        public static DataView GetFormulaList(int feeTypeId)
        {
            return db.GetDataView("GetFeeFormulaList", feeTypeId);
        }
        public static DataView GetTitleOfficerList()
        {
            return db.GetDataView("GetTitleOfficerList");
        }
        public static DataView GetInvoiceTypeList(int categoryid, int mortgageId, int id)
        {
            return db.GetDataView("GetInvoiceTypeList", categoryid,mortgageId, id);
        }
        public static DataView GetInvoiceStatusList()
        {
            return db.GetDataView("GetInvoiceStatusList");
        }
        public static DataView GetInvoiceChargeToList()
        {
            return db.GetDataView("GetInvoiceChargeToList");
        }
        public static Decimal GetInvoiceTotalLenderFees(int MortgageID)
        {
            string res = db.ExecuteScalarString("GetInvoiceTotalLenderFees", MortgageID);
            if (!String.IsNullOrEmpty(res)) return Convert.ToDecimal(res);
            else return 0;
        }
        public static Decimal GetInvoiceTotalOriginationFees(int MortgageID)
        {
            string res = db.ExecuteScalarString("GetInvoiceOriginatorFee", MortgageID, 1);
            if (!String.IsNullOrEmpty(res)) return Convert.ToDecimal(res);
            else return 0;
        }
        public static DataView GetProviderType()
        {
            return db.GetDataView("GetProviderType");
        }

        public static Decimal GetInvoiceOtherClosingCosts(int MortgageID)
        {
            string res = db.ExecuteScalarString("GetInvoiceOtherClosingCosts", MortgageID);
            if (!String.IsNullOrEmpty(res)) return Convert.ToDecimal(res);
            else return 0;
        }
        public static bool GetUpdateStatus(int MortgageId)
        {
            return db.ExecuteScalarBool("GetInvoiceUpdateNeeded", MortgageId);
        }
        public static bool Delete(int id)
        {
            return db.ExecuteScalarInt(DELETE, id)==1;
        }
        public static bool IsTypeOther(int typeId)
        {        
            return (typeId == 3) || (typeId == 8) || (typeId == 21) || (typeId == 27);
        }
        #endregion
    }

    public interface IInvoiceData
    {
        int ID
        { get; }
        decimal POCAmount
        {
            get; set;
        }
        decimal MortgageAmount
        {
            get;
            set;
        }
        decimal LenderCreditAmount
        {
            get;
            set;
        }
        decimal ThirdPartyPaidAmount
        {
            get;
            set;
        }
        decimal DeedAmount
        {
            get;
            set;
        }
        decimal ReleaseAmount
        {
            get;
            set;
        }
        decimal Amount
        {
            get;
            set;
        }

        decimal AmountFinanced
        {
            get;
        }

        int FormulaId
        {
            get;
            set;
        }
        decimal MaxClaimAmount
        {
            get;
        }
        decimal PrincipalLimit
        { 
            get;
        }
        string GetFormulaData(string name);
        void SetFormulaData(string name, string value);
        decimal CalculateTotal();
    }
}
using System;
using System.Data;
using System.Xml;
using System.Collections;

namespace LoanStar.Common
{
    public class ClosingCostProfile : BaseObject
    {
        public class ClosingCostProfileException : BaseObjectException
        {
            public ClosingCostProfileException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public ClosingCostProfileException(string message)
                : base(message)
            {
            }

            public ClosingCostProfileException()
            {
            }
        }

        #region constants
        private const string LOAD = "LoadClosingCostProfileById";
        private const string GETLIST = "GetClosingCostProfileListByCompany";
        private const string GETPROFIELDETAILS = "GetClosingCostProfileDetailsById";
        private const string GETALLOWEDTYPE = "GetClosingCostProfileAllowedType";
        private const string SAVE = "SaveClosingCostProfile";
        private const string DELETE = "DeleteClosingCostProfile";
        private const string NAMEFIELDNAME = "name";
        private const string COMPANYIDFIELDNAME = "companyid";
        #endregion

        #region fields
        private string name = String.Empty;
        private int companyId = 0;
        private DataView profileDetails;
        #endregion

        #region properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
        public DataView ProfileDetails
        {
            get
            {
                if(profileDetails==null)
                {
                    profileDetails = GetProfileDetails();
                }
                return profileDetails;
            }
        }
        #endregion

        #region constructor
        public ClosingCostProfile(int id)
        {
            ID = id;
            LoadById();
        }

        #endregion

        #region methods
        public bool Delete()
        {
            return db.ExecuteScalarInt(DELETE, ID) > 0;
        }

        public void ResetDetails()
        {
            profileDetails = null;
        }
        private void LoadById()
        {
            if(ID>0)
            {
                DataView dv = db.GetDataView(LOAD, ID);
                if(dv.Count==1)
                {
                    name = dv[0][NAMEFIELDNAME].ToString();
                    companyId = int.Parse(dv[0][COMPANYIDFIELDNAME].ToString());
                }
            }
        }
        public static DataView GetClosingCostProfileList(int companyId)
        {
            return db.GetDataView(GETLIST, companyId);
        }
        public static DataView GetClosingCostProfileDetailes(int closingCostProfileId)
        {
            return db.GetDataView(GETPROFIELDETAILS, closingCostProfileId);
        }
        private DataView GetProfileDetails()
        {
            return db.GetDataView(GETPROFIELDETAILS, ID);
        }
        public DataView GetAllowedType(int detailsId)
        {
            return db.GetDataView(GETALLOWEDTYPE, ID, detailsId);
        }
        public int Save(int _companyId)
        {
            int res = db.ExecuteScalarInt(SAVE, ID, name, _companyId);
            if(res>0)
            {
                ID = res;
            }
            return res;
        }
        #endregion

        public class ClosingCostProfileDetail : BaseObject, IInvoiceData
        {

            #region constants
            private const string LOADBYID = "GetClosingCostDetailsById";
            private const string SAVE = "SaveClosingCostDetail";
            private const string SAVEFORFORMULA = "SaveClosingCostDetailForFormula";
            private const string DELETE = "DeleteClosingCostDetail";
            private const string GETFEETYPES = "GetFeeTypesForProfile";
            private const string GETVENDORSFORPROFILE = "GetVendorsForProfile";
            private const string GETFORMULALIST = "GetFeeFormulaListForProfile";
            private const string GETSTATELIST = "GetStateListForClosingCostFormula";
            private const string LOADFORMULADATA = "LoadFormulaDataForClosingCost";
            public const string FORMULAID = "FormulaId";
            private const string ROOTELEMENT = "root";
            #endregion

            #region fields
            private int closingCostProfileId;
            private int companyId;
            private int typeId;
            private string typeName;
            private int feeCategoryId;
            private string feeCategoryName;
            private string description;
            private int providerId;
            private int statusId = 2;
            private string statusName;
            private decimal amount = 0;
            private int chargeToId =  1;
            private string chargeToName;
            private bool calculateFee = false;
            private int providerTypeId = 1;
            private string otherTypeDescription;
            private decimal pocAmount = 0;
            private decimal deedAmount = 0;
            private decimal mortgageAmount = 0;
            private decimal releaseAmount = 0;
            private string listendorsements;
            private string otherProviderName;
            private string vendorName;
            private Hashtable formulasData;
            private int formulaId = -1;
            private int stateId = -1;
//            private Hashtable currentData;
            private decimal lenderCreditAmount;
            private decimal thirdPartyPaidAmount;
            #endregion

            #region  properties
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
            private Hashtable FormulasData
            {
                get
                {
                    if(formulasData == null)
                    {
                       formulasData = LoadFormulaData();
                    }
                    return formulasData;
                }
            }
            public decimal MaxClaimAmount
            {
                get { return 0; }
            }
            public decimal PrincipalLimit
            {
                get { return 0; }
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
            public string Provider
            {
                get
                {
                    string res = string.Empty;

                    switch (providerTypeId)
                    {
                        case Invoice.LENDERPROVIDERTYPEID:
                            res = "Lender";
                            break;
                        case Invoice.ORIGINATORPROVIDERTYPEID:
                            res = "Originator";
                            break;
                        case Invoice.OTHERPROVIDERTYPEID:
                            res = otherProviderName;
                            break;
                        case Invoice.COUNTYRECORDERTYPEID:
                            if (HasFormula)
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
                                if (officerTitleId > 0)
                                {
                                    if (FormulaData.ContainsKey("OfficerTitle"))
                                    {
                                        res = FormulaData["OfficerTitle"].ToString();
                                    }
                                }
                                if (String.IsNullOrEmpty(res))
                                {
                                    res = otherProviderName;
                                }
                            }
                            else
                            {
                                res = "County recorder";
                            }
                            break;
                        case Invoice.PREFFEREDVENDORTYPEID:
                            res = "Preferred provider";
                            break;
                        case Invoice.VENDORPROVIDERTYPEID:
                            if (providerId > 0)
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
            public int ClosingCostProfileId
            {
                set { closingCostProfileId = value; }
                get { return closingCostProfileId; }
            }
            public int CompanyId
            {
                set { companyId = value; }
                get { return companyId; }
            }
            public int TypeId
            {
                set { typeId = value; }
                get { return typeId; }
            }
            public string TypeName
            {
                set { typeName = value; }
                get { return typeName; }
            }
            public int FeeCategoryId
            {
                set { feeCategoryId = value; }
                get { return feeCategoryId; }
            }
            public string FeeCategoryName
            {
                set { feeCategoryName = value; }
                get { return feeCategoryName; }
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
            public string OtherProviderName
            {
                get { return otherProviderName; }
                set { otherProviderName = value; }
            }
            public int StatusID
            {
                set { statusId = value; }
                get { return statusId; }
            }
            public string StatusName
            {
                set { statusName = value; }
                get { return statusName; }
            }
            
            public int ChargeToId
            {
                set { chargeToId = value; }
                get { return chargeToId; }
            }
            public string ChargeToName
            {
                set { chargeToName = value; }
                get { return chargeToName; }
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
                set { amount = value; }
                get
                {
                    decimal res = amount;
                    if (HasFormula)
                    {
                        switch (typeId)
                        {
                            case Invoice.RECORDINGFEETYPEID:
                                if (FormulaId == Invoice.RECORDINGFEETOTALCOSTFORMULAID)
                                {
                                    res = mortgageAmount + deedAmount + releaseAmount;
                                }
                                else if (FormulaId == Invoice.RECORDINGFEEFIRSTADDITIONALPAGEFORMULAID)
                                {
                                    res = GetAmountForFirstAdditional("Mortgage") + GetAmountForFirstAdditional("Deed") + GetAmountForFirstAdditional("Release");
                                }
                                else if (FormulaId == Invoice.RECORDINGFEESAMEPRICEPAGEFORMULAID)
                                {
                                    res = GetAmountForSamePrice("Mortgage") + GetAmountForSamePrice("Deed") + GetAmountForSamePrice("Release");
                                }
                                break;
                            case Invoice.STATETAXSTAMPSFEETYPEID:
                            case Invoice.CITYCOUNTYSTAMPSFEETYPEID:
                                if (FormulaId == Invoice.STAMPSTOTALCOSTFORMULAID)
                                {
                                    res = mortgageAmount + deedAmount;
                                }
                                break;
                            case Invoice.TITLEINSURANCEFEETYPEID:
                                res = GetFormulaDataDecimal("Totalprice");
                                break;
                        }
                    }
                    else
                    {
                        if (feeCategoryId == Invoice.GOVERNMENTCHARGECATEGORYID)
                        {
                            res = deedAmount + mortgageAmount;
                        }

                    }
                    return res;
                }

            }
            public decimal AmountFinanced
            {
                get
                {
                    decimal res = 0;
                    if (!HasFormula||(typeId==Invoice.TITLEINSURANCEFEETYPEID&&formulaId==Invoice.TITLEINSURANCETOTALCOSTFORMULAID))
                    {
                        res = Amount - (pocAmount + lenderCreditAmount + thirdPartyPaidAmount);
                    }
                    return res;
                }
            }
            public bool HasFormula
            {
                get
                {
                    return (typeId == Invoice.RECORDINGFEETYPEID) || (typeId == Invoice.CITYCOUNTYSTAMPSFEETYPEID) ||
                           (typeId == Invoice.STATETAXSTAMPSFEETYPEID) || (typeId == Invoice.TITLEINSURANCEFEETYPEID);
                }
            }
            public int FormulaId
            {
                get { return formulaId; }
                set
                {
                    formulaId = value;
                    SetFormulaData("FormulaId", formulaId.ToString());
                }
            }
            public int StateId
            {
                get { return stateId; }
                set 
                {
                    int oldVal = stateId;
                    stateId = value;
                    if(oldVal!=stateId)
                    {
                        SetFormulaData("StateId", stateId.ToString());
                        formulaId = GetFormulaDataInt("FormulaId");
                    }
                }
            }
            public Hashtable FormulaData
            {
                get
                {
                    Hashtable res;
                    if(FormulasData.ContainsKey(stateId))
                    {
                        res = FormulasData[stateId] as Hashtable;
                    }
                    else
                    {
                        res = new Hashtable();
                        FormulasData.Add(stateId,res);
                    }
                    return res;
                }
            }
            //public Hashtable FormulaData
            //{
            //    get
            //    {
            //        if(currentData == null)
            //        {
            //            currentData = LoadData(GetKey(stateId, formulaId));
            //        }
            //        return currentData;
            //    }
            //}
            #endregion

            #region constructors
            public ClosingCostProfileDetail()
                : this(-1)
            {
            }
            public ClosingCostProfileDetail(int id)
            {
                ID = id;
                if (id > 0)
                {
                    LoadById();
                }
            }
            public ClosingCostProfileDetail(DataRowView dr)
            {
                LoadFromDataRow(dr);
            }
            #endregion

            #region methods
            private decimal GetAmountForSamePrice(string name)
            {
                decimal u = GetFormulaDataDecimal(name + "Pages");
                decimal p = GetFormulaDataDecimal(name + "PricePerPageUnit");
                return u * p;
            }
            private decimal GetAmountForFirstAdditional(string name)
            {
                decimal p1 = GetFormulaDataDecimal(name + "FirstPage");
                decimal p2 = GetFormulaDataDecimal(name + "AdditionalPage");
                int n = GetFormulaDataInt(name + "TotalPages");
                return p1 + p2 * (n - 1);
            }
            private Hashtable LoadData(Object key)
            {
                Hashtable res;
                if(FormulasData.ContainsKey(key))
                {
                    res = FormulasData[key] as Hashtable;
                }
                else
                {
                    res= new Hashtable();
                }
                return res;
            }

            public decimal RecalculateAmount(decimal maxClaimAmount)
            {
                decimal res = amount;
                switch(typeId)
                {
                    case Invoice.CITYCOUNTYSTAMPSFEETYPEID:
                    case Invoice.STATETAXSTAMPSFEETYPEID:
                        if (FormulaId == Invoice.STAMPSPRICEPERUNITFORMULAID)
                        {
                            res = GetPricePerUnitAmount(maxClaimAmount);
                        }
                        else if (FormulaId == Invoice.STAMPSPERCENTAGEFORMULAID)
                        {
                            res = GetPercentageAmount(maxClaimAmount);
                        }
                        break;
                    case Invoice.TITLEINSURANCEFEETYPEID:
                        if (FormulaId == Invoice.TITLEINSURANCETWOTIERFORMULAID)
                        {
                            res = GetTwoTierAmount(maxClaimAmount);
                        }
                        break;
                }
                return res;
            }
            private Hashtable LoadFormulaData()
            {
                Hashtable res = new Hashtable();
                DataView dv = db.GetDataView(LOADFORMULADATA, ID);
                if(dv!=null&&dv.Count>0)
                {
                    for(int i=0;i<dv.Count;i++)
                    {
                        int stateId_ = int.Parse(dv[i]["stateId"].ToString());
                        int formulaId_ = int.Parse(dv[i]["formulaId"].ToString());
                        if (stateId < 0)
                        {
                            stateId = stateId_;
                            formulaId = formulaId_;
                        }
                        Hashtable data = GetHashTable(dv[i]["InvoiceData"].ToString());
                        if(data!=null&&data.Count>0)
                        {
//                            res.Add(GetKey(stateId_, formulaId_), data);
                            res.Add(stateId_, data);
                        }
                    }
                }
                return res;
            }
            //private static Object GetKey(int id1, int id2)
            //{
            //    return id1 + "_" + id2;
            //}

            private static Hashtable GetHashTable(string data)
            {
                if(String.IsNullOrEmpty(data))
                {
                    return null;
                }
                Hashtable res = new Hashtable();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
                {
                    XmlNode n = doc.DocumentElement.ChildNodes[i];
                    string name = n.Attributes[0].Name;
                    string val = n.Attributes[0].Value;
                    if (!res.ContainsKey(name))
                    {
                        res.Add(name, val);
                    }
                }
                return res;
            }

            private decimal GetPricePerUnitAmount(decimal maxClaimAmount)
            {
                decimal res = 0;
                decimal pu = GetFormulaDataDecimal("PricePerUnit");
                decimal u = GetFormulaDataDecimal("Units");
                decimal r = pu * u;
                if (r > 0)
                {
                    res = RoundTotal(GetMaxAmount(maxClaimAmount), pu, u);
                }
                return res;
            }
            private decimal GetMaxAmount(decimal maxClaimAmount)
            {
                decimal res = maxClaimAmount;
                int i = GetFormulaDataInt("MaxSelect");
                if (i == Invoice.MAXPRINCIPLELIMITID)
                {
                    res *= (decimal)1.5;
                }
                return res;
            }
            private decimal RoundTotal(decimal d, decimal pu, decimal u)
            {
                decimal res = 0;
                int i = GetFormulaDataInt("RoundSelect");
                if (i == Invoice.ROUNDUPID)
                {
                    decimal k = Math.Round(d / u);
                    if (k * u < d)
                    {
                        k++;
                    }
                    res = k * pu;
                }
                else if (i == Invoice.ROUNDNEARESTID)
                {
                    res = Math.Round(d / u) * pu;
                }
                return res;
            }
            private decimal GetPercentageAmount(decimal maxClaimAmount)
            {
                return GetMaxAmount(maxClaimAmount) * (GetFormulaDataDecimal("Percentage") / (decimal)100.0);
            }
            private decimal GetTwoTierAmount(decimal maxClaimAmount)
            {
                decimal res;
                decimal total = GetMaxAmount(maxClaimAmount);
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
            private int GetFormulaDataInt(string name)
            {
                int res = 0;
                string s = GetFormulaData(name);
                if (!String.IsNullOrEmpty(s))
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
            private decimal GetMaxAmount()
            {
                return 0;
            }
            private void UpdateFormulaData(Hashtable tbl)
            {
                if(FormulasData.ContainsKey(stateId))
                {
                    FormulasData[stateId] = tbl;
                }
                else
                {
                    FormulasData.Add(stateId,tbl);
                }
            }
            public void SetFormulaData(string name, string val)
            {
                Hashtable tbl = FormulaData;
                if(tbl.ContainsKey(name))
                {
                    tbl[name] = val;
                }
                else
                {
                    tbl.Add(name, val);
                }
                UpdateFormulaData(tbl);
            }
            public decimal CalculateTotal()
            {
                decimal res = 0;
                switch (typeId)
                {
                    case Invoice.CITYCOUNTYSTAMPSFEETYPEID:
                    case Invoice.STATETAXSTAMPSFEETYPEID:
                        if (FormulaId == Invoice.STAMPSPRICEPERUNITFORMULAID)
                        {
                            decimal pu = GetFormulaDataDecimal("PricePerUnit");
                            decimal u = GetFormulaDataDecimal("Units");
                            decimal r = pu * u;
                            if (r > 0)
                            {
                                res = RoundTotal(GetMaxAmount(), pu, u);
                            }
                        }
                        else if (FormulaId == Invoice.STAMPSPERCENTAGEFORMULAID)
                        {
                            res = GetMaxAmount() * (GetFormulaDataDecimal("Percentage") / (decimal)100.0);
                        }
                        break;
                    case Invoice.TITLEINSURANCEFEETYPEID:
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

                        break;
                }
                return res;
            }
            private static decimal GetTierUnitFee(decimal t, decimal per, decimal unitfee)
            {
                decimal res = 0;
                if ((t > 0) && (per != 0) && (unitfee != 0))
                {
                    res = (t / per) * unitfee;
                }
                return res;
            }

            public string GetFormulaData()
            {
                string res = String.Empty;
                if (HasFormula)
                {
                    if (FormulaData.Count > 0)
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
            public static bool Delete(int id)
            {
                return db.ExecuteScalarInt(DELETE, id) > 0;
            }
            public int Save()
            {
                int res;
                if(!HasFormula)
                {
                    res = db.ExecuteScalarInt(SAVE, ID
                                            , closingCostProfileId
                                            , companyId
                                            , typeId
                                            , providerId
                                            , statusId
                                            , chargeToId
                                            , providerTypeId
                                            , amount
                                            , pocAmount
                                            , deedAmount
                                            , mortgageAmount
                                            , releaseAmount
                                            , lenderCreditAmount
                                            , thirdPartyPaidAmount
                                            , description
                                            , otherTypeDescription
                                            , otherProviderName
                                            , listendorsements
                                            , calculateFee
                                        );
                }
                else
                {
                    res = db.ExecuteScalarInt(SAVEFORFORMULA, ID
                                            , closingCostProfileId
                                            , companyId
                                            , typeId
                                            , providerId
                                            , statusId
                                            , chargeToId
                                            , providerTypeId
                                            , amount
                                            , pocAmount
                                            , deedAmount
                                            , mortgageAmount
                                            , releaseAmount
                                            , lenderCreditAmount
                                            , thirdPartyPaidAmount
                                            , description
                                            , otherTypeDescription
                                            , otherProviderName
                                            , listendorsements
                                            , calculateFee
                                            , stateId
		                                    , formulaId
                                            , GetFormulaData()
                                        );
                }
                if (res > 0) ID=res;
                return res;
            }
            public DataView GetStateList()
            {
                return db.GetDataView(GETSTATELIST, ID, typeId, closingCostProfileId);
            }
            public DataView GetFeeTypes()
            {
                return db.GetDataView(GETFEETYPES, feeCategoryId, closingCostProfileId, ID);
            }
            public DataView GetVendors(int companyId_)
            {
                return db.GetDataView(GETVENDORSFORPROFILE, companyId_, typeId);
            }
            private void LoadFromDataRow(DataRowView dr)
            {
                ID = int.Parse(dr["id"].ToString());
                closingCostProfileId = int.Parse(dr["closingCostProfileId"].ToString());
                companyId = int.Parse(dr["companyid"].ToString());
                typeId = int.Parse(dr["typeid"].ToString());
                typeName = dr["typename"].ToString();
                feeCategoryId = int.Parse(dr["feecategoryid"].ToString());
                feeCategoryName = dr["categoryname"].ToString();
                description = dr["description"].ToString();
                providerId = int.Parse(dr["provider"].ToString());
                statusId = int.Parse(dr["statusid"].ToString());
                statusName = dr["statusname"].ToString();
                amount = decimal.Parse(dr["amount"].ToString());
                chargeToId = int.Parse(dr["chargeToId"].ToString());
                chargeToName = dr["chargename"].ToString();
                if (dr["calculateFee"] != DBNull.Value) calculateFee = bool.Parse(dr["calculateFee"].ToString());
                providerTypeId = int.Parse(dr["providerTypeId"].ToString());
                otherTypeDescription = dr["otherTypeDescription"].ToString();
                pocAmount = decimal.Parse(dr["pocAmount"].ToString());
                deedAmount = decimal.Parse(dr["deedAmount"].ToString());
                mortgageAmount = decimal.Parse(dr["mortgageAmount"].ToString());
                releaseAmount = decimal.Parse(dr["releaseAmount"].ToString());
                if (dr["lenderCreditAmount"] != DBNull.Value)
                {
                    lenderCreditAmount = decimal.Parse(dr["lenderCreditAmount"].ToString());
                }
                if (dr["thirdPartyPaidAmount"] != DBNull.Value)
                {
                    thirdPartyPaidAmount = decimal.Parse(dr["thirdPartyPaidAmount"].ToString());
                }
                
                listendorsements = dr["Listendorsements"].ToString();
                otherProviderName = dr["otherProviderName"].ToString();
                vendorName = dr["VendorName"].ToString();
            }
            private void LoadById()
            {
                DataView dv = db.GetDataView(LOADBYID, ID);
                if (dv.Count == 1)
                {
                    LoadFromDataRow(dv[0]);
                }
            }
            public DataView GetFormulaList()
            {
                return db.GetDataView(GETFORMULALIST, typeId, stateId);
            }
            #endregion

        }
    }

}


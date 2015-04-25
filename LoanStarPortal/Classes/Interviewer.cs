using System;
using System.Data;

namespace LoanStar.Common
{
    public class Interviewer : BaseObject
    {
        public class InterviewerException : BaseObjectException
        {
            public InterviewerException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public InterviewerException(string message)
                : base(message)
            {
            }

            public InterviewerException()
            {
            }
        }

        #region constnats
        private const string GETINTERVIEWERBYID = "GetInterviewerById";
        private const string INTERVIEWRTABLE = "Interviewer";
        #endregion

        #region fields
        private readonly MortgageProfile mp;
        private bool? isLoanOfficer;
        private string firstName = String.Empty;
        private string lastName = String.Empty;
        private string phone = String.Empty;
        private string originatorName = String.Empty;
        private string originatorAddress1 = String.Empty;
        private string originatorAddress2 = String.Empty;
        private string originatorCity = String.Empty;
        private int originatorStateId = 0;
        private string originatorZip = String.Empty;
        private Originator originator;
        #endregion

        #region properties
        public Originator Originator
        {
            get
            {
                if(originator==null)
                {
                    int companyid = 0;
                    if((isLoanOfficer!=null) &&((bool)isLoanOfficer))
                    {
                        companyid = mp.LoanOfficer.CompanyId;
                    }
                    originator = new Originator(companyid);
                }
                return originator;
            }
        }
        public bool? IsLoanOfficer
        {
            get { return isLoanOfficer; }
            set { isLoanOfficer = value; }
        }
        public string Name
        {
            get { return FirstName + " " + LastName; }
        }
        public string FirstName
        {
            get
            {
                string res = String.Empty;
                if(isLoanOfficer!=null)
                {
                    if((bool)isLoanOfficer)
                    {
                        res = mp.LoanOfficer.FirstName;
                    }
                    else
                    {
                        res = firstName;
                    }
                }
                return res;
            }
            set { firstName = value; }
        }
        public string LastName
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = mp.LoanOfficer.LastName;
                    }
                    else
                    {
                        res = lastName;
                    }
                }
                return res;
            }
            set { lastName = value; }
        }
        public string Phone
        {
            get
            {
                string res = String.Empty;
                if(isLoanOfficer!=null)
                {
                    if((bool)isLoanOfficer)
                    {
                        res = mp.LoanOfficer.Phone;
                    }
                    else
                    {
                        res = phone;
                    }
                }
                return res;
            }
            set { phone = value; }
        }
        public string OriginatorName
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.Name;
                    }
                    else
                    {
                        res = originatorName;
                    }
                }
                return res;
            }
            set { originatorName = value; }
        }
        public string OriginatorAddress1
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.Address1;
                    }
                    else
                    {
                        res = originatorAddress1;
                    }
                }
                return res;
            }
            set { originatorAddress1 = value; }
        }
        public string OriginatorAddress2
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.Address2;
                    }
                    else
                    {
                        res = originatorAddress2;
                    }
                }
                return res;
            }
            set { originatorAddress2 = value; }
        }
        public string OriginatorCity
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.City;
                    }
                    else
                    {
                        res = originatorCity;
                    }
                }
                return res;
            }
            set { originatorCity = value; }
        }
        public int OriginatorStateId
        {
            get
            {
                int res = 0;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.StateId;
                    }
                    else
                    {
                        res = originatorStateId;
                    }
                }
                return res;
            }
            set { originatorStateId = value; }
        }
        public string OriginatorZip
        {
            get
            {
                string res = String.Empty;
                if (isLoanOfficer != null)
                {
                    if ((bool)isLoanOfficer)
                    {
                        res = Originator.Zip;
                    }
                    else
                    {
                        res = originatorZip;
                    }
                }
                return res;
            }
            set { originatorZip = value; }
        }
        #endregion

        #region constructor
        public Interviewer():this(0,null)
        {
        }
        public Interviewer(int id,MortgageProfile mp_)
        {
            mp = mp_;
            ID = id;
            if (id > 0)
            {
                DataView dv = db.GetDataView(GETINTERVIEWERBYID, ID);
                if (dv.Count == 1)
                {
                    PopulateFromDataRow(dv[0]);
                }
                else
                {
                    ID = -1;
                }
            }

        }
        #endregion

        #region methods
        public override int Save(MortgageProfile mp_, string objectName, string fullPropertyName, string propertyName, int objectTypeId, object val, object oldVal, string parentFieldName, int parentId, bool isRequired)
        {
            return base.Save(mp_, INTERVIEWRTABLE, fullPropertyName, propertyName, objectTypeId, val, oldVal, parentFieldName, parentId, isRequired);
        }
        #endregion

    }
}

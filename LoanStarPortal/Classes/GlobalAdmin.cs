using System;
using System.Data;

namespace LoanStar.Common
{
    public class GlobalAdmin : BaseObject
    {

        public class GlobalAdminException : BaseObjectException
        {
            public GlobalAdminException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public GlobalAdminException(string message)
                : base(message)
            {
            }

            public GlobalAdminException()
            {
            }
        }

        #region Constants
//        private const string GETGLOBALADMIN = "GetGlobalAdmin";
        private const string GETGLOBALADMIN = "GetGlobalAdminValues";
        private const string SAVE = "SaveGlobalAdmin";
        private const string MAXFEMAFLOODCOVERAGEFIELDNAME = "MaxFEMAFloodCoverage";
        #endregion

        #region fields
        private decimal maxFEMAFloodCoverage;
        private string fhaConnectionUrl = String.Empty;
        private string ldpUrl = String.Empty;
        private string gsaUrl = String.Empty;
        private string uspsUrl = String.Empty;
        private string defaultCreditReportVendor = String.Empty;
        private string defaultFloodCertificationVendor = String.Empty;
        private string defaultAppraisalVendor = String.Empty;
        private string defaultCounselingVendor = String.Empty;
        private string defaultSurveyVendor = String.Empty;
        private string defaultTitleVendor = String.Empty;
        #endregion

        #region properties
        public decimal MaxFEMAFloodCoverage
        {
            get { return maxFEMAFloodCoverage; }
            set { maxFEMAFloodCoverage = value; }
        }
        public string FHAConnectionUrl
        {
            get { return fhaConnectionUrl;}
            set { fhaConnectionUrl = value;}
        }
        public string LDPUrl
        {
            get { return ldpUrl;}
            set { ldpUrl = value;}
            
        }
        public string GSAUrl
        {
            get { return gsaUrl; }
            set { gsaUrl = value; }

        }
        public string USPSUrl
        {
            get { return uspsUrl;}
            set { uspsUrl = value;}
        }
        public string DefaultCreditReportVendor
        {
            get { return defaultCreditReportVendor;}
            set { defaultCreditReportVendor = value;}
        }
        public string DefaultFloodCertificationVendor
        {
            get{ return defaultFloodCertificationVendor;}
            set { defaultFloodCertificationVendor = value;}
        }
        public string DefaultAppraisalVendor
        {
            get{ return defaultAppraisalVendor;}
            set { defaultAppraisalVendor = value;}
        }
        public string DefaultCounselingVendor
        {
            get { return defaultCounselingVendor;}
            set { defaultCounselingVendor = value;}
        }
        public string DefaultSurveyVendor
        {
            get { return defaultSurveyVendor;}
            set { defaultSurveyVendor = value;}
        }
        public string DefaultTitleVendor
        {
            get { return defaultTitleVendor; }
            set { defaultTitleVendor = value; }
        }
        #endregion

        #region Constructors
        //public GlobalAdmin()
        //    : this(0)
        //{ }
        public GlobalAdmin()
        {
            //ID = id;
            //if (id > 0)
            //{
                Load();
            //}
        }
        #endregion

        #region methods
        
        #region public
        public int Save()
        {
            return db.ExecuteScalarInt(SAVE, ID
                , maxFEMAFloodCoverage
                , fhaConnectionUrl
                , ldpUrl
                , gsaUrl
                , uspsUrl
                , defaultCreditReportVendor
                , defaultFloodCertificationVendor
                , defaultAppraisalVendor
                , defaultCounselingVendor
                , defaultSurveyVendor
                , defaultTitleVendor
                );
        }
        #endregion

        #region private
        private void Load()
        {
            DataView dv = db.GetDataView(GETGLOBALADMIN);
            if (dv.Count == 1)
            {
                maxFEMAFloodCoverage = Convert.ToDecimal(dv[0][MAXFEMAFLOODCOVERAGEFIELDNAME]);
                fhaConnectionUrl = dv[0]["fhaConnectionUrl"].ToString();
                ldpUrl = dv[0]["ldpUrl"].ToString();
                gsaUrl = dv[0]["gsaUrl"].ToString();
                uspsUrl = dv[0]["uspsUrl"].ToString();
                defaultCreditReportVendor = dv[0]["defaultCreditReportVendor"].ToString();
                defaultFloodCertificationVendor = dv[0]["defaultFloodCertificationVendor"].ToString();
                defaultAppraisalVendor = dv[0]["defaultAppraisalVendor"].ToString();
                defaultCounselingVendor = dv[0]["defaultCounselingVendor"].ToString();
                defaultSurveyVendor = dv[0]["defaultSurveyVendor"].ToString();
                defaultTitleVendor = dv[0]["defaultTitleVendor"].ToString();
            }
            else
            {
                ID = 0;
            }
        }
        #endregion

        #endregion
    }
}
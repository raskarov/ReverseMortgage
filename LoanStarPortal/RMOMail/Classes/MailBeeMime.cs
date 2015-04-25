using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MailBee.Mime;


namespace LoanStarPortal.RMOMail.Classes
{
    public class MailBeeMime : MailMessage
    {

        private string _LoanApplicantNames;
        private string _LoanApplicationIDs;
        public string LoanApplicationIDs
        {
            get { return _LoanApplicationIDs; }
            set { _LoanApplicationIDs = value; }
        }

        public string LoanApplicantNames
        {
            get { return _LoanApplicantNames; }
            set { _LoanApplicantNames = value; }
        }


        public MailBeeMime() {}


    }
}

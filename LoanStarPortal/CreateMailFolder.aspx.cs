using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class CreateMailFolder : AppPage
    {
        #region Properties
        private string EmailsUniqueID
        {
            get { return GetValue(Constants.EMAILSUNIQUEID, String.Empty); }
        }
        private string EmailsUserID
        {
            get { return GetValue(Constants.EMAILSUSERID, String.Empty); }
        }
        #endregion

        #region Methods
        private void SetMessage(string message)
        {
            lblFolderNameErr.Text = message;
            lblFolderNameErr.Visible = !String.IsNullOrEmpty(message);
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            SetMessage(String.Empty);

            if (!IsPostBack)
            {
                tbFolderName.Text = String.Empty;
                hfEmailsUniqueID.Value = EmailsUniqueID;
                hfEmailsUserID.Value = EmailsUserID;
            }
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            string statusName = tbFolderName.Text;
            int userID = Convert.ToInt32(hfEmailsUserID.Value);
            if (String.IsNullOrEmpty(statusName) || userID <= 0)
            {
                SetMessage("Invalid parameters");
                return;
            }

            try
            {
                Mailer.CreateMailStatus(statusName, userID);
                ClientScript.RegisterStartupScript(this.GetType(), "NewMailFolder", "<script type='text/javascript'>CloseAndRebind()</script>");
            }
            catch (Exception ex)
            {
                SetMessage(ex.Message);
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Cancel", "<script type='text/javascript'>Close()</script>");
        }
        #endregion
    }
}

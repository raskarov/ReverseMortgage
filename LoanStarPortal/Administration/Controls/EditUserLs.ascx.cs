using System;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditUserLs : AppControl
    {
        #region constants
        private const string ADDHEADERTEXT = "Add new administrative user";
        private const string EDITHEADETEXT = "Edit administrative user({0})";
        private const string ADMININPUTCSS = "admininput";
        private const string DISABLEDSUFFIX = "dis";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETPASSWORD = "SetPassword(this.checked,{0},{1})";
        #endregion

        #region fields
/*
        private const bool ALL = true;
*/
        private int userid = -1;
        private AppUser user;
        private bool isNew;
//        private bool isMe;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lblPasswordRules.Text = Utils.PASSWORDRULESTEXT;
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            userid = CurrentPage.GetValueInt(Constants.IDPARAM, userid);
            isNew = userid < 0;
            if (!Page.IsPostBack)
            {
                BindData();
            }
            ClearError();
        }
        #region methods
        private void ClearError()
        {

            lblMessage.Text = "";
            lblLoginErr.Text = "";
            lblPasswordErr.Text = "";
            lblConfirmPasswordErr.Text = "";
        }
        private void BindData()
        {
            user = new AppUser(userid);
            if (isNew)
            {
                lblHeader.Text = ADDHEADERTEXT;
            }
            else
            {
//                isMe = user.Id == CurrentUser.Id;
                lblHeader.Text = String.Format(EDITHEADETEXT, user.LoginName); 
                tbLogin.Text = user.LoginName;
                tbFirstName.Text = user.FirstName;
                tbLastName.Text = user.LastName;
                CurrentPage.StoreObject(user, Constants.USEREDITOBJECTNAME);
            }
            SetControlsVisibility();
            tbLogin.Focus();
        }
        private void SetControlsVisibility()
        {
            trPassword.Visible = !isNew;
            tbPassword.Enabled = !trPassword.Visible;
            tbConfirmPassword.Enabled = tbPassword.Enabled;
            tbPassword.CssClass = ADMININPUTCSS + (tbPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbConfirmPassword.CssClass = ADMININPUTCSS + (tbConfirmPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            if (trPassword.Visible)
            {
                cbEnablePassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbPassword.ClientID, tbConfirmPassword.ClientID));
            }
        }
        private void goBack()
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWUSER);
        }
        #endregion

        #region event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            goBack();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            user = CurrentPage.GetObject(Constants.USEREDITOBJECTNAME) as AppUser;
            if (user == null)
            {
                user = new AppUser(userid);
            }
            if (ValidateData())
            {
                user.LoginName = tbLogin.Text;
                if (isNew || cbEnablePassword.Checked)
                {
                    user.Password = tbPassword.Text;
                }
                user.FirstName = tbFirstName.Text;
                user.LastName = tbLastName.Text;
                //if (isNew || !isMe)
                //{
                //    user.CompanyId = int.Parse(ddlCompany.SelectedValue);
                //}
                int result = user.SaveGlobalAdminUser();
                if (result < 1)
                {
                    lblMessage.Text = user.LastError;
                }
                else
                {
                    lblMessage.Text = Constants.SUCCESSMESSAGE;
                }
            }
        }
        private bool ValidateData()
        {
            bool res = true;
            if (isNew)
            {
                string message;
                bool r = Utils.ValidatePassword(tbPassword.Text, user.PreviosPasswords, out message);
                if (!r)
                {
                    lblPasswordErr.Text = message;
                    res = false;
                }
                else
                {
                    if (tbConfirmPassword.Text != tbPassword.Text)
                    {
                        lblConfirmPasswordErr.Text = Utils.PASSWORDNOTIDENTICALTEXT;
                        res = false;
                    }
                }
            }
            else
            {
                if (cbEnablePassword.Checked)
                {
                    string message;
                    bool r = Utils.ValidatePassword(tbPassword.Text,user.PreviosPasswords, out message);
                    if (!r)
                    {
                        lblPasswordErr.Text = message;
                        res = false;
                    }
                    else
                    {
                        if (tbConfirmPassword.Text != tbPassword.Text)
                        {
                            lblConfirmPasswordErr.Text = Utils.PASSWORDNOTIDENTICALTEXT;
                            res = false;
                        }
                    }
                }
            }
            return res;
        }

        #endregion
    }
}
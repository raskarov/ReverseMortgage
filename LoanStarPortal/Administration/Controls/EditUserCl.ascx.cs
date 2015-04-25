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

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditUserCl : AppControl
    {
        #region constants
        private const string ADDHEADERTEXT = "Add new  user";
        private const string EDITHEADETEXT = "Edit  user({0})";
        private const string ADMININPUTCSS = "admininput";
        private const string DISABLEDSUFFIX = "dis";
        private const string CLICKHANDLER = "onclick";
        private const string JSSETPASSWORD = "SetPassword(this.checked,{0},{1})";
        #endregion

        #region fields
        private int userid = -1;
        private AppUser user;
        bool isNew;
        //bool isMe;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsCorrespondentLenderAdmin)
            {
                Response.Redirect(ResolveUrl("../" + CurrentUser.GetDefaultPage()));
            }
            userid = ((AppPage)Page).GetValueInt(Constants.IDPARAM, userid);
            isNew = userid < 0;
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        #region methods
        private void BindData()
        {
            user = new AppUser(userid);
            user.CompanyId = CurrentUser.CompanyId;
            DataList1.DataSource = user.GetAllPossibleRoles();
            DataList1.DataBind();
            if (isNew)
            {
                lblHeader.Text = ADDHEADERTEXT;
            }
            else
            {
                //isMe = user.Id == CurrentUser.Id;
                lblHeader.Text = String.Format(EDITHEADETEXT, user.LoginName); 
                tbLogin.Text = user.LoginName;
                tbFirstName.Text = user.FirstName;
                tbLastName.Text = user.LastName;
                tbUserName.Text = user.MailUserName;
                tbMailPassword.Text = user.MailPassword;
                tbUserMail.Text = user.MailAddress;
            }
            CurrentPage.StoreObject(user, Constants.USEREDITOBJECTNAME);
            SetControlsVisibility();
            tbLogin.Focus();
        }
        private void SetControlsVisibility()
        {
            trPassword.Visible = !isNew;
            tbPassword.Enabled = !trPassword.Visible;
            tbConfirmPassword.Enabled = tbPassword.Enabled;
            trMailPassword.Visible = !isNew;
            tbMailPassword.Enabled = !trMailPassword.Visible;
            tbMailPasswordConfirm.Enabled = tbMailPassword.Enabled;
            tbPassword.CssClass = ADMININPUTCSS + (tbPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbConfirmPassword.CssClass = ADMININPUTCSS + (tbConfirmPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbMailPassword.CssClass = ADMININPUTCSS + (tbMailPassword.Enabled ? String.Empty : DISABLEDSUFFIX);
            tbMailPasswordConfirm.CssClass = ADMININPUTCSS + (tbMailPasswordConfirm.Enabled ? String.Empty : DISABLEDSUFFIX);
            if (trPassword.Visible)
            {
                cbEnablePassword.Attributes.Add(CLICKHANDLER,String.Format(JSSETPASSWORD,tbPassword.ClientID,tbConfirmPassword.ClientID));
            }
            if (trMailPassword.Visible)
            {
                cbEnableMailPassword.Attributes.Add(CLICKHANDLER, String.Format(JSSETPASSWORD, tbMailPassword.ClientID, tbMailPasswordConfirm.ClientID));
            }
        }
        private int[] GetRoles()
        {
            int[] result = null;
            ArrayList list = new ArrayList();
            foreach (DataListItem item in DataList1.Items)
            {
                CheckBox cb = (CheckBox)item.FindControl("Checkbox1");
                if (cb != null && cb.Checked)
                {
                    list.Add(DataList1.DataKeys[item.ItemIndex]);
                }
            }
            if (list.Count > 0)
            {
                result = new int[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    result[i] = (int)list[i];
                }
            }
            return result;
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
            user.LoginName = tbLogin.Text;
            if (isNew || cbEnablePassword.Checked)
            {
                user.Password = tbPassword.Text;
            }
            user.FirstName = tbFirstName.Text;
            user.LastName = tbLastName.Text;
            user.MailUserName = tbUserName.Text;
            user.MailPassword = tbMailPassword.Text;
            user.MailAddress = tbUserMail.Text;
            user.CompanyId = CurrentUser.CompanyId;
            int[] roles = GetRoles();
            if (roles == null)
            { 
                roles = new int[1];
                roles[0] = 0;
            }
            int result = user.SaveWithRole(roles);
            if (result < 1)
            {
                lblMessage.Text = user.LastError;
            }
            else
            {
                lblMessage.Text = Constants.SUCCESSMESSAGE;
            }
        }
        #endregion
    }
}
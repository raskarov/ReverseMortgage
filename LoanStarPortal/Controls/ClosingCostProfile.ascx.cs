using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class ClosingCostProfile : AppControl
    {
        #region constants
        public const string ADDNEW = "Add new";
        public const int ADDNEWVALUE = 0;
        public const string CURRENTPROFILEID = "currentprofileid";
        private const string ONCLICK = "onclick";
        private const string DELETEJS = "javascript:{{var r=confirm('Delete this profile?');if (!r)return false;}};";
        #endregion

        #region fields
        private LoanStar.Common.ClosingCostProfile currentProfile;
        private DataView dvProfiles;
        private bool lateBinding = false;
        #endregion

        #region properties
        private DataView DvProfiles
        {
            get 
            {
                if (dvProfiles == null)
                {
                    dvProfiles = LoanStar.Common.ClosingCostProfile.GetClosingCostProfileList(CurrentUser.CompanyId);
                }
                return dvProfiles;
            }
        }
        private int ClosingCostProfileId
        {
            get
            {
                int res = ADDNEWVALUE;
                Object o = Session[CURRENTPROFILEID];
                if (o != null)
                {
                    try
                    {
                        res = int.Parse(o.ToString());
                    }
                    catch
                    {
                    }
                }
                return res;
            }
            set
            {
                Session[CURRENTPROFILEID] = value;
            }
        }
        private LoanStar.Common.ClosingCostProfile CurrentProfile
        {
            get
            {
                if (currentProfile == null)
                {
                    currentProfile = new LoanStar.Common.ClosingCostProfile(ClosingCostProfileId);
                }
                return currentProfile;
            }
        }
        #endregion
        private void BindData()
        {
            ddlProfiles.DataSource = DvProfiles;
            ddlProfiles.DataTextField = "Name";
            ddlProfiles.DataValueField = "Id";
            ddlProfiles.DataBind();
            ddlProfiles.Items.Insert(0,new ListItem(ADDNEW, ADDNEWVALUE.ToString()));
            if (ClosingCostProfileId > 0)
            {
                ddlProfiles.SelectedValue = ClosingCostProfileId.ToString();
                tbProfileName.Text = ddlProfiles.SelectedItem.Text;
                btnDelete.Enabled = true;
            }
            else
            {
                tbProfileName.Text = "";
                btnDelete.Enabled = false;
            }
       }
        protected void ddlProfile_IndexChanged(object sender, EventArgs e)
        {
            ClosingCostProfileId = int.Parse(ddlProfiles.SelectedValue);
            currentProfile = null;
            BindData();
        }
        protected void Save_Click(object sender, EventArgs e)
        {
            CurrentProfile.Name = tbProfileName.Text;
            int res = CurrentProfile.Save(CurrentUser.CompanyId);
            if (res > 0)
            {
                ClosingCostProfileId = res;
                currentProfile = null;
                dvProfiles = null;
                BindData();
                ClosingCostProfileView1.BindData();
            }
            else if (res == -1)
            {
                lblErr.Text = "Profile with such name already exists. Please select another one"; 
            }
        }
        protected void Delete_Click(object sender, EventArgs e)
        {
            if (CurrentProfile.Delete())
            {
                ClosingCostProfileId = 0;
                currentProfile = null;
                dvProfiles = null;
                BindData();
                ClosingCostProfileView1.ResetGridData();
                ClosingCostProfileView1.BindData();
            }
        }
        private bool ProcessPostBack()
        {
            if (!String.IsNullOrEmpty(Page.Request["__EVENTTARGET"]))
            {
                string controlName = Page.Request["__EVENTTARGET"];
                if (controlName.EndsWith(ddlProfiles.ID))
                {
                    try
                    {
                        ClosingCostProfileId = int.Parse(Page.Request.Form[controlName.Replace(":", "$")]);
                        lateBinding = true;
                        return false;
                    }
                    catch
                    {
                    }

                }
                else if (controlName.EndsWith(btnSave.ID) || controlName.EndsWith(btnDelete.ID))
                {
                    return true;
                }
            }
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Attributes.Add(ONCLICK, DELETEJS);
            lblErr.Text = "";
            if (ProcessPostBack())
            {
                if (!lateBinding) BindData();
            }            
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (lateBinding) BindData();
        }
    }
}


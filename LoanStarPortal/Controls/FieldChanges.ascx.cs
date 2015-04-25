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
using Telerik;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class FieldChanges : AppControl
    {
        #region Fields/Properties
        private const string FIRST_LOAD = "FieldChangesFirstLoad";

        private int MortgageProfileID
        {
            get
            {
                if (Session[Constants.MortgageID] == null)
                    Session[Constants.MortgageID] = 0;
                return Convert.ToInt32(Session[Constants.MortgageID].ToString());
            }
        }

        #endregion

        #region Methods
        private void BindGroups()
        {
            DataView dv = Field.GetFieldGroup(true);
            if (dv.Table.Rows.Count > 0) dv.Sort = Field.GROUPNAMEFIELDNAME + " ASC";
            ddlGroup.DataSource = dv;
            ddlGroup.DataTextField = Field.GROUPNAMEFIELDNAME;
            ddlGroup.DataValueField = Field.IDFIELDNAME;
            ddlGroup.DataBind();
            ddlField.Items.Insert(0, new RadComboBoxItem(" - Select - ", "0"));
            ddlField.Enabled = false;
        }

        private void BindFields()
        {
            DataView dv = Field.GetFieldInGroup(Convert.ToInt32(ddlGroup.SelectedValue));
            if (dv.Table.Rows.Count > 0) dv.Sort = Field.DESCRIPTIONFIELDNAME + " ASC";
            ddlField.DataSource = dv;
            ddlField.DataTextField = Field.DESCRIPTIONFIELDNAME;
            ddlField.DataValueField = Field.IDFIELDNAME;
            ddlField.DataBind();
            ddlField.Items.Insert(0, new RadComboBoxItem(" - Select - ", "0"));
            ddlField.Enabled = true;
        }
        private void BindGrid(int MPFieldID)
        {
            DataView dv = Field.GetFieldChanges(MortgageProfileID, MPFieldID);
            gridChanges.DataSource = dv;
            gridChanges.DataBind();
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState[FIRST_LOAD] == null)
            {
                BindGrid(0);
                BindGroups();
                ViewState[FIRST_LOAD] = 1;
            }
        }
        protected void ddlField_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindGrid(Convert.ToInt32(ddlField.SelectedValue));
        }
        protected void ddlGroup_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindFields();
        }

        #endregion

        protected void gridChanges_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            DataView dv = Field.GetFieldChanges(MortgageProfileID, Convert.ToInt32(ddlField.SelectedValue));
            gridChanges.DataSource = dv;
        }

    }
}
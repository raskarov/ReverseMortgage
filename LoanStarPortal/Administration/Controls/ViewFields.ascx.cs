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
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Administration.Controls
{
    public partial class ViewFields : System.Web.UI.UserControl
    {
        private const string PrevFilter = "FiledFilter";

        string PrevEntity
        {
            set
            {
                ViewState[PrevFilter] = value;
            }
            get
            {
                if (ViewState[PrevFilter] == null)
                    return "";
                else
                    return Convert.ToString(ViewState[PrevFilter]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
            else
            {
                this.PreRender += new EventHandler(ViewFields_PreRender);
            }
        }

        void ViewFields_PreRender(object sender, EventArgs e)
        {
            if (PrevEntity != txtEntity.Text)
                BindGrid();
        }

        private void BindGrid()
        {
            G.DataSource = FieldInfo.GetFieldInfoList(txtEntity.Text);
            G.DataBind();
            PrevEntity = txtEntity.Text;
        }

        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            if ((e.CommandName == RadGrid.PerformInsertCommandName) || (e.CommandName == RadGrid.UpdateCommandName))
            {
                int id = -1;
                if (e.CommandName == RadGrid.UpdateCommandName && e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]!=DBNull.Value)
                {
                    id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                }

                FieldInfo fi = new FieldInfo(id);

                CheckBox chbSignedOff = e.Item.FindControl("chbSignedOff") as CheckBox;
                if (chbSignedOff != null)
                    fi.SignedOff = chbSignedOff.Checked;

                TextBox txtEntity = e.Item.FindControl("txtEntity") as TextBox;
                if(txtEntity != null)
                    fi.Entity = txtEntity.Text;

                DropDownList ddlUIControl = e.Item.FindControl("ddlUIControl") as DropDownList;
                if (ddlUIControl != null)
                    fi.UIControlID = Convert.ToInt32(ddlUIControl.SelectedValue);

                TextBox txtLabel = e.Item.FindControl("txtLabel") as TextBox;
                if (txtLabel != null)
                {
                    fi.Label = txtLabel.Text;
                }

                HiddenField hdnMortgageProfileID = e.Item.FindControl("hdnMortgageProfileID") as HiddenField;
                if (hdnMortgageProfileID != null && hdnMortgageProfileID.Value!="")
                    fi.MortgageProfileID = Convert.ToInt32(hdnMortgageProfileID.Value);

                TextBox txtValue = e.Item.FindControl("txtValue") as TextBox;
                if (txtValue != null)
                {
                    fi.Value = txtValue.Text;
                }
                TextBox txtFieldName = e.Item.FindControl("txtFieldName") as TextBox;
                if (txtFieldName != null)
                {
                    fi.FieldName = txtFieldName.Text;
                }

                DropDownList ddlGeneralPurpose = e.Item.FindControl("ddlGeneralPurpose") as DropDownList;
                if (ddlGeneralPurpose != null)
                    fi.GeneralPurposeID = Convert.ToInt32(ddlGeneralPurpose.SelectedValue);

                DropDownList ddlValueType = e.Item.FindControl("ddlValueType") as DropDownList;
                 if (ddlValueType != null)
                     fi.ValueTypeID = Convert.ToInt32(ddlValueType.SelectedValue);

                TextBox txtDescription = e.Item.FindControl("txtDescription") as TextBox;
                if (txtDescription != null)
                {
                    fi.Description = txtDescription.Text;
                }

                DropDownList ddlSiteType = e.Item.FindControl("ddlSiteType") as DropDownList;
                if (ddlSiteType != null)
                     fi.SiteTypeID = Convert.ToInt32(ddlSiteType.SelectedValue);

                TextBox txtPath1 = e.Item.FindControl("txtPath1") as TextBox;
                if (txtPath1 != null)
                {
                    fi.Path1 = txtPath1.Text;
                }
                TextBox txtPath2 = e.Item.FindControl("txtPath2") as TextBox;
                if (txtPath2 != null)
                {
                    fi.Path2 = txtPath2.Text;
                }
                TextBox txtPath3 = e.Item.FindControl("txtPath3") as TextBox;
                if (txtPath3 != null)
                {
                    fi.Path3 = txtPath3.Text;
                }
                TextBox txtPath4 = e.Item.FindControl("txtPath4") as TextBox;
                if (txtPath4 != null)
                {
                    fi.Path4 = txtPath4.Text;
                }
                TextBox txtPath5 = e.Item.FindControl("txtPath5") as TextBox;
                if (txtPath5 != null)
                {
                    fi.Path5 = txtPath5.Text;
                }

                fi.Save();
            }
            BindGrid();            
        }

        protected void G_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (editedItem != null)
                {
                    DropDownList ddlUIControl = editedItem.FindControl("ddlUIControl") as DropDownList;
                    if (ddlUIControl != null)
                    {
                        ddlUIControl.DataSource = FieldInfo.GetUIControlList();
                        ddlUIControl.DataBind();
                        //ddlUIControl.Items.Insert(0, new ListItem("-Not Stated-", "-1"));
                    }
                    DropDownList ddlGeneralPurpose = editedItem.FindControl("ddlGeneralPurpose") as DropDownList;
                    if (ddlGeneralPurpose != null)
                    {
                        ddlGeneralPurpose.DataSource = FieldInfo.GetGeneralPurposeList();
                        ddlGeneralPurpose.DataBind();
                        //ddlGeneralPurpose.Items.Insert(0, new ListItem("-Not Stated-", "-1"));
                    }
                    DropDownList ddlValueType = editedItem.FindControl("ddlValueType") as DropDownList;
                    if (ddlValueType != null)
                    {
                        ddlValueType.DataSource = FieldInfo.GetValueTypeList();
                        ddlValueType.DataBind();
                        //ddlValueType.Items.Insert(0, new ListItem("-Not Stated-", "-1"));
                    }
                    DropDownList ddlSiteType = editedItem.FindControl("ddlSiteType") as DropDownList;
                    if (ddlSiteType != null)
                    {
                        ddlSiteType.DataSource = FieldInfo.GetSiteTypeList();
                        ddlSiteType.DataBind();
                        //ddlSiteType.Items.Insert(0, new ListItem("-Not Stated-", "-1"));
                    }

                    CheckBox chbSignedOff = editedItem.FindControl("chbSignedOff") as CheckBox;

                    DataRowView dr = editedItem.DataItem as DataRowView;
                    if (dr != null)
                    {
                        ddlUIControl.SelectedIndex = ddlUIControl.Items.IndexOf(ddlUIControl.Items.FindByValue(dr["UIControlID"].ToString()));
                        ddlGeneralPurpose.SelectedIndex = ddlGeneralPurpose.Items.IndexOf(ddlGeneralPurpose.Items.FindByValue(dr["GeneralPurposeID"].ToString()));
                        ddlValueType.SelectedIndex = ddlValueType.Items.IndexOf(ddlValueType.Items.FindByValue(dr["ValueTypeID"].ToString()));
                        ddlSiteType.SelectedIndex = ddlSiteType.Items.IndexOf(ddlSiteType.Items.FindByValue(dr["SiteTypeID"].ToString()));
                        chbSignedOff.Checked = (dr["SignedOff"] == DBNull.Value) ? false : Convert.ToBoolean(dr["SignedOff"]);
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
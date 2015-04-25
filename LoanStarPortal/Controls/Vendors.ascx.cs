using System;
using System.Data;
using System.Xml;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Telerik.WebControls;
using LoanStar.Common;

namespace LoanStarPortal.Controls
{
    public partial class Vendors : AppControl
    {
        //#region constants
        //private const string VENDORFEETYPE = "vendorfees";
        //private const string CATEGORYFILTER = "chargecategoryid={0}";
        //private const string ROOTELEMENT = "Root";
        //private const string ITEMELEMENT = "item";
        //private const string IDATTRIBUTE = "id";
        //#endregion

        //#region fields
        //private Vendor vendor;
        //private DataView dvGridData;
        //private DataView dvChargeCategory;
        //private DataView dvVendorFee;
        //#endregion

        //#region Properties
        //protected DataView DvGridData
        //{
        //    get
        //    {
        //        if (dvGridData == null)
        //        {
        //            dvGridData = Vendor.GetCompanyVendors(CurrentUser.CompanyId);
        //        }
        //        return dvGridData;
        //    }
        //}
        //protected DataView DvVendorFee
        //{
        //    get
        //    {
        //        if(dvVendorFee==null)
        //        {
        //            dvVendorFee = Vendor.GetVendorFeeType(VendorID);
        //        }
        //        return dvVendorFee;
        //    }
        //}
        //protected DataView DvChargeCategory
        //{
        //    get
        //    {
        //        if(dvChargeCategory==null)
        //        {
        //            dvChargeCategory = Vendor.GetChargeCategory();
        //        }
        //        return dvChargeCategory;
        //    }
        //}
        //public int VendorID
        //{
        //    get
        //    {
        //        if (ViewState["VendorID"] == null)
        //            ViewState["VendorID"] = -1;
        //        return Convert.ToInt32(ViewState["VendorID"].ToString());
        //    }
        //    set
        //    {
        //        ViewState["VendorID"] = value;
        //    }
        //}
        //private Hashtable FeeTypes
        //{
        //    get
        //    {
        //        Hashtable res = (Hashtable)Session[VENDORFEETYPE];
        //        if(res==null)
        //        {
        //            res = GetVendorFees();
        //            Session[VENDORFEETYPE] = res;
        //        }
        //        return res;
        //    }
        //    set { Session[VENDORFEETYPE] = value; }
        //}
        //#endregion

        //#region methods
        //public DataView LoadTypes()
        //{
        //    DataView dv = DvChargeCategory;
        //    return dv;
        //}
        //private Hashtable GetVendorFees()
        //{
        //    DataView dv = new DataView(DvVendorFee.Table);
        //    Hashtable res = new Hashtable();
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        int id = int.Parse(dv[i]["id"].ToString());
        //        bool selected = int.Parse(dv[i]["selected"].ToString()) == 1;
        //        res.Add(id, selected);
        //    }
        //    return res;
        //}
        //private void SetFeeType(ListControl cbl)
        //{
        //    Hashtable ht = FeeTypes;
        //    for (int i = 0; i < cbl.Items.Count; i++)
        //    {
        //        int id = int.Parse(cbl.Items[i].Value);
        //        bool selected = cbl.Items[i].Selected;
        //        if (ht.ContainsKey(id))
        //        {
        //            ht[id] = selected;
        //        }
        //        else
        //        {
        //            ht.Add(id, selected);
        //        }
        //    }
        //    FeeTypes = ht;
        //}
        //private void BindFeeTypes(ListControl cbl, int chargeCategoryId, string divId)
        //{
        //    DataView dv = DvVendorFee;
        //    dv.RowFilter = String.Format(CATEGORYFILTER, chargeCategoryId);
        //    cbl.Items.Clear();
        //    string arr = String.Empty;
        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        int id = int.Parse(dv[i]["id"].ToString());
        //        ListItem li = new ListItem(dv[i]["name"].ToString(), id.ToString());
        //        li.Attributes.Add("typeid",id.ToString());
        //        bool selected = false;
        //        if (FeeTypes.ContainsKey(id))
        //        {
        //            selected = (bool)FeeTypes[id];
        //        }
        //        bool hasExtendedProperty = bool.Parse(dv[i]["hasextendedproperty"].ToString());
        //        if (hasExtendedProperty)
        //        {
        //            if(!String.IsNullOrEmpty(arr))
        //            {
        //                arr += ",";
        //            }
        //            arr += id.ToString();
        //        }

        //        li.Selected = selected;
        //        cbl.Items.Add(li);
        //    }
        //    if(!String.IsNullOrEmpty(arr))
        //    {
        //        cbl.Attributes.Add("onclick","SetExtPropertyDiv(this,'"+arr+"','"+divId+"');");
        //    }
        //}
        //protected void SaveVendor(GridItem item)
        //{
        //    vendor = new Vendor(VendorID);
        //    if (VendorID <= 0) vendor.CompanyID = CurrentUser.CompanyId;
        //    TextBox CompanyName = (TextBox)item.FindControl("CompanyName");
        //    if (CompanyName != null)
        //    {
        //        vendor.CompanyName = CompanyName.Text;
        //    }
        //    TextBox FirstName = (TextBox)item.FindControl("FirstName");
        //    if (FirstName != null)
        //    {
        //        vendor.FirstName = FirstName.Text;
        //    }
        //    TextBox LastName = (TextBox)item.FindControl("LastName");
        //    if (LastName != null)
        //    {
        //        vendor.LastName = LastName.Text;
        //    }
        //    TextBox Address = (TextBox)item.FindControl("Address");
        //    if (Address != null)
        //    {
        //        vendor.Address = Address.Text;
        //    }
        //    string xml = String.Empty;
        //    DropDownList ddl = (DropDownList)item.FindControl("ddlChargeCategory");
        //    if (ddl != null)
        //    {
        //        CheckBoxList cbl = (CheckBoxList)item.FindControl("cblFeeType");
        //        if (cbl != null)
        //        {
        //            SetFeeType(cbl);
        //            xml = GetFeeTypesXml();
        //        }
        //    }
        //    if(HasExtraParameters())
        //    {
        //        TextBox PhoneNumber = (TextBox)item.FindControl("PhoneNumber");
        //        if (PhoneNumber != null && !String.IsNullOrEmpty(PhoneNumber.Text))
        //        {
        //            vendor.PhoneNumber = PhoneNumber.Text;
        //        }
        //        TextBox AltPhoneNumber = (TextBox)item.FindControl("AltPhoneNumber");
        //        if (AltPhoneNumber != null && !String.IsNullOrEmpty(AltPhoneNumber.Text))
        //        {
        //            vendor.AltPhoneNumber = AltPhoneNumber.Text;
        //        }
        //        TextBox FaxNumber = (TextBox)item.FindControl("FaxNumber");
        //        if (FaxNumber != null && !String.IsNullOrEmpty(FaxNumber.Text))
        //        {
        //            vendor.FaxNumber = FaxNumber.Text;
        //        }
        //        TextBox AltFaxNumber = (TextBox)item.FindControl("AltFaxNumber");
        //        if (AltFaxNumber != null && !String.IsNullOrEmpty(AltFaxNumber.Text))
        //        {
        //            vendor.AltFaxNumber = AltFaxNumber.Text;
        //        }
        //        TextBox Email = (TextBox)item.FindControl("MailAddress");
        //        if (Email != null && !String.IsNullOrEmpty(Email.Text))
        //        {
        //            vendor.MailAddress = Email.Text;
        //        }
        //        TextBox AltEmail = (TextBox)item.FindControl("AltMailAddress");
        //        if (AltEmail != null && !String.IsNullOrEmpty(AltEmail.Text))
        //        {
        //            vendor.AltMailAddress = AltEmail.Text;
        //        }
        //        TextBox Login = (TextBox)item.FindControl("Login");
        //        if (Login != null && !String.IsNullOrEmpty(Login.Text))
        //        {
        //            vendor.LoginName = Login.Text;
        //        }
        //        TextBox Password = (TextBox)item.FindControl("Password");
        //        if (Password != null && !String.IsNullOrEmpty(Password.Text))
        //        {
        //            vendor.Password = Password.Text;
        //        }
        //        CheckBox chkDisabled = (CheckBox)item.FindControl("chkDisabled");
        //        if (chkDisabled != null)
        //        {
        //            bool isDisabled = chkDisabled.Checked;
        //            vendor.IsDisabled = isDisabled;
        //        }

        //    }
        //    vendor.Save(xml);
        //}
        //private string GetFeeTypesXml()
        //{
        //    string res = String.Empty;
        //    XmlDocument d = new XmlDocument();
        //    XmlNode root = d.CreateElement(ROOTELEMENT);
        //    foreach (DictionaryEntry item in FeeTypes)
        //    {
        //        if ((bool)item.Value)
        //        {
        //            XmlNode n = d.CreateElement(ITEMELEMENT);
        //            XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
        //            a.Value = item.Key.ToString();
        //            n.Attributes.Append(a);
        //            root.AppendChild(n);
        //        }
        //    }
        //    if (root.ChildNodes.Count > 0)
        //    {
        //        d.AppendChild(root);
        //        res = d.OuterXml;
        //    }
        //    return res;
        //}
        //private bool HasExtraParameters()
        //{
        //    bool res = false;
        //    DataView dv = new DataView(DvVendorFee.Table);
        //    dv.RowFilter = "hasextendedproperty=1 and selected=1";
        //    if(dv.Count>0)
        //    {
        //        for(int i=0;i<dv.Count;i++)
        //        {
        //            int id = int.Parse(dv[i]["id"].ToString());
        //            if(FeeTypes.ContainsKey(id))
        //            {
        //                res = (bool) FeeTypes[id];
        //                if (res) break;
        //            }
        //        }
        //    }
        //    return res;
        //}
        //private void BindFeeTypePanel(WebControl ctl)
        //{
        //    DropDownList ddl = (DropDownList)ctl.FindControl("ddlChargeCategory");
        //    if (ddl != null)
        //    {
        //        vendor = new Vendor(VendorID);
        //        int chargeCategoryId = 1;
        //        try
        //        {
        //            chargeCategoryId = int.Parse(ddl.SelectedValue);
        //        }
        //        catch
        //        {
        //        }
        //        CheckBoxList cbl = (CheckBoxList)ctl.FindControl("cblFeeType");
        //        Panel pnl = (Panel)ctl.FindControl("ExtraParam");
        //        if (pnl != null)
        //        {
        //            string vis = "none";
        //            if (HasExtraParameters()) vis = "block";
        //            pnl.Attributes.Add("style", "display:" + vis);
        //        }
        //        BindFeeTypes(cbl, chargeCategoryId, pnl.ClientID);
        //    }
        //}
        //#endregion

        //#region eventhandlers
        //protected void gVendors_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        //{
        //    gVendors.DataSource = DvGridData;
        //}
        //protected void ChargeCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GridEditableItem geitem = ((DropDownList)sender).NamingContainer as GridEditableItem;
        //    if (geitem != null)
        //    {
        //        DropDownList ddl = (DropDownList)geitem.FindControl("ddlChargeCategory");
        //        if (ddl != null)
        //        {
        //            vendor = new Vendor(VendorID);
        //            int categoryId = int.Parse(ddl.SelectedValue);
        //            CheckBoxList cbl = (CheckBoxList)geitem.FindControl("cblFeeType");
        //            if (cbl != null)
        //            {
        //                SetFeeType(cbl);
        //                Panel pnl = (Panel)geitem.FindControl("ExtraParam");
        //                BindFeeTypes(cbl, categoryId,pnl.ClientID);
        //            }
        //        }
        //    }
        //}
        //protected void gVendors_ItemCommand(object source, GridCommandEventArgs e)
        //{
        //    rowError.Visible = false;
        //    if (e.CommandName == RadGrid.InitInsertCommandName)
        //    {
        //        VendorID = -1;
        //        if (gVendors.EditItems.Count > 0)
        //        {
        //            gVendors.MasterTableView.ClearEditItems();
        //        }
        //    }
        //    else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
        //    {
        //        e.Canceled = true;
        //    }
        //    else if (e.CommandName == RadGrid.PerformInsertCommandName)
        //    {
        //        Panel PanelTitleCompanies = (Panel)e.Item.FindControl("PanelTitleCompanies");
        //        if (PanelTitleCompanies != null && PanelTitleCompanies.Visible)
        //        {
        //            TextBox tb = (TextBox)e.Item.FindControl("Login");
        //            if (tb != null)
        //            {
        //                if (!Vendor.CheckLogin(tb.Text))
        //                {
        //                    rowError.Visible = true;
        //                    e.Canceled = true;
        //                    return;
        //                }
        //            }
        //        }
        //        SaveVendor(e.Item);
        //    }
        //    else if (e.CommandName == RadGrid.UpdateCommandName)
        //    {
        //        VendorID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
        //        Panel PanelTitleCompanies = (Panel)e.Item.FindControl("PanelTitleCompanies");
        //        if (PanelTitleCompanies != null && PanelTitleCompanies.Visible)
        //        {
        //            vendor = new Vendor(VendorID);
        //            TextBox tb = (TextBox)e.Item.FindControl("Login");
        //            if (tb != null)
        //            {
        //                if (vendor.LoginName != tb.Text)
        //                {
        //                    if (!Vendor.CheckLogin(tb.Text))
        //                    {
        //                        rowError.Visible = true;
        //                        e.Item.OwnerTableView.IsItemInserted = false;
        //                        e.Canceled = true;
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //        SaveVendor(e.Item);
        //        gVendors.MasterTableView.ClearEditItems();
        //    }
        //    else if (e.CommandName == RadGrid.DeleteCommandName)
        //    {
        //        int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
        //        vendor = new Vendor(id);
        //        vendor.Delete();
        //    }
        //    if (e.CommandName == RadGrid.EditCommandName)
        //    {
        //        e.Item.OwnerTableView.IsItemInserted = false;
        //        Session[VENDORFEETYPE] = null;
        //    }
        //    gVendors.Rebind();
        //}
        //protected void gVendors_ItemDataBound(object sender, GridItemEventArgs e)
        //{
        //    if (e.Item is GridEditFormItem && e.Item.IsInEditMode && e.Item.ItemIndex >= 0)
        //    {
        //        VendorID = Convert.ToInt32(gVendors.MasterTableView.DataKeyValues[e.Item.ItemIndex]["ID"]);
        //        GridEditFormItem edit = (GridEditFormItem)e.Item;
        //        BindFeeTypePanel(edit as WebControl);
        //    }
        //    else if (e.Item is GridEditFormInsertItem)
        //    {
        //        VendorID = -1;
        //        GridEditFormInsertItem item = (GridEditFormInsertItem)e.Item;
        //        BindFeeTypePanel(item as WebControl);
        //    }
        //}
        
        //#endregion
   }
}
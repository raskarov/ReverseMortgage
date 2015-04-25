using System;
using System.Data;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class TabPackage : AppControl
    {
        //private const string NOTSELECTEDTEXT = "-Select-";
        //private const string NOTSELECTEDVALUE = "";
        #region Fields
        private const string FIRST_LOAD = "TabPackFirstLoad";
        private bool isBaydocsGridVisible = true;
        #endregion

        #region properties
        private int CompanyID
        {
            get
            {
                return ((AppPage)Page).CurrentUser.CompanyId;
            }
        }
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CurrentPage.MortgageChanged += CurrentPage_MortgageChanged;
        }
        private DataTable GetMortgageDocTemplateTable()
        {
            DataTable docTemplTbl = null;
            try
            {
                if (MortgageProfileID <= 0)
                    return null;
                
                MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
                mp.BuildRuleEvaluationTree(CurrentPage.GetRuleTreePublic());
                if (CompanyID != mp.CompanyID)
                    throw new Exception("Mortgage Profile is from another Company");

                docTemplTbl = mp.GetDocTemplateList(true);
            }
            catch { }

            return docTemplTbl == null || docTemplTbl.Rows.Count == 0 ? null : docTemplTbl;
        }

        private void RebindData()
        {
            DataTable docTemplateTable = GetMortgageDocTemplateTable();

            ucAppPackage.DocTemplateTable = docTemplateTable;
            ucAppPackage.BindData();

            ucClosingPackage.DocTemplateTable = docTemplateTable;
            ucClosingPackage.BindData();

            ucMiscPackage.DocTemplateTable = docTemplateTable;
            ucMiscPackage.BindData();

            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "InitTabPackage", "<script language=\"javascript\" type=\"text/javascript\">OnClientTabPackageLoaded();</script>");
        }
        private void SetBaydocsGridVisibility(MortgageProfile mp)
        {
            isBaydocsGridVisible = mp.Product != null && mp.ProductID>0 && mp.Product.UseBaydocsAppPackagesYN;
        }
        private int GetPackageType()
        {
            int res = 0;
            switch (RadTabStripPackage.SelectedIndex)
            {
                case 0:
                    res = DocumentCompany.APPLICATIONPACKAGETYPE;
                    break;
                case 1:
                    res = DocumentCompany.CLOSINGPACKAGETYPE;
                    break;
            }
            return res;
        }

        #endregion

        #region Event handlers
        protected void G_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                if (dataItem != null)
                {
                    ImageButton btn = (ImageButton)dataItem["EditCommandColumn"].FindControl("EditButton");
                    if(btn!=null)
                    {
                        btn.Visible = false;
                    }
                }
            }
        }
        protected void G_ItemCommand(object source, GridCommandEventArgs e)
        {
            bool isInEditMode = e.CommandName == RadGrid.InitInsertCommandName;
            if (e.CommandName == RadGrid.PerformInsertCommandName)
            {
                XmlMapperWrapper wrapper = new XmlMapperWrapper(CurrentPage, true);
                DocumentCompany company = new DocumentCompany(1);
                wrapper.Map(CurrentPage.GetMortgage(MortgageProfileID), company, CurrentUser.Id,GetPackageType());
            }
            else if (e.CommandName == "GetPdf")
            {
                string fileName = DocumentRequestor.SavePdf(int.Parse(e.CommandArgument.ToString()), Server.MapPath(Constants.STORAGEFOLDER));
                if(!String.IsNullOrEmpty(fileName))
                {
                    CurrentPage.ClientScript.RegisterStartupScript(GetType(), "CreatePdf", String.Format("<script language=\"javascript\" type=\"text/javascript\">location.href = 'DocPackage.aspx?file={0}';</script>",fileName));
                }
            }
        }
        protected void G_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem editedItem = e.Item as GridEditableItem;
                if (editedItem != null)
                {
                    DropDownList ddl = editedItem.FindControl("ddlCompany") as DropDownList;
                    if (ddl != null)
                    {
                        ddl.DataSource = DocumentCompany.GetCompanyList();
                        ddl.DataTextField = "name";
                        ddl.DataValueField = "id";
                        ddl.DataBind();
                        ListItem li = new ListItem("-Select-", "");
                        ddl.Items.Insert(0,li);
                    }
                }
            }  
        }

        protected void MortgageProfiles_DataChange()
        {
            RebindData();
        }
        public void RecalculateDocList(MortgageProfile profile)
        {
            SetBaydocsGridVisibility(profile);
            RebindData();
        }
        protected void CurrentPage_MortgageChanged(object sender, int MortgageID)
        {
            RebindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "InitTabPackage", "<script language=\"javascript\" type=\"text/javascript\">OnClientTabPackageLoaded();</script>");

            if (ViewState[FIRST_LOAD] != null)
            {
                return;
            }
            ViewState[FIRST_LOAD] = 1;
            DataTable docTemplateTable = GetMortgageDocTemplateTable();
            ucAppPackage.DocTemplateTable = docTemplateTable;
            ucClosingPackage.DocTemplateTable = docTemplateTable;
            ucMiscPackage.DocTemplateTable = docTemplateTable;
            SetBaydocsGridVisibility(CurrentPage.GetMortgage(MortgageProfileID));
        }

        protected void btnPrintPack_Click(object sender, EventArgs e)
        {
            switch (RadTabStripPackage.SelectedIndex)
            {
                case 0:
                    ucAppPackage.StoreFilledPDFStream();
                    break;
                case 1:
                    ucClosingPackage.StoreFilledPDFStream();
                    break;
                case 2:
                    ucMiscPackage.StoreFilledPDFStream();
                    break;
                default:
                    break;
            }
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "CreatePackage", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DocPackage.aspx';</script>");
        }
        protected void btnSendToClosingAgent_Click(object sender, EventArgs e)
        {
            string msg = String.Empty;
            switch (RadTabStripPackage.SelectedIndex)
            {
                case 0:
                    msg = ucAppPackage.SavePackage();
                    break;
                case 1:
                    msg = ucClosingPackage.SavePackage();
                    break;
                case 2:
                    msg = ucMiscPackage.SavePackage();
                    break;
                default:
                    break;
            }

            resTabPackageMessage.Value = msg;
        }
        #endregion
    }
}
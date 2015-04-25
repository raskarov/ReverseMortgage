using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using LoanStar.Common;
using Telerik.WebControls;

namespace LoanStarPortal.Administration.Controls
{
    public partial class EditDocTemplate : AppControl
    {
        #region Fields / Properties
        protected int DocID
        {
            get
            {
                if (ViewState["DocID"] == null)
                    ViewState["DocID"] = 0;
                return Convert.ToInt32(ViewState["DocID"].ToString());
            }
            set
            {
                ViewState["DocID"] = value;
            }
        }
        protected int DocVersionID
        {
            get
            {
                if (ViewState["DocVersionID"] == null)
                    ViewState["DocVersionID"] = 0;
                return Convert.ToInt32(ViewState["DocVersionID"].ToString());
            }
            set
            {
                ViewState["DocVersionID"] = value;
            }
        }
        protected int Step
        {
            get
            {
                if (ViewState["Step"] == null)
                    ViewState["Step"] = 1;
                return Convert.ToInt32(ViewState["Step"].ToString());
            }
            set
            {
                ViewState["Step"] = value;
            }
        }
        #endregion
        
        #region Methods
        protected void InitPage()
        {
            if (Step == 1)
            {
                PanelStep1.Visible = true;
                PanelStep2.Visible = false;
                BindData();
            }
            else if (Step == 2)
            {
                PanelStep1.Visible = false;
                PanelStep2.Visible = true;
                btnSaveStep1.Visible = false;
            }
        }

        protected void BindGrid()
        {
            GridVersion.DataSource = DocTemplate.GetDocTemplateVersionList(DocID);
            GridVersion.DataBind();
        }

        protected void BindData()
        {
            ddlSelectDocGroup.DataSource = CurrentPage.GetDictionary("DocumentGroup");
            ddlSelectDocGroup.DataTextField = "Name";
            ddlSelectDocGroup.DataValueField = "id";
            ddlSelectDocGroup.DataBind();

            if (DocID > 0)
            {
                DocTemplate dc = new DocTemplate(DocID);
                lblHeader.Text = "Edit document template";
                txtTitle.Text = dc.Title;
                ddlSelectDocGroup.SelectedValue = dc.DocGroupID.ToString();
                PanelEdit.Visible = true;
                PanelAddDoc.Visible = false;
                txtVersion.Visible = false;
                btnSaveStep1.Text = "Save";
                BindGrid();
            }
            else
            {
                PanelEdit.Visible = false;
                PanelAddDoc.Visible = true;
                txtVersion.Visible = true;
                btnSaveStep1.Text = "Next";
                lblHeader.Text = "Add new document template";
            }
        }
        #endregion

        #region Wizard step voids
        protected void btnSaveStep1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtTitle.Text))
            {
                lblMessage.Text = "You need to input title of document template.";
                return;
            }
            else if (Convert.ToInt32(ddlSelectDocGroup.SelectedValue) == 0)
            {
                lblMessage.Text = "You need to set group of document template.";
                return;
            }
            else if (DocID <= 0 && !UploadPdfFile.HasFile)
            {
                lblMessage.Text = "Select file to upload.";
                return;
            }

            if (DocID > 0)
            {
                DocTemplate doc = new DocTemplate(DocID);
                doc.Title = txtTitle.Text.Trim();
                doc.DocGroupID = Convert.ToInt32(ddlSelectDocGroup.SelectedValue);
                lblMessage.Text = doc.Update();
            }
            else
            {
                DocTemplateVersion dtv = new DocTemplateVersion();
                dtv.DocTemplate.Title = txtTitle.Text.Trim();
                dtv.DocTemplate.DocGroupID = Convert.ToInt32(ddlSelectDocGroup.SelectedValue);
                dtv.Version = txtVersion.Text.Trim();
                dtv.IsCurrent = true;

                lblMessage.Text = dtv.Update(UploadPdfFile.PostedFile);
                if (!String.IsNullOrEmpty(lblMessage.Text))
                    return;

                DocID = dtv.DocTemplateID;
                DocVersionID = dtv.ID;

                DocFieldMapping1.DocTemplateVersion = new DocTemplateVersion(dtv.ID);
                Step = 2;
                InitPage();
                DocFieldMapping1.BindData();
            }
        }

        protected void btnSaveStep2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWDOCTEMPLATE);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentUser.IsLoanStarAdmin)
            {
                Response.Redirect("~/" + CurrentUser.GetDefaultPage());
            }
            if (!Page.IsPostBack)
            {
                DocID = ((AppPage)Page).GetValueInt(Constants.IDPARAM, DocID);
                InitPage();
            }
            else if (Step == 2)
                DocFieldMapping1.DocTemplateVersion = new DocTemplateVersion(DocVersionID);
        }

        #region Event handlers
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Step == 1) 
                Response.Redirect("~/" + Constants.ADMINPAGENAME + "?" + Constants.CONTROLPARAM + "=" + Constants.VIEWDOCTEMPLATE);
            else if (Step == 2)
            {
                Step = 1;
                DocFieldMapping1.DocTemplateVersion = null;
            }
            InitPage();
        }

        protected void GridVersion_ItemCommand(object source, GridCommandEventArgs e)
        {
            lblMessage.Text = String.Empty;

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                if (GridVersion.EditItems.Count > 0)
                {
                    GridVersion.MasterTableView.ClearEditItems();
                }
            }
            if (e.CommandName == RadGrid.EditCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
            }

            if(e.CommandName == RadGrid.UpdateCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Canceled = true;

                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                DocTemplateVersion dtv = new DocTemplateVersion(id);
                dtv.IsCurrent = true;

                TextBox textBox = e.Item.FindControl("txtVersionG") as TextBox;
                if (textBox != null) dtv.Version = textBox.Text.Trim();

                FileUpload fileUpload = (FileUpload)e.Item.FindControl("UploadPdfFileG");
                if (fileUpload == null || !fileUpload.HasFile)
                    lblMessage.Text = dtv.Update();
                else
                    lblMessage.Text = dtv.Update(fileUpload.PostedFile);

                GridVersion.MasterTableView.ClearEditItems();
                BindGrid();
            }
            else if (e.CommandName == RadGrid.PerformInsertCommandName)
            {
                e.Item.OwnerTableView.IsItemInserted = false;
                e.Canceled = true;

                FileUpload fileUpload = (FileUpload)e.Item.FindControl("UploadPdfFileG");
                if (fileUpload == null || !fileUpload.HasFile)
                    lblMessage.Text = "Select file to upload.";
                else
                {
                    DocTemplateVersion dtv = new DocTemplateVersion();
                    dtv.IsCurrent = true;

                    TextBox textBox = e.Item.FindControl("txtVersionG") as TextBox;
                    if (textBox != null) dtv.Version = textBox.Text.Trim();

                    dtv.DocTemplateID = DocID;

                    lblMessage.Text = dtv.Update(fileUpload.PostedFile);
                }

                GridVersion.MasterTableView.ClearEditItems();
                BindGrid();
            }
            else if (e.CommandName == "MapFilelds")
            {
                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                DocVersionID = id;

                DocTemplateVersion dtv = new DocTemplateVersion(id);
                DocFieldMapping1.DocTemplateVersion = dtv;
                Step = 2;
                InitPage();
                DocFieldMapping1.BindData();
            }
            else if (e.CommandName == "Current")
            {
                int id = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString());
                DocTemplateVersion dtv = new DocTemplateVersion(id);
                dtv.SetCurrent();
                BindGrid();
            }
            else// if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.InitInsertCommandName)
            {
                BindGrid();
            }
        }

        protected void GridVersion_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item.ItemType == GridItemType.Item) || (e.Item.ItemType == GridItemType.AlternatingItem))
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                if (row != null)
                {
                    CheckBox checkBox = e.Item.FindControl("IsCurrent") as CheckBox;
                    if (checkBox != null)
                    {
                        checkBox.Checked = Convert.ToBoolean(row["IsCurrent"].ToString());
                        checkBox.Attributes.Add("onclick", "javascript:CheckAllDeny(this);");
                    }
                    HyperLink hlnk = (HyperLink)e.Item.FindControl("lnkFile");
                    if(hlnk!=null)
                    {
                        string relativePath = Path.Combine(Constants.STORAGEFOLDER + Constants.TEMPLATESFOLDER, row["filename"].ToString());
                        string pathToFile = Server.MapPath(relativePath);
                        if (File.Exists(pathToFile))
                        {
                            hlnk.NavigateUrl = relativePath;
                        }
                    }
                }
            }
        }

        protected void IsCurrent_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridDataItem grdItem in GridVersion.Items)
            {

                CheckBox chk = (CheckBox)grdItem.FindControl("IsCurrent");
                //In the radgrid's second cell we are binding the userid and delete   
                //the row using this unique id   
                int id = Convert.ToInt32(grdItem.Cells[2].Text);
                if (chk.Checked)
                {
                    DocTemplateVersion dtv = new DocTemplateVersion(id);
                    dtv.SetCurrent();
                }
            }
            BindGrid();
        }   
 
       #endregion
    }
}
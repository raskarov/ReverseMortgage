using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LoanStar.Common;

using Telerik.WebControls;

namespace LoanStarPortal.Controls
{
    public partial class Package : AppControl
    {
        #region Private fields
        private DataTable docTemplateTable;
        private string filter = String.Empty;
        private string packageFileName = String.Empty;
        private string packageCBOnClickScript = String.Empty;

        private const string FIRST_LOAD = "PackFirstLoad";
        #endregion

        #region Properties
        public DataTable DocTemplateTable
        {
            get
            {
                return docTemplateTable;
            }
            set
            {
                docTemplateTable = value;
            }
        }

        public string PackageCBOnClickScript
        {
            get
            {
                return packageCBOnClickScript;
            }
            set
            {
                packageCBOnClickScript = value ?? String.Empty;
            }
        }

        public string PackageFileName
        {
            get
            {
                return packageFileName;
            }
            set
            {
                packageFileName = value ?? String.Empty;
            }
        }

        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value ?? String.Empty;
            }
        }

        protected MemoryStream FilledPDFStream
        {
            get
            {
                string tempPDFFile = Session["FilledPDFStream"].ToString();
                if (String.IsNullOrEmpty(tempPDFFile) || !File.Exists(tempPDFFile))
                    return null;
                
                FileStream tempPDFFileStream = File.OpenRead(tempPDFFile);
                byte[] tempPDFFileBuffer = new byte[tempPDFFileStream.Length];
                tempPDFFileStream.Read(tempPDFFileBuffer, 0, tempPDFFileBuffer.Length);
                tempPDFFileStream.Close();
                File.Delete(tempPDFFile);

                MemoryStream tempPDFMemoryStream = new MemoryStream(tempPDFFileBuffer);
                return tempPDFMemoryStream;
            }
            set
            {
                MemoryStream tempPDFMemoryStream = value;
                if (tempPDFMemoryStream != null)
                {
                    string tempPDFFile = Server.MapPath(Path.Combine(Constants.STORAGEFOLDER, Guid.NewGuid() + "_tempDocStorage.pdf"));
                    FileStream tempPDFFileStream = new FileStream(tempPDFFile, FileMode.Create);

                    tempPDFMemoryStream.WriteTo(tempPDFFileStream);

                    tempPDFMemoryStream.Close();
                    tempPDFFileStream.Close();

                    Session["FilledPDFStream"] = tempPDFFile;
                }
                else
                    Session["FilledPDFStream"] = String.Empty;
            }
        }

        protected string PDFFileName
        {
            get
            {
                if (Session["PDFFileName"] == null)
                    Session["PDFFileName"] = String.Empty;
                return Convert.ToString(Session["PDFFileName"]);
            }
            set
            {
                Session["PDFFileName"] = value;
            }
        }

        protected string PDFGenerationError
        {
            get
            {
                if (Session["PDFGenerationError"] == null)
                    Session["PDFGenerationError"] = String.Empty;
                return Convert.ToString(Session["PDFGenerationError"]);
            }
            set
            {
                Session["PDFGenerationError"] = value;
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
        public void BindData()
        {
            foreach (RadPanelItem panelItem in RadPBDocTemplates.Items)
            {
                DataView docTemplateView = null;
                if (docTemplateTable != null && Filter.Trim().Length > 0)
                {
                    string groupID = panelItem.Value;
                    docTemplateView = new DataView(docTemplateTable);
                    docTemplateView.RowFilter = Filter + " AND GroupID = " + groupID;
                    docTemplateView.Sort = "DTTitle";
                }

                panelItem.Visible = docTemplateView != null && docTemplateView.Count > 0;
                if (panelItem.Visible)
                    foreach (Control insideCtrl in panelItem.Items[0].Controls)
                    {
                        if (insideCtrl.GetType() == typeof(Repeater))
                        {
                            Repeater curRepeater = (Repeater)insideCtrl;
                            curRepeater.DataSource = docTemplateView;
                            curRepeater.DataBind();
                        }
                        else if (insideCtrl.GetType() == typeof(Label))
                        {
                            Label curLabel = (Label)insideCtrl;
                            curLabel.Visible = false;
                        }
                    }
            }
        }

        private static string GetDocTemplatesIDXml(int dtID)
        {
            XmlDocument docXml = new XmlDocument();
            XmlNode rootNode = docXml.CreateElement("Root");

            XmlNode node = docXml.CreateElement("item");
            XmlAttribute attr = docXml.CreateAttribute("id");
            attr.Value = dtID.ToString();
            node.Attributes.Append(attr);
            rootNode.AppendChild(node);

            docXml.AppendChild(rootNode);
            return docXml.OuterXml;
        }

        private string GetDocTemplatesIDsXml()
        {
            XmlDocument docXml = new XmlDocument();
            XmlNode rootNode = docXml.CreateElement("Root");

            foreach (RadPanelItem panelItem in RadPBDocTemplates.Items)
                foreach (Control insideCtrl in panelItem.Items[0].Controls)
                    if (insideCtrl.GetType() == typeof(Repeater))
                    {
                        Repeater curRepeater = (Repeater)insideCtrl;
                        foreach (RepeaterItem repeaterItem in curRepeater.Items)
                        {
                            bool docSelected = ((CheckBox) repeaterItem.Controls[1]).Checked;
                            if (docSelected)
                            {
                                string dtID = ((HiddenField) repeaterItem.Controls[3]).Value;

                                XmlNode node = docXml.CreateElement("item");
                                XmlAttribute attr = docXml.CreateAttribute("id");
                                attr.Value = dtID;
                                node.Attributes.Append(attr);
                                rootNode.AppendChild(node);
                            }
                        }

                        break;
                    }

            docXml.AppendChild(rootNode);
            return rootNode.ChildNodes.Count > 0 ? docXml.OuterXml : String.Empty;
        }

        public void StoreFilledPDFStream()
        {
            string dtIDXml = GetDocTemplatesIDsXml();
            MemoryStream dtResMemoryStream = GetResPdfStream(dtIDXml);

            PDFFileName = PackageFileName;
            FilledPDFStream = dtResMemoryStream;
        }

        private MemoryStream GetResPdfStream(string dtIDXml)
        {
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            string errMsg;
            MemoryStream resPdfStream = mp.GetResPdfStream(dtIDXml, out errMsg);
            PDFGenerationError = errMsg;

            return resPdfStream;
        }

        public string SavePackage()
        {
            string packageName = PackageFileName.Replace(".pdf", null);
            string packFileName = PackageFileName;
            MortgageProfile mp = CurrentPage.GetMortgage(MortgageProfileID);
            LoanStar.Common.Package package = new LoanStar.Common.Package(packageName, packFileName, mp);

            string dtIDXml = GetDocTemplatesIDsXml();
            AppUser userFrom = CurrentPage.CurrentUser;
            string url = Page.Request.Url.OriginalString.Replace(Page.Request.Url.PathAndQuery, null);
            url += "/DocPackage.aspx";
            string resMessage = package.SendPackage(dtIDXml, userFrom, url);
            if (!String.IsNullOrEmpty(resMessage))
                return resMessage;

            return String.Format("The closing package has been successfully {0}placed in a secure web location and {0}an email has been sent to the closing agent.", Environment.NewLine);
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState[FIRST_LOAD] != null)
                return;
            ViewState[FIRST_LOAD] = 1;

            BindData();
        }

        protected void lbtnDTTitle_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "DocClicked")
                return;

            string dtIDXml = GetDocTemplatesIDXml( Convert.ToInt32(e.CommandArgument) );
            MemoryStream dtResMemoryStream = GetResPdfStream(dtIDXml);
            string pdfFileName = ((LinkButton) sender).Text + ".pdf";

            FilledPDFStream = dtResMemoryStream;
            PDFFileName = pdfFileName;
            CurrentPage.ClientScript.RegisterStartupScript(GetType(), "CreatePackage", "<script language=\"javascript\" type=\"text/javascript\">location.href = 'DocPackage.aspx';</script>");
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
            else if (PackageCBOnClickScript.Trim().Length == 0)
                return;

            CheckBox cbSelected = (e.Item.Controls[1] as CheckBox);
            if (cbSelected != null) cbSelected.Attributes.Add("onclick", PackageCBOnClickScript);
        }
        #endregion
    }
}
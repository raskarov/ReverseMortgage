using System;
using System.IO;
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

namespace LoanStarPortal.Controls
{
    public partial class EmailAttachUpload : AppControl
    {
        #region Private fields
        private Mailer mailer;
        #endregion

        #region Properties
        public Mailer Mailer
        {
            get
            {
                return mailer;
            }
            set
            {
                mailer = value;
            }
        }
        #endregion

        #region Methods
        public void BindData()
        {
            rpEmailAttachments.DataSource = mailer.AttachesList;
            rpEmailAttachments.DataBind();

            divUploadEmailAttachments.Visible = Mailer.ID <= 0 || Mailer.MailStatus == MailStatus.Draft;
            divEmailAttachments.Visible = /*Mailer.ID > 0 && */Mailer.AttachesCount > 0;
        }

        public void UpdateAttachments()
        {
            foreach (RepeaterItem rpItem in rpEmailAttachments.Items)
            {
                HiddenField hfAttachID = (HiddenField)rpItem.Controls[1].Controls[1].Controls[1].Controls[3];
                int atttachId = Convert.ToInt32(hfAttachID.Value);
                if (atttachId < 0)
                    Mailer.GetAttachmentByID(atttachId * (-1)).ID = atttachId;
                else if (atttachId == 0)
                {
                    bool attachIsInline = Convert.ToBoolean((rpItem.Controls[1].Controls[1].Controls[1].Controls[5] as HiddenField).Value);
                    string attachContentID = (rpItem.Controls[1].Controls[1].Controls[1].Controls[7] as HiddenField).Value;
                    string attachName = (rpItem.Controls[1].Controls[1].Controls[1].Controls[9] as HiddenField).Value;
                    string attachFileName = (rpItem.Controls[1].Controls[1].Controls[1].Controls[11] as HiddenField).Value;
                    string attachContentType = (rpItem.Controls[1].Controls[1].Controls[1].Controls[13] as HiddenField).Value;
                    string atachTransferEncoding = (rpItem.Controls[1].Controls[1].Controls[1].Controls[15] as HiddenField).Value;
                    string attachNamePageCode = (rpItem.Controls[1].Controls[1].Controls[1].Controls[17] as HiddenField).Value;
                    mailer.AddAttach(attachFileName,
                                        attachName,
                                        attachContentType,
                                        attachContentID,
                                        atachTransferEncoding,
                                        attachNamePageCode,
                                        attachIsInline);
                }
            }

            foreach (string postedFileKey in Page.Request.Files.AllKeys)
            {
                HttpPostedFile postedFile = Page.Request.Files[postedFileKey];
                if (postedFile == null || String.IsNullOrEmpty(postedFile.FileName))
                    continue;

                string attachName = new FileInfo(postedFile.FileName).Name;
                string attachContentType = postedFile.ContentType;
                mailer.AddAttach(postedFile.InputStream, attachName, attachContentType);
            }
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (RepeaterItem rpItem in rpEmailAttachments.Items)
            {
                HiddenField hfAttachID = (HiddenField)rpItem.Controls[1].Controls[1].Controls[1].Controls[3];
                object objAttachID = this.Page.Request.Form[hfAttachID.ClientID.Replace('_', '$')];
                if (Convert.ToInt32(objAttachID) < 0)
                    (rpItem.Controls[1] as HtmlTableRow).Attributes["style"] = "display:none;";
            }
        }

        protected void rpEmailAttachments_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            //LinkButton lbRemoveAttach = 
            //put in asp.net linkbutton here instead of a 
            HyperLink hlRemoveAttach = (HyperLink)e.Item.Controls[1].Controls[1].Controls[1].Controls[1];
            hlRemoveAttach.NavigateUrl = String.Format("javascript:ClickRemoveAttach('{0}')", e.Item.Controls[1].ClientID);

            if (Mailer.MailStatus == MailStatus.Draft)
                (e.Item.Controls[1].Controls[3] as HtmlTableRow).Attributes["style"] = "display:none;";
            else
                (e.Item.Controls[1].Controls[1] as HtmlTableRow).Attributes["style"] = "display:none;";

            bool attachIsInline = Convert.ToBoolean((e.Item.Controls[1].Controls[1].Controls[1].Controls[5] as HiddenField).Value);
            if (attachIsInline)
                (e.Item.Controls[1] as Panel).Visible = false;
        }

        protected void lbtnSaveAttach_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "SaveAttach")
                return;
            int attachID = Convert.ToInt32(e.CommandArgument);

            try
            {
                MailerAttachment attach = MailerAttachment.GetAttachmentByID(attachID);
                if (attach == null)
                    throw new Exception(String.Format("Mail attachment with ID={0} is not found", attachID));

                base.DownloadGenerationError = String.Empty;
                base.DownloadStream = attach.ContentStream;
                base.DownloadContentType = attach.ContentType.MediaType;
                base.DownloadFileName = attach.Name;
            }
            catch (Exception ex)
            {
                base.DownloadGenerationError = ex.Message;
                base.DownloadStream = null;
                base.DownloadContentType = String.Empty;
                base.DownloadFileName = (sender as LinkButton).Text;
            }

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">WinOpen('DownLoad.aspx');</script>");
        }
        #endregion
    }
}
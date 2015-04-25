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

namespace LoanStarPortal
{
    public partial class DownLoad : System.Web.UI.Page
    {
        #region Download properties
        public Stream DownloadStream
        {
            get
            {
                return (Stream)Session["DownloadStream"];
            }
            set
            {
                Session["DownloadStream"] = value;
            }
        }

        public string DownloadGenerationError
        {
            get
            {
                 string downloadGenerationError = Convert.ToString(Session["DownloadGenerationError"]);
                 return String.IsNullOrEmpty(downloadGenerationError) ? String.Empty : downloadGenerationError;
            }
            set
            {
                Session["DownloadGenerationError"] = value;
            }
        }

        public string DownloadFileName
        {
            get
            {
                string downloadFileName = Convert.ToString(Session["DownloadFileName"]);
                return String.IsNullOrEmpty(downloadFileName) ? String.Empty : downloadFileName;
            }
            set
            {
                Session["DownloadFileName"] = value;
            }
        }

        public string DownloadContentType
        {
            get
            {
                string downloadContentType = Convert.ToString(Session["DownloadContentType"]);
                return String.IsNullOrEmpty(downloadContentType) ? "application/octet-stream" : downloadContentType;
            }
            set
            {
                Session["DownloadContentType"] = value;
            }
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            Stream downloadStream = DownloadStream;
            try
            {
                if (downloadStream != null && downloadStream.Length > 0)
                {
                    int buffLength = (int)downloadStream.Length;
                    byte[] buff = new byte[buffLength];
                    downloadStream.Read(buff, 0, buffLength);

                    Response.Clear();
                    Response.Expires = -1000;
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + DownloadFileName);
                    Response.AddHeader("Content-Length", buffLength.ToString());
                    Response.ContentType = DownloadContentType;
                    Response.BinaryWrite(buff);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    Response.Clear();
                    Response.Expires = -1000;
                    Response.Write(String.Format("Can't download file {0}<br/>Error: {1}", DownloadFileName, DownloadGenerationError));
                    Response.Flush();
                }
            }
            finally
            {
                DownloadStream = null;
                DownloadContentType = String.Empty;
                DownloadFileName = String.Empty;
                DownloadGenerationError = String.Empty;

                if (downloadStream != null)
                    downloadStream.Close();
            }
        }
        #endregion
    }
}

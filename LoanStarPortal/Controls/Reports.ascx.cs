using System;
using System.Web.UI.WebControls;
using LoanStar.Common;
using LoanStarPortal.ReportWrapers;
using CrystalDecisions.Shared;

namespace LoanStarPortal.Controls
{
    public partial class Reports : AppControl
    {
        #region Properties
        private int CompanyID
        {
            get
            {
                return ((AppPage)this.Page).CurrentUser.CompanyId;
            }
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lbtnPipeLineReport_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "PipeLine")
                return;

            PipeLineWraper reportWraper = new PipeLineWraper(CompanyID);
            reportWraper.Load();

            base.DownloadStream = reportWraper.ExportToStream(ExportFormatType.PortableDocFormat);
            base.DownloadContentType = "application/pdf";
            base.DownloadFileName = reportWraper.ResourceName.Replace(".rpt", ".pdf");

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">WinOpen('DownLoad.aspx');</script>");
        }
        #endregion

        #region Old code
/*        protected void lbtnPipeLineReport_Click(object sender, EventArgs e)
        {
            CrystalReport1 report = new CrystalReport1();
            report.InitReport += new EventHandler(report_InitReport);
            report.Load();

            base.DownloadStream = report.ExportToStream(ExportFormatType.PortableDocFormat);
            base.DownloadContentType = "application/pdf";
            base.DownloadFileName = report.ResourceName.Replace(".rpt", ".pdf");

            CurrentPage.ClientScript.RegisterStartupScript(this.GetType(), "DownloadFile", "<script language=\"javascript\" type=\"text/javascript\">WinOpen('DownLoad.aspx');</script>");
        }

        private void report_InitReport(object sender, System.EventArgs e)
        {
            ArrayList sourceArr = new ArrayList();
            Class1 obj = new Class1();
            obj.X = 10000;
            sourceArr.Add(obj);

            CrystalReport1 report = (CrystalReport1)sender;
            report.SetDataSource(sourceArr);
        }*/
        #endregion
    }
}
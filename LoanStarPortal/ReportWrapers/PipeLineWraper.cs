using System;
using System.Data;
using System.IO;
using BossDev.CommonUtils;
using CrystalDecisions.Shared;
using LoanStar.Common;
using LoanStarReports;

namespace LoanStarPortal.ReportWrapers
{
    class PipeLineWraper
    {
        #region Private fields
        private int companyID = -1;
//        private PipeLine report = new PipeLine();
        private PipeLineReport report = new PipeLineReport();
        #endregion

        #region Constructors
        public PipeLineWraper(int compID)
        {
            companyID = compID;
            report.InitReport += new EventHandler(report_InitReport);
        }
        #endregion

        #region Event handlers
        protected virtual void report_InitReport(object sender, EventArgs e)
        {
            //DataTable reportTable = db.GetDataTable("GetReportMortgageUserList", companyID);
            //reportTable.TableName = "MortgageUser";
            DataTable reportTable = db.GetDataTable("RptPipeLine", companyID);
            reportTable.TableName = "PipeLine";
            report.SetDataSource(reportTable);
        }
        #endregion

        #region Properties
        public string ResourceName
        {
            get
            {
                return report.ResourceName;
            }
        }
        #endregion

        #region Methods
        public void Load()
        {
            report.Load();
        }

        public Stream ExportToStream(ExportFormatType exportType)
        {
            return report.ExportToStream(exportType);
        }
        #endregion

        #region Static fields
        private static DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion
    }
}

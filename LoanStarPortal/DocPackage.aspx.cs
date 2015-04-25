using System;
using System.IO;
using System.Configuration;
using System.Web;

using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class DocPackage : AppPage
    {
        #region Private fields
        private Package package = null;
        #endregion

        #region Properties
        protected MemoryStream FilledPDFStream
        {
            get
            {
                string tempPDFFile = package == null ? Session["FilledPDFStream"].ToString() : package.PackageFilePath;
                if (String.IsNullOrEmpty(tempPDFFile) || !File.Exists(tempPDFFile))
                    return null;

                FileStream tempPDFFileStream = File.OpenRead(tempPDFFile);
                byte[] tempPDFFileBuffer = new byte[tempPDFFileStream.Length];
                tempPDFFileStream.Read(tempPDFFileBuffer, 0, tempPDFFileBuffer.Length);
                tempPDFFileStream.Close();
                if (package == null)
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
                return package == null ? Convert.ToString(Session["PDFFileName"]) : package.PackFileName;
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
        #endregion

        #region Private methods
        private void CheckCurrentVendor()
        {
            object objPackageUI = Page.Request["PackageUI"];
            if (objPackageUI == null)
                return;
            
            if (CurrentVendor == null)
                Response.Redirect(String.Format("~/LoginVendor.aspx?ReturnUrl=/DocPackage.aspx{0}", Page.Request.Url.Query));

            string strPackageUI = Convert.ToString(objPackageUI);
            Package foundPackage = new Package(new Guid(strPackageUI));
            if (foundPackage.ID <= 0)
                PDFGenerationError = String.Format("Package with UI={0} was not found in database", strPackageUI);
            else if (CurrentVendor.ID != foundPackage.VendorID)
                PDFGenerationError = "You have not access rights to view this package";
            else
                package = foundPackage;
        }
        private void OutputPdf(string fileName)
        {
            string fullName = Path.Combine(Server.MapPath(Constants.STORAGEFOLDER), fileName);
            FileInfo fi = new FileInfo(fullName);
            Response.Clear();
            Response.Expires = -1000;
            if(fi.Exists)
            {
                try
                {
                    FileStream fs = new FileStream(fullName, FileMode.Open, FileAccess.Read);
                    BinaryReader rd = new BinaryReader(fs);
                    byte[] data = rd.ReadBytes((int)fi.Length);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.AddHeader("Content-Length", data.Length.ToString());
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(data);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch(Exception ex)
                {
                    WriteToResponse(ex.Message);
                    Response.Flush();
                }
            }
            else
            {
                WriteToResponse(String.Format("File {0} not found", fileName));
                Response.Flush();
            }
        }
        private void WriteToResponse(string message)
        {
            Response.Write(message);
        }
        #endregion

        #region Event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            string param_ctl = GetValue("file","");
            if(!String.IsNullOrEmpty(param_ctl))
            {
                OutputPdf(param_ctl);
                return;
            }
            CheckCurrentVendor();

            Logger log = null;
            if (Convert.ToInt32(ConfigurationManager.AppSettings["LogDTVersions"]) != 0)
                log = new Logger();

            if (log != null)
            {
                log.WriteLine("*********************************************************************************************");
                log.WriteLine(String.Empty);
                log.WriteLine("Download document package");
            }

            MemoryStream dtResMemoryStream = null;
            try
            {
                dtResMemoryStream = FilledPDFStream;
                if (dtResMemoryStream != null)
                {
                    byte[] dtBuffer = dtResMemoryStream.ToArray();
                    string pdfFileName = PDFFileName.Trim().Length == 0 ? "FileName.pdf" : PDFFileName;

                    if (log != null)
                    {
                        log.WriteLine("\tStart download");
                        log.WriteLine(String.Format("\tPDFFileName={0}; BufferLength={1}", pdfFileName, dtBuffer.Length));
                    }

                    Response.Clear();
                    Response.Expires = -1000;
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfFileName);
                    Response.AddHeader("Content-Length", dtBuffer.Length.ToString());
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(dtBuffer);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                    if (log != null)
                        log.WriteLine("\tFinish download");

                }
                else
                {
                    if (log != null)
                        log.WriteLine("Download is not executed");

                    Response.Clear();
                    Response.Expires = -1000;
                    Response.Write(String.Format("Document has not been created<br/>Error: {0}", PDFGenerationError));
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                if (log != null)
                    log.WriteException(ex);
                throw;
            }
            finally
            {
                FilledPDFStream = null;
                PDFFileName = String.Empty;
                PDFGenerationError = String.Empty;

                if (dtResMemoryStream != null)
                    dtResMemoryStream.Close();
            }
        }
        #endregion
    }
}

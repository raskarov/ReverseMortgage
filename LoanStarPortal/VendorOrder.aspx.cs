using System;
using System.IO;
using System.Web;
using LoanStar.Common;

namespace LoanStarPortal
{
    public partial class VendorOrder : AppPage
    {
        private string param_ctl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            param_ctl = GetValue("get", "");
            bool isRequestValid = !String.IsNullOrEmpty(param_ctl);
            if (isRequestValid)
            {
                isRequestValid = CurrentVendor != null;
                if (isRequestValid)
                {
                    isRequestValid = CheckOrderNumber(param_ctl);
                }
            }
            if(!isRequestValid)
            {
                string url = Constants.VENDORLOGINPAGE + "?ReturnUrl=/VendorOrder.aspx?get=" + param_ctl;
                Response.Redirect(url);

            }
            else
            {
                lbDownLoad.CommandArgument = param_ctl;
            }
        }
        private void SendFile(string fileName)
        {
            string storagePath = Path.Combine(Server.MapPath(Constants.STORAGEFOLDER), VendorFeeOrder.VENDORORDERFOLDER);
            string fullName = Path.Combine(storagePath, fileName);
            FileInfo fi = new FileInfo(fullName);
            Response.Clear();
            Response.Expires = -1000;
            if (fi.Exists)
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
                catch (Exception ex)
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
        private bool CheckOrderNumber(string orderguid)
        {
            return VendorFeeOrder.CheckOrder(CurrentVendor.ID, orderguid);
        }
        private void WriteToResponse(string message)
        {
            Response.Write(message);
        }

        protected void lbDownLoad_Click(object sender, EventArgs e)
        {
            SendFile(lbDownLoad.CommandArgument + ".pdf");
        }
    }
}

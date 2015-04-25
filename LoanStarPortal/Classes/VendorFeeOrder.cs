using System;
using System.Xml;
using System.Data;
using System.IO;
using System.Net.Mail;

namespace LoanStar.Common
{
    public class VendorFeeOrder : BaseObject
    {
        #region constants
        
        #region error messages
        private const string ERRFEENOTSERVED = "This fee don't use order feature";
        private const string ERREMPTYVENDOREMAIL = "Vendor's email is empty";
        private const string ERREMAILTEMPLATENOTFOUND = "Email's template not found";
        #endregion

        private const string GETTEMPLATEFILEID = "GetVendorOrderTemplateIdByFeeId";
        private const string GETFEETYPEBYID = "GetFeeTypeById";
        private const string SAVEORDERINFO = "SaveOrderInfo";
        private const string CHECKVENDORORDER = "CheckVendorOrder";
        public const string VENDORORDERFOLDER = @"VWD\Orders";
        private const string VENDORORDERPAGE = "VendorOrder.aspx";
        public const string VENDORORDERTEMPLATEFOLDER = @"VWD\Templates";
        public const string SUBJECT = "New Order from RM LOS";
        public const string EMAILTEMPLATEFILENAME = "emailordertemplate.txt";

        #endregion

        #region fields
        private readonly MortgageProfile mp;
        private readonly VendorGlobal vendor;
        private readonly int feeTypeId;
        private readonly AppUser creator;
        private string errorMessage = String.Empty;
        private readonly string storageBasePath;
        private readonly int templateFileId;
        private string documentUrl;
        private string feeType;
        #endregion

        #region properpties
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        #endregion

        #region constaructor
        public VendorFeeOrder(int vendorId, int feeTypeId_, AppUser user , string storageBasePath_, MortgageProfile mp_)
        {
            vendor = new VendorGlobal(vendorId);
            storageBasePath = storageBasePath_;
            creator = user;
            feeTypeId = feeTypeId_;
            mp = mp_;
            templateFileId = GetTemplateId();
            feeType = GetFeeTypeById();
        }
        #endregion

        #region methods
        #region private
        private string GetFeeTypeById()
        {
            return db.ExecuteScalarString(GETFEETYPEBYID, feeTypeId);
        }

        private string GetIDXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateElement("Root");
            XmlNode node = doc.CreateElement("item");
            XmlAttribute attr = doc.CreateAttribute("id");
            attr.Value = templateFileId.ToString();
            node.Attributes.Append(attr);
            rootNode.AppendChild(node);
            doc.AppendChild(rootNode);
            return doc.OuterXml;
        }
        private int GetTemplateId()
        {
            int res = 0;
            DataView dv = db.GetDataView(GETTEMPLATEFILEID, feeTypeId);
            if(dv.Count==1)
            {
                res = int.Parse(dv[0]["id"].ToString());
            }
            else
            {
                errorMessage = ERRFEENOTSERVED;
            }
            return res;
        }
        private bool PrepairFile(out string fileName)
        {
            bool res = false;
            fileName = String.Empty;
            if(templateFileId>0)
            {
                MemoryStream outputDocument = null;
                try
                {
                    string errMsg;
                    outputDocument = mp.GetResPdfStream(GetIDXml(), out errMsg);
                    if(!String.IsNullOrEmpty(errMsg))
                    {
                        errorMessage = errMsg;
                    }
                    else
                    {
                        fileName = StoreToFileSystem(outputDocument);
                        res = true;
                    }
                }
                catch(Exception ex)
                {
                    errorMessage = ex.Message;
                }
                finally
                {
                    if(outputDocument!=null) outputDocument.Close();
                }
            }
            return res;
        }
        private string StoreToFileSystem(MemoryStream data)
        {
            string res = Guid.NewGuid() + ".pdf";
            string storagePath = Path.Combine(storageBasePath, VENDORORDERFOLDER);
            string file = Path.Combine(storagePath, res);
            FileStream fs=null;
            try
            {
                fs = new FileStream(file, FileMode.Create);
                data.WriteTo(fs);
            }
            finally
            {
                if(fs!=null) fs.Close();
            }
            return res;
        }
        private bool SendEmail()
        {
            bool res = false;
            MailMessage msg = GetMessage();
            if(msg!=null)
            {
                string errMessage;
                res = Utils.SendEmail(msg, AppSettings.SmtpServerHost, AppSettings.SmtpServerPort, out errMessage);
                if(!res)
                {
                    errorMessage = errMessage;
                }
            }
            return res;
        }
        private MailMessage GetMessage()
        {
            MailMessage msg = new MailMessage();
            string vendorEmail = vendor.CompanyEmail;
            if (String.IsNullOrEmpty(vendorEmail))
            {
                errorMessage = ERREMPTYVENDOREMAIL;
                return null;
            }
            msg.From = new MailAddress(AppSettings.AdminEmail);
            msg.To.Add(new MailAddress(vendorEmail));
            string userEmail = creator.PrimaryEmail;
            if(!String.IsNullOrEmpty(userEmail))
            {
                msg.CC.Add(new MailAddress(userEmail));
            }
            if (!String.IsNullOrEmpty(AppSettings.AdminEmail))
            {
                msg.CC.Add(new MailAddress(AppSettings.AdminEmail));
            }
            msg.Subject = SUBJECT;
            string body = GetBody();
            if(String.IsNullOrEmpty(body))
            {
                return null;
            }
            msg.Body = body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            return msg;
        }
        private string GetBody()
        {
            string res = String.Empty;
            string storagePath = Path.Combine(storageBasePath, VENDORORDERTEMPLATEFOLDER);
            string file = Path.Combine(storagePath, EMAILTEMPLATEFILENAME);
            if(!File.Exists(file))
            {
                errorMessage = ERREMAILTEMPLATENOTFOUND;
            }
            else
            {
                StreamReader sr=null;
                try
                {
                    sr = new StreamReader(file,System.Text.Encoding.Default);
                    res = ParseBody(sr.ReadToEnd());
                }
                catch(Exception ex)
                {
                    errorMessage = ex.Message;
                }
                finally
                {
                    if(sr!=null) sr.Close();
                }
            }
            return res;
        }
        private string ParseBody(string data)
        {
            return data.Replace("<%VendorName%>", vendor.Name).Replace("<%DocumentURL%>", documentUrl).Replace("<%FeeType%>", feeType);
        }
        private void SaveOrderInfo(string orderguid)
        {
            db.ExecuteScalarInt(SAVEORDERINFO, orderguid, vendor.ID);
        }

        #endregion

        #region public
        public bool View(out string fileName)
        {
            return PrepairFile(out fileName);
        }
        public bool Send(string url)
        {
            string fileName;
            bool res  = PrepairFile(out fileName);
            if(res)
            {
                string orderguid = fileName.Replace(".pdf", "");
                documentUrl = url+VENDORORDERPAGE+"?get="+orderguid;
                res = SendEmail();
                if(res)
                {
                    SaveOrderInfo(orderguid);
                }
            }
            return res;
        }
        public static bool CheckOrder(int vendorId, string orderguid)
        {
            return db.ExecuteScalarInt(CHECKVENDORORDER, orderguid, vendorId)==1;
        }

        #endregion

        #endregion
    }
}

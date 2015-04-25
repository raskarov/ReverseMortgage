using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Text;
using System.Configuration;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public class Package : BaseObject
    {
        #region Private fields
        private string storeFileName = String.Empty;
        private string packName = String.Empty;
        private string packFileName = String.Empty;
        DateTime uploadDate = DateTime.Now;
        private Guid ui;

        private MortgageProfile mortgage = new MortgageProfile();
        #endregion

        #region Properties
        public int MortgageID
        {
            get
            {
                return mortgage.ID;
            }
            set
            {
                mortgage.ID = value;
            }
        }

        public int VendorID
        {
            get
            {
                return mortgage.MortgageInfo.ClosingAgentId;
            }
            set
            {
                mortgage.MortgageInfo.ClosingAgentId = value;
            }
        }

        public DateTime UploadDate
        {
            get
            {
                return uploadDate;
            }
            set
            {
                uploadDate = value;
            }
        }

        public string PackName
        {
            get
            {
                return packName;
            }
            set
            {
                packName = String.IsNullOrEmpty(value) ? String.Empty : value;
            }
        }

        public string PackFileName
        {
            get
            {
                return packFileName;
            }
            set
            {
                packFileName = String.IsNullOrEmpty(value) ? String.Empty : value;
            }
        }

        public string PackageFilePath
        {
            get
            {
                return Path.Combine(Package.PackageFolder, storeFileName);
            }
        }

        private string Xml
        {
            get
            {
                XmlDocument docXml = new XmlDocument();
                XmlNode rootNode = docXml.CreateElement("Root");
                XmlNode node = docXml.CreateElement("item");

                XmlAttribute attr = docXml.CreateAttribute("MortgageID");
                attr.Value = MortgageID.ToString();
                node.Attributes.Append(attr);

                attr = docXml.CreateAttribute("FileName");
                attr.Value = storeFileName;
                node.Attributes.Append(attr);

                attr = docXml.CreateAttribute("Title");
                attr.Value = packFileName;
                node.Attributes.Append(attr);

                attr = docXml.CreateAttribute("PackageName");
                attr.Value = packName;
                node.Attributes.Append(attr);

                attr = docXml.CreateAttribute("UploadDate");
                attr.Value = uploadDate.ToString();
                node.Attributes.Append(attr);

                attr = docXml.CreateAttribute("UI");
                attr.Value = ui.ToString();
                node.Attributes.Append(attr);

                rootNode.AppendChild(node);
                docXml.AppendChild(rootNode);

                return docXml.OuterXml;
            }
        }
        #endregion

        #region Constructors
        public Package()
        {
            ui = Guid.NewGuid();
            storeFileName = ui.ToString() + ".pdf";
        }

        public Package(string _packName, string _packFileName, MortgageProfile _mortgage) : this()
        {
            PackName = _packName;
            PackFileName = _packFileName;
            mortgage = _mortgage;
        }

        public Package(int packageID) : this()
        {
            ID = packageID;
            if (packageID <= 0)
                return;

            DataTable tblPackage = db.GetDataTable("GetPackageByID", packageID);
            if (tblPackage.Rows.Count == 0)
                return;

            LoadFromDataRow(tblPackage.DefaultView[0]);
        }

        public Package(Guid packageUI) : this()
        {
            DataTable tblPackage = db.GetDataTable("GetPackageByUI", packageUI);
            if (tblPackage.Rows.Count == 0)
                return;

            LoadFromDataRow(tblPackage.DefaultView[0]);
        }
        #endregion

        #region Private methods
        private void LoadFromDataRow(DataRowView rowPackage)
        {
            ID = ConvertToInt(rowPackage["ID"], -1);
            MortgageID = ConvertToInt(rowPackage["MortgageID"], -1);
            VendorID = ConvertToInt(rowPackage["VendorId"], -1);

            PackName = ConvertToString(rowPackage["PackageName"], String.Empty);
            PackFileName = ConvertToString(rowPackage["Title"], String.Empty);
            UploadDate = ConvertToDateTime(rowPackage["UploadDate"], DateTime.Now);

            storeFileName = ConvertToString(rowPackage["FileName"], String.Empty);
            ui = ConvertToGuid(rowPackage["UI"], Guid.NewGuid());
        }
        #endregion

        #region Save methods
        private string SavePackageToDisk(MemoryStream dtResMemoryStream)
        {
            string packFilePath = PackageFilePath;
            FileStream packageStream = null;
            try
            {
                byte[] dtBuffer = dtResMemoryStream.ToArray();

                packageStream = new FileStream(packFilePath, FileMode.Create);
                packageStream.Write(dtBuffer, 0, dtBuffer.Length);
                packageStream.Flush();
            }
            catch (Exception)
            {
                packFilePath = String.Empty;
            }
            finally
            {
                if (packageStream != null)
                    packageStream.Close();
            }

            return packFilePath;
        }

        private int SavePackageToDatabase()
        {
            int resAction;
            try
            {
                resAction = db.ExecuteScalarInt("SaveFilledDocsInfo", Xml);
            }
            catch (Exception)
            {
                resAction = 0;
            }

            if (resAction <= 0)
                File.Delete(PackageFilePath);

            return resAction;
        }

        public string Save(MemoryStream dtResMemoryStream)
        {
            string packFilePath = SavePackageToDisk(dtResMemoryStream);
            if (String.IsNullOrEmpty(packFilePath))
                return String.Format("Can't save file {0} on the disk", PackName);

            int packageID = SavePackageToDatabase();
            if (packageID <= 0)
                return String.Format("File info of {0} can't be stored to the database", PackName);

            ID = packageID;
            return String.Empty;
        }

        public string SendPackage(string dtIDXml, AppUser userFrom, string url)
        {
            if (mortgage == null || VendorID <= 0)
                return "Closing agent is not set";
            else if (String.IsNullOrEmpty(mortgage.MortgageInfo.ClosingAgent.CompanyEmail))
                return "Closing Agent does not have mail address to send to";

            string errMsg = String.Empty;
            MemoryStream dtResMemoryStream = mortgage.GetResPdfStream(dtIDXml, out errMsg);
            if (dtResMemoryStream == null)
                return errMsg;

            errMsg = Save(dtResMemoryStream);
            dtResMemoryStream.Close();
            if (!String.IsNullOrEmpty(errMsg))
                return errMsg;

            VendorGlobal closingAgent = mortgage.MortgageInfo.ClosingAgent;
            try
            {
                Mailer mail = new Mailer();
                mail.From = String.IsNullOrEmpty(userFrom.EmailAccount.Email) ? "donotreply@gmail.com" : userFrom.EmailAccount.Email;
                mail.To = closingAgent.CompanyEmail;
                mail.Subject = String.Format("{0} {1}", PackName, UploadDate.ToString());
                mail.Body = "To:";
                mail.Body += String.IsNullOrEmpty(closingAgent.Name) ? " " : String.Format(" {0} \n      ", closingAgent.Name);
                mail.Body += String.IsNullOrEmpty(closingAgent.PrimaryContactName) ? String.Empty : String.Format("{0} ", closingAgent.PrimaryContactName);
                mail.Body += String.Format("\nThis message has been sent to you from {0} from {1}. \n", userFrom.FullName, userFrom.OriginatorName);
                mail.Body += String.Format("\nThe settlement documents for {0} are ready for pickup. You can \nretrieve them using the secure link below. \n", mortgage.Borrowers[0].LastName);
                mail.Body += String.Format("{2}{0}?PackageUI={1}{2}{2}{2}{2}{2}", url, ui.ToString(), System.Environment.NewLine);
                mail.Body += 
@"This e-mail transmission contains information that is confidential and may be privileged. 
It is intended only for the addressee(s) named above. If you receive this e-mail in error, 
please do not read, copy or disseminate it in any manner. If you are not the intended 
recipient, any disclosure, copying, distribution or use of the contents of this information 
is prohibited. Please reply to the message immediately by informing the sender that the 
message was misdirected. After replying, please erase it from your computer system. 
Your assistance in correcting this error is appreciated.";

                mail.Send(userFrom.CompanyId);
            }
            catch (Exception ex)
            {
                db.Execute("DeleteFilledDocInfo", ID);
                File.Delete(PackageFilePath);
                return String.Format("{0} has not been sent to the closing agent. \nError: {1}", PackName, ex.Message);
            }

            return String.Empty;
        }
        #endregion

        #region Static fields
        private static new DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        public static string PackageFolder = String.Empty;
        #endregion
    }
}

using System;
using System.Configuration;
using System.Text.RegularExpressions;
using BossDev.CommonUtils;
using System.Xml;
using WebMailPro;
using MailBee;
using MailBee.Pop3Mail;
using MailBee.Security;

namespace LoanStar.Common
{
    public class WebMailHelper
    {
        private const string UPDATEMESSAGEASSOCIATION = "UpdateMessageAssociation";
        private const string ADDLOANASSOCIATION = "AddLoanEmailAssociation";
        private const string ADDMESSAGEASSOCIATION = "AddEmailMessageAssociation";
        private const string TESTMESSAGETEXT = "Send email test";
        private const string TESTSUBJECT = "Email test";
        private const string DOMAINNAMEREGEXP = @"^[a-zA-Z0-9\-\.]+\.(aero|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly| ma|mc|md|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk| pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr| st|su|sv|sy|sz|tc|td|tf|tg|th|tj|tk|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zr|zw|AERO|BIZ|COM|COOP|EDU|GOV|INFO|INT|MIL|MUSEUM|NAME|NET|ORG|AC)$";
        private const string IPADDRESSREGEXP = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
        private const string ROOTELEMENT = "Root";
        private const string ITEMELEMENT = "item";
        private const string IDATTRIBUTE = "id";


        #region Private fields
        private static readonly DatabaseAccess dbAccess = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        public static bool UpdateMessageAssociation(int messageId, int mortgageId, int op)
        {
//            InitConnection();
            return dbAccess.ExecuteScalarInt(UPDATEMESSAGEASSOCIATION, messageId, mortgageId, op) == 1;
        }
        //private static void InitConnection()
        //{
        //    if(dbAccess==null)
        //    {
        //        WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
        //        string connectionString = DbManager.CreateConnectionString(settings.UseCustomConnectionString,
        //                                            settings.DbCustomConnectionString,
        //                                            settings.DbDsn,
        //                                            settings.DbType,
        //                                            settings.DbPathToMdb,
        //                                            settings.DbLogin,
        //                                            settings.DbPassword,
        //                                            settings.DbName,
        //                                            settings.DbHost);
        //        dbAccess = new DatabaseAccess(connectionString);
        //    }
        //}
        public static Account GetAccoun(int accountId)
        {
                if (LoanStarPortal.Global.integration == null)
                {
                    LoanStarPortal.Global.integration = new Integration(ConfigurationManager.AppSettings["dataFolderPath"], @".");
                }
            return LoanStarPortal.Global.integration.GetAccountByID(accountId);
        }
        public static void SaveEmailAccount(Account acc)
        {
            if (acc!=null)
            {
                if(acc.ID>0)
                {
                    acc.Update(false);
                }
                else
                {
                    if (!String.IsNullOrEmpty(acc.Email))
                    {
                        User usr = User.CreateUser();
                        Account a = usr.CreateAccount(acc.DefaultAccount,acc.Deleted,acc.Email
                                        ,acc.MailIncomingProtocol,acc.MailIncomingHost,acc.MailIncomingLogin,acc.MailIncomingPassword,acc.MailIncomingPort
                                        ,acc.MailOutgoingHost,acc.MailOutgoingLogin,acc.MailOutgoingPassword,acc.MailOutgoingPort,acc.MailOutgoingAuthentication
                                        ,acc.FriendlyName,acc.UseFriendlyName,acc.DefaultOrder,acc.GetMailAtLogin,acc.MailMode,acc.MailsOnServerDays
                                        , acc.Signature, acc.SignatureType, acc.SignatureOptions, acc.Delimiter, acc.MailboxSize, FolderSyncType.AllEntireMessages, true);
//                                        ,acc.Signature,acc.SignatureType,acc.SignatureOptions,acc.Delimiter,acc.MailboxSize,FolderSyncType.DirectMode,true);
                        acc.ID = a.ID;
                    }
                }
            }
        }
        public static void TestEmailAccount(string from, string to, string smtpHost, int smtpPort, string login, string password, bool smtpAuth, string pop3Server, int pop3Port, out string smtpResult, out string pop3Result)
        {
            smtpResult = TestSmtpAccount(from, to, smtpHost, smtpPort, login, password, smtpAuth);
            pop3Result = TestPop3Account(pop3Server, pop3Port, login, password);
        }
        private static string TestSmtpAccount(string from, string to, string smtpHost, int smtpPort, string login, string password, bool smtpAuth)
        {
            string res = string.Empty;
            try
            {

                Smtp.SendMail(from, to, TESTSUBJECT, TESTMESSAGETEXT, smtpHost, smtpPort, login, password, smtpAuth);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        private static string TestPop3Account(string pop3Server, int pop3Port, string login, string password)
        {
            string res = string.Empty;
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            Pop3.LicenseKey = settings.LicenseKey;
            Pop3 pop3 = new Pop3();
            System.Threading.Thread.Sleep(50);
            try
            {
                pop3.InboxPreloadOptions = Pop3InboxPreloadOptions.Uidl;
                if (pop3Port == 995)
                {
                    pop3.SslMode = SslStartupMode.OnConnect;
                    pop3.SslProtocol = SecurityProtocol.Auto;
                    pop3.SslCertificates.AutoValidation = CertificateValidationFlags.None;
                }
                pop3.Connect(pop3Server, pop3Port, true);
                pop3.Login(login, password, AuthenticationMethods.Auto, AuthenticationOptions.PreferSimpleMethods, null);
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            finally
            {
                if (pop3.IsConnected) pop3.Disconnect();
            }
            return res;
        }
        public static bool ValidatePort(string sport)
        {
            bool res = false;
            try
            {
                int port = int.Parse(sport);
                if (port >= 0 && port <= 65535)
                {
                    res = true;
                }
            }
            catch
            {
            }
            return res;
        }

        public static bool ValidateServer(string server)
        {
            bool res = Regex.Match(server, DOMAINNAMEREGEXP).Success;
            if (!res)
            {
                res = Regex.Match(server, IPADDRESSREGEXP).Success;
            }
            return res;
        }
        public static bool AddEmailAssociation(long msgId, int assosiationType, int conditionId, int mortgageId)
        {
            return dbAccess.ExecuteScalarInt(ADDMESSAGEASSOCIATION, msgId, conditionId, mortgageId, assosiationType) > 0;
        }

        public static bool AddLoanAssosiation(long msgId, string loanIds)
        {
            bool res = false;
            if(!String.IsNullOrEmpty(loanIds))
            {
                string[] tmp = loanIds.Split(';');
                if(tmp.Length>0)
                {
                    XmlDocument d = new XmlDocument();
                    XmlNode root = d.CreateElement(ROOTELEMENT);
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if(!String.IsNullOrEmpty(tmp[i]))
                        {
                            XmlNode n = d.CreateElement(ITEMELEMENT);
                            XmlAttribute a = d.CreateAttribute(IDATTRIBUTE);
                            a.Value = tmp[i];
                            n.Attributes.Append(a);
                            root.AppendChild(n);
                        }
                    }
                    if(root.ChildNodes.Count>0)
                    {
                        d.AppendChild(root);
                        res = dbAccess.ExecuteScalarInt(ADDLOANASSOCIATION, msgId, d.OuterXml) > 0;
                    }
                }
            }
            return res;
        }
    }
}

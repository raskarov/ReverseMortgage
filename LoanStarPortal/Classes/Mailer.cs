using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using BossDev.CommonUtils;
using LumiSoft.Net.Mime;
using LumiSoft.Net.POP3.Client;
//using LoanStar

namespace LoanStar.Common
{
    public enum MailStatus
    {
        InboxNew    = 0,
        Inbox       = 1,
        Outbox      = 2,
        Sent        = 3,
        Draft       = 4
    }

    public enum MailObjectType
    {
        Mortgage    = 1,
        Task        = 2,
        Condition   = 3
    }

    [XmlRoot("Mail")]
    public class Mailer : BaseObject
    {
        #region Private fields
        private int userID = -1;
        private MailStatus mailStatus = MailStatus.InboxNew;
        private DateTime date = DateTime.Now;

        private readonly MailMessage mail = new MailMessage();
        private readonly Guid mailGuid = Guid.NewGuid();
        private readonly List<MailObject> mailObjectList = new List<MailObject>();
        private string messageUID = String.Empty;
        #endregion

        #region Properties
        public string MessageUID
        {
            get
            {
                return messageUID;
            }
            set
            {
                messageUID = value ?? String.Empty;
            }
        }

        public List<MailObject> MailObjectList
        {
            get
            {
                return mailObjectList;
            }
            set
            {
            }
        }

        [XmlElement("AttachesList")]
        public String AttachesXml
        {
            get
            {
                string resXml = String.Empty;
                foreach (MailerAttachment mailAttach in mail.Attachments)
                    resXml += resXml.Length == 0 ? mailAttach.Xml : Environment.NewLine + mailAttach.Xml;

                return resXml;//.Length == 0 ? "<MailAttachment />" : resXml;
            }
            set
            {
            }
        }

        [XmlIgnore]
        public int FirstMPID
        {
            get
            {
                foreach (MailObject mailObject in mailObjectList)
                    if (mailObject.ObjectType == MailObjectType.Mortgage)
                        return mailObject.ObjectID;

                return -1;
            }
        }

        [XmlIgnore]
        public XmlNode MailNode
        {
            get
            {
                string serDocXml;
                StringWriter xmlObject = new StringWriter();
                try
                {
                    XmlSerializer xs = new XmlSerializer(GetType());
                    xs.Serialize(xmlObject, this);
//                    serDocXml = xmlObject.ToString().Replace(">&lt;", "><").Replace("&gt;<", "><").Replace("&gt;\r\n&lt;", "><");
                    serDocXml = xmlObject.ToString().Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
                }
                finally
                {
                    xmlObject.Close();
                }

                XmlDocument serDoc = new XmlDocument();
                serDoc.LoadXml(serDocXml);
                return serDoc.DocumentElement;
            }
        }

        [XmlIgnore]
        public int AttachesCount
        {
            get
            {
                int attachesCount = 0;
                foreach (MailerAttachment mailAttach in mail.Attachments)
                    if (mailAttach.IsAttachment)
                        attachesCount++;

                return attachesCount;
            }
        }

        [XmlIgnore]
        public ICollection AttachesList
        {
            get
            {
                return mail.Attachments;
            }
        }

        [XmlIgnore]
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public Guid MailGuid
        {
            get
            {
                return mailGuid;
            }
            set
            {
            }
        }

        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public int UserID
        {
            get
            {
                return userID;
            }
            set
            {
                userID = value;
            }
        }

        public int MailStatusID
        {
            get
            {
                return Convert.ToInt32(mailStatus);
            }
            set
            {
                mailStatus = (MailStatus)value;
            }
        }

        [XmlIgnore]
        public MailStatus MailStatus
        {
            get
            {
                return mailStatus;
            }
            set
            {
                mailStatus = value;
            }
        }

        public string FromXml
        {
            get
            {
                string innerXml = From;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string From
        {
            set
            {
                mail.From = String.IsNullOrEmpty(value) ? null : new MailAddress(value);
            }
            get
            {
                return mail.From == null ? String.Empty : mail.From.ToString();
            }
        }

         private void SetFrom(AddressList addressList)
        {
            From = addressList == null || addressList.Mailboxes.Length == 0 ? String.Empty : addressList.Mailboxes[0].ToMailboxAddressString();
        }

        public string ToXml
        {
            get
            {
                string innerXml = To;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string To
        {
            get
            {
                return mail.To.ToString();
            }
            set
            {
                string[] toAddressArr = value.Split(',', ';');
                SetTo(toAddressArr);
            }
        }

        private void SetTo(string[] toAddressArr)
        {
            mail.To.Clear();

            foreach (string toAddress in toAddressArr)
            {
                if (String.IsNullOrEmpty(toAddress))
                    continue;

                mail.To.Add(new MailAddress(toAddress.Trim()));
            }
        }

        private void SetTo(AddressList addressList)
        {
            mail.To.Clear();
            if (addressList == null)
                return;

            MailboxAddress[] addressArr = addressList.Mailboxes;
            foreach (MailboxAddress address in addressArr)
                mail.To.Add(new MailAddress(address.ToMailboxAddressString()));
        }

        public string CCXml
        {
            get
            {
                string innerXml = CC;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string CC
        {
            get
            {
                return mail.CC.ToString();
            }
            set
            {
                string[] ccAddressArr = value.Split(',', ';');
                SetCC(ccAddressArr);
            }
        }

        private void SetCC(string[] ccAddressArr)
        {
            mail.CC.Clear();

            foreach (string ccAddress in ccAddressArr)
            {
                if (String.IsNullOrEmpty(ccAddress))
                    continue;

                mail.CC.Add(new MailAddress(ccAddress.Trim()));
            }
        }

        private void SetCC(AddressList addressList)
        {
            mail.CC.Clear();
            if (addressList == null)
                return;

            MailboxAddress[] addressArr = addressList.Mailboxes;
            foreach (MailboxAddress address in addressArr)
                mail.CC.Add(new MailAddress(address.ToMailboxAddressString()));
        }

        public string BccXml
        {
            get
            {
                string innerXml = Bcc;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string Bcc
        {
            get
            {
                return mail.Bcc.ToString();
            }
            set
            {
                string[] bccAddressArr = value.Split(',', ';');
                SetBcc(bccAddressArr);
            }
        }

        private void SetBcc(string[] bccAddressArr)
        {
            mail.Bcc.Clear();

            foreach (string bccAddress in bccAddressArr)
            {
                if (String.IsNullOrEmpty(bccAddress))
                    continue;

                mail.Bcc.Add(new MailAddress(bccAddress.Trim()));
            }
        }

        private void SetBcc(AddressList addressList)
        {
            mail.Bcc.Clear();
            if (addressList == null)
                return;

            MailboxAddress[] addressArr = addressList.Mailboxes;
            foreach (MailboxAddress address in addressArr)
                mail.Bcc.Add(new MailAddress(address.ToMailboxAddressString()));
        }

        public string ReplyToXml
        {
            get
            {
                string innerXml = ReplyTo;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string ReplyTo
        {
            set
            {
                mail.ReplyTo = String.IsNullOrEmpty(value) ? null : new MailAddress(value);
            }
            get
            {
                return mail.ReplyTo == null ? From : mail.ReplyTo.ToString();
            }
        }

        private void SetReplyTo(AddressList addressList)
        {
            ReplyTo = addressList == null || addressList.Mailboxes.Length == 0 ? String.Empty : addressList.Mailboxes[0].ToMailboxAddressString();
        }

        public string SenderXml
        {
            get
            {
                string innerXml = Sender;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string Sender
        {
            set
            {
                mail.Sender = String.IsNullOrEmpty(value) ? null : new MailAddress(value);
            }
            get
            {
                return mail.Sender == null ? this.From : mail.Sender.ToString();
            }
        }

        private void SetSender(MailboxAddress address)
        {
            Sender = address == null ? String.Empty : address.ToMailboxAddressString();
        }

        public string SubjectXml
        {
            get
            {
                string innerXml = Subject;
                if (String.IsNullOrEmpty(innerXml))
                    return innerXml;

                return "<![CDATA[" + innerXml + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string Subject
        {
            get
            {
                return mail.Subject;
            }
            set
            {
                mail.Subject = value == null ? String.Empty : value;
            }
        }

        public int SubjectPageCode
        {
            get
            {
                return mail.SubjectEncoding.CodePage;
            }
            set
            {
                mail.SubjectEncoding = Encoding.GetEncoding(value);
            }
        }

        public string BodyXml
        {
            get
            {
                if (String.IsNullOrEmpty(mail.Body))
                    return mail.Body;

                return "<![CDATA[" + mail.Body + "]]>";
            }
            set
            {
            }
        }

        [XmlIgnore]
        public string BodyHTML
        {
            get
            {
                if (String.IsNullOrEmpty(this.Body) || mail.Attachments.Count == 0)
                    return this.Body;

                string bodyInline = this.Body.Replace("CID:", "cid:");
                foreach (MailerAttachment mailAttach in mail.Attachments)
                    if (mailAttach.IsInline && !String.IsNullOrEmpty(mailAttach.ContentId))
                    {
                        string contentSource = String.Format("cid:{0}", mailAttach.ContentId);
                        string fileSource = Path.Combine("Storage/MailAttachments", mailAttach.AttachFileInfo.Name);
                        bodyInline = bodyInline.Replace(contentSource, fileSource);
                    }

                return bodyInline;
            }
            set
            {
                string bodyHTML = String.IsNullOrEmpty(value) ? String.Empty : value;
                foreach (MailerAttachment mailAttach in mail.Attachments)
                    if (mailAttach.IsInline && !String.IsNullOrEmpty(mailAttach.ContentId))
                    {
                        string contentSource = String.Format("cid:{0}", mailAttach.ContentId);
                        string fileSource = Path.Combine("Storage/MailAttachments", mailAttach.AttachFileInfo.Name);
                        bodyHTML = bodyHTML.Replace(fileSource, contentSource);
                    }

                this.Body = bodyHTML;
            }
        }

        [XmlIgnore]
        public string Body
        {
            get
            {
                return mail.Body;
            }
            set
            {
                mail.Body = value;
            }
        }

        public int BodyPageCode
        {
            get
            {
                return mail.BodyEncoding.CodePage;
            }
            set
            {
                mail.BodyEncoding = Encoding.GetEncoding(value);
            }
        }

        [XmlElement("IsBodyHtml")]
        public int IsBodyHtmlInt
        {
            get
            {
                return Convert.ToInt32(IsBodyHtml);
            }
            set
            {
                IsBodyHtml = Convert.ToBoolean(value);
            }
        }

        [XmlIgnore]
        public bool IsBodyHtml
        {
            get
            {
                return mail.IsBodyHtml;
            }
            set
            {
                mail.IsBodyHtml = value;
            }
        }

        [XmlElement("Priority")]
        public int PriorityInt
        {
            get
            {
                return Convert.ToInt32(mail.Priority);
            }
            set
            {
                mail.Priority = (MailPriority)value;
            }
        }

        [XmlIgnore]
        public string PriorityStr
        {
            get
            {
                return mail.Priority.ToString();
            }
            set
            {
                MailPriority resPriority = MailPriority.Normal;
                try
                {
                    resPriority = (MailPriority)Enum.Parse(typeof(MailPriority), value, true);
                }
                catch (Exception){}
            }
        }

        [XmlIgnore]
        public MailPriority Priority
        {
            get
            {
                return mail.Priority;
            }
            set
            {
                mail.Priority = value;
            }
        }
        #endregion

        #region Constructors
        public Mailer()
        {
            SubjectPageCode = 65001;
            BodyPageCode = 65001;
            Priority = MailPriority.Normal;
            IsBodyHtml = false;
        }

        public Mailer(string msgUID, byte[] messageBuffer, int appUserID)
        {
            MessageUID = msgUID;

            this.UserID = appUserID;
            this.MailStatus = MailStatus.InboxNew;

            Mime mime = Mime.Parse(messageBuffer);

            SetFrom(mime.MainEntity.From);
            SetTo(mime.MainEntity.To);
            SetCC(mime.MainEntity.Cc);
            SetBcc(mime.MainEntity.Bcc);
            SetReplyTo(mime.MainEntity.ReplyTo);
            SetSender(mime.MainEntity.Sender);

            Subject = mime.MainEntity.Subject;
            SubjectPageCode = 65001;
            BodyPageCode = 65001;
            IsBodyHtml = false;

            Date = mime.MainEntity.Date;

            if (mime.MainEntity.Header.Contains("X-MSMail-Priority:"))
                PriorityStr = mime.MainEntity.Header.GetFirst("X-MSMail-Priority:").Value;
            else
                Priority = MailPriority.Normal;

            string mortgageKey = String.Format("{0}:", MailObjectType.Mortgage);
            if (mime.MainEntity.Header.Contains(mortgageKey))
            {
                string mortgageInfo = mime.MainEntity.Header.GetFirst(mortgageKey).Value;
                AddMailObjectInfo(mortgageInfo, MailObjectType.Mortgage);
            }
            string taskKey = String.Format("{0}:", MailObjectType.Task.ToString());
            if (mime.MainEntity.Header.Contains(taskKey))
            {
                string taskInfo = mime.MainEntity.Header.GetFirst(taskKey).Value;
                AddMailObjectInfo(taskInfo, MailObjectType.Task);
            }
            string conditionKey = String.Format("{0}:", MailObjectType.Condition.ToString());
            if (mime.MainEntity.Header.Contains(conditionKey))
            {
                string conditionInfo = mime.MainEntity.Header.GetFirst(conditionKey).Value;
                AddMailObjectInfo(conditionInfo, MailObjectType.Condition);
            }

            GoThroughMimeEntries(mime.MainEntity);
        }

        public Mailer(int mailerID) : this()
        {
            ID = mailerID;
            if (mailerID <= 0)
                return;

            DataSet dsMailer = db.GetDataSet("GetMailByID", mailerID);
            if (dsMailer.Tables.Count == 0 || dsMailer.Tables[0].Rows.Count == 0)
                return;

            LoadFromDataRow(dsMailer.Tables[0].DefaultView[0]);

            if (dsMailer.Tables.Count < 2)
                return;

            foreach (DataRowView attachRow in dsMailer.Tables[1].DefaultView)
                AddAttach(attachRow);

            if (dsMailer.Tables.Count < 3)
                return;

            foreach (DataRowView mailObjectRow in dsMailer.Tables[2].DefaultView)
                AddMailObject(mailObjectRow);
        }
        #endregion

        #region Public methods

        public Mailer ReplyAllToMail(AppUser user)
        {
            //Mailer newMailer = new Mailer();
            //newMailer.From = user.MailAddress;
            //newMailer.To = this.ReplyTo;
            //newMailer.CC = this.CC;
            //newMailer.Subject = String.Format("Re: {0}", this.Subject);
            //newMailer.Body = this.Body;
            //newMailer.ImportMailObjectList(this);
            //return newMailer;
            Mailer newMailer = CreateMail(user, "Re: {0}");
            newMailer.To = ReplyTo;
            newMailer.CC = CC;

            newMailer.ImportAttachList(this, true);
            newMailer.ImportMailObjectList(this);
            return newMailer;
        }

        public Mailer ReplyToMail(AppUser user)
        {
            //Mailer newMailer = new Mailer();
            //newMailer.From = user.MailAddress;
            //newMailer.To = this.ReplyTo;
            //newMailer.Subject = String.Format("Re: {0}", this.Subject);
            //newMailer.Body = this.Body;
            //newMailer.ImportMailObjectList(this);
            //return newMailer;
            Mailer newMailer = CreateMail(user, "Re: {0}");
            newMailer.To = ReplyTo;

            newMailer.ImportAttachList(this, true);
            newMailer.ImportMailObjectList(this);
            return newMailer;
        }

        public Mailer ForwardMail(AppUser user)
        {
            //Mailer newMailer = new Mailer();
            //newMailer.From = user.MailAddress;
            //newMailer.Subject = String.Format("Fw: {0}", this.Subject);
            //newMailer.Body = this.Body;
            //newMailer.ImportMailObjectList(this);
//            return newMailer;
            Mailer newMailer = CreateMail(user, "Fw: {0}");

            newMailer.ImportAttachList(this, false);
            newMailer.ImportMailObjectList(this);
            return newMailer;
        }

        public int GetMailObjectsCount(MailObjectType objectType)
        {
            int resCount = 0;
            foreach (MailObject mailObject in mailObjectList)
                if (mailObject.ObjectType == objectType)
                    resCount++;

            return resCount;
        }

        public ArrayList GetMailObjectIDList(MailObjectType objectType)
        {
            ArrayList idList = new ArrayList();
            foreach (MailObject mailObject in mailObjectList)
                if (mailObject.ObjectType == objectType)
                    idList.Add(mailObject.ObjectID);

            return idList;
        }

        public MailerAttachment GetAttachmentByID(int attachID)
        {
            if (attachID <= 0)
                return null;

            foreach (MailerAttachment mailAttach in mail.Attachments)
                if (mailAttach.ID == attachID)
                    return mailAttach;

            return null;
        }

        public void AddAttach(Stream attachStream, string attachName, string attachContentType)
        {
            MailerAttachment attach;
            try
            {
                byte[] attachBuffer = new byte[attachStream.Length];
                attachStream.Read(attachBuffer, 0, (int)attachStream.Length);

                string attachFileName = CreateAttachFile(attachBuffer);
                attach = new MailerAttachment(attachFileName, attachName, attachContentType);
            }
            catch (IOException ex)
            {
                throw new Exception(String.Format("Could not find file {0}", attachName), ex);
            }

            mail.Attachments.Add(attach);
        }

        public void AddMailObject(int objID, MailObjectType objType, int order)
        {
            mailObjectList.Add(new MailObject(this, objID, objType, order));
        }

        public void AddMailObject(DataRowView mailObjectRow)
        {
            mailObjectList.Add(new MailObject(this, mailObjectRow));
        }
 
        public void AddMailObjectList(ArrayList mailObjectIDList, MailObjectType objectType)
        {
            for (int i = 0; i < mailObjectIDList.Count; i++)
                mailObjectList.Add(new MailObject(this, (int)mailObjectIDList[i], objectType, i));
        }

        public void AddMailObjectRemovedList(ArrayList mailObjectIDList, MailObjectType objectType)
        {
            foreach (int objID in mailObjectIDList)
                mailObjectList.Add(new MailObject(this, objID, objectType, -1));
        }

        public void ClearAttachments()
        {
            try
            {
                foreach (MailerAttachment mailAttach in mail.Attachments)
                    if (mailAttach.AttachFileInfo.Exists)
                        mailAttach.AttachFileInfo.Delete();
            }
            catch (Exception) { }

            mail.Attachments.Clear();
        }

        public void ClearMailObjects()
        {
            mailObjectList.Clear();
        }

        public void ClearAllRelations()
        {
            ClearAttachments();
            ClearMailObjects();
        }

        public void Send(int CompanyID)
        {
            string mortgageInfo = GetMailObjectInfoString(MailObjectType.Mortgage);
            if (!String.IsNullOrEmpty(mortgageInfo))
                mail.Headers.Add(MailObjectType.Mortgage.ToString(), mortgageInfo);

            string taskInfo = GetMailObjectInfoString(MailObjectType.Task);
            if (!String.IsNullOrEmpty(taskInfo))
                mail.Headers.Add(MailObjectType.Task.ToString(), taskInfo);

            string conditionInfo = GetMailObjectInfoString(MailObjectType.Condition);
            if (!String.IsNullOrEmpty(conditionInfo))
                mail.Headers.Add(MailObjectType.Condition.ToString(), conditionInfo);

            Company company = new Company(CompanyID);
            SmtpClient client = new SmtpClient(company.SMTPServer);
//            client.Credentials = new System.Net.NetworkCredential(company.SMTPUserID, company.SMTPPassword);

            //hate hard code, but need to get the system going...
            if (String.Compare(company.SMTPServer, "gmail.com", true) == 0)
                client.Port = 587;
            else
                client.Port = company.SMTPPort;
            client.EnableSsl = company.POP3SSL;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
        }

        public void Send(Company company)
        {
            string mortgageInfo = GetMailObjectInfoString(MailObjectType.Mortgage);
            if (!String.IsNullOrEmpty(mortgageInfo))
                mail.Headers.Add(MailObjectType.Mortgage.ToString(), mortgageInfo);

            string taskInfo = GetMailObjectInfoString(MailObjectType.Task);
            if (!String.IsNullOrEmpty(taskInfo))
                mail.Headers.Add(MailObjectType.Task.ToString(), taskInfo);

            string conditionInfo = GetMailObjectInfoString(MailObjectType.Condition);
            if (!String.IsNullOrEmpty(conditionInfo))
                mail.Headers.Add(MailObjectType.Condition.ToString(), conditionInfo);

            SmtpClient client = new SmtpClient(company.SMTPServer);
//            client.Credentials = new System.Net.NetworkCredential(company.SMTPUserID, company.SMTPPassword);

            //hate hard code, but need to get the system going...
            if (String.Compare(company.SMTPServer, "gmail.com", true) == 0)
                client.Port = 587;
            else
                client.Port = company.SMTPPort;
            client.EnableSsl = company.POP3SSL;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mail);
        }
        public void Save()
        {
            XmlDocument docXml = new XmlDocument();
            XmlNode rootNode = docXml.CreateElement("Root");

            XmlNode importMailNode = docXml.ImportNode(MailNode, true);
            rootNode.AppendChild(importMailNode);
            docXml.AppendChild(rootNode);

            DataSet resDS = SaveMailList(docXml.OuterXml);
            if (resDS.Tables.Count == 1)
                throw new Exception("Data was not saved to database");
            else
                ID = Convert.ToInt32(resDS.Tables[0].Rows[0]["MailID"]);
        }

        public void SaveMailObjectList()
        {
            XmlDocument docXml = new XmlDocument();
            XmlNode rootNode = docXml.CreateElement("Root");

            XmlNodeList nodeList = MailNode.SelectNodes("MailObjectList/MailObject");
            foreach (XmlNode moNode in nodeList)
            {
                XmlNode importMONode = docXml.ImportNode(moNode, true);
                rootNode.AppendChild(importMONode);
            }
            docXml.AppendChild(rootNode);

            int resSave = db.ExecuteScalarInt("SaveMailObjectList", docXml.OuterXml);
            if (resSave > 0)
                throw new Exception("Data was not saved to database");
        }

        public void AddAttach(string attachFileName,
                                string attachName,
                                string attachContentType,
                                string attachContentID,
                                string atachTransferEncoding,
                                string attachNamePageCode,
                                bool attachIsInline)
        {
            MailerAttachment attach;
            FileStream attachStream = null;
            try
            {
                FileInfo attachFileInfo = new FileInfo(MailAttachFolder + attachFileName);
                attachStream = attachFileInfo.OpenRead();
                byte[] attachBuffer = new byte[attachStream.Length];
                attachStream.Read(attachBuffer, 0, (int)attachStream.Length);

                string attachFileNameNew = CreateAttachFile(attachBuffer);
                attach = new MailerAttachment(attachFileNameNew,
                                                attachName,
                                                attachContentType,
                                                attachContentID,
                                                atachTransferEncoding,
                                                attachNamePageCode,
                                                attachIsInline);

            }
            catch (IOException ex)
            {
                throw new Exception(String.Format("Could not find file {0}", attachName), ex);
            }
            finally
            {
                if (attachStream != null)
                    attachStream.Close();
            }

            mail.Attachments.Add(attach);
            if (attach.IsInline && !String.IsNullOrEmpty(attach.ContentId))
            {
                string contentSource = String.Format("cid:{0}", attach.ContentId);
                string fileSource = Path.Combine("Storage/MailAttachments", attachFileName);
                Body = Body.Replace(fileSource, contentSource);
            }
        }

        public bool TestPopServer(Company company, out string ErrorMessage)
        {

            bool bResult = false;
            ErrorMessage = string.Empty;
            try
            {
                using (POP3_Client pop3Client = new POP3_Client())
                {
                    pop3Client.Connect(company.POP3Server, company.POP3Port, company.POP3SSL);
//                    pop3Client.Authenticate(company.POP3UserID, company.POP3Password, false);
                    pop3Client.Disconnect();
                }
                bResult = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return bResult;

        }


        public bool TestPopServer(
            string sPOP3Server,
            int iPOP3Port,
            bool bPOP3SSL,
            string sPOP3UserID,
            string sPOP3Password,
            out string ErrorMessage)
        {

            bool bResult = false;
            ErrorMessage = string.Empty;
            try
            {

                using (POP3_Client pop3Client = new POP3_Client())
                {
                    pop3Client.Connect(sPOP3Server, iPOP3Port, bPOP3SSL);
                    pop3Client.Authenticate(sPOP3UserID, sPOP3Password, false);
                    pop3Client.Disconnect();
                }
                bResult = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return bResult;

        }

        public bool TestSmtpServer(AppUser CurrentUser, Company company, out string ErrorMessage)
        {
            bool bResult = false;
            ErrorMessage = string.Empty;

            try
            {

                BuildHtmlMail(
                    MailStatus.Sent,
                    MailPriority.High,
                    "Test send email from RM Originator",
                    "<b>Test Email<b>",
                    99999999,
                    CurrentUser.EmailAccount.Email,
                    company.TestSendTo,
                    "",
                    "");

                Send(company);
                bResult = true;

            }
            catch (Exception ex)
            {

                string sInnerMessage = String.Empty;

                if (ex.InnerException != null)
                    sInnerMessage = " Due to: " + ex.InnerException.Message;

                ErrorMessage = ex.Message + sInnerMessage;
            }
            return bResult;
        }

        public void BuildHtmlMail(MailStatus mailStatus, 
                                 MailPriority mailPriority,
                                 string subject,
                                 string html,
                                 int userID,
                                 string from,
                                 string to,
                                 string cc,
                                 string bcc) 
        {

            this.Priority = mailPriority;
            this.Subject = subject;
            this.BodyHTML = html;
            this.IsBodyHtml = true;

            this.UserID = userID;
            this.MailStatus = mailStatus;

            this.From = from;
            this.To = to;
            this.CC = cc;
            this.Bcc = bcc;

        }

        #endregion

        #region Private methods

        private Mailer CreateMail(AppUser user, string subj)
        {
            Mailer newMailer = new Mailer();
            newMailer.From = user.EmailAccount.Email;
            newMailer.Subject = String.Format(subj, Subject);
            newMailer.Body = GetBody();
            return newMailer;
        }

        private string GetBody()
        {
            string htmlInsert = "<br/><br/><hr/>";
            htmlInsert += "-----Original Message-----";
            htmlInsert += "<b>";
            htmlInsert += String.Format("From: {0}<br/>", From);
            htmlInsert += String.Format("Sent: {0}<br/>", Date);
            htmlInsert += String.Format("To: {0}<br/>", To);
            htmlInsert += String.IsNullOrEmpty(CC) ? String.Empty : String.Format("Cc: {0}<br/>", this.CC);
            htmlInsert += String.Format("Subject: {0}<br/>", Subject);
            htmlInsert += "</b><br/>";

            return htmlInsert + Body;
        }

        private string GetMailObjectInfoString(MailObjectType objectType)
        {
            string moInfoList = String.Empty;
            foreach (MailObject mailObject in mailObjectList)
                if (mailObject.ObjectType == objectType)
                {
                    string moInfo = String.Format("{0}&{1}", mailObject.ObjectID, mailObject.Order);
                    moInfoList += String.IsNullOrEmpty(moInfoList) ? moInfo : String.Format("; {0}", moInfo);
                }

            return moInfoList;
        }

        private void ImportMailObjectList(Mailer mailer)
        {
            ClearMailObjects();
            mailObjectList.AddRange(mailer.mailObjectList);

            foreach (MailObject mailObject in mailObjectList)
                mailObject.ParentMailer = this;
        }

        private void AddAttach(DataRowView rowAttach)
        {
            MailerAttachment attach;
            try
            {
                string attachFileName = MailAttachFolder + ConvertToString(rowAttach["FileName"], String.Empty);
                attach = new MailerAttachment(attachFileName, rowAttach);
            }
            catch (IOException ex)
            {
                string attachName = ConvertToString(rowAttach["Name"], String.Empty);
                throw new Exception(String.Format("Could not find file {0}", attachName), ex);
            }

            mail.Attachments.Add(attach);
        }

        private void AddAttach(MimeEntity mimeEntity)
        {
            MailerAttachment attach;
            try
            {
                string attachFileName = CreateAttachFile(mimeEntity.Data);
                attach = new MailerAttachment(attachFileName, mimeEntity);
            }
            catch (IOException ex)
            {
                string attachName = mimeEntity.ContentDisposition_FileName;
                if (attachName == null)
                    attachName = mimeEntity.ContentType_Name;
                if (attachName == null)
                    attachName = String.Empty;
                throw new Exception(String.Format("Could not find file {0}", attachName), ex);
            }

            mail.Attachments.Add(attach);
        }

        private void AddMailObjectInfo(string moInfoList, MailObjectType objectType)
        {
            if (String.IsNullOrEmpty(moInfoList))
                return;

            string[] moInfoArr = moInfoList.Split(';');
            foreach (string moInfo in moInfoArr)
            {
                if (String.IsNullOrEmpty(moInfo.Trim()))
                    continue;

                string[] moInfoPair = moInfo.Split('&');
                if (moInfoPair.Length != 2 || String.IsNullOrEmpty(moInfoPair[0].Trim()) || String.IsNullOrEmpty(moInfoPair[1].Trim()))
                    continue;

                int moInfoObjectID = Convert.ToInt32(moInfoPair[0]);
                int moInfoOrder = Convert.ToInt32(moInfoPair[1]);
                AddMailObject(moInfoObjectID, objectType, moInfoOrder);
            }
        }


        private void ImportAttachList(Mailer mailer, bool inlineOnly)
        {
            ClearAttachments();
            foreach (MailerAttachment mailAttach in mailer.mail.Attachments)
                if (mailAttach.IsInline || (mailAttach.IsAttachment && !inlineOnly))
                {
                    try
                    {
                        string attachFileName = MailAttachFolder + mailAttach.AttachFileInfo.Name;
                        MailerAttachment newMailAttach = new MailerAttachment(attachFileName, mailAttach);
                        mail.Attachments.Add(newMailAttach);
                    }
                    catch (Exception) { }
                }
        }

        private void LoadFromDataRow(DataRowView rowMail)
        {
            ID = ConvertToInt(rowMail["ID"], -1);

            From = ConvertToString(rowMail["From"], String.Empty);
            To = ConvertToString(rowMail["To"], String.Empty);
            CC = ConvertToString(rowMail["CC"], String.Empty);
            Bcc = ConvertToString(rowMail["Bcc"], String.Empty);
            ReplyTo = ConvertToString(rowMail["ReplyTo"], String.Empty);
            Sender = ConvertToString(rowMail["Sender"], String.Empty);

            Subject = ConvertToString(rowMail["Subject"], String.Empty);
            SubjectPageCode = ConvertToInt(rowMail["SubjectPageCode"], 65001);

            Body = ConvertToString(rowMail["Body"], String.Empty);
            BodyPageCode = ConvertToInt(rowMail["BodyPageCode"], 65001);

            IsBodyHtml = ConvertToBool(rowMail["IsBodyHtml"], false);
            Date = ConvertToDateTime(rowMail["Date"], DateTime.MaxValue);
            Priority = (MailPriority)ConvertToInt(rowMail["Priority"], Convert.ToInt32(MailPriority.Normal));

            UserID = ConvertToInt(rowMail["UserID"], -1);
            MailStatus = (MailStatus)ConvertToInt(rowMail["MailStatusID"], (int)MailStatus.InboxNew);

            MessageUID = ConvertToString(rowMail["MessageUID"], String.Empty);
        }

        private void GoThroughMimeEntries(MimeEntity mimeEntity)
        {
            if (mimeEntity.ChildEntities.Count == 0)
            {
                if (mimeEntity.ContentType == MediaType_enum.Text_html &&
                            (mimeEntity.ContentDisposition == ContentDisposition_enum.NotSpecified ||
                              mimeEntity.ContentDisposition == ContentDisposition_enum.Inline) &&
                        String.IsNullOrEmpty(mimeEntity.ContentID))
                {
                    if (!IsBodyHtml)
                    {
                        Body = mimeEntity.DataText;
                        IsBodyHtml = true;
                    }
                    else
                        Body += Body.Length == 0 ? mimeEntity.DataText : "<br />" + mimeEntity.DataText;
                }
                else if ((mimeEntity.ContentType == MediaType_enum.Text_plain || mimeEntity.ContentType == MediaType_enum.Text) &&
                            (mimeEntity.ContentDisposition == ContentDisposition_enum.NotSpecified ||
                              mimeEntity.ContentDisposition == ContentDisposition_enum.Inline) &&
                         String.IsNullOrEmpty(mimeEntity.ContentID))
                {
                    if (!IsBodyHtml)
                        Body += Body.Length == 0 ? mimeEntity.DataText : Environment.NewLine + mimeEntity.DataText;
                }
                else if (!String.IsNullOrEmpty(mimeEntity.ContentID) ||
                            mimeEntity.ContentDisposition == ContentDisposition_enum.Attachment)
                    AddAttach(mimeEntity);
            }
            else
                foreach (MimeEntity mimeEntityInside in mimeEntity.ChildEntities)
                    GoThroughMimeEntries(mimeEntityInside);
        }
        #endregion

        #region Static fields
//        private static new DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        public static string MailAttachFolder = String.Empty;
        #endregion

        #region Static methods
        public static string CreateAttachFile(byte[] attachBuffer)
        {
            string attachFileName = MailAttachFolder + Guid.NewGuid() + ".pdf";
            try
            {
                File.WriteAllBytes(attachFileName, attachBuffer);
            }
            catch (Exception) { }
            return attachFileName;
        }

        public static void MoveMailsToStatus(string mailsXml, int statusID)
        {
            int resMove = db.ExecuteScalarInt("MoveMailsToStatus", mailsXml, statusID);
            if (resMove <= 0)
                throw new Exception("Data was not saved to database");
        }

        public static void DeleteMailStatus(int statusID)
        {
            int resDelete = db.ExecuteScalarInt("DeleteMailStatus", statusID);
            if (resDelete < 0)
                throw new Exception("Data was not saved to database");
            else if (resDelete > 0)
                throw new Exception("You need at first move mail(s) from this folder");
        }

        public static void CreateMailStatus(string statusName, int userID)
        {
            int resCreate = db.ExecuteScalarInt("CreateMailStatus", statusName, userID);
            if (resCreate < 0)
                throw new Exception("Data was not saved to database");
            else if (resCreate > 0)
                throw new Exception("Folder with this name already exists");
        }

        //public static bool Send(string from, string to, string cc, string bcc, string subject, string message)
        //{
        //    MailMessage msg = new MailMessage();
        //    msg.From = new MailAddress(from);
        //    msg.To.Add(to);
        //    msg.CC.Add(cc);
        //    msg.Bcc.Add(bcc);
        //    msg.Priority = MailPriority.High;
        //    msg.Subject = subject;
        //    msg.IsBodyHtml = true;
        //    msg.BodyEncoding = Encoding.UTF8;
        //    msg.Body = message;

        //    //System.Net.NetworkCredential cred = new System.Net.NetworkCredential(userName, password);
        //    //Company company = new Company(user.CompanyId);
        //    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings.Get("smtpServer"));
        //    //client.Credentials = new System.Net.NetworkCredential(company.SMTPUserID, company.SMTPPassword);
        //    client.EnableSsl = true;
        //    client.Port = 587;
        //    client.UseDefaultCredentials = true;

        //    try
        //    {
        //        client.Send(msg);

        //    }
        //    catch { }
        //    return true;
        //}

        public static DataView GetInboxMailsList(int userID)
        {
            return db.GetDataView("GetInboxMailsList", userID);
        }

        public static DataTable GetMailStatusList(int userID)
        {
            return db.GetDataTable("GetMailStatusList", userID);
        }

        public static void SavePop3MailList(AppUser user)
        {
            Company company = new Company(user.CompanyId);
            string pop3Host = company.POP3Server;
            int pop3Port = company.POP3Port;
            bool pop3SSL = company.POP3SSL;
            string pop3UserName = user.MailUserName;
            string pop3Password = user.MailPassword;
            if (pop3Port <= 0 ||
                    String.IsNullOrEmpty(pop3Host) ||
                    String.IsNullOrEmpty(pop3UserName) ||
                    String.IsNullOrEmpty(pop3Password))
                throw new Exception(String.Format("Email settings for user {0} are empty. Please, contact administrator.", user.FullName));

            ArrayList mailerList = new ArrayList();
            using (POP3_Client pop3Client = new POP3_Client())
            {
                pop3Client.Connect(pop3Host, pop3Port, pop3SSL);
                pop3Client.Authenticate(pop3UserName, pop3Password, true);

                POP3_MessagesInfo pop3MessagesInfo = pop3Client.GetMessagesInfo();
                POP3_MessageInfo[] pop3MessageInfoArr = pop3MessagesInfo.Messages;

                DataView existedMsgUIDs = GetExistedMessageUIDs(user.Id);
                foreach (POP3_MessageInfo pop3MessageInfo in pop3MessageInfoArr)
                {
                    string filter = String.Format("MessageUID='{0}'", pop3MessageInfo.MessageUID);
                    existedMsgUIDs.RowFilter = filter;
                    if (existedMsgUIDs.Count > 0)
                        continue;

                    byte[] pop3MessageBuffer = pop3Client.GetMessage(pop3MessageInfo.MessageNumber);
                    //                    pop3Client.DeleteMessage(pop3MessageInfo.MessageNumber);

                    mailerList.Add(new Mailer(pop3MessageInfo.MessageUID, pop3MessageBuffer, user.Id));
                }
            }

            SaveMailList(mailerList);
        }

        protected static DataView GetExistedMessageUIDs(int userID)
        {
            return db.GetDataView("GetExistedMessageUIDs", userID);
        }

        protected static void SaveMailList(ICollection mailerList)
        {
            if (mailerList.Count == 0)
                return;

            XmlDocument docXml = new XmlDocument();
            XmlNode rootNode = docXml.CreateElement("Root");

            foreach (Mailer mailer in mailerList)
            {
                XmlNode importMailNode = docXml.ImportNode(mailer.MailNode, true);
                rootNode.AppendChild(importMailNode);
            }
            docXml.AppendChild(rootNode);

            int resSave = SaveMailList(docXml.OuterXml).Tables.Count;
            if (resSave == 1)
                throw new Exception("Data was not saved to database");
        }

        protected static DataSet SaveMailList(string mailListXml)
        {
            return db.GetDataSet("SaveMailList", mailListXml);
        }

        public static int SetMailAsRead(int mailID)
        {
            return db.ExecuteScalarInt("SetMailAsRead", mailID);
        }
        #endregion

        #region Old code
        /*        public void ReplaceMailObjectList(ArrayList mailObjectIDList, MailObjectType objectType)
        {
            int i = 0;
            Hashtable removedMailObjHash = new Hashtable();
            while (i < mailObjectList.Count)
            {
                if (mailObjectList[i].ObjectType == objectType)
                {
                    if (mailObjectList[i].ID > 0)
                    {
                        string key = String.Format("{0}_{1}", mailObjectList[i].ObjectID, mailObjectList[i].ObjectTypeID);
                        removedMailObjHash[key] = mailObjectList[i];
                    }
                    mailObjectList.RemoveAt(i);
                }
                else
                    i++;
            }

            for (i = 0; i < mailObjectIDList.Count; i++)
            {
                string key = String.Format("{0}_{1}", (int)mailObjectIDList[i], (int)objectType);
                if (removedMailObjHash.ContainsKey(key))
                {
                    MailObject removedMailObj = (MailObject)removedMailObjHash[key];
                    removedMailObj.Order = i;
                    mailObjectList.Add(removedMailObj);
                }
                else
                    mailObjectList.Add(new MailObject(this, (int)mailObjectIDList[i], objectType, i));
            }
        }*/
        #endregion
    }

    public class MailerAttachment : Attachment
    {
        #region Private fields
        private int id = 0;
        private readonly FileInfo attachFileInfo;
        #endregion

        #region Properties
        public bool IsAttachment
        {
            get { return ContentDisposition.DispositionType == DispositionTypeNames.Attachment; }
        }

        public bool IsInline
        {
            get { return ContentDisposition.DispositionType == DispositionTypeNames.Inline; }
        }

        public string FileName
        {
            get { return AttachFileInfo.Name; }
        }

        public string ContentTypeStr
        {
            get { return ContentType.ToString(); }
        }

        public string TransferEncodingStr
        {
            get { return TransferEncoding.ToString(); }
        }

        public string NamePageCodeStr
        {
            get { return NamePageCode.ToString(); }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public FileInfo AttachFileInfo
        {
            get { return attachFileInfo; }
        }

        public int NamePageCode
        {
            get { return NameEncoding.CodePage; }
            set { NameEncoding = Encoding.GetEncoding(value); }
        }

        public string Xml
        {
            get
            {
                XmlDocument docXml = new XmlDocument();
                XmlNode mailAttachmentNode = docXml.CreateElement("MailAttachment");
                docXml.AppendChild(mailAttachmentNode);

                XmlAttribute attr = docXml.CreateAttribute("ID");
                attr.Value = ID.ToString();
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("FileName");
                attr.Value = attachFileInfo.Name;
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("NamePageCode");
                attr.Value = NamePageCode.ToString();
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("TransferEncoding");
                attr.Value = Convert.ToInt32(TransferEncoding).ToString();
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("Name");
                attr.Value = Name;
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("ContentType");
                attr.Value = ContentType.MediaType;
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("ContentID");
                attr.Value = this.ContentId;
                mailAttachmentNode.Attributes.Append(attr);

                attr = docXml.CreateAttribute("DispositionType");
                attr.Value = this.ContentDisposition.DispositionType;
                mailAttachmentNode.Attributes.Append(attr);

                return mailAttachmentNode.OuterXml;
            }
        }
        #endregion

        #region Static fields
        private static readonly DatabaseAccess db = new DatabaseAccess(AppSettings.SqlConnectionString);
        #endregion

        #region Static methods
        public static MailerAttachment GetAttachmentByID(int attachID)
        {
            DataTable tblAttach = db.GetDataTable("GetAttachByID", attachID);
            if (tblAttach.Rows.Count == 0)
                return null;

            MailerAttachment attach;
            DataRowView rowAttach = tblAttach.DefaultView[0];
            try
            {
                string attachFileName = Mailer.MailAttachFolder + ObjectConvert.ConvertToString(rowAttach["FileName"], String.Empty);
                attach = new MailerAttachment(attachFileName, rowAttach);
            }
            catch (IOException ex)
            {
                string attachName = ObjectConvert.ConvertToString(rowAttach["Name"], String.Empty);
                throw new Exception(String.Format("Could not find file {0}", attachName), ex);
            }

            return attach;
        }
        #endregion

        #region Constructors
        public MailerAttachment(string attachFileName, DataRowView rowAttach) : base(attachFileName)
        {
            attachFileInfo = new FileInfo(attachFileName);
            LoadFromDataRow(rowAttach);
        }

        public MailerAttachment(string attachFileName, MimeEntity mimeEntity) : base(attachFileName)
        {
            attachFileInfo = new FileInfo(attachFileName);
            LoadFromMimeEntity(mimeEntity);
        }

        public MailerAttachment(string attachFileName, MailerAttachment attach) : base(attachFileName)
        {
            attachFileInfo = new FileInfo(attachFileName);
            LoadFromAttachment(attach);
        }

        public MailerAttachment(string attachFileName, string attachName, string attachContentType) : base(attachFileName)
        {
            attachFileInfo = new FileInfo(attachFileName);

            ContentType = String.IsNullOrEmpty(attachContentType) ? new ContentType() : new ContentType(attachContentType);
            Name = attachName;
            NamePageCode = 65001;
            TransferEncoding = TransferEncoding.Base64;

            ContentDisposition.FileName = attachName;
            ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
            ContentDisposition.Inline = false;
        }

        public MailerAttachment(string attachFileName, 
                                string attachName, 
                                string attachContentType,
                                string attachContentID,
                                string atachTransferEncoding,
                                string attachNamePageCode,
                                bool attachIsInline)
            : base(attachFileName)
        {
            attachFileInfo = new FileInfo(attachFileName);

            ContentId = attachContentID;
            ContentType = String.IsNullOrEmpty(attachContentType) ? new ContentType() : new ContentType(attachContentType);
            Name = attachName;
            TransferEncoding = (TransferEncoding)Enum.Parse(typeof(TransferEncoding), atachTransferEncoding);
            NamePageCode = Convert.ToInt32(attachNamePageCode);

            ContentDisposition.FileName = attachName;
            ContentDisposition.DispositionType = attachIsInline ? DispositionTypeNames.Inline : DispositionTypeNames.Attachment;
            ContentDisposition.Inline = attachIsInline;
        }
        #endregion

        #region Private methods
        private void LoadFromAttachment(MailerAttachment attach)
        {
            ContentId = attach.ContentId;
            ContentType = attach.ContentType;
            Name = attach.Name;
            TransferEncoding = attach.TransferEncoding;
            NamePageCode = attach.NamePageCode;

            ContentDisposition.FileName = attach.ContentDisposition.FileName;
            ContentDisposition.DispositionType = attach.ContentDisposition.DispositionType;
            ContentDisposition.Inline = attach.ContentDisposition.Inline;
        }

        private void LoadFromDataRow(DataRowView rowAttach)
        {
            string contentType = ObjectConvert.ConvertToString(rowAttach["ContentType"], String.Empty);
            string attachName = ObjectConvert.ConvertToString(rowAttach["Name"], String.Empty);

            string contentID = ObjectConvert.ConvertToString(rowAttach["ContentID"], String.Empty);
            if (!String.IsNullOrEmpty(contentID))
                ContentId = contentID;

            ContentType = String.IsNullOrEmpty(contentType) ? new ContentType() : new ContentType(contentType);
            Name = attachName;
            TransferEncoding = (TransferEncoding)ObjectConvert.ConvertToInt(rowAttach["TransferEncoding"], Convert.ToInt32(TransferEncoding.Base64));
            NameEncoding = Encoding.GetEncoding(ObjectConvert.ConvertToInt(rowAttach["NamePageCode"], 65001));
            ID = ObjectConvert.ConvertToInt(rowAttach["ID"], 0);

            ContentDisposition.FileName = attachName;
            string dispositionType = ObjectConvert.ConvertToString(rowAttach["DispositionType"], String.Empty);
            if (dispositionType == DispositionTypeNames.Inline)
                ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            else if (dispositionType == DispositionTypeNames.Attachment)
                ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
            ContentDisposition.Inline = dispositionType == DispositionTypeNames.Inline;
        }

        private void LoadFromMimeEntity(MimeEntity mimeEntity)
        {
            string contentType = mimeEntity.ContentTypeString;
            string contentTypeName = mimeEntity.ContentType_Name;
            string contentTypeFileName = mimeEntity.ContentDisposition_FileName;
            if (String.IsNullOrEmpty(contentTypeName))
                contentTypeName = contentTypeFileName;
            else if (String.IsNullOrEmpty(contentTypeFileName))
                contentTypeFileName = contentTypeName;

            ContentType = String.IsNullOrEmpty(contentType) ? new ContentType() : new ContentType(contentType);
            Name = String.IsNullOrEmpty(contentTypeName) ? String.Empty : contentTypeName;
            this.NamePageCode = 65001;
            this.TransferEncoding = TransferEncoding.Base64;

            if (!String.IsNullOrEmpty(mimeEntity.ContentID))
                this.ContentId = mimeEntity.ContentID.TrimStart('<').TrimEnd('>');

            this.ContentDisposition.FileName = String.IsNullOrEmpty(contentTypeFileName) ? String.Empty : contentTypeFileName;
            this.ContentDisposition.Inline = mimeEntity.ContentDisposition == ContentDisposition_enum.Inline || 
                                                (!String.IsNullOrEmpty(mimeEntity.ContentID) && mimeEntity.ContentDisposition != ContentDisposition_enum.Attachment);
            if (mimeEntity.ContentDisposition == ContentDisposition_enum.Inline ||
                !String.IsNullOrEmpty(mimeEntity.ContentID) && mimeEntity.ContentDisposition != ContentDisposition_enum.Attachment)
                ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            else if (mimeEntity.ContentDisposition == ContentDisposition_enum.Attachment)
                ContentDisposition.DispositionType = DispositionTypeNames.Attachment;
        }
        #endregion
    }

    [XmlRoot("MailObject")]
    public class MailObject : BaseObject
    {
        #region Private fields
        private int order = -1;
        private int objectID = -1;
        private MailObjectType objectType = MailObjectType.Mortgage;
        private Mailer parentMailer = null;
        #endregion

        #region Properties
        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

        public int MailID
        {
            get
            {
                return parentMailer.ID;
            }
            set
            {
            }
        }

        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        public int ObjectID
        {
            get
            {
                return objectID;
            }
            set
            {
                objectID = value;
            }
        }

        public int ObjectTypeID
        {
            get
            {
                return Convert.ToInt32(objectType);
            }
            set
            {
                objectType = (MailObjectType)value;
            }
        }

        [XmlIgnore]
        public Mailer ParentMailer
        {
            get
            {
                return parentMailer;
            }
            set
            {
                parentMailer = value ?? new Mailer();
            }
        }

        [XmlIgnore]
        public MailObjectType ObjectType
        {
            get
            {
                return objectType;
            }
            set
            {
                objectType = value;
            }
        }
        #endregion

        #region Constructors
        public MailObject()
        {
            parentMailer = new Mailer();
        }

        public MailObject(Mailer pMailer, int objID, MailObjectType objType, int ord)
        {
            parentMailer = pMailer;
            ObjectID = objID;
            objectType = objType;
            Order = ord;
        }

        public MailObject(Mailer pMailer, DataRowView mailObjectRow)
        {
            parentMailer = pMailer;
            LoadFromDataRow(mailObjectRow);
        }
        #endregion

        #region Methods
        private void LoadFromDataRow(DataRowView mailObjectRow)
        {
            ID = ConvertToInt(mailObjectRow["ID"], 0);
            ObjectID = ConvertToInt(mailObjectRow["ObjectID"], 0);
            ObjectTypeID = ConvertToInt(mailObjectRow["ObjectTypeID"], 0);
            Order = ConvertToInt(mailObjectRow["Order"], -1);
        }
        #endregion
    }
}

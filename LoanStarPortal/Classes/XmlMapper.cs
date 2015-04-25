using System;
using System.Data;
using System.Net;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.IO;


namespace LoanStar.Common
{
    public class DocumentCompany : BaseObject
    {
        #region constants
        public const int APPLICATIONPACKAGETYPE = 1;
        public const int CLOSINGPACKAGETYPE = 2;
        private const string LOAD = "LoadDocumentCompanyById";
        private const string GETLIST = "GetDocumentCompanyList";
        #endregion

        #region fields
        private string name;
        private string appPackageXmlFile;
        private string closingPackageXmlFile;
        private string appPackageXsdFile;
        private string closingPackageXsdFile;
        private string validationXPath;
        private string postURL;
        #endregion

        #region properties
        public string Name
        {
            get { return name; }
        }
        //public string AppPackageXmlFile
        //{
        //    get { return appPackageXmlFile; }
        //}
        //public string ClosingPackageXmlFile
        //{
        //    get { return closingPackageXmlFile; }
        //}
        //public string AppPackageXsdFile
        //{
        //    get { return appPackageXsdFile; }
        //}
        //public string ClosingPackageXsdFile
        //{
        //    get { return closingPackageXsdFile; }
        //}
        public string ValidationXPath
        {
            get { return validationXPath; }
        }
        public string PostURL
        {
            get { return postURL; }
        }
        #endregion

        #region constructor
        public DocumentCompany(int id)
        {
            ID = id;
            if(id>0)
            {
                LoadById();
            }
        }
        #endregion

        #region methods
        private void LoadById()
        {
            DataView dv = db.GetDataView(LOAD, ID);
            if((dv!=null)&&(dv.Count==1))
            {
                name = dv[0]["Name"].ToString();
                appPackageXmlFile = dv[0]["appPackageXmlFile"].ToString();
                closingPackageXmlFile = dv[0]["closingPackageXmlFile"].ToString();
                appPackageXsdFile = dv[0]["appPackageXsdFile"].ToString();
                closingPackageXsdFile = dv[0]["closingPackageXsdFile"].ToString();
                validationXPath = dv[0]["ValidationXPath"].ToString();
                postURL = dv[0]["PostURL"].ToString();
            }
        }

        public static DataView GetCompanyList()
        {
            return db.GetDataView(GETLIST);
        }
        public string GetXmlFile(int packageType)
        {
            string res = "";
            switch (packageType)
            {
                case APPLICATIONPACKAGETYPE:
                    res = appPackageXmlFile;
                    break;
                case CLOSINGPACKAGETYPE:
                    res = closingPackageXmlFile;
                    break;
            }
            return res;
        }
        public string GetXsdFile(int packageType)
        {
            string res = "";
            switch (packageType)
            {
                case APPLICATIONPACKAGETYPE:
                    res = appPackageXsdFile;
                    break;
                case CLOSINGPACKAGETYPE:
                    res = closingPackageXsdFile;
                    break;
            }
            return res;
        }
        #endregion

    }
    public class DocumentRequestor :BaseObject
    {

        #region fields
        private const string ERREMPTYSTREAM = "Input file is null";
        private const int REQUESTRESULTSUCCESS = 0;
        private const int REQUESTRESULTFAILURE = 1;
        private const int REQUESTRESULTMISSINGORDERNUMBER = 2;
        private const int REQUESTRESULTWRONGXML = 3;
        private const int STATUSPENDINGID = 1;
        private const string SAVE = "SaveVendorDocumentOrder";
        private const string GETORDERLIST = "GetMortgageDocumentOrderList";
        private const string GETPDF = "GetPdf";
        #endregion

        #region fields
        private readonly string urlToPost;
        private string errorMessage;
        private string orderNumber;
        private string xmlSubmitted;
        private int requestResult=-1;
        private string requestError;
        private readonly int userId;
        private readonly int mortgageId;
        private readonly int documentCompanyId;
        private readonly int documentTypeId;
        #endregion

        #region properties
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        public string RequestError
        {
            get { return requestError; }
        }
        #endregion

        #region constructor
        public DocumentRequestor(string url_,int userId_, int mortgageId_, int documentCompanyId_, int documentTypeId_)
        {
            ID = -1;
            urlToPost = url_;
            userId = userId_;
            mortgageId = mortgageId_;
            documentCompanyId = documentCompanyId_;
            documentTypeId = documentTypeId_;
        }
        #endregion

        #region methods
        public bool PostData(StreamReader dataStream)
        {
            orderNumber = String.Empty;
            if(dataStream==null)
            {
                errorMessage = ERREMPTYSTREAM;
                return false;
            }
            xmlSubmitted = dataStream.ReadToEnd();
            dataStream.Close();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(xmlSubmitted);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = bytesToSend.Length;
            request.ContentType = "text/xml";
            request.Credentials = CredentialCache.DefaultCredentials;
            Stream requestStream = null;
            try
            {
                requestStream = request.GetRequestStream();
                requestStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (requestStream!=null) requestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            using(StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                ParseResponse(sr.ReadToEnd());
            }
            bool res = requestResult == REQUESTRESULTSUCCESS;
            if(res)
            {
                res = SaveResults();
            }
            return res;
        }
        private void ParseResponse(string response)
        {
            XmlDocument xmlResult = new XmlDocument();
            try
            {
                xmlResult.LoadXml(response);
                XmlNode node = xmlResult.SelectSingleNode("//response/status");
                if(node!=null)
                {
                    if(node.InnerText == "success")
                    {
                        requestResult = REQUESTRESULTSUCCESS;
                        XmlNode n = xmlResult.SelectSingleNode("//response/OrderNumber");
                        
                        if(n!=null)
                        {
                            orderNumber = n.InnerText;
                        }
                        if(String.IsNullOrEmpty(orderNumber))
                        {
                            requestResult = REQUESTRESULTMISSINGORDERNUMBER;
                        }
                    }
                    else
                    {
                        requestResult = REQUESTRESULTFAILURE;
                        XmlNode n = xmlResult.SelectSingleNode("//response/message");
                        if(n!=null)
                        {
                            requestError = n.InnerText;
                        }
                        else
                        {
                            requestError = "n/a";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                requestError = ex.Message;
                requestResult = REQUESTRESULTWRONGXML;
            }
        }
        private bool SaveResults()
        {
            return db.ExecuteScalarInt(SAVE
                   , orderNumber
                   , userId
                   , mortgageId
                   , documentCompanyId
                   , documentTypeId
                   , STATUSPENDINGID
                   , xmlSubmitted
                   ) > 0;
        }

        public static DataView GetOrders(int mortgageId, int packageType)
        {
            return db.GetDataView(GETORDERLIST, mortgageId, packageType);
        }
        private static byte[] GetPdfFile(int id)
        {
            return GetPdf(id);
        }
        public static byte[] GetPdf(int id)
        {
            DataView dv = db.GetDataView(GETPDF, id);
            if((dv == null)||(dv.Count==0))
            {
                return null;
            }
            int length = int.Parse(dv[0]["filesize"].ToString());
            if(length>1)
            {
                byte[] data = (byte[])dv[0]["RequestedDocument"]; 
                return data;
            }
            return null;
        }
        public static string SavePdf(int id, string folder)
        {
            string res = String.Empty;
            byte[] file = GetPdfFile(id);
            if(file!= null)
            {
                byte[] b = Convert.FromBase64String(Encoding.UTF8.GetString(file));
                FileStream fs = null;
                BinaryWriter sr = null;
                string fileName = Guid.NewGuid() + ".pdf";
                try
                {
                    fs = new FileStream(Path.Combine(folder, fileName), FileMode.CreateNew, FileAccess.Write);
                    sr = new BinaryWriter(fs);
                    sr.Write(b);
                    res = fileName;
                }
                catch
                {
                }
                finally
                {
                    if (sr!=null) sr.Close();
                    if(fs!=null) fs.Close();
                }
            }
            return res;
        }
        #endregion
    }
    public class XmlMapper
    {
        #region constants
        private const string PROPERTYATTRIBUTE = "property";
        private const string TABLEATTRIBUTE = "table";
        private const string FIELDATTRIBUTE = "field";

        private const string NULLVALUE = "";
        #endregion

        #region fields
        
        private XmlDocument errDoc;
        private XmlNode errRoot;
        private XmlDocument undefinedDoc;
        private XmlNode undefinedRoot;

        private XmlDocument validationDoc;
        private XmlNode validationRoot;

        private MortgageProfile mp;
        private int errCount;
        private int validationErrCount;

        private int undefinedCount;
        private bool isCalculatorValid;
        #endregion

        #region events/delegates
        public event DataNeeded OnDataNeeded;
        public delegate DataView DataNeeded(string dictionaryName);
        #endregion

        #region properties
        public XmlDocument ErrorXml
        {
            get { return errDoc; }
        }
        public XmlDocument UndefinedXml
        {
            get { return undefinedDoc; }
        }
        public XmlDocument ValidationErrorXml
        {
            get { return validationDoc; }
        }
        public int ErrorCount
        {
            get { return errCount; }
        }
        public int UndefinedCount
        {
            get { return undefinedCount; }
        }
        public int ValidationErrorCount
        {
            get { return validationErrCount; }
        }
        #endregion

        #region methods
        public XmlDocument Map(MortgageProfile mp_, XmlDocument input, bool isValidationNeeded, string validationRootPath, XmlSchema schema)
        {
            mp = mp_;
            InitCalculator();
            XmlDocument doc = new XmlDocument();
            XmlNode rootNode = doc.CreateNode(input.DocumentElement.NodeType, input.DocumentElement.Name, input.DocumentElement.NamespaceURI);
            doc.AppendChild(rootNode);
            errDoc = new XmlDocument();
            errRoot = errDoc.CreateNode(XmlNodeType.Element, "root", "");
            errDoc.AppendChild(errRoot);
            errCount = 0;
            undefinedDoc = new XmlDocument();
            undefinedRoot = undefinedDoc.CreateNode(XmlNodeType.Element, "root", "");
            undefinedDoc.AppendChild(undefinedRoot);
            undefinedCount = 0;

            validationDoc = new XmlDocument();
            validationRoot = validationDoc.CreateNode(XmlNodeType.Element, "root", "");
            validationDoc.AppendChild(validationRoot);
            validationErrCount = 0;

            for (int i = 0; i < input.DocumentElement.ChildNodes.Count; i++)
            {
                XmlNode sourceNode = input.DocumentElement.ChildNodes[i];
                XmlNode destNode = doc.CreateNode(sourceNode.NodeType,sourceNode.Name,sourceNode.NamespaceURI);
                rootNode.AppendChild(destNode);
                MapNode(sourceNode, destNode, doc);
            }
            if (isValidationNeeded)
            {
                XmlNode nodeToValidate;
                if(String.IsNullOrEmpty(validationRootPath))
                {
                    nodeToValidate = doc.DocumentElement;
                }
                else
                {
                    nodeToValidate = doc.SelectSingleNode(validationRootPath);
                }
                if(nodeToValidate!=null)
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    if(schema!=null)
                    {
                        settings.Schemas.Add(schema);
                        settings.ValidationType = ValidationType.Schema;
                        settings.ValidationEventHandler += XmlValidationEventHandler;
                        
                        XmlReader xmlReader = XmlReader.Create(new XmlNodeReader(nodeToValidate), settings);
                        while (xmlReader.Read())
                        {
                        }
                    }
                }
            }
            return doc;
        }
        private void XmlValidationEventHandler(object sender, ValidationEventArgs e)
        {
            validationErrCount++;
            XmlNode node = validationDoc.CreateNode(XmlNodeType.Element, "undefined", "");
            XmlAttribute a = validationDoc.CreateAttribute("errortype");
            string errorType = String.Empty;
            if(e.Severity == XmlSeverityType.Error)
            {
                errorType = "Error";
            }
            else if (e.Severity == XmlSeverityType.Warning)
            {
                errorType = "Warning";
            }
            a.Value = errorType;
            node.Attributes.Append(a);
            a = validationDoc.CreateAttribute("message");
            a.Value = e.Message;
            node.Attributes.Append(a);
            validationRoot.AppendChild(node);
        }
        private void MapNode(XmlNode sourceNode, XmlNode destNode, XmlDocument parent)
        {
            if (sourceNode.HasChildNodes)
            {
                for (int i = 0; i < sourceNode.ChildNodes.Count; i++)
                {
                    XmlNode sn = sourceNode.ChildNodes[i];
                    XmlNode dn = parent.CreateNode(sn.NodeType, sn.Name, sn.NamespaceURI);
                    destNode.AppendChild(dn);
                    MapNode(sn, dn, parent);
                }
            }
            else
            {
                if (sourceNode.NodeType == XmlNodeType.Element)
                {
                    XmlNode n = sourceNode.Attributes.GetNamedItem(PROPERTYATTRIBUTE);
                    if (n != null)
                    {
                        string value;
                        if (GetValue(sourceNode, n.Value, out value))
                        {
                            XmlNode t = sourceNode.Attributes.GetNamedItem(TABLEATTRIBUTE);
                            XmlNode f = sourceNode.Attributes.GetNamedItem(FIELDATTRIBUTE);
                            bool isDictionary = false;
                            string dictionaryValue=String.Empty;
                            if ((t != null) && (f != null))
                            {
                                isDictionary = IsDictionaryMapping(t.Value, f.Value, value, out dictionaryValue);
                            }
                            if(isDictionary)
                            {
                                destNode.InnerText = dictionaryValue;
                            }
                            else
                            {
                                destNode.InnerText = value;
                            }
                        }
                    }
                }
                else
                {
                    destNode.Value = sourceNode.Value;
                }
            }
        }
        private bool IsDictionaryMapping(string tableName, string fieldName,string value, out string dictionaryValue)
        {
            dictionaryValue = String.Empty;
            bool res = false;
            if(!String.IsNullOrEmpty(tableName)&&!String.IsNullOrEmpty(fieldName))
            {
                try
                {
                    int id = int.Parse(value);
                    if(id<1)
                    {
                        return true;
                    }
                }
                catch
                {
                    return true;
                }
                if(OnDataNeeded!=null)
                {
                    DataView dv = OnDataNeeded(tableName);
                    if (dv!=null)
                    {
                        dv.RowFilter = String.Format("id={0}", value);
                        if(dv.Count==1)
                        {
                            dictionaryValue = dv[0][fieldName].ToString().Trim();
                            res = true;
                        }
                    }
                }
            }
            return res;
        }
        private bool GetValue(XmlNode node, string fullPropertyName, out string result)
        {
            bool res = false;
            result = String.Empty;
            if (fullPropertyName == "?")
            {
                AddUndefined(node);
                return res;
            }
            string[] names = fullPropertyName.Split('.');
            try
            {
                Object o = mp;
                string propertyName = names[names.Length - 1];
                for (int i = 0; i < names.Length - 1; i++)
                {
                    string objectName;
                    int index;
                    bool isIndexer = CheckIndexer(names[i], out objectName, out index);
                    o = o.GetType().GetProperty(objectName).GetValue(o, null);
                    if (isIndexer)
                    {
                        IList list = o as IList;
                        if (list == null)
                        {
                            AddError(node, fullPropertyName, "Object is not IList");
                        }
                        else 
                        {
                            if (index < list.Count)
                            {
                                o = list[index];
                            }
                            else
                            {
                                return res;
                            }
                        }
                    }
                }
                if (o == null)
                {
                    AddError(node, fullPropertyName, "Object is null");
                }
                else
                {

                    PropertyInfo pi = o.GetType().GetProperty(propertyName);
                    Object val = pi.GetValue(o, null);
                    Type t = pi.PropertyType;
                    if (BaseObject.IsNullableType(t))
                    {
                        if (val == null)
                        {
                            result = NULLVALUE;
                        }
                        else
                        {
                            result = GetValue(val);
                        }
                    }
                    else
                    {
                        result = GetValue(val);
                    }
                    res = true;
                }
            }            
            catch (Exception ex)
            {
                AddError(node, fullPropertyName, ex.Message);
            }
            return res;
        }
        private static string GetValue(Object val)
        {
            string res;
            if(val is DateTime)
            {
                res = ((DateTime) val).ToShortDateString();
            }
            else
            {
                res = val.ToString();
            }
            return res.Trim();
        }
        public static bool CheckIndexer(string name, out string objectName,out int index)
        {
            bool res = false;
            objectName = name;
            string tmp = objectName;
            index = -1;
            if (tmp.EndsWith("]"))
            {
                int i = tmp.LastIndexOf("[");
                if (i > 0)
                {
                    tmp = tmp.Substring(i+1, tmp.Length-i-2);
                    try
                    {
                        index = int.Parse(tmp);
                        res = true;
                        objectName = objectName.Substring(0, i);
                    }
                    catch { }
                }
            }
            return res;
        }
        private void AddError(XmlNode node, string  fullPropertyName, string message)
        {
            if (fullPropertyName != "?")
            {
                errCount++;
                XmlNode errNode = errDoc.CreateNode(XmlNodeType.Element, "error", "");
                XmlAttribute a = errDoc.CreateAttribute("nodeName");
                a.Value = node.Name;
                errNode.Attributes.Append(a);
                a = errDoc.CreateAttribute("propertyName");
                a.Value = fullPropertyName;
                errNode.Attributes.Append(a);
                a = errDoc.CreateAttribute("message");
                a.Value = message;
                errNode.Attributes.Append(a);
                errRoot.AppendChild(errNode);
            }
        }
        private void AddUndefined(XmlNode node)
        {
            undefinedCount++;
            XmlNode undefinedNode = undefinedDoc.CreateNode(XmlNodeType.Element, "undefined", "");
            XmlAttribute a = undefinedDoc.CreateAttribute("nodeName");
            a.Value = node.Name;
            undefinedNode.Attributes.Append(a);
            undefinedNode.Attributes.Append(a);
            undefinedRoot.AppendChild(undefinedNode);
        }
        private void InitCalculator()
        {
            isCalculatorValid = false;
            if (mp.ProductID > 0)
            {
                AdvCalculator calculator = new AdvCalculator(mp);
                string err = calculator.ValidateFirst();
                if (String.IsNullOrEmpty(err))
                {
                    err = calculator.Validate();
                    if (String.IsNullOrEmpty(err))
                    {
                        isCalculatorValid = calculator.CalculatedForSelectedProduct();
                        if (isCalculatorValid)
                        {
                            mp.Calculator = calculator;
                        }
                    }
                }
            }
        }
        #endregion
    }

    public class XmlMapperWrapper
    {
        #region constants
        private const string RESULTFILENAME = "Baydocs/{0}_out_{1}.xml";
        private const string ERRFILENAME = "Baydocs/{0}_err_{1}.xml";
        private const string UNDEFFILENAME = "Baydocs/{0}_undef_{1}.xml";
        private const string VALIDATIONERRORFILENAME = "Baydocs/{0}_validation_err_{1}.xml";
        private const string BAYDOCFOLDER = Constants.STORAGEFOLDER;
        #endregion

        #region fields
        private readonly System.Web.UI.Page page;
        private readonly bool saveToFile;
        #endregion

        #region constructor
        public XmlMapperWrapper(System.Web.UI.Page page_, bool saveToFile_)
        {
            page = page_;
            saveToFile = saveToFile_;
        }
        #endregion

        #region methods
        public bool Map(MortgageProfile mp, DocumentCompany company, int userId, int packageType)
        {
            string sourceFile = page.Server.MapPath(Path.Combine(BAYDOCFOLDER, company.GetXmlFile(packageType)));
            XmlDocument doc = new XmlDocument();
            doc.Load(sourceFile);
            XmlMapper mapper = new XmlMapper();
            mapper.OnDataNeeded += ((AppPage) page).GetDictionary;
            string xsdFile = company.GetXsdFile(packageType);
            XmlSchema schema = null;
            if(!String.IsNullOrEmpty(xsdFile))
            {
                XmlTextReader reader = new XmlTextReader(page.Server.MapPath(Path.Combine(BAYDOCFOLDER, xsdFile)));
                schema = XmlSchema.Read(reader, null);
            }
            XmlDocument result = mapper.Map(mp, doc, true, company.ValidationXPath, schema);
            if(saveToFile)
            {
                string resultFile = page.Server.MapPath(Path.Combine(BAYDOCFOLDER, String.Format(RESULTFILENAME,company.Name,mp.ID)));
                result.Save(resultFile);
                string errorFileName = page.Server.MapPath(Path.Combine(BAYDOCFOLDER, String.Format(ERRFILENAME, company.Name, mp.ID)));
                if (File.Exists(errorFileName))
                {
                    File.Delete(errorFileName);
                }
                if (mapper.ErrorCount > 0)
                {
                    XmlDocument errDoc = mapper.ErrorXml;
                    errDoc.Save(errorFileName);
                }
                string undefinedFileName = page.Server.MapPath(Path.Combine(BAYDOCFOLDER, String.Format(UNDEFFILENAME, company.Name, mp.ID)));
                if (File.Exists(undefinedFileName))
                {
                    File.Delete(undefinedFileName);
                }
                if (mapper.UndefinedCount > 0)
                {
                    XmlDocument undefDoc = mapper.UndefinedXml;
                    undefDoc.Save(undefinedFileName);
                }
                string validationErrorFileName = page.Server.MapPath(Path.Combine(BAYDOCFOLDER, String.Format(VALIDATIONERRORFILENAME, company.Name, mp.ID)));
                if (File.Exists(validationErrorFileName))
                {
                    File.Delete(validationErrorFileName);
                }
                if (mapper.ValidationErrorCount > 0)
                {
                    XmlDocument validationErrDoc = mapper.ValidationErrorXml;
                    validationErrDoc.Save(validationErrorFileName);
                }
            }
            DocumentRequestor docRequest = new DocumentRequestor(company.PostURL, userId, mp.ID, company.ID, packageType);
            MemoryStream ms = new MemoryStream();
            result.Save(ms);
            StreamReader sr = new StreamReader(ms);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            bool res = docRequest.PostData(sr);
            return res;
        }
        #endregion
    }

    public class RMOInfo
    {
        private RMOInfoObject info;
        public RMOInfoObject GetInfo()
        {
            info= new RMOInfoObject();
            Type t = typeof(MortgageProfile);
            AddObject(t);
            return info;
        }
        private void AddObject(Type t)
        {
            string objectName = t.Name;
            if(!info.ObjectsInfo.ContainsKey((objectName)))
            {
                info.AllObjects.Add(objectName);
                info.ObjectsInfo.Add(objectName,GetObjectInfo(t));
            }
        }
        private ObjectInfo GetObjectInfo(Type t)
        {
            ObjectInfo res = new ObjectInfo();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public );
            for (int i = 0; i < properties.Length; i++)
            {
                string propertyName = properties[i].Name;
                res.AllProperties.Add(propertyName);
                RMOPropertyInfo rmoinfo = GetPropertyInfo(properties[i]);
                res.Info.Add(propertyName, rmoinfo);
                if (rmoinfo.IsRMOType)
                {
                    AddObject(rmoinfo.RealType);
                }
            }
            return res;
        }
        private static RMOPropertyInfo GetPropertyInfo(PropertyInfo pi)
        {
            PropertyInfo p = pi;
            RMOPropertyInfo res = new RMOPropertyInfo();
            res.Name = p.Name;
            Type t = p.PropertyType;
            res.IsGeneric = t.IsGenericType;
            res.IsNullable = BaseObject.IsNullableType(t);
            if (t.IsGenericType)
            {
                res.IsList = t.Name.StartsWith("List`");
                t = t.GetGenericArguments()[0];
            }
            if (t.IsClass)
            {
                res.IsRMOType = t.FullName.StartsWith("LoanStar.");
            }
            res.IsReadOnly = !pi.CanWrite;
            res.RealType = t;
            return res;
        }
    }
    public class RMOPropertyInfo
    {
        private bool isGeneric;
        private bool isRMOType = false;
        private bool isList = false;
        private bool isNullable = false;
        private bool isReadOnly = false;
        private bool isDbField = false;
        private string name;
        private string tableName;
        private string fieldName;
        private string fieldtype;
        private bool isDbNullable;
        private Type realType;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public bool IsGeneric
        {
            get { return isGeneric; }
            set { isGeneric = value; }
        }
        public bool IsRMOType
        {
            get { return isRMOType; }
            set { isRMOType = value; }
        }
        public bool IsList
        {
            get { return isList; }
            set { isList = value; }
        }
        public bool IsNullable
        {
            get { return isNullable; }
            set { isNullable = value; }
        }
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value; }
        }
        public bool IsDbField
        {
            get { return isDbField; }
            set { isDbField = value; }
        }
        public Type RealType
        {
            get { return realType; }
            set { realType = value; }
        }
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        public string Fieldtype
        {
            get { return fieldtype; }
            set { fieldtype = value; }
        }
        public bool IsDbNullable
        {
            get { return isDbNullable; }
            set { isDbNullable = value; }
        }

        public new string ToString()
        {
            string res = name;
            if(isList)
            {
                res += ",list of " + realType.Name;
            }
            else
            {
                res += ",type of " + realType.Name;
            }
            if (isNullable)
            {
                res += ",(nullable)";
            }
            if(isReadOnly)
            {
                res += ",(readonly)";
            }
            return res;
        }
    }
    public class ObjectInfo
    {
        private ArrayList allProperties;
        private Hashtable info;

        public ArrayList AllProperties
        {
            get
            {
                if (allProperties == null)
                {
                    allProperties = new ArrayList();
                }
                return allProperties;
            }
        }

        public Hashtable Info
        {
            get
            {
                if (info == null)
                {
                    info = new Hashtable();
                }
                return info;
            }
        }
    }
    public class RMOInfoObject
    {
        private ArrayList allObjects;
        private Hashtable objectsInfo;

        public ArrayList AllObjects
        {
            get
            {
                if(allObjects==null)
                {
                    allObjects = new ArrayList();
                }
                return allObjects;
            }
        }

        public Hashtable ObjectsInfo
        {
            get
            {
                if(objectsInfo==null)
                {
                    objectsInfo = new Hashtable();
                }
                return objectsInfo;
            }
        }
    }
}

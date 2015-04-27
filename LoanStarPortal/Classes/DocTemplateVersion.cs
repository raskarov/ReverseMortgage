using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using Aspose.Pdf;
//using Aspose.Pdf.Kit;
using Aspose.Words;
using Aspose.Words.Reporting;
using BossDev.CommonUtils;

namespace LoanStar.Common
{
    public enum DTVFormatType
    {
        Unknown = 0,
        Word    = 1,
        PDF     = 2
    }

    public class DocTemplateVersionLogger : Logger
    {
        private readonly bool needToLog = false;
        private readonly DocTemplateVersion dtv = null;

        public DocTemplateVersionLogger(DocTemplateVersion _dtv)
        {
            dtv = _dtv;
            needToLog = Convert.ToInt32(ConfigurationManager.AppSettings["LogDTVersions"]) != 0;
        }

        public void WriteHeader()
        {
            if (!needToLog)
                return;

            WriteLine("");
            WriteLine("=============================================================================================");
            WriteLine("DocTemplateVersion");
            WriteLine(String.Format("FileName={0}; FormatType={1}", dtv.FileName, dtv.DTVFormatType));
            WriteLine(String.Format("DTID={0}; DTVID={1}; Version={2}; IsCurrent={3}", dtv.DocTemplateID, dtv.ID, dtv.Version, dtv.IsCurrent));
            WriteLine("---------------------------------------------------------------------------------------------");
        }

        public void WriteEmptyLine()
        {
            if (!needToLog)
                return;

            WriteLine(String.Empty);
        }

        public void WriteOperation(string operationName)
        {
            if (!needToLog)
                return;

            WriteLine(String.Format("Operation : {0}", operationName));
            WriteLine(String.Empty);
        }

        public void WriteOperationCompleeted()
        {
            if (!needToLog)
                return;

            WriteLine("Operation compleeted");
        }

        public void WriteOperationLine(string content)
        {
            if (!needToLog)
                return;

            WriteLine("\t" + content);
        }

        public void WriteFieldsCount()
        {
            if (!needToLog)
                return;

            WriteLine(String.Format("\tFields count {0}", dtv.FieldsCount));
        }

        public void WriteFieldInfo(string dtvFieldName, string dtvFieldValue)
        {
            if (!needToLog)
                return;

            WriteLine(String.Format("\t\tFieldName={0}; FieldValue={1}", dtvFieldName, dtvFieldValue));
        }

        public override void WriteException(Exception ex)
        {
            if (!needToLog)
                return;

            base.WriteException(ex);
        }
    }

    public class DocTemplateVersion : BaseObject
    {
        /// <summary>
        /// Any method of DocTemplate class must throw custom exceptions of this type only
        /// </summary>
        public class DocTemplateVersionException : BaseObjectException
        {
            public DocTemplateVersionException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            public DocTemplateVersionException(string message)
                : base(message)
            {
            }

            public DocTemplateVersionException()
            {
            }
        }

        #region Private fields
        private DocTemplate docTemplate = null;
        private int docTemplateID;

        private string version = String.Empty;
        private DateTime uploadDate = DateTime.Now;
        private bool isCurrent;
        private DTVFormatType dtvFormatType = DTVFormatType.Unknown;

        private HttpPostedFile postedFileInfo = null;
        private string fileName = String.Empty;

        private readonly Hashtable docFieldHash = new Hashtable();
        private readonly ArrayList docFieldRemovableList = new ArrayList();
        private DataTable docFieldTable;

        private static readonly DatabaseAccess dbAccess = new DatabaseAccess(AppSettings.SqlConnectionString);

        private readonly DocTemplateVersionLogger log = null;
        private readonly Aspose.Words.License wordLicense = new Aspose.Words.License();
        private readonly Aspose.Pdf.License pdfLicense = new Aspose.Pdf.License();
        #endregion

        #region properties
        public override int ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
                foreach (DocTemplateField field in docFieldHash.Values)
                    field.DocTemplateVersionID = value;
            }
        }
        public int DocTemplateID
        {
            get 
            { 
                return docTemplateID; 
            }
            set 
            { 
                docTemplateID = value; 
            }
        }
        public DocTemplate DocTemplate
        {
            get
            {
                if (docTemplate == null || docTemplate.ID != DocTemplateID)
                    docTemplate = new DocTemplate(DocTemplateID);

                return docTemplate;
            }
        }
        public DTVFormatType DTVFormatType
        {
            get
            {
                return dtvFormatType;
            }
            set
            {
                dtvFormatType = value;
            }
        }
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set { uploadDate = value; }
        }
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }
        public int FieldsCount
        {
            get
            {
                return docFieldHash.Count;
            }
        }

        public DataView BoundDocFieldView
        {
            get
            {
                if (docFieldTable == null)
                    return new DataView();

                DataView boundDocFieldView = new DataView(docFieldTable);
                boundDocFieldView.Sort = "FGID, FGIndID, GroupName, MPFiledName, DTVFieldName";
                boundDocFieldView.RowFilter = "FieldID > 0";

                return boundDocFieldView;
            }
        }
        public DataView UnBoundDocFieldView
        {
            get
            {
                if (docFieldTable == null)
                    return new DataView();

                DataView unBoundDocFieldView = new DataView(docFieldTable);
                unBoundDocFieldView.Sort = "DTVFieldName";
                unBoundDocFieldView.RowFilter = "FieldID = 0";

                return unBoundDocFieldView;
            }
        }

        private string DTVCreatedFilePath
        {
            get
            {
                if (postedFileInfo == null)
                    return String.Empty;

                string fname = PostedFileNameWithoutExt;
                string ext = PostedFileExt;
                string newFileName = fname + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "." + ext;

                return Path.Combine(DocTemplateFolder, newFileName);
            }
        }

        private string PostedFileNameWithoutExt
        {
            get
            {
                return Path.GetFileNameWithoutExtension(postedFileInfo.FileName);
            }
        }
        private string PostedFileExt
        {
            get
            {
                return Path.GetExtension(postedFileInfo.FileName).Replace(".", null);
            }
        }
        #endregion

        #region Constructors
        public DocTemplateVersion() : this(0)
	    {
	    }
        public DocTemplateVersion(int id)
        {
            pdfLicense.SetLicense("Aspose.Custom.lic");
            wordLicense.SetLicense("Aspose.Custom.lic");
            log = new DocTemplateVersionLogger(this);

            ID = id;
            if (id <= 0)
                return;

            DataSet ds = dbAccess.GetDataSet("GetDocTemplateVersionById", ID);
            if ((ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                LoadFromDataRow(ds.Tables[0].DefaultView[0]);

                if (ds.Tables.Count > 1)
                {
                    Hashtable fieldHash = DocTemplateField.GetDocFieldHash( ds.Tables[1].DefaultView );
                    SetDocFieldHash(fieldHash);
                    docFieldTable = ds.Tables[1];
                }
            }
        }
        #endregion

        #region PDF functions
        public MemoryStream AppendToPDFStream(MortgageProfile mp, Hashtable mpFieldHash, MemoryStream streamToAppend)
        {
            log.WriteHeader();

            byte[] pdfMemoryBuffer;
            switch (DTVFormatType)
            {
                case DTVFormatType.PDF:
                    pdfMemoryBuffer = CreateFilledPDFDocument(mp, mpFieldHash);
                    break;
                case DTVFormatType.Word:
                    {
                        byte[] docMemoryBuffer = CreateFilledWordDocument(mp, mpFieldHash);
                        pdfMemoryBuffer = SaveDocAsPDF(docMemoryBuffer);
                    }
                    break;
                default:
                    throw new DocTemplateVersionException("Unrecognized format for document template");
            }

            if (streamToAppend == null)
                return new MemoryStream(pdfMemoryBuffer);
            else
                return AppendPDF(streamToAppend, pdfMemoryBuffer);
        }

        private static Hashtable GetDTVPDFFields(HttpPostedFile postedFI)
        {
            Hashtable ht = new Hashtable();
/*            Stream PdfStream = null;
            FileStream tmpFile = null;

            try
            {
                PdfStream = postedFI.InputStream;
                tmpFile = new FileStream(Path.Combine(DocTemplateVersion.DocTemplateFolder, "test.pdf"), FileMode.Create);
                Form form = new Form(PdfStream, tmpFile);

                string[] fields = form.FieldsNames;
                foreach (string fieldName in fields)
                {
                    DTVFieldType dtvFieldType = form.GetFieldType(fieldName) == Aspose.Pdf.Kit.FieldType.CheckBox ?
                        dtvFieldType = DTVFieldType.FieldBool : dtvFieldType = DTVFieldType.FieldText;

                    DocTemplateField docTemplateField = new DocTemplateField(fieldName, dtvFieldType);
                    ht.Add(fieldName, docTemplateField);
                }
            }
            finally
            {
                if (tmpFile != null)
                    tmpFile.Close();
            }*///PDF

            return ht;
        }

        private static MemoryStream AppendPDF(MemoryStream dtResMemoryStream, byte[] dtMemoryBuffer)
        {
/*            log.WriteOperation("AppendPDF");

            MemoryStream dtOutMemoryStream = null;
            MemoryStream dtMemoryStream = null;
            try
            {
                dtMemoryStream = new MemoryStream(dtMemoryBuffer);
                PdfFileInfo pdfFileInfo = new PdfFileInfo(dtMemoryStream);
                int pageCount = pdfFileInfo.NumberofPages;
                log.WriteOperationLine(String.Format("Number of pages is {0}", pageCount));

                dtMemoryStream.Close();
                dtMemoryStream = new MemoryStream(dtMemoryBuffer);
                dtOutMemoryStream = new MemoryStream();
                PdfFileEditor pdfEditor = new PdfFileEditor();

                bool bRes = pdfEditor.Append(dtResMemoryStream, dtMemoryStream, 0, pageCount - 1, dtOutMemoryStream);
                if (!bRes)
                {
                    dtOutMemoryStream.Close();
                    dtOutMemoryStream = new MemoryStream(dtResMemoryStream.ToArray());
                }
                log.WriteOperationLine(String.Format("Appended block length is {0}", dtResMemoryStream.Length));
                log.WriteOperationLine(String.Format("Appending block length is {0}", dtMemoryStream.Length));
                log.WriteOperationLine(String.Format("Result block length is {0}", dtOutMemoryStream.Length));
            }
            catch (Exception ex)
            {
                log.WriteException(ex);
                throw ex;
            }
            finally
            {
                if (dtMemoryStream != null)
                    dtMemoryStream.Close();
            }

            log.WriteOperationCompleeted();
            return dtOutMemoryStream;*///PDF
            return null;
        }

        private static byte[] CreateFilledPDFDocument(MortgageProfile mp, Hashtable mpFieldHash)
        {
/*            log.WriteOperation("CreateFilledPDFDocument");

            byte[] dtMemoryBuffer = null;
            Stream dtvStream = null;
            MemoryStream dtMemoryStream = null;
            try
            {
                dtvStream = GetDTVStream();
                dtMemoryStream = new MemoryStream();
                Form pdfForm = new Form(dtvStream, dtMemoryStream);

                foreach (DictionaryEntry dtFieldEntry in docFieldHash)
                {
                    DocTemplateField dtField = (DocTemplateField)dtFieldEntry.Value;

                    MortgageProfileField mpField = (MortgageProfileField)mpFieldHash[dtField.MPFieldID];
                    if (mpField == null || mpField.ID == 0)
                        continue;

                    mp.PopulateMPField(mpField, dtField);
                    if (mpField.FieldValue == null)
                        continue;

                    string dtvFieldValue = Convert.ToString(mpField.FieldValue);
                    string dtvFieldName = dtField.DTVFieldName;

                    pdfForm.FillField(dtvFieldName, dtvFieldValue);
                }

                pdfForm.FlattenAllFields();
                pdfForm.Save();

                dtMemoryBuffer = dtMemoryStream.ToArray();
            }
            finally
            {
                if (dtvStream != null)
                    dtvStream.Close();
                if (dtMemoryStream != null)
                    dtMemoryStream.Close();
            }

            log.WriteOperationCompleeted();
            return dtMemoryBuffer;*///PDF
            return null;
        }
        #endregion

        #region Word functions
        public MemoryStream AppendToWordStream(MortgageProfile mp, Hashtable mpFieldHash, MemoryStream streamToAppend)
        {
            log.WriteHeader();

            if (DTVFormatType != DTVFormatType.Word)
                throw new DocTemplateVersionException("PDF templates are not used in this version of mapping");

            byte[] docMemoryBuffer = CreateFilledWordDocument(mp, mpFieldHash);
            if (streamToAppend == null)
                return new MemoryStream(docMemoryBuffer);
            else
                return AppendWord(streamToAppend, docMemoryBuffer);
        }

        private static Hashtable GetDTVWordFields(HttpPostedFile postedFI)
        {
            Hashtable ht = new Hashtable();
            Document doc = new Document(postedFI.InputStream);

            ArrayList regionList = new ArrayList();
            string regionName = String.Empty;
            string[] fields = doc.MailMerge.GetFieldNames();
            foreach (string fieldName in fields)
            {
                int delimInd = fieldName.LastIndexOf(':');
                if (delimInd < 0)
                {
                    if (String.IsNullOrEmpty(fieldName))
                        throw new DocTemplateVersionException("Field name can not be empty");
                    else if (!String.IsNullOrEmpty(regionName) && fieldName.IndexOf(String.Format("{0}.", regionName)) != 0)
                        throw new DocTemplateVersionException(String.Format("Field {0} inside region {1} must contain Region name and point", fieldName, regionName));

                    DocTemplateField docTemplateField = new DocTemplateField(fieldName, DTVFieldType.FieldText, regionName);
                    ht[docTemplateField.Key] = docTemplateField;
                }
                else if (fieldName.ToUpper().Contains("TABLESTART"))
                {
                    string foundRegionName = fieldName.Substring(delimInd + 1);

                    if (!String.IsNullOrEmpty(regionName))
                        throw new DocTemplateVersionException(String.Format("Region can not be started inside another region {0}", regionName));
                    else if (String.IsNullOrEmpty(foundRegionName))
                        throw new DocTemplateVersionException("Region name near TABLESTART prefix can not be empty");
                    else if (regionList.Contains(foundRegionName.ToUpper()))
                        throw new DocTemplateVersionException(String.Format("Region {0} allready is used in region list", foundRegionName));

                    regionName = foundRegionName;
                    regionList.Add(foundRegionName.ToUpper());
                }
                else if (fieldName.ToUpper().Contains("TABLEEND"))
                {
                    string foundRegionName = fieldName.Substring(delimInd + 1);

                    if (String.IsNullOrEmpty(regionName))
                        throw new DocTemplateVersionException(String.Format("You need to close region {0}", regionName));
                    else if (String.IsNullOrEmpty(foundRegionName))
                        throw new DocTemplateVersionException("Region name near TABLEEND prefix can not be empty");
                    else if (String.Compare(regionName, foundRegionName, true) != 0)
                        throw new DocTemplateVersionException(String.Format("Region near TABLEEND prefix must be equal to region {0} near TABLESTART prefix", regionName));

                    regionName = String.Empty;
                }
                else
                    throw new DocTemplateVersionException("Incorrect field format of Doc Template Version");
            }

            return ht;
        }

        private byte[] CreateFilledWordDocument(MortgageProfile mp, IDictionary mpFieldHash)
        {
            log.WriteOperation("CreateFilledWordDocument");

            byte[] dtMemoryBuffer;
            Stream dtvStream = null;
            MemoryStream dtMemoryStream = null;
            try
            {
                dtvStream = GetDTVStream();
                dtMemoryStream = new MemoryStream();
                Document docForm = new Document(dtvStream);
                log.WriteOperationLine("Document object is created");
                log.WriteEmptyLine();
                log.WriteFieldsCount();

                Hashtable regionHash = new Hashtable();
                
                foreach (DocTemplateField dtField in docFieldHash.Values)
                {
                    ArrayList valueList = null;
                    MortgageProfileField mpField = (MortgageProfileField)mpFieldHash[dtField.MPFieldID];
                    if (mpField != null && mpField.ID > 0)
                    {
                        try
                        {
                            mp.PopulateMPField(mpField, dtField);
                            if (mpField.FieldValue != null)
                            {
                                if (mpField.FieldValue is ArrayList)
                                    valueList = (ArrayList)mpField.FieldValue;
                                else
                                {
                                    valueList = new ArrayList();
                                    valueList.Add(mpField.FieldValue);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            log.WriteLine(ex.Message); 
                        }
                    }

                    if (!regionHash.ContainsKey(dtField.RegionName))
                        regionHash[dtField.RegionName] = dtField.GetRegionTable();
                    DataTable regionTable = (DataTable)regionHash[dtField.RegionName];
                    
                    if (valueList == null || valueList.Count == 0)
                        regionTable.Columns.Add(dtField.DTVFieldName);
                    else
                    {
                        regionTable.Columns.Add(dtField.DTVFieldName, valueList[0].GetType());
                        for (int i = 0; i < valueList.Count; i++)
                        {
                            if (i < regionTable.Rows.Count)
                            {
                                DataRow foundRow = regionTable.Rows[i];
                                foundRow[dtField.DTVFieldName] = valueList[i] ?? DBNull.Value;
                            }
                            else
                            {
                                DataRow newRow = regionTable.NewRow();
                                newRow[dtField.DTVFieldName] = valueList[i] ?? DBNull.Value;
                                regionTable.Rows.Add(newRow);
                            }
                            log.WriteFieldInfo(dtField.DTVFieldName, valueList[i] == null ? String.Empty : valueList[i].ToString());
                        }
                    }
                }
                log.WriteEmptyLine();

                docForm.MailMerge.MergeField += HandleMergeWithCheckBox;
                foreach (DataTable regionTable in regionHash.Values)
                {
                    if (!String.IsNullOrEmpty(regionTable.TableName))
                        docForm.MailMerge.ExecuteWithRegions(regionTable);
                    else if (regionTable.Rows.Count > 0)
                        docForm.MailMerge.Execute(regionTable);
                }
                docForm.MailMerge.MergeField -= HandleMergeWithCheckBox;
                log.WriteOperationLine("MailMerge is compleeted");

                docForm.Save(dtMemoryStream, SaveFormat.Doc);
                dtMemoryBuffer = dtMemoryStream.ToArray();
               
            }
            catch (Exception ex)
            {
                log.WriteException(ex);
                throw;
            }
            finally
            {
                if (dtvStream != null)
                    dtvStream.Close();
                if (dtMemoryStream != null)
                    dtMemoryStream.Close();
            }

            log.WriteOperationCompleeted();
            return dtMemoryBuffer;
        }

        private MemoryStream AppendWord(Stream dtResMemoryStream, byte[] dtMemoryBuffer)
        {
            log.WriteOperation("AppendWord");

            MemoryStream dtOutMemoryStream;
            MemoryStream dtMemoryStream = null;
            try
            {
                dtMemoryStream = new MemoryStream(dtMemoryBuffer);
                Document dtSource = new Document(dtMemoryStream);
                Document dtDestination = new Document(dtResMemoryStream);

                foreach (Aspose.Words.Section srcSection in dtSource.Sections)
                {
                    Aspose.Words.Section importSection = (Aspose.Words.Section)dtDestination.ImportNode(srcSection, true);
                    dtDestination.Sections.Add(importSection);
                }

                dtOutMemoryStream = new MemoryStream();
                dtDestination.Save(dtOutMemoryStream, SaveFormat.Doc);
            }
            catch (Exception ex)
            {
                log.WriteException(ex);
                throw;
            }
            finally
            {
                if (dtMemoryStream != null)
                    dtMemoryStream.Close();
            }

            log.WriteOperationCompleeted();
            return dtOutMemoryStream;
        }

        private static void HandleMergeWithCheckBox(object sender, MergeFieldEventArgs e)
        {
            try
            {
                if (e != null && e.FieldValue != null && e.FieldValue.GetType() != typeof(Boolean))
                    return;

                DocumentBuilder docBuilder = new DocumentBuilder(e.Document);
                docBuilder.MoveToMergeField(e.FieldName);
                docBuilder.InsertCheckBox(e.DocumentFieldName, (Boolean)e.FieldValue, 0);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion

        #region Private methods
        private void CalculateFieldAutoMapping(DatabaseAccess _dbAccess)
        {
            _dbAccess.Execute("CalculateFieldAutoMapping", ID);
        }

        private Stream GetDTVStream()
        {
            string dtFilePath = Path.Combine(DocTemplateFolder, FileName);
            FileInfo dtFileInfo = new FileInfo(dtFilePath);
            if (!dtFileInfo.Exists)
                throw new Exception(String.Format("File {0} as current version for selected DocTemplate does not exist in file system", dtFileInfo.Name));
            log.WriteOperationLine("File source is found");

            Stream dtvStream = dtFileInfo.OpenRead();
            log.WriteOperationLine("File source is opened");

            return dtvStream;
        }

        private void LoadFromDataRow(DataRowView rowView)
        {
            ID = Convert.ToInt32(rowView["ID"].ToString());
            docTemplateID = Convert.ToInt32(rowView["DocTemplateID"].ToString());
            fileName = rowView["FileName"].ToString();
            version = rowView["Version"].ToString();
            uploadDate = Convert.ToDateTime(rowView["UploadDate"].ToString());
            isCurrent = Convert.ToBoolean(rowView["IsCurrent"].ToString());
            dtvFormatType = (DTVFormatType)Convert.ToInt32(rowView["FormatTypeID"]);
        }

        private void PopulateRemovableHash()
        {
            docFieldRemovableList.AddRange(docFieldHash.Values);
            docFieldHash.Clear();
        }

        private void SetDocFieldHash(Hashtable _docFieldHash)
        {
            docFieldTable = null;

            if (_docFieldHash == null)
                PopulateRemovableHash();
            else if (docFieldHash != _docFieldHash)
            {
                PopulateRemovableHash();
                foreach (DictionaryEntry entryField in _docFieldHash)
                {
                    DocTemplateField field = (DocTemplateField)entryField.Value;
                    field.DocTemplateVersionID = ID;
                    docFieldHash[entryField.Key] = field;
                }
            }
        }

        private string AnalyseContentType(HttpPostedFile postedFI)
        {
            postedFileInfo = postedFI;
            string ext = PostedFileExt;

            if (postedFileInfo.ContentType.ToLower().Contains("application/pdf") || 
                (postedFileInfo.ContentType.ToLower().Contains("application/octet-stream") && String.Compare(ext, "pdf", true) == 0))
                DTVFormatType = DTVFormatType.PDF;
            else if (postedFileInfo.ContentType.ToLower().Contains("application/msword") ||
                    (postedFileInfo.ContentType.ToLower().Contains("application/octet-stream") && String.Compare(ext, "doc", true) == 0))
                DTVFormatType = DTVFormatType.Word;
            else
                DTVFormatType = DTVFormatType.Unknown;

            switch (DTVFormatType)
            {
                case DTVFormatType.PDF:
                    if (String.Compare(ext, "pdf", true) != 0)
                        return String.Format("Unrecognized extention *.{0} for pdf document template", ext);
                    break;
                case DTVFormatType.Word:
                    if (String.Compare(ext, "doc", true) != 0)
                        return String.Format("Unrecognized extention *.{0} for word document template", ext);
                    break;
                default:
                    return String.Format("Unrecognized format *.{0} for document template", ext);
            }

            return String.Empty;
        }

        private string SaveDTVToDisk()
        {
            string dtvFilePath = DTVCreatedFilePath;
            if (String.IsNullOrEmpty(dtvFilePath))
                return String.Empty;

            postedFileInfo.SaveAs(dtvFilePath);
            return new FileInfo(dtvFilePath).Name;
        }
        #endregion

        #region Public methods
        public byte[] CreateFilledDocument(MortgageProfile mp, Hashtable mpFieldHash)
        {
            log.WriteHeader();

            switch (DTVFormatType)
            {
                case DTVFormatType.PDF:
                    return CreateFilledPDFDocument(mp, mpFieldHash);
                case DTVFormatType.Word:
                    return CreateFilledWordDocument(mp, mpFieldHash);
                default:
                    return null;
            }
        }

        public string RefreshDocTemplateFields(HttpPostedFile postedFI)
        {
            string errMessage = AnalyseContentType(postedFI);
            if (!String.IsNullOrEmpty(errMessage))
                return errMessage;

            Hashtable dtvFieldHash = null;
            try
            {
                switch (DTVFormatType)
                {
                    case DTVFormatType.PDF:
                        dtvFieldHash = GetDTVPDFFields(postedFI);
                        break;
                    case DTVFormatType.Word:
                        dtvFieldHash = GetDTVWordFields(postedFI);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return String.Format("Can't read fields of document {0} <br />Error: {1}", postedFI.FileName, ex.Message);
            }

            SetDocFieldHash(dtvFieldHash);
            return String.Empty;
        }

        public String Update(HttpPostedFile postedFI)
        {
            string errMessage = RefreshDocTemplateFields(postedFI);
            if (!String.IsNullOrEmpty(errMessage))
                return errMessage;

            errMessage = Update();
            return errMessage;
        }

        public String Update()
        {
            string dtvOldFileName = String.Empty;
            if (postedFileInfo != null)
            {
                string dtvFileName = SaveDTVToDisk();
                if (String.IsNullOrEmpty(dtvFileName))
                    return String.Format("Can't save file {0} on the disk", postedFileInfo.FileName);

                dtvOldFileName = FileName;
                FileName = dtvFileName;
            }

            int res = 0;
            try
            {
                dbAccess.BeginTransaction();

                if (DocTemplateID <= 0)
                {
                    DocTemplate.Update(dbAccess);
                    DocTemplateID = docTemplate.ID;
                }

                bool isNew = ID <= 0;
                res = dbAccess.ExecuteScalarInt("EditDocTemplateVersion",
                                            ID,
                                            DocTemplateID,
                                            Version,
                                            FileName,
                                            IsCurrent,
                                            (int)DTVFormatType);

                if (isNew) 
                    ID = res;

                DocTemplateField.DeleteFieldList(docFieldRemovableList, dbAccess);
                DocTemplateField.UpdateFieldList(docFieldHash.Values, dbAccess);

                if (!String.IsNullOrEmpty(dtvOldFileName))
                    File.Delete(dtvOldFileName);

                if (isNew)
                    CalculateFieldAutoMapping(dbAccess);

                dbAccess.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbAccess.RollbackTransaction();
                if (postedFileInfo != null)
                {
                    FileName = dtvOldFileName;
                    File.Delete(DTVCreatedFilePath);
                }

                return String.Format("Version {0} of Document Template can't be stored to the database <br />Error: {1}", Version, ex.Message);
            }

            return String.Empty;
        }

        public string UpdateFieldList()
        {
            try
            {
                dbAccess.BeginTransaction();

                DocTemplateField.DeleteFieldList(docFieldRemovableList, dbAccess);
                DocTemplateField.UpdateFieldList(docFieldHash.Values, dbAccess);

                dbAccess.CommitTransaction();
            }
            catch (Exception)
            {
                dbAccess.RollbackTransaction();
                return "Database error!";
            }

            return String.Empty;
        }

        public bool ChangeDocFieldMap(int mpFieldID, string mpFieldName, string dtvFieldName, string dtvRegionName, int groupIndex1, int groupIndex2, string groupName)
        {
            string key = String.IsNullOrEmpty(dtvRegionName) ? dtvFieldName : String.Format("{0}:{1}", dtvRegionName, dtvFieldName);
            if (!docFieldHash.ContainsKey(key))
                return false;

            DocTemplateField docTemplateField = (DocTemplateField)docFieldHash[key];

            docTemplateField.MPFieldID = mpFieldID;
            docTemplateField.MPFieldName = mpFieldName;
            docTemplateField.GroupIndex1 = groupIndex1;
            docTemplateField.GroupIndex2 = groupIndex2;

            if (docFieldTable == null)
                return false;

            foreach (DataRow row in docFieldTable.Rows)
                if (ConvertToString(row["DTVFieldName"], String.Empty) == dtvFieldName &&
                    ConvertToString(row["RegionName"], String.Empty) == dtvRegionName)
                {
                    row["FieldID"] = mpFieldID;
                    row["MPFiledName"] = mpFieldName;
                    row["GroupIndex1"] = groupIndex1;
                    row["GroupIndex2"] = groupIndex2;
                    row["GroupName"] = groupName;
                }

            return true;
        }

        public void SetCurrent()
        {
            dbAccess.ExecuteScalar("SetDocTemplateVersionCurrent", ID, DocTemplateID);
        }

        public bool AnalyseDTVFieldMapping(string regionName, int fieldGroupID, int groupIndex1, int groupIndex2)
        {
            if (docFieldTable == null)
                return false;

            if (!String.IsNullOrEmpty(regionName))
            {
                DataView regionFieldView = new DataView(docFieldTable);
                regionFieldView.RowFilter = String.Format("RegionName = '{0}'", regionName);
                if (regionFieldView.Count == 0)
                    return false;
            }

            object objRegionName = String.IsNullOrEmpty(regionName) ? DBNull.Value : (object)regionName;
            bool bRes =  dbAccess.ExecuteScalarBool("AnalyseDTVFieldMapping", ID, objRegionName, fieldGroupID, groupIndex1, groupIndex2);
            return bRes;
        }

        public DataView GetRegionList()
        {
            return dbAccess.GetDataView("GetRegionList", ID);
        }
        #endregion

        #region Static fields
        public static string DocTemplateFolder = String.Empty;
        #endregion

        #region Static methods
        public static ArrayList GetDocTemplateVersionsByIDs(string docTemplatesXml)
        {
            ArrayList dtVersionList = new ArrayList();
            if (docTemplatesXml.Trim().Length == 0)
                return dtVersionList;

            DataSet ds = dbAccess.GetDataSet("GetDocTemplateVersionByIds", docTemplatesXml);
            if (ds.Tables.Count == 0)
                return dtVersionList;

            DataView dtVersionView = ds.Tables[0].DefaultView;
            DataView dtFieldView = null;
            if (ds.Tables.Count > 1)
                dtFieldView = ds.Tables[1].DefaultView;

            foreach (DataRowView dtVersionRow in dtVersionView)
            {
                DocTemplateVersion dtVersion = new DocTemplateVersion();
                dtVersion.LoadFromDataRow(dtVersionRow);

                if (dtFieldView != null)
                {
                    dtFieldView.RowFilter = "DocTemplateVerID = " + dtVersion.ID;
                    Hashtable fieldHash = DocTemplateField.GetDocFieldHash(dtFieldView);
                    dtVersion.SetDocFieldHash(fieldHash);
                }

                dtVersionList.Add(dtVersion);
            }
            
            return dtVersionList;
        }

        public static MemoryStream SaveDocAsPDF(MemoryStream docMemoryStream)
        {
            //Save the document in Aspose.Pdf.Xml format into a memory stream.
            MemoryStream dtMemoryStream = new MemoryStream();
            Document docForm = new Document(docMemoryStream);
            docForm.Save(dtMemoryStream, SaveFormat.AsposePdf);
            //Seek to the beginning so it can be read by XmlDocument.
            dtMemoryStream.Seek(0, SeekOrigin.Begin);

            //Load the document into an XmlDocument
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(dtMemoryStream);

            //Load the XML document into Aspose.Pdf
            Pdf pdf = new Pdf();
            //Make sure the images that were saved by Aspose.Words into Windows temporary
            //folder are automatically deleted by Aspose.Pdf when they are no longer needed.
            pdf.IsImagesInXmlDeleteNeeded = true;
            pdf.BindXML(xmlDoc, null);

            //*** Aspose.Pdf font cache, see comments below.
            pdf.IsTruetypeFontMapCached = false;

            //If you convert to PDF multiple files in your application, 
            //uncomment the following lines to improve the speed of conversion.
            //pdf.IsTruetypeFontMapCached = true;
            //pdf.TruetypeFontMapPath = <some path where you have read/write access>

            //Now produce the PDF file.
            dtMemoryStream.Close();
            dtMemoryStream = new MemoryStream();
            pdf.Save(dtMemoryStream);

            return dtMemoryStream;
        }

        private static byte[] SaveDocAsPDF(byte[] docMemoryBuffer)
        {
            byte[] dtMemoryBuffer;
            MemoryStream dtMemoryStream = null;
            MemoryStream docMemoryStream = null;
            try
            {
                //Save the document in Aspose.Pdf.Xml format into a memory stream.
                docMemoryStream = new MemoryStream(docMemoryBuffer);
                dtMemoryStream = SaveDocAsPDF(docMemoryStream);
                dtMemoryBuffer = dtMemoryStream.ToArray();
            }
            finally
            {
                if (docMemoryStream != null)
                    docMemoryStream.Close();
                if (dtMemoryStream != null)
                    dtMemoryStream.Close();
            }

            return dtMemoryBuffer;
        }
        #endregion


        #region unused
        /*
        public ArrayList GetDocFieldList()
        {
            ArrayList docFieldList = new ArrayList();

            IEnumerator fieldEnumerator = docFieldHash.Values.GetEnumerator();
            while (fieldEnumerator.MoveNext())
                docFieldList.Add(fieldEnumerator.Current);

            fieldEnumerator.Reset();
            return docFieldList;
        }*/
        #endregion
    }
}

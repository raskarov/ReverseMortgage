using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Collections;
using System.Web;

namespace LoanStar.Common
{
    public class LeadImport : BaseObject
    {
        #region constants

        #region sp
        private const string GETMAPPERDATA = "GetLeadImportData";
        private const string GETIMPORTLEADSOURCELIST = "GetLeadImportSourceList";
        private const string SAVEIMPORTSTEP = "SaveImportStep";
        private const string SAVESOURCEFILE = "SaveImportSourceFile";
        #endregion

        #region steps
        public const int STEPUPLOADFILE = 1;
        public const int STEPIMPORT = 2;
        #endregion

        #region errors messages
        private const string ERRMAPPINGFILENOTFOUND = "File for mapping not specified";
        private const string ERRUNKNOWNSOURCEFILEFORMAT = "Unknown source file format";
        private const string ERRUNSUPPORTEDFILEFORMAT = "Unsupported file format";
        private const string ERRNULLPOSTEDFILE = "Posted file is null";
        #endregion

        public const string IMPORTTMPFOLDER = "Temp";

        #endregion

        #region fields
        private readonly int sourceId;
        private readonly int companyId;
        private readonly int userId;
        private int step;
        private string configFileName;
        private string errorMessage;
        private LeadImportMapper mapper;
        private DataProcessor dataProcessor;
        private int importFileTypeId;
        private readonly string importBaseFolder;
        private string sourceFileName;
        private int totalRows = 0;
        private int goodRows = 0;
        private int importedRows = 0;
        private int badRows = 0;
        private new readonly MappedRow.DataNeeded GetDictionary;
        private readonly string importGuid;
        private string tmpFileName;
        private DateTime stepTime;
        private int elapsedSeconds; 
        #endregion

        enum ImportFileType
        {
            Excel = 1
        }

        #region properties
        public int ElapsedSeconds
        {
            get { return elapsedSeconds; }
        }
        public int ImportedRows
        {
            get { return importedRows; }
        }
        public DataTable  DtImportedRows
        {
            get { return dataProcessor.ImportedRows; }
        }
        public DataTable DtBadRows
        {
            get { return dataProcessor.RejectedRows; }
        }
        public int TotalRows
        {
            get { return totalRows; }
        }
        public int GoodRows
        {
            get { return goodRows; }
        }
        public int BadRows
        {
            get { return badRows; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        public DataTable NotMappedFields
        {
            get
            {
                return mapper.NotMappedFields;
            }
        }
        public DataTable MappedFields
        {
            get
            {
                return mapper.MappedFields;
            }
        }
        #endregion

        #region constructor
        public LeadImport(int sourceId_, int companyId_, int userId_, string importBaseFolder_, MappedRow.DataNeeded GetDictionary_)
        {
            sourceId = sourceId_;
            companyId = companyId_;
            userId = userId_;
            importBaseFolder = importBaseFolder_;
            ID = -1;
            GetDictionary = GetDictionary_;
            importGuid = Guid.NewGuid().ToString();
        }
        #endregion

        #region methods

        #region public
        public bool ExecuteStep(int step_, Object o)
        {
            step = step_;
            return DoStep(o);
        }
        public bool DeleteRow(int id)
        {
            return dataProcessor.DeleteRow(id);
        }
        public void CleanupStep(int step_)
        {
            DeleteTempFile();
            if(step_==STEPIMPORT)
            {
                dataProcessor.CleanDb(ID);
                ID = -1;
            }
        }
        public string GetErrorById(int id)
        {
            return dataProcessor.GetErrorById(id);
        }
        #endregion

        #region private

        private void DeleteTempFile()
        {
            FileInfo fi = new FileInfo(tmpFileName);
            if (fi.Exists)
            {
                fi.Delete();
            }
        }
        private bool SaveStepResult()
        {
            bool r;
            int res = db.ExecuteScalarInt(SAVEIMPORTSTEP
                                          , ID
                                          , companyId
                                          , userId
                                          , sourceFileName
                                          , sourceId
                                          , totalRows
                                          , goodRows
                                          , badRows
                                          , importedRows
                                          , step
                );
            if(res>0&&ID<0)
            {
                ID = res;
                r = SaveSourceFile();
            }
            else
            {
                r = ID > 0;
            }
            return r;
        }
        private bool SaveSourceFile()
        {
            byte[] sourceData = GetSorceFileData();
            if(sourceData==null||sourceData.Length==0)
            {
                return false;
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SAVESOURCEFILE;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = ID;
            cmd.Parameters.Add("@sourceFile", SqlDbType.VarBinary, sourceData.Length).Value = sourceData;
            return db.ExecuteCommandInt(cmd)==1;
        }
        private byte[] GetSorceFileData()
        {
            byte[] res = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(tmpFileName, FileMode.Open,FileAccess.Read);
                res = new byte[fs.Length];
                fs.Read(res, 0, (int) fs.Length);
            }
            catch
            {
            }
            finally
            {
                if(fs!=null) fs.Close();
            }
            return res;
        }
        private bool DoStep(Object o)
        {
            bool res = false;
            stepTime = DateTime.Now;
            switch(step)
            {
                case STEPUPLOADFILE:
                    res = UploadFile(o as HttpPostedFile);
                    if(res)
                    {
                        res = CreateMapper();
                    }
                    if (res)
                    {
                        res = mapper.CreateMapping();
                        dataProcessor = CreateSaver();
                        if (dataProcessor != null)
                        {
                            CreateBusinessObjectAndSave();
                        }
                    }
                    break;
                case STEPIMPORT:
                    importedRows = dataProcessor.MoveToSystem((int)o);
                    res = importedRows > 0;
                    break;
            }
            if (res)
            {
                res = SaveStepResult();
            }
            CalculateStepTime();
            return res;
        }
        private void CalculateStepTime()
        {
            TimeSpan ts = DateTime.Now.Subtract(stepTime);
            elapsedSeconds = ts.Seconds;
        }
        private bool UploadFile(HttpPostedFile postedFile)
        {
            bool res = false;
            if(postedFile==null)
            {
                errorMessage = ERRNULLPOSTEDFILE;
            }
            else
            {
                sourceFileName = postedFile.FileName;
                tmpFileName = Path.Combine(Path.Combine(importBaseFolder, IMPORTTMPFOLDER), importGuid + Path.GetExtension(sourceFileName));
                try
                {
                    postedFile.SaveAs(tmpFileName);
                    res = true;
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            return res;
        }
        private void CreateBusinessObjectAndSave()
        {
            if(mapper.ImportData== null||mapper.ImportData.Rows.Count==0)
            {
                errorMessage = "No data for import found";
                return;
            }
            totalRows = mapper.ImportData.Rows.Count;
            for (int i = 0; i < totalRows; i++)
            {
                MappedRow obj = new MappedRow(i,mapper.ImportData.Rows[i], mapper.ConfigMapData, mapper.DateFormatString);
                obj.OnDataNeeded += GetDictionary;
                bool res = obj.CreateObjects();
                if(!res)
                {
                    mapper.AddRowToBad(i);
                }
                bool r = dataProcessor.Save(obj, res);
                if(r)
                {
                    if (res)
                    {
                        goodRows++;
                    }
                    else
                    {
                        badRows++;
                    }
                }
            }
        }
        private DataProcessor CreateSaver()
        {
            DataProcessor res;
            switch (importFileTypeId)
            {
                case (int)ImportFileType.Excel:
                    res = new DataProcessorRMAExcel(companyId,userId,importGuid,mapper.StoredProcedure);
                    break;
                default:
                    throw  new Exception(ERRUNSUPPORTEDFILEFORMAT);
            }
            return res;
        }
        private bool CreateMapper()
        {
            bool res = false;
            GetMapperData();
            if (String.IsNullOrEmpty(configFileName))
            {
                errorMessage = ERRMAPPINGFILENOTFOUND;
                return res;
            }
            else if (importFileTypeId == 0)
            {
                errorMessage = ERRUNKNOWNSOURCEFILEFORMAT;
                return res;
            }
            switch(importFileTypeId)
            {
                case (int)ImportFileType.Excel: 
                    mapper = new LeadImportMapperExcel(importBaseFolder, configFileName, tmpFileName);
                    res = true;
                    break;
                default:
                    errorMessage = ERRUNSUPPORTEDFILEFORMAT;
                    break;
            }
            return res;
        }
        private void GetMapperData()
        {
            DataView dv = db.GetDataView(GETMAPPERDATA, sourceId);
            if(dv.Count==1)
            {
                configFileName = dv[0]["mappingfile"].ToString();
                importFileTypeId = int.Parse(dv[0]["SourceFileTypeId"].ToString());
            }
        }
        #endregion

        #region static
        public static DataView LeadImportSourceList()
        {
            return db.GetDataView(GETIMPORTLEADSOURCELIST);
        }
        #endregion

        #endregion
    }

    public class LeadImportMapper : BaseObject
    {
        #region constants
        private const string ERRCONFIGFILENOTFOUNDFILE = "Configuration file {0} not found";
        protected const string ERRDATASECTIONMISSING = "Data section is missing";
        protected const string IMPORTCONFIGFOLDER = "MapperData";
        #endregion

        #region fields
        protected string errorMessage;
        private readonly string configFileName;
        protected Hashtable configMapData;
        protected XmlDocument configData;
        protected ArrayList columnNames;
        protected ArrayList propertyNames;
        protected ArrayList notMappedColumns;
        private DataTable notMappedFields;
        private DataTable mappedFields;
        private DataTable importData;
        protected string dateFormatString;
        protected string storedProcedure;
        protected string importBaseFolder;
        protected readonly string dataFileName;
        protected string errorFileName = "";
        #endregion

        #region properties
        public string StoredProcedure
        {
            get { return storedProcedure;}
        }
        public string DateFormatString
        {
            get { return dateFormatString; }
        }
        public Hashtable ConfigMapData
        {
            get { return configMapData; }
        }
        public DataTable ImportData
        {
            get
            {
                if(importData==null)
                {
                    importData = GetImportData();
                }
                return importData;
            }
        }
        public DataTable MappedFields
        {
            get
            {

                if (mappedFields == null)
                {
                    mappedFields = GetMappedFieldsTable();
                }
                return mappedFields;
            }
        }

        public DataTable NotMappedFields
        {
            get
            {

                if (notMappedFields == null)
                {
                    notMappedFields = GetNotMappedFieldsTable();
                }
                return notMappedFields;
            }
        }
        #endregion

        #region constructor
        public LeadImportMapper(string importBaseFolder_, string configFileName_, string dataFileName_)
        {
            importBaseFolder = importBaseFolder_;
            configFileName = configFileName_;
            dataFileName = dataFileName_;
            configMapData = new Hashtable();
        }
        #endregion

        #region methods
        public virtual void AddRowToBad(int i)
        {
        }
        public virtual bool CreateMapping()
        {
            bool res = CreateMapTable();
            if(res)
            {
                res = MapData();
            }
            return res;
        }
        protected virtual bool CreateMapTable()
        {
            bool res = false;
            string fileName = Path.Combine(Path.Combine(importBaseFolder, IMPORTCONFIGFOLDER), configFileName);
            if(!File.Exists(fileName))
            {
                errorMessage = String.Format(ERRCONFIGFILENOTFOUNDFILE, fileName);
                return res;
            }
            try
            {
                configData = new XmlDocument();
                configData.Load(fileName);
                res = ParseConfig();
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            return res;
        }
        protected virtual bool ParseConfig()
        {
            return false;
        }
        protected virtual bool MapData()
        {
            return false;
        }
        protected virtual DataTable GetNotMappedFieldsTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(GetColumn("ID", "ID", typeof(Int32)));
            tbl.Columns.Add(GetColumn("Column name", "ColumnName", typeof(string)));
            
            for(int i = 0; i < notMappedColumns.Count; i++)
            {
                tbl.Rows.Add(i, notMappedColumns[i].ToString());
            }
            return tbl;
        }
        protected virtual DataTable GetMappedFieldsTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(GetColumn("ID", "ID", typeof(Int32)));
            tbl.Columns.Add(GetColumn("Column name", "ColumnName", typeof(string)));
            tbl.Columns.Add(GetColumn("Field name", "PropertyName", typeof(string)));
            
            foreach (DictionaryEntry item in configMapData)
            {
                MapperObject obj = (MapperObject) item.Value;
                tbl.Rows.Add(int.Parse(obj.Key), obj.ColumnName, obj.PropertyName);
            }
            return tbl;
        }
        protected virtual DataTable GetImportData()
        {
            return null;
        }
        private static DataColumn GetColumn(string caption, string name, Type t)
        {
            DataColumn column = new DataColumn(name, t);
            column.Caption = caption;
            return column;
        }

        #endregion
    }
    public class LeadImportMapperExcel :LeadImportMapper
    {
        #region constants
        private const string GENERALNODE = "//ImportConfigDocument/GeneralData";
        private const string DATANODE = "//ImportConfigDocument/MappingData";
        private const string EMPTYFILENAME = "RmaExcel_empty.xls";
        #endregion

        #region fields
        private bool hasHeaderRow=true;
        #endregion

        #region constructor
        public LeadImportMapperExcel(string importBaseFolder_, string configFileName_, string dataFileName_)
            : base(importBaseFolder_, configFileName_, dataFileName_)
        {
            errorFileName = CreateBadRowsFile();
        }
        #endregion

        #region methods

        #region overriden
        //public override void AddRowToBad(int i)
        //{
        //    if(!String.IsNullOrEmpty(errorFileName))
        //    {
        //        ExcelWrapper excel = new ExcelWrapper(errorFileName,hasHeaderRow,false);
        //        excel.InsertDataRow(ImportData.Rows[i]);
        //    }
        //}
        protected override bool MapData()
        {
            bool res = true;
            ExcelWrapper excel = new ExcelWrapper(dataFileName,hasHeaderRow);
            columnNames = excel.GetColumnNames();
            SetColumnNames();
            return res;
        }
        protected override DataTable GetImportData()
        {
            ExcelWrapper excel = new ExcelWrapper(dataFileName,hasHeaderRow);
            return excel.DtData;
        }
        protected override bool ParseConfig()
        {
            bool res;
            ParseGeneral();
            res = ParseMapping();
            return res;
        }
        #endregion
        private string CreateBadRowsFile()
        {
            string res = "";
            try
            {
                string emptyFile = Path.Combine(Path.Combine(importBaseFolder, IMPORTCONFIGFOLDER), EMPTYFILENAME);
                string tmp = Path.Combine(Path.GetDirectoryName(dataFileName), Path.GetFileNameWithoutExtension(dataFileName)) + "_err" + Path.GetExtension(dataFileName);
                FileInfo fi = new FileInfo(emptyFile);
                fi.CopyTo(tmp);
                res = tmp;
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            return res;
        }

        private void SetColumnNames()
        {
            notMappedColumns = new ArrayList();
            for (int i=0; i<columnNames.Count;i++)
            {
                if(configMapData.ContainsKey(i.ToString()))
                {
                    ((MapperObject) configMapData[i.ToString()]).ColumnName = columnNames[i].ToString();
                }
                else
                {
                    notMappedColumns.Add(columnNames[i].ToString());
                }
            }
        }
        private void ParseGeneral()
        {
            XmlNode node = configData.SelectSingleNode(GENERALNODE);
            if(node!=null)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes[i].Name == "data")
                    {
                        string name = node.ChildNodes[i].Attributes["name"].Value.ToLower();
                        if (name == "hasheaderrow")
                        {
                            try
                            {
                                hasHeaderRow = bool.Parse(node.ChildNodes[i].Attributes["value"].Value);
                            }
                            catch
                            {
                            }
                        }
                        else if(name=="dateformat")
                        {
                            dateFormatString = node.ChildNodes[i].Attributes["value"].Value;
                        }
                        else if (name == "spname")
                        {
                            storedProcedure = node.ChildNodes[i].Attributes["value"].Value;
                        }
                    }
                }
            }
        }
        private bool ParseMapping()
        {
            bool res = false;
            Hashtable tbl = new Hashtable();
            propertyNames = new ArrayList();
            XmlNode node = configData.SelectSingleNode(DATANODE);
            if(node != null&&node.ChildNodes.Count>0)
            {
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    MapperObject obj = new MapperObject(node.ChildNodes[i]);
                    if (!String.IsNullOrEmpty(obj.Key))
                    {
                        if (!configMapData.ContainsKey(obj.Key))
                        {
                            configMapData.Add(obj.Key, obj);
                        }
                        string propertyName = obj.PropertyName;
                        if(!tbl.ContainsKey((propertyName)))
                        {
                            tbl.Add(propertyName,null);
                            propertyNames.Add(propertyName);
                        }
                    }
                }
                res = configMapData.Count > 0;
            }
            else
            {
                errorMessage = ERRDATASECTIONMISSING;
            }
            return res;
        }

        #endregion

    }

    public class MapperObject
    {
        #region fields
        private readonly string key;
        private readonly string propertyName;
        private string columnName;
        private readonly MapperDataSource dataSource;

        #endregion

        #region properties
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        public string PropertyName
        {
            get { return propertyName; }
        }
        public string Key
        {
            get { return key; }
        }
        public bool HasFilter
        {
            get
            {
                bool res = false;
                if(dataSource!=null)
                {
                    res = dataSource.HasFilter;
                }
                return res;
            }
        }
        public MapperDataSource DataSource
        {
            get { return dataSource; }
        }
        #endregion

        #region constructors
        public MapperObject(string key_, string columnName_, string propertyName_)
        {
            columnName = columnName_;
            propertyName = propertyName_;
            key = key_;
        }
        public MapperObject(XmlNode node)
        {
            string tableName = "";
            string keyFieldName = "";
            string valueFieldName = "";
            string filter = "";
            string isString = "";
            for (int i = 0; i < node.Attributes.Count; i++)
            {
                string name = node.Attributes[i].Name;
                string value = node.Attributes[i].Value;
                switch (name)
                {
                    case "key":
                        columnName = value;
                        key = ExcelWrapper.ConvertToIndex(value).ToString();
                        break;
                    case "property":
                        propertyName = value;
                        break;
                    case "table":
                        tableName = value;
                        break;
                    case "keyfield":
                        keyFieldName = value;
                        break;
                    case "valuefield":
                        valueFieldName = value;
                        break;
                    case "filter":
                        filter = value;
                        break;
                    case "isstring":
                        isString = value;
                        break;
                }
            }
            if(!String.IsNullOrEmpty(tableName)&&!String.IsNullOrEmpty(keyFieldName)&&!String.IsNullOrEmpty(valueFieldName))
            {
                dataSource = new MapperDataSource(tableName, keyFieldName, valueFieldName, filter, isString);
            }
        }
        #endregion
    }

    public class MapperDataSource
    {
        #region constants
        private const string QUOTEDFILTER = "{0}='{1}'";
        private const string UNQUOTEDFILTER = "{0}={1}";
        #endregion

        #region fields
        private readonly string tableName;
        private readonly string keyFieldName;
        private readonly string valueFieldName;
        private readonly string filter;
        private readonly bool isQuoted;
        #endregion

        #region properties
        public bool HasFilter
        {
            get { return !String.IsNullOrEmpty(filter); }
        }
        public string TableName
        {
            get { return tableName; }
        }
        #endregion

        #region constructor
        public MapperDataSource(string tableName_, string keyFieldName_, string valueFieldName_, string filter_, string isString)
        {
            tableName = tableName_;
            keyFieldName = keyFieldName_;
            valueFieldName = valueFieldName_;
            filter = filter_;
            isQuoted = !String.IsNullOrEmpty(isString) && isString == "1";
        }

        #endregion

        #region methods
        public string GetDictionaryValue(DataView dv, string propertyValue, MortgageProfile mp)
        {
            string res;
            string rowFilter = String.Format((isQuoted ? QUOTEDFILTER : UNQUOTEDFILTER), keyFieldName, propertyValue);
            if (!String.IsNullOrEmpty(filter))
            {
                string[] parts = filter.Split('=');
                if (parts.Length != 2)                
                {
                    throw new Exception(String.Format("Wrong filter expression {0}", filter));
                }
                string objectName;
                string err;
                int index;
                string[] names = parts[1].Split('.');
                XmlMapper.CheckIndexer(names[0], out objectName, out index);
                string val = mp.GetObjectValueByIndex(objectName + "." + names[1], index, out err);
                if(!String.IsNullOrEmpty(err))
                {
                    throw new Exception(err);
                }
                string extraFilter = String.Format("{0}={1}",parts[0],val);
                if(!String.IsNullOrEmpty(rowFilter))
                {
                    rowFilter += " and " + extraFilter;
                }
                else
                {
                    rowFilter =extraFilter;
                }
            }
            dv.RowFilter = rowFilter;
            if (dv.Count == 0)
            {
                throw new Exception(String.Format("For value({0}) - no record found in dictionary {1} ", propertyValue, tableName));
            }
            else if (dv.Count > 1)
            {
                throw new Exception(String.Format("For value({0}) - multiple records found  in dictionary {1}", propertyValue, tableName));
            }
            res = dv[0][valueFieldName].ToString();
            return res;
        }
        #endregion
    }
    public class MappedRow : BaseObject
    {
        //#region constants
        //private const string SAVEROW = "SaveImportedRow";
        //#endregion

        #region fields
        private MortgageProfile mp;
        private readonly XmlDocument errors;
        private readonly DataRow row;
        private readonly Hashtable mapData;
        private readonly ArrayList secondMap = new ArrayList();
        private readonly string dateFormat;
        private int errorCount;
        private readonly XmlNode root;
        private readonly ArrayList customObjects = new ArrayList();
        #endregion

        #region proeprties
        public MortgageProfile Mp
        {
            get { return mp; }
        }
        #endregion

        #region events/delegates
        public event DataNeeded OnDataNeeded;
        public delegate DataView DataNeeded(string dictionaryName);
        #endregion


        #region constructor
        public MappedRow(int i, DataRow row_, Hashtable mapData_,string dateFormat_)
        {
            ID = i;
            row = row_;
            mapData = mapData_;
            dateFormat = dateFormat_;
            errors = new XmlDocument();
            root = errors.CreateElement("Root");
            errors.AppendChild(root);
            errorCount = 0;
        }
        #endregion

        #region methods
        #region public
        public string GetCustomObjectData(string objectName)
        {
            string res = "";
            for (int i = 0; i < customObjects.Count;i++)
            {
                string val = customObjects[i].ToString();
                string[] parts = val.Split('=');
                if(parts.Length==2&&parts[0]==objectName)
                {
                    res = parts[1];
                    break;
                }
            }
            return res;
        }

        public bool CreateObjects()
        {
            mp = new MortgageProfile();
            foreach(DictionaryEntry item in mapData)
            {
                MapperObject obj = item.Value as MapperObject;
                try
                {
                    if (obj != null)
                        if (!obj.HasFilter)
                        {
                            MapProperty(obj);
                        }
                        else
                        {
                            secondMap.Add(obj.Key);
                        }
                }
                catch (Exception ex)
                {
                    if (obj != null) AddError(obj.ColumnName, ex.Message);
                }
            }
            if(secondMap.Count>0)
            {
                for(int i=0; i<secondMap.Count;i++)
                {
                    MapperObject obj = (MapperObject) mapData[secondMap[i]];
                    try
                    {
                        MapProperty(obj);
                    }
                    catch (Exception ex)
                    {
                        AddError(obj.ColumnName, ex.Message);
                    }
                }
            }
            ValidateObjects();
            return errorCount==0;
        }
        public string GetErrors()
        {
            string res = String.Empty;
            if(errorCount>0)
            {
                res = errors.OuterXml;
            }
            return res;
        }

        #endregion

        #region private
        private void AddError(string propertyName,string errorMessage)
        {
            errorCount++;
            XmlNode err = errors.CreateElement(ITEMELEMENT);
            XmlAttribute attr = errors.CreateAttribute("fieldname");
            attr.Value = propertyName;
            err.Attributes.Append(attr);
            err.InnerText = errorMessage;
            root.AppendChild(err);
        }

        private void ValidateObjects()
        {
            if (String.IsNullOrEmpty(mp.Borrowers[0].FirstName) || String.IsNullOrEmpty(mp.Borrowers[0].LastName))
            {
                AddError("Borrower 1", "First Borrower FirstName and LastName can't be empty");
            }
            if(mp.Borrowers.Count>1)
            {
                if (!String.IsNullOrEmpty(mp.Borrowers[1].FullName))
                {
                    if (String.IsNullOrEmpty(mp.Borrowers[1].FirstName) || String.IsNullOrEmpty(mp.Borrowers[1].LastName))
                    {
                        AddError("Borrower 2", "Second Borrower FirstName and LastName can't be empty");
                    }
                }
            }
            ValidateDateOfBirth();
        }
        private void ValidateDateOfBirth()
        {
            for(int i=0; i<mp.Borrowers.Count; i++)
            {
                if(mp.Borrowers[i].DateOfBirth!=null)
                {
                    DateTime dt = (DateTime) mp.Borrowers[i].DateOfBirth;
                    if(dt.Year - 2000 > 0)
                    {
                        mp.Borrowers[i].DateOfBirth = dt.AddYears(-100);
                    }
                }
            }
        }

        private void MapProperty(MapperObject map)
        {
            int n = int.Parse(map.Key);
            string propertyName = map.PropertyName;
            if(String.IsNullOrEmpty(propertyName))
            {
                throw new Exception("Property name is empty");
            }
            string propertyValue = row[n].ToString();
            string[] names = propertyName.Split('.');
            if(names[0]=="*")
            {
                MapCustomObject(names[1],propertyValue);
            }
            else
            {
                string objectName;
                int index;
                XmlMapper.CheckIndexer(names[0], out objectName, out index);
                if (map.DataSource != null && !String.IsNullOrEmpty(propertyValue))
                {
                    propertyValue = GetDictionaryValue(map.DataSource, propertyValue);
                }
                mp.SetProperty(objectName + "." + names[1], index, propertyValue, dateFormat);
            }
        }
        private void MapCustomObject(string name,string val)
        {
            customObjects.Add(String.Format("{0}={1}", name, val));
        }
        private string GetDictionaryValue(MapperDataSource dataSource, string propertyValue)
        {
            string res = "";
            if (OnDataNeeded != null)
            {
                DataView dv = OnDataNeeded(dataSource.TableName);
                if (dv == null || dv.Count == 0)
                {
                    throw new Exception(String.Format("Dictionary {0} not found or empty", dataSource.TableName));
                }
                res = dataSource.GetDictionaryValue(dv,propertyValue,mp);
            }
            return res;
        }
        #endregion

        #endregion
    }

    public class DataProcessor : BaseObject
    {
        protected const string ROOTELEMENT = "Root";
        #region fields
        protected readonly string storedProcedureName;
    	protected readonly string importGuid;
        protected readonly int companyId;
        protected readonly int userId;
        protected DataTable dtImportedRows;
        protected DataTable dtBadRows;
        #endregion

        #region properties
        public DataTable ImportedRows
        {
            get
            {
                if(dtImportedRows==null)
                {
                    dtImportedRows = GetImportedRows();
                }
                return dtImportedRows;
            }
        }
        public DataTable RejectedRows
        {
            get
            {
                if (dtBadRows == null)
                {
                    dtBadRows = GetBadRows();
                }
                return dtBadRows;
            }
        }
        #endregion

        #region contsructor
        public DataProcessor(int companyId_, int userId_, string importGuid_, string storedProcedureName_)
        {
            storedProcedureName = storedProcedureName_;
    	    importGuid = importGuid_;
            companyId = companyId_;
            userId = userId_;
        }

        #endregion
        public virtual bool DeleteRow(int rowId)
        {
            return false;
        }
        public virtual bool Save(MappedRow obj, bool isGood)
        {
            return false;
        }
        protected virtual DataTable GetImportedRows()
        {
            return null;
        }
        protected virtual DataTable GetBadRows()
        {
            return null;
        }
        public virtual int MoveToSystem(int userId_)
        {
            return 0;
        }
        public virtual string GetErrorById(int id)
        {
            return "";
        }
        public virtual void CleanDb(int importId)
        {

        }
    }

    public class DataProcessorRMAExcel : DataProcessor
    {
        #region constants

        private readonly string[] Borrower0Fileds = {
                                                        "FirstName", "LastName", "DateOfBirth", "Phone", "EmailAddress",
                                                        "Address1", "City", "StateID", "Zip"
                                                    };
        private readonly string[] PropertyFileds = { "Address1", "City", "StateId", "Zip", "CountyID", "SPValue", "SPHomeTypeId" };

        private readonly string[] MortgageInfoFileds = {
                                                           "AltContactName", "AltContactAddress1", "AltContactAddress2",
                                                           "AltContactPhone", "AltContactRelationship", "ApplicationDate"
                                                       };

        private readonly string[] PayoffsFileds = { "Amount"} ;
        private const string DELETEROW = "DeleteImportedRMAExcel";
        #endregion

        #region contsructor
        public DataProcessorRMAExcel(int companyId_, int userId_, string importGuid_, string storedProcedureName_)
            : base(companyId_, userId_, importGuid_, storedProcedureName_)
        {
        }
        #endregion

        #region methods
        public override bool Save(MappedRow obj, bool isGood)
        {
            string borrowerXml = GetBorrowerXml(obj.Mp);
            string propertyXml = GetPropertyXml(obj.Mp);
            string mortgageInfoXml = GetMortgageInfoXml(obj.Mp);
            string payoffsXml = GetPayoffsXml(obj.Mp);
            string errors = "";
            if(!isGood)
            {
                errors = obj.GetErrors();
            }
            string note = obj.GetCustomObjectData("Note");
            return StoreToDb(obj, borrowerXml,propertyXml,mortgageInfoXml,payoffsXml, isGood,errors,note);
        }
        protected override DataTable GetImportedRows()
        {
            return db.GetDataTable(String.Format("GetImportedRows{0}", storedProcedureName), importGuid);
        }
        protected override DataTable GetBadRows()
        {
            return db.GetDataTable(String.Format("GetBadRows{0}", storedProcedureName), importGuid);
        }
        public override int MoveToSystem(int userId_)
        {
            return db.ExecuteScalarInt(String.Format("MoveToSystem{0}", storedProcedureName), importGuid, companyId, userId_);
        }
        public override void CleanDb(int importId)
        {
           db.ExecuteScalar(String.Format("CleanImportLead{0}", storedProcedureName), importId, importGuid);
        }

        public override string GetErrorById(int id)
        {
            string err = db.ExecuteScalarString(String.Format("GetError{0}", storedProcedureName), id);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(err);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count;i++ )
            {
                XmlNode node = doc.DocumentElement.ChildNodes[i];
                sb.AppendLine(String.Format("{0} : {1}", node.Attributes["fieldname"].Value, node.InnerText));
            }
            return sb.ToString();
        }
        public override bool DeleteRow(int id)
        {
            bool res =  db.ExecuteScalarInt(DELETEROW, id) == 1;
            if (res) dtImportedRows = null;
            return res;
        }
        #region private(source specific methods)
        private bool StoreToDb(BaseObject obj, string borrowerXml, string propertyXml, string mortgageInfoXml,string payoffsXml, bool isGood, string errors, string note)
        {
            return
                db.ExecuteScalarInt(String.Format("SaveForImport{0}", storedProcedureName), obj.ID, companyId, userId,
                                    importGuid, borrowerXml, propertyXml, mortgageInfoXml, payoffsXml, isGood, errors, note) > 0;
        }
        private string GetPayoffsXml(MortgageProfile mp)
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlNode node1 = GetPayoffsNode(d, 0, PayoffsFileds, mp);
            root.AppendChild(node1);
            if(mp.Payoffs[1].Amount>0)
            {
                XmlNode node2 = GetPayoffsNode(d, 1, PayoffsFileds, mp);
                root.AppendChild(node2);
            }
            d.AppendChild(root);
            return d.OuterXml;
        }
        private string GetMortgageInfoXml(MortgageProfile mp)
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlNode node = GetMortgageInfoNode(d, MortgageInfoFileds, mp);
            root.AppendChild(node);
            d.AppendChild(root);
            return d.OuterXml;
        }
        private string GetPropertyXml(MortgageProfile mp)
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlNode node = GetPropertyNode(d, PropertyFileds, mp);
            root.AppendChild(node);
            d.AppendChild(root);
            return d.OuterXml;
        }
        private string GetBorrowerXml(MortgageProfile mp)
        {
            XmlDocument d = new XmlDocument();
            XmlNode root = d.CreateElement(ROOTELEMENT);
            XmlNode node0 = GetBorrowerNode(d, 0, Borrower0Fileds, mp);
            XmlNode node1 = null;
            if(!String.IsNullOrEmpty(mp.Borrowers[1].FirstName)&&!String.IsNullOrEmpty(mp.Borrowers[1].LastName))
            {
                node1 = GetBorrowerNode(d, 1, Borrower0Fileds, mp);
            }
            if(node0!=null)
            {
                root.AppendChild(node0);
            }
            else
            {
                return "";
            }
            if(node1!=null)
            {
                root.AppendChild(node1);
            }
            d.AppendChild(root);
            return d.OuterXml;
        }
        private static XmlNode GetBorrowerNode(XmlDocument d, int index, string[] fields, MortgageProfile mp)
        {
            XmlNode borrower = d.CreateElement(ITEMELEMENT);
            for(int i=0; i<fields.Length; i++)
            {
                string err;
                XmlAttribute attr = d.CreateAttribute(fields[i]);
                Object o = mp.GetObjectByIndex("Borrowers." + fields[i], index, out err);
                if(o!=null)
                {
                    attr.Value = o.ToString();
                    borrower.Attributes.Append(attr);
                }
            }
            return borrower;
        }
        private static XmlNode GetPayoffsNode(XmlDocument d, int index, string[] fields, MortgageProfile mp)
        {
            XmlNode borrower = d.CreateElement(ITEMELEMENT);
            for (int i = 0; i < fields.Length; i++)
            {
                string err;
                XmlAttribute attr = d.CreateAttribute(fields[i]);
                Object o = mp.GetObjectByIndex("Payoffs." + fields[i], index, out err);
                if (o != null)
                {
                    attr.Value = o.ToString();
                    borrower.Attributes.Append(attr);
                }
            }
            return borrower;
        }
        private static XmlNode GetPropertyNode(XmlDocument d, string[] fields, MortgageProfile mp)
        {
            XmlNode borrower = d.CreateElement(ITEMELEMENT);
            for (int i = 0; i < fields.Length; i++)
            {
                string err;
                XmlAttribute attr = d.CreateAttribute(fields[i]);
                Object o = mp.GetObjectByIndex("Property." + fields[i], -1, out err);
                if (o != null)
                {
                    attr.Value = o.ToString();
                    borrower.Attributes.Append(attr);
                }
            }
            return borrower;
        }
        private static XmlNode GetMortgageInfoNode(XmlDocument d, string[] fields, MortgageProfile mp)
        {
            XmlNode borrower = d.CreateElement(ITEMELEMENT);
            for (int i = 0; i < fields.Length; i++)
            {
                string err;
                XmlAttribute attr = d.CreateAttribute(fields[i]);
                Object o = mp.GetObjectByIndex("MortgageInfo." + fields[i], -1, out err);
                if (o != null)
                {
                    attr.Value = o.ToString();
                    borrower.Attributes.Append(attr);
                }
            }
            return borrower;
        }
        #endregion

        #endregion
    }
}

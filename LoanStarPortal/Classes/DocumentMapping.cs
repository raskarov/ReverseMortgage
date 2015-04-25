using System;
using System.Collections;
using System.IO;
using Aspose.Words;



namespace LoanStar.Common
{
    #region for future use
    // public enum TemplateFieldType
    //{
    //    FieldText = 0, 
    //    FieldBool = 1
    //}

    //public class DocumentMapping :BaseObject
    //{
    //    #region constants
    //    private const string ERRTEMPLATENOTFOUND = "Template file {0} not found";
    //    private const string ERREMPTYFIELDNAME = "Field name can not be empty";
    //    private const string ERRWRONGREGIONFIELDNAME = "Field {0} inside region {1} must contain Region name and point";
    //    private const string ERRNESTEDREGIONS = "Region can not be nested inside another region {0}";
    //    private const string ERREMPTYREGIONAME = "Region name near TABLESTART prefix can not be empty";
    //    private const string ERRDUPLICATEREGIONNAME = "Region {0} allready is used in region list";
    //    private const string ERRUNCLOSEDREGION = "You need to close region {0}";
    //    private const string ERREMPTYREGIONEBDNAME = "Region name near TABLEEND prefix can not be empty";
    //    private const string ERRREGIONNAMENOTEQUAL = "Region near TABLEEND prefix must be equal to region {0} near TABLESTART prefix";
    //    private const string ERRINCORRECTFORMAT = "Incorrect field format of Doc Template Version";
    //    #endregion

    //    #region fields
    //    private readonly string templateFileName;
    //    private MemoryStream result;
    //    private string errorMessage = String.Empty;
    //    private readonly License wordLicense = new License();
    //    private readonly Aspose.Pdf.License pdfLicense = new Aspose.Pdf.License();
    //    private Document templateDocument;

    //    #endregion

    //    #region properties
    //    public string ErrorMessage
    //    {
    //        get { return errorMessage; }
    //    }
    //    public MemoryStream Result
    //    {
    //        get { return result; }
    //    }
    //    #endregion

    //    #region constructor
    //    public DocumentMapping(string fileName)
    //    {
    //        pdfLicense.SetLicense("Aspose.Custom.lic");
    //        wordLicense.SetLicense("Aspose.Custom.lic");
    //        templateFileName = fileName;
    //    }
    //    #endregion

    //    #region methods
    //    public bool Map(MortgageProfile mp)
    //    {
    //        bool res = false;
    //        FileInfo templateFile = new FileInfo(templateFileName);
    //        if(!templateFile.Exists)
    //        {
    //            errorMessage = String.Format(ERRTEMPLATENOTFOUND, templateFileName);
    //            return res;
    //        }
    //        Stream inputStream = null;
    //        try
    //        {
    //            inputStream = templateFile.OpenRead();
    //            templateDocument = new Document(inputStream);
    //            Hashtable fields = GetFields(inputStream);
    //            byte[] filledDocument = FillTemplate(mp, fields);
                
    //        }
    //        catch(Exception ex)
    //        {
    //            res = false;
    //            errorMessage = ex.Message;
    //        }
    //        finally
    //        {
    //            if(inputStream!=null)
    //            {
    //                inputStream.Close();
    //            }
    //        }
    //        return res;
    //    }
    //    private byte[] FillTemplate(MortgageProfile mp, Hashtable fields)
    //    {
    //        byte[] res=null;
    //        Hashtable regionHash = new Hashtable();
    //        foreach (DocTemplateField dtField in fields.Values)
    //        {
    //            ArrayList valueList = null;
    //            MortgageProfileField mpField = (MortgageProfileField)mpFieldHash[dtField.MPFieldID];
    //            if (mpField != null && mpField.ID > 0)
    //            {
    //                try
    //                {
    //                    mp.PopulateMPField(mpField, dtField);
    //                    if (mpField.FieldValue != null)
    //                    {
    //                        if (mpField.FieldValue is ArrayList)
    //                            valueList = (ArrayList)mpField.FieldValue;
    //                        else
    //                        {
    //                            valueList = new ArrayList();
    //                            valueList.Add(mpField.FieldValue);
    //                        }
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    log.WriteLine(ex.Message);
    //                }
    //            }

    //            if (!regionHash.ContainsKey(dtField.RegionName))
    //                regionHash[dtField.RegionName] = dtField.GetRegionTable();
    //            DataTable regionTable = (DataTable)regionHash[dtField.RegionName];

    //            if (valueList == null || valueList.Count == 0)
    //                regionTable.Columns.Add(dtField.DTVFieldName);
    //            else
    //            {
    //                regionTable.Columns.Add(dtField.DTVFieldName, valueList[0].GetType());
    //                for (int i = 0; i < valueList.Count; i++)
    //                {
    //                    if (i < regionTable.Rows.Count)
    //                    {
    //                        DataRow foundRow = regionTable.Rows[i];
    //                        foundRow[dtField.DTVFieldName] = valueList[i] ?? DBNull.Value;
    //                    }
    //                    else
    //                    {
    //                        DataRow newRow = regionTable.NewRow();
    //                        newRow[dtField.DTVFieldName] = valueList[i] ?? DBNull.Value;
    //                        regionTable.Rows.Add(newRow);
    //                    }
    //                    log.WriteFieldInfo(dtField.DTVFieldName, valueList[i] == null ? String.Empty : valueList[i].ToString());
    //                }
    //            }
    //        }
    //        return res;
    //    }

    //    private Hashtable GetFields(Stream inputStream)
    //    {
    //        Hashtable ht = new Hashtable();

    //        ArrayList regionList = new ArrayList();
    //        string regionName = String.Empty;
    //        string[] fields = templateDocument.MailMerge.GetFieldNames();
    //        foreach (string fieldName in fields)
    //        {
    //            int delimInd = fieldName.LastIndexOf(':');
    //            if (delimInd < 0)
    //            {
    //                if (String.IsNullOrEmpty(fieldName))
    //                {
    //                    throw new Exception(ERREMPTYFIELDNAME);
    //                }
    //                else if (!String.IsNullOrEmpty(regionName) && fieldName.IndexOf(String.Format("{0}.", regionName)) != 0)
    //                {
    //                    throw new Exception(String.Format(ERRWRONGREGIONFIELDNAME, fieldName, regionName));
    //                }
    //                TemplateField docTemplateField = new TemplateField(fieldName, DTVFieldType.FieldText, regionName);
    //                ht[docTemplateField.Key] = docTemplateField;
    //            }
    //            else if (fieldName.ToUpper().Contains("TABLESTART"))
    //            {
    //                string foundRegionName = fieldName.Substring(delimInd + 1);

    //                if (!String.IsNullOrEmpty(regionName))
    //                {
    //                    throw new Exception(String.Format(ERRNESTEDREGIONS, regionName));
    //                }
    //                else if (String.IsNullOrEmpty(foundRegionName))
    //                {
    //                    throw new Exception(ERREMPTYREGIONAME);
    //                }
    //                else if (regionList.Contains(foundRegionName.ToUpper()))
    //                {
    //                    throw new Exception(String.Format(ERRDUPLICATEREGIONNAME, foundRegionName));
    //                }
    //                regionName = foundRegionName;
    //                regionList.Add(foundRegionName.ToUpper());
    //            }
    //            else if (fieldName.ToUpper().Contains("TABLEEND"))
    //            {
    //                string foundRegionName = fieldName.Substring(delimInd + 1);

    //                if (String.IsNullOrEmpty(regionName))
    //                {
    //                    throw new Exception(String.Format(ERRUNCLOSEDREGION, regionName));
    //                }
    //                else if (String.IsNullOrEmpty(foundRegionName))
    //                {
    //                    throw new Exception(ERREMPTYREGIONEBDNAME);
    //                }
    //                else if (String.Compare(regionName, foundRegionName, true) != 0)
    //                {
    //                    throw new Exception(String.Format(ERRREGIONNAMENOTEQUAL, regionName));
    //                }
    //                regionName = String.Empty;
    //            }
    //            else
    //            {
    //                throw new Exception(ERRINCORRECTFORMAT);
    //            }
    //        }
    //        return ht;
    //    }

    //    #endregion

    //}
    
    //public class TemplateField
    //{
    //    #region fields
    //    private string propertyName = String.Empty;
    //    private TemplateFieldType type;
    //    private string regionName = String.Empty;
    //    #endregion

    //    #region properties
    //    public string Key
    //    {
    //        get
    //        {
    //            return String.IsNullOrEmpty(regionName) ? propertyName : String.Format("{0}:{1}", regionName, propertyName);
    //        }
    //    }
    //    #endregion

    //    #region constructor
    //    public TemplateField(string propertyName_, TemplateFieldType type_, string regionName_)
    //    {
    //        propertyName = propertyName_;
    //        type = type_;
    //        regionName = regionName_;
    //    }

    //    #endregion
    //}
    #endregion
}

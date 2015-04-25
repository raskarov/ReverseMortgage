using System;
using System.Data;

namespace LoanStar.Common
{
    public class FieldInfo : BaseObject
    {
        int mortgageProfileID;
        bool addedToDB;
        bool addedToUI;
        bool signedOff;
        string entity;
        string label;
        int uiControlID;
        string _value;
        string fieldName;
        int generalPurposeID;
        int valueTypeID;
        string description;
        int siteTypeID;
        string path1;
        string path2;
        string path3;
        string path4;
        string path5;

        #region Properties
        public int MortgageProfileID
        {
            set{mortgageProfileID=value;}
            get{return mortgageProfileID;}
        }
        public bool AddedToDB
        {
            set{addedToDB=value;}
            get{return addedToDB;}
        }
        public bool AddedToUI
        {
            set{addedToUI=value;}
            get{return addedToUI;}
        }
        public bool SignedOff
        {
            set{signedOff=value;}
            get{return signedOff;}
        }
        public string Entity
        {
            set{entity=value;}
            get{return entity;}
        }
        public string Label
        {
            set{label=value;}
            get{return label;}
        }
        public int UIControlID
        {
            set{uiControlID=value;}
            get{return uiControlID;}
        }
        public string Value
        {
            set{_value=value;}
            get{return _value;}
        }
        public string FieldName
        {
            set{fieldName=value;}
            get{return fieldName;}
        }
        public int GeneralPurposeID
        {
            set{generalPurposeID=value;}
            get{return generalPurposeID;}
        }
        public int ValueTypeID
        {
            set{valueTypeID=value;}
            get{return valueTypeID;}
        }
        public string Description
        {
            set{description=value;}
            get{return description;}
        }
        public int SiteTypeID
        {
            set{siteTypeID=value;}
            get{return siteTypeID;}
        }
        public string Path1
        {
            set{path1=value;}
            get{return path1;}
        }
        public string Path2
        {
            set{path2=value;}
            get { return path2; }
        }
        public string Path3
        {
            set { path3 = value; }
            get { return path3; }
        }
        public string Path4
        {
            set { path4 = value; }
            get { return path4; }
        }
        public string Path5
        {
            set { path5 = value; }
            get { return path5; }
        }
        #endregion

        #region Constructors
        public FieldInfo()
            : this(0)
        { }
        public FieldInfo(int id)
        {
            ID = id;
            if (id > 0)
            {
                LoadById();
            }
        }
        #endregion


        private void LoadById()
        {
            DataView dv = db.GetDataView("CMS_GetFieldInfo", ID);
            if (dv.Count == 1)
            {
                ID = (dv[0]["id"]==DBNull.Value)?-1:Convert.ToInt32(dv[0]["id"]);
                mortgageProfileID = (dv[0]["MortgageProfileID"] == DBNull.Value) ? -1 : Convert.ToInt32(dv[0]["MortgageProfileID"]);
                addedToDB = (dv[0]["AddedToDB"] == DBNull.Value) ? false : Convert.ToBoolean(dv[0]["AddedToDB"]);
                addedToUI = (dv[0]["AddedToUI"] == DBNull.Value) ? false : Convert.ToBoolean(dv[0]["AddedToUI"]);
                signedOff = (dv[0]["SignedOff"] == DBNull.Value) ? false : Convert.ToBoolean(dv[0]["SignedOff"]);
                entity = (dv[0]["Entity"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Entity"]);
                label = (dv[0]["Label"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Label"]);
                uiControlID = (dv[0]["UIControlID"] == DBNull.Value) ? -1 : Convert.ToInt32(dv[0]["UIControlID"]);
                _value = (dv[0]["Value"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Value"]);
                fieldName = (dv[0]["FieldName"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["FieldName"]);
                generalPurposeID = (dv[0]["GeneralPurposeID"] == DBNull.Value) ? -1 : Convert.ToInt32(dv[0]["GeneralPurposeID"]);
                valueTypeID = (dv[0]["ValueTypeID"] == DBNull.Value) ? -1 : Convert.ToInt32(dv[0]["ValueTypeID"]);
                description = (dv[0]["Description"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Description"]);
                siteTypeID = (dv[0]["SiteTypeID"] == DBNull.Value) ? -1 : Convert.ToInt32(dv[0]["SiteTypeID"]);
                path1 = (dv[0]["Path1"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Path1"]);
                path2 = (dv[0]["Path2"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Path2"]);
                path3 = (dv[0]["Path3"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Path3"]);
                path4 = (dv[0]["Path4"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Path4"]);
                path5 = (dv[0]["Path5"] == DBNull.Value) ? "" : Convert.ToString(dv[0]["Path5"]);
            }
            else
            {
                ID = 0;
            }
        }


        #region Static procedures
        public static DataTable GetFieldInfoList(string entity)
        {
            entity = "%"+entity.Trim() + "%";
            return db.GetDataTable("CMS_GetFieldInfoList", entity);
        }

        public static DataTable GetEntityList()
        {
            return db.GetDataTable("CMS_GetEntityList");
        }

        public static DataTable GetSiteTypeList()
        {
            return db.GetDataTable("CMS_GetSiteTypeList");
        }
        public static DataTable GetUIControlList()
        {
            return db.GetDataTable("CMS_GetUIControlList");
        }
        public static DataTable GetValueTypeList()
        {
            return db.GetDataTable("CMS_GetValueTypeList");
        }
        public static DataTable GetGeneralPurposeList()
        {
            return db.GetDataTable("CMS_GetGeneralPurposeList");
        }

        #endregion

        public void Save()
        {
            db.ExecuteScalar("CMS_SaveFieldInfo", ID,	(MortgageProfileID==0)?DBNull.Value:(object)MortgageProfileID, AddedToDB,	AddedToUI,	SignedOff,
			    Entity,Label,UIControlID,_value,FieldName,GeneralPurposeID,ValueTypeID,Description,SiteTypeID,
			    Path1,Path2,Path3,Path4,Path5);

        }
    }
}
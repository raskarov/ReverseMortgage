using System;
using System.Text;
using System.Collections;
using System.Data.OleDb;
using System.Data;

namespace LoanStar.Common
{
    public class ExcelWrapper
    {
        #region constants
//        private const string EXCELCONNECTIONSTRING = "Provider=Microsoft.Jet.OLEDB.4.0;Data source={0};Extended Properties='Excel 8.0;HDR={1};IMEX=1;'";
        private const string EXCELCONNECTIONSTRING = "Provider=Microsoft.Jet.OLEDB.4.0;Data source={0};Extended Properties='Excel 8.0;HDR={1};{2}'";
        private const string SELECTALLCOMMAND = "Select * FROM [{0}]";
        #endregion

        #region fields
        private readonly string fileName;
        private readonly bool hasHeaderRow;
        private string sheetName;
        private string errorMessage;
        private DataTable dtData;
        private readonly string connectionString;
        #endregion

        #region properties
        public string ErrorMessage
        {
            get { return errorMessage; }
        }
        private string SheetName
        {
            get
            {
                if(String.IsNullOrEmpty(sheetName))
                {
                    sheetName = GetFirstSheetName();
                }
                return sheetName;
            }
        }
        public DataTable DtData
        {
            get
            {
                if(dtData==null)
                {
                    dtData = GetData();
                }
                return dtData;
            }
        }
        #endregion

        #region constructor
        public ExcelWrapper(string fileName_, bool hasHeaderRow_) : this(fileName_,hasHeaderRow_,true)
        {
            //fileName = fileName_;
            //hasHeaderRow = hasHeaderRow_;
            //connectionString = String.Format(EXCELCONNECTIONSTRING, fileName, hasHeaderRow ? "Yes" : "No", imex ? "IMEX=1;" : "");
        }
        public ExcelWrapper(string fileName_, bool hasHeaderRow_, bool imex)
        {
            fileName = fileName_;
            hasHeaderRow = hasHeaderRow_;
            connectionString = String.Format(EXCELCONNECTIONSTRING, fileName, hasHeaderRow ? "Yes" : "No", imex?"IMEX=1;":"");
        }
        #endregion

        #region methods
        #region public
        public void InsertDataRow(DataRow row)
        {
            OleDbConnection con = new OleDbConnection(connectionString);
            try
            {
                string ins = GenerateInsertStatement(row);
//                string ins =
//                    "INSERT INTO [qryExportHOAll$](HomeOwnerID,\"FIRST NAME 1\",\"LAST NAME 1\")VALUES('6207','Aaron','Garrett')";
//                    "INSERT INTO [qryExportHOAll$](HomeOwnerID,[FIRST NAME 1])VALUES('6207','FirstName')";
                OleDbCommand cmd = new OleDbCommand(ins, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                con.Dispose();
            }

        }
        private string GenerateInsertStatement(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            bool firstcol = true;
            sb.AppendFormat("INSERT INTO [{0}](", SheetName);


            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (!firstcol)
                {
                    sb.Append(",");
                }
                firstcol = false;

                sb.Append("["+dc.Caption+"]");
            }

            sb.Append(") VALUES(");
            firstcol = true;
            for (int i = 0; i <= dr.Table.Columns.Count - 1; i++)
            {
                if (!object.ReferenceEquals(dr.Table.Columns[i].DataType, typeof(int)))
                {
                    sb.Append("'");
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                    sb.Append("'");
                }
                else
                {
                    sb.Append(dr[i].ToString().Replace("'", "''"));
                }
                if (i != dr.Table.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }


        public ArrayList GetColumnNames()
        {
            ArrayList res = new ArrayList();
            for(int i=0; i<DtData.Columns.Count; i++)
            {
                res.Add(DtData.Columns[i].ColumnName);
            }
            return res;
        }
        #endregion
        #region private
        private string GetFirstSheetName()
        {
            string res = "";
            OleDbConnection con = new OleDbConnection(connectionString);
            try
            {
                con.Open();
                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt != null)
                {
                    res = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                else
                {
                    errorMessage = "No sheets found";
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                con.Dispose();
            }
            return res;
        }
        private DataTable GetData()
        {
            DataTable res = null;
            DataSet ds = new DataSet();
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection con = new OleDbConnection(connectionString);
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = String.Format(SELECTALLCOMMAND, SheetName);
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            try
            {
                con.Open();
                adapter.Fill(ds);
                res = ds.Tables[0];
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                con.Dispose();
            }
            return res;
        }
        #endregion

        public static int ConvertToIndex(string column)
        {
            int res = 0;
            string s = column.ToUpper();
            if(s.Length==1)
            {
                res = Convert.ToInt32(s[0]) - 64;
            }
            else if(s.Length==2)
            {
                res = Convert.ToInt32(s[1]) - 64;
                res += (Convert.ToInt32(s[0]) - 64)*26;
            }
            else if(s.Length == 3)
            {
                res = Convert.ToInt32(s[2]) - 64;
                res += (Convert.ToInt32(s[1]) - 64) * 26;
                res += (Convert.ToInt32(s[0]) - 64) * 26 * 26;
            }
            res -= 1;
            return res;
        }

        #endregion
    }
}

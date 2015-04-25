using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Calendar_NET;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.Odbc;
using WebMailPro;

enum CalendarDBVariants
{
    MsSql=0,
    OleDb,
    MySql,
    Odbc
}

public interface ICalendarDB
{
    DataTable ExecuteQuery(string sqlCmd);
    DataTable ExecuteQuery(string sqlCmd, NameValueCollection pars);
    int       ExecuteNonQueryAndGetLastID(string sqlCmd, NameValueCollection pars);
    bool      ExecuteNonQuery(string sqlCmd, NameValueCollection pars);
}

public class CalendarDBWorker
{
	const string MsSQL_CS = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};";
	const string MySQL_CS = "Server={0};Database={1};Uid={2};Pwd={3};CharSet=utf8;";
    const string Access2000_CS = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};"; //User Id=admin;Password=;
    const string Access2007_CS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;";

    private IDbConnection _base_connection = null;
    private ICalendarDB _data_base = null;
    private CalendarDBVariants _dbType;

	public CalendarDBWorker(WebmailSettings settings):this(settings.DbType, settings.DbHost,
		settings.DbName, settings.DbDsn, settings.DbLogin, settings.DbPassword, 
		settings.DbPathToMdb, settings.UseCustomConnectionString, settings.DbCustomConnectionString)
	{
	}

    public CalendarDBWorker(SupportedDatabase dbType, string host, string dbname, string dsn, string login,
		string pass, string mdbPath, bool useCustomCS, string customCS)
    {
        if (!useCustomCS && dsn == string.Empty)
        {
            switch (dbType)
            {
                case SupportedDatabase.MsAccess:
                    string cs = "";
                    if (mdbPath.EndsWith("accdb"))
                        cs = string.Format(Access2007_CS, mdbPath);
                    else
                        cs = string.Format(Access2000_CS, mdbPath);

                    _base_connection = new OleDbConnection(cs);
                    _dbType = CalendarDBVariants.OleDb;
                    break;
                case SupportedDatabase.MySql:
                    _base_connection = new MySqlConnection(string.Format(MySQL_CS, host, dbname, login, pass));
                    _dbType = CalendarDBVariants.MySql;
                    break;
                case SupportedDatabase.MsSqlServer:
                    _base_connection = new SqlConnection(string.Format(MsSQL_CS, host, dbname, login, pass));
                    _dbType = CalendarDBVariants.MsSql;
                    break;
            }
        }
        else
        {
            if (useCustomCS)
                _base_connection = new OdbcConnection(customCS);
            if (dsn!=string.Empty && !useCustomCS)
            {
                switch (dbType)
                {
                    case SupportedDatabase.MsAccess:
                        _base_connection = new OdbcConnection("DSN=" + dsn);
                        break;
                    //DSN=myDsn;Uid=myUsername;Pwd=;
                    case SupportedDatabase.MsSqlServer:
                    case SupportedDatabase.MySql:
                        _base_connection = new OdbcConnection("DSN=" + dsn + ";Uid=" + login + ";Pwd=" + pass);
                        break;
                }
            }
            _dbType = CalendarDBVariants.Odbc;
        }

        switch (_dbType)
        {
            case CalendarDBVariants.Odbc:
                _data_base = new OdbcDBWorker(_base_connection, dbType);
                break;
            case CalendarDBVariants.MsSql:
                _data_base = new MsSQLDBWorker(_base_connection);
                break;
            case CalendarDBVariants.MySql:
                _data_base = new MySQLDBWorker(_base_connection);
                break;
            case CalendarDBVariants.OleDb:
                _data_base = new AccessDBWorker(_base_connection);
                break;
        }
    }

    public DataTable ExecuteQuery(string sqlCmd, NameValueCollection pars)
    {
        return _data_base.ExecuteQuery(sqlCmd, pars);
    }
    public DataTable ExecuteQuery(string sqlCmd)
    {
        return _data_base.ExecuteQuery(sqlCmd);
    }

    public int ExecuteNonQueryAndGetLastID(string sqlCmd, NameValueCollection pars)
    {
        return _data_base.ExecuteNonQueryAndGetLastID(sqlCmd, pars);
    }
    public bool ExecuteNonQuery(string sqlCmd, NameValueCollection pars)
    {
        return _data_base.ExecuteNonQuery(sqlCmd, pars);
    }

    public IDbConnection GetCurrentConnection()
    {
        return _base_connection;
    }
}

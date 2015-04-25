using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Odbc;
using System.Collections.Specialized;
using Calendar_NET;
using WebMailPro;

/// <summary>
/// Summary description for OdbcDBWorker
/// </summary>
public class OdbcDBWorker : ICalendarDB
{
    SupportedDatabase _dbType;
    OdbcConnection _connect = null;

	public OdbcDBWorker(IDbConnection conn,SupportedDatabase dbt)
	{
        _dbType = dbt;
        _connect = conn as OdbcConnection;
	}

    public int ExecuteNonQueryAndGetLastID(string sqlCmd, NameValueCollection pars)
    {
        OdbcTransaction trans = null;
        OdbcCommand cmd = null;
        int lid = 0;

        try
        {
            if (_connect.State == ConnectionState.Closed || _connect.State == ConnectionState.Broken)
                _connect.Open();
            //_connect.Open();
            trans = _connect.BeginTransaction();

            cmd = new OdbcCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new OdbcParameter(pars.Keys[i], pars.Get(i)));
            }
            cmd.ExecuteNonQuery();
            trans.Commit();
            cmd.Dispose();

            string selectCmd = "SELECT " + GetLastInsertIdCmd();
            cmd = new OdbcCommand(selectCmd, _connect);
            lid = int.Parse(cmd.ExecuteScalar().ToString());
            return lid;
        }
        catch (Exception ex)
        {
            ExecutionLog.WriteException(ex);
            trans.Rollback();
            return 0;
        }
        finally
        {
            if (_connect != null && _connect.State == ConnectionState.Open)
            {
                _connect.Close();
                cmd.Dispose();
                trans.Dispose();
            }

        }
    }
    public bool ExecuteNonQuery(string sqlCmd, NameValueCollection pars)
    {
        OdbcTransaction trans = null;
        OdbcCommand cmd = null;

        try
        {
            _connect.Open();
            trans = _connect.BeginTransaction();

            cmd = new OdbcCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new OdbcParameter(pars.Keys[i], pars.Get(i)));
            }
            cmd.ExecuteNonQuery();

            trans.Commit();
            return true;
        }
        catch (Exception ex)
        {
            ExecutionLog.WriteException(ex);
            trans.Rollback();
            return false;
        }
        finally
        {
            if (_connect != null && _connect.State == ConnectionState.Open)
            {
                _connect.Close();
                cmd.Dispose();
                trans.Dispose();
            }

        }
    }

    public DataTable ExecuteQuery(string sqlCmd, NameValueCollection pars)
    {
        OdbcDataAdapter da = null;
        OdbcCommand cmd = null;

        try
        {
            _connect.Open();
            DataSet ds = new DataSet();
            cmd = new OdbcCommand(sqlCmd, _connect);
            for (int i = 0; i < pars.Count; i++)
                cmd.Parameters.Add(new OdbcParameter(pars.Keys[i], pars.Get(i)));

            da = new OdbcDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
        catch (Exception ex)
        {
            ExecutionLog.WriteException(ex);
            return null;
        }
        finally
        {
            if (_connect != null && _connect.State == ConnectionState.Open)
            {
                _connect.Close();
                da.Dispose();
            }

        }
    }
    public DataTable ExecuteQuery(string sqlCmd)
    {
        OdbcDataAdapter da = null;
        OdbcCommand cmd = null;

        try
        {
            _connect.Open();
            DataSet ds = new DataSet();
            cmd = new OdbcCommand(sqlCmd, _connect);

            da = new OdbcDataAdapter(cmd);
            da.Fill(ds);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
        catch (Exception ex)
        {
            ExecutionLog.WriteException(ex);
            return null;
        }
        finally
        {
            if (_connect != null && _connect.State == ConnectionState.Open)
            {
                _connect.Close();
                da.Dispose();
            }

        }
    }

    private string GetLastInsertIdCmd()
    {
        string ret = "";

        switch (_dbType)
        {
            case SupportedDatabase.MsAccess:
            case SupportedDatabase.MsSqlServer:
                ret = "@@identity";
                break;
            case SupportedDatabase.MySql:
                ret = "LAST_INSERT_ID()";
                break;
        }
        return ret;
    }

}

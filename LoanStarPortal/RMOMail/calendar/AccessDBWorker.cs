using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Collections.Specialized;
using Calendar_NET;

public class AccessDBWorker : ICalendarDB
{
    const string Access_GetLastID = "SELECT @@identity";

    OleDbConnection _connect = null;

	public AccessDBWorker(IDbConnection conn)
	{
        _connect = conn as OleDbConnection;
	}

    public DataTable ExecuteQuery(string sqlCmd, NameValueCollection pars)
    {
        OleDbDataAdapter da = null;
        OleDbCommand cmd = null;

        try
        {
            if (_connect.State != ConnectionState.Open)
                _connect.Open();

            DataSet ds = new DataSet();
            cmd = new OleDbCommand(sqlCmd, _connect);
            for (int i = 0; i < pars.Count; i++)
                cmd.Parameters.Add(new OleDbParameter(pars.Keys[i], pars.Get(i)));

            da = new OleDbDataAdapter(cmd);
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
        OleDbDataAdapter da = null;
        OleDbCommand cmd = null;

        try
        {
            if (_connect.State != ConnectionState.Open)
                _connect.Open();

            DataSet ds = new DataSet();
            cmd = new OleDbCommand(sqlCmd, _connect);

            da = new OleDbDataAdapter(cmd);
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

    public bool ExecuteNonQuery(string sqlCmd, NameValueCollection pars)
    {
        OleDbTransaction trans = null;
        OleDbCommand cmd = null;

        try
        {
            if (_connect.State != ConnectionState.Open)
                _connect.Open();

            trans = _connect.BeginTransaction();

            cmd = new OleDbCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new OleDbParameter(pars.Keys[i], pars.Get(i)));
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
    public int ExecuteNonQueryAndGetLastID(string sqlCmd, NameValueCollection pars)
    {
        OleDbTransaction trans = null;
        OleDbCommand cmd = null;
        int lid = 0;

        try
        {
            if (_connect.State == ConnectionState.Closed || _connect.State == ConnectionState.Broken)
                _connect.Open();

            trans = _connect.BeginTransaction();

            cmd = new OleDbCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new OleDbParameter(pars.Keys[i], pars.Get(i)));
            }
            cmd.ExecuteNonQuery();
            trans.Commit();
            cmd.Dispose();

            cmd = new OleDbCommand(Access_GetLastID, _connect);
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
}

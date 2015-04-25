using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;
using WebMailPro;
using Calendar_NET;

/// <summary>
/// Summary description for MySQLDBWorker
/// </summary>
public class MySQLDBWorker : ICalendarDB
{
    const string MySql_GetLastID = "SELECT LAST_INSERT_ID()";

    MySqlConnection _connect = null;

	public MySQLDBWorker(IDbConnection conn)
	{
        _connect = conn as MySqlConnection;    
	}

    public DataTable ExecuteQuery(string sqlCmd, NameValueCollection pars)
    {
        MySqlDataAdapter da = null;
        MySqlCommand cmd = null;

        try
        {
			if (_connect.State != ConnectionState.Open)
				_connect.Open();

            DataSet ds = new DataSet();
            cmd = new MySqlCommand(sqlCmd, _connect);
            for (int i = 0; i < pars.Count; i++)
                cmd.Parameters.Add(new MySqlParameter(pars.Keys[i], pars.Get(i)));

            da = new MySqlDataAdapter(cmd);
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
        MySqlDataAdapter da = null;
        MySqlCommand cmd = null;

        try
        {
			if (_connect.State != ConnectionState.Open)
				_connect.Open();

            DataSet ds = new DataSet();
            cmd = new MySqlCommand(sqlCmd, _connect);

            da = new MySqlDataAdapter(cmd);
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
        MySqlTransaction trans = null;
        MySqlCommand cmd = null;

        try
        {
			if (_connect.State != ConnectionState.Open)
				_connect.Open();

            trans = _connect.BeginTransaction();

            cmd = new MySqlCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new MySqlParameter(pars.Keys[i], pars.Get(i)));
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
        MySqlTransaction trans = null;
        MySqlCommand cmd = null;
        int lid = 0;

        try
        {
			if (_connect.State == ConnectionState.Closed || _connect.State == ConnectionState.Broken)
				_connect.Open();

            trans = _connect.BeginTransaction();

            cmd = new MySqlCommand(sqlCmd, _connect);
            cmd.Transaction = trans;
            if (pars != null)
            {
                for (int i = 0; i < pars.Count; i++)
                    cmd.Parameters.Add(new MySqlParameter(pars.Keys[i], pars.Get(i)));
            }
            cmd.ExecuteNonQuery();
            trans.Commit();
            cmd.Dispose();

            cmd = new MySqlCommand(MySql_GetLastID, _connect);
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

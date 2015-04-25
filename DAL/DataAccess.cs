using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace BossDev.CommonUtils
{
	/// <summary>
	/// Encapsulates database related routines. Exposes usefull methods
	/// that allows easy accessing to database objects. Examples:
	/// 
	/// DatabaseAccess db = new DatabaseAccess("server=sqlServerName;uid=***;pwd=***;database=dbName;Connect Timeout=1000;");
	///
	///	DataTable table = db.GetDataTable("select * from member");
	///	foreach (DataRow row in table.Rows)
	///		Console.WriteLine(row[0]);
	///
	///	using (SqlDataReader reader = db.GetDataReader("select * from member"))
	///		while (reader.Read())
	///			Console.WriteLine(reader[0]);
	///
	///	DataSet dataset = db.GetDataSet("select * from member");
	///	if (dataset.Tables.Count > 0)
	///		foreach (DataRow row in dataset.Tables[0].Rows)
	///			Console.WriteLine(row[0]);
	///			
	///	string name = db.ExecuteScalarString("sp_get_member_name", ID);
	///
	/// Console.WriteLine(PrepareSqlString("sp_some", DateTime.Now, true, DBNull.Value, "string with ' char"));
	/// The output:
	/// sp_some '2004.03.04 14:05:21.310', 1, null, 'string with '' char'
	/// 
	///	db.BeginTransaction();
	///	try
	///	{
	///		int result = DB.ExecuteScalarInt("sp_some", memberID, subject, body);
	///		db.CommitTransaction();
	///	}	
	///	catch 
	///	{
	///		db.RollbackTransaction();
	///		throw;
	///	}
	/// </summary>
	public class DatabaseAccess
	{
		/// <summary>
		/// Any method of DatabaseAccess class must throw custom exceptions of this type only
		/// </summary>
		public class DatabaseAccessException : Exception
		{
			public DatabaseAccessException(string message, Exception innerException) : base(message, innerException)
			{
			}

			public DatabaseAccessException(string message) : base(message)
			{
			}

			public DatabaseAccessException() : base()
			{
			}
		}

		public readonly string SqlConnectionString = "";
		public const string DEFAULT_DATASET_NAME = "QueryResult";

		/// <summary>
		/// This properties is used internally for transaction support
		/// </summary>
		private SqlConnection _connection;
		private SqlTransaction _transaction;

		/// <summary>
		/// Returns SqlTransaction object if transaction has been started,
		/// otherwise null
		/// </summary>
		public SqlTransaction CurrentTransaction
		{
			get
			{
				return _transaction;
			}
		}

		/// <summary>
		/// Returns previously created connection if the transaction has been started 
		/// or creates a new connection. The connection returned is always opened.
		/// </summary>
		public SqlConnection GetConnection()
		{
			if (_connection != null)
				return _connection;
			else
			{
				SqlConnection connection = new SqlConnection(SqlConnectionString);
				connection.Open();
				return connection;
			}
		}

		/// <summary>
		/// Default DatabaseAccess constructor
		/// </summary>
		/// <param name="sqlConnectionString">Connection string used to connect to database server</param>
		public DatabaseAccess(string sqlConnectionString)
		{
			SqlConnectionString = sqlConnectionString;
		}

		/// <summary>
		/// For a given SQL command returns a result set as an instance of DataTable
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public DataTable GetDataTable(string sql) 
		{
			SqlConnection connection = GetConnection();
			try
			{
				DataTable table = new DataTable();
				SqlCommand command = new SqlCommand(sql, connection, _transaction);
				new SqlDataAdapter(command).Fill(table);
				return table;
			}
			finally
			{
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();
			}
		}

		/// <summary>
		/// For a given SQL command returns a result set as an instance of DataAdapter
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public SqlDataAdapter GetDataAdapter(string sql, DataSet ds, SqlConnection connection) 
		{
			//SqlConnection connection = GetConnection();
			try
			{
				SqlCommand command = new SqlCommand(sql, connection, _transaction);
				SqlDataAdapter da = new SqlDataAdapter(command);
				da.Fill(ds);
				return da;
			}
			finally
			{/*
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();*/
			}
		}


		/// <summary>
		/// For a given SQL command returns a result set as an instance of DataView
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public DataView GetDataView(string sql) 
		{
			return GetDataTable(sql).DefaultView;
		}

		/// <summary>
		/// For a given SQL command returns a result set as an instance of DataSet
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public DataSet GetDataSet(string sql) 
		{
			SqlConnection connection = GetConnection();
			try
			{
				DataSet dataset = new DataSet(DEFAULT_DATASET_NAME);
				SqlCommand command = new SqlCommand(sql, connection, _transaction);
				new SqlDataAdapter(command).Fill(dataset);
				return dataset;
			}
			finally
			{
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();
			}
		}

		/// <summary>
		/// For a given SQL command returns a result set as an instance of SqlDataReader.
		/// Do not forget to close it!
		/// The method does not work with transaction mechanism (it always creates a new connection).
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public SqlDataReader GetDataReader(string sql)
		{
			SqlConnection connection = new SqlConnection(SqlConnectionString);
			connection.Open();
			return new SqlCommand(sql, connection).ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// For a given SQL command returns a first row from result set as an instance of DataRow.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public DataRow GetSingleRow(string sql)
		{
			DataTable table = GetDataTable(sql);
			if (table.Rows.Count > 0)
				return table.Rows[0];
			else
				throw new DatabaseAccessException("The result set is empty");
		}

		/// <summary>
		/// Executes a given SQL command returning no result set
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		public void Execute(string sql)
		{
			SqlConnection connection = GetConnection();
			SqlCommand command = new SqlCommand(sql, connection, _transaction);
			try
			{
				command.ExecuteNonQuery();
			}
			finally
			{
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();
			}
		}
		/// <summary>
		/// Executes a given SQL command returning no result set
		/// </summary>
		/// <param name="command">SQL command string to execute</param>
		public void Execute(SqlCommand command)
		{
			SqlConnection connection = GetConnection();
			command.Connection = connection;
			command.Transaction = _transaction;
			//SqlCommand command = new SqlCommand(sql, connection, _transaction);
			try
			{
				command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				throw ex;
			}

			finally
			{
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();
			}
		}
        public int ExecuteCommandInt(SqlCommand command)
        {
            SqlConnection connection = GetConnection();
            command.Connection = connection;
            command.Transaction = _transaction;
            //SqlCommand command = new SqlCommand(sql, connection, _transaction);
            try
            {

                object res = command.ExecuteScalar();
                if (res == null)
                    throw new DatabaseAccessException("ExecuteScalar returned null");
                else
                    if (res == DBNull.Value)
                        throw new DatabaseAccessException("ExecuteScalar returned DBNull.Value");
                    else
                        return int.Parse(res.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                // If no transaction has been started we need to close the current connection
                if (_connection == null)
                    connection.Close();
            }
        }
		/// <summary>
		/// Executes a given SQL command returning scalar value (first value of first row)
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public object ExecuteScalar(string sql)
		{
			SqlConnection connection = GetConnection();
			SqlCommand command = new SqlCommand(sql, connection, _transaction);
			try
			{
				return command.ExecuteScalar();
			}
            catch (SqlException ex)
            {
                throw ex;
            }
			finally
			{
				// If no transaction has been started we need to close the current connection
				if (_connection == null)
					connection.Close();
			}
		}

		/// <summary>
		/// Typed version of ExecuteScalar
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public int ExecuteScalarInt(string sql)
		{
			object result = ExecuteScalar(sql);
			if (result == null)
				throw new DatabaseAccessException("ExecuteScalar returned null");
			else
				if (result == DBNull.Value)
					throw new DatabaseAccessException("ExecuteScalar returned DBNull.Value");
				else
					return int.Parse(result.ToString());
		}

		/// <summary>
		/// Typed version of ExecuteScalar
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public long ExecuteScalarLong(string sql)
		{
			object result = ExecuteScalar(sql);
			if (result == null)
				throw new DatabaseAccessException("ExecuteScalar returned null");
			else
				if (result == DBNull.Value)
					throw new DatabaseAccessException("ExecuteScalar returned DBNull.Value");
				else
					return long.Parse(result.ToString());
		}

		/// <summary>
		/// Typed version of ExecuteScalar
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public string ExecuteScalarString(string sql)
		{
			object result = ExecuteScalar(sql);
			if (result == null)
				throw new DatabaseAccessException("ExecuteScalar returned null");
			else
				if (result == DBNull.Value)
					throw new DatabaseAccessException("ExecuteScalar returned DBNull.Value");
				else
					return result.ToString();
		}

		/// <summary>
		/// Typed version of ExecuteScalar
		/// </summary>
		/// <param name="sql">SQL command string to execute</param>
		/// <returns></returns>
		public bool ExecuteScalarBool(string sql)
		{
			object result = ExecuteScalar(sql);
			if (result == null)
				throw new DatabaseAccessException("ExecuteScalar returned null");
			else
				if (result == DBNull.Value)
					throw new DatabaseAccessException("ExecuteScalar returned DBNull.Value");
				else
					return Convert.ToBoolean(result);
		}

		#region Overloaded versions of the methods declared above which allow using of dynamic parameters

		public DataTable GetDataTable(string sql, params object[] parameters) 
		{
			return GetDataTable(PrepareSqlString(sql, parameters));
		}

		public DataView GetDataView(string sql, params object[] parameters) 
		{
			return GetDataView(PrepareSqlString(sql, parameters));
		}			

		public DataSet GetDataSet(string sql, params object[] parameters) 
		{
			return GetDataSet(PrepareSqlString(sql, parameters));
		}			
			
		public SqlDataReader GetDataReader(string sql, params object[] parameters) 
		{
			return GetDataReader(PrepareSqlString(sql, parameters));
		}			

		public DataRow GetSingleRow(string sql, params object[] parameters)
		{
			return GetSingleRow(PrepareSqlString(sql, parameters));
		}

		public void Execute(string sql, params object[] parameters)
		{
			Execute(PrepareSqlString(sql, parameters));
		}

		public object ExecuteScalar(string sql, params object[] parameters)
		{
			return ExecuteScalar(PrepareSqlString(sql, parameters));
		}

		public int ExecuteScalarInt(string sql, params object[] parameters)
		{
			return ExecuteScalarInt(PrepareSqlString(sql, parameters));
		}

		public long ExecuteScalarLong(string sql, params object[] parameters)
		{
			return ExecuteScalarLong(PrepareSqlString(sql, parameters));
		}

		public bool ExecuteScalarBool(string sql, params object[] parameters)
		{
			return ExecuteScalarBool(PrepareSqlString(sql, parameters));
		}

		public string ExecuteScalarString(string sql, params object[] parameters)
		{
			return ExecuteScalarString(PrepareSqlString(sql, parameters));
		}

		#endregion

		#region Auxiliary functions
		/// <summary>
		/// Generates SQL statement from an operator name and parameters.
		/// Performs proper convertion and escaping.
		/// </summary>
		/// <param name="sql">SQL operator</param>
		/// <param name="parameters">List of parameters to pass to the operator</param>
		/// <returns>Generated SQL statement</returns>
		public string PrepareSqlString(string sql, params object[] parameters)
		{
			string result = "";
			foreach (object param in parameters)
			{
				result += result == "" ? "" : ", ";

                if (param is DBNull || param == null)
                    result += "null";
                else if (param is string)
                    result += "'" + EscapeString((string)param) + "'";
                else if (param is DateTime)
                    result += "'" + DateTimeToString((DateTime)param) + "'";
                else if (param is bool)
                    result += BoolToString((bool)param);
                else if (param is double || param is Single)
                    result += DoubleToString((double)param);
                else if (param is decimal)
                    result += DecimalToString((decimal)param);
                else if (param is Guid)
                    result += GuidToString((Guid)param);
                else if (param is SqlParameter)
                    result += SqlParameterToString((SqlParameter)param);
                else
                    result += param.ToString();
			}
			return sql + " " + result;
		}

		/// <summary>
		/// Replaces all occurances of ' to double '' to pass to the 
		/// database server as a parameter
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string EscapeString(string str)
		{
			return str.Replace("'", "''");
		}

		/// <summary>
		/// Converts DataTime into string to pass to the 
		/// database server as a parameter
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static string DateTimeToString(DateTime x)
		{
			return x.ToString("yyyy.MM.dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Converts bool value into string to pass to the 
		/// database server as a parameter
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static string BoolToString(bool x)
		{
			return x ? "1" : "0";
		}

		/// <summary>
		/// Converts double value into string to pass to the 
		/// database server as a parameter
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static string DoubleToString(double x)
		{
			return x.ToString(CultureInfo.InvariantCulture);
		}

        public static string DecimalToString(decimal x)
        {
            return x.ToString(CultureInfo.InvariantCulture);
        }

        public static string GuidToString(Guid x)
        {
            return String.Format("'{0}'", x.ToString());
        }

		/// <summary>
		/// Converts SqlParameter value into string to pass to the 
		/// database server as a parameter. Performs escaping for
		/// string values.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static string SqlParameterToString(SqlParameter x)
		{
			SqlDbType type = x.SqlDbType;
			if (type == SqlDbType.Char || type == SqlDbType.VarChar || type == SqlDbType.NChar || type == SqlDbType.NVarChar)
				return "'" + EscapeString(x.Value.ToString()) + "'";
			else if (type == SqlDbType.DateTime || type == SqlDbType.SmallDateTime)
				return "'" + DateTimeToString(Convert.ToDateTime(x.Value)) + "'";
			else if (type == SqlDbType.Bit)
				return BoolToString(Convert.ToBoolean(x.Value));
			else if (type == SqlDbType.Real || type == SqlDbType.Decimal || type == SqlDbType.Float || type == SqlDbType.Money)
				return DoubleToString(Convert.ToDouble(x.Value));
			else
				return x.Value.ToString();
		}

		#endregion

		#region Transaction related routines
		public void BeginTransaction(IsolationLevel iso)
		{
			if (_connection != null)
				throw new DatabaseAccessException("The previous transaction is not yet completed");
			else
			{
				_connection = new SqlConnection(SqlConnectionString);
				_connection.Open();
				_transaction = _connection.BeginTransaction(iso);
			}
		}

		public void BeginTransaction()
		{
			if (_connection != null)
				throw new DatabaseAccessException("The previous transaction is not yet completed");
			else
			{
				_connection = new SqlConnection(SqlConnectionString);
				_connection.Open();
				_transaction = _connection.BeginTransaction();
			}
		}

		public void CommitTransaction()
		{
			if (_transaction == null)
				throw new DatabaseAccessException("The transaction has not been started");
			else
				try
				{
					_transaction.Commit();
					_connection.Close();
				}
				finally
				{
					_transaction = null;
					_connection = null;
				}
		}

		public void RollbackTransaction()
		{
			if (_transaction == null)
				throw new DatabaseAccessException("The transaction has not been started");
			else
				try
				{
					_transaction.Rollback();
					_connection.Close();
				}
				finally
				{
					_transaction = null;
					_connection = null;
				}
		}

		public bool IsInTransaction
		{
			get 
			{
				return _transaction != null;
			}
		}
		#endregion

		#region Like Parameters Utils

		public static string PrepareLikeParameter ( string Input, string pattern )
		{
			if ( Input == null || Input == "" )
				return "%";

			return String.Format ( pattern, Input.Replace("%", "[%]").Replace ("_", "[_]").Replace( "[", "[[]").Replace( "]", "[]]").Replace ("'", "''") );
		}

		/// <summary>
		/// Prepare input string to be passed as a SP parameter
		/// </summary>
		/// <param name="Input">Input string parameter</param>
		/// <returns>String parameter with replaced special symbols</returns>
		public static string PrepareLikeParameter ( string Input )
		{
			string Pattern = "{0}%";
			return PrepareLikeParameter(Input, Pattern);
		}

		/// <summary>
		/// Prepare input string to be passed as a SP parameter in the text like SP. 
		/// Replaces also ' to ''.
		/// </summary>
		/// <param name="Input">Input string parameter</param>
		/// <returns>String parameter with replaced special symbols</returns>
		public static string PrepareLikeParameterString ( string Input )
		{
			Input =   PrepareLikeParameter(Input);
			return	Input.Replace ("'", "''");
		}

		#endregion

		public bool Recompile(string objname)
		{
			bool res = false;
			try
			{
				ExecuteScalar("exec sp_recompile "+objname);
				res = true;
			}
			catch
			{
				res = false;
			}
			return res;
		}
	}
}

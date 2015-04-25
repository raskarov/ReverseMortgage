using System;

namespace LoanStar.Common
{
    /// <summary>
	/// All custom exceptions in the project must be derived from this class.
	/// Any time you need to throw some exception do it in this way:
	///		throw new AppException("The 'cond' parameter must not be null")
	///	instead of
	///		throw new Exception("The 'cond' parameter must not be null")
	/// </summary>
    [Serializable]
	public class AppException : Exception
	{
		public AppException()
		{
		}
		
		public AppException(string message, params object[] parameters) : base(String.Format(message, parameters))
		{
		}

		public AppException(Exception innerException, string message, params object[] parameters) : base(String.Format(message, parameters), innerException)
		{
		}
	}
    [Serializable]
	public class AppNoPermissionsException : AppException
	{
		public AppNoPermissionsException() : base("You have not permissions to access the resource.")
		{
		}

		public AppNoPermissionsException(string message, params object[] parameters) : base(String.Format(message, parameters))
		{
		}	
	}

    [Serializable]
	public class AppRequestValidationException : AppException
	{
		public AppRequestValidationException(Exception innerException) : base("Rethrowing HttpRequestValidationException", innerException)
		{
		}
	}

	/// <summary>
	/// Log entry type. See SYS_Event_Log table.
	/// </summary>
	public enum LogEntry {Info, Error, Warning, Audit, Debug};

	/// <summary>
	/// Contains common routines and definitions that are used all over the project.
	/// </summary>
	public class AppUtils
	{
		public AppUtils()
		{
		}
	}
}

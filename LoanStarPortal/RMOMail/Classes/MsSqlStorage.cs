namespace WebMailPro
{
	/// <summary>
	/// Summary description for MsSqlStorage.
	/// </summary>
	public class MsSqlStorage : DbStorage
	{
		public MsSqlStorage(Account account) : base(account) {}
	}

	public class MySqlStorage : DbStorage
	{
		public MySqlStorage(Account account) : base(account) {}
	}

	public class MsAccessStorage : DbStorage
	{
		public MsAccessStorage(Account account) : base(account) {}
	}
}

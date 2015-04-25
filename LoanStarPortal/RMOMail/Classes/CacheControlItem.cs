using System;
using System.Web.Caching;

namespace WebMailPro
{
	public enum CacheControlAction
	{
		AddItem =1,
		RemoveItem=2 
	}

	/// <summary>
	/// Represents a serializable wrapper class to hold a Cache item
	/// </summary>	
	[Serializable]
	public class CacheControlItem
	{
		public string Key;
		public object Item;
		public DateTime AbsoluteExpiration;
		public TimeSpan SlidingExpiration;
		public System.Web.Caching.CacheItemPriority Priority;
		// sorry, Dependency not marked as Serializable...class is sealed. Too bad!
		//public System.Web.Caching.CacheDependency Dependency;
		public int Action;
        
		public CacheControlItem(string Key, object Item, DateTime AbsoluteExpiration,
		        TimeSpan SlidingExpiration,CacheItemPriority Priority, CacheControlAction Action)
		{
			this.Key=Key;
			this.Item=Item;
			this.AbsoluteExpiration=AbsoluteExpiration;
			this.SlidingExpiration=SlidingExpiration;
			this.Priority=Priority;
			this.Action=(int)Action;
			 
		}
	}
}
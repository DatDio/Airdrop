using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PipWorld
{
	public class UsertInvestItems
	{
		public List<UserInvestItem> investItems { get; set; }
		public double maxValidUntil { get; set; }
	}
	public class Boost
	{
		public string id { get; set; }
		public int level { get; set; }
	}

	public class UserInvestItem
	{
		public string id { get; set; }
		public string type { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string image { get; set; }
		public double price { get; set; }
		public double profitPerHour { get; set; }
		public double profitPerSecond { get; set; }
		public int requiredLevel { get; set; }
		public double? validUntil { get; set; }
		public Boost boost { get; set; }
		public double? upgradeValuePerHour { get; set; }
	}



}

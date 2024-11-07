using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PipWorld
{
	public class OwnedInvestItem
	{
		public List<UserOwnedInvestItem> userOwnedInvestItems { get; set; }
	}
	public class UserOwnedInvestItem
	{
		public string id { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public double profitPerHour { get; set; }
		public double price { get; set; }
		public int level { get; set; }
		public string image { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Vertus
{

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Group
	{
		public string title { get; set; }
		public List<List<Sponsors2>> missions { get; set; }
	}

	public class TaskDto
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		//public long reward { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		//public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string telegramChatId { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public string groupType { get; set; }
		public bool isCompleted { get; set; }
		public string icon { get; set; }
	}

	public class Sponsors2
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public object reward { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string telegramChatId { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public object minReward { get; set; }
		public double rewardPercent { get; set; }
		public string rewardType { get; set; }
		public bool isCompleted { get; set; }
		public string icon { get; set; }
		public bool? isOnlyAdmin { get; set; }
		public string excludedCodes { get; set; }
		public string groupType { get; set; }
	}



}

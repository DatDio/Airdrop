using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Vertus
{

	public class TaskDto
	{
		public int notCompleted { get; set; }
		public int sponsorNotCompleted { get; set; }
		public Group[] groups { get; set; }
		public Sponsor[][] sponsors { get; set; }
		public Sponsors2[] sponsors2 { get; set; }
		public Community[][] community { get; set; }
		public object[] recommendations { get; set; }
		public Newdata[][] newData { get; set; }
	}

	public class Group
	{
		public string title { get; set; }
		public Mission[][] missions { get; set; }
	}

	public class Mission
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public long reward { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string telegramChatId { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public string groupType { get; set; }
		public bool isCompleted { get; set; }
		public string icon { get; set; }
		public bool isOnlyAdmin { get; set; }
		public string adsProviderName { get; set; }
		public int completion { get; set; }
		public int maxCompletion { get; set; }
		public object nextTime { get; set; }
		public string providerName { get; set; }
		public float rewardPercent { get; set; }
		public string rewardType { get; set; }
		public long minReward { get; set; }
	}

	public class Sponsor
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public float rewardPercent { get; set; }
		public string rewardType { get; set; }
		public long minReward { get; set; }
		public bool isOnlyAdmin { get; set; }
		public string icon { get; set; }
		public bool isCompleted { get; set; }
		public long reward { get; set; }
		public string telegramChatId { get; set; }
		public string excludedCodes { get; set; }
		public string groupType { get; set; }
	}

	public class Sponsors2
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public float rewardPercent { get; set; }
		public string rewardType { get; set; }
		public long minReward { get; set; }
		public bool isOnlyAdmin { get; set; }
		public string icon { get; set; }
		public bool isCompleted { get; set; }
		public long reward { get; set; }
		public string telegramChatId { get; set; }
		public string excludedCodes { get; set; }
		public string groupType { get; set; }
	}

	public class Community
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public BigInteger reward { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public string icon { get; set; }
		public string groupType { get; set; }
		public bool isCompleted { get; set; }
	}

	public class Newdata
	{
		public string title { get; set; }
		public object[] missions { get; set; }
		public Missionsgroup[][] missionsGroup { get; set; }
	}

	public class Missionsgroup
	{
		public bool isCIS { get; set; }
		public bool isWeb3User { get; set; }
		public string _id { get; set; }
		public string title { get; set; }
		public string link { get; set; }
		public string type { get; set; }
		public string resource { get; set; }
		public DateTime createdAt { get; set; }
		public bool isActive { get; set; }
		public int __v { get; set; }
		public string description { get; set; }
		public int timeToComplete { get; set; }
		public float rewardPercent { get; set; }
		public string rewardType { get; set; }
		public long minReward { get; set; }
		public bool isOnlyAdmin { get; set; }
		public string icon { get; set; }
		public bool isCompleted { get; set; }
		public long reward { get; set; }
		public string telegramChatId { get; set; }
		public string excludedCodes { get; set; }
		public string groupType { get; set; }
	}

}

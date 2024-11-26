using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Vertus
{
	public class UserInforDto
	{
		public bool isValid { get; set; }
		public User user { get; set; }
	}

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Abilities
	{
		public Farm farm { get; set; }
		public Storage storage { get; set; }
		public Population population { get; set; }
	}

	public class Alert
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
		public long __v { get; set; }
		public string description { get; set; }
		public long timeToComplete { get; set; }
		public long rewardPercent { get; set; }
		public string rewardType { get; set; }
		public long minReward { get; set; }
		public bool isOnlyAdmin { get; set; }
		public string icon { get; set; }
		public bool isCompleted { get; set; }
		public long reward { get; set; }
	}

	public class DailyCode
	{
		public object validDate { get; set; }
	}

	public class DailyRewards
	{
		public int consecutiveDays { get; set; }
		public string lastRewardClaimed { get; set; }
	}

	public class Farm
	{
		public double value { get; set; }
		public int level { get; set; }
		public string title { get; set; }
		public string image { get; set; }
		public string description { get; set; }
		public double priceToLevelUp { get; set; }
		public NextLevel nextLevel { get; set; }
	}

	public class NextLevel
	{
		public int level { get; set; }
		public string title { get; set; }
		public string image { get; set; }
		public string description { get; set; }
		public double priceToLevelUp { get; set; }
	}

	public class Population
	{
		public double value { get; set; }
		public int level { get; set; }
		public string title { get; set; }
		public string image { get; set; }
		public string description { get; set; }
		public int priceToLevelUp { get; set; }
		public NextLevel nextLevel { get; set; }
	}


	public class Storage
	{
		public long value { get; set; }
		public int level { get; set; }
		public string title { get; set; }
		public string image { get; set; }
		public string description { get; set; }
		public double priceToLevelUp { get; set; }
		public NextLevel nextLevel { get; set; }
	}

	public class User
	{
		public long telegramId { get; set; }
		public string walletAddress { get; set; }
		public string bounceableWallet { get; set; }
		public DateTime createdAt { get; set; }
		public BigInteger balance { get; set; }
		public BigInteger storage { get; set; }
		public bool activated { get; set; }
		public string role { get; set; }
		public Abilities abilities { get; set; }
		public string groupId { get; set; }
		public BigInteger vertStorage { get; set; }
		public bool isIpSaved { get; set; }
		public DailyRewards dailyRewards { get; set; }
		public DailyCode dailyCode { get; set; }
		public DateTime lastOnline { get; set; }
		public BigInteger valuePerHour { get; set; }
		public BigInteger earnedOffline { get; set; }
		public bool isCompletedEvent { get; set; }
		public string eventUuid { get; set; }
		public List<Alert> alerts { get; set; }
	}


}

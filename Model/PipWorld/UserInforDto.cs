using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PipWorld
{
	public class UserInforDto
	{
		public User user { get; set; }
		public List<AllStreak> allStreaks { get; set; }
	}

	public class AllStreak
	{
		public int streak { get; set; }
		public int reward { get; set; }
	}

	
	public class FlashDropSettings
	{
		public bool enabled { get; set; }
		public int maxUsers { get; set; }
		public int reward { get; set; }
		public int referralCount { get; set; }
		public int flashDropTimer { get; set; }
		public string message { get; set; }
		public string wonMessage { get; set; }
		public string lostMessage { get; set; }
		public string inactiveFlashdropMsg { get; set; }
		public string announcement { get; set; }
	}

	public class FreeEnergyRefills
	{
		public int lastTimeUpdated { get; set; }
		public int available { get; set; }
	}

	public class FreeTapsMultiplier
	{
		public int lastTimeUpdated { get; set; }
		public int available { get; set; }
	}

	public class InvestItem
	{
		public string id { get; set; }
		public int boughtAt { get; set; }
	}

	

	public class Level
	{
		public int level { get; set; }
		public string title { get; set; }
		public string badge { get; set; }
		public int nextLevelRequiredBalance { get; set; }
	}

	public class Rank
	{
		public object today { get; set; }
		public object yesterday { get; set; }
		public int lastTimeUpdated { get; set; }
	}

	public class Referred
	{
		public string id { get; set; }
		public long telegramId { get; set; }
		public int commission { get; set; }
		public int lastTimeUpdated { get; set; }
	}


	public class Streak
	{
		public int lastTimeUpdated { get; set; }
		public int count { get; set; }
	}

	public class TradingGroupData
	{
		public string image { get; set; }
		public string profileImage { get; set; }
		public string logo { get; set; }
		public string name { get; set; }
		public string color { get; set; }
		public string id { get; set; }
	}

	public class User
	{
		public int telegramId { get; set; }
		public string type { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public string username { get; set; }
		public object avatar { get; set; }
		public int energy { get; set; }
		public bool disabled { get; set; }
		public int lastTimeTapsUpdate { get; set; }
		public int paidRefillRate { get; set; }
		public int defaultRefillRate { get; set; }
		public int maxUserEnergy { get; set; }
		public double balance { get; set; }
		public int coinsPerTap { get; set; }
		public FreeEnergyRefills freeEnergyRefills { get; set; }
		public FreeTapsMultiplier freeTapsMultiplier { get; set; }
		public List<object> boosts { get; set; }
		public List<object> completedQuests { get; set; }
		public Level level { get; set; }
		public Rank rank { get; set; }
		public Streak streak { get; set; }
		public List<InvestItem> investItems { get; set; }
		public TradingGroupData tradingGroupData { get; set; }
		public List<object> claimedPromotionLevels { get; set; }
		public int referredBy { get; set; }
		public List<Referred> referred { get; set; }
		public int referredCount { get; set; }
		public int totalCommission { get; set; }
		public string languageCode { get; set; }
		public bool boardingCompleted { get; set; }
		public int createdAt { get; set; }
		public string id { get; set; }
	}
	

}

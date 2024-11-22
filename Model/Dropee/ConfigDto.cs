using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Dropee
{
	public class ConfigDto
	{
		public Config config { get; set; }
	}
	public class Config
	{
		public List<Upgrade> upgrades { get; set; }
		public List<string> upgradeSections { get; set; }
		public List<Task> tasks { get; set; }
		public Game game { get; set; }
	}
	public class Activation
	{
		public int afterDays { get; set; }
	}

	public class Ads
	{
		public DoubleOfflineProfit doubleOfflineProfit { get; set; }
	}

	public class AllDone
	{
		public int reward { get; set; }
		public int spins { get; set; }
	}

	public class Benefit
	{
		public string type { get; set; }
		public double amount { get; set; }
	}



	public class CardFastForward
	{
		public int minimumCooldown { get; set; }
		public List<Range> ranges { get; set; }
	}


	public class ConnectTonWallet
	{
		public int reward { get; set; }
	}

	public class CrackTheEgg
	{
		public int reward { get; set; }
		public int spins { get; set; }
	}

	public class Daily
	{
		public int amount { get; set; }
	}

	public class DailyCheckin
	{
		public int endHour { get; set; }
		public List<int> rewards { get; set; }
		public List<RewardsV2> rewardsV2 { get; set; }
		public RewardAfterLastDay rewardAfterLastDay { get; set; }
	}

	public class DailyCipher
	{
		public int reward { get; set; }
		public int endHour { get; set; }
	}

	public class DailyCombo
	{
		public int reward { get; set; }
		public int endHour { get; set; }
	}

	public class DailyQuestion
	{
		public int reward { get; set; }
	}

	public class DailyTasks
	{
		public Upgrade upgrade { get; set; }
		public CrackTheEgg crackTheEgg { get; set; }
		public Poke poke { get; set; }
		public SpinTheWheel spinTheWheel { get; set; }
		public ShareToStory shareToStory { get; set; }
		public AllDone allDone { get; set; }
	}

	public class DoubleOfflineProfit
	{
		public int maxPerDay { get; set; }
		public int multiplier { get; set; }
	}

	public class Ecommerce
	{

	}

	

	public class ExtraSpin
	{
		public WatchAd watchAd { get; set; }
		public Referral referral { get; set; }
		public Daily daily { get; set; }
		public LevelUp levelUp { get; set; }
	}

	public class FortuneWheel
	{
		public WatchAdForDoublePrize watchAdForDoublePrize { get; set; }
		public ExtraSpin extraSpin { get; set; }
		public List<Prize> prizes { get; set; }
		public GoldenWheel goldenWheel { get; set; }
	}



	public class Game
	{
		public Onboarding onboarding { get; set; }
		public Ads ads { get; set; }
		public FortuneWheel fortuneWheel { get; set; }
		public Ecommerce ecommerce { get; set; }
	}

	public class GoldenWheel
	{
		public List<Prize> prizes { get; set; }
		public Activation activation { get; set; }
	}

	public class InitialRefereeReward
	{
		public int premium { get; set; }
		public int normal { get; set; }
	}

	public class Invite3friends
	{
		public int reward { get; set; }
	}

	public class Invite5friends
	{
		public int reward { get; set; }
	}





	public class LevelUp
	{
		public int amount { get; set; }
	}

	public class Limits
	{
		public Time time { get; set; }
	}

	public class Onboarding
	{
		public int reward { get; set; }
	}

	public class Others
	{
		public Invite3friends invite3friends { get; set; }
		public Invite5friends invite5friends { get; set; }
		public ConnectTonWallet connectTonWallet { get; set; }
		public SetToken setToken { get; set; }
	}

	public class PerLevelReferrerReward
	{
		public object normal { get; set; }
		public object premium { get; set; }
		public int level { get; set; }
	}

	public class Poke
	{
		public int reward { get; set; }
	}

	public class Price
	{
		public string type { get; set; }
		public int amount { get; set; }
		public double usdAmount { get; set; }
		public CardFastForward cardFastForward { get; set; }
	}

	public class Prize
	{
		public string id { get; set; }
		public string type { get; set; }
		public int amount { get; set; }
		public string subtype { get; set; }
	}



	public class Range
	{
		public int cooldownUpTo { get; set; }
		public List<Price> prices { get; set; }
	}

	public class Referral
	{
		public int amount { get; set; }
		public int onboarding { get; set; }
		public int maxPerDay { get; set; }
	}



	public class Refill
	{
		public int singleUseCooldown { get; set; }
		public int rechargeCooldown { get; set; }
	}

	public class Requirements
	{
		public RequirementsUpgrade upgrade { get; set; }
	}

	public class RewardAfterLastDay
	{
		public int hourProfit { get; set; }
	}

	public class RewardsV2
	{
		public int coins { get; set; }
		public int? spins { get; set; }
	}

	public class SetToken
	{
		public int reward { get; set; }
	}

	public class ShareToStory
	{
		public int reward { get; set; }
	}

	public class SpinTheWheel
	{
		public int reward { get; set; }
	}



	public class Task
	{
		public string id { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string cta { get; set; }
		public string ctaAfterDone { get; set; }
		public int reward { get; set; }
		public int spins { get; set; }
		public int claimDelay { get; set; }
		public List<string> validations { get; set; }
		public string url { get; set; }
		public string imageUrl { get; set; }
		public string category { get; set; }
		public List<string> categories { get; set; }
		public object startsOn { get; set; }
		public bool isDone { get; set; }
		public bool isVerified { get; set; }
		public object claimedReward { get; set; }
		public Others others { get; set; }
		public DailyCheckin dailyCheckin { get; set; }
		public DailyTasks dailyTasks { get; set; }
	}

	public class Time
	{
		public string type { get; set; }
		public int duration { get; set; }
	}

	public class Upgrade
	{
		public string id { get; set; }
		public int level { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public int price { get; set; }
		public Ecommerce ecommerce { get; set; }
		public int profit { get; set; }
		public int profitDelta { get; set; }
		public string section { get; set; }
		public string imageUrl { get; set; }
		public int cooldownUntil { get; set; }
		public int cooldown { get; set; }
		public object appearsOn { get; set; }
		public object disappearsOn { get; set; }
		public int? expiresOn { get; set; }
		public object highlight { get; set; }
		public Requirements requirements { get; set; }
		public int every { get; set; }
		public int amount { get; set; }
	}

	public class RequirementsUpgrade
	{
		public string id { get; set; }
		public int level { get; set; }
		public int reward { get; set; }
		public int required { get; set; }
	}

	public class WatchAd
	{
		public int maxPerDay { get; set; }
		public int amount { get; set; }
	}

	public class WatchAdForDoublePrize
	{
		public int maxPerDay { get; set; }
	}




}

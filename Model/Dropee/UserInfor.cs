using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Dropee
{
	public class UserInfor
	{
		public PlayerStats playerStats { get; set; }
		public double offlineProfit { get; set; }
		public double offlineProfitHours { get; set; }
		public bool showLingoOffer { get; set; }
	}
	public class Activities
	{
		public string date { get; set; }
		public int tap { get; set; }
		public int watchAdForSpin { get; set; }
		public int watchAdForDoublePrize { get; set; }
		public int upgrade { get; set; }
		public bool poke { get; set; }
		public bool spinTheWheel { get; set; }
		public int watchAdForDoubleOfflineProfit { get; set; }
	}

	public class BecomeOkxRacer
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class BybitLingo10usdt
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class Challenges
	{
		public DailyCombo dailyCombo { get; set; }
	}

	public class ClaimedDoublePrizeByAdd
	{
		public int completedAt { get; set; }
	}

	public class Collabs
	{
		public int level { get; set; }
		public int lastUpgrade { get; set; }
		public int profit { get; set; }
	}

	public class ConfigUser
	{
		public long playerId { get; set; }
		public int referrerId { get; set; }
		public string season { get; set; }
	}

	public class CrackEggLingoislands
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class DailyCheckinUser
	{
		public string lastCheckin { get; set; }
		public int consecutiveDays { get; set; }
	}

	public class DailyComboUser
	{
		public string currentDate { get; set; }
		public List<object> foundCombo { get; set; }
	}

	public class EcommerceUser
	{
		public int starsBalance { get; set; }
		public OneTimePurchases oneTimePurchases { get; set; }
		public LastPurchases lastPurchases { get; set; }
	}

	public class EnabledFeatures
	{
	}

	public class Energy
	{
		public int level { get; set; }
		public int available { get; set; }
		public int max { get; set; }
	}

	public class ExchangeListing
	{
		public int level { get; set; }
		public int lastUpgrade { get; set; }
		public int profit { get; set; }
	}

	public class FeatureFlags
	{
	}

	public class FollowDropeeTwitterX1
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class FortuneWheelUser
	{
		public Prizes prizes { get; set; }
		public int available { get; set; }
		public int lastDailyTicketTimestamp { get; set; }
		public LastPrize lastPrize { get; set; }
	}

	public class Friends
	{
		public int count { get; set; }
	}

	public class Gifts
	{
	}

	public class JoinTg2
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class LastPrize
	{
		public string id { get; set; }
		public string type { get; set; }
		public int amount { get; set; }
	}

	public class LastPurchases
	{
	}

	public class LikeRtDropee25
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class LingoX
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class Notifications
	{
		public bool energyRefilled { get; set; }
		public bool poke { get; set; }
		public bool offlineProfit { get; set; }
		public bool referral { get; set; }
	}

	public class OnboardingUser
	{
		public bool done { get; set; }
	}

	public class OneTimePurchases
	{
	}

	public class OthersUser
	{
		public LikeRtDropee25 like_rt_dropee_25 { get; set; }
		public JoinTg2 join_tg_2 { get; set; }
		public LingoX lingo_x { get; set; }
		public ReactLastPostTg56 react_last_post_tg_56 { get; set; }
		public BecomeOkxRacer become_okx_racer { get; set; }
		public FollowDropeeTwitterX1 follow_dropee_twitter_x1 { get; set; }
		public PostDropee post_dropee { get; set; }
		public BybitLingo10usdt bybit_lingo_10usdt { get; set; }
		public CrackEggLingoislands crack_egg_lingoislands { get; set; }
	}

	public class PlayerStats
	{
		public Config config { get; set; }
		public double coins { get; set; }
		public int profit { get; set; }
		public int level { get; set; }
		public bool temporaryHasDoneTxV2 { get; set; }
		public int experience { get; set; }
		public int lastUpdate { get; set; }
		public Referrals referrals { get; set; }
		public Friends friends { get; set; }
		public Energy energy { get; set; }
		public Tap tap { get; set; }
		public RefillBooster refillBooster { get; set; }
		public Upgrades upgrades { get; set; }
		public int upgradeCount { get; set; }
		public Challenges challenges { get; set; }
		public Tasks tasks { get; set; }
		public OnboardingUser onboarding { get; set; }
		public EnabledFeatures enabledFeatures { get; set; }
		public FeatureFlags featureFlags { get; set; }
		public Reminders reminders { get; set; }
		//public FortuneWheel fortuneWheel { get; set; }
		public Settings settings { get; set; }
		public SimpleActions simpleActions { get; set; }
		public Gifts gifts { get; set; }
		public Ecommerce ecommerce { get; set; }
		public int migrationVersion { get; set; }
		public Activities activities { get; set; }
		public string token { get; set; }
		public int lastOfflineProfit { get; set; }
	}

	public class PostDropee
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
		public int nextRepeatTimestamp { get; set; }
	}

	public class Prizes
	{
		public int freeUpgrades { get; set; }
	}

	public class ReactLastPostTg56
	{
		public bool isDone { get; set; }
		public int actionCompletedAt { get; set; }
		public int reward { get; set; }
	}

	public class Referrals
	{
		public int count { get; set; }
	}

	public class RefillBooster
	{
		public int max { get; set; }
		public int available { get; set; }
		public int firstUseTimestamp { get; set; }
		public int lastUseTimestamp { get; set; }
	}

	public class Reminders
	{
	}

	

	public class Settings
	{
		public Notifications notifications { get; set; }
	}

	public class SimpleActions
	{
		public ClaimedDoublePrizeByAdd claimedDoublePrizeByAdd { get; set; }
	}

	public class Tap
	{
		public int level { get; set; }
		public int earnPerTap { get; set; }
		public int coinPerUnit { get; set; }
	}

	public class Tasks
	{
		public DailyCheckinUser dailyCheckin { get; set; }
		public OthersUser others { get; set; }
	}

	public class Upgrades
	{
		public ExchangeListing exchange_listing { get; set; }
		public Collabs collabs { get; set; }
	}




}

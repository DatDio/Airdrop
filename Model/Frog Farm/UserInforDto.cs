using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Frog_Farm
{
	public class UserInforDto
	{
		public Data data { get; set; }
	}
	
	public class ActiveDayBonus
	{
		public int id { get; set; }
		public int coins { get; set; }
		public string title_ru { get; set; }
		public string title_en { get; set; }
		public string title { get; set; }
		public string img { get; set; }
		public int day { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
	}

	public class Data
	{
		public string backend { get; set; }
		public int id { get; set; }
		public long user_id { get; set; }
		public int contest_coins { get; set; }
		public int contest_balance { get; set; }
		public int game_coins { get; set; }
		public string username { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string full_name { get; set; }
		public double coins_total { get; set; }
		public double? balance { get; set; }
		public int coins_per_hour { get; set; }
		public int coins_per_hour_boosted { get; set; }
		public int collected_coins { get; set; }
		public bool code_active { get; set; }
		public List<int> referral_levels { get; set; }
		public object package { get; set; }
		public object boost { get; set; }
		public object boost_at { get; set; }
		public object auto_farm { get; set; }
		public object auto_farm_at { get; set; }
		public object bing_x_uid { get; set; }
		public object by_bit_uid { get; set; }
		public int day_bonus_id { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
		public string bonus_date { get; set; }
		public DateTime? roulette_balance_at { get; set; }
		public DateTime? roulette_coins_at { get; set; }
		public DateTime last_coins_per_hour_at { get; set; }
		public string task_boost_from { get; set; }
		public string task_boost_to { get; set; }
		public double usd_rate { get; set; }
		public double gas { get; set; }
		public NftPrices nft_prices { get; set; }
		public List<Task> tasks { get; set; }
		public int tasks_badge { get; set; }
		public GameFrog game_frog { get; set; }
		public object game_money { get; set; }
		public DayBonus day_bonus { get; set; }
		public ActiveDayBonus active_day_bonus { get; set; }
		public int? game_high_score { get; set; }
		public DateTime? game_last_played_at { get; set; }
		public int? game_energy { get; set; }
		public int? max_game_energy { get; set; }
		public DateTime? game_last_energy_at { get; set; }
	}

	public class DayBonus
	{
		public int id { get; set; }
		public int coins { get; set; }
		public string title_ru { get; set; }
		public string title_en { get; set; }
		public string title { get; set; }
		public string img { get; set; }
		public int day { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
	}

	public class GameFrog
	{
		public int id { get; set; }
		public int winners_count { get; set; }
		public int puzzles_count { get; set; }
		public int prize { get; set; }
		public int type { get; set; }
		public DateTime ends_at { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
	}

	public class NftPrices
	{
		public int bronze { get; set; }
		public long gold { get; set; }
		public long premium { get; set; }
	}

	

	public class Task
	{
		public int id { get; set; }
	}

}

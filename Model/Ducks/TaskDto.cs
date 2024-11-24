using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Ducks
{
	public class TaskDto
	{
		public string error_code { get; set; }
		public Data data { get; set; }
	}

	public class Data
	{
		public Datum[] data { get; set; }
		public int total { get; set; }
	}

	public class Datum
	{
		public int id { get; set; }
		public string partner_name { get; set; }
		public string partner_image { get; set; }
		public int priority { get; set; }
		public int rate { get; set; }
		public int type { get; set; }
		public DateTime updated_time { get; set; }
		public object metadata { get; set; }
		public Partner_Missions[] partner_missions { get; set; }
		public int total_participants { get; set; }
	}

	public class Partner_Missions
	{
		public string link { get; set; }
		public int type { get; set; }
		public string image { get; set; }
		public int pm_id { get; set; }
		public string title { get; set; }
		public int reward { get; set; }
		public Metadata metadata { get; set; }
		public Airdrop_Info airdrop_info { get; set; }
		public object[] mission_condition { get; set; }
	}

	public class Metadata
	{
		public string button { get; set; }
		public string type { get; set; }
		public string content { get; set; }
		public string required_ref { get; set; }
		public int require_game { get; set; }
		public string[] condition { get; set; }
		public string title { get; set; }
		public string data { get; set; }
		public bool require { get; set; }
		public string widget_link { get; set; }
		public string widget_name { get; set; }
		public string post_id { get; set; }
	}

	public class Airdrop_Info
	{
		public int reward { get; set; }
		public int max_user { get; set; }
		public int won_user { get; set; }
		public int currency_id { get; set; }
		public int max_win_user { get; set; }
	}



}

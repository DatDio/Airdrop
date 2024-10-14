using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model
{
	public class AccountModel
	{
		[JsonIgnore]
		public string Api { get; set; } = "";
		[JsonIgnore]
		public string UserId { get; set; } = "";
		[JsonIgnore]
		public int MinLevelDuck { get; set; } = 0;
		[JsonIgnore]
		public int Win { get; set; } = 0;
		[JsonIgnore]
		public int Lose { get; set; } = 0;
		[JsonIgnore]
		public int Reward { get; set; } = 0;
		[JsonIgnore]
		public string idRef { get; set; } = "";
		[JsonIgnore]
		public string InitData { get; set; } = "";
		[JsonIgnore]
		public string CacheId_Tapswap { get; set; } = "";
		[JsonIgnore]
		public string XCV_Tapswap { get; set; } = "";
		[JsonIgnore]
		public string Refresh_Token { get; set; } = "";

		public string NameGpm { get; set; } = "";
		public string InitParam { get; set; } = "";
		public string Proxy { get; set; } = "";
		public string Token { get; set; } = "";
		public string Health { get; set; } = "";
		public string Amount { get; set; } = "";
		public string CurrentEnergy { get; set; } = "";
		public string Level { get; set; } = "";
		public string Speed { get; set; } = "";
		public string Time { get; set; } = "";
		public string Status { get; set; } = "";
	}
}

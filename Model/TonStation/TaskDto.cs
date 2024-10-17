using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.TonStation
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

	public class TaskDto
	{
		public int code { get; set; }
		public string message { get; set; }
		public List<Datum> data { get; set; }
	}
	public class Datum
	{
		public string project { get; set; }
		public string type { get; set; }
		public string description { get; set; }
		public Reward reward { get; set; }
		public string data { get; set; }
		public Subtype subtype { get; set; }
		public bool active { get; set; }
		public int sortOrder { get; set; }
		public string category { get; set; }
		public string id { get; set; }
		public string status { get; set; }
		public DateTime? dateStart { get; set; }
		public Payment payment { get; set; }
	}

	public class Payment
	{
		public string address { get; set; }
		public int amount { get; set; }
	}

	public class Reward
	{
		public string type { get; set; }
		public int amount { get; set; }
	}

	

	public class Subtype
	{
		public string social { get; set; }
		public string lightTheme { get; set; }
		public string darkTheme { get; set; }
		public string link { get; set; }
	}


}

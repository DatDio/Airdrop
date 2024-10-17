using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Nomis
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);

	public class TaskDto
	{
		public int id { get; set; }
		public string title { get; set; }
		public bool isDisabled { get; set; }
		public string description { get; set; }
		public bool? isPriority { get; set; }
		public int? sort { get; set; }
		public object eligibility { get; set; }
		public Icon icon { get; set; }
		public List<TonTwaTask> ton_twa_tasks { get; set; }
	}

	public class Formats
	{
		public Thumbnail thumbnail { get; set; }
		public Small small { get; set; }
		public Medium medium { get; set; }
		public Large large { get; set; }
	}

	public class Icon
	{
		public int id { get; set; }
		public Formats formats { get; set; }
		public string url { get; set; }
	}

	public class Large
	{
		public string ext { get; set; }
		public string url { get; set; }
		public string hash { get; set; }
		public string mime { get; set; }
		public string name { get; set; }
		public object path { get; set; }
		public double size { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int sizeInBytes { get; set; }
	}

	public class Medium
	{
		public string ext { get; set; }
		public string url { get; set; }
		public string hash { get; set; }
		public string mime { get; set; }
		public string name { get; set; }
		public object path { get; set; }
		public double size { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int sizeInBytes { get; set; }
	}



	public class Small
	{
		public string ext { get; set; }
		public string url { get; set; }
		public string hash { get; set; }
		public string mime { get; set; }
		public string name { get; set; }
		public object path { get; set; }
		public double size { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int sizeInBytes { get; set; }
	}

	public class Thumbnail
	{
		public string ext { get; set; }
		public string url { get; set; }
		public string hash { get; set; }
		public string mime { get; set; }
		public string name { get; set; }
		public object path { get; set; }
		public double size { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int sizeInBytes { get; set; }
	}

	public class TonTwaTask
	{
		public int id { get; set; }
		public string title { get; set; }
		public int reward { get; set; }
		public bool isDisabled { get; set; }
		public DateTime publishedAt { get; set; }
		public object startAt { get; set; }
		public DateTime? endAt { get; set; }
		public string link { get; set; }
		public string handler { get; set; }
		public object buttonText { get; set; }
		public bool useScoreInfluence { get; set; }
		public object influenceFormula { get; set; }
		public string description { get; set; }
		public bool isDaily { get; set; }
		public bool isDynamicReward { get; set; }
		public bool isReusable { get; set; }
		public object rewardDescription { get; set; }
		public bool? isPriority { get; set; }
		public List<object> required_tasks { get; set; }
		public Icon icon { get; set; }
	}


}

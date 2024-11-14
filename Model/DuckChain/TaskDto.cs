using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.DuckChain
{
	public class TaskDto
	{
		public int code { get; set; }
		public string message { get; set; }
		public Data data { get; set; }
	}
	public class Daily
	{
		public int taskId { get; set; }
		public string taskType { get; set; }
		public string content { get; set; }
		public string action { get; set; }
		public int integral { get; set; }
		public string icon_url { get; set; }
		public int type { get; set; }
	}

	public class Data
	{
		public List<object> social_media { get; set; }
		public List<Daily> daily { get; set; }
		public List<OneTime> oneTime { get; set; }
		public List<Partner> partner { get; set; }
	}

	public class OneTime
	{
		public int taskId { get; set; }
		public string taskType { get; set; }
		public string content { get; set; }
		public string action { get; set; }
		public int integral { get; set; }
		public string icon_url { get; set; }
		public int type { get; set; }
	}

	public class Partner
	{
		public int taskId { get; set; }
		public string taskType { get; set; }
		public string content { get; set; }
		public string action { get; set; }
		public int integral { get; set; }
		public string icon_url { get; set; }
		public int type { get; set; }
	}

	


}

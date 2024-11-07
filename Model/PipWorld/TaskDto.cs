using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PipWorld
{
	public class TaskDto
	{
		public Quest quests { get; set; }
	}
	public class Quest
	{
		public string id { get; set; }
		public string type { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string image { get; set; }
		public string url { get; set; }
		public int reward { get; set; }
		public int? validUntil { get; set; }
		public int delayBeforeClaim { get; set; }
		public bool completed { get; set; }
		public int? requiredFriends { get; set; }
		public List<Quest> quests { get; set; }
		public int maxValidUntil { get; set; }
	}

	
}

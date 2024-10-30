using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Clayton
{
	public class TaskDto
	{
		public bool is_claimed { get; set; }
		public bool is_completed { get; set; }
		public Task task { get; set; }
		public int task_id { get; set; }
	}
	public class Task
	{
		public string avatar_path { get; set; }
		public string description { get; set; }
		public int game_attempts { get; set; }
		public int id { get; set; }
		public bool requires_check { get; set; }
		public int reward_tokens { get; set; }
		public string title { get; set; }
	}
}

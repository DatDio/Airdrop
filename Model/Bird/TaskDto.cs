using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Bird
{
	public class TaskDto
	{
		public string _id { get; set; }
		public string name { get; set; }
		public string logo { get; set; }
		public string description { get; set; }
		public int priority { get; set; }
		public bool is_enable { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public int __v { get; set; }
		public List<Task> tasks { get; set; }
		public List<Reward> rewards { get; set; }
		public string content { get; set; }
		public string code { get; set; }
		public DateTime? time { get; set; }
	}
	public class Task
	{
		public string _id { get; set; }
		public string title { get; set; }
		public string logo { get; set; }
		public string channelId { get; set; }
		public int point { get; set; }
		public string url { get; set; }
		public int __v { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public bool is_enable { get; set; }
		public string slug { get; set; }
		public int priority { get; set; }
		public bool is_captcha { get; set; }
		public string refId { get; set; }
		public string query_url { get; set; }
	}
	public class Reward
	{
		public int amount { get; set; }
		public string symbol { get; set; }
		public string logo { get; set; }
	}

	
}

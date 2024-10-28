using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.CryptoRank
{
	public class TaskDto
	{
		public string id { get; set; }
		public string name { get; set; }
		public string group { get; set; }
		public string type { get; set; }
		public int reward { get; set; }
		public string linkUrl { get; set; }
		public string iconUrl { get; set; }
		public bool isDone { get; set; }
		public object storyText { get; set; }
		public object storyMediaFileUrl { get; set; }
		public int dailySequence { get; set; }
		public Question question { get; set; }
	}
	public class Option
	{
		public string answer { get; set; }
		public string text { get; set; }
	}

	public class Question
	{
		public string id { get; set; }
		public string text { get; set; }
		public string cryptorankUrl { get; set; }
		public List<Option> options { get; set; }
	}



}

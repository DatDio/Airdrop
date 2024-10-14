using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Major
{
	public class TaskDto
	{
		public int id { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string icon_url { get; set; }
		public string type { get; set; }
		public int award { get; set; }
		public Payload payload { get; set; }
		public bool is_completed { get; set; }
	}

	public class Payload
	{
		public string url { get; set; }
		public string ton_wallet { get; set; }
		public double? amount { get; set; }
		public string run_button { get; set; }
		public string check_button { get; set; }
		public int? group { get; set; }
	}
}

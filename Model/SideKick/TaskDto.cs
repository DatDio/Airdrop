using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.SideKick
{
	public class TaskDto
	{
		public string _id { get; set; }
		public int index { get; set; }
		public string title { get; set; }
		public int point { get; set; }
		public string type { get; set; }
		public string link { get; set; }
		public bool isGatePass { get; set; }
		public string group { get; set; }
		public string icon { get; set; }
		public bool status { get; set; }
		public int __v { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public bool isFinish { get; set; }
	}
}

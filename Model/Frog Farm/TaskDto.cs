using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Frog_Farm
{
	public class TaskDto
	{
		public List<Datum> data { get; set; }
	}
	public class Datum
	{
		public int id { get; set; }
		public string title_ru { get; set; }
		public string title_en { get; set; }
		public string desc_ru { get; set; }
		public string desc_en { get; set; }
		public string title { get; set; }
		public string desc { get; set; }
		public string img { get; set; }
		public string url { get; set; }
		public string channel_id { get; set; }
		public int input { get; set; }
		public int coins { get; set; }
		public int count { get; set; }
		public object hours { get; set; }
		public bool is_external { get; set; }
		public string type { get; set; }
		public object countries { get; set; }
		public string created_at { get; set; }
		public string updated_at { get; set; }
	}
}

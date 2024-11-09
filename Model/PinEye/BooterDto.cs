using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PinEye
{
	public class BooterDto
	{
		public List<DataBooter> data { get; set; }
		public object errors { get; set; }
	}
	public class DataBooter
	{
		public int id { get; set; }
		public string title { get; set; }
		public string type { get; set; }
		public int minLevel { get; set; }
		public int maxLevel { get; set; }
		public int reward { get; set; }
		public int cost { get; set; }
		public int currentLevel { get; set; }
	}

}

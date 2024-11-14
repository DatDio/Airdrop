using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.DuckChain
{
	public class TaskInfor
	{
		public int code { get; set; }
		public string message { get; set; }
		public DataInfor data { get; set; }
	}

	public class DataInfor
	{
		public List<object> socialMedia { get; set; }
		public List<int> daily { get; set; }
		public List<int> partner { get; set; }
		public List<int> oneTime { get; set; }
		public string total { get; set; }
		public object twitterDaily { get; set; }
		public int oneTimeIsFinished { get; set; }
		public int partnerIsFinished { get; set; }
	}

	
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Coub
{
	public class TaskFinishedDto
	{
		public DateTime created_at { get; set; }
		public int id { get; set; }
		public int points { get; set; }
	}
}

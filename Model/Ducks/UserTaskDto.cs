using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Ducks
{
	//chứa các task đã làm
	public class UserTaskDto
	{
		public string error_code { get; set; }
		public List<TaskComplted> data { get; set; }
	}
	public class TaskComplted
	{
		public int user_id { get; set; }
		public int partner_mission_id { get; set; }
	}
}

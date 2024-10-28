using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Bird
{
	public class JointedTaskDto
	{
		public string _id { get; set; }
		public string taskId { get; set; }
		public string telegramId { get; set; }
		public DateTime createdAt { get; set; }
		public DateTime updatedAt { get; set; }
		public int __v { get; set; }
	}
}

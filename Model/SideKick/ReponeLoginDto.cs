using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.SideKick
{
	public class ReponeLoginDto
	{
		public List<List> list { get; set; }
	}
	public class List
	{
		public string _id { get; set; }
		public int day { get; set; }
		public int reward { get; set; }
		public bool isSignin { get; set; }
		public bool isToday { get; set; }
	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PinEye
{
	public class TaskDto
	{
		public List<Datum> data { get; set; }
		public object errors { get; set; }
	}
	public class Datum
	{
		public int id { get; set; }
		public string title { get; set; }
		public string link { get; set; }
		public string iconUrl { get; set; }
		public Modal modal { get; set; }
		public string category { get; set; }
		public string subCategory { get; set; }
		public int score { get; set; }
		public bool isFollowed { get; set; }
		public bool isClaimed { get; set; }
		public int claimedScore { get; set; }
	}

	public class Modal
	{
		public string buttonTitle { get; set; }
		public string modalText { get; set; }
		public string modalIconUrl { get; set; }
	}

	


}

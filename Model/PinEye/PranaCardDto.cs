using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.PinEye
{
	public class PranaCardDto
	{
		public Data data { get; set; }
		public List<object> errors { get; set; }
	}
	public class Card
	{
		public int id { get; set; }
		public int collectionId { get; set; }
		public string title { get; set; }
		public int profit { get; set; }
		public int cost { get; set; }
		public int totalCooldownTime { get; set; }
		public int cooldownTime { get; set; }
		public int currentLevel { get; set; }
		public int minLevel { get; set; }
		public int maxLevel { get; set; }
		public string imageUrl { get; set; }
		public bool isCompleted { get; set; }
	}

	public class Category
	{
		public int id { get; set; }
		public string title { get; set; }
		public int userLevel { get; set; }
		public List<Collection> collections { get; set; }
	}

	public class Collection
	{
		public int id { get; set; }
		public string title { get; set; }
		public int categoryId { get; set; }
		public int level { get; set; }
		public string status { get; set; }
		public List<Card> cards { get; set; }
	}

	public class Data
	{
		public Info info { get; set; }
		public List<Category> categories { get; set; }
	}

	public class Info
	{
		public int profit { get; set; }
		public int level { get; set; }
		public List<Category> categories { get; set; }
	}

	


}

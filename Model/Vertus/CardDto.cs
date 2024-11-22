using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Model.Vertus
{
	public class CardDto
	{
		public List<EconomyCard> economyCards { get; set; }
		public List<MilitaryCard> militaryCards { get; set; }
		public List<ScienceCard> scienceCards { get; set; }
	}

	public class EconomyCard
	{
		public bool isTon { get; set; }
		public List<string> prerequisites { get; set; }
		public string _id { get; set; }
		public string cardName { get; set; }
		public List<Level> levels { get; set; }
		public string icon { get; set; }
		public string type { get; set; }
		public int currentLevel { get; set; }
		public int currentValue { get; set; }
		public bool isUpgradable { get; set; }
		public object nextValue { get; set; }
		public bool isLocked { get; set; }
		public List<string> mustBeOwnedNames { get; set; }
	}

	public class Level
	{
		public string _id { get; set; }
		public object cost { get; set; }
		public object value { get; set; }
	}

	public class MilitaryCard
	{
		public bool isTon { get; set; }
		public List<string> prerequisites { get; set; }
		public string _id { get; set; }
		public string cardName { get; set; }
		public List<Level> levels { get; set; }
		public string icon { get; set; }
		public string type { get; set; }
		public int currentLevel { get; set; }
		public int currentValue { get; set; }
		public bool isUpgradable { get; set; }
		public object nextValue { get; set; }
		public bool isLocked { get; set; }
		public List<string> mustBeOwnedNames { get; set; }
	}



	public class ScienceCard
	{
		public bool isTon { get; set; }
		public List<string> prerequisites { get; set; }
		public string _id { get; set; }
		public string cardName { get; set; }
		public List<Level> levels { get; set; }
		public string icon { get; set; }
		public string type { get; set; }
		public int currentLevel { get; set; }
		public int currentValue { get; set; }
		public bool isUpgradable { get; set; }
		public object nextValue { get; set; }
		public bool isLocked { get; set; }
		public List<string> mustBeOwnedNames { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Airdrop.Helper
{
	public class RegexHelper
	{
		public static string GetValueFromRegex(string regex, string content, int group = 1, RegexOptions options = RegexOptions.Multiline)
		{
			try
			{
				var match = Regex.Match(content, regex, options);
				if (match.Success)
				{
					return match.Groups[group].Value;
				}
			}
			catch
			{
				//
			}

			return "";
		}

		public static string GetValueFromRegex1(string content, string regex, int group = 1, RegexOptions options = RegexOptions.Multiline)
		{
			try
			{
				var match = Regex.Match(content, regex, options);
				if (match.Success)
				{
					return match.Groups[group].Value;
				}
			}
			catch
			{
				//
			}

			return "";
		}
	}
}

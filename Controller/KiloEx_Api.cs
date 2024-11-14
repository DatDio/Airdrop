using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.KiloEx;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;

namespace Airdrop.Controller
{
	public class KiloEx_Api
	{
		private List<MarginLevel> marginLevels;
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body, username;
		public KiloEx_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			var urlJson = HttpUtility.UrlDecode(urlDecode);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://app.kiloex.io
					  Referer: https://app.kiloex.io/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
			marginLevels = new List<MarginLevel>
						{
							new MarginLevel { Required = 10000, Margin = 5000 },
							new MarginLevel { Required = 2000, Margin = 1000 },
							new MarginLevel { Required = 1000, Margin = 500 },
							new MarginLevel { Required = 200, Margin = 100 },
							new MarginLevel { Required = 100, Margin = 50 },
							new MarginLevel { Required = 20, Margin = 10 }
						};
		}
		public string GetProfile()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp);
				body = rq.Get($"https://opapi.kiloex.io/tg/user/info?account={model.UserId}&name={username}&from=kiloextrade").ToString();
				model.Amount = RegexHelper.GetValueFromRegex("\"balance\":(.*?),", body);
				model.CurrentEnergy = RegexHelper.GetValueFromRegex("\"stamina\":(.*?),", body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return body;
		}
		public bool ClaimOfflineCoins()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				var jsonPayload = new
				{
					account = model.UserId,
					shared = false
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://opapi.kiloex.io/tg/coin/claim", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}

		public bool checkAndBindReferral()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp);
				body = rq.Get($"https://opapi.kiloex.io/tg/referral/code?account=${model.UserId}").ToString();


			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}

		public bool UpdateMining()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				var jsonPayload = new
				{
					account = model.UserId,
					stamina = model.CurrentEnergy,
					coin = model.CurrentEnergy
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://opapi.kiloex.io/tg/mining/update", jsonString, "application/json").ToString();


			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}

		private bool OpenOrder(string positionType, double margin)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				var jsonPayload = new
				{
					account = model.UserId,
					productId = 2,
					margin = margin,
					leverage = 100,
					positionType = positionType,
					settleDelay = 300
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://opapi.kiloex.io/tg/order/open", jsonString, "application/json").ToString();


			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}

		private void OpenOrdersForMargin(double margin)
		{
			var longResult = OpenOrder("long", margin);

			var shortResult = OpenOrder("short", margin);
			//var longResult = OpenOrder("long",)
		}
		public void HandelLongShort()
		{
			var appropriateLevel = marginLevels.FirstOrDefault(level => double.Parse(model.Amount) >= level.Required);
			if (appropriateLevel != null)
			{
				Debug.WriteLine($"Balance {model.Amount} đủ điều kiện mở lệnh mốc {appropriateLevel.Margin}", "info");
				OpenOrdersForMargin(appropriateLevel.Margin);
			}
			else
			{
				Debug.WriteLine($"Balance {model.Amount} chưa đủ điều kiện mở lệnh", "warning");
			}
		}
	}
}

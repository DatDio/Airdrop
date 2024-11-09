using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Airdrop.Model.PinEye;
using System.Threading.Tasks;
using Airdrop.Model.PipWorld;
using Acornima;
using System.Threading;

namespace Airdrop.Controller
{
	public class PinEye_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		public PinEye_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			//var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			//var urlJson = HttpUtility.UrlDecode(urlDecode);

			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://app.pineye.io
					  Referer: https://app.pineye.io/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string Login()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					userinfo = model.InitData
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.pineye.io/api/v2/Login", jsonString, "application/json").ToString();
				model.Token = RegexHelper.GetValueFromRegex("\"token\": \"(.*?)\",", body);
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return model.Token;
		}
		public string GetProfile()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");
				body = rq.Get($"https://api.pineye.io/api/v2/profile").ToString();
				model.Amount = RegexHelper.GetValueFromRegex("\"totalBalance\": (.*?),", body);
				model.Level = RegexHelper.GetValueFromRegex("\"level\": (.*?),", body);
				model.CurrentEnergy = RegexHelper.GetValueFromRegex("\"currentEnergy\": (.*?),", body);
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return body;
		}
		private Model.PinEye.TaskDto GetTask()
		{
			var Alltask = new Model.PinEye.TaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/Social").ToString();
				Alltask = JsonConvert.DeserializeObject<Model.PinEye.TaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}


		private void claimTask(int taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");


				var jsonPayload = new
				{
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.pineye.io/api/v1/SocialFollower/claim?socialId={taskID}", jsonString, "application/json").ToString();
				model.Amount = RegexHelper.GetValueFromRegex("\"balance\": (.*?),", body);
			}
			catch
			{

				body = "";
			}

		}

		public void HandleTask()
		{
			var allTask = GetTask();
			foreach (var task in allTask.data)
			{
				if (task.isClaimed == true) continue;

				claimTask(task.id);
				Thread.Sleep(1000);
			}

		}

		public void HandleBooters()
		{
			var allBooters = GetBooters();
			foreach (var booter in allBooters.data)
			{
				if (double.Parse(model.Amount) > booter.cost)
				{
					BuyBooster(booter.id);
				}
			}

		}
		private BooterDto GetBooters()
		{
			var allBoooters = new BooterDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/Booster").ToString();
				allBoooters = JsonConvert.DeserializeObject<BooterDto>(body);

			}
			catch
			{
				body = "";
			}

			return allBoooters;
		}

		private bool BuyBooster(int boosterId)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");

				body = rq.Post($"https://api.pineye.io/api/v2/profile/BuyBooster?boosterId={boosterId}").ToString();
			}
			catch
			{

				body = "";
				return false;
			}
			return true;
		}

		public bool DailyReward()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");


				var jsonPayload = new
				{
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.pineye.io/api/v1/DailyReward/claim", jsonString, "application/json").ToString();
			}
			catch
			{

				body = "";
				return false;
			}
			return true;
		}

		public bool TapEnergy(string energy)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/Tap?count={energy}").ToString();

			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private int GetRemainedCountFreeFullEnergy()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/FullEnergy").ToString();

			}
			catch
			{
				body = "";
			}
			var remainedCountString = RegexHelper.GetValueFromRegex("\"remainedCount\": (.*?),", body);
			if (!String.IsNullOrEmpty(remainedCountString))
			{
				return int.Parse(remainedCountString);
			}
			return 0;
		}
		private bool GetFreeFullEnergy()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Post($"https://api.pineye.io/api/v1/FullEnergy/SetFullEnergy").ToString();

			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}
		public void MoreTapEnergy()
		{
			var remainedCount = GetRemainedCountFreeFullEnergy();
			for (int i = 0; i < remainedCount; i++)
			{
				if (GetFreeFullEnergy())
				{
					TapEnergy(model.CurrentEnergy);
				}
				Thread.Sleep(3600);
			}

		}
		public bool BuyLottery()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/Lottery").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			var hasBuyed = RegexHelper.GetValueFromRegex("\"hasBuyed\":(.*?),", body);
			if (hasBuyed.Trim() == "false")
			{
				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");

					body = rq.Post($"https://api.pineye.io/api/v1/Lottery/BuyTicket").ToString();
				}
				catch
				{
					body = "";
					return false;
				}
			}
			return true;
		}


		private PranaCardDto GetPranaGameMarketplace()
		{
			var AllCard = new PranaCardDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.pineye.io/api/v1/PranaGame/Marketplace").ToString();
				AllCard = JsonConvert.DeserializeObject<PranaCardDto>(body);

			}
			catch
			{
				body = "";
			}

			return AllCard;
		}

		private bool BuyPranaGameCard(int cardId, int level)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");

				body = rq.Post($"https://api.pineye.io/api/v1/PranaGame/Purch?cardId={cardId}&level={level + 1}").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		public void HandlePranaGameCard()
		{
			var allCard = GetPranaGameMarketplace();

			foreach (var category in allCard.data.categories)
			{
				foreach (var collection in category.collections)
				{
					if (collection.status == "doing")
					{
						foreach (var card in collection.cards)
						{
							if (!card.isCompleted)
							{
								if (double.Parse(model.Amount) > card.cost)
								{
									BuyPranaGameCard(card.id, card.currentLevel);
								}
							}
						}
					}

				}

			}
		}
	}
}
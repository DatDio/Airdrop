using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Dropee;
using Airdrop.Model.PinEye;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Airdrop.Controller
{
	public class Dropee_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, username, id;
		double availableCoins;
		string referrerCode = "54K3Y8gNxaO";
		public Dropee_Api(AccountModel accountModel)
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
                      accept: application/json, text/plain, */*
					  Origin:https://webapp.game.dropee.xyz
					  Referer: https://webapp.game.dropee.xyz/
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
				//var jsonPayload = new
				//{
				//	impersonationToken = "",
				//	initData = model.InitData,
				//	referrerCode = "54K3Y8gNxaO",
				//	user_id = model.UserId,
				//	utmSource = ""
				//};
				var jsonPayload = new Dictionary<string, object>
{
	{ "impersonationToken", null },
	{ "initData", model.InitData },
	{ "referrerCode", referrerCode },
	{ "user_id", model.UserId },
	{ "utmSource", null }
};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);

				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/telegram/me", jsonString, "application/json").ToString();
				model.Token = RegexHelper.GetValueFromRegex("\"token\":\"(.*?)\",", body);

			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}

			//Chaỵ lần đầu cần addfriend
			AddFriend();
			CheckReferral();
			var data = SyncGame();
			if (!data.playerStats.onboarding.done)
			{
				CompleteOnboarding();
			}
			return model.Token;
		}

		public string CheckReferral()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					referrerCode = referrerCode,
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/player-by-referral-code", jsonString, "application/json").ToString();


			}
			catch
			{
				body = "";
			}
			return model.Token;
		}

		public bool HandleDailyCheckin()
		{
			//so sánh lần cuối điểm danh là ngày nào để điểm danh tiếp
			var data = SyncGame();
			var lastCheckin = data.playerStats.tasks.dailyCheckin.lastCheckin;
			string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
			if (!string.IsNullOrEmpty(lastCheckin))
			{
				if (today != lastCheckin)
				{
					DailyCheckin();
				}
			}
			//Điểm danh lần đầu
			else
			{
				DailyCheckin();
			}

			return true;
		}
		private bool DailyCheckin()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					timezoneOffset = -420
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/tasks/daily-checkin", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private bool AddFriend()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					referrerCode = referrerCode
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/friends", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private bool CompleteOnboarding()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");

				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/onboarding/done").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		#region Handle Tap
		public void HandelTap()
		{
			var data = SyncGame();
			var totalEnergy = data.playerStats.energy.available;
			//var energyParts = GenerateEnergyDistribution(totalEnergy, 10);
		
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				//var jsonPayload = new
				//{
				//	availableEnergy = totalEnergy - usedEnergy,
				//	count = energyParts[i],
				//	duration = Form1.random.Next(35, 41),
				//	startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
				//};
				var jsonPayload = new
				{
					availableEnergy = totalEnergy,
					count = Form1.random.Next(35, 100),
					duration = Form1.random.Next(35, 41),
					startTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
				};
				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/tap", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
			}

		}


		#endregion

		#region Handle WheelSpins
		private int GetFortuneWheelState()
		{
			string availableSpinString = "";
			int availableSpin = 0;

			try
			{
				FunctionHelper.AddHeader(
					rq,
					header_temp + $@"
Content-Type: application/json
Authorization: Bearer {model.Token}"
				);

				body = rq.Get("https://dropee.clicker-game-api.tropee.com/api/game/fortune-wheel").ToString();

				availableSpinString = RegexHelper.GetValueFromRegex("\"available\":(.*?),", body);

				if (!int.TryParse(availableSpinString, out availableSpin))
				{
					availableSpin = 0; // Nếu không chuyển được, giữ giá trị mặc định
				}
			}
			catch
			{

			}

			return availableSpin; // Trả về số lượt quay hoặc 0 nếu có lỗi
		}

		private bool SpinFortuneWheel()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					version = 2
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/fortune-wheel/spin", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		public void HandleFortuneWheelSpins()
		{
			var fortuneWheelState = GetFortuneWheelState();
			for (int i = 0; i < fortuneWheelState; i++)
			{
				SpinFortuneWheel();
			}

		}
		#endregion

		#region HandleTask
		public void HandleTask()
		{
			var allInfor = GetConfig();
			var allTask = allInfor.config.tasks.ToList();
			var unCompletedTask = allTask.Where(t => !t.isDone).ToList();
			foreach (var task in unCompletedTask)
			{
				completeTask(task.id);

				ClaimTask(task.id);
			}
		}
		private bool completeTask(string taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					taskId = taskID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/tasks/action-completed", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private bool ClaimTask(string taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					taskId = taskID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/tasks/done", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		#endregion

		#region HandelCard Nâng cấp card 
		public void HandleUpgradeCard()
		{
			//Giá tối đa dùng để mua card giống bên dân cày
			var maxPriceCard = 500000;

			var allData = GetConfig();
			var allCard = allData.config.upgrades.Where(c => c.price < maxPriceCard
														&& c.price < availableCoins
														&& (c.expiresOn == null
														|| c.expiresOn > DateTimeOffset.UtcNow.ToUnixTimeSeconds())).ToList();
			foreach (var card in allCard)
			{
				if (card.price < availableCoins)
				{
					if (BuyCard(card.id))
					{
						availableCoins -= card.price;
					}
				}
			}
		}

		private bool BuyCard(string cardID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new
				{
					upgradeId = cardID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/actions/upgrade", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}
		#endregion

		private ConfigDto GetConfig()
		{
			ConfigDto config = new ConfigDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");

				body = rq.Get($"https://dropee.clicker-game-api.tropee.com/api/game/config").ToString();
				config = JsonConvert.DeserializeObject<ConfigDto>(body);
			}
			catch
			{
				body = "";
			}
			return config;
		}

		//Lấy data 
		private UserInfor SyncGame()
		{
			UserInfor infor = new UserInfor();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");
				var jsonPayload = new Dictionary<string, object>
{
	{ "initialSync", true },
	{ "utmSource", null }
};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://dropee.clicker-game-api.tropee.com/api/game/sync", jsonString, "application/json").ToString();
				infor = JsonConvert.DeserializeObject<UserInfor>(body);
				availableCoins = infor.playerStats.coins;
			}
			catch
			{
				body = "";
			}
			return infor;
		}
	}
}

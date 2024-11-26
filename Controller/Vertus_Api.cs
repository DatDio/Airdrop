using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airdrop.Model.Vertus;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json.Linq;
using Airdrop.Model.PinEye;
using TaskDto = Airdrop.Model.Vertus.TaskDto;
namespace Airdrop.Controller
{
	public class Vertus_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		UserInforDto userInfor;
		public Vertus_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;
			userInfor = new UserInforDto();
			//var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			//var urlJson = HttpUtility.UrlDecode(urlDecode);

			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin: https://thevertus.app
					  Referer: https://tonclayton.fun/games
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string GetProfile()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				body = rq.Post($"https://api.thevertus.app/users/get-data").ToString();
				userInfor = JsonConvert.DeserializeObject<UserInforDto>(body);
				if (String.IsNullOrEmpty(userInfor.user.walletAddress))
				{
					CreateNewWallet();
				}
				//var wallet = RegexHelper.GetValueFromRegex("\"walletAddress\":\"(.*?)\",", body);

				////Lần đầu đăng nhập phải tạo wallet
				//if (String.IsNullOrEmpty(wallet))
				//{

				//}

			}
			catch
			{
				body = "";
			}

			return body;
		}

		private string GetAllTask()
		{
			var Alltask = new TaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				return rq.Post($"https://api.thevertus.app/missions/get").ToString();
				Alltask = JsonConvert.DeserializeObject<TaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return "";
		}

		private bool ClaimTask(string taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");
				var jsonPayload = new
				{
					missionId = taskID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.thevertus.app/missions/complete", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}
		public void HandelTask()
		{
			var allTask = GetAllTask();

			//JObject obj = JObject.Parse(allTask);
			//JArray arr = (JArray)obj["newData"];
			//JArray arr1 = (JArray)arr[0];
			//foreach (JObject obj1 in arr1) {
			//	arr = (JArray)obj1["missions"];
			//	JArray arr2 = (JArray)arr[0];
			//	foreach (JObject obj2 in arr2)
			//	{
			//		var task = JsonConvert.DeserializeObject<TaskDto>(obj2.ToString());
			//		if (task.isCompleted) continue;
			//		ClaimTask(task._id);
			//	}
			//}


			JObject obj = JObject.Parse(allTask);
			JArray allArrayNewData = (JArray)obj["newData"];
			JArray arr1 = (JArray)allArrayNewData[0];
			foreach (JObject obj1 in arr1)
			{
				var arrMission = (JArray)obj1["missions"];
				JArray arr2 = (JArray)arrMission[0];
				foreach (JObject obj2 in arr2)
				{
					var task = JsonConvert.DeserializeObject<TaskDto>(obj2.ToString());
					if (task.isCompleted) continue;
					ClaimTask(task._id);
				}
			}


			//Task Sponsor
			try
			{
				// Kiểm tra allArrayNewData[1] có phải là JArray không
				if (allArrayNewData[1] is JArray arrSponsor)
				{
					foreach (var item in arrSponsor)
					{
						if (item is JObject obj1)
						{
							// Kiểm tra "missions" có tồn tại và là JArray
							if (obj1["missions"] is JArray arrSponsorMission)
							{
								foreach (var mission in arrSponsorMission)
								{
									if (mission is JObject obj2)
									{
										var task = JsonConvert.DeserializeObject<TaskDto>(obj2.ToString());
										if (task.isCompleted) continue;
										ClaimTask(task._id);
									}
								}
							}
						}
					}
				}
			}
			catch
			{

			}

			//Task Web3
			try
			{
				// Kiểm tra allArrayNewData[1] có phải là JArray không
				if (allArrayNewData[2] is JArray arrWeb3)
				{
					foreach (var item in arrWeb3)
					{
						if (item is JObject obj1)
						{
							// Kiểm tra "missions" có tồn tại và là JArray
							if (obj1["missions"] is JArray arrWeb3Mission)
							{
								foreach (var mission in arrWeb3Mission)
								{
									foreach (var missionWeb3 in mission)
									{
										if (missionWeb3 is JObject obj2)
										{
											var task = JsonConvert.DeserializeObject<TaskDto>(obj2.ToString());
											if (task.isCompleted) continue;
											ClaimTask(task._id);
										}
									}
										
								}
							}
						}
					}
				}
			}
			catch
			{

			}

		}


		public bool ClaimDaily()
		{
			var lastClaimDailyString = userInfor.user.dailyRewards.lastRewardClaimed;
			DateTimeOffset claimTimeOffset = DateTimeOffset.Parse(lastClaimDailyString);
			DateTime lastClaimDailyDateTime = claimTimeOffset.ToLocalTime().DateTime;
			if (lastClaimDailyDateTime >= DateTime.Now)
			{
				return false;
			}
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				body = rq.Post($"https://api.thevertus.app/users/claim-daily").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}

		public bool ClaimFarming()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				body = rq.Post($"https://api.thevertus.app/game-service/collect").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}

		private bool CompleteAdsGram(string missionId)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					missionId = missionId
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.thevertus.app/missions/complete-adsgram", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}

		private bool CheckAdsGram(string missionId)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					missionId = missionId
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.thevertus.app/missions/check-adsgram", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}

		private bool CreateNewWallet()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				body = rq.Post($"https://api.thevertus.app/users/create-wallet").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;

		}

		private Airdrop.Model.Vertus.CardDto GetAllCard()
		{
			var AllCard = new CardDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");

				body = rq.Get($"https://api.thevertus.app/upgrade-cards").ToString();
				AllCard = JsonConvert.DeserializeObject<CardDto>(body);

			}
			catch
			{
				body = "";
			}

			return AllCard;
		}

		private bool BuyCard(string cardID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");
				var jsonPayload = new
				{
					cardId = cardID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.thevertus.app/upgrade-cards/upgrade", jsonString, "application/json").ToString();
				
			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;
		}

		public void HandleCard()
		{
			var allCard = GetAllCard();
			foreach (var card in allCard.economyCards)
			{
				var avaiblePriceCard = card.levels[card.currentLevel].cost;
				if (card.isUpgradable && !card.isLocked && userInfor.user.balance> avaiblePriceCard)
				{
					if (BuyCard(card._id))
					{
						userInfor.user.balance-= avaiblePriceCard;
					}
				}
			}
		}

		//typeFarm: storage,farm,population
		private bool UpgradeFarming(string typeFarm)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	authorization: Bearer {model.InitData}");
				var jsonPayload = new
				{
					upgrade = typeFarm
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.thevertus.app/users/upgrade", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;
		}

		public void HandleUpgradeFarming()
		{
			//Mỗi lần sẽ nâng cấp 1 level
			try
			{
				double balance = double.Parse(userInfor.user.balance.ToString());
				var balanceConverted = balance / Math.Pow(10, 18);

				if (balanceConverted > userInfor.user.abilities.farm.priceToLevelUp)
				{
					if (UpgradeFarming("farm"))
					{
						balanceConverted -= userInfor.user.abilities.farm.priceToLevelUp;
					}
				}

				if (balanceConverted > userInfor.user.abilities.population.priceToLevelUp)
				{
					if (UpgradeFarming("population"))
					{
						balanceConverted -= userInfor.user.abilities.farm.priceToLevelUp;
					}
				}

				if (balanceConverted > userInfor.user.abilities.storage.priceToLevelUp)
				{
					if (UpgradeFarming("storage"))
					{
						balanceConverted -= userInfor.user.abilities.farm.priceToLevelUp;
					}
				}
			}
			catch
			{

			}
		}


	}
}

using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.PipWorld;
using Jint.Native;
using Leaf.xNet;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Airdrop.Controller
{
	public class PipWorld_Api
	{
		//Leaf.xNet.HttpRequest rq;
		RestClientOptions options;
		RestRequest request;
		RestClient client;
		AccountModel model;
		string header_temp, body;

		public PipWorld_Api(AccountModel accountModel)
		{
			options = new RestClientOptions()
			{
				MaxTimeout = -1,
				UserAgent = Form1.useragent
			};
			client = new RestClient(options);
			this.model = accountModel;
			//rq = new Leaf.xNet.HttpRequest();
			//rq.Cookies = new CookieStorage();
			//rq.AllowAutoRedirect = true;
			//rq.KeepAlive = true;
			//rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			//rq.UserAgent = Form1.useragent;

			//var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			//var urlJson = HttpUtility.UrlDecode(urlDecode);

			header_temp = $@"accept:*/*
				accept-language:vi,fr-FR;q=0.9,fr;q=0.8,en-US;q=0.7,en;q=0.6
				origin:https://tg.pip.world
				priority:u=1, i
				sec-ch-ua:""Google Chrome"";v=""129"", ""Not=A?Brand"";v=""8"", ""Chromium"";v=""129""
				sec-ch-ua-mobile:?0
				sec-ch-ua-platform:""Windows""
				sec-fetch-dest:empty
				sec-fetch-mode:cors
				sec-fetch-site:same-site
				user-agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36";
		}

		public string Login()
		{
			request = new RestRequest("https://api.tg.pip.world/app/post/login29458", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json");

			var jsonPayload = new
			{
				initData = model.InitData,
				referredBy = 0
			};
			request.AddJsonBody(jsonPayload);
			try
			{
				RestResponse response = client.Execute(request);
				body = response.Content.ToString();
			}
			catch
			{
				body = "";
			}

			return body;
		}

		public TaskDto GetTask()
		{
			var TaskDto = new TaskDto();
			try
			{
				var request = new RestRequest("https://api.tg.pip.world/app/get/getUserQuests", Method.Get);

				FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + $"authorization:{model.InitData}");
				//request.AddHeader("authorization", "query_id=AAFRK7RWAAAAAFErtFbmy4o7&user=%7B%22id%22%3A1454648145%2C%22first_name%22%3A%22%C4%90%E1%BA%A1t%22%2C%22last_name%22%3A%22Dio%22%2C%22username%22%3A%22DatDio%22%2C%22language_code%22%3A%22en%22%2C%22allows_write_to_pm%22%3Atrue%7D&auth_date=1730779593&hash=18776891e8917a678e6108b9971261157cce77c56e497bed47192b39a5ad97d2");

				// Thực hiện yêu cầu và lấy phản hồi
				RestResponse response = client.Execute(request);
				string body = response.Content;

				try
				{
					TaskDto = JsonConvert.DeserializeObject<TaskDto>(body);
				}
				catch
				{

				}

				// Chuyển đổi JSON sang List<TaskDto>
				return TaskDto;
			}
			catch
			{
				return TaskDto;
			}
		}

		public bool ClaimTask(string taskID)
		{
			var request = new RestRequest("https://api.tg.pip.world/app/post/checkQuest49944", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json"
																			+ "\n" + $"authorization:{model.InitData}");

			var jsonPayload = new
			{
				questId = taskID
			};
			request.AddJsonBody(jsonPayload);

			try
			{
				RestResponse response = client.Execute(request);
				return response.IsSuccessful;
			}
			catch
			{
				return false;
			}
		}

		public void HandleTask()
		{
			var TasksRespone = GetTask();
			var allTasks = TasksRespone.quests.quests;
			var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
			var uncompletedTask = allTasks.Where(t => !t.completed && (t.validUntil == null || currentTimestamp < t.validUntil)).ToList();

			foreach (var task in uncompletedTask)
			{
				ClaimTask(task.id);
				Thread.Sleep(1000);
			}
		}

		public void PerformTaps(UserInforDto userInfo)
		{
			bool isFirstTap = true;
			while (true)
			{
				var tapAmount = isFirstTap ? userInfo.user.coinsPerTap : userInfo.user.energy;
				if (tapAmount == 0) break;

				if (!isFirstTap && userInfo.user.freeTapsMultiplier.available > 0)
				{
					long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
					if (currentTime > userInfo.user.freeTapsMultiplier.lastTimeUpdated + 3600)
					{
						var updatedUser = ActivateFreeTapsMultiplier();
						if (updatedUser.user != null)
						{
							userInfo = updatedUser;
							tapAmount = userInfo.user.energy * 5;
							Debug.WriteLine($"Đã kích hoạt Tap boost. Số lần tap được tăng cường: {tapAmount}", "custom");
						}
					}
				}

				model.Status = "Tap";

				var request = new RestRequest("https://api.tg.pip.world/app/post/tapHandler22224", Method.Post);
				FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json"
																			+ "\n" + $"authorization:{model.InitData}");

				var jsonPayload = new
				{
					coins = tapAmount
				};
				request.AddJsonBody(jsonPayload);

				try
				{
					RestResponse response = client.Execute(request);
					body = response.Content.ToString();
					userInfo = JsonConvert.DeserializeObject<UserInforDto>(body);
					if (userInfo.user == null)
					{
						//Có lỗi xảy ra
						break;
					}
					isFirstTap = false;
				}
				catch
				{
					// Log or handle exception as needed
				}
				if (userInfo.user.energy < 20 && userInfo.user.freeEnergyRefills.available > 0)
				{
					userInfo = RefillEnergy();
					if (userInfo.user == null)
					{
						//Hết lần nạp năng lượng
						break;
					}
				}
				else if (userInfo.user.energy < 20)
				{
					//Hết lần nạp miễn phí
					break;
				}
			}
		}

		public void UpgradeCards(UserInforDto userInfor)
		{
			var availableItems = new UsertInvestItems();
			var UserOwnedInvestItems = new OwnedInvestItem();

			var request1 = new RestRequest("https://api.tg.pip.world/app/get/getUserInvestItems", Method.Get);
			FunctionHelper.AddHeaderRestSharp(request1, header_temp + $"\nauthorization:{model.InitData}");

			try
			{
				RestResponse response1 = client.Execute(request1);
				body = response1.Content.ToString();
				try
				{
					availableItems = JsonConvert.DeserializeObject<UsertInvestItems>(response1.Content ?? string.Empty);
				}
				catch
				{
				}

			}
			catch
			{
				// Handle exception
			}

			var request2 = new RestRequest("https://api.tg.pip.world/app/get/getUserOwnedInvestItems", Method.Get);
			FunctionHelper.AddHeaderRestSharp(request2, header_temp + $"\nauthorization:{model.InitData}");

			try
			{
				RestResponse response2 = client.Execute(request2);
				UserOwnedInvestItems = JsonConvert.DeserializeObject<OwnedInvestItem>(response2.Content ?? string.Empty);
			}
			catch
			{
				// Handle exception
			}

			var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			foreach (var item in availableItems.investItems)
			{
				if(item.requiredLevel > userInfor.user.level.level)
				{
					continue;
				}
				// Kiểm tra nếu thẻ đã hết hạn
				if (item.validUntil.HasValue && currentTimestamp > item.validUntil.Value)
				{
					//Hết hạn
					continue;
				}

				var ownedItem = UserOwnedInvestItems.userOwnedInvestItems.FirstOrDefault(i => i.id == item.id);

				
				// Kiểm tra nâng cấp cho thẻ đã sở hữu
				if (item.upgradeValuePerHour > 0 && ownedItem != null)
				{

					if (userInfor.user.balance >= item.price && item.price <= 1000000)
					{
						var buyResult = BuyInvestItem(item.id);
						if (buyResult.user == null)
						{
							Debug.WriteLine($"Không thể nâng cấp thẻ: {item.id}");
							continue;
						}
						userInfor = buyResult;
					}
					else
					{
						Debug.WriteLine($"Không đủ tiền để nâng cấp thẻ {item.title} ({item.price} > {userInfor.user.balance})");
					}
				}

				// Kiểm tra mua thẻ mới nếu chưa sở hữu
				else if (ownedItem == null )
				{
					if (userInfor.user.balance >= item.price && item.price <= 1000000)
					{
						var buyResult = BuyInvestItem(item.id);
						if (buyResult.user == null)
						{
							Debug.WriteLine($"Không thể mua thẻ: {item.id}");
							continue;
						}
						userInfor = buyResult;
					}
				}
			}


		}

		public UserInforDto ActivateFreeTapsMultiplier()
		{
			var request = new RestRequest("https://api.tg.pip.world/app/post/activateFreeTapsMultiplier", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json"
																			+ "\n" + $"authorization:{model.InitData}");

			try
			{
				RestResponse response = client.Execute(request);
				body = response.Content.ToString();
				return JsonConvert.DeserializeObject<UserInforDto>(response.Content ?? string.Empty);
			}
			catch
			{
				return null;
			}
		}

		public UserInforDto RefillEnergy()
		{
			var request = new RestRequest("https://api.tg.pip.world/app/post/activateFreeRefillEnergy", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json"
																			+ "\n" + $"authorization:{model.InitData}");

			try
			{
				RestResponse response = client.Execute(request);
				return JsonConvert.DeserializeObject<UserInforDto>(response.Content ?? string.Empty);
			}
			catch
			{
				return null;
			}
		}

		public UserInforDto BuyInvestItem(string itemID)
		{
			var request = new RestRequest("https://api.tg.pip.world/app/post/buyInvestItem38539", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp + "\n" + "content-type:application/json"
																			+ "\n" + $"authorization:{model.InitData}");

			var jsonPayload = new
			{
				itemId = itemID
				//itemId = "carpet_1"
			};
			request.AddJsonBody(jsonPayload);

			try
			{
				RestResponse response = client.Execute(request);
				return JsonConvert.DeserializeObject<UserInforDto>(response.Content ?? string.Empty);
			}
			catch
			{
				return null;
			}
		}

		public bool UpdateUserTradingGroup()
		{
			var request = new RestRequest("https://api.tg.pip.world/app/patch/updateUserTradingGroup", Method.Patch);
			FunctionHelper.AddHeaderRestSharp(request, header_temp);

			var jsonPayload = new
			{
				groupId = new Random().Next(1, 4)
			};
			request.AddJsonBody(jsonPayload);

			try
			{
				RestResponse response = client.Execute(request);
				body = response.Content;
				return response.IsSuccessful;
			}
			catch
			{
				return false;
			}
		}

		public bool BoardingCompleted()
		{
			var request = new RestRequest("/app/post/boardingCompleted", Method.Post);
			FunctionHelper.AddHeaderRestSharp(request, header_temp);

			try
			{
				RestResponse response = client.Execute(request);
				return response.IsSuccessful;
			}
			catch
			{
				return false;
			}
		}

		public void ProcessAccount()
		{
			UserInforDto uerInfor = new UserInforDto();
			var login = Login();
			try
			{
				uerInfor = JsonConvert.DeserializeObject<UserInforDto>(login);
			}
			catch
			{
			}

			if (!uerInfor.user.boardingCompleted)
			{
				UpdateUserTradingGroup();
				BoardingCompleted();
			}
			HandleTask();
			PerformTaps(uerInfor);


			//Update đang mỗi lần chơi là mua và nâng cấp 1 cấp
			UpgradeCards(uerInfor);
		}
	}
}

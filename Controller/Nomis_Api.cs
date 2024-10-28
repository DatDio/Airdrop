using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Nomis;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Airdrop.Controller
{
	public class Nomis_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, username, id;
		public Nomis_Api(AccountModel accountModel)
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
					  Origin:https://telegram.nomis.cc/
					Referer: https://telegram.nomis.cc/
					X-App-Init-Data: {model.InitData}
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
					  Authorization: Bearer 8e25d2673c5025dfcd36556e7ae1b330095f681165fe964181b13430ddeb385a0844815b104eff05f44920b07c073c41ff19b7d95e487a34aa9f109cab754303cd994286af4bd9f6fbb945204d2509d4420e3486a363f61685c279ae5b77562856d3eb947e5da44459089b403eb5c80ea6d544c5aa99d4221b7ae61b5b4cbb55
                      Sec-Fetch-Site:same-site";
		}

		public string Auth()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://cms-api.nomis.cc/api/users/auth", $"{{\"telegram_user_id\":\"{model.UserId}\",\"telegram_username\":\"{username}\",\"referrer\":\"\"}}", "application/json").ToString();

				//Lấy ra id để làm payload khi post claimTask
				id = RegexHelper.GetValueFromRegex("{\"id\":(.*?),", body);
			}
			catch
			{
				body = "";
			}

			return id;
		}


		public string GetReferralData()
		{
			try
			{
				//FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://cms-api.nomis.cc/api/users/referrals-data?user_id=${id}").ToString();

			
				id = RegexHelper.GetValueFromRegex("{\"id\":(.*?),", body);
			}
			catch
			{
				body = "";
			}

			return id;
		}

		public string GetProfile()
		{
			string nextFarmClaim = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://cms-api.nomis.cc/api/users/farm-data?user_id=${model.UserId}").ToString();
				var nextFarmClaimAt = RegexHelper.GetValueFromRegex("\"nextFarmClaimAt\":\"(.*?)\"", body);
				DateTimeOffset claimTimeOffset = DateTimeOffset.Parse(nextFarmClaimAt);
				DateTime localClaimTime = claimTimeOffset.ToLocalTime().DateTime;
				model.Time = localClaimTime.ToString();
				if (DateTime.Now >= localClaimTime)
				{
					ClaimFarm();
					StartFarm();
				}
			}
			catch
			{
				body = "";
			}

			return nextFarmClaim;
		}

		private List<TaskDto> GetTask()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://cms-api.nomis.cc/api/users/tasks").ToString();
				Alltask = JsonConvert.DeserializeObject<List<TaskDto>>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}

		private List<TaskDto> CheckTaskCompleted()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://cms-api.nomis.cc/api/users/tasks?completed=true").ToString();
				CompletedTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
			}

			return CompletedTask;
		}

		public void claimTask()
		{
			var allTasks = GetTask();
			var completedTasks = CheckTaskCompleted();

			// Lọc ra các nhiệm vụ chưa hoàn thành, có phần thưởng và handler không nằm trong danh sách loại bỏ
			var pendingTasks = allTasks
				.SelectMany(t => t.ton_twa_tasks)
				.Where(t => t.reward > 0
							&& !new[] { "telegramAuth", "pumpersToken", "pumpersTrade" }.Contains(t.handler)
							&& !completedTasks.SelectMany(ct => ct.ton_twa_tasks).Any(ct => ct.id == t.id))
				.ToList();

			foreach (var task in pendingTasks)
			{
				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");


					var jsonPayload = new
					{
						task_id = task.id,
						user_id = id
					};

					string jsonString = JsonConvert.SerializeObject(jsonPayload);
					body = rq.Post("https://cms-tg.nomis.cc/api/ton-twa-user-tasks/verify", jsonString, "application/json").ToString();
				}
				catch
				{

					body = "";
				}
			}
		}

		public bool StartFarm()
		{
			using (var rq = new Leaf.xNet.HttpRequest())
			{
				rq.Cookies = new CookieStorage();
				rq.AllowAutoRedirect = true;
				rq.KeepAlive = true;
				rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
				//rq.UserAgent = Form1.useragent;
				FunctionHelper.AddHeader(rq, $@"Connection: keep-alive
Content-Length: 0
sec-ch-ua: ""Chromium"";v=""128"", ""Not;A=Brand"";v=""24"", ""Google Chrome"";v=""128""
Accept: application/json, text/plain, */*
sec-ch-ua-mobile: ?0
X-APP-INIT-DATA: {model.InitData}
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36
sec-ch-ua-platform: ""Windows""
Origin: https://telegram.nomis.cc
Sec-Fetch-Site: same-site
Sec-Fetch-Mode: cors
Sec-Fetch-Dest: empty
Referer: https://telegram.nomis.cc/
Accept-Language: vi,fr-FR;q=0.9,fr;q=0.8,en-US;q=0.7,en;q=0.6");
				try
				{

					body = rq.Post($"https://cms-api.nomis.cc/api/users/start-farm").ToString();
					//var nextFarmClaimAt = RegexHelper.GetValueFromRegex("\"next_farm_claim_at\":\"(.*?)\"", body);
					//DateTimeOffset claimTimeOffset = DateTimeOffset.Parse(nextFarmClaimAt);
					//var nextFarmClaim = claimTimeOffset.ToString();
					//DateTime claimTime = DateTime.Parse(nextFarmClaimAt, null, System.Globalization.DateTimeStyles.RoundtripKind);

				}
				catch
				{
					body = "";
					var respon = rq.Response.ToString();
					var message = RegexHelper.GetValueFromRegex("{\"message\":\"(.*?)\"", respon);
					if (message == "Farm already started")
					{
						ClaimFarm();
					
					}
				}
			}


			return true;
		}

		public bool ClaimFarm()
		{
			using (var rq = new Leaf.xNet.HttpRequest())
			{
				rq.Cookies = new CookieStorage();
				rq.AllowAutoRedirect = true;
				rq.KeepAlive = true;
				rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
				//rq.UserAgent = Form1.useragent;
				FunctionHelper.AddHeader(rq, $@"Connection: keep-alive
Content-Length: 0
sec-ch-ua: ""Chromium"";v=""128"", ""Not;A=Brand"";v=""24"", ""Google Chrome"";v=""128""
Accept: application/json, text/plain, */*
sec-ch-ua-mobile: ?0
X-APP-INIT-DATA: {model.InitData}
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36
sec-ch-ua-platform: ""Windows""
Origin: https://telegram.nomis.cc
Sec-Fetch-Site: same-site
Sec-Fetch-Mode: cors
Sec-Fetch-Dest: empty
Referer: https://telegram.nomis.cc/
Accept-Language: vi,fr-FR;q=0.9,fr;q=0.8,en-US;q=0.7,en;q=0.6");
				try
				{
					//FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

					body = rq.Post($"https://cms-api.nomis.cc/api/users/claim-farm").ToString();
					var result = RegexHelper.GetValueFromRegex("{\"result\":(.*?)}", body);
					if (result == "true")
					{
						return true;
					}
				}
				catch
				{
					body = "";
					var respon = rq.Response.ToString();
				}
			}
			return false;
		}
	}
}

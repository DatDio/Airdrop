using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.TonStation;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop.Controller
{
	public class TonStation_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		string[] skipTaskIds = new string[] { "66dad41d9b1e65019ad30629", "66f560c1c6fc8ba931b33420" };

		public TonStation_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;



			header_temp = @"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: */*
					  Origin:https://tonstation.app
					  Referer: https://tonstation.app/app/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Content-Type:application/json
                      Sec-Ch-Ua-Platform:""Android""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}

		public string GetToken()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://tonstation.app/userprofile/api/v1/users/auth", $"{{\"initData\":\"{model.InitData}\"}}", "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			var token = RegexHelper.GetValueFromRegex("\"accessToken\":\"(.*?)\",", body);
			return RegexHelper.GetValueFromRegex("\"accessToken\":\"(.*?)\",", body);
		}

		public string GetFarmStatus()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, $@"Content-Type:application/json");

				body = rq.Get($"https://tonstation.app/farming/api/v1/farming/{model.UserId}/running").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}

			return body;
		}
		public bool ClaimFarm(string farmId)
		{
			try
			{
				var payload = new
				{
					userId = model.UserId,
					taskId = farmId
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://tonstation.app/farming/api/v1/farming/claim", jsonPayload, "application/json").ToString();
			}
			catch
			{
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}
		public bool StartFarm()
		{
			try
			{
				var payload = new
				{
					userId = model.UserId,
					taskId = "1"
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://tonstation.app/farming/api/v1/farming/start", jsonPayload, "application/json").ToString();
			}
			catch
			{
				var respone = rq.Response.ToString();
				return false;

			}

			return true;
		}

		public void HandleFarming()
		{
			var farmStatus = GetFarmStatus();
			var nexFarmTime = RegexHelper.GetValueFromRegex("\"timeEnd\":\"(.*?)\"", farmStatus);
			var nextFarmTimeLocal = DateTimeOffset.Parse(nexFarmTime).ToLocalTime().DateTime;
			model.Time = nextFarmTimeLocal.ToString();
			var farmID = RegexHelper.GetValueFromRegex("_id\":\"(.*?)\",", farmStatus);

			if (DateTime.Now >= nextFarmTimeLocal)
			{
				ClaimFarm(farmID);
				StartFarm();
			}

		}

		public void HandleTask()
		{
			var respone = GetTask();
			var allTask = JsonConvert.DeserializeObject<TaskDto>(body);
			foreach (var task in allTask.data)
			{
				if (skipTaskIds.Contains(task.id))
				{
					continue;
				}
				if(StartTask(task.id, task.project))
				{
					ClaimTask(task.id, task.project);
				}
				
			}
		}

		public string GetTask()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://tonstation.app/quests/api/v1/quests?userId={model.UserId}").ToString();

			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}

			return body;
		}

		public bool StartTask(string taskid, string project)
		{
			try
			{
				var payload = new
				{
					userId = model.UserId,
					questId = taskid,
					project = project
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://tonstation.app/quests/api/v1/start", jsonPayload, "application/json").ToString();
			}
			catch
			{
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public bool ClaimTask(string taskid, string project)
		{
			try
			{
				var payload = new
				{
					userId = model.UserId,
					questId = taskid,
					project = project
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://tonstation.app/quests/api/v1/claim", jsonPayload, "application/json").ToString();
			}
			catch
			{
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}
	}
}

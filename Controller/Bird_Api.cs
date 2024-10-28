using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Bird;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop
{
	public class Bird_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, username, id;
		public Bird_Api(AccountModel accountModel)
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
					  Origin:https://birdx.birds.dog
					  Referer: https://birdx.birds.dog/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string GetInfo()
		{
			try
			{

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/user").ToString();
				model.Level = RegexHelper.GetValueFromRegex("\"level\":(.*?)}", body);
				model.Amount = RegexHelper.GetValueFromRegex("\"balance\":(.*?),", body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return model.Level;
		}
		public bool CallWormMintAPI()
		{
			string status = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Authorization: tma {model.InitData}");
				body = rq.Get($"https://worm.birds.dog/worms/mint-status").ToString();
			}
			catch
			{
				return false;
			}
			status = RegexHelper.GetValueFromRegex("{\"status\":\"(.*?)\",", body);
			if (status == "MINT_OPEN")
			{
				CatchWorm();
			}
			else if (status == "WAITING")
			{
				var nextMintTime = RegexHelper.GetValueFromRegex("\"nextMintTime\":\"(.*?)\"", body);
				var nextMintTimeLocal = DateTimeOffset.Parse(nextMintTime).ToLocalTime().DateTime;
				model.Time = nextMintTimeLocal.ToString();
			}
			return true;
		}

		public bool PlayEggMinigame()
		{
			string turn = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/minigame/egg/join").ToString();
			}
			catch
			{
				return false;
			}
			turn = RegexHelper.GetValueFromRegex("\"turn\":(.*?),", body);
			if (int.TryParse(turn, out int t))
			{
				for (int i = 0; i < t; i++)
				{
					if (!BreakEgg())
					{
						Thread.Sleep(1000);
						break;
					}
				}
				ClaimEgg();
			}


			return true;
		}

		private bool BreakEgg()
		{
			string status = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/minigame/egg/play").ToString();
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return body.Contains("result");
		}

		private bool ClaimEgg()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/minigame/egg/claim").ToString();
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return body.Contains("true");
		}

		private void CatchWorm()
		{
			string status = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Authorization: tma {model.InitData}");
				body = rq.Post($"https://worm.birds.dog/worms/mint").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			status = RegexHelper.GetValueFromRegex("\"message\":\"MISSED\"", body);
			if (status == "MISSED")
			{
				model.Status = "";
			}

			//Thời gian bắt sâu tiếp theo
			var nextMintTime = RegexHelper.GetValueFromRegex("\"nextMintTime\":\"(.*?)\"", body);
			var nextMintTimeLocal = DateTimeOffset.Parse(nextMintTime).ToLocalTime().DateTime;
			model.Time = nextMintTimeLocal.ToString();
		}

		public bool UpgradeEgg()
		{
			//Get info level egg:
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/minigame/incubate/info").ToString();
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			model.Level = RegexHelper.GetValueFromRegex("\"level\":(.*?),", body);
			var upgradedAt = RegexHelper.GetValueFromRegex("\"upgradedAt\":(.*?),", body);
			var duration = RegexHelper.GetValueFromRegex("\"duration\":(.*?),", body);
			var currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			var upgradeCompletionTime = long.Parse(upgradedAt) + (long.Parse(duration) * 60 * 60 * 1000);
			var status = RegexHelper.GetValueFromRegex("\"status\": \"(.*?)\",", body);
			if (status == "processing")
			{
				if (currentTime > upgradeCompletionTime)
				{
					confirmUpgraded();
				}
			}
			else if (status == "confirmed")
			{
				body = Upgrade();
				model.Level = RegexHelper.GetValueFromRegex("\"level\":(.*?),", body);
				upgradedAt = RegexHelper.GetValueFromRegex("\"upgradedAt\":(.*?),", body);
				duration = RegexHelper.GetValueFromRegex("\"duration\":(.*?),", body);
				currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
				upgradeCompletionTime = long.Parse(upgradedAt) + (long.Parse(duration) * 60 * 60 * 1000);
				//DateTimeOffset claimTimeOffset = DateTimeOffset.Parse(nextFarmClaimAt);
				var time = FunctionHelper.TimeStampToDateTime(upgradeCompletionTime);
				//DateTime localClaimTime = claimTimeOffset.ToLocalTime().DateTime;
				model.Time = time.ToString();
			}


			return true;
		}
		private void confirmUpgraded()
		{
			string status = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Authorization: tma {model.InitData}");
				body = rq.Post($"https://api.birds.dog/minigame/incubate/confirm-upgraded").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
		}

		private string Upgrade()
		{
			try
			{
				var payload = new
				{
					userId = model.UserId,
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"telegramauth: tma {model.InitData}");

				body = rq.Get($"https://api.birds.dog/minigame/incubate/upgrade").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			return body;
		}

		public void HandelTask()
		{
			var allTasks = GetAllTasks(); // Trả về một List chứa tất cả các tasks
			var joinedTasks = GetJoinedTasks(); // Trả về một List chứa các task đã tham gia

			var incompleteTasks = allTasks
	.SelectMany(taskDto => taskDto.tasks) // Truy cập từng `Task` trong `allTasks`
	.Where(task => !joinedTasks.Any(joinedTask => joinedTask.taskId == task._id)) // So sánh `task._id` với `joinedTask._id`
	.ToList();


			foreach (var task in incompleteTasks)
			{
				var s = ClaimTask(task._id, task.channelId, task.slug, task.point);
				if (s)
				{

				}
			}
		}

		private List<TaskDto> GetAllTasks()
		{
			var allTask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/project").ToString();
				allTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return allTask;
		}

		private List<JointedTaskDto> GetJoinedTasks()
		{
			var jointedTaskDto = new List<JointedTaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				body = rq.Get($"https://api.birds.dog/user-join-task").ToString();
				jointedTaskDto = JsonConvert.DeserializeObject<List<JointedTaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return jointedTaskDto;
		}

		private bool ClaimTask(string taskID, string channelId, string slug, int point)
		{
			string status = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $"Telegramauth: tma {model.InitData}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				var jsonPayload = new
				{
					task_id = taskID,
					channelId = channelId,
					slug = slug,
					point = point
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);

				body = rq.Post($"https://api.birds.dog/project/join-task", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			return body.Contains("Successfully");
		}
	}
}

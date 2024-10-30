using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Bird;
using Airdrop.Model.Clayton;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TaskDto = Airdrop.Model.Clayton.TaskDto;

namespace Airdrop.Controller
{
	public class Clayton_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		public Clayton_Api(AccountModel accountModel)
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
					  Origin:https://tonclayton.fun
					  Referer: https://tonclayton.fun/games
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
					  init-data:{model.InitData}
                      Sec-Fetch-Site:same-site";
		}
		private string Login()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Post($"https://tonclayton.fun/api/user/authorization").ToString();
				model.Amount = RegexHelper.GetValueFromRegex("\"tokens\":(.*?),", body);
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return body;
		}

		#region DailyTask
		private void handleDailyTasks()
		{
			List<TaskDto> allDaiLyTask = null;
			for (int i = 0; i <= 3; i++)
			{
				if (i >= 3) return;

				allDaiLyTask = GetDailyTask();
				if (allDaiLyTask.Count > 0) break;
				else Thread.Sleep(2000);
			}
			var uncompletedDailyTasks = allDaiLyTask.Where(t => !t.is_completed && !t.is_claimed).ToList();
			foreach (var task in uncompletedDailyTasks)
			{
				for (int i = 0; i <= 5; i++)
				{
					if (CompleteTask(task.task_id))
					{
						claimTask(task.task_id);
						break;
					}
					else
					{
						Thread.Sleep(1000);
					}
				}
				Thread.Sleep(1000);
			}
		}

		private List<TaskDto> GetDailyTask()
		{
			var allDaiLyTask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Get($"https://tonclayton.fun/api/tasks/daily-tasks").ToString();
				allDaiLyTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return allDaiLyTask;
		}



		#endregion

		//Điểm danh hằng ngày:
		private bool ClaimDaily()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				//var jsonPayload = new
				//{
				//	task_id = taskID,
				//	channelId = channelId,
				//	slug = slug,
				//	point = point
				//};

				//string jsonString = JsonConvert.SerializeObject(jsonPayload);

				body = rq.Post($"https://tonclayton.fun/api/user/daily-claim").ToString();
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return body.Contains("Successfully");
		}

		#region PartnerTask
		private List<TaskDto> GetPartnerTasks()
		{
			var allPartnerTask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				body = rq.Get($"https://tonclayton.fun/api/tasks/partner-tasks").ToString();
				allPartnerTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return allPartnerTask;
		}

		private void handlePartnerTasks()
		{
			var allPartnerTask = GetPartnerTasks();
			var uncompletedDailyTasks = allPartnerTask.Where(t => !t.is_completed && !t.is_claimed).ToList();
			foreach (var task in uncompletedDailyTasks)
			{
				for (int i = 0; i <= 5; i++)
				{
					if (CompleteTask(task.task_id))
					{
						claimTask(task.task_id);
						break;
					}
					else
					{
						Thread.Sleep(1000);
					}
				}
				Thread.Sleep(1000);
			}
		}
		#endregion


		#region DefaultTask
		private void handleDefaultTasks()
		{
			List<TaskDto> allDefaultTasks = null;
			for (int i = 0; i <= 3; i++)
			{
				if (i >= 3) return;

				allDefaultTasks = GetDefaultTask();
				if (allDefaultTasks.Count > 0) break;
				else Thread.Sleep(1000);

			}

			var uncompletedDefaultTasks = allDefaultTasks.Where(t => !t.is_completed && !t.is_claimed).ToList();
			foreach (var task in uncompletedDefaultTasks)
			{
				for (int i = 0; i <= 3; i++)
				{
					if (CompleteTask(task.task_id))
					{
						claimTask(task.task_id);
						break;
					}
					else
					{
						Thread.Sleep(1000);
					}
				}
			}
		}

		private List<TaskDto> GetDefaultTask()
		{
			var allDefaultTask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Get($"https://tonclayton.fun/api/tasks/default-tasks").ToString();
				allDefaultTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return allDefaultTask;
		}
		#endregion

		#region SuperTask
		private void handleSuperTask()
		{
			List<TaskDto> allSuperTask = null;
			for (int i = 0; i <= 3; i++)
			{
				if (i >= 3) return;

				allSuperTask = GetSuperTask();
				if (allSuperTask.Count > 0) break;
				else Thread.Sleep(1000);

			}
			var uncompletedSuperTask = allSuperTask.Where(t => !t.is_completed).ToList();
			foreach (var task in uncompletedSuperTask)
			{
				for (int i = 0; i <= 5; i++)
				{
					if (CompleteTask(task.task_id))
					{
						claimTask(task.task_id);
					}
				}
			}
		}

		private List<TaskDto> GetSuperTask()
		{
			var allSuperTask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Get($"https://tonclayton.fun/api/tasks/super-tasks").ToString();
				allSuperTask = JsonConvert.DeserializeObject<List<TaskDto>>(body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return allSuperTask;
		}

		#endregion

		#region PlayGame
		private void Play2048()
		{
			string sessionID = "";
			int finalTile = 0;
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Post($"https://tonclayton.fun/api/game/start").ToString();
				sessionID = RegexHelper.GetValueFromRegex("\"session_id\":\"(.*?)\"", body);
			}
			catch
			{

			}

			// Các mốc điểm cần đạt
			var fixedMilestones = new int[] { 4, 8, 16, 32, 64, 128, 256, 512, 1024 };
			var gameEndTime = DateTime.Now.AddMilliseconds(150000);

			// Cập nhật điểm cho đến khi đạt hết mốc hoặc hết thời gian
			foreach (var milestone in fixedMilestones)
			{
				if (DateTime.Now >= gameEndTime) break;

				Thread.Sleep(new Random().Next(5000, 15000));

				try
				{
					finalTile = milestone;
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
					var jsonPayload = new
					{
						maxTile = milestone,
						session_id = sessionID
					};

					string jsonString = JsonConvert.SerializeObject(jsonPayload);
					body = rq.Post($"https://tonclayton.fun/api/game/save-tile", jsonString, "application/json").ToString();
				}
				catch
				{

				}

			}

			// Kết thúc trò chơi
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					maxTile = finalTile,
					multiplier = 1,
					session_id = sessionID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://tonclayton.fun/api/game/over", jsonString, "application/json").ToString();
			}
			catch
			{

			}

		}
		private void playStack()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				body = rq.Post($"https://tonclayton.fun/api/stack/st-game").ToString();
			}
			catch
			{
				body = "";
				return;
			}
			var gameEndTime = DateTime.Now.AddMilliseconds(120000);
			var scores = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
			int currentScoreIndex = 0;

			// Cập nhật điểm cho đến khi hết thời gian hoặc đạt đến điểm cuối cùng
			while (DateTime.Now < gameEndTime && currentScoreIndex < scores.Length)
			{
				int score = scores[currentScoreIndex];

				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
					var jsonPayload = new
					{
						score = score,
					};

					string jsonString = JsonConvert.SerializeObject(jsonPayload);
					body = rq.Post($"https://tonclayton.fun/api/stack/update-game", jsonString, "application/json").ToString();
					currentScoreIndex++;
				}
				catch
				{

				}
				Thread.Sleep(new Random().Next(5000, 15000));
			}


			int finalScore = scores[Math.Max(0, currentScoreIndex - 1)];

			// Kết thúc trò chơi Stack
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					score = finalScore,
					multiplier = 1
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://tonclayton.fun/api/stack/en-game", jsonString, "application/json").ToString();
			}
			catch
			{

			}
		}
		private void playGames()
		{
			var login = Login();
			if (login.Contains("error"))
			{
				return;
			}
			var dailyAttemptsString = RegexHelper.GetValueFromRegex("\"daily_attempts\":(.*?),", body);

			if (int.TryParse(dailyAttemptsString, out int tickets))
			{
				if (tickets <= 0) return;

				for (int i = 0; i < tickets; i++)
				{
					playStack();
				}

				//Game 2048 đang bị lỗi ko claim được
				//if (tickets >= 2)
				//{
				//	Play2048();
				//	if (tickets > 1)
				//	{
				//		playStack();
				//	}
				//}
				//else
				//{
				//	Play2048();
				//}
			}


		}
		#endregion

		public void ProccessAccount()
		{
			// Server con game đỏ suốt ngày nên phải for nhiều! lỏ quá
			for (int i = 0; i <= 5; i++)
			{
				var login = Login();
				if (!login.Contains("error"))
				{
					var canClaimToday = RegexHelper.GetValueFromRegex("\"can_claim_today\":(.*?),", body);
					if (canClaimToday == "true")
					{
						ClaimDaily();
					}
					break;
				}
				else
				{
					Thread.Sleep(1000);
				}
				if (i >= 5) return;
			}


			playGames();
			handleDefaultTasks();
			handlePartnerTasks();
			handleDailyTasks();
			handleSuperTask();

		}


		private bool CompleteTask(int taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				var jsonPayload = new
				{
					task_id = taskID,
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://tonclayton.fun/api/tasks/complete", jsonString, "application/json").ToString();

			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;
		}

		private bool claimTask(int taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + "Content-Type: application/json");

				var jsonPayload = new
				{
					task_id = taskID,
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://tonclayton.fun/api/tasks/claim", jsonString, "application/json").ToString();
			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;
		}
	}
}

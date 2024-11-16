using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Frog_Farm;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Airdrop.Controller
{
	public class FrogFarm_Api
	{
		private UserInforDto _userInforDto;
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body, first_name, last_name, username;
		public FrogFarm_Api(AccountModel accountModel)
		{
			_userInforDto = new UserInforDto();
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			var urlJson = HttpUtility.UrlDecode(urlDecode);
			first_name = RegexHelper.GetValueFromRegex("\"first_name\":\"(.*?)\"", urlJson);
			last_name = RegexHelper.GetValueFromRegex("\"last_name\":\"(.*?)\"", urlJson);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://web.frogfarm.site
					  Referer: https://web.frogfarm.site/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
					  init-data:{model.InitData}
                      Sec-Fetch-Site:same-site";
		}
		public UserInforDto Login()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				var jsonPayload = new
				{
					first_name = first_name,
					last_name = last_name,
					referral = "1454648145",
					user_id = model.UserId,
					username = username
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/per/hour", jsonString, "application/json").ToString();
				_userInforDto = JsonConvert.DeserializeObject<UserInforDto>(body);
				DateTime localClaimTime = _userInforDto.data.last_coins_per_hour_at.ToLocalTime();
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return _userInforDto;
		}

		private TaskDto GetAllTask()
		{
			var Alltask = new TaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp);

				body = rq.Get($"https://api.frogfarm.site/api/tasks").ToString();
				Alltask = JsonConvert.DeserializeObject<TaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}

		private bool ClaimTask(int taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/task/{taskID}").ToString();
				_userInforDto = JsonConvert.DeserializeObject<UserInforDto>(body);
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
			var completedTasks = _userInforDto.data.tasks;
			foreach (var task in allTask.data)
			{
				if (completedTasks.Any(t => t.id == task.id)) continue;
				ClaimTask(task.id);
			}
		}

		public string StartFarming()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/claim").ToString();
				_userInforDto = JsonConvert.DeserializeObject<UserInforDto>(body);
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return body;
		}

		public bool DailyRewards()
		{
			if (_userInforDto.data.active_day_bonus != null)
			{
				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

					body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/bonus").ToString();
					_userInforDto = JsonConvert.DeserializeObject<UserInforDto>(body);
				}
				catch
				{
					body = "";
					return false;
					//var respone = rq.Response.ToString();
				}
				return true;
			}
			return false;
		}

		public bool ClaimReferral()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/referral/claim").ToString();
			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			return true;
		}


		public bool PlayGameFrog()
		{
			for (int i = 0; i < _userInforDto.data.game_energy; i++)
			{
				//Start Game
				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

					body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/game-run/start").ToString();

				}
				catch
				{
					body = "";
					return false;
					//var respone = rq.Response.ToString();
				}
				Random random = new Random();
				int timePlay = random.Next(50, 100) * 100;

				//Finish Game
				try
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
					var jsonPayload = new
					{
						croaks = timePlay,
						score = timePlay + random.Next(20, 50)
					};

					string jsonString = JsonConvert.SerializeObject(jsonPayload);
					body = rq.Post($"https://api.frogfarm.site/api/user/{model.UserId}/game-run/finish", jsonString, "application/json").ToString();

				}
				catch
				{
					body = "";
					return false;
					//var respone = rq.Response.ToString();
				}
			}

			return true;
		}
	}
}

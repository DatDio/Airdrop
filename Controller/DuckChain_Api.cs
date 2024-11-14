using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airdrop.Model.DuckChain;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
namespace Airdrop.Controller
{
	public class DuckChain_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body, username;
		public DuckChain_Api(AccountModel accountModel)
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
					  Origin:https://tgdapp.duckchain.io
					  Referer: https://tgdapp.duckchain.io/
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
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");
				body = rq.Get($"https://preapi.duckchain.io/user/info").ToString();
				var duckName = RegexHelper.GetValueFromRegex("\"duckName\":\"(.*?)\",", body);
				if (String.IsNullOrEmpty(duckName))
				{
					SetDuckName();
				}
			}
			catch
			{
				//body = "";
				//var respone = rq.Response.ToString();
			}
			return body;
		}
		private TaskDto GetAllTask()
		{
			var Alltask = new TaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/task/task_list").ToString();
				Alltask = JsonConvert.DeserializeObject<TaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}
		private TaskInfor GetTaskInfor()
		{
			var Alltask = new TaskInfor();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/task/task_info").ToString();
				Alltask = JsonConvert.DeserializeObject<TaskInfor>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}
		private bool ClaimTaskOneTime(int taskId)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/task/onetime?taskId={taskId}").ToString();


			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}
		private bool ClaimTaskPartner(int taskId)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/task/partner?taskId={taskId}").ToString();

			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}
		public void HandleTask()
		{
			CollectDailyEgg();
			var allUserTask = GetTaskInfor();
			var allTask = GetAllTask();
			foreach (var task in allTask.data.daily)
			{
				var taskCompleted = allUserTask.data.daily.FirstOrDefault(t => t == 8);
				if (task.taskId == 8 && taskCompleted != 8)
				{
					DailyCheckIn();
				}
			}
			foreach (var task in allTask.data.oneTime)
			{
				if (!allUserTask.data.oneTime.Contains(task.taskId))
				{
					ClaimTaskOneTime(task.taskId);
				}
			}
			foreach (var task in allTask.data.partner)
			{
				if (!allUserTask.data.partner.Contains(task.taskId))
				{
					ClaimTaskPartner(task.taskId);
				}
			}
		}
		private bool SetDuckName()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/user/set_duck_name?duckName={username}").ToString();

			}
			catch
			{
				body = "";
				return false;
			}

			return true;
		}

		public bool DailyCheckIn()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/task/sign_in").ToString();
			}
			catch
			{

				body = "";
				return false;
			}
			return true;
		}

		public bool CollectDailyEgg()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/property/daily/isfinish?taskId=1").ToString();
			}
			catch
			{

				body = "";
				return false;
			}

			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization: tma {model.InitData}");

				body = rq.Get($"https://preapi.duckchain.io/property/daily/finish?taskId=1").ToString();
			}
			catch
			{

				body = "";
				return false;
			}
			return true;
		}
	}
}

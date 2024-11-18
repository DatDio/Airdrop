using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airdrop.Controller
{
	public class Vertus_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		public Vertus_Api(AccountModel accountModel)
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
					  Origin: https://thevertus.app
					  Referer: https://tonclayton.fun/games
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
					  authorization: Bearer {model.InitData}
                      Sec-Fetch-Site:same-site";
		}
		public string GetProfile()
		{
			string nextFarmClaim = "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.thevertus.app/users/get-data").ToString();

				var wallet = RegexHelper.GetValueFromRegex("\"walletAddress\": \"(.*?)\",", body);

				//Lần đầu đăng nhập phải tạo wallet
				if (String.IsNullOrEmpty(wallet))
				{
					CreateNewWallet();
				}

			}
			catch
			{
				body = "";
			}

			return nextFarmClaim;
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

		public bool ClaimDaily()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

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

		public bool CreateNewWallet()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

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
	}
}

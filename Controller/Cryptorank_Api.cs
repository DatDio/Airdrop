using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.CryptoRank;
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
	internal class Cryptorank_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		string header_temp, body;
		public Cryptorank_Api(AccountModel accountModel)
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

			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7
					  Origin:https://tma.cryptorank.io
					Referer: https://tma.cryptorank.io/
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
				//var jsonPayload = new
				//{
				//	inviterId = 1454648145
				//};
				//string jsonString = JsonConvert.SerializeObject(jsonPayload);

				//FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
				//													Authorization:{model.Token}");

				//body = rq.Post($"https://api.cryptorank.io/v0/tma/account", jsonString, "application/json").ToString();

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.cryptorank.io/v0/tma/account").ToString();
				model.Amount = RegexHelper.GetValueFromRegex("\"balance\":(.*?),", body);

			}
			catch
			{
				body = "";
				var reponse = rq.Response.ToString();
			}

			return body;
		}


		public void HandleTask()
		{
			var allTask = GetTask();
			foreach (var task in allTask)
			{
				if(task.isDone==true) continue;

				claimTask(task.id);
			}
			
		}


		private List<TaskDto> GetTask()
		{
			var Alltask = new List<TaskDto>();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.cryptorank.io/v0/tma/account/tasks").ToString();
				Alltask = JsonConvert.DeserializeObject<List<TaskDto>>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}


		private void claimTask(string taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");


				var jsonPayload = new
				{
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.cryptorank.io/v0/tma/account/claim/task/{taskID}", jsonString, "application/json").ToString();
			}
			catch
			{

				body = "";
			}

		}

		public void HandleBuddies()
		{
			var status = GetBuddies();
			var cooldown = RegexHelper.GetValueFromRegex("cooldown\":(.*?),", status);
			if (long.TryParse(cooldown, out long cooldownValue))
			{
				if (cooldownValue == 0)
				{
					ClaimBuddies();
				}
			}
		}


		public void HandleFarm()
		{
			var info = GetInfo();
			var farmingState = RegexHelper.GetValueFromRegex("\"state\":\"(.*?)\",", info);
			if(double.TryParse(RegexHelper.GetValueFromRegex("\"timestamp\":(.*?)},", info), out double nexTimeFarmDouble))
			{
				var nexTimeFarm = FunctionHelper.TimeStampToDateTime(nexTimeFarmDouble);
				if(DateTime.Now >= nexTimeFarm)
				{
					ClaimFarm();

					StartFarm();
				}
			}
			
			//if (farmingState== "START")
			//{
			//	return;
			//}
			//StartFarm();
		}

		private bool StartFarm()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.cryptorank.io/v0/tma/account/start-farming").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private bool ClaimFarm()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api.cryptorank.io/v0/tma/account/end-farming").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private bool ClaimBuddies()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");

				body = rq.Post($"https://api.cryptorank.io/v0/tma/account/claim/buddies").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}
		private string GetBuddies()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.cryptorank.io/v0/tma/account/buddies").ToString();
			}
			catch
			{
				body = "";
			}
			return body;
		}
	}
}

using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Coub;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop.Controller
{
	public class Coub_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, urlJson, username, auth_date, chat_instance, start_param, hash;
		public Coub_Api(AccountModel accountModel)
		{
			this.model = accountModel;
			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			var urlDecode = HttpUtility.UrlDecode(accountModel.InitParam);
			urlJson = HttpUtility.UrlDecode(urlDecode);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			auth_date = RegexHelper.GetValueFromRegex("auth_date=(.*?)&", urlJson);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			chat_instance = RegexHelper.GetValueFromRegex("chat_instance=(.*?)&", urlJson);
			start_param = RegexHelper.GetValueFromRegex("start_param=(.*?)&", urlJson);
			hash = RegexHelper.GetValueFromRegex("hash=(.*?)&", urlJson);
			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: application/json, text/plain, */*
					  Origin:https://coub.com
					  Referer: https://coub.com/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		private string Login()
		{

			try
			{
				var user = RegexHelper.GetValueFromRegex("user=(.*?)&", urlJson);
				var param = new RequestParams
				{
					["user"] = user,
					["chat_instance"] = chat_instance,
					["chat_type"] = "private",
					["start_param"] = start_param,
					["auth_date"] = auth_date,
					["hash"] = hash,
				};
				//FunctionHelper.AddHeader(rq, header_temp + "\n" + $"content-type:application/x-www-form-urlencoded");
				body = rq.Post($"https://coub.com/api/v2/sessions/login_mini_app", param).ToString();
				model.Token = RegexHelper.GetValueFromRegex("\"api_token\":\"(.*?)\"", body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return model.Token;
		}

		public string GetToken()
		{
			var api_Token = Login();
			if (api_Token == "") return "";
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"x-auth-token:{model.Token}");
				body = rq.Post($"https://coub.com/api/v2/torus/token").ToString();
				model.Token = RegexHelper.GetValueFromRegex("\"access_token\":\"(.*?)\"", body);
			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}
			return model.Token;
		}

		//Lấy ra các task đã hoàn thành
		private List<TaskFinishedDto> GetUserRewards()
		{
			var taskFinished = new List<TaskFinishedDto>();
			try
			{

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"x-tg-authorization:{model.InitData}
																	authorization:Bearer {model.Token}");
				body = rq.Get($"https://rewards.coub.com/api/v2/get_user_rewards").ToString();
				taskFinished = JsonConvert.DeserializeObject<List<TaskFinishedDto>>(body);
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}
			return taskFinished;
		}
		public void HandleTask()
		{
			var taskFinished = GetUserRewards();

			for (int i = 0; i < 100; i++)
			{
				if (taskFinished.Any(t => t.id == i)) continue;
				if (!ClaimTask(i)) break;
			}
		}
		private bool ClaimTask(int taskID)
		{
			try
			{

				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, $"x-tg-authorization:{model.InitParam}");
				body = rq.Get($"https://rewards.coub.com/api/v2/complete_task?task_reward_id={taskID}").ToString();
			}
			catch
			{
				body = "";
				return false;
				//var respone = rq.Response.ToString();
			}
			Debug.WriteLine(body);
			return true;
		}


	}
}

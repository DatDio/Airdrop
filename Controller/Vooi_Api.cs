using Airdrop.Helper;
using Airdrop.Model;
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
	public class Vooi_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<Vooi_Api> Alltask, CompletedTask;
		string header_temp, body, username, id;
		public Vooi_Api(AccountModel accountModel)
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
					  Origin:https://app.tg.vooi.io
					Referer: https://app.tg.vooi.io/
                      Sec-Ch-Ua-Mobile:?1
				sec-ch-ua:""Chromium"";v=""128"", ""Not;A=Brand"";v=""24"", ""Google Chrome"";v=""128""
                      Sec-Ch-Ua-Platform:""Windows""
					sentry-trace:f20c9a4739274a30ba3099a8b019e676-8cff5a7d72dff025-1
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
				baggage:sentry-environment=production,sentry-release=ecb43e10896e398562fe347e6a46b6390837daa2,sentry-public_key=148714cb7ba71890f66916bd415e4553,sentry-trace_id=f20c9a4739274a30ba3099a8b019e676,sentry-sample_rate=1,sentry-sampled=true
                      Sec-Fetch-Site:same-site";
		}

		public void HandleAutoTrade()
		{
			var autotradeData = CheckAutotrade();
			if (autotradeData != null)
			{

			}
		}

		public string GetToken(string referral)
		{
			try
			{
				var payload = new
				{
					initData = model.InitData,
					//inviterTelegramId = referral,
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/v2/auth/login", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respon = rq.Response.ToString();
			}

			return RegexHelper.GetValueFromRegex("\"access_token\":\"(.*?)\"", body);
		}
		public string GetTask()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://api-tg.vooi.io/api/tasks?limit=200&skip=0").ToString();

			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}

			return body;
		}

		public bool StartTask(string task_id)
		{
			try
			{
				var payload = new
				{

				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/tasks/start/{task_id}", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public bool ClaimTask(string task_id)
		{
			try
			{
				var payload = new
				{

				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/tasks/claim/{task_id}", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public string CheckAutotrade()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Get($"https://api-tg.vooi.io/api/autotrade").ToString();

			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
			}

			return body;
		}

		public bool StartAutoTrade()
		{
			try
			{
				var payload = new
				{

				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/autotrade/start", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public bool ClaimAutoTrade(string auto_trade_id)
		{
			try
			{
				var payload = new
				{
					autoTradeId = auto_trade_id
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/autotrade/claim", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public bool StartTappingSession()
		{
			try
			{
				var payload = new
				{

				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/tapping/start_session", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}

		public bool FinishTappingSession(string session_id, string virt_money, string virt_points)
		{
			try
			{
				var payload = new
				{
					sessionId = session_id,
					tapped = new
					{
						virtMoney = virt_money,
						virtPoints = virt_points
					}
				};
				string jsonPayload = JsonConvert.SerializeObject(payload);
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://api-tg.vooi.io/api/tapping/finish", jsonPayload, "application/json").ToString();
			}
			catch
			{
				body = "";
				var respone = rq.Response.ToString();
				return false;
			}

			return true;
		}
	}
}

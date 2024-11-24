using Airdrop.Helper;
using Airdrop.Model;
using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Airdrop.Controller
{
	public class Duck_Api
	{
		Leaf.xNet.HttpRequest rq;
		AccountModel model;
		//List<TaskDto> Alltask, CompletedTask;
		string header_temp, body, username, id, auth_date, chat_instance,
			first_name, last_name, start_param, language_code, hash, photo_url, signature;
		double availableCoins;
		string referral_code = "BPSdrQLD4b";
		public Duck_Api(AccountModel accountModel)
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
			auth_date = RegexHelper.GetValueFromRegex("auth_date=(.*?)&", urlJson);
			first_name = RegexHelper.GetValueFromRegex("\"first_name\":\"(.*?)\"", urlJson);
			last_name = RegexHelper.GetValueFromRegex("\"last_name\":\"(.*?)\"", urlJson);
			username = RegexHelper.GetValueFromRegex("\"username\":\"(.*?)\"", urlJson);
			chat_instance = RegexHelper.GetValueFromRegex("chat_instance=(.*?)&", urlJson);
			start_param = RegexHelper.GetValueFromRegex("start_param=(.*?)&", urlJson);
			language_code = RegexHelper.GetValueFromRegex("\"language_code\":\"(.*?)\",", urlJson);
			hash = RegexHelper.GetValueFromRegex("hash=(.*?)&", urlJson);
			photo_url = RegexHelper.GetValueFromRegex("\"photo_url\":\"(.*?)\"", urlJson);
			signature = RegexHelper.GetValueFromRegex("signature=(.*?)&", urlJson);

			header_temp = $@"Accept-Language: vi-VN,vi;q=0.9,fr-FR;q=0.8,fr;q=0.7,en-US;q=0.6,en;q=0.5
                      accept: application/json, text/plain, */*
					  Origin:https://app.duckcoop.xyz
					  Referer: https://app.duckcoop.xyz/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?1
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}
		public string Login()
		{
			try
			{
				//photo_url = "https://t.me/i/userpic/320/bZYO1JgmwSPn4df4nKLJNPpIHIvNNfbG75mSZVgOym5USoSPqIKH0Ykl3b2t65Zk.svg";
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");
				//var jsonPayload = new
				//{
				//	user = new
				//	{
				//		id = model.UserId,
				//		first_name = first_name,
				//		last_name = last_name,
				//		username = username,
				//		language_code = language_code,
				//		photo_url = photo_url
				//	},
				//	chat_instance = chat_instance,
				//	chat_type = "sender",
				//	start_param = start_param,
				//	auth_date = auth_date,
				//	signature = signature,
				//	hash = hash,
				//	referral_code = "BPSdrQLD4b"
				//};

				//string jsonString = JsonConvert.SerializeObject(jsonPayload);

				var jsstring = $@"{{""user"":{{""id"":{model.UserId},""first_name"":""{first_name}"",""last_name"":""{last_name}"",""username"":""{username}"",""language_code"":""{language_code}"",""photo_url"":""{photo_url}""}},""chat_instance"":""{chat_instance}"",""chat_type"":""sender"",""start_param"":""{start_param}"",""auth_date"":""{auth_date}"",""signature"":""{signature}"",""hash"":""{hash}"",""referral_code"":""{referral_code}""}}";
				body = rq.Post($"https://api.apiduck.xyz/auth/telegram-login", jsstring, "application/json").ToString();
				//body = rq.Post($"https://api.apiduck.xyz/auth/telegram-login", $@"{{""user"":{{""id"":7034382478,""first_name"":""Tùng Ngô"",""last_name"":"""",""username"":""TungNgo3076"",""language_code"":""vi"",""allows_write_to_pm"":true,""photo_url"":""https://t.me/i/userpic/320/bZYO1JgmwSPn4df4nKLJNPpIHIvNNfbG75mSZVgOym5USoSPqIKH0Ykl3b2t65Zk.svg""}},""chat_instance"":""1871140948465814109"",""chat_type"":""sender"",""start_param"":""BPSdrQLD4b"",""auth_date"":""1732384417"",""signature"":""QevYNKCIDZgXBxMzhf6oxzrZ7y8K0WjAcNXvCoGmc0zAaPJU7K56q-kxrjyR4cRv4bFAS4qCUNz6lf_IbxTCCA"",""hash"":""e09c529880e01969f2b3f3bfd23bf0b129573372b7c7d281ca29298eb3a615f3"",""referral_code"":""BPSdrQLD4b""}}", "application/json").ToString();
				model.Token = RegexHelper.GetValueFromRegex("\"token\":\"(.*?)\",", body);

			}
			catch
			{
				body = "";
				//var respone = rq.Response.ToString();
			}

			return model.Token;
		}

		public bool DailyCheckin()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");

				body = rq.Get($"https://api.apiduck.xyz/checkin/get").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			var canCheckin = RegexHelper.GetValueFromRegex("\"can_claim\":(.*?),", body);
			if (canCheckin == "false") return false;
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization: Bearer {model.Token}");

				body = rq.Post($"https://api.apiduck.xyz/checkin/claim").ToString();
			}
			catch
			{
				body = "";
				return false;
			}
			return true;
		}

		private Model.Ducks.TaskDto GetTask()
		{
			var Alltask = new Model.Ducks.TaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"https://api.apiduck.xyz/partner-mission/list").ToString();
				Alltask = JsonConvert.DeserializeObject<Model.Ducks.TaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return Alltask;
		}

		private Model.Ducks.UserTaskDto GetTaskCompledted()
		{
			var alltaskCompledted = new Model.Ducks.UserTaskDto();
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Authorization:{model.Token}");

				body = rq.Get($"\r\nhttps://api.apiduck.xyz/user-partner-mission/get").ToString();
				alltaskCompledted = JsonConvert.DeserializeObject<Model.Ducks.UserTaskDto>(body);

			}
			catch
			{
				body = "";
			}

			return alltaskCompledted;
		}
		private bool claimTask(int taskID)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
																	Authorization:{model.Token}");


				var jsonPayload = new
				{
					partner_mission_id = taskID
				};

				string jsonString = JsonConvert.SerializeObject(jsonPayload);
				body = rq.Post($"https://api.apiduck.xyz/user-partner-mission/claim", jsonString, "application/json").ToString();

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
			var allTask = GetTask();
			var allTaskCompleted = GetTaskCompledted();
			foreach (var partnerMission in allTask.data.data)
			{
				foreach (var task in partnerMission.partner_missions)
				{
					if (allTaskCompleted.data.Any(t => t.partner_mission_id == task.pm_id)) continue;
					claimTask(task.pm_id);
				}
				//Thread.Sleep(1000);
			}

		}

	}
}

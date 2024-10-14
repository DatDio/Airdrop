using Airdrop.Helper;
using Airdrop.Model;
using Airdrop.Model.Major;
using Leaf.xNet;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airdrop.Controller
{
	public class Major_Api
	{
		HttpRequest rq;
		AccountModel model;
		string header_temp, body, path_js, Correlation_ID = "";

		public Major_Api(AccountModel model)
		{
			path_js = "C:/js/major_" + model.NameGpm.Replace("+", "").Replace("-", "").Replace(" ", "") + ".js";
			if (!File.Exists(path_js))
				File.WriteAllText(path_js, @"const protobufjs = require('protobufjs');
const toProtobuf = d => {
    const b = protobufjs.Root.fromJSON({
        nested: {
          user: {
            fields: {
              time: {
                type: 'int64',
                id: 1,
              },
            },
          },
        },
      }).lookupType('user'),
      _ = b.create({
        time: d,
      });
    return b.encode(_).finish();
  },
  encodeProtobufToBase64 = d => btoa(String.fromCharCode(...d));
const protobuf = toProtobuf(Date.now());
const base64 = encodeProtobufToBase64(protobuf);
console.log(base64)");

			try
			{
				Correlation_ID = FunctionHelper.RunNodeScript_old("node", path_js).Trim();
			}
			catch { }

			this.model = model;

			rq = new Leaf.xNet.HttpRequest();
			rq.Cookies = new CookieStorage();
			rq.AllowAutoRedirect = true;
			rq.KeepAlive = true;
			rq.Proxy = FunctionHelper.ConvertToProxyClient(model.Proxy);
			rq.UserAgent = Form1.useragent;

			header_temp = @"accept-language: en,en-US;q=0.9
                      accept: application/json, text/plain, */*
                      Origin:https://major.bot
                      Referer:https://major.bot/
                      Priority:u=1, i
                      Sec-Ch-Ua-Mobile:?0
                      Sec-Ch-Ua-Platform:""Windows""
                      Sec-Fetch-Dest:empty
                      Sec-Fetch-Mode:cors
                      Sec-Fetch-Site:same-site";
		}

		public string GetToken()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json");

				body = rq.Post($"https://major.bot/api/auth/tg/", $"{{\"init_data\":\"{model.InitData}\"}}", "application/json").ToString();
			}
			catch
			{
				body = "";
			}

			return RegexHelper.GetValueFromRegex("\"access_token\":\"(.*?)\"", body);
		}

		public string GetBalance()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/users/{model.UserId}/").ToString();
			}
			catch
			{
				body = "";
			}

			return RegexHelper.GetValueFromRegex("\"rating\":(.*?),", body);
		}

		public void Visit()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer {model.Token}");

				body = rq.Post($"https://major.bot/api/user-visits/visit/").ToString();
			}
			catch
			{
				body = "";
			}
		}

		public void Tasks()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/tasks/?is_daily=false").ToString();

				var dtos = JsonConvert.DeserializeObject<List<TaskDto>>(body);
				foreach (var dto in dtos)
				{
					if (dto.is_completed || dto.type == "code") continue;

					try
					{
						FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer {model.Token}");

						body = rq.Post($"https://major.bot/api/tasks/", "{\"task_id\":" + dto.id + "}", "application/json").ToString();

					}
					catch
					{
						body = "";
					}

					Thread.Sleep(1000);
				}

			}
			catch
			{
				body = "";
			}

			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/tasks/?is_daily=true").ToString();
				var dtos = JsonConvert.DeserializeObject<List<TaskDto>>(body);
				foreach (var dto in dtos)
				{
					if (dto.is_completed || dto.type == "code") continue;

					try
					{
						FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer {model.Token}");

						body = rq.Post($"https://major.bot/api/tasks/", "{\"task_id\":" + dto.id + "}", "application/json").ToString();

					}
					catch
					{
						body = "";
					}

					Thread.Sleep(1000);
				}

			}
			catch
			{
				body = "";
			}
		}

		public bool PlayGame_Puzzle(string key)
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/durov/").ToString();

				if (body.Contains("\"success\":true"))
				{
					var keys = key.Replace("-", ",").Split(',');
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer {model.Token}");
					if (Correlation_ID != "")
						FunctionHelper.AddHeader(rq, $"X-Correlation-ID:{Correlation_ID}");

					body = rq.Post($"https://major.bot/api/durov/", $"{{\"choice_1\":{keys[0]},\"choice_2\":{keys[1]},\"choice_3\":{keys[2]},\"choice_4\":{keys[3]}}}", "application/json").ToString();
				}
			}
			catch
			{
				body = "";
			}

			return body.Contains("{\"correct\":[" + key + "]}");
		}

		public void PlayGame_Hold()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/bonuses/coins/").ToString();

				if (body.Contains("\"success\":true"))
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer 
					{model.Token}");
					if (Correlation_ID != "")
						FunctionHelper.AddHeader(rq, $"X-Correlation-ID:{Correlation_ID}");

					body = rq.Post($"https://major.bot/api/bonuses/coins/", $"{{\"coins\":{2 * Form1.random.Next(300, 450)}}}", "application/json").ToString();
				}
			}
			catch
			{
				body = "";
			}
		}

		public void PlayGame_Roulette()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/roulette/").ToString();

				if (body.Contains("\"success\":true"))
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");
					if (Correlation_ID != "")
						FunctionHelper.AddHeader(rq, $"X-Correlation-ID:{Correlation_ID}");

					body = rq.Post($"https://major.bot/api/roulette/").ToString();
				}
			}
			catch
			{
				body = "";
			}
		}

		public void PlayGame_Swipe()
		{
			try
			{
				FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"authorization:Bearer {model.Token}");

				body = rq.Get($"https://major.bot/api/swipe_coin/").ToString();

				if (body.Contains("\"success\":true"))
				{
					FunctionHelper.AddHeader(rq, header_temp + "\n" + $@"Content-Type:application/json
                                                                    authorization:Bearer {model.Token}");
					if (Correlation_ID != "")
						FunctionHelper.AddHeader(rq, $"X-Correlation-ID:{Correlation_ID}");

					body = rq.Post($"https://major.bot/api/swipe_coin/", $"{{\"coins\":{10 * Form1.random.Next(200, 300)}}}", "application/json").ToString();
				}
			}
			catch
			{
				body = "";
			}
		}
	}
}
